﻿using RAGE;
using RAGE.Game;
using System;
using System.Collections.Generic;
using System.Text;
using static RAGE.Events;
using static RAGE.Ui.Cursor;

namespace cs_packages.Camera
{
    static class CamRotator
    {
        // Камера которая была до работы скрипта
        private static int StartCamera = -1;

        private static int Camera { get; set; }
        private static Vector3 BasePosition { get; set; }
        private static Vector3 LookAtPosition { get; set; }
        private static Vector3 OffsetVector { get; set; }
        private static float Heading { get; set; }
        private static float BaseHeading { get; set; }
        private static Vector2 CurrentPoint { get; set; }

        private static bool IsPause = false;
        private static bool IsActive = false;
        private static float ZUp { get; set; }
        private static float ZUpMultipler { get; set; }
        private static Vector2 XBound { get; set; }
        private static Vector2 ZBound { get; set; }
        private static int Fov { get; set; }

        private static int MinFov = 30;
        private static int MaxFov = 90;

        private static bool OnMove = false;

        private static int MarginLeft {get;set;}
        private static int MarginRight {get;set;}
        private static int MarginTop {get;set;}
        private static int MarginBottom {get;set;}

        public static void Start(Vector3 basePosition, Vector3 lookAtPosition, Vector3 offsetVector, float heading = -1f, int camera = -1, int fov = 45, int marginLeft = 600, int marginRight = 0, int marginTop = 0, int marginBottom = 100)
        {
            SetDefault();
            Camera = camera;
            BasePosition = basePosition;
            LookAtPosition = lookAtPosition;
            OffsetVector = offsetVector;
            Heading = heading;
            BaseHeading = heading;
            CurrentPoint = new Vector2(0, 0);
            IsPause = false;
            ZUp = 0;
            ZUpMultipler = 1;
            XBound = new Vector2(0, 0);
            ZBound = new Vector2(-0.01f, 0.8f);
            Fov = fov;
            MarginLeft = marginLeft;
            MarginRight = marginRight;
            MarginTop = marginTop;
            MarginBottom = marginBottom;
            CheckHeading();
            CreateCam();
            Cam.PointCamAtCoord(Camera, LookAtPosition.X, LookAtPosition.Y, LookAtPosition.Z);


            Activate(true);
            Events.Tick += OnTick;
        }

        public static void Pause(bool state)
        {
            IsPause = state;
        }

        public static void Stop(int returnToCamera = -1)
        {
            if (!IsActive) return;
            if (returnToCamera != -1)
            {
                Cam.SetCamActiveWithInterp(returnToCamera, Camera, 1000, 1, 1);
            }
            else if (StartCamera != -1)
            {
                Cam.SetCamActiveWithInterp(StartCamera, Camera, 1000, 1, 1);
            }
            else 
            {
                Cam.RenderScriptCams(false, false, 0, true, false, 0);
            }
            RAGE.Task.Run(() => {
                Cam.DestroyCam(Camera, true);
                Activate(false);
            }, 
            delayTime: 1100);
        }

        public static void Reset()
        {
            Heading = BaseHeading;
            ZUp = 0;
            ChangePosition();
        }

        public static int GetCam()
        {
            return Camera;
        }

        private static void CreateCam()
        {
            if (Camera == -1)
            {
                Vector3 playerPos = RAGE.Elements.Player.LocalPlayer.Position;
                Vector3 camRot = Cam.GetGameplayCamRot(2);
                int cam = Cam.CreateCamWithParams("DEFAULT_SCRIPTED_CAMERA", playerPos.X, playerPos.Y, playerPos.Z, camRot.X, camRot.Y, camRot.Z, Fov, true, 0);
                Cam.SetCamActive(cam, true);
                Cam.RenderScriptCams(true, false, 0, true, false, 0);
                Camera = cam;
                ChangePosition();
            }
            else 
            {
                Vector3 playerPos = RAGE.Elements.Player.LocalPlayer.Position;
                Vector3 camRot = Cam.GetGameplayCamRot(2);
                int cam = Cam.CreateCamWithParams("DEFAULT_SCRIPTED_CAMERA", playerPos.X, playerPos.Y, playerPos.Z, camRot.X, camRot.Y, camRot.Z, Fov, true, 0);
                //Cam.SetCamActive(cam, true);
                Cam.RenderScriptCams(true, false, 0, true, false, 0);

                StartCamera = Camera;
                Camera = cam;
                ChangePosition();
                Cam.SetCamActiveWithInterp(Camera, StartCamera, 1000, 1, 1);
            }
        }

        private static void CheckHeading()
        {
            if (Heading == -1f)
            {
                Heading = RAGE.Elements.Player.LocalPlayer.GetRotation(2).Z;
            }
        }

        private static void OnTick(List<Events.TickNametagData> nametags)
        {
            if (!RAGE.Ui.Cursor.Visible || !IsActive || IsPause)
            {
                return;
            }

            float x = RAGE.Game.Pad.GetDisabledControlNormal(2, (int)RAGE.Game.Control.CursorX);
            float y = RAGE.Game.Pad.GetDisabledControlNormal(2, (int)RAGE.Game.Control.CursorY);

            if (IsPointEmpty())
            {
                SetPoint(x, y);
            }

            Vector2 currentPoint = GetPoint();
            float dX = currentPoint.X - x;
            float dY = currentPoint.Y - y;


            int screenX = 0;
            int screenY = 0;

            Graphics.GetActiveScreenResolution(ref screenX, ref screenY);
            float marginLeft = (MarginLeft / (1920f / screenX));
            float marginRight = (MarginRight / (1920f / screenX));
            float marginTop = (MarginTop / (1080f / screenX));
            float marginBottom = (MarginBottom / (1080f / screenX));
            
            float _x = RAGE.Ui.Cursor.Position.X;
            float _y = RAGE.Ui.Cursor.Position.Y;


            SetPoint(x, y);

            if (RAGE.Game.Pad.IsDisabledControlPressed(2, (int)RAGE.Game.Control.CursorAccept))
            {
                if (_x > 0 + marginLeft && _x < screenX - marginRight && _y > 0 + marginTop && _y < screenY - marginBottom)
                //if (_x < screenX/2 + radiusX && _x > screenX/2 - radiusX && _y < screenY/2 + radiusY && _y > screenY/2 - radiusY)
                {
                    OnMove = true;
                    //Chat.Output("OnMove: true");
                }
                if(OnMove)
                {
                    OnMouseMove(dX, dY);
                    //Chat.Output("MouseMove");
                }
            }
            else 
            {
                //Chat.Output("OnMove: false");
                OnMove = false;
            }

            if (RAGE.Game.Pad.IsDisabledControlJustPressed(2, (int)RAGE.Game.Control.CursorScrollDown))
            {
                ChangeScale(false);
            }
            if (RAGE.Game.Pad.IsDisabledControlJustPressed(2, (int)RAGE.Game.Control.CursorScrollUp))
            {
                ChangeScale(true);
            }
        }

        private static void ChangeScale(bool side)
        {
            int f = 2;
            if (side) f = f * -1;

            Fov = Fov + f;
            if (Fov > MaxFov)
            {
                Fov = MaxFov;
            }
            if (Fov < MinFov)
            {
                Fov = MinFov;
            }
            Cam.SetCamFov(Camera, Fov);

            ChangePosition();

        }

        public static void OnMouseMove(float dX, float dY)
        {
            Heading = NormilizeHeading(Heading + dX * 100);

            float relativeHeading = GetRelativeHeading();

            if (relativeHeading > XBound.X && relativeHeading < XBound.Y)
            {
                relativeHeading = Math.Abs(XBound.X - relativeHeading) > Math.Abs(XBound.X - relativeHeading)
                    ? XBound.Y
                    : XBound.X;
            }

            Heading = NormilizeHeading(-relativeHeading + BaseHeading);
            ZUp += dY * ZUpMultipler * -1;

            if (ZUp > ZBound.Y)
            {
                ZUp = ZBound.Y;
            }
            else if (ZUp < ZBound.X)
            {
                ZUp = ZBound.X;
            }

            ChangePosition();
        }

        private static float NormilizeHeading(float heading)
        {
            if (heading > 360)
            {
                heading = heading - 360;
            }
            else if (heading < 0)
            {
                heading = 360 + heading;
            }

            return heading;
        }

        private static float GetRelativeHeading()
        {
            return NormilizeHeading(BaseHeading - Heading);
        }

        private static void Activate(bool state)
        {
            IsActive = state;
            if (!IsActive)
            {
                Events.Tick -= OnTick;
                RAGE.Ui.Cursor.Visible = false;
            }
            else {
                RAGE.Task.Run(() =>
                {
                    RAGE.Ui.Cursor.Visible = true;
                },
                delayTime: 100);
            }
        }

        private static void ChangePosition()
        {
            Vector3 Position = RAGE.Game.Object.GetObjectOffsetFromCoords(BasePosition.X, BasePosition.Y, BasePosition.Z + ZUp, Heading, OffsetVector.X, OffsetVector.Y, OffsetVector.Z);

            Cam.SetCamCoord(Camera, Position.X, Position.Y, Position.Z);
        }

        private static Vector2 GetPoint()
        {
            return CurrentPoint;
        }

        private static void SetPoint(float x, float y)
        {
            CurrentPoint = new Vector2(x, y);
        }

        private static bool IsPointEmpty()
        {
            return CurrentPoint.X == 0 && CurrentPoint.Y == 0;
        }

        private static void SetXBound(float min, float max)
        {
            XBound = new Vector2(min, max);
        }

        public static void SetZBound(float min, float max)
        {
            ZBound = new Vector2(min, max);
        }

        public static void SetMaxMinPov(int maxPov, int minPov)
        {
            MaxFov = maxPov;
            MinFov = minPov;
        }

        public static void SetZUpMultipler(float value)
        {
            ZUpMultipler = value;
        }


        private static void SetDefault()
        {
            SetZBound(-0.8f, 1.8f);
            SetZUpMultipler(8f);
            SetMaxMinPov(90, 30);
            StartCamera = -1;
            OnMove = false;
        }
    }
}
