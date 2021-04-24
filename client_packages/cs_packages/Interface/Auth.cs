using RAGE;
using RAGE.Game;
using System;
using System.Collections.Generic;
using System.Text;

namespace cs_packages.Interface
{
    class Auth : Events.Script
    {
        int camHandle;
        public bool isMenuOpen = false;

        private Auth() 
        {
            Chat.Activate(false);
            Vui.VuiModals("openAuthMenu()");
            Events.Add("vui_isAuthMenuOpen", ChangeMenuOpenStatus);
            Events.Add("vui_login", LoginAccount);
            Events.Add("vui_register", RegisterAccount);
            Events.Add("trigger_FinishAuth", FinishAuth);
            Events.Add("trigger_FinishRegister", FinishRegister);
            Events.Add("trigger_RegisterError", RegisterError);
            Events.Add("trigger_AuthError", AuthError);
            Events.OnPlayerCommand += cmd;
            Events.OnPlayerReady += SetCam;
        }

        private void FinishRegister(object[] args)
        {
            RemoveCam();
            CharacterEditor.OpenMenu();
        }

        private void AuthError(object[] args)
        {
            Events.CallRemote("vui_AuthError");
            Vui.VuiModals($"AuthMenu.authError('{args[0]}')");
        }

        private void RegisterError(object[] args)
        {
            Vui.VuiModals($"RegisterMenu.registerError('{args[0]}')");
        }

        private void FinishAuth(object[] args)
        {
            Vui.CloseModals();
            RemoveCam();
            RAGE.Elements.Player.LocalPlayer.Position = new Vector3(-427.4464f, 1116.9916f, 326.76862f);
            RAGE.Elements.Player.LocalPlayer.SetRotation(0, 0, -20.288025f, 2, true);
        }

        private void RegisterAccount(object[] args)
        {
            string login = args[0].ToString();
            string password = args[1].ToString();
            string characterName = args[2].ToString();
            Events.CallRemote("remote_register", login, password, characterName);
        }

        private void LoginAccount(object[] args)
        {
            string login = args[0].ToString();
            string password = args[1].ToString();
            Events.CallRemote("remote_login", login, password);
        }

        private void ChangeMenuOpenStatus(object[] args)
        {
            bool status = Convert.ToBoolean(args[0]);
            Chat.Output(status.ToString());
            isMenuOpen = status;
            if (isMenuOpen)
            {
                RAGE.Ui.Cursor.Visible = true;
            }
            else
            {
                RAGE.Ui.Cursor.Visible = false;
            }
        }

        private void SetCam() 
        {
            RAGE.Elements.Player player = RAGE.Elements.Player.LocalPlayer;

            RAGE.Task.Run(() =>
            {
                player.Position = new Vector3(2335.206f, 227.16092f, 201.87308f);

                player.FreezePosition(true);
                player.SetInvincible(true);
                player.SetVisible(false, false);
                player.SetCollision(false, false);
                //2338.4814f, 238.27805f, 195.79007f    Rot:    -26.315529f
                camHandle = Cam.CreateCamWithParams("DEFAULT_SCRIPTED_CAMERA", 2338.4814f, 238.27805f, 210.02042f, 0, 0, -26.315529f, 45, true, 0);
                Cam.SetCamActive(camHandle, true);
                Cam.RenderScriptCams(true, false, 0, true, false, 0);
            }, 100);

        }

        private void RemoveCam() 
        {
            Cam.RenderScriptCams(false, false, 0, true, false, 0);
            RAGE.Elements.Player player = RAGE.Elements.Player.LocalPlayer;
            player.FreezePosition(false);
            player.SetInvincible(false);
            player.SetVisible(true, true);
            player.SetCollision(true, true);
            camHandle = 0;
            Cam.DestroyCam(camHandle, true);
        }

        private void cmd(string cmd, Events.CancelEventArgs cancel) {
            string[] args = cmd.Split(new char[] { ' ' });
            string commandName = args[0].Trim(new char[] { '/' });

            if (commandName == "fix")
            {
                RAGE.Elements.Player player = RAGE.Elements.Player.LocalPlayer;
                player.FreezePosition(false);
                player.SetInvincible(false);
                player.SetVisible(true, true);
                player.SetCollision(true, true);
            }
        }
    }
}
