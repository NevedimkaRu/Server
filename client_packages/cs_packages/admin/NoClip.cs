using System;
using System.Collections.Generic;
using System.Text;
using RAGE;
using RAGE.Game;
using RAGE.Elements;

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
            Input.Bind((int)ControlKeys.F2, true, () => {
                isNoclip = !isNoclip;
                if (isNoclip) StartNoclip();
                else StopNoclip();
            });
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
            if (camHandle == 0) return;
            float rightAxisX = Pad.GetDisabledControlNormal(0, 220);
            float rightAxisY = Pad.GetDisabledControlNormal(0, 221);
            float leftAxisX = Pad.GetDisabledControlNormal(0, (int)Control.ScriptLeftAxisX);//left/right
            float leftAxisY = Pad.GetDisabledControlNormal(0, (int)Control.ScriptLeftAxisY);//up/down
            var rot = Cam.GetCamRot(camHandle,2);
            Vector3 pos = Cam.GetCamCoord(camHandle);
            var rr = GetDirectionByRotation(Cam.GetCamRot(camHandle,2));

            float fastMult = 1;
            float slowMult = 1;
            if (Input.IsDown((int)ControlKeys.Shift))
            {
                fastMult = 3;
            }
            else if (Input.IsDown((int)ControlKeys.LCtrl))
            {
                slowMult = 0.3f;
            }

            Vector3 vector = new Vector3(0, 0, 0);
            vector.X = rr.X * leftAxisY * fastMult * slowMult;
            vector.Y = rr.Y * leftAxisY * fastMult * slowMult;
            vector.Z = rr.Z * leftAxisY * fastMult * slowMult;
            Vector3 upVector = new Vector3(0, 0, 1);
            Vector3 rightVector = GetCrossProduct(
              GetNormalizedVector(rr),
              GetNormalizedVector(upVector)
            );

            RAGE.Elements.Player.LocalPlayer.Position = new Vector3(
              pos.X + vector.X + 1,
              pos.Y + vector.Y + 1,
              pos.Z + vector.Z + 1
            );

            rightVector.X *= leftAxisX * 0.5f;
            rightVector.Y *= leftAxisX * 0.5f;
            rightVector.Z *= leftAxisX * 0.5f;

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
                pos.Z - vector.Z + rightVector.Z + upMovement - downMovement
            );
            Cam.SetCamRot(
                camHandle,
                rot.X + rightAxisY * -5.0f,
                0.0f,
                rot.Z + rightAxisX * -5.0f,
                2
            );
        }

        public Vector3 GetNormalizedVector(Vector3 vector) 
        {
            
            float mag = (float)Math.Sqrt(
                vector.X * vector.X + vector.Y * vector.Y + vector.Z * vector.Z
                );

            vector.X = vector.X / mag;
            vector.Y = vector.Y / mag;
            vector.Z = vector.Z / mag;
            return vector;
        }

        public Vector3 GetCrossProduct(Vector3 v1, Vector3 v2) 
        {
            Vector3 vector = new Vector3(0, 0, 0);
            vector.X = v1.Y * v2.Z - v1.Z * v2.Y;
            vector.Y = v1.Z * v2.X - v1.X * v2.Z;
            vector.Z = v1.X * v2.Y - v1.Y * v2.X;
            return vector;
        }
        public static Vector3 GetDirectionByRotation(Vector3 rotation)
        {
            float num = rotation.Z * 0.0174532924f;
            float num2 = rotation.X * 0.0174532924f;
            float num3 = MathF.Abs(MathF.Cos(num2));
            return new Vector3 { X = -MathF.Sin(num) * num3, Y = MathF.Cos(num) * num3, Z = MathF.Sin(num2) };
        }
    }
}
