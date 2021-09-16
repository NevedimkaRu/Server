using System;
using System.Collections.Generic;
using System.Text;
using RAGE;
using RAGE.Game;
using RAGE.Elements;
using RAGE.Ui;

namespace cs_packages.admin
{
    public class NoClip : Events.Script
    {
        enum ControlKeys
        {
            F2 = 0x71,
            Q = 69,
            E = 81,
            LCtrl = 17,
            Shift = 16
        }
        bool isNoclip = false;
        int camHandle;
        public NoClip()
        {
            Events.OnPlayerCommand += Cmd;
            //Input.Bind(RAGE.Ui.VirtualKeys.X, true, CameraTest);
            Input.Bind((int)ControlKeys.F2, true, () => {
                isNoclip = !isNoclip;
                if (isNoclip) StartNoclip();
                else StopNoclip();
            });
            Events.Add("trigger_SetVelocity", SetVelocity);
            Events.Add("trigger_pointCamAtVeh", PointCamAtVeh);
        }

        private void SetVelocity(object[] args)
        {
            RAGE.Elements.Player.LocalPlayer.Vehicle.SetVelocity(Convert.ToSingle(args[0]), Convert.ToSingle(args[1]), 0);
        }

        private void PointCamAtVeh(object[] args)
        {
            Chat.Output("Дошёл");
            Chat.Output(args[0].ToString());
            RAGE.Elements.Vehicle v = (RAGE.Elements.Vehicle) args[0];
            
            PointCamAtVeh(v.Handle);
        }

        private void Cmd(string cmd, Events.CancelEventArgs cancel)
        {
            string[] args = cmd.Split(new char[] { ' ' });
            string commandName = args[0].Trim(new char[] { '/' });

            if (commandName == "savecam")
            {
                if (camHandle == 0) return;
                Vector3 rot = Cam.GetCamRot(camHandle, 2);
                Vector3 pos = Cam.GetCamCoord(camHandle);
                Events.CallRemote("SaveCam", Convert.ToString(pos), Convert.ToString(rot));
            }
            if (commandName == "fl")
            {
                RAGE.Game.Interior.EnableInteriorProp(252673, "basic_style_set");
                RAGE.Game.Interior.RefreshInterior(252673);
            }
            if (commandName == "int")
            {
                Vector3 pos = RAGE.Elements.Player.LocalPlayer.Position;
                Chat.Output(Interior.GetInteriorAtCoords(pos.X, pos.Y, pos.Z).ToString());
            }
            /*if (commandName == "switch")
            {
                utils.Utils.SmoothTeleport(new Vector3(0, 0, 0),0, 2000);
            }
            if (commandName == "switchs")
            {
                RAGE.Game.Streaming.StopPlayerSwitch();
            }
            if (commandName == "go")
            {
                //RAGE.Elements.Player.LocalPlayer.Vehicle.
                //RAGE.Elements.Player.LocalPlayer.TaskVehicleGotoNavmesh(RAGE.Elements.Player.LocalPlayer.Vehicle.Handle, 0, 0, 10, 30, 0, 10f);
                RAGE.Elements.Player.LocalPlayer.TaskVehicleDriveWander(RAGE.Elements.Player.LocalPlayer.Vehicle.Handle, 30, 0);
            }
            if (commandName == "gos")
            {
                RAGE.Elements.Player.LocalPlayer.ClearTasks();
            }*/
            if (commandName == "setcam") 
            {
                Events.Tick -= CamRender;
                Cam.SetCamCoord(camHandle, -2036.722f, -366.7631f, 48.32808f);
                Cam.SetCamRot(camHandle, -13.28009f, 0f, 144.4374f, 2);
            }
        }

        private void CameraTest()
        {
            Vector3 playerPos = RAGE.Elements.Player.LocalPlayer.Position;
            Vector3 camRot = Cam.GetGameplayCamRot(2);
            int cumHandle = Cam.CreateCamWithParams("DEFAULT_SCRIPTED_CAMERA", playerPos.X, playerPos.Y, playerPos.Z, camRot.X, camRot.Y, camRot.Z, 45, true, 0);
            Cam.SetCamActive(cumHandle, true);
            Cam.RenderScriptCams(true, false, 0, true, false, 0);
            RAGE.Task.Run(() => 
            {
                int cumHandle2 = Cam.CreateCamWithParams("DEFAULT_SCRIPTED_CAMERA", playerPos.X + 100, playerPos.Y, playerPos.Z, camRot.X, camRot.Y, 228, 45, true, 0);
                Cam.SetCamActiveWithInterp(cumHandle2, cumHandle, 3000, 1, 1);
            },delayTime: 500);
        }

        public void StartNoclip()
        {
            Api.Notify("NoClip ~g~активирован");
            Vector3 playerPos = RAGE.Elements.Player.LocalPlayer.Position;
            Vector3 camRot = Cam.GetGameplayCamRot(2);
            camHandle = Cam.CreateCamWithParams("DEFAULT_SCRIPTED_CAMERA", playerPos.X, playerPos.Y, playerPos.Z, camRot.X, camRot.Y,camRot.Z, 45, true, 0);
            Cam.SetCamActive(camHandle, true);
            Cam.RenderScriptCams(true, false, 0, true, false,0);
            RAGE.Elements.Player player = RAGE.Elements.Player.LocalPlayer;
            player.FreezePosition(true);
            player.SetInvincible(true);
            player.SetVisible(false, false);
            player.SetCollision(false, false);
            Events.Tick += CamRender;
        }

        public void StopNoclip()
        {
            Api.Notify("NoClip ~r~деактивирован");
            Cam.RenderScriptCams(false, false, 0, true, false, 0);
            RAGE.Elements.Player player = RAGE.Elements.Player.LocalPlayer;
            player.FreezePosition(false);
            player.SetInvincible(false);
            player.SetVisible(true, true);
            player.SetCollision(true, true);
            camHandle = 0;
            Cam.DestroyCam(camHandle,true);
            Events.Tick -= CamRender;
        }
        public void CamRender(List<Events.TickNametagData> nametags)
        {
            if (camHandle == 0 || utils.Check.GetPlayerStatus(utils.Check.PlayerStatus.OpenChat)) return;
            float rightAxisX = Pad.GetDisabledControlNormal(0, 220);
            float rightAxisY = Pad.GetDisabledControlNormal(0, 221);
            float leftAxisX = Pad.GetDisabledControlNormal(0, (int)Control.ScriptLeftAxisX);//left/right
            float leftAxisY = Pad.GetDisabledControlNormal(0, (int)Control.ScriptLeftAxisY);//up/down
            Vector3 rot = Cam.GetCamRot(camHandle,2);
            Vector3 pos = Cam.GetCamCoord(camHandle);
            Vector3 rr = utils.Utils.GetDirectionByRotation(Cam.GetCamRot(camHandle,2));

            float fastMult = 1;
            float slowMult = 1;
            float superSlowMult = 1;
            if (Input.IsDown((int)ControlKeys.Shift))
            {
                fastMult = 3;
            }
            else if (Input.IsDown((int)ControlKeys.LCtrl))
            {
                slowMult = 0.1f;
            }
            else if(Input.IsDown(VirtualKeys.Menu))
            {
                superSlowMult = 0.01f;
            }

            Vector3 vector = new Vector3(0, 0, 0);
            vector.X = rr.X * leftAxisY * fastMult * slowMult * superSlowMult;
            vector.Y = rr.Y * leftAxisY * fastMult * slowMult * superSlowMult;
            vector.Z = rr.Z * leftAxisY * fastMult * slowMult * superSlowMult;
            Vector3 upVector = new Vector3(0, 0, 1);
            Vector3 rightVector = utils.Utils.GetCrossProduct(
              utils.Utils.GetNormalizedVector(rr),
              utils.Utils.GetNormalizedVector(upVector)
            );

            RAGE.Elements.Player.LocalPlayer.Position = new Vector3(
              pos.X + vector.X + 1,
              pos.Y + vector.Y + 1,
              pos.Z + vector.Z + 1
            );
            RAGE.Elements.Player.LocalPlayer.SetRotation(rot.X, 0, rot.Z, 2, true);

            rightVector.X *= leftAxisX * fastMult * slowMult * superSlowMult;
            rightVector.Y *= leftAxisX * fastMult * slowMult * superSlowMult;
            rightVector.Z *= leftAxisX * fastMult * slowMult * superSlowMult;

            float upMovement = 0.0f;
            if (Input.IsDown((int)ControlKeys.Q))
            {
                upMovement = 0.5f;
            }
            float downMovement = 0.0f;
            if (Input.IsDown((int)ControlKeys.E))
            {
                downMovement = 0.5f;
            }

            Cam.SetCamCoord(
                camHandle,
                pos.X - vector.X + rightVector.X,
                pos.Y - vector.Y + rightVector.Y,
                pos.Z - vector.Z + (rightVector.Z + upMovement - downMovement) * fastMult * slowMult * superSlowMult
            );
            Cam.SetCamRot(
                camHandle,
                rot.X + rightAxisY * -5.0f,
                0.0f,
                rot.Z + rightAxisX * -5.0f,
                2
            );
        }

        private void PointCamAtVeh(int veh)
        {
            Chat.Output(veh.ToString());
            //Events.Tick -= CamRender;
            Cam.PointCamAtEntity(camHandle, veh, 0.1f, 0.1f, 0.1f, true);
            //Cam.AttachCamToEntity(camHandle, veh, 2f, 3f, 0.5f, true);
            Chat.Output("Норм");
        }
    }
}
