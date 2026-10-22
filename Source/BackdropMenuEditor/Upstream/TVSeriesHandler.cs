using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using WindowPlugins.GUITVSeries;


namespace MediaPortal.Plugins
{
    public static class TVSeriesHandler
    {
        private static ArrayList al = new ArrayList();

        public static ArrayList GetViews()
        {
            //al = new ArrayList();
            //al.Add("");
            try
            {
                List<KeyValuePair<string, string>> _views = DBView.GetSkinViews();

                foreach (KeyValuePair<string, string> tvv in _views)
                {         
                    al.Add(tvv.Key);
                }


                return al;
            }
            catch// (Exception ex)
            {
                //MediaPortal.GUI.Library.Log.Error(ex.ToString());
            }
            return al;
        }


    }
}
