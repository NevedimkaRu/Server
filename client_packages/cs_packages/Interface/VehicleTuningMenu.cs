using RAGE;
using RAGE.Elements;
using System;
using System.Collections.Generic;
using System.Text;

namespace cs_packages.Interface
{
    class VehicleTuningMenu : Events.Script
    {
        private static Dictionary<int, int> CurrMods = new Dictionary<int, int>();
        public static bool isMenuOpen = false;

        int CurComponentId;
        int CurItemId;

        private VehicleTuningMenu() 
        {
            Events.OnPlayerCommand += cmd;
            Events.Add("vui_isVTMenuOpen", ChangeMenuOpenStatus);
            Events.Add("vui_setTuning", SetTuning);
            Events.Add("vui_discardTuning", DiscardTuning);
            Events.Add("vui_buyTuning", BuyTuning);
            Events.Add("trigger_buyTuningSuccess", BuySuccess);
            Events.Add("trigger_buyTuningError", BuyError);
        }

        private void BuyError(object[] args)
        {
            string ErrorText = args[0].ToString();
            Api.Notify(ErrorText);
            Vui.VuiModals($"VehicleTuning.buyError('{ErrorText}')");
        }

        private void BuySuccess(object[] args)
        {
            Api.Notify("Покупка успешна");
            Vui.VuiModals("VehicleTuning.buySuccess()");
            CurrMods[CurComponentId] = CurItemId;
        }

        public static async void OpenVTMenu()
        {
            if (Player.LocalPlayer.Vehicle == null) return;
            var json = (string) await Events.CallRemoteProc("remote_GetVehTuningStoreData");
            if (json.Length > 0)
            {
                Vui.VuiModals("openVehicleTuningMenu()");
                Vui.VuiModals($"VehicleTuning.fillData({ json });");
                SetCurrentMods();

            }
        }

        private void BuyTuning(object[] args)
        {
            CurComponentId = Convert.ToInt32(args[0]);
            CurItemId = Convert.ToInt32(args[1]);
            Events.CallRemote("remote_BuyTuning", CurComponentId, CurItemId);
        }

        private void DiscardTuning(object[] args)
        {
            int ComponentId = Convert.ToInt32(args[0]);
            int ItemId = Convert.ToInt32(args[1]);
            Vehicle veh = Player.LocalPlayer.Vehicle;
            if (veh != null)
            {
                veh.SetMod(ComponentId, CurrMods[ComponentId], true);
            }
        }

        private void SetTuning(object[] args)
        {
            int ComponentId = Convert.ToInt32(args[0]);
            int ItemId = Convert.ToInt32(args[1]);
            Vehicle veh = Player.LocalPlayer.Vehicle;
            if (veh != null)
            {
                veh.SetMod(ComponentId, ItemId, false);
            }
        }

        private static void SetCurrentMods()
        {
            Vehicle veh = Player.LocalPlayer.Vehicle;
            if (veh != null) 
            { 
                for(int i = 0; i < 49; i++)
                {
                    int itemId = veh.GetMod(i);
                    CurrMods.Add(i, itemId);
                }
            }
        }

        private void ChangeMenuOpenStatus(object[] args)
        {
            isMenuOpen = Convert.ToBoolean(args[0]);

            if (isMenuOpen)
            {
                RAGE.Ui.Cursor.Visible = true;
                SetCam();
            }
            else
            {
                Chat.Activate(true);
                RAGE.Ui.Cursor.Visible = false;
                CurrMods.Clear();
                RemoveCam();
            }
        }

        private static void SetCam()
        {
            Camera.CamRotator.Start(Player.LocalPlayer.Position, Player.LocalPlayer.Position, new Vector3(2.5f, 2.5f, 1.3f), fov: 60);
            Camera.CamRotator.SetZBound(-0.8f, 2.1f);
        }

        private static void RemoveCam()
        {
            Camera.CamRotator.Stop();
        }

        private void cmd(string cmd, Events.CancelEventArgs cancel)
        {
            string[] args = cmd.Split(new char[] { ' ' });
            string commandName = args[0].Trim(new char[] { '/' });

            if (commandName == "vt")
            {
                OpenVTMenu();
            }
        }
    }
}
