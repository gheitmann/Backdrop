using System;
using System.IO;
using System.Reflection;
using System.Drawing;
using System.Windows.Forms;
using Backdrop.InstallerUi;

// Real isolated file I/O, forwarded through the native progress event handler.
// Does not invoke the MPE install engine, dependencies or package registration.
class CopyProgressProbe
{
    [STAThread]
    static int Main(string[] args)
    {
        string local = AppDomain.CurrentDomain.BaseDirectory;
        string installed = File.ReadAllText(Path.Combine(local, "MediaPortalDirectory.txt")).Trim();
        AppDomain.CurrentDomain.AssemblyResolve += delegate(object s, ResolveEventArgs e) {
            string path = Path.Combine(installed, new AssemblyName(e.Name).Name + ".dll");
            return File.Exists(path) ? Assembly.LoadFrom(path) : null;
        };
        try
        {
            string destination = Path.GetFullPath(args[0]);
            if (Directory.Exists(destination)) throw new IOException("Use a new copy fixture directory.");
            Directory.CreateDirectory(destination);
            byte[] data = new byte[65536];
            new Random(139).NextBytes(data);
            string source = Path.Combine(destination, "fixture.bin");
            File.WriteAllBytes(source, data);
            Assembly core = Assembly.LoadFrom(Path.Combine(local, "MpeCore.dll"));
            using (Form form = (Form)Activator.CreateInstance(core.GetType("MpeCore.Classes.SectionPanel.InstallSection")))
            {
                Label label = (Label)InstallerPresentation.Find(form, "lbl_curr_file");
                ProgressBar progress = (ProgressBar)InstallerPresentation.Find(form, "progressBar1");
                progress.Maximum = 60;
                MethodInfo handler = form.GetType().GetMethod("packageClass_FileInstalled", BindingFlags.Instance | BindingFlags.NonPublic);
                Type eventType = handler.GetParameters()[1].ParameterType;
                Rectangle bounds = label.Bounds;
                int clean = 0;
                for (int i = 0; i < 60; i++)
                {
                    string name = i % 2 == 0 ? new string('W', 130) + i + ".bin" : "a" + i + ".bin";
                    string target = Path.Combine(destination, name);
                    File.Copy(source, target, false);
                    if (!System.Linq.Enumerable.SequenceEqual(data, File.ReadAllBytes(target))) throw new IOException("Copied bytes differ.");
                    handler.Invoke(form, new object[] { form, Activator.CreateInstance(eventType, new object[] { target }) });
                    if (label.Bounds != bounds || label.AutoSize) throw new Exception("Filename viewport changed size.");
                    if (i % 2 == 1)
                    {
                        using (Bitmap bitmap = new Bitmap(label.Width, label.Height))
                        {
                            label.DrawToBitmap(bitmap, new Rectangle(0, 0, bitmap.Width, bitmap.Height));
                            // An ellipsis/path may span the label; test the cleared margin after it.
                            if (bitmap.GetPixel(bitmap.Width - 2, bitmap.Height - 2).ToArgb() != label.BackColor.ToArgb())
                                throw new Exception("Filename background was not cleared.");
                        }
                        clean++;
                    }
                }
                if (progress.Value != 60) throw new Exception("Final copied-file count differs.");
                File.WriteAllText(Path.Combine(destination, "result.txt"),
                    "PASS: 60/60 real copies byte-identical; 30/30 long-to-short viewport checks; native progress 60/60. No MPE package installed or registered.");
            }
            return 0;
        }
        catch (Exception e) { Console.Error.WriteLine(e); return 1; }
    }
}
