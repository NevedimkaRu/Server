using cs_packages.model;
using cs_packages.vehicle;
using Newtonsoft.Json;
using RAGE;
using RAGE.Elements;
using System;
using System.Collections.Generic;
using System.Text;

namespace cs_packages.Interface
{
    class HandlingEditor : Events.Script
    {

        public static bool isMenuOpen = false;
        private HandlingEditor()
        {
            Events.OnPlayerCommand += cmd;
            Events.Add("vui_saveHandling", SaveHandling);
            Events.Add("vui_isHEMenuOpen", ChangeMenuOpenStatus);
        }

        private void SaveHandling(object[] args)
        {
            Chat.Output(args[0].ToString());
            VehicleHandling model = RAGE.Util.Json.Deserialize<VehicleHandling>(args[0].ToString());
            if (Player.LocalPlayer.Vehicle != null)
            {
                Handling.SetVehicleHandling(Player.LocalPlayer.Vehicle, model);
                Events.CallRemote("remote_SetHandling", model);
            }
            RAGE.Ui.Cursor.Visible = false;
        }

        private void ChangeMenuOpenStatus(object[] args)
        {
            bool status = Convert.ToBoolean(args[0]);
            Chat.Output(status.ToString());
            isMenuOpen = status;
            //Api.Notify("isMenuOpen: " + isMenuOpen.ToString());
            if (isMenuOpen)
            {
                RAGE.Ui.Cursor.Visible = true;
            }
            else
            {
                Chat.Activate(true);
                RAGE.Ui.Cursor.Visible = false;
            }
        }

        private void cmd(string cmd, Events.CancelEventArgs cancel)
        {
            string[] args = cmd.Split(new char[] { ' ' });
            string commandName = args[0].Trim(new char[] { '/' });

            if (commandName == "he")
            {
                Vehicle veh = Player.LocalPlayer.Vehicle;
                if (veh != null)
                {
                    Vui.VuiModals("openHandlingEditor()");
                    Vui.VuiModals($"HandlingEditor.fillData({ veh.GetSharedData("sd_Handling1") });");
                }
            }
        }
    }
}
