// Exercise the actual designer-wired FormClosing event without showing the
// editor, displaying prompts, loading a menu, or writing production settings.
using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using MediaPortal.Plugins;

internal static class EditorCloseLifecycleProbe
{
    [STAThread]
    private static int Main(string[] args)
    {
        string source = Path.GetFullPath(args[0]), client = Path.GetFullPath(args[1]);
        AppDomain.CurrentDomain.AssemblyResolve += delegate(object sender, ResolveEventArgs request) {
            string filename = new AssemblyName(request.Name).Name + ".dll";
            foreach (string folder in new[] { source, client, Path.Combine(client, "plugins", "Windows") })
            {
                string path = Path.Combine(folder, filename);
                if (File.Exists(path)) return Assembly.LoadFrom(path);
            }
            return null;
        };
        return Run();
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static int Run()
    {
        Check(DialogResult.No, true, false, false, 0, 0);
        Check(DialogResult.Yes, true, false, false, 1, 0);
        Check(DialogResult.Yes, false, false, true, 1, 0);
        Check(DialogResult.Yes, true, true, true, 1, 1);
        Check(DialogResult.Cancel, true, false, true, 0, 0);
        using (TestEditor form = new TestEditor())
        {
            FormClosingEventArgs cancelled = new FormClosingEventArgs(CloseReason.UserClosing, true);
            form.RaiseClosing(cancelled);
            Require(cancelled.Cancel && form.Prompts == 0 && form.Saves == 0, "Prior cancellation must be respected");
        }
        Console.WriteLine("PASS: 6/6 actual FormClosing paths; No never saves; failures retain the form; no recursive disposal. No UI shown or production settings written.");
        return 0;
    }

    private static void Check(DialogResult choice, bool success, bool fail, bool cancel, int saves, int errors)
    {
        using (TestEditor form = new TestEditor { Choice = choice, Success = success, Fail = fail })
        {
            FormClosingEventArgs closing = new FormClosingEventArgs(CloseReason.UserClosing, false);
            form.RaiseClosing(closing);
            Require(form.Prompts == 1 && form.Saves == saves && form.Errors == errors && closing.Cancel == cancel,
                "Unexpected close behavior for " + choice + "/" + success + "/" + fail);
            Require(!form.IsDisposed, "FormClosing must not recursively dispose the form");
        }
    }

    private static void Require(bool value, string message)
    {
        if (!value) throw new InvalidOperationException(message);
    }

    private sealed class TestEditor : BackdropMenuEditorConfig
    {
        internal DialogResult Choice;
        internal bool Success, Fail;
        internal int Prompts, Saves, Errors;
        protected override DialogResult ConfirmSaveOnClose() { Prompts++; return Choice; }
        protected override bool SaveMenuConfiguration()
        {
            Saves++;
            if (Fail) throw new IOException("Synthetic save failure");
            return Success;
        }
        protected override void ReportSaveError(Exception error) { Errors++; }
        internal void RaiseClosing(FormClosingEventArgs args) { base.OnFormClosing(args); }
    }
}
