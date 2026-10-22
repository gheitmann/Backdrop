using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cornerstone.Database;
using Cornerstone.Database.Tables;
using MediaPortal.Plugins.MovingPictures;
using MediaPortal.Plugins.MovingPictures.Database;
using MediaPortal.Plugins.MovingPictures.MainUI;


namespace MediaPortal.Plugins
{
    public static class MovingPictureHandler
    {
        private static ArrayList al = new ArrayList();

        public static ArrayList GetRootNodes()
        {           
            al = new ArrayList();
            al.Add("");
            try
            {            
                DBMenu<DBMovieInfo> menu = MediaPortal.Plugins.MovingPictures.MovingPicturesCore.Settings.CategoriesMenu;
                foreach (DBNode<DBMovieInfo> currNode in menu.RootNodes)
                {
                    al.Add(currNode.Name);
                }
            }
            catch 
            { }
            return al;
        }

        public static ArrayList GetSubNodes(string category)
        {
            al = new ArrayList();
            al.Add("");
            DBNode<DBMovieInfo> _dbnode = GetSubNodes(MovingPicturesCore.Settings.CategoriesMenu.RootNodes, category, false);
            return al;
        }

        private static DBNode<DBMovieInfo> GetSubNodes(List<DBNode<DBMovieInfo>> nodeList, string category, bool foundCategory)
        {            
            try
            {                
                for (int i = 0; i < nodeList.Count; i++)
                {
                    if (foundCategory)
                    {
                        al.Add(nodeList[i].Name);
                    }
                    if (nodeList[i].Name.Equals(category))
                    {                        
                        if (nodeList[i].HasChildren)
                        {
                            DBNode<DBMovieInfo> result = GetSubNodes(nodeList[i].Children, category, true);
                            if (result != null) return result;
                        }
                    }                    
                }
                
                                              
            }
            catch //(Exception ex
            {
                
            }
            return null;
        }
    }
}
