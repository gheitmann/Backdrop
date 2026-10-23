using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

class BackdropInstaller
{
    [STAThread]
    static int Main(string[] args)
    {
        try
        {
            string local = AppDomain.CurrentDomain.BaseDirectory;
            string installed = File.ReadAllText(Path.Combine(local, "MediaPortalDirectory.txt")).Trim();
            AppDomain.CurrentDomain.AssemblyResolve += delegate(object sender, ResolveEventArgs e)
            {
                string name = new AssemblyName(e.Name).Name + ".dll";
                string path = Path.Combine(installed, name);
                return File.Exists(path) ? Assembly.LoadFrom(path) : null;
            };
            Assembly.LoadFrom(Path.Combine(local, "Backdrop.InstallerPresentation.dll"));
            Assembly patched = Assembly.LoadFrom(Path.Combine(local, "MpeCore.dll"));
            if (args.Length == 1 && args[0] == "--verify")
            {
                Assembly nativeInstaller = Assembly.LoadFrom(Path.Combine(installed, "MpeInstaller.exe"));
                foreach (AssemblyName reference in nativeInstaller.GetReferencedAssemblies())
                    if (reference.Name == "MpeCore" && Assembly.Load(reference) != patched)
                        throw new InvalidOperationException("Native installer did not bind to the isolated MpeCore copy.");
                File.WriteAllText(Path.Combine(local, "binding-probe.txt"),
                    "PASS: native installer binds to isolated MpeCore; entry point was not invoked. No installation performed.");
                return 0;
            }
            if (args.Length == 0 || (args.Length == 1 && args[0] == "--preview"))
            {
                Application.EnableVisualStyles();
                using (Form preview = (Form)Activator.CreateInstance(patched.GetType("MpeCore.Classes.SectionPanel.InstallSection")))
                {
                    object section = preview.GetType().GetField("Section").GetValue(preview);
                    section.GetType().GetProperty("Params").SetValue(section,
                        preview.GetType().GetMethod("GetDefaultParams").Invoke(preview, null), null);
                    preview.Text = "Backdrop installer presentation preview - no installation";
                    preview.Shown += delegate { preview.Text = "Backdrop installer preview - no installation"; };
                    // Default ShowMode is Preview: no package files or registration are installed.
                    Application.Run(preview);
                }
                return 0;
            }
            if (args.Length != 1 || !File.Exists(args[0]) || !string.Equals(Path.GetExtension(args[0]), ".mpe1", StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException("Drag one .mpe1 package onto BackdropInstaller.exe, or use --preview.");
            Assembly native = Assembly.LoadFrom(Path.Combine(installed, "MpeInstaller.exe"));
            MethodInfo entry = native.EntryPoint;
            entry.Invoke(null, entry.GetParameters().Length == 0 ? null : new object[] { args });
            return 0;
        }
        catch (Exception error)
        {
            File.WriteAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "preview-error.txt"), error.ToString());
            MessageBox.Show(error.GetBaseException().Message, "Backdrop installer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return 1;
        }
    }
}
