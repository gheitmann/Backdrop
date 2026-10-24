using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
//using System.Timers;
using System.Threading;
using MediaPortal.Configuration;
using MediaPortal.Dialogs;
using MediaPortal.GUI.Library;
using MediaPortal.Playlists;
using MediaPortal.Util;
using MediaPortal.Player;
using System.Timers;
using System.Xml;
using System.ComponentModel;
using System.Data;
using System.Drawing;


namespace MediaPortal.Plugins
{

[PluginIcons("BackdropMenuEditor.logo.png", "BackdropMenuEditor.logo_disabled.png")]

    public class BackdropMenuEditor : IPlugin, ISetupForm
    {
        #region declarations
        private readonly object lockObject = new object();
        private System.Timers.Timer refreshTimer = null;
        private int iTimeout = 5000;
        private string IdleTimeOther = null;
        private string IdleTimeBasic = null;
        private bool _isVisible = false;
        //private bool _isVisibleBasichomeMenu = false;
        private int _prevFocusedID = 0;
        private int _prevFocusedIDHome = 0;
        private int _currFocusedID = 0;
        private string doHideClock = null;
        #endregion


        private void SetProperty(string property, string value)
        {
            if (property == null)
                return;
            // If the value is empty always add a space
            // otherwise the property will keep 
            // displaying it's previous value
            if (String.IsNullOrEmpty(value))
                value = " ";

            GUIPropertyManager.SetProperty(property, value);
        }

        private void SetBlankProperty(string property, string value)
        {
            if (property == null)
                return;
            // If the value is empty always add a space
            // otherwise the property will keep 
            // displaying it's previous value
            if (String.IsNullOrEmpty(value))
                value = "";

            GUIPropertyManager.SetProperty(property, value);
        }

        private static void ApplyOptionalMovieFallback(int windowId)
        {
            if (windowId != 35 || GUIWindowManager.GetWindow(96742) != null) return;
            GUIWindow home = GUIWindowManager.GetWindow(35);
            if (home == null) return;
            foreach (GUIControl control in home.controlList)
            {
                GUIButtonControl button = control as GUIButtonControl;
                if (button != null && button.HyperLink == 96742) button.HyperLink = 6;
            }
        }
        public void GUIWindowManager_OnActivateWindow(int activeWindowId)
        {
            try
            {
                if (refreshTimer != null && refreshTimer.Enabled && (activeWindowId == 602 || activeWindowId == 2005))
                {
                    refreshTimer.Stop();
                }
                ApplyOptionalMovieFallback(activeWindowId);
                PluginMenuLayout.Apply(activeWindowId);
                bool hasFacade = false;
                GUIControlCollection gcg = GUIWindowManager.GetWindow(activeWindowId).controlList;
                foreach (var _guicontrol in gcg)
                {
                    if (_guicontrol is GUIFacadeControl)
                    {
                        hasFacade = true;
                    }
                }
                if (!hasFacade)
                {
                    GUIPropertyManager.SetProperty("#facadeview.layout", string.Empty);
                }

                SetTimeout(activeWindowId, 0);
                

            }
            catch
            {
                
            }
        }

        private static int ReadIdleSeconds(string value, int fallback)
        {
            int seconds;
            return Int32.TryParse(value, out seconds) && seconds >= 1 && seconds <= 3600 ? seconds : fallback;
        }

        private void SetTimeout(int activeWindowId, int addSeconds)
        {
            if (activeWindowId == 35)
            {
                iTimeout = (ReadIdleSeconds(IdleTimeBasic, 7) + addSeconds) * 1000;             
            }
            else
            {
                iTimeout = (ReadIdleSeconds(IdleTimeOther, 3) + addSeconds) * 1000;
            }

        }

        /*public void OnPlayBackStarted(g_Player.MediaType type, string filename)
            {
                try
                {                
                    if (refreshTimer != null && refreshTimer.Enabled && (type == g_Player.MediaType.Recording || type == g_Player.MediaType.TV || type == g_Player.MediaType.Video))
                    {
                        refreshTimer.Stop();
                    }
                }
                catch
                {
                }
            }

        public void OnPlayBackEnded(g_Player.MediaType type, string filename)
        {
            try
            {
                int activeWindowId = GUIWindowManager.ActiveWindow;
                SetTimeout(activeWindowId, 0);
                if (refreshTimer != null && !refreshTimer.Enabled)
                {
                    refreshTimer.Start();
                }
             
            }
            catch
            {              
            }
        }

        public void OnPlayBackStopped(g_Player.MediaType type, int stoptime, string filename)
        {
            try
            {
                int activeWindowId = GUIWindowManager.ActiveWindow;
                SetTimeout(activeWindowId, 0);
                if (refreshTimer != null && !refreshTimer.Enabled)
                {
                    refreshTimer.Start();
                }             
            }
            catch
            {
            }
        }*/

        public bool readMPConfigFile(MediaPortal.Profile.Settings xmlreader)
        {
            string skinName = xmlreader.GetValueAsString("skin", "name", "");
            if (skinName != null)
            {
                if (skinName.Equals("Backdrop"))
                {
                    return true;
                }
            }
            return false;
        }
    

        public void readConfigFile(MediaPortal.Profile.Settings xmlreader)
        {
            doHideClock = xmlreader.GetValueAsString("BackdropMenuEditor", "doHideClock", "");
            if (doHideClock == null || doHideClock.Length <= 0)
            {
                doHideClock = "False";
            }

            IdleTimeOther = xmlreader.GetValueAsString("BackdropMenuEditor", "IdleTimeOther", "");
            if (IdleTimeOther == null || IdleTimeOther.Length <= 0)
            {
                IdleTimeOther = "3";
            }

            IdleTimeBasic = xmlreader.GetValueAsString("BackdropMenuEditor", "IdleTimeBasic", "");
            if (IdleTimeBasic == null || IdleTimeBasic.Length <= 0)
            {
                IdleTimeBasic = "7";
            }
        }

        /// <summary>
        /// The plugin is started by Mediaportal
        /// </summary>
        public void Start()
        {
            try 
            {
                if (CheckSkinName())
                {
                    PointerSeek.Install();
                    PointerTopbar.Install();
                    PointerRadio.Install();
                    PluginMenuLayout.Install();
                    using (MediaPortal.Profile.Settings xmlreader = new MediaPortal.Profile.Settings(Config.GetFile(Configuration.Config.Dir.Config, "BackdropMenuEditor.xml")))
                    {
                        readConfigFile(xmlreader);
                    }
                    //iTimeout = Int32.Parse(IdleTimeBasic)*1000;
                    refreshTimer = new System.Timers.Timer(500);
                    SetTimeout(35, 0);
                    refreshTimer.Elapsed += new ElapsedEventHandler(UpdateImageTimer);
                    refreshTimer.Interval = 500;
                    GUIButtonControl.HideControl(GUIWindowManager.ActiveWindow, 93761234);
                    GUIWindowManager.OnActivateWindow += new GUIWindowManager.WindowActivationHandler(GUIWindowManager_OnActivateWindow);
                    GUIWindowManager.OnNewAction += new OnActionHandler(this.GUIWindowManager_OnNewAction);
                    //                g_Player.PlayBackStarted += new MediaPortal.Player.g_Player.StartedHandler(OnPlayBackStarted);
                    //                g_Player.PlayBackEnded += new MediaPortal.Player.g_Player.EndedHandler(OnPlayBackEnded);
                    //                g_Player.PlayBackStopped += new MediaPortal.Player.g_Player.StoppedHandler(OnPlayBackStopped);
                }
            }
            catch //(Exception ex)
            {            
            }
            
        }

        /// <summary>
        /// User has been "idle" for a short time. Method used to prevent loading
        /// images during a fast scroll of selected items
        /// </summary>
        /// <returns></returns>
        public bool IsIdle()
        {
            try
            {
                if (iTimeout == 0)
                {
                    return false;
                }
                TimeSpan ts = DateTime.Now - GUIGraphicsContext.LastActivity;
                if (ts.TotalMilliseconds >= iTimeout)
                {
                    return true;
                }
            }
            catch
            {
            }
            return false;
        }

        public bool IsIdleBasichomeMenu()
        {
            try
            {
                if (iTimeout == 0)
                {
                    return false;
                }
                TimeSpan ts = DateTime.Now - GUIGraphicsContext.LastActivity;
                if (ts.TotalMilliseconds >= 350)
                {
                    return true;
                }
            }
            catch
            {
            }
            return false;
        }

        private void GUIWindowManager_OnNewAction(MediaPortal.GUI.Library.Action action)
        {           
            // Mouse hover must wake the submenu after the idle timer stopped.
            // This event runs on the GUI thread; do not restore/load windows here.
            if (action.wID == MediaPortal.GUI.Library.Action.ActionType.ACTION_MOUSE_MOVE
                && GUIWindowManager.ActiveWindow == 35)
            {
                GUIButtonControl.ShowControl(35, 93761236);
                GUIButtonControl.HideControl(35, 93761234);
                if (refreshTimer != null && !refreshTimer.Enabled) refreshTimer.Start();
                return;
            }
            if (action.IsUserAction())
            {
                int _activeWindow = GUIWindowManager.ActiveWindow;
                
                if (action.wID == MediaPortal.GUI.Library.Action.ActionType.ACTION_MOVE_LEFT)
                {
                    if (refreshTimer != null && refreshTimer.Enabled == false && !(_activeWindow == 602 || _activeWindow == 2005))
                    {
                        refreshTimer.Start();
                    }
                    GUIButtonControl.HideControl(_activeWindow, 93761254);
                    GUIButtonControl.HideControl(_activeWindow, 93761234);
                    GUIButtonControl.HideControl(_activeWindow, 93761236);
                    if (_activeWindow == 35)
                    {                                               
                        if (_currFocusedID.ToString().StartsWith("9191998") || _currFocusedID.ToString().StartsWith("9191999"))
                        {
                            GUIMessage msg = new GUIMessage(GUIMessage.MessageType.GUI_MSG_SETFOCUS, _activeWindow, 0, _prevFocusedID, 0, 0, null);
                            GUIWindowManager.GetWindow(_activeWindow).OnMessage(msg);
                            //_prevFocusedID = 0;
                            _currFocusedID = 0;
                        }
                    }
                }
                else if (action.wID == MediaPortal.GUI.Library.Action.ActionType.ACTION_MOVE_RIGHT)
                {
                    if (refreshTimer != null && refreshTimer.Enabled == false && !(_activeWindow == 602 || _activeWindow == 2005))
                    {
                        refreshTimer.Start();
                    }
                    GUIButtonControl.ShowControl(_activeWindow, 93761254);
                    GUIButtonControl.HideControl(_activeWindow, 93761234);
                    GUIButtonControl.HideControl(_activeWindow, 93761236);
                    _currFocusedID = GUIWindowManager.GetWindow(_activeWindow).GetFocusControlId();                    
                }
                else if (action.wID == MediaPortal.GUI.Library.Action.ActionType.ACTION_MOVE_UP)
                {
                    if (refreshTimer != null && refreshTimer.Enabled == false && !(_activeWindow == 602 || _activeWindow == 2005))
                    {
                        refreshTimer.Start();
                    }
                    GUIButtonControl.HideControl(_activeWindow, 93761264);
                    GUIButtonControl.HideControl(_activeWindow, 93761234);
                    //GUIButtonControl.HideControl(_activeWindow, 93761236);
                    if (_activeWindow == 35)
                    {
                        if (!_currFocusedID.ToString().StartsWith("9191998") && !_currFocusedID.ToString().StartsWith("9191999"))
                        {
                            _prevFocusedID = GUIWindowManager.GetWindow(_activeWindow).PreviousFocusedId;
                        }

                    }
                }
                else if (action.wID == MediaPortal.GUI.Library.Action.ActionType.ACTION_MOVE_DOWN)
                {
                    if (refreshTimer != null && refreshTimer.Enabled == false && !(_activeWindow == 602 || _activeWindow == 2005))
                    {
                        refreshTimer.Start();
                    }
                    GUIButtonControl.ShowControl(_activeWindow, 93761264);
                    GUIButtonControl.HideControl(_activeWindow, 93761234);
                    //GUIButtonControl.HideControl(_activeWindow, 93761236);
                    if (_activeWindow == 35)
                    {
                        if (!_currFocusedID.ToString().StartsWith("9191998") && !_currFocusedID.ToString().StartsWith("9191999"))
                        {
                            _prevFocusedID = GUIWindowManager.GetWindow(_activeWindow).PreviousFocusedId;
                        }
                    }
                }
                
            }
           
        }



        /// <summary>
        /// The Plugin is stopped
        // </summary>
        public void Stop()
        {
            try
            {
                PointerSeek.Uninstall();
                PointerTopbar.Uninstall();
                PointerRadio.Uninstall();
                PluginMenuLayout.Uninstall();
                if (refreshTimer != null && refreshTimer.Enabled)
                {
                    refreshTimer.Stop();
                }
                if (refreshTimer != null) { refreshTimer.Dispose(); refreshTimer = null; }
                GUIWindowManager.OnActivateWindow -= new GUIWindowManager.WindowActivationHandler(GUIWindowManager_OnActivateWindow);
                GUIWindowManager.OnNewAction -= new OnActionHandler(this.GUIWindowManager_OnNewAction);
//                g_Player.PlayBackEnded -= new MediaPortal.Player.g_Player.EndedHandler(OnPlayBackEnded);
//                g_Player.PlayBackStopped -= new MediaPortal.Player.g_Player.StoppedHandler(OnPlayBackStopped);
//                g_Player.PlayBackStarted -= new MediaPortal.Player.g_Player.StartedHandler(OnPlayBackStarted);
            }
            catch
            {
            }
        } 
      
        public void UpdateImageTimer()
        {
            try
            {
                int _activeWindow = GUIWindowManager.ActiveWindow;
                if (_activeWindow == 35 && GUIWindowManager.GetWindow(_activeWindow).GetFocusControlId() == -1)
                {
                    GUIControl.FocusControl(_activeWindow, _prevFocusedIDHome);
                }
                else
                {
                    _prevFocusedIDHome = GUIWindowManager.GetWindow(_activeWindow).GetFocusControlId();
                }
                
                if (IsIdleBasichomeMenu())
                {
                    
                    if (_activeWindow == 35)
                    {
                        GUIButtonControl.ShowControl(GUIWindowManager.ActiveWindow, 93761236);
                        //_isVisibleBasichomeMenu = true;                        
                    }
                }

                if (IsIdle())
                {                                              
                    if (_activeWindow == 35)
                    {
                         GUIButtonControl.ShowControl(GUIWindowManager.ActiveWindow, 93761234);
                         _isVisible = true;
                         if (doHideClock.Equals("True"))
                         {
                             GUIButtonControl.ShowControl(GUIWindowManager.ActiveWindow, 93761232);
                         }
                         else
                         {
                             GUIButtonControl.HideControl(GUIWindowManager.ActiveWindow, 93761232);
                         }
                    }
                    else if (_activeWindow == 2)
                    {
                        GUIButtonControl.ShowControl(GUIWindowManager.ActiveWindow, 93761234);                       
                        _isVisible = true;
                    }
                    else if (_activeWindow == 9811 && GUIWindowManager.GetWindow(_activeWindow).GetFocusControlId() == 50)
                    {
                            GUIButtonControl.ShowControl(GUIWindowManager.ActiveWindow, 93761234);
                            GUIControl.FocusControl(GUIWindowManager.ActiveWindow, 93761234);
                            _isVisible = true;
                    }                        
                    else if ((_activeWindow == 603) && GUIWindowManager.GetWindow(_activeWindow).GetFocusControlId() == 50)
                    {
                        string _selecteditem = GUIPropertyManager.GetProperty("#selecteditem");
                        if ((_selecteditem != null && _selecteditem.Length > 1 && !_selecteditem.Equals("..")))
                        {
                            GUIButtonControl.ShowControl(GUIWindowManager.ActiveWindow, 93761234);
                            GUIControl.FocusControl(GUIWindowManager.ActiveWindow, 93761234);
                            _isVisible = true;
                        }
                        else if (_isVisible)
                        {
                            GUIButtonControl.HideControl(GUIWindowManager.ActiveWindow, 93761234);
                        }
                    }
                    else if ((_activeWindow == 25 || _activeWindow == 6) && GUIWindowManager.GetWindow(_activeWindow).GetFocusControlId() == 50)
                    {
                        string _selecteditem = GUIPropertyManager.GetProperty("#selecteditem");
                        if ((_selecteditem != null && _selecteditem.Length > 1 && !_selecteditem.Equals("..")))
                        {
                            GUIButtonControl.ShowControl(GUIWindowManager.ActiveWindow, 93761234);
                            GUIControl.FocusControl(GUIWindowManager.ActiveWindow, 93761234);
                            _isVisible = true;
                        }
                        else if (_isVisible)
                        {
                            GUIButtonControl.HideControl(GUIWindowManager.ActiveWindow, 93761234);
                        }
                    }
                    else if ((_activeWindow == 504) && GUIWindowManager.GetWindow(_activeWindow).GetFocusControlId() == 50)
                    {
                        string _selecteditem = GUIPropertyManager.GetProperty("#selecteditem");
                        string _genre = GUIPropertyManager.GetProperty("#ArtistInfo.Genres");
                        string _genre1 = GUIPropertyManager.GetProperty("#genre");
                        string _bio = GUIPropertyManager.GetProperty("#ArtistInfo.Bio");
                        string _review = GUIPropertyManager.GetProperty("#AlbumInfo.Review");
                        string _comment = GUIPropertyManager.GetProperty("#comment");
                        if ((_selecteditem != null && _selecteditem.Length > 1 && !_selecteditem.Equals("..")) && ((_genre != null && _genre.Length > 0) || (_genre1 != null && _genre1.Length > 0) || (_bio != null && _bio.Length > 0) || (_review != null && _review.Length > 0) || (_comment != null && _comment.Length > 0)))
                        {
                            GUIButtonControl.ShowControl(GUIWindowManager.ActiveWindow, 93761234);
                            GUIControl.FocusControl(GUIWindowManager.ActiveWindow, 93761234);
                            _isVisible = true;
                        }
                        else if (_isVisible)
                        {
                            GUIButtonControl.HideControl(GUIWindowManager.ActiveWindow, 93761234);
                        }
                    }                        
                    else if ((_activeWindow == 96742))
                    {
                        String _layout = GUIPropertyManager.GetProperty("#facadeview.layout");

                        if (_layout != null && !_layout.Equals("coverflow") && GUIWindowManager.GetWindow(_activeWindow).GetFocusControlId() == 50)
                        {
                            GUIButtonControl.ShowControl(GUIWindowManager.ActiveWindow, 93761234);
                            
                            if (GUIWindowManager.GetWindow(_activeWindow).GetFocusControlId() != 9879876 && GUIWindowManager.GetWindow(_activeWindow).GetFocusControlId() != 9879877 && GUIWindowManager.GetWindow(_activeWindow).GetFocusControlId() != 6 && GUIWindowManager.GetWindow(_activeWindow).GetFocusControlId() != 4755 && GUIWindowManager.GetWindow(_activeWindow).GetFocusControlId() != 69999 && GUIWindowManager.GetWindow(_activeWindow).GetFocusControlId() != 992899)
                            {                            
                                GUIControl.FocusControl(GUIWindowManager.ActiveWindow, 93761234);
                            }
                            _isVisible = true;
                        }
                    }
                    else if ((_activeWindow == 880))
                    {
                        GUIButtonControl.ShowControl(GUIWindowManager.ActiveWindow, 93761234);
                        GUIControl.FocusControl(GUIWindowManager.ActiveWindow, 93761234);
                        _isVisible = true;
                    }                    
                }
                else
                {
                    if (_isVisible && GUIWindowManager.ActiveWindow != 35)
                    {
                        GUIButtonControl.HideControl(GUIWindowManager.ActiveWindow, 93761234);
                        if (GUIWindowManager.GetWindow(_activeWindow).GetFocusControlId() != 9879876 && GUIWindowManager.GetWindow(_activeWindow).GetFocusControlId() != 9879877 && GUIWindowManager.GetWindow(_activeWindow).GetFocusControlId() != 6 && GUIWindowManager.GetWindow(_activeWindow).GetFocusControlId() != 4755 && GUIWindowManager.GetWindow(_activeWindow).GetFocusControlId() != 69999 && GUIWindowManager.GetWindow(_activeWindow).GetFocusControlId() != 992899)
                        {
                            GUIControl.FocusControl(GUIWindowManager.ActiveWindow, 50);
                        }
                        _isVisible = false;                        
                    }
                    if (GUIWindowManager.ActiveWindow == 35)
                    {
                        GUIButtonControl.HideControl(GUIWindowManager.ActiveWindow, 93761232);
                    }
                }
            }
            catch
            {
            }
        }

        public void UpdateImageTimer(Object stateInfo, ElapsedEventArgs e)
        {
            try
            {
                if (Monitor.TryEnter(lockObject))
                {
                    try
                    {
                        UpdateImageTimer();
                    }
                    finally
                    {
                        Monitor.Exit(lockObject);
                    }
                }
            }
            catch
            {                
            }
        }        

        #region ISetupForm Members

        // Returns the name of the plugin which is shown in the plugin menu
        public string PluginName()
        {
            return "BackdropMenuEditor Menu Editor";
        }

        // Returns the description of the plugin is shown in the plugin menu
        public string Description()
        {
            return "BackdropMenuEditor skin menu (basichome) editor for MediaPortal.";
        }

        // Returns the author of the plugin which is shown in the plugin menu
        public string Author()
        {
            return "cul8er";
        }

        // show the setup dialog
        public void ShowPlugin()
        {
            BackdropMenuEditorConfig config = new BackdropMenuEditorConfig();
            config.ShowDialog();
        }

        // Indicates whether plugin can be enabled/disabled
        public bool CanEnable()
        {
            return true;
        }

        private bool CheckSkinName()
        {
            try
            {
                bool doEnable = false;
                using (MediaPortal.Profile.Settings xmlreader = new MediaPortal.Profile.Settings(Config.GetFile(Configuration.Config.Dir.Config, "MediaPortal.xml")))
                {
                    doEnable = readMPConfigFile(xmlreader);
                }
                return doEnable;
            }
            catch
            {
                return false;
            }

        }

        // Get Windows-ID
        public int GetWindowId()
        {
            // WindowID of windowplugin belonging to this setup
            // enter your own unique code
            return 730710;
        }

        // Indicates if plugin is enabled by default;
        public bool DefaultEnabled()
        {
            return true;
        }

        // indicates if a plugin has it's own setup screen
        public bool HasSetup()
        {
            return true;
        }

        /// <summary>
        /// If the plugin should have it's own button on the main menu of MediaPortal then it
        /// should return true to this method, otherwise if it should not be on home
        /// it should return false
        /// </summary>
        /// <param name="strButtonText">text the button should have</param>
        /// <param name="strButtonImage">image for the button, or empty for default</param>
        /// <param name="strButtonImageFocus">image for the button, or empty for default</param>
        /// <param name="strPictureImage">subpicture for the button or empty for none</param>
        /// <returns>true : plugin needs it's own button on home
        /// false : plugin does not need it's own button on home</returns>

        public bool GetHome(out string strButtonText, out string strButtonImage,
          out string strButtonImageFocus, out string strPictureImage)
        {
            strButtonText = String.Empty;// strButtonText = PluginName();
            strButtonImage = String.Empty;
            strButtonImageFocus = String.Empty;
            strPictureImage = String.Empty;
            return false;
        }



        #endregion









    }
}





