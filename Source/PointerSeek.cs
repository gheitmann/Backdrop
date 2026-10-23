using System;
using System.Drawing;
using System.Windows.Forms;
using MediaPortal.GUI.Library;
using MediaPortal.Player;

namespace MediaPortal.Plugins
{
    // Required progress control 118 remains a GUIProgressControl. Input is handled
    // by the existing process plugin, without replacing the native bound type.
    internal static class PointerSeek
    {
        private static DateTime lastSeek = DateTime.MinValue;
        private static SeekFilter filter;

        internal static void Install()
        {
            if (filter != null) return;
            filter = new SeekFilter();
            Application.AddMessageFilter(filter);
        }

        internal static void Uninstall()
        {
            if (filter == null) return;
            Application.RemoveMessageFilter(filter);
            filter = null;
        }

        internal static bool TryGetFraction(Rectangle bounds, int x, int y, out double fraction)
        {
            fraction = 0;
            if (bounds.Width <= 0 || bounds.Height <= 0 ||
                x < bounds.Left || x > bounds.Right ||
                y < bounds.Top - 8 || y > bounds.Bottom + 8)
                return false;
            fraction = Math.Max(0, Math.Min(1, (double)(x - bounds.Left) / bounds.Width));
            return true;
        }

        private static bool TryGetProgress(out GUIProgressControl progress, out double duration)
        {
            progress = null;
            duration = 0;
            if (GUIWindowManager.ActiveWindow != 510 || GUIWindowManager.RoutedWindow != -1 ||
                !string.Equals(GUIGraphicsContext.SkinName, "Backdrop", StringComparison.OrdinalIgnoreCase) ||
                !g_Player.Playing || !g_Player.IsMusic || g_Player.IsRadio || !g_Player.CanSeek)
                return false;

            duration = g_Player.Duration;
            if (double.IsNaN(duration) || double.IsInfinity(duration) || duration <= 0)
                return false;
            GUIWindow window = GUIWindowManager.GetWindow(510);
            progress = window == null ? null : window.GetControl(118) as GUIProgressControl;
            return progress != null && progress.IsVisible;
        }

        private static void Seek(double fraction, double duration, bool finalClick)
        {
            DateTime now = DateTime.UtcNow;
            if (!finalClick && (now - lastSeek).TotalMilliseconds < 200)
                return;
            lastSeek = now;
            // Avoid ending the track merely by dropping the pointer on the edge.
            double target = Math.Min(duration - Math.Min(0.25, duration / 2), duration * fraction);
            try
            {
                g_Player.SeekAbsolute(target);
                Log.Debug("Backdrop pointer seek: {0:0.00}s of {1:0.00}s ({2})", target, duration,
                          finalClick ? "click" : "drag");
            }
            catch (Exception error)
            {
                Log.Warn("Backdrop pointer seek failed: {0}", error.Message);
            }
        }

        // The MP action stream does not reliably preserve the held button during
        // a drag. Observe the application's own WinForms messages on its UI thread.
        private sealed class SeekFilter : IMessageFilter
        {
            private bool dragging;

            public bool PreFilterMessage(ref Message message)
            {
                try { return HandleMessage(ref message); }
                catch (Exception error)
                {
                    dragging = false;
                    Log.Warn("Backdrop pointer input failed: {0}", error.Message);
                    return false;
                }
            }

            private bool HandleMessage(ref Message message)
            {
                const int MouseMove = 0x0200, LeftDown = 0x0201, LeftUp = 0x0202;
                if (message.Msg != MouseMove && message.Msg != LeftDown && message.Msg != LeftUp)
                    return false;
                if (message.Msg == MouseMove && (!dragging || (message.WParam.ToInt64() & 1) == 0))
                {
                    dragging = false;
                    return false;
                }
                GUIProgressControl progress;
                double duration;
                if (!TryGetProgress(out progress, out duration))
                {
                    dragging = false;
                    return false;
                }
                Control receiver = Control.FromHandle(message.HWnd);
                Form form = receiver == null ? null : receiver.FindForm();
                if (form == null || form != Form.ActiveForm || form.ClientSize.Width <= 0 || form.ClientSize.Height <= 0)
                {
                    dragging = false;
                    return false;
                }
                long packed = message.LParam.ToInt64();
                Point client = form.PointToClient(receiver.PointToScreen(new Point((short)packed, (short)(packed >> 16))));
                int x = (int)Math.Round(client.X * (double)GUIGraphicsContext.Width / form.ClientSize.Width);
                int y = (int)Math.Round(client.Y * (double)GUIGraphicsContext.Height / form.ClientSize.Height);
                double fraction;
                bool inside = TryGetFraction(new Rectangle(progress.XPosition, progress.YPosition, progress.Width, progress.Height), x, y, out fraction);
                if (message.Msg == LeftDown)
                    dragging = inside;
                bool seek = inside && dragging;
                if (message.Msg == LeftUp)
                    dragging = false;
                if (seek)
                {
                    GUIGraphicsContext.ResetLastActivity();
                    Seek(fraction, duration, message.Msg == LeftUp);
                }
                return false;
            }
        }
    }
}
