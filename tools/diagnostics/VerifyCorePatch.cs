using System;
using System.Linq;
using System.Collections.Generic;
using System.Security.Cryptography;
using Mono.Cecil;
using Mono.Cecil.Cil;
class VerifyCorePatch {
 static IEnumerable<TypeDefinition> All(IEnumerable<TypeDefinition> types){foreach(var t in types){yield return t;foreach(var n in All(t.NestedTypes))yield return n;}}
 static string Canonical(MethodDefinition m){
  if(!m.HasBody)return "";
  var list=m.Body.Instructions;
  return String.Join("\n",list.Select(i=>i.OpCode.Name+" "+(i.Operand is Instruction?"target:"+list.IndexOf((Instruction)i.Operand):i.Operand is Instruction[]?String.Join(",",((Instruction[])i.Operand).Select(x=>list.IndexOf(x).ToString())):Convert.ToString(i.Operand))).ToArray());
 }
 static string Hash(byte[] b){using(var h=SHA256.Create())return BitConverter.ToString(h.ComputeHash(b));}
 static int Main(string[] args){try{
  using(var before=AssemblyDefinition.ReadAssembly(args[0]))using(var after=AssemblyDefinition.ReadAssembly(args[1])){
   if(before.Name.FullName!=after.Name.FullName || before.MainModule.Architecture!=after.MainModule.Architecture || before.MainModule.Mvid!=after.MainModule.Mvid)throw new Exception("Assembly identity changed");
   if(String.Join("|",before.MainModule.AssemblyReferences.Select(x=>x.FullName).ToArray())!=String.Join("|",after.MainModule.AssemblyReferences.Select(x=>x.FullName).ToArray()))throw new Exception("References changed");
   var methods=All(after.MainModule.Types).SelectMany(t=>t.Methods).ToDictionary(m=>m.FullName);
   int total=0,changed=0;
   foreach(var m in All(before.MainModule.Types).SelectMany(t=>t.Methods)){
    total++;if(Canonical(m)!=Canonical(methods[m.FullName])){changed++;Console.WriteLine("Changed body: "+m.FullName);if(!m.FullName.Contains("MixerStream::PlaybackEndProc("))throw new Exception("Unexpected body change");}
   }
   if(total!=methods.Count||changed!=1)throw new Exception("Unexpected method count/change count");
   if(before.MainModule.Resources.Count!=after.MainModule.Resources.Count)throw new Exception("Resource count changed");
   foreach(var r in before.MainModule.Resources.OfType<EmbeddedResource>())if(Hash(r.GetResourceData())!=Hash(after.MainModule.Resources.OfType<EmbeddedResource>().Single(x=>x.Name==r.Name).GetResourceData()))throw new Exception("Resource changed: "+r.Name);
   Console.WriteLine("PASS: "+total+" methods compared; exactly 1 body changed; assembly identity, references and embedded resources preserved.");
  }return 0;
 }catch(Exception e){Console.WriteLine(e);return 1;}}
}
