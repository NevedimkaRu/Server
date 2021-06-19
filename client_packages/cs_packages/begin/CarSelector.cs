using System;
using System.Collections.Generic;
using System.Text;
using RAGE;
using RAGE.Game;

namespace cs_packages.begin
{
    public class CarSelector : Events.Script
    {
        public CarSelector()
        {
            Events.OnPlayerCommand += Cmd;
            Events.OnClickWithRaycast += OnClick;
        }
        public class CarList
        {
            public string CarHash { get; set; }
            public Vector3 CarPos { get; set; }
            public Vector3 CamPos { get; set; }
            public Vector3 CamRot { get; set; }
            public int CamId { get; set; }
            public RAGE.Elements.Vehicle Veh;
            public CarList(string CarHash, Vector3 CarPos, Vector3 CamPos, Vector3 CamRot)
            {
                this.CarHash = CarHash;
                this.CarPos = CarPos;
                this.CamPos = CamPos;
                this.CamRot = CamRot;
            }
        }

        public static Dictionary<int, CarList> Cars = new Dictionary<int, CarList>()
        {
            {0, new CarList(null,
                null,
                new Vector3(997.0209f, -3001.767f, -38.99113f),
                new Vector3(-6.245818f, 0, 89.35027f)) },
            {1, new CarList("accord",
                new Vector3(988.03546f, -3005.588f, -40.3263f), 
                new Vector3(991.6177f, -3002.787f, -38.8937f),
                new Vector3(-11.86801f, 0, 131.8012f)) },
            {2, new CarList("civic",
                new Vector3(987.86316f, -3001.7742f, -40.326305f),
                new Vector3(991.6177f, -2999.106f, -38.8937f),
                new Vector3(-11.86801f, 0, 131.8012f)) },
            {3, new CarList("mark2",
                new Vector3(987.8483f, -2998.0576f, -40.328674f),
                new Vector3(991.6177f, -2995.224f, -38.8937f),
                new Vector3(-11.86801f, 0, 131.8012f)) },
        };

        

        private static int SelectedCar = -1;

        private void OnClick(int x, int y, bool up, bool right, float relativeX, float relativeY, Vector3 worldPos, int entityHandle)
        {
            if (SelectedCar == -1) return;
            float radiusX = 200;
            foreach (var cars in Cars)
            {
                if (cars.Value.CarHash != null)
                {
                    float _screenX = 0;
                    float _screenY = 0;
                    Graphics.GetScreenCoordFromWorldCoord(cars.Value.CarPos.X, cars.Value.CarPos.Y, cars.Value.CarPos.Z, ref _screenX, ref _screenY);
                    float screenX = _screenX * 1920;
                    float screenY = _screenY * 1080;
                    if (screenX < x + radiusX && screenX > x - radiusX)
                    {
                        SelectDefaultCar(cars.Key);
                        Chat.Output(cars.Key.ToString());
                        return;
                    }
                }
                Chat.Output("false");
            }
        }

        public static void CreateDafaultCams()
        {
            RAGE.Game.Interior.EnableInteriorProp(252673, "basic_style_set");
            RAGE.Game.Interior.RefreshInterior(252673);
            RAGE.Elements.Player.LocalPlayer.Position = new Vector3(994.5925f, -3002.594f, -39.64699f);
            foreach (var cars in Cars)
            {
                cars.Value.CamId = Cam.CreateCamWithParams("DEFAULT_SCRIPTED_CAMERA", cars.Value.CamPos.X, cars.Value.CamPos.Y, cars.Value.CamPos.Z,
                    cars.Value.CamRot.X, cars.Value.CamRot.Y, cars.Value.CamRot.Z, 45, true, 0);
                Chat.Output(cars.Value.CamId.ToString());
                if (cars.Value.CarHash != null)
                {
                    RAGE.Task.Run(() =>
                    {
                        cars.Value.Veh = new RAGE.Elements.Vehicle(RAGE.Util.Joaat.Hash(cars.Value.CarHash),
                            new Vector3(cars.Value.CarPos.X, cars.Value.CarPos.Y, cars.Value.CarPos.Z), -90.0f);
                    });
                }
            }
            Cam.SetCamActive(Cars[0].CamId, true);
            Cam.RenderScriptCams(true, false, 0, true, false, 0);

            SelectedCar = 0;
            RAGE.Task.Run(() =>
            {
                RAGE.Ui.Cursor.Visible = true;
            },delayTime:100);
        }

        public static void SelectDefaultCar(int i)
        {
            if(i != SelectedCar || i < 3 || i >= 0)
            {
                if(i == 0)
                {
                    Cam.SetCamActiveWithInterp(Cars[i].CamId, Cars[SelectedCar].CamId, 3000, 1, 1);
                }
                else
                {
                    Cam.SetCamActiveWithInterp(Cars[i].CamId, Cars[0].CamId, 3000, 1, 1);
                }
                SelectedCar = i;
            }
        }

        private void Cmd(string cmd, Events.CancelEventArgs cancel)
        {
            string[] args = cmd.Split(new char[] { ' ' });
            string commandName = args[0].Trim(new char[] { '/' });

            if (commandName == "sv")
            {
                SelectDefaultCar(Convert.ToInt32(args[1]));
            }
            if (commandName == "sv0")
            {
                CreateDafaultCams();
            }
        }
    }
}
