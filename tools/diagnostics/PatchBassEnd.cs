using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Mono.Cecil;
using Mono.Cecil.Cil;
class PatchBassEnd {
 static string Hash(string file){using(var s=File.OpenRead(file))using(var h=SHA256.Create())return BitConverter.ToString(h.ComputeHash(s)).Replace("-","").ToLowerInvariant();}
 static int Main(string[] args){
  try{
   if(args.Length!=2)throw new Exception("Usage: PatchBassEnd original-Core.dll output-Core.dll");
   if(Path.GetFullPath(args[0])==Path.GetFullPath(args[1]))throw new Exception("Output must be separate from input");
   const string expected="0d9160a8e6fb61a6b6bfecacc80df0207396bc4cc01d4c5dc2f5850c86bb4b39";
   if(Hash(args[0])!=expected)throw new Exception("Unsupported Core.dll hash; no patch applied");
   var resolver=new DefaultAssemblyResolver();resolver.AddSearchDirectory(Path.GetDirectoryName(Path.GetFullPath(args[0])));
   using(var assembly=AssemblyDefinition.ReadAssembly(args[0],new ReaderParameters { AssemblyResolver=resolver })){
    if(assembly.Name.HasPublicKey)throw new Exception("Unexpected signed assembly");
    var method=assembly.MainModule.Types.Single(t=>t.FullName=="MediaPortal.MusicPlayer.BASS.MixerStream").Methods.Single(m=>m.Name=="PlaybackEndProc");
    if(method.Body.Variables[2].VariableType.FullName!="MediaPortal.Playlists.PlayListItem"||method.Body.Variables[3].VariableType.FullName!="System.Boolean")throw new Exception("Unexpected locals");
    var matches=method.Body.Instructions.Where(i=>i.OpCode==OpCodes.Ldc_I4_0 && i.Next!=null && i.Next.OpCode==OpCodes.Stloc_3).ToArray();
    if(matches.Length!=1)throw new Exception("Expected exactly one newMixerNeeded initializer");
    var anchor=matches[0];var il=method.Body.GetILProcessor();
    anchor.OpCode=OpCodes.Ldloc_2;anchor.Operand=null;
    var loadNull=il.Create(OpCodes.Ldnull);il.InsertAfter(anchor,loadNull);il.InsertAfter(loadNull,il.Create(OpCodes.Ceq));
    // Source equivalent: bool newMixerNeeded = nextSong == null;
    // Existing asynchronous path already handles a mixer replacement safely.
    foreach(var instruction in method.Body.Instructions){
     if(instruction.OpCode.OperandType!=OperandType.ShortInlineBrTarget)continue;
     var field=typeof(OpCodes).GetField(instruction.OpCode.Code.ToString().Replace("_S",""));
     if(field==null)throw new Exception("Unknown short branch");
     instruction.OpCode=(OpCode)field.GetValue(null);
    }
    assembly.Write(args[1]);
   }
   Console.WriteLine("Patched only MixerStream.PlaybackEndProc: no-next-song shutdown now uses existing asynchronous path.");
   Console.WriteLine("SHA256 "+Hash(args[1]));return 0;
  }catch(Exception ex){Console.WriteLine(ex.Message);return 1;}
 }
}
