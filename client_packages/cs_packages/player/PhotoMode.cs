using RAGE;
using RAGE.Game;
using System;
using System.Collections.Generic;
using System.Text;

namespace cs_packages.player
{
    /*todo Что должно быть:
    Выбор погоды и времени суток
    Выбор анимации(думаю пока только, если игрок вне машины, нужно крч просмотреть возможные анимации)
    Выбор режима камеры(приатаченный к камере или свободный)
    Выбор фильтров
    Там где у нас будут дефолтные подсказки по кнопкам, поставить подсказки по Фоторежиму
    Возможность скрывать игрока(тупо невидимым делать)
    */
    public class PhotoMode : Events.Script
    {
        bool IsPhotoMode = false;
        int CamHandle;

        public PhotoMode()
        {
            Events.Add("vui_pTime", SetTime);
            Events.Add("vui_pFov", SetPov);
            Events.Add("vui_pRotate", SetRotate);
            //Events.Add("vui_pHideNickNames", HideNicNames);
            //Events.Add("vui_pHidePlayers", HidePlayers);
            //Events.Add("vui_pHidePlayer", HidePlayer);
            Events.Add("vui_pSetWeather", SetWeather);
            Events.OnPlayerCommand += Cmd;
            //Input.Bind(RAGE.Ui.VirtualKeys.X, true, CameraTest);
            Input.Bind(RAGE.Ui.VirtualKeys.F5, true, () => {
                IsPhotoMode = !IsPhotoMode;
                if (IsPhotoMode) StartPhotoMode();
                else StopPhotoMode();
            });
        }

       

        public static List<string> ScreenEffect = new List<string>()
        {
            {"glasses_black"},
            {"Glasses_BlackOut"},
            {"glasses_blue"},
            {"glasses_brown"},
            {"glasses_Darkblue"},
            {"glasses_green"},
            {"glasses_orange"},
            {"glasses_pink"},
            {"glasses_purple"},
            {"glasses_red"},
            {"glasses_Scuba"},
            {"glasses_VISOR"},
            {"glasses_yellow"},
            {"facebook_serveroom"},
            {"eyeINtheSKY"},
            {"crane_cam"},
            {"Drunk"},
            {"DRUG_gas_huffin"},
            {"Drug_deadman"},
            {"BombCam01"},
            {"Bloom"},
            {"BloomMid"},
            {"BloomLight"},
            {"AirRaceBoost01"},
            {"AirRaceBoost02"},
            {"pulse"},
            {"RaceTurboDark"},
            {"RaceTurboFlash"},
            {"RaceTurboLight"},
            {"ranch"},
            {"REDMIST"},
            {"REDMIST_blend"},
            {"rply_contrast"},
            {"rply_contrast_neg"},
            {"rply_saturation"},
            {"rply_saturation_neg"},
            {"spectator1"},
            {"spectator2"},
            {"spectator3"},
            {"spectator4"},
            {"spectator5"},
            {"spectator6"},
            {"spectator7"},
            {"spectator8"},
            {"spectator9"},
            {"spectator10"}
        };

        private void Cmd(string cmd, Events.CancelEventArgs cancel)
        {
            string[] args = cmd.Split(new char[] { ' ' });
            string commandName = args[0].Trim(new char[] { '/' });

            if (commandName == "tc")
            {
                Graphics.ClearTimecycleModifier();
                Graphics.SetTimecycleModifier(args[1]);
            }
        }

        public void StartPhotoMode()
        {
            Api.Notify("Фоторежим ~g~активирован");
            Vector3 playerPos = RAGE.Elements.Player.LocalPlayer.Position;
            Vector3 camRot = Cam.GetGameplayCamRot(2);
            Vector3 playerCamPos = Cam.GetGameplayCamCoord();
            Vector3 playerCamRot = Cam.GetGameplayCamRot(2);
            CamHandle = Cam.CreateCamWithParams("DEFAULT_SCRIPTED_CAMERA", playerCamPos.X, playerCamPos.Y, playerCamPos.Z, playerCamRot.X, playerCamRot.Y, playerCamRot.Z, Cam.GetGameplayCamFov(), true, 0);
            Cam.SetCamActive(CamHandle, true);
            Cam.RenderScriptCams(true, false, 0, true, false, 0);
            RAGE.Elements.Player player = RAGE.Elements.Player.LocalPlayer;
            
            player.FreezePosition(true);
            Pad.DisableAllControlActions(0);
            Events.Tick += CamRender;
            Interface.PhotoEditor.OpenMenu();
        }

        public void StopPhotoMode()
        {
            Api.Notify("Фоторежим ~r~деактивирован");
            Cam.RenderScriptCams(false, false, 0, true, false, 0);
            RAGE.Elements.Player player = RAGE.Elements.Player.LocalPlayer;
            player.FreezePosition(false);
            Pad.EnableAllControlActions(1);
            CamHandle = 0;
            Cam.DestroyCam(CamHandle, true);
            Events.Tick -= CamRender;
            Interface.PhotoEditor.CloseMenu();
        }
        public void CamRender(List<Events.TickNametagData> nametags)
        {
            if(!IsPhotoMode) Events.Tick -= CamRender;
            if (RAGE.Ui.Cursor.Visible) return;
            if (CamHandle == 0 || utils.Check.GetPlayerStatus(utils.Check.PlayerStatus.OpenChat)) return;
            Pad.DisableAllControlActions(0);
            float rightAxisX = Pad.GetDisabledControlNormal(0, 220);
            float rightAxisY = Pad.GetDisabledControlNormal(0, 221);
            float leftAxisX = Pad.GetDisabledControlNormal(0, (int)Control.ScriptLeftAxisX);//left/right
            float leftAxisY = Pad.GetDisabledControlNormal(0, (int)Control.ScriptLeftAxisY);//up/down
            Vector3 rot = Cam.GetCamRot(CamHandle, 2);
            Vector3 pos = Cam.GetCamCoord(CamHandle);
            Vector3 rr = utils.Utils.GetDirectionByRotation(Cam.GetCamRot(CamHandle, 2));

            float slowMult = 0.3f;
            if (Input.IsDown(RAGE.Ui.VirtualKeys.Menu))
            {
                slowMult = 0.06f;
            }

            Vector3 vector = new Vector3(0, 0, 0);
            vector.X = rr.X * leftAxisY * slowMult;
            vector.Y = rr.Y * leftAxisY * slowMult;
            vector.Z = rr.Z * leftAxisY * slowMult;
            Vector3 upVector = new Vector3(0, 0, 1);
            Vector3 rightVector = utils.Utils.GetCrossProduct(
              utils.Utils.GetNormalizedVector(rr),
              utils.Utils.GetNormalizedVector(upVector)
            );

            rightVector.X *= leftAxisX * slowMult;
            rightVector.Y *= leftAxisX * slowMult;
            rightVector.Z *= leftAxisX * slowMult;

            float angleMovement = 0.0f;
            if (Input.IsDown(RAGE.Ui.VirtualKeys.Q))
            {
                angleMovement = -0.3f;
            }            
            else if (Input.IsDown(RAGE.Ui.VirtualKeys.E))
            {
                angleMovement = 0.3f;
            }
            float fov = 0f;
            if (Input.IsDown(RAGE.Ui.VirtualKeys.Up))
            {
                fov = -0.5f;
            }
            else if (Input.IsDown(RAGE.Ui.VirtualKeys.Down))
            {
                fov = 0.5f;
            }

            float upMovement = 0.0f;
            if (Input.IsDown(RAGE.Ui.VirtualKeys.LeftShift))
            {
                upMovement = 0.5f;
            }
            float downMovement = 0.0f;
            if (Input.IsDown(RAGE.Ui.VirtualKeys.LeftControl))
            {
                downMovement = 0.5f;
            }
            Vector3 nextMovement =
                new Vector3(
                    pos.X - vector.X + rightVector.X,
                    pos.Y - vector.Y + rightVector.Y,
                    pos.Z - vector.Z + (rightVector.Z + upMovement - downMovement) * slowMult
                );
            if (nextMovement.DistanceTo(RAGE.Elements.Player.LocalPlayer.Position) < 100)
            {
                Cam.SetCamCoord(
                    CamHandle,
                    pos.X - vector.X + rightVector.X,
                    pos.Y - vector.Y + rightVector.Y,
                    pos.Z - vector.Z + (rightVector.Z + upMovement - downMovement) * slowMult
                );
            }
            Cam.SetCamRot(
                CamHandle,
                rot.X + rightAxisY * -5.0f,
                rot.Y + angleMovement,
                rot.Z + rightAxisX * -5.0f,
                2
            );
            float camFov = Cam.GetCamFov(CamHandle);
            float nextFov = camFov + fov;
            if (nextFov >= 20 && nextFov <= 110)
            {
                Cam.SetCamFov(CamHandle, nextFov);
            }
        }

        private void SetWeather(object[] args)
        {
            RAGE.Game.Misc.SetWeatherTypeNow(args[0].ToString());
        }

        private void SetRotate(object[] args)
        {
            //todo Доделать
        }

        private void SetPov(object[] args)
        {
            float fov = Convert.ToSingle(args[0]);
            Cam.SetCamFov(CamHandle, fov);
        }

        private void SetTime(object[] args)
        {
            int hour = Convert.ToInt32(args[0]);
            int min = Convert.ToInt32(args[1]);
            int sec = Convert.ToInt32(args[2]);
            RAGE.Game.Clock.SetClockTime(hour, min, sec);
        }
    }
}
