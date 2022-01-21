using RAGE;
using RAGE.Elements;
using System;
using System.Collections.Generic;
using System.Text;

namespace cs_packages.Interface
{
    class ClothesStore : Events.Script
    {
        public static bool isMenuOpen = false;
        private static string clothesStoreData = "";
        private ClothesStore() 
        {
            //todo Убрать жту срань
            UnableAFKCam();
            Events.OnPlayerCommand += cmd;
            Events.Add("vui_isCSMenuOpen", ChangeMenuOpenStatus);
            Events.Add("vui_setCloth", SetCloth);
            Events.Add("vui_discardCloth", DiscardCloth);
            Events.Add("vui_buyCloth", BuyCloth);
            Events.Add("vui_loadDownTops", LoadDownTops);
            Events.Add("trigger_buyClothesSuccess", BuySucces);
            Events.Add("trigger_buyClothesError", BuyError);
            Events.Add("trigger_bigPortionClothesData", BigPortionClothesData);
        }


        //todo Убрать жту срань
        private void UnableAFKCam()
        {
            RAGE.Game.Invoker.Invoke(0x9E4CFFF989258472); 
            RAGE.Game.Invoker.Invoke(0xF4F2C0D4EE209E20); 
            //mp.game.invoke('0x9E4CFFF989258472'); // void _INVALIDATE_VEHICLE_IDLE_CAM();
            //mp.game.invoke('0xF4F2C0D4EE209E20'); // void INVALIDATE_IDLE_CAM();
            Task.Run(UnableAFKCam, delayTime: 25000);
        }

        private void BigPortionClothesData(object[] args)
        {
            clothesStoreData += args[0].ToString();
            Chat.Output("ADIN");
        }

        private void BuyError(object[] args)
        {
            Vui.VuiModals($"ClothesStore.buyError('{ args[0]}');");
        }

        private void BuySucces(object[] args)
        {
            Vui.VuiModals("ClothesStore.buySuccess();");
            Api.Notify("Вы купили одежду");
        }

        private void BuyCloth(object[] args)
        {
            Events.CallRemote("remote_BuyCloth");
        }

        private void DiscardCloth(object[] args)
        {
            Events.CallRemote("remote_DiscardCloth");
        }

        private async void LoadDownTops(object[] args)
        {
            string data = (string)await RAGE.Events.CallRemoteProc("remote_GetDownTops");
            Vui.VuiModals($"ClothesStore.setDownTops({data})");
        }

        private void SetCloth(object[] args)
        {
            
            int clothType = Convert.ToInt32(args[0]);
            int clothId = Convert.ToInt32(args[1]);
            int texture = Convert.ToInt32(args[2]);
            Events.CallRemote("remote_SetCloth", clothType, clothId, texture);
            //bool res = (bool) await Events.CallRemoteProc("remote_SetCloth", clothType, clothId, texture);
            //if (!res) 
            //{
            //    Vui.Notify("Одежда несовместима с текущей");
            //}
        }

        public static void OpenCSMenu()
        {
            Vui.VuiModals("openClothesStoreMenu()");
            Vui.VuiModals($"ClothesStore.fillData({ clothesStoreData });");
            Chat.Output(clothesStoreData.Substring(0, 200));
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
                //CurrMods.Clear();
                RemoveCam();
                Events.CallRemote("remote_CloseClothesStore");
            }
        }

        private static void SetCam()
        {
            Camera.CamRotator.Start(Player.LocalPlayer.Position, Player.LocalPlayer.Position, new Vector3(2.5f, 2.5f, 0.7f), fov: 40);
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

            if (commandName == "cs")
            {
                OpenCSMenu();
            }
        }
    }
}
