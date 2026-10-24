using System;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

public sealed class MenuEditorLauncher : MarshalByRefObject
{
    [STAThread]
    public static int Main(string[] args)
    {
        string report = null;
        try
        {
            string installation = null;
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "--mediaportal" && i + 1 < args.Length) installation = args[++i];
                else if (args[i] == "--verify" && i + 1 < args.Length) report = Path.GetFullPath(args[++i]);
                else throw new ArgumentException("Usage: BackdropMenuEditor.exe [--mediaportal <folder>] [--verify <report.txt>]");
            }
            if (installation == null)
            {
                string[] candidates = {
                    AppDomain.CurrentDomain.BaseDirectory,
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), @"Team MediaPortal\MediaPortal"),
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), @"Team MediaPortal\MediaPortal")
                };
                foreach (string candidate in candidates)
                    if (File.Exists(Path.Combine(candidate, "MediaPortal.exe"))) { installation = candidate; break; }
            }
            if (installation == null && report == null)
                using (FolderBrowserDialog picker = new FolderBrowserDialog())
                {
                    picker.Description = "Select the MediaPortal installation folder";
                    if (picker.ShowDialog() != DialogResult.OK) return 1;
                    installation = picker.SelectedPath;
                }
            if (installation == null || !File.Exists(Path.Combine(installation, "MediaPortal.exe")))
                throw new DirectoryNotFoundException("MediaPortal was not found. Use --mediaportal to specify its installation folder.");
            installation = Path.GetFullPath(installation);
            // Inspect metadata before loading native-bitness MediaPortal assemblies.
            ProcessorArchitecture architecture = AssemblyName.GetAssemblyName(Path.Combine(installation, "Utils.dll")).ProcessorArchitecture;
            if (architecture == ProcessorArchitecture.X86 && Environment.Is64BitProcess)
                return RunX86(installation, report);
            if (architecture == ProcessorArchitecture.Amd64 && !Environment.Is64BitProcess)
                throw new InvalidOperationException("This MediaPortal installation requires a 64-bit editor process.");
            AppDomain domain = AppDomain.CreateDomain("Backdrop menu editor", null, new AppDomainSetup {
                ApplicationBase = installation,
                ConfigurationFile = Path.Combine(installation, "Configuration.exe.config")
            });
            try
            {
                var worker = (MenuEditorLauncher)domain.CreateInstanceFromAndUnwrap(
                    Assembly.GetExecutingAssembly().Location, typeof(MenuEditorLauncher).FullName);
                worker.Run(installation, report);
            }
            finally { AppDomain.Unload(domain); }
            return 0;
        }
        catch (Exception error)
        {
            if (report != null) File.WriteAllText(report, error.ToString());
            else MessageBox.Show(error.GetBaseException().Message, "Backdrop Menu Editor", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return 1;
        }
    }

    private static string Quote(string value)
    {
        // Windows command-line quoting, including trailing backslashes.
        StringBuilder result = new StringBuilder("\"");
        int slashes = 0;
        foreach (char character in value)
        {
            if (character == '\\') { slashes++; continue; }
            result.Append('\\', character == '"' ? slashes * 2 + 1 : slashes);
            result.Append(character); slashes = 0;
        }
        result.Append('\\', slashes * 2);
        return result.Append('"').ToString();
    }

    private static int RunX86(string installation, string report)
    {
        byte[] helper;
        using (Stream resource = Assembly.GetExecutingAssembly().GetManifestResourceStream("BackdropMenuEditor.x86.exe"))
        using (MemoryStream bytes = new MemoryStream())
        {
            if (resource == null) throw new InvalidOperationException("The embedded x86 editor is missing. Reinstall the complete Backdrop editor.");
            resource.CopyTo(bytes); helper = bytes.ToArray();
        }
        string digest;
        using (SHA256 hash = SHA256.Create()) digest = BitConverter.ToString(hash.ComputeHash(helper)).Replace("-", "");
        string folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "BackdropMenuEditor", digest);
        Directory.CreateDirectory(folder);
        string executable = Path.Combine(folder, "BackdropMenuEditor.x86.exe");
        if (!File.Exists(executable) || !System.Linq.Enumerable.SequenceEqual(File.ReadAllBytes(executable), helper))
        {
            string temporary = Path.Combine(folder, Guid.NewGuid().ToString("N") + ".tmp");
            File.WriteAllBytes(temporary, helper);
            if (File.Exists(executable)) File.Delete(executable);
            File.Move(temporary, executable);
        }
        string arguments = "--mediaportal " + Quote(installation);
        if (report != null) arguments += " --verify " + Quote(report);
        using (Process process = Process.Start(new ProcessStartInfo(executable, arguments) {
            UseShellExecute = false, CreateNoWindow = true, WorkingDirectory = installation
        })) { process.WaitForExit(); return process.ExitCode; }
    }

    public void Run(string installation, string report)
    {
        Directory.SetCurrentDirectory(installation);
        AppDomain.CurrentDomain.AssemblyResolve += delegate(object sender, ResolveEventArgs e) {
            string filename = new AssemblyName(e.Name).Name + ".dll";
            foreach (string folder in new[] { installation, Path.Combine(installation, "plugins", "Windows"), Path.Combine(installation, "plugins", "process") })
            {
                string path = Path.Combine(folder, filename);
                if (File.Exists(path)) return Assembly.LoadFrom(path);
            }
            return null;
        };
        Assembly utils = Assembly.LoadFrom(Path.Combine(installation, "Utils.dll"));
        Type config = utils.GetType("MediaPortal.Configuration.Config", true);
        Type dir = config.GetNestedType("Dir");
        MethodInfo getFolder = config.GetMethod("GetFolder");
        string skin = (string)getFolder.Invoke(null, new[] { Enum.Parse(dir, "Skin") });
        string settings = (string)getFolder.Invoke(null, new[] { Enum.Parse(dir, "Config") });
        string plugins = (string)getFolder.Invoke(null, new[] { Enum.Parse(dir, "Plugins") });
        if (!String.Equals(Path.GetFullPath(plugins).TrimEnd('\\'), Path.Combine(installation, "plugins"), StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("MediaPortal plugin paths do not match the selected installation.");
        if (!File.Exists(Path.Combine(skin, "Backdrop", "BasicHome.xml")))
            throw new FileNotFoundException("The Backdrop skin is not installed in " + skin);
        Assembly editor;
        using (Stream resource = Assembly.GetExecutingAssembly().GetManifestResourceStream("BackdropMenuEditor.dll"))
        using (MemoryStream bytes = new MemoryStream()) { resource.CopyTo(bytes); editor = Assembly.Load(bytes.ToArray()); }
        AppDomain.CurrentDomain.SetData("Backdrop.StandaloneEditor", true);
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        using (Form form = (Form)Activator.CreateInstance(editor.GetType("MediaPortal.Plugins.BackdropMenuEditorConfig", true)))
        {
            if (report != null)
            {
                // Run the actual Load event without displaying or saving the editor.
                typeof(Form).GetMethod("OnLoad", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(form, new object[] { EventArgs.Empty });
                File.WriteAllText(report, "PASS: editor created and loaded\r\nProcess: " + (Environment.Is64BitProcess ? "x64" : "x86") + "\r\nInstallation: " + installation + "\r\nSettings: " + settings + "\r\nSkin: " + skin + "\r\nPlugins: " + plugins);
            }
            else Application.Run(form);
        }
    }
}
