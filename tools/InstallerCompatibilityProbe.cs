using System;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Security.Cryptography;
using Mono.Cecil;
using Mono.Cecil.Cil;
class InstallerCompatibilityProbe {
 static string Body(MethodDefinition m) { return m.HasBody ? string.Join("|",m.Body.Instructions.Select(i=>i.ToString()).ToArray()) : ""; }
 static int Main(string[] args) {
  try { Run(args); return 0; }
  catch(Exception error) { Console.Error.WriteLine(error.Message); return 1; }
 }
 static void Run(string[] args) {
  string input=args[0], patcher=args[1], helper=args[2], root=args[3]; Directory.CreateDirectory(root);
  byte[] original=File.ReadAllBytes(input);
  foreach(string test in new[]{"1.38.0.0","1.39.1.0","1.40.0.0","2.0.0.0","changed-logic","missing-control","missing-constructor","wrong-name","signed"}) {
   string path=Path.Combine(root,test); Directory.CreateDirectory(path); string native=Path.Combine(path,"MpeCore.dll"), output=Path.Combine(path,"patched.dll");
   bool accepted=char.IsDigit(test[0]);
   using(var a=AssemblyDefinition.ReadAssembly(input)) {
    if(accepted) a.Name.Version=new Version(test);
    var type=a.MainModule.GetType("MpeCore.Classes.SectionPanel.InstallSection");
    if(test=="changed-logic") { var m=type.Methods.Single(x=>x.Name=="RefreshControls"); m.Body.GetILProcessor().InsertBefore(m.Body.Instructions[0],Instruction.Create(OpCodes.Break)); }
    if(test=="missing-control") type.Fields.Single(x=>x.Name=="lbl_curr_file").Name="renamed";
    if(test=="missing-constructor") type.Methods.Single(x=>x.Name==".ctor"&&!x.HasParameters).Name="MissingConstructor";
    if(test=="wrong-name") a.Name.Name="ForeignCore";
    if(test=="signed") {a.Name.PublicKey=new byte[]{1,2,3,4};a.Name.HasPublicKey=true;}
    a.Write(native);
   }
   var start=new ProcessStartInfo(patcher,"\""+native+"\" \""+helper+"\" \""+output+"\"") {UseShellExecute=false,CreateNoWindow=true,RedirectStandardOutput=true,RedirectStandardError=true};
   using(var p=Process.Start(start)) { string stdout=p.StandardOutput.ReadToEnd(),stderr=p.StandardError.ReadToEnd();p.WaitForExit();if((p.ExitCode==0)!=accepted)throw new Exception(test+": unexpected patch result "+stderr); }
   if(File.Exists(output)!=accepted)throw new Exception(test+": unsafe output");
   if(accepted) using(var a=AssemblyDefinition.ReadAssembly(native)) using(var b=AssemblyDefinition.ReadAssembly(output)) {
    if(a.Name.FullName!=b.Name.FullName)throw new Exception("Assembly identity changed");
    int changed=0;
    foreach(var t in a.MainModule.Types) foreach(var m in t.Methods) {
     var match=b.MainModule.GetType(t.FullName).Methods.Single(x=>x.FullName==m.FullName);
     if(Body(m)!=Body(match)) {changed++;if(!(t.FullName.StartsWith("MpeCore.Classes.SectionPanel.")&&(m.Name==".ctor"||m.Name=="RefreshControls")))throw new Exception("Non-presentation method changed");}
    }
    if(changed!=4)throw new Exception("Expected four presentation-only changes, got "+changed);
   }
   Console.WriteLine("PASS: "+test+" "+(accepted?"contract accepted; identity retained, four presentation methods changed":"contract rejected, no patched output"));
  }
  if(!original.SequenceEqual(File.ReadAllBytes(input)))throw new Exception("Installed input changed");
  Console.WriteLine("PASS: installed input bytes unchanged. Version fixtures are not actual releases.");
 }
}

