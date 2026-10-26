using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Xml;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Backdrop.DayNight;
using MediaPortal.GUI.Library;

internal static class DayNightProbe
{
    private static int checks;
    private static string client;
    private static string source;
    private static void Check(bool valid, string name)
    { if (!valid) throw new Exception("FAIL: " + name); checks++; }

    [STAThread]
    private static int Main(string[] args)
    {
        try
        {
            client = args[0]; source = Path.Combine(args[1], "Source");
            AppDomain.CurrentDomain.AssemblyResolve += delegate(object sender, ResolveEventArgs request)
            {
                string name = new AssemblyName(request.Name).Name + ".dll";
                foreach (string folder in new[] { source, client, Path.Combine(client, "plugins", "windows"), Path.Combine(client, "plugins", "process") })
                {
                    string path = Path.Combine(folder, name);
                    if (File.Exists(path)) return Assembly.LoadFrom(path);
                }
                return null;
            };
            Run(args[1], args[2]);
            return 0;
        }
        catch (Exception error) { Console.Error.WriteLine(error); return 1; }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static void Run(string repo, string scratch)
    {
        Directory.CreateDirectory(scratch);
        var defaults = new DayNightSettings();
        foreach (BackgroundMode mode in Enum.GetValues(typeof(BackgroundMode)))
        {
            var settings = new DayNightSettings { Mode = mode, MorningStarts = new TimeSpan(6,30,0), NoonStarts = new TimeSpan(12,15,0), DayStarts = new TimeSpan(15,45,0), NightStarts = new TimeSpan(19,15,0) };
            string path = Path.Combine(scratch, "settings.xml");
            settings.Save(path); settings.Save(path);
            var loaded = DayNightSettings.Load(path);
            Check(loaded.Mode == mode && loaded.MorningStarts == settings.MorningStarts && loaded.NoonStarts == settings.NoonStarts && loaded.DayStarts == settings.DayStarts && loaded.NightStarts == settings.NightStarts, "mode/time round trip: " + mode);
            foreach (string time in new[] { "00:00", "06:29", "06:30", "12:14", "12:15", "15:44", "15:45", "19:14", "19:15", "23:59" })
            {
                var now = DateTime.Today.Add(TimeSpan.ParseExact(time, @"hh\:mm", CultureInfo.InvariantCulture));
                string expected = mode.ToString().ToLowerInvariant();
                if (mode == BackgroundMode.Disabled) expected = "night";
                if (mode == BackgroundMode.Automatic)
                    expected = now.TimeOfDay < settings.MorningStarts || now.TimeOfDay >= settings.NightStarts ? "night" : now.TimeOfDay < settings.NoonStarts ? "morning" : now.TimeOfDay < settings.DayStarts ? "noon" : "day";
                Check(loaded.Period(now) == expected, mode + " boundary " + time);
            }
        }
        var overnight = new DayNightSettings { MorningStarts=TimeSpan.FromHours(20),NoonStarts=TimeSpan.Zero,DayStarts=TimeSpan.FromHours(3),NightStarts=TimeSpan.FromHours(6) };
        string[] overnightTimes={"00:00","02:59","03:00","05:59","06:00","19:59","20:00","23:59"};
        string[] overnightExpected={"noon","noon","day","day","night","night","morning","morning"};
        for(int i=0;i<overnightTimes.Length;i++)
            Check(overnight.Period(DateTime.Today.Add(TimeSpan.ParseExact(overnightTimes[i],@"hh\:mm",CultureInfo.InvariantCulture)))==overnightExpected[i],"overnight "+overnightTimes[i]);
        Check(defaults.Period(DateTime.Today.AddHours(6))=="morning","default morning");
        Check(defaults.Period(DateTime.Today.AddHours(12))=="noon","default noon");
        Check(defaults.Period(DateTime.Today.AddHours(15))=="day","default afternoon");
        Check(defaults.Period(DateTime.Today.AddHours(19))=="night","default night");
        foreach(var invalid in new[] {
            new DayNightSettings { NoonStarts=TimeSpan.FromHours(6) },
            new DayNightSettings { NoonStarts=TimeSpan.FromHours(18),DayStarts=TimeSpan.FromHours(12) },
            new DayNightSettings { MorningStarts=TimeSpan.FromHours(6).Add(TimeSpan.FromMilliseconds(1)) } })
        {
            bool rejected=false;
            try { invalid.Save(Path.Combine(scratch,"invalid-time.xml")); } catch(InvalidDataException) { rejected=true; }
            Check(rejected,"invalid schedule rejected");
        }
        string bad=Path.Combine(scratch,"invalid.xml");
        foreach(string xml in new[] { "<broken", "<dayNight version='2' mode='Unexpected' morningStarts='06:00' noonStarts='12:00' dayStarts='15:00' nightStarts='19:00'/>", "<dayNight version='2' mode='Automatic' morningStarts='25:00' noonStarts='12:00' dayStarts='15:00' nightStarts='19:00'/>", "<!DOCTYPE x [<!ENTITY y SYSTEM 'file:///invalid'>]><dayNight>&y;</dayNight>" })
        {
            File.WriteAllText(bad,xml);bool rejected=false;
            try { DayNightSettings.Load(bad); } catch { rejected=true; }
            Check(rejected,"invalid settings rejected");
        }
        string legacy=Path.Combine(scratch,"legacy.xml");
        File.WriteAllText(legacy,"<dayNight version='1' mode='Automatic' dayStarts='06:00' nightStarts='19:00'/>");
        var migrated=DayNightSettings.Load(legacy);
        Check(migrated.MorningStarts==TimeSpan.FromHours(6) && migrated.NoonStarts==TimeSpan.FromHours(12) && migrated.DayStarts==TimeSpan.FromHours(15) && migrated.NightStarts==TimeSpan.FromHours(19),"legacy default migration");
        File.WriteAllText(legacy,"<dayNight version='1' mode='Day' dayStarts='20:00' nightStarts='06:00'/>");
        migrated=DayNightSettings.Load(legacy);migrated.Validate();
        Check(migrated.Mode==BackgroundMode.Day && migrated.MorningStarts==TimeSpan.FromHours(20) && migrated.NightStarts==TimeSpan.FromHours(6),"legacy overnight endpoints retained");
        var catalogue=new XmlDocument();catalogue.Load(Path.Combine(repo,"Source","DayNight","Languages.xml"));
        int languages=0;
        foreach(XmlElement language in catalogue.SelectNodes("/languages/language"))
        {
            languages++;
            foreach(XmlElement entry in language.SelectNodes("text"))
                Check(DayNightText.Get(entry.GetAttribute("key"),language.GetAttribute("code"))==entry.InnerText,"translation "+language.GetAttribute("code")+"/"+entry.GetAttribute("key"));
        }
        Check(languages==37,"all 37 language catalogues");
        Check(DayNightText.Get("Morning","de-DE")=="Morgen" && DayNightText.Get("Noon","de-DE")=="Mittag","German period names");
        Check(DayNightText.Get("Morning","unknown-XX")=="Morning","unknown language English fallback");
        string reason;
        Check(SkinSupport.Check(Path.Combine(repo, "skin", "Backdrop"), out reason), "complete Backdrop assets supported");
        Check(SkinSupport.ResolvePeriod(Path.Combine(repo,"skin","Backdrop"),"morning")=="morning","four-period skin capability");
        string fake = Path.Combine(scratch, "unsupported"); Directory.CreateDirectory(fake);
        Check(!SkinSupport.Check(fake, out reason), "unsupported skin fallback");
        string marker = Path.Combine(fake, "Media", "DayNight", "skin-support.xml"); Directory.CreateDirectory(Path.GetDirectoryName(marker));
        foreach (string xml in new[] { "<dayNightSkin contract='2'><asset path='x.png'/></dayNightSkin>", "<dayNightSkin contract='1'/>", "<dayNightSkin contract='1'><asset path='missing.png'/></dayNightSkin>", "<dayNightSkin contract='1'><asset path='../../../escape.png'/></dayNightSkin>" })
        {
            File.WriteAllText(marker, xml); Check(!SkinSupport.Check(fake, out reason), "invalid/incomplete skin fallback");
        }

        var editor = Assembly.LoadFrom(Path.Combine(source, "BackdropMenuEditor.dll"));
        var apply = editor.GetType("MediaPortal.Plugins.DayNightBackgrounds", true).GetMethod("Apply", BindingFlags.NonPublic | BindingFlags.Static);
        string input = "<window><texture>Modern/tv.jpg</texture><texture mask=\"keep.png\">Modern\\studio.jpg</texture><texture>Animations//games//default.jpg</texture><texture>#fanart.custom</texture><texture>custom-photo.jpg</texture></window>";
        string output = (string)apply.Invoke(null, new object[] { input });
        Check(output.Contains("Modern/Day/tv.png") && output.Contains("Modern/Day/studio.png") && output.Contains("Modern/Day/abstract.png"), "editor transforms stock paths with mixed separators");
        Check(output.Contains("mask=\"keep.png\"") && output.Contains("<texture>#fanart.custom</texture><texture>custom-photo.jpg</texture>"), "editor retains masks, fanart and custom images");
        Check((string)apply.Invoke(null, new object[] { output }) == output, "editor transformation is idempotent");

        string uiSettings = Path.Combine(scratch, "dialog-settings.xml");
        new DayNightSettings().Save(uiSettings);
        var dialogType = typeof(DayNightPlugin).Assembly.GetType("Backdrop.DayNight.DayNightConfiguration", true);
        using (var dialog = (Form)Activator.CreateInstance(dialogType, BindingFlags.Instance | BindingFlags.NonPublic, null,
            new object[] { uiSettings }, CultureInfo.InvariantCulture))
        {
            var mode = (ComboBox)dialogType.GetField("mode", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(dialog);
            var day = (DateTimePicker)dialogType.GetField("day", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(dialog);
            var night = (DateTimePicker)dialogType.GetField("night", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(dialog);
            Check(mode.Items.Count == 6 && mode.SelectedIndex == 0 && day.Enabled && night.Enabled, "dialog exposes automatic mode and both times");
            // Real WinForms layout/paint with no visible window or taskbar entry.
            dialog.ShowInTaskbar = false;
            dialog.Opacity = 0;
            dialog.StartPosition = FormStartPosition.Manual;
            dialog.Location = new Point(-32000, -32000);
            dialog.Show();
            Application.DoEvents();
            using (var bitmap = new Bitmap(dialog.Width, dialog.Height))
            {
                dialog.DrawToBitmap(bitmap, new Rectangle(Point.Empty, bitmap.Size));
                bitmap.Save(Path.Combine(scratch, "configuration.png"), ImageFormat.Png);
            }
            mode.SelectedIndex = 4;
            Check(!day.Enabled && !night.Enabled, "manual mode disables schedule controls");
            day.Value = DateTime.Today.AddHours(15).AddMinutes(45);
            night.Value = DateTime.Today.AddHours(20).AddMinutes(5);
            var save = (Button)dialog.AcceptButton;
            typeof(Button).GetMethod("OnClick", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(save, new object[] { EventArgs.Empty });
            var saved = DayNightSettings.Load(uiSettings);
            Check(saved.Mode == BackgroundMode.Night && saved.DayStarts == new TimeSpan(15, 45, 0) && saved.NightStarts == new TimeSpan(20, 5, 0), "dialog saves selected mode and retained schedule");
            Check(dialog.DialogResult == DialogResult.OK, "successful dialog save closes with OK");
        }

        int expressions = 0;
        foreach (string page in Directory.GetFiles(Path.Combine(repo, "skin", "Backdrop"), "*.xml"))
        {
            var doc = new XmlDocument(); doc.Load(page);
            foreach (XmlNode node in doc.SelectNodes("//texture"))
            {
                string expression = node.InnerText;
                if (!expression.Contains("#DayNight.Period")) continue;
                expressions++;
                string[] values = expression.Split('\'');
                string morningPath=values[3], noonPath=values[7], day=values[11], night=values[13];
                foreach (string period in new[] { "night", "morning", "noon", "day", "night", "", "unsupported" })
                {
                    GUIPropertyManager.SetProperty("#DayNight.Period", period);
                    string resolved = GUIExpressionManager.Parse(expression);
                    Check(resolved == (period == "morning" ? morningPath : period == "noon" ? noonPath : period == "day" ? day : night), Path.GetFileName(page) + " resolves " + period);
                    Check(File.Exists(Path.Combine(repo, "skin", "Backdrop", "Media", resolved.Replace('/', Path.DirectorySeparatorChar))), "resolved texture exists");
                }
            }
        }
        Check(expressions == 42, "all 42 shipped texture bindings exercised");
        var plugin = new DayNightPlugin();
        Check(plugin.HasSetup() && plugin.CanEnable() && plugin.DefaultEnabled(), "plugin setup/enable contract");
        plugin.Start(); plugin.Start(); plugin.Stop(); plugin.Stop();
        Check(true, "repeat Start/Stop without GUI handle is safe");
        var previousForm = GUIGraphicsContext.form;
        using (var host = new Form { ShowInTaskbar = false, Opacity = 0, StartPosition = FormStartPosition.Manual, Location = new Point(-32000, -32000) })
        {
            try
            {
                host.Show(); GUIGraphicsContext.form = host;
                GUIPropertyManager.SetProperty("#DayNight.Supported", "pending");
                GUIPropertyManager.SetProperty("#DayNight.Period", "day");
                plugin.Start();
                DateTime deadline = DateTime.UtcNow.AddSeconds(3);
                while (GUIPropertyManager.GetProperty("#DayNight.Supported") == "pending" && DateTime.UtcNow < deadline)
                { Application.DoEvents(); Thread.Sleep(10); }
                Check(GUIPropertyManager.GetProperty("#DayNight.Supported") == "false" && GUIPropertyManager.GetProperty("#DayNight.Period") == "night", "timer publishes unsupported-skin fallback through the GUI thread");
                string configPath = (string)typeof(DayNightPlugin).GetProperty("SettingsPath", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null, null);
                string expectedMode;
                try { expectedMode = DayNightSettings.Load(configPath).Mode.ToString().ToLowerInvariant(); }
                catch { expectedMode = "disabled"; }
                Check(GUIPropertyManager.GetProperty("#DayNight.Mode") == expectedMode, "runtime update respects configuration or safe error fallback");
                plugin.Stop(); Application.DoEvents();
                Check(GUIPropertyManager.GetProperty("#DayNight.Mode") == "disabled" && GUIPropertyManager.GetProperty("#DayNight.Period") == "night", "Stop clears live state on GUI thread");
            }
            finally { plugin.Stop(); GUIGraphicsContext.form = previousForm; }
        }
        Console.WriteLine("PASS: {0} assertions; {1} native skin expressions; configuration save/render and GUI-thread lifecycle exercised; {2}-bit host. No installation or personal settings changes.", checks, expressions, IntPtr.Size * 8);
    }
}
