using System;
using System.Drawing;
using System.Windows.Forms;
using MediaPortal.GUI.Library;
using MediaPortal.Player;
using MPAction = MediaPortal.GUI.Library.Action;

namespace MediaPortal.Plugins
{
    // The animated music menu's native checkbutton misses pointer activation at
    // its state mark. Retain required control 18 and dispatch one native selection.
    internal sealed class PointerRadio : IMessageFilter
    {
        private static PointerRadio filter;
        private int pressedWindow = -1;
        private IntPtr pressedHandle;
        internal static void Install()
        {
            if (filter != null) return;
            filter = new PointerRadio();
            Application.AddMessageFilter(filter);
        }
        internal static void Uninstall()
        {
            if (filter == null) return;
            Application.RemoveMessageFilter(filter);
            filter = null;
        }
        public bool PreFilterMessage(ref Message message)
        {
            const int Down=0x0201, DoubleDown=0x0203, Up=0x0202;
            if (message.Msg==0x001f || (message.Msg==0x001c && message.WParam==IntPtr.Zero))
            { pressedWindow=-1; return false; }
            if (message.Msg!=Down && message.Msg!=DoubleDown && message.Msg!=Up) return false;
            int capturedWindow=pressedWindow;
            IntPtr capturedHandle=pressedHandle;
            if (message.Msg==Up) pressedWindow=-1;
            try
            {
                int active=GUIWindowManager.ActiveWindow;
                if ((active!=501 && active!=504) || GUIWindowManager.RoutedWindow!=-1 ||
                    !string.Equals(GUIGraphicsContext.SkinName,"Backdrop",StringComparison.OrdinalIgnoreCase))
                    return message.Msg==Up && capturedWindow>=0;
                GUIWindow window=GUIWindowManager.GetWindow(active);
                GUICheckButton radio=window==null?null:window.GetControl(18) as GUICheckButton;
                int focused=window==null?-1:window.GetFocusControlId();
                bool menuFocused=focused==2 || focused==3 || focused==5 || focused==9 || focused==10 ||
                    focused==9876561 || focused==12 || focused==18 || focused==14 || focused==16 || focused==991799 || focused==991899;
                Control receiver=Control.FromHandle(message.HWnd);
                Form form=receiver==null?null:receiver.FindForm();
                if (radio==null || !radio.IsVisible || !radio.IsEnabled || !menuFocused || form==null ||
                    form!=Form.ActiveForm || form.ClientSize.Width<=0 || form.ClientSize.Height<=0)
                    return message.Msg==Up && capturedWindow>=0;
                long packed=message.LParam.ToInt64();
                Point p=form.PointToClient(receiver.PointToScreen(new Point((short)packed,(short)(packed>>16))));
                int x=(int)Math.Round(p.X*(double)GUIGraphicsContext.Width/form.ClientSize.Width);
                int y=(int)Math.Round(p.Y*(double)GUIGraphicsContext.Height/form.ClientSize.Height);
                bool inside=new Rectangle(radio.XPosition,radio.YPosition,radio.Width,radio.Height).Contains(x,y);
                if (message.Msg==Down || message.Msg==DoubleDown)
                {
                    pressedWindow=-1;
                    if (!inside) return false;
                    pressedWindow=active; pressedHandle=message.HWnd;
                    GUIGraphicsContext.ResetLastActivity();
                    return true;
                }
                if (capturedWindow<0) return false;
                if (inside && capturedWindow==active && capturedHandle==message.HWnd)
                {
                    GUIControl.FocusControl(active,18);
                    GUIGraphicsContext.ResetLastActivity();
                    radio.OnAction(new MPAction(MPAction.ActionType.ACTION_SELECT_ITEM,0,0));
                    Log.Debug("Backdrop radio pointer: selected control 18 on music window {0}",active);
                }
                return true;
            }
            catch (Exception error)
            {
                pressedWindow=-1;
                Log.Warn("Backdrop radio pointer failed: {0}",error.Message);
                return capturedWindow>=0;
            }
        }
    }

    // Only the visible Backdrop topbar is handled here. Native list/context input is untouched.
    internal sealed class PointerTopbar : IMessageFilter
    {
        private static PointerTopbar filter;
        private int pressedId = -1, pressedWindow = -1;
        private IntPtr pressedHandle;

        internal static void Install()
        {
            if (filter != null) return;
            filter = new PointerTopbar();
            Application.AddMessageFilter(filter);
            Log.Debug("Backdrop topbar pointer: installed on thread {0}", System.Threading.Thread.CurrentThread.ManagedThreadId);
        }
        internal static void Uninstall()
        {
            if (filter == null) return;
            Application.RemoveMessageFilter(filter);
            filter = null;
        }

        private static bool Eligible()
        {
            return string.Equals(GUIGraphicsContext.SkinName, "Backdrop", StringComparison.OrdinalIgnoreCase) &&
                GUIWindowManager.RoutedWindow == -1 && !GUIGraphicsContext.IsFullScreenVideo &&
                !GUIGraphicsContext.DisableTopBar && !GUIGraphicsContext.TopBarHidden;
        }

        private static GUIButtonControl Hit(GUIWindow bar, int x, int y)
        {
            GUIOverlayWindow overlay = bar as GUIOverlayWindow;
            if (overlay == null || !overlay.DoesPostRender()) return null;
            foreach (GUIControl control in bar.controlList)
            {
                GUIButtonControl button = control as GUIButtonControl;
                int id; bool focus;
                if (button != null && button.IsVisible && button.IsEnabled &&
                    button.HitTest(x, y, out id, out focus)) return button;
            }
            return null;
        }

        public bool PreFilterMessage(ref Message message)
        {
            const int LeftDown = 0x0201, LeftDoubleDown = 0x0203, LeftUp = 0x0202, CancelMode = 0x001f, ActivateApp = 0x001c;
            if (message.Msg == CancelMode || (message.Msg == ActivateApp && message.WParam == IntPtr.Zero))
            { pressedId = -1; return false; }
            if (message.Msg != LeftDown && message.Msg != LeftDoubleDown && message.Msg != LeftUp) return false;
            bool captured = pressedId >= 0;
            int id = pressedId, window = pressedWindow;
            IntPtr handle = pressedHandle;
            if (message.Msg == LeftUp) pressedId = -1;
            try
            {
                if (!Eligible()) return message.Msg == LeftUp && captured;
                Control receiver = Control.FromHandle(message.HWnd);
                Form form = receiver == null ? null : receiver.FindForm();
                // An active WinForms configuration/help dialog is not the DirectX renderer.
                if (form == null || form.GetType().FullName != "MediaPortalApp" || form != Form.ActiveForm ||
                    form.ClientSize.Width <= 0 || form.ClientSize.Height <= 0)
                    return message.Msg == LeftUp && captured;
                long packed = message.LParam.ToInt64();
                Point p = form.PointToClient(receiver.PointToScreen(new Point((short)packed, (short)(packed >> 16))));
                int x = (int)Math.Round(p.X * (double)GUIGraphicsContext.Width / form.ClientSize.Width);
                int y = (int)Math.Round(p.Y * (double)GUIGraphicsContext.Height / form.ClientSize.Height);
                GUIWindow bar = GUIWindowManager.GetWindow(3005);
                GUIButtonControl button = Hit(bar, x, y);
                if (message.Msg == LeftDown || message.Msg == LeftDoubleDown)
                {
                    pressedId = -1;
                    if (button == null) return false;
                    pressedId = button.GetID;
                    pressedWindow = GUIWindowManager.ActiveWindow;
                    pressedHandle = message.HWnd;
                    GUIGraphicsContext.ResetLastActivity();
                    // Consume both halves of this gesture, so native Click cannot fire a second command.
                    return true;
                }
                if (!captured) return false;
                if (button != null && button.GetID == id && GUIWindowManager.ActiveWindow == window && message.HWnd == handle)
                {
                    bar.Focused = true;
                    GUIControl.FocusControl(bar.GetID, id);
                    GUIGraphicsContext.ResetLastActivity();
                    int actionId = button.ActionID;
                    button.OnAction(new MPAction(MPAction.ActionType.ACTION_SELECT_ITEM, 0, 0));
                    Log.Debug("Backdrop topbar pointer: activated control {0} on window {1}; action {2}, speed {3}, position {4:0.00}s",
                        id, window, actionId, g_Player.Speed, g_Player.CurrentPosition);
                }
                return true;
            }
            catch (Exception error)
            {
                pressedId = -1;
                Log.Warn("Backdrop topbar pointer failed: {0}", error.Message);
                return captured;
            }
        }
    }
}
