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

        private Auth() 
        {
            Chat.Activate(false);
            SetCam();
            Vui.VuiModals("openAuthMenu()");
            Events.Add("vui_login", LoginAccount);
            Events.Add("vui_test", vuitest);
            Events.Add("vui_register", RegisterAccount);
            Events.Add("trigger_FinishAuth", FinishAuth);
            Events.Add("trigger_RegisterError", RegisterError);
            Events.Add("trigger_AuthError", AuthError);
            Events.OnPlayerCommand += cmd;
        }

        private void vuitest(object[] args)
        {
            Chat.Output("event works");
        }

        private void AuthError(object[] args)
        {
            Events.CallRemote("vui_AuthError");
            Vui.VuiModals($"AuthMenu.authError('{args[0].ToString()}')");
        }

        private void RegisterError(object[] args)
        {
            Vui.VuiModals($"RegisterMenu.registerError('{args[0].ToString()}')");
        }

        private void FinishAuth(object[] args)
        {
            MainMenu.CloseMenu();
            RemoveCam();
            Chat.Activate(true);
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
                camHandle = Cam.CreateCamWithParams("DEFAULT_SCRIPTED_CAMERA", 2335.206f, 227.16092f, 201.87308f, 0, 0, 140.2711f, 45, true, 0);
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
