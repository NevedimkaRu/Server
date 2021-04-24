using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RAGE;
using RAGE.Ui;
using RAGE.Elements;
using cs_packages.utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace cs_packages.Interface
{
    class MainMenu : Events.Script
    {
        private static bool isMenuOpen = false;
        MainMenu()
        {
            Input.Bind(VirtualKeys.M, true, ToggleMenu);
            Input.Bind(VirtualKeys.RightButton, true, BackRouter);
            Input.Bind(VirtualKeys.Escape, true, CloseMenu);
            Events.Add("trigger_OpenMenuData", OpenMenuData);
            Events.Add("vui_spawnCar", SpawnCar);
            Events.Add("vui_isMenuOpen", ChangeMenuOpenStatus);
        }

        private void CloseMenu()
        {
            Task.Run(() =>
            {
                Vui.CloseModals();
            }, delayTime: 100);
        }

        public static void ToggleMenu()
        {
            if (Check.GetPlayerStatus(Check.PlayerStatus.OpenChat)) return;
            if (isMenuOpen)
            {
                Vui.CloseModals();
            }
            else
            {
                OpenMenu();
            }
        }

        private void BackRouter()
        {
            if(isMenuOpen)
            {
                Vui.VuiExec("back()");
            }
        }

        public static void OpenMenu()
        {
            //RAGE.Game.Pad.DisableControlAction(2, 202, true);
            Events.CallRemote("remote_PrepareMenuData");
            Chat.Activate(false);
        }

        private void SpawnCar(object[] args)
        {
            int carId = Convert.ToInt32(args[0]);
            Chat.Output("Спавним машину id - " + carId);
            Events.CallRemote("remote_SpawnPlayerCar", carId);
        }

        //Эвент после получения данных открыть меню
        private void OpenMenuData(object[] args)
        {
            Chat.Activate(true);
            Chat.Output(args[0].ToString());
            Vui.VuiModals("openMenu(" + args[0].ToString() + ")");
            //Vui.VuiModals("openMenu()");
        }


        private void ChangeMenuOpenStatus(object[] args)
        {
            bool status = Convert.ToBoolean(args[0]);
            Chat.Output(status.ToString());
            isMenuOpen = status;
            if (isMenuOpen)
            {
                RAGE.Game.Graphics.TransitionToBlurred(0);
                RAGE.Ui.Cursor.Visible = true;
            }
            else
            {
                RAGE.Game.Graphics.TransitionFromBlurred(0);
                RAGE.Ui.Cursor.Visible = false;
            }
        }
    }
}