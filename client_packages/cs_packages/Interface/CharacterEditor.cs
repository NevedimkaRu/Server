using cs_packages.model;
using cs_packages.player;
using Newtonsoft.Json;
using RAGE;
using RAGE.Game;
using System;
using System.Collections.Generic;
using System.Text;
using static RAGE.Events;

namespace cs_packages.Interface
{
    class CharacterEditor : Events.Script
    {
        static int camHandle;
        public static bool isMenuOpen = false;
        private CharacterEditor()
        {
            Events.Add("vui_isCEMenuOpen", ChangeMenuOpenStatus);
            Events.Add("vui_editCostumization", EditCostumization);
            Events.Add("vui_saveCostumization", SaveCostumization);
        }

        private void SaveCostumization(object[] args)
        {
            CharacterCustomize.OnSaveCharacter();
            CharacterCustomize.UpdateAllModel();
            Vui.CloseModals();
            RemoveCam();
            //-427.4464f, 1116.9916f, 326.76862f    Rot:    -20.288025f
            RAGE.Elements.Player.LocalPlayer.Position = new Vector3(-427.4464f, 1116.9916f, 326.76862f);
            RAGE.Elements.Player.LocalPlayer.SetRotation(0, 0, -20.288025f, 2, true);

        }

        private void EditCostumization(object[] args)
        {
            Customize newModel = JsonConvert.DeserializeObject<Customize>(args[0].ToString());
            CharacterCustomize.UpdatePlayerByModel(newModel);
            CharacterCustomize.model = newModel;
        }

        public static void OpenMenu()
        {
            Vui.VuiModals("openCharacterEditor()");
            CharacterCustomize.SetDefaultModel();
            SetCam();
        }


        private void ChangeMenuOpenStatus(object[] args)
        {
            bool status = Convert.ToBoolean(args[0]);
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
        private static void SetCam()
        {
            RAGE.Elements.Player player = RAGE.Elements.Player.LocalPlayer;

            RAGE.Task.Run(() =>
            {
                //402.55817f, -996.4657f, -99.00027f    Rot:    176.13338
                player.Position = new Vector3(402.55817f, -996.4657f, -99.00027f);
                player.SetRotation(0, 0, 176.13338f, 2, true);
                player.Dimension = (uint)(player.Id + 200);
                player.FreezePosition(true);
                player.SetCollision(false, false);
                player.ForceMotionState(2, true, true, true);
                RAGE.Game.Pad.DisableAllControlActions(1);
                RAGE.Game.Pad.DisableAllControlActions(0);
                //402.65216f, -1000.26294f, -99.00414f    Rot:    -3.4659793f
                //camHandle = Cam.CreateCamWithParams("DEFAULT_SCRIPTED_CAMERA", 402.65216f, -1000.26294f, -99.00414f, 0, 0, -3.4659793f, 35, true, 0);

                //Cam.SetCamActive(camHandle, true);
                //Cam.RenderScriptCams(true, false, 0, true, false, 0);
                Camera.CamRotator.Start(player.Position,player.Position, new Vector3(2.5f, 2.5f, 0.7f), fov: 40);
                Camera.CamRotator.SetZBound(-0.8f, 2.1f);
            }, 100);

        }
        public static void RemoveCam()
        {
            Cam.RenderScriptCams(false, false, 0, true, false, 0);
            RAGE.Elements.Player player = RAGE.Elements.Player.LocalPlayer;
            player.FreezePosition(false);
            player.SetInvincible(false);
            player.SetVisible(true, true);
            player.SetCollision(true, true);
            Camera.CamRotator.Stop();
            //camHandle = 0;
            //Cam.DestroyCam(camHandle, true);
        }

    }
}
