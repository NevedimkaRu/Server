using RAGE;
using System;
using System.Collections.Generic;
using System.Text;

namespace cs_packages.Interface
{
    public class PhotoEditor : Events.Script
    {
        public static bool isMenuOpen = false;
        public PhotoEditor()
        {
            Input.Bind(RAGE.Ui.VirtualKeys.RightControl, true, () =>
            {
                RAGE.Ui.Cursor.Visible = !RAGE.Ui.Cursor.Visible;
            });
            Events.Add("vui_isPhotoModeMenuOpen", ChangeMenuOpenStatus);
            Input.Bind(RAGE.Ui.VirtualKeys.O, true, () =>
            {
                RAGE.Game.Misc.SetTimeScale(0);
            });
            Events.OnPlayerCommand += cmd;
        }

        private void ChangeMenuOpenStatus(object[] args)
        {
            isMenuOpen = Convert.ToBoolean(args[0]);

            if (isMenuOpen)
            {
                RAGE.Ui.Cursor.Visible = true;

            }
            else
            {
                RAGE.Ui.Cursor.Visible = false;

            }
        }

        public static void OpenMenu()
        {
                
            Vui.VuiModals("openPhotoModeMenu()");
        }

        public static void CloseMenu()
        {
            if (isMenuOpen) 
            {
                Vui.CloseModals();
            }
        }
        private void cmd(string cmd, Events.CancelEventArgs cancel)
        {
            string[] args = cmd.Split(new char[] { ' ' });
            string commandName = args[0].Trim(new char[] { '/' });

            if (commandName == "pm")
            {
                OpenMenu();
            }
            if (commandName == "maxpane")
            {
                RAGE.Game.Misc.SetTimeScale(Convert.ToSingle(args[1]));
            }
            if (commandName == "maxpane1")
            {
                RAGE.Game.Misc.SetThisScriptCanBePaused(true);
                RAGE.Game.Misc.SetGamePaused(true);
            }
            if (commandName == "maxpane2")
            {
                RAGE.Game.Misc.SetGamePaused(false);
            }

        }
    }
}
