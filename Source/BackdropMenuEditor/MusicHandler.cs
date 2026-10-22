using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using MediaPortal.GUI.Music;
using MediaPortal.GUI.View;

namespace MediaPortal.Plugins
{
    public static class MusicHandler
    {
        private static ArrayList al = new ArrayList();

        public static ArrayList GetViews()
        {
            al = new ArrayList();
            al.Add("");
            try
            {
                MusicViewHandler MusicViews = new MusicViewHandler();
                foreach (ViewDefinition MusicView in MusicViews.Views)
                {
                    al.Add(MusicView.LocalizedName + "<" + MusicView.Name + ">");
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
