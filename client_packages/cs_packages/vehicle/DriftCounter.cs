using System;
using System.Collections.Generic;
using System.Drawing;
using RAGE;
using RAGE.Ui;
//using RAGE.Game;
using RAGE.Elements;
using cs_packages.model;

namespace cs_packages.vehicle
{
    public class DriftCounter : Events.Script
    {
        private static float modV;
        private static bool playerDrifting = false;
        private static int multiplier = 1;
        private static int totalscore = 0;
        private static DateTime lastTickTime;
        private static int score = 0;
        private static int vehHealth;
        private static HtmlWindow driftHTML;
        private static HtmlWindow speedometerHTML;

        private DriftCounter()
        {
            Events.OnPlayerEnterVehicle += OnPlayerEnterVehicle;
            Events.OnPlayerLeaveVehicle += OnPlayerLeaveVehicle;
            Events.Add("trigger_ResetDriftScore", ResetPlayerDriftScoreFromServer);
            Events.Add("trigger_GetPlayerScore", GetPlayerScore);
            //todo оптимизировать
            
        }
        
        public void GetPlayerScore(object[] args)
        {
            Events.CallRemote("remote_GetPlayerScore", score);
        }

        private void OnPlayerEnterVehicle(Vehicle vehicle, int seatId)
        {
            if ((bool)Player.LocalPlayer.GetSharedData("IsSpawn"))
            {
                //Дрифт счётчик
                Events.Tick += UpdateSpeedometer;
                vehicle.SetRadioEnabled(false);
                vehHealth = vehicle.GetHealth();
                if(driftHTML == null) 
                {
                    driftHTML = new HtmlWindow("package://statics/html/drift.html");
                    driftHTML.Active = false;
                }
                //Спидометр
                /*speedometerHTML = new HtmlWindow("package://statics/html/speedometer.html");
                speedometerHTML.Active = true;*/
            }


        }
        public void OnPlayerLeaveVehicle(Vehicle vehicle, int seatId)//todo проверка на авторизацию
        {
            if ((bool)Player.LocalPlayer.GetSharedData("IsSpawn"))
            {
                multiplier = 1;
                score = 0;
                playerDrifting = false;
                Events.Tick -= UpdateSpeedometer;
            }
        }

        public void SetV1elocity(object[] args)
        {
            Player player = Player.LocalPlayer;
            int gravity = Convert.ToInt32(args[0]);
            player.Vehicle.SetGravity(false);
        }

        private static void DriftScore()
        {
            Browser.ExecuteFunctionEvent(driftHTML, "driftScore", new object[] { score.ToString(), multiplier });
        }




        public static void UpdateSpeedometer(List<Events.TickNametagData> nametags)
        {
            if (Player.LocalPlayer.Vehicle == null) {
                Events.Tick -= UpdateSpeedometer;
                //if (speedometerHTML.Active) speedometerHTML.Active = false;
                if (driftHTML.Active)driftHTML.Active = false;
               
                return; 
            }
            Vehicle vehicle = Player.LocalPlayer.Vehicle;


            //Browser.ExecuteFunctionEvent(speedometerHTML, "updateSpeedometer", new object[] { GetVehicleSpeed(vehicle), vehicle.Rpm, vehicle.Gear});
            OnDrift(vehicle);
        }

        private static void OnDrift(Vehicle vehicle)
        {
            float angle = Angle(vehicle);
            float velocity = Velocity(vehicle);
            int timeLost = 3;
            DateTime tickTime = DateTime.UtcNow;
            if (angle > 0)
            {
                if (!driftHTML.Active)
                {
                    driftHTML.Active = true;
                }
                playerDrifting = true;
                lastTickTime = tickTime;

                score += (int)Math.Floor(angle) * multiplier / 5;

                if (score > 1000) multiplier = 2;
                if (score > 4000) multiplier = 3;
                if (score > 8000) multiplier = 4;
                if (score > 12000) multiplier = 5;
                if (score > 18000) multiplier = 6;
                if (score > 25000) multiplier = 7;
            }

            switch (multiplier)
            {
                case 1:
                    {
                        timeLost = 6;
                        break;
                    }
                case 3:
                    {
                        timeLost = 5;
                        break;
                    }
                case 5:
                    {
                        timeLost = 4;
                        break;
                    }
                case 7:
                    {
                        timeLost = 3;
                        break;
                    }
            }

            if ((tickTime.Ticks - lastTickTime.Ticks) >= timeLost * 10000000)
            {
                if (playerDrifting)
                {
                    StopDrift(0);
                }
            }
            else DriftScore();
            if (vehicle.GetHealth() < vehHealth)
            {
                vehHealth = vehicle.GetHealth();
                if (playerDrifting)
                {
                    StopDrift(228);
                }
            }
            else DriftScore();

            RAGE.Game.UIText.Draw(totalscore.ToString(), new Point(1175, 560), 0.5f, Color.White, RAGE.Game.Font.ChaletComprimeCologne, false); // Вывоб общего числа очков
        }
        
        public static void StopDrift(int reason)
        {
            if (reason == 0)
            {
                totalscore += score;
                ThisPlayer.Score += score;
                Api.Notify("~g~Time Out");
                UpdatePlayerScore(totalscore);
            }
            else
            {
                Api.Notify("~r~Crash!");
                Browser.ExecuteFunctionEvent(driftHTML, "drifterror", new object[] {});
            }
            ResetPlayerDriftScore();
        }

        public static void ResetPlayerDriftScore()
        {
            multiplier = 1;
            score = 0;
            playerDrifting = false;
        }

        private static void UpdatePlayerScore(int _score)
        {
            _score = totalscore;
            Events.CallRemote("onScoreUpdate", _score);//Отправляем на сервер
        }

        public static int GetVehicleSpeed(Vehicle vehicle)
        {
            Vector3 velocity = vehicle.GetVelocity();
            int speed = (int)Math.Round(Math.Sqrt(velocity.X * velocity.X + velocity.Y * velocity.Y + velocity.Z * velocity.Z) * 3.6f);

            return speed;
        }

        private static float Velocity(Vehicle vehicle)
        {
            Vector3 velocity = vehicle.GetVelocity();
            modV = (float)Math.Sqrt(velocity.X * velocity.X + velocity.Y * velocity.Y);
            return modV;
        }

        private static float Angle(Vehicle vehicle)
        {
            if (GetVehicleSpeed(vehicle) < 30) return 0;
            Vector3 rotation = vehicle.GetRotation(0);

            float sin = -(float)Math.Sin(Math.PI * rotation.Z / 180.0);
            float cos = (float)Math.Cos(Math.PI * rotation.Z / 180.0);

            float cosX = (sin * vehicle.GetVelocity().X + cos * vehicle.GetVelocity().Y) / modV;
            if (cosX > 0.966 || cosX < 0) return 0;
            return (float)(Math.Acos(cosX) * (180.0 / Math.PI) * 0.5);
        }
        public static void ResetPlayerDriftScoreFromServer(object[] args)
        {
            multiplier = 1;
            score = 0;
            playerDrifting = false;
        }
    }
}
