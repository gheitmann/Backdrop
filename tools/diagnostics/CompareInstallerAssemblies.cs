using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Mono.Cecil;

// Metadata/IL inspection only; never loads or executes the installer assemblies.
class CompareInstallerAssemblies
{
    static IEnumerable<TypeDefinition> Types(IEnumerable<TypeDefinition> types)
    {
        foreach (var type in types) { yield return type; foreach (var nested in Types(type.NestedTypes)) yield return nested; }
    }
    static Dictionary<string, string> Bodies(AssemblyDefinition assembly)
    {
        return Types(assembly.MainModule.Types).SelectMany(t => t.Methods).ToDictionary(m => m.FullName,
            m => m.HasBody ? string.Join("|", m.Body.Instructions.Select(i => i.ToString()).ToArray()) : "<no body>");
    }
    static int Main(string[] args)
    {
        try
        {
            foreach (string name in new[] { "Backdrop.InstallerPresentation.dll", "BackdropInstaller.x86.exe", "BackdropInstaller.x64.exe", "PresentationProbe.x86.exe", "PresentationProbe.x64.exe", "PatchInstaller.exe" })
            using (var before = AssemblyDefinition.ReadAssembly(Path.Combine(args[0], name)))
            using (var after = AssemblyDefinition.ReadAssembly(Path.Combine(args[1], name)))
            {
                var old = Bodies(before); var current = Bodies(after);
                var changed = old.Keys.Union(current.Keys).Where(k => !old.ContainsKey(k) || !current.ContainsKey(k) || old[k] != current[k]).ToArray();
                Console.WriteLine(name + ": changed IL bodies=" + changed.Length + "; MVID changed=" + (before.MainModule.Mvid != after.MainModule.Mvid));
                foreach (string method in changed) Console.WriteLine("  " + method);
            }
            return 0;
        }
        catch (Exception error) { Console.Error.WriteLine(error.Message); return 1; }
    }
}
