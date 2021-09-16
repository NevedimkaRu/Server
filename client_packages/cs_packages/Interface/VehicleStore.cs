using RAGE;
using RAGE.Game;
using System;
using System.Collections.Generic;
using System.Text;

namespace cs_packages.Interface
{
    class VehicleStore : Events.Script
    {
        private static int selectedVSid;

        public static bool isMenuOpen = false;
        static int camHandle;
        //-48.43142f, -1101.4808f, 26.422335f
        private static Vector3 PlayerPosNearCam = new Vector3(-48.43142f, -1101.4808f, 26.422335f);
        private static Vector3 SelectCarPosition = new Vector3(-41.484047f, -1096.5426f, 25.741745f);
        RAGE.Elements.Vehicle SelectedVeh;

        private VehicleStore()
        {
            Events.OnPlayerCommand += cmd;
            Events.Add("vui_isVSMenuOpen", ChangeMenuOpenStatus);
            Events.Add("vui_selectVehicle", SelectVehicle);
            Events.Add("vui_buyVehicle", BuyVehicle);
            Events.Add("vui_selectGarage", SelectGarage);
            Events.Add("trigger_ErrorVehicleStore", ErrorVehicleStore);
            Events.Add("trigger_SendGaragesVS", SendGaragesVS);

        }

        private void SendGaragesVS(object[] args)
        {
            string data = args[0].ToString();
            Vui.VuiModals($"VehicleStore.selectGarage({data})");
        }

        private void ErrorVehicleStore(object[] args)
        {
            string error = args[0].ToString();
            Vui.VuiModals($"VehicleStore.buyError('{error}')");
        }

        private void SelectGarage(object[] args)
        {
            Chat.Output("Селект гараж");
            int garageId = Convert.ToInt32(args[0]);
            Events.CallRemote("remote_BuyVehicle", selectedVSid, garageId);
            Vui.CloseModals();
        }

        // после покупки машины ещё следует выбор гаража, так что это не последний этап покупки машины
        private void BuyVehicle(object[] args)
        {
            selectedVSid = Convert.ToInt32(args[0]);
            Events.CallRemote("remote_getFreeGarages", selectedVSid);
        }

        private void SelectVehicle(object[] args)
        {
            Api.Notify("Приходит");
            string hash = args[0].ToString();
            Api.Notify("Выбрана " + hash);


            RAGE.Task.Run(() =>
            {
                if (SelectedVeh != null)
                {
                    SelectedVeh.Destroy();
                }
                SelectedVeh = new RAGE.Elements.Vehicle(RAGE.Util.Joaat.Hash(hash), SelectCarPosition, 97.280594f);
            });
            
        }


        public static async void OpenVSMenu()
        {
            var json = (string)await Events.CallRemoteProc("remote_GetVehStoreData");
            if (json.Length > 0)
            {
                Vui.VuiModals("openVehicleStoreMenu()");
                Vui.VuiModals($"VehicleStore.fillData({ json });");
            }
        }

        private void ChangeMenuOpenStatus(object[] args)
        {
            isMenuOpen = Convert.ToBoolean(args[0]);
            Chat.Output("Должен быть крусор");
            Chat.Output(isMenuOpen.ToString());

            if (isMenuOpen)
            {
                RAGE.Ui.Cursor.Visible = true;
                SetCam();
            }
            else
            {
                Chat.Activate(true);
                RAGE.Ui.Cursor.Visible = false;
                RemoveCam();
                if (SelectedVeh != null)
                {
                    SelectedVeh.Destroy();
                }
            }
        }

        private static void SetCam()
        {
            RAGE.Elements.Player player = RAGE.Elements.Player.LocalPlayer;

            //402.55817f, -996.4657f, -99.00027f    Rot:    176.13338
            player.Position = PlayerPosNearCam;
            player.Dimension = (uint)(player.Id + 200);
            Camera.CamRotator.Start(SelectCarPosition, SelectCarPosition, new Vector3(2.5f, 2.5f, 1.3f), fov: 60, heading: 240f);
            Camera.CamRotator.SetZBound(-0.8f, 2.1f);
            //player.FreezePosition(false);
            //player.SetInvincible(false);
            //player.SetVisible(true, true);
            //player.SetCollision(true, true);
            ////402.65216f, -1000.26294f, -99.00414f    Rot:    -3.4659793f
            //camHandle = Cam.CreateCamWithParams("DEFAULT_SCRIPTED_CAMERA", -46.448814f, -1100.5309f, 26.422335f, 0, 0, -48.993202f, 45, true, 0);

            //Cam.SetCamActive(camHandle, true);
            //Cam.RenderScriptCams(true, false, 0, true, false, 0);

        }

        public static void RemoveCam()
        {
            Camera.CamRotator.Stop();
            //Cam.RenderScriptCams(false, false, 0, true, false, 0);
            //RAGE.Elements.Player player = RAGE.Elements.Player.LocalPlayer;
            //player.FreezePosition(false);
            //player.SetInvincible(false);
            //player.SetVisible(true, true);
            //player.SetCollision(true, true);
            //camHandle = 0;
            //Cam.DestroyCam(camHandle, true);
        }

        private void cmd(string cmd, Events.CancelEventArgs cancel)
        {
            string[] args = cmd.Split(new char[] { ' ' });
            string commandName = args[0].Trim(new char[] { '/' });

            if (commandName == "vs")
            {
                OpenVSMenu();
            }
        }
    }
}
