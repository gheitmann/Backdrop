using System;
using System.Collections.Generic;
using System.Reflection;
using MediaPortal.GUI.Library;

namespace MediaPortal.Plugins
{
    internal static class PluginMenuLayout
    {
        private static System.Windows.Forms.Timer timer;
        private static GUIButtonControl lastFirstButton;

        internal static void Install()
        {
            if (timer != null) return;
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 200;
            timer.Tick += delegate { Apply(GUIWindowManager.ActiveWindow); };
            timer.Start();
        }

        internal static void Uninstall()
        {
            if (timer != null) { timer.Stop(); timer.Dispose(); timer = null; }
            lastFirstButton = null;
        }

        // The native home menu repeats its entries to fill numberOfButtons.
        // Preserve that control and its plugin routing, but clip to one cycle.
        internal static void Apply(int windowId)
        {
            if ((windowId != 0 && windowId != 34) || !String.Equals(GUIGraphicsContext.SkinName, "Backdrop", StringComparison.OrdinalIgnoreCase)) return;
            try
            {
                GUIWindow window = GUIWindowManager.GetWindow(windowId);
                GUIMenuControl menu = window == null ? null : window.GetControl(50) as GUIMenuControl;
                if (menu == null || menu.ButtonInfos.Count == 0) return;
                int rows = Math.Min(11, menu.ButtonInfos.Count);
                Type type = typeof(GUIMenuControl);
                BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
                FieldInfo rowCount = type.GetField("_numberOfButtons", flags);
                FieldInfo focus = type.GetField("_focusPosition", flags);
                FieldInfo buttons = type.GetField("_buttonList", flags);
                FieldInfo height = type.GetField("_buttonHeight", flags);
                FieldInfo spacing = type.GetField("_spaceBetweenButtons", flags);
                FieldInfo offset = type.GetField("_buttonOffset", flags);
                FieldInfo selectedGap = type.GetField("_spaceAfterSelected", flags);
                if (rowCount == null || focus == null || buttons == null || height == null || spacing == null || offset == null || selectedGap == null)
                    throw new MissingFieldException("Unsupported native plugin menu layout.");
                var generated = (List<GUIButtonControl>)buttons.GetValue(menu);
                if ((int)rowCount.GetValue(menu) == rows && generated.Count > 0 && Object.ReferenceEquals(lastFirstButton, generated[0])) return;
                if ((int)rowCount.GetValue(menu) != rows)
                {
                    rowCount.SetValue(menu, rows);
                    focus.SetValue(menu, rows / 2 + 1);
                    menu.Height = ((int)height.GetValue(menu) + (int)spacing.GetValue(menu)) * rows
                        + 2 * (int)offset.GetValue(menu) + (int)selectedGap.GetValue(menu);
                    menu.AllocResources();
                    menu.FocusedButton = rows / 2 + 1;
                }
                foreach (GUIButtonControl button in (List<GUIButtonControl>)buttons.GetValue(menu))
                    button.TextVAlignment = GUIControl.VAlignment.ALIGN_MIDDLE;
                generated = (List<GUIButtonControl>)buttons.GetValue(menu);
                lastFirstButton = generated.Count == 0 ? null : generated[0];
                Log.Debug("Backdrop plugin menu: {0} entries, {1} visible rows", menu.ButtonInfos.Count, rows);
            }
            catch (Exception error)
            {
                Log.Warn("Backdrop plugin menu layout failed: {0}", error.Message);
            }
        }
    }
}
