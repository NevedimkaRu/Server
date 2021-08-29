using cs_packages.Camera;
using RAGE;
using RAGE.Elements;
using System;
using System.Collections.Generic;
using System.Text;

namespace cs_packages.utils
{
    class AdUtils : Events.Script
    {

        private AdUtils()
        {
            Events.OnPlayerCommand += cmd;
        }

        public static void SetRotateCam()
        {
            Vector3 pos = Player.LocalPlayer.Position;
            Vector3 offset = new Vector3(2.5f, 2.5f, 1.3f);
            if (Player.LocalPlayer.Vehicle != null) 
            {
                pos = Player.LocalPlayer.Vehicle.Position;
                offset = new Vector3(3.5f, 3.5f, 1.3f);
            }
            Camera.CamRotator.Start(pos, pos, offset, fov: 60);
            Camera.CamRotator.SetZBound(-0.8f, 2.1f);
        }

        public static void StartRotateCam()
        {

            Events.Tick += onTck;
        }

        private static void onTck(List<Events.TickNametagData> nametags)
        {
            CamRotator.OnMouseMove(0.01f, 0f);
        }

    private void cmd(string cmd, Events.CancelEventArgs cancel)
        {
            string[] args = cmd.Split(new char[] { ' ' });
            string commandName = args[0].Trim(new char[] { '/' });

            if (commandName == "scr1")
            {
                Chat.Output("setCamRotator");
                SetRotateCam();

            }
            if (commandName == "scr2")
            {
                Chat.Output("startCamRotator");
                StartRotateCam();

            }
            if (commandName == "scr3")
            {
                CamRotator.Stop();
                Events.Tick -= onTck;

            }
        }

    }
}
