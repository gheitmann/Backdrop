using System;
using System.IO;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class PatchInstaller
{
    static int Main(string[] args)
    {
        try
        {
            using (AssemblyDefinition assembly = AssemblyDefinition.ReadAssembly(args[0]))
            using (AssemblyDefinition helper = AssemblyDefinition.ReadAssembly(args[1]))
            {
                if (assembly.Name.Name != "MpeCore" || assembly.Name.Version != new Version(1,39,0,0) || assembly.Name.HasPublicKey)
                    throw new InvalidOperationException("Expected unsigned MediaPortal 1.39 MpeCore.dll.");
                ModuleDefinition module = assembly.MainModule;
                TypeDefinition presentation = helper.MainModule.GetType("Backdrop.InstallerUi.InstallerPresentation");
                MethodReference apply = module.ImportReference(presentation.Methods.Single(m => m.Name == "Apply"));
                MethodReference refresh = module.ImportReference(presentation.Methods.Single(m => m.Name == "RefreshProgress"));
                // The derived constructors have finished creating their controls at this point.
                foreach (string name in new[] { "BaseHorizontalLayout", "BaseVerticalLayout", "InstallSection" })
                {
                    TypeDefinition type = module.GetType("MpeCore.Classes.SectionPanel." + name);
                    MethodDefinition ctor = type.Methods.Single(m => m.Name == ".ctor" && !m.HasParameters);
                    ILProcessor il = ctor.Body.GetILProcessor();
                    foreach (Instruction ret in ctor.Body.Instructions.Where(i => i.OpCode == OpCodes.Ret).ToArray())
                    {
                        il.InsertBefore(ret, il.Create(OpCodes.Ldarg_0));
                        il.InsertBefore(ret, il.Create(OpCodes.Call, apply));
                    }
                }
                MethodDefinition method = module.GetType("MpeCore.Classes.SectionPanel.InstallSection")
                    .Methods.Single(m => m.Name == "RefreshControls");
                string[] expected = { "System.Void System.Windows.Forms.Control::Refresh()", "System.Void System.Windows.Forms.Control::Update()", "System.Void System.Windows.Forms.Control::Invalidate(System.Boolean)" };
                if (!expected.All(name => method.Body.Instructions.Any(i => i.Operand is MethodReference && ((MethodReference)i.Operand).FullName == name)))
                    throw new InvalidOperationException("Unexpected RefreshControls implementation; refusing to patch.");
                method.Body.Instructions.Clear();
                method.Body.ExceptionHandlers.Clear();
                method.Body.Variables.Clear();
                ILProcessor body = method.Body.GetILProcessor();
                body.Append(body.Create(OpCodes.Ldarg_0));
                body.Append(body.Create(OpCodes.Call, refresh));
                body.Append(body.Create(OpCodes.Ret));
                if (File.Exists(args[2])) throw new IOException("Output already exists; use a new output directory.");
                assembly.Write(args[2]);
                Console.WriteLine("Patched presentation in isolated MpeCore copy; native installation methods unchanged.");
            }
            return 0;
        }
        catch (Exception error) { Console.Error.WriteLine(error.Message); return 1; }
    }
}
