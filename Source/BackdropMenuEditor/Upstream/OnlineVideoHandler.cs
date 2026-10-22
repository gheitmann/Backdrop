using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using OnlineVideos;
using MediaPortal.Configuration;
//using MediaPortal.GUI.Library;

//Credits to the author of the StreamedMP plugin for base for this code

namespace MediaPortal.Plugins
{
    public static class OnlineVideoHandler
    {
        private static ArrayList al = new ArrayList();

        public static ArrayList GetSites()
        {           
            try
            {
                //al = new ArrayList();
                //al.Add("");
                string _lang = OnlineVideos.MediaPortal1.Translator.Lang;
                OnlineVideoSettings.Instance.ConfigDir = Config.GetFolder(Configuration.Config.Dir.Config);
                string _downloadDir = "";
                using (MediaPortal.Profile.Settings xmlreader = new MediaPortal.Profile.Settings(Config.GetFile(Config.Dir.Config, "MediaPortal.xml")))
                {
                    _downloadDir = xmlreader.GetValueAsString("onlinevideos", "downloadDir", string.Empty);
                }
                OnlineVideoSettings.Instance.DllsDir = Path.Combine(Config.GetFolder(Configuration.Config.Dir.Plugins), @"Windows\OnlineVideos");                    
                OnlineVideoSettings.Instance.DownloadDir = _downloadDir;                
                OnlineVideoSettings.Instance.FavDB = OnlineVideos.MediaPortal1.FavoritesDatabase.Instance;
                OnlineVideoSettings onlineVideos = OnlineVideos.OnlineVideoSettings.Instance;
                onlineVideos.LoadSites();
                OnlineVideoSettings.Instance.BuildSiteUtilsList();
                foreach (var site in OnlineVideoSettings.Instance.SiteUtilsList)
                {
                    al.Add(site.Value.Settings.Name);                    
                }                
                return al;
            }
            catch// (Exception ex)
            {
                //MediaPortal.GUI.Library.Log.Error(ex.ToString());
            }
            return al;
        }

        public static ArrayList GetCategories(string site)
        {
            try
            {
                //al = new ArrayList();
                //al.Add("");
                var siteUtil = OnlineVideoSettings.Instance.SiteUtilsList.SingleOrDefault(s => s.Key == site);

                if (siteUtil.Value.Settings.Categories != null)
                {
                    if (!siteUtil.Value.Settings.DynamicCategoriesDiscovered)
                    {
                        try
                        {
                            siteUtil.Value.DiscoverDynamicCategories();
                        }
                        catch { }
                        List<string> categories = new List<string>();
                        categories = siteUtil.Value.Settings.Categories.Select(c => c.Name).ToList();
                        for (int i = 0; i < categories.Count; i++)
                        {
                            al.Add(categories[i].ToString());                    
                        }
                        
                    }
                }
                return al;
            }
            catch //(Exception ex)
            {
                //MediaPortal.GUI.Library.Log.Error(ex.ToString());
            }
            return al;
        }

        
    }
}
