// Backdrop MP 1.39 port: optional plugins are resolved at configuration time.
// No compile-time references to the MP 1.2 versions of these plugins.
using System;
using System.Collections;
using System.IO;
using System.Reflection;
using MediaPortal.Configuration;
using MediaPortal.GUI.Library;

namespace MediaPortal.Plugins
{
    internal static class OptionalPlugin
    {
        internal static Type Type(string assembly, string name)
        {
            foreach (Assembly loaded in AppDomain.CurrentDomain.GetAssemblies())
                if (loaded.GetName().Name == assembly) return loaded.GetType(name, true);
            string file = Path.Combine(Config.GetFolder(Config.Dir.Plugins), "Windows\\" + assembly + ".dll");
            if (!File.Exists(file)) file = Path.Combine(Config.GetFolder(Config.Dir.Plugins), "Windows\\" + assembly + "\\" + assembly + ".dll");
            if (!File.Exists(file)) throw new FileNotFoundException("Optional plugin is not installed", file);
            return Assembly.LoadFrom(file).GetType(name, true);
        }
        internal static object Get(object target, string member)
        {
            Type type = target as Type ?? target.GetType();
            object instance = target is Type ? null : target;
            BindingFlags flags = BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.FlattenHierarchy;
            PropertyInfo property = type.GetProperty(member, flags);
            if (property != null) return property.GetValue(instance, null);
            FieldInfo field = type.GetField(member, flags);
            if (field != null) return field.GetValue(instance);
            throw new MissingMemberException(type.FullName, member);
        }
        internal static void Set(object target, string member, object value)
        {
            Type type = target.GetType();
            PropertyInfo property = type.GetProperty(member);
            if (property != null) { property.SetValue(target, value, null); return; }
            FieldInfo field = type.GetField(member);
            if (field != null) { field.SetValue(target, value); return; }
            throw new MissingMemberException(type.FullName, member);
        }
        internal static object Call(object target, string member)
        {
            Type type = target as Type ?? target.GetType();
            return type.GetMethod(member, System.Type.EmptyTypes).Invoke(target is Type ? null : target, null);
        }
        internal static void Error(string plugin, Exception ex)
        {
            Log.Warn("Backdrop: {0} menu choices unavailable: {1}", plugin, ex.GetBaseException().Message);
        }
    }
    public static class TVSeriesHandler
    {
        public static ArrayList GetViews()
        {
            ArrayList result = new ArrayList();
            try {
                foreach (object pair in (IEnumerable)OptionalPlugin.Call(OptionalPlugin.Type("MP-TVSeries", "WindowPlugins.GUITVSeries.DBView"), "GetSkinViews"))
                    result.Add(OptionalPlugin.Get(pair, "Key"));
            } catch (Exception ex) { OptionalPlugin.Error("MP-TVSeries", ex); }
            return result;
        }
    }
    public static class MovingPictureHandler
    {
        private static object Roots()
        {
            return OptionalPlugin.Get(OptionalPlugin.Get(OptionalPlugin.Get(OptionalPlugin.Type("MovingPictures", "MediaPortal.Plugins.MovingPictures.MovingPicturesCore"), "Settings"), "CategoriesMenu"), "RootNodes");
        }
        public static ArrayList GetRootNodes()
        {
            ArrayList result = new ArrayList(); result.Add("");
            try { foreach (object node in (IEnumerable)Roots()) result.Add(OptionalPlugin.Get(node, "Name")); }
            catch (Exception ex) { OptionalPlugin.Error("Moving Pictures", ex); }
            return result;
        }
        private static bool Find(IEnumerable nodes, string category, ArrayList result, int depth)
        {
            if (depth > 32) return false;
            foreach (object node in nodes) {
                bool children = (bool)OptionalPlugin.Get(node, "HasChildren");
                if (Convert.ToString(OptionalPlugin.Get(node, "Name")) == category) {
                    if (children) foreach (object child in (IEnumerable)OptionalPlugin.Get(node, "Children")) result.Add(OptionalPlugin.Get(child, "Name"));
                    return true;
                }
                if (children && Find((IEnumerable)OptionalPlugin.Get(node, "Children"), category, result, depth + 1)) return true;
            }
            return false;
        }
        public static ArrayList GetSubNodes(string category)
        {
            ArrayList result = new ArrayList(); result.Add("");
            try { Find((IEnumerable)Roots(), category, result, 0); }
            catch (Exception ex) { OptionalPlugin.Error("Moving Pictures", ex); }
            return result;
        }
    }
    public static class OnlineVideoHandler
    {
        private static object Settings()
        {
            return OptionalPlugin.Get(OptionalPlugin.Type("OnlineVideos", "OnlineVideos.OnlineVideoSettings"), "Instance");
        }
        public static ArrayList GetSites()
        {
            ArrayList result = new ArrayList();
            try {
                object settings = Settings();
                OptionalPlugin.Get(OptionalPlugin.Type("OnlineVideos.MediaPortal1", "OnlineVideos.MediaPortal1.PluginConfiguration"), "Instance");
                OptionalPlugin.Get(OptionalPlugin.Type("OnlineVideos.MediaPortal1", "OnlineVideos.MediaPortal1.Translator"), "Lang");
                OptionalPlugin.Set(settings, "ConfigDir", Config.GetFolder(Config.Dir.Config));
                OptionalPlugin.Set(settings, "DllsDir", Path.Combine(Config.GetFolder(Config.Dir.Plugins), "Windows\\OnlineVideos"));
                using (MediaPortal.Profile.Settings config = new MediaPortal.Profile.Settings(Config.GetFile(Config.Dir.Config, "MediaPortal.xml")))
                    OptionalPlugin.Set(settings, "DownloadDir", config.GetValueAsString("onlinevideos", "downloadDir", ""));
                OptionalPlugin.Set(settings, "FavDB", OptionalPlugin.Get(OptionalPlugin.Type("OnlineVideos.MediaPortal1", "OnlineVideos.MediaPortal1.FavoritesDatabase"), "Instance"));
                OptionalPlugin.Call(settings, "BuildSiteUtilsList");
                foreach (object pair in (IEnumerable)OptionalPlugin.Get(settings, "SiteUtilsList"))
                    result.Add(OptionalPlugin.Get(OptionalPlugin.Get(OptionalPlugin.Get(pair, "Value"), "Settings"), "Name"));
            } catch (Exception ex) { OptionalPlugin.Error("OnlineVideos", ex); }
            return result;
        }
        public static ArrayList GetCategories(string site)
        {
            ArrayList result = new ArrayList();
            try {
                foreach (object pair in (IEnumerable)OptionalPlugin.Get(Settings(), "SiteUtilsList")) {
                    if (Convert.ToString(OptionalPlugin.Get(pair, "Key")) != site) continue;
                    object utility = OptionalPlugin.Get(pair, "Value");
                    object settings = OptionalPlugin.Get(utility, "Settings");
                    if (!(bool)OptionalPlugin.Get(settings, "DynamicCategoriesDiscovered")) OptionalPlugin.Call(utility, "DiscoverDynamicCategories");
                    IEnumerable categories = OptionalPlugin.Get(settings, "Categories") as IEnumerable;
                    if (categories != null) foreach (object category in categories) result.Add(OptionalPlugin.Get(category, "Name"));
                }
            } catch (Exception ex) { OptionalPlugin.Error("OnlineVideos", ex); }
            return result;
        }
    }
}
