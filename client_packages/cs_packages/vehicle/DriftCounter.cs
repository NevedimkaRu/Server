using System;
using System.Collections.Generic;
using System.Drawing;
using RAGE;
using RAGE.Ui;
//using RAGE.Game;
using RAGE.Elements;
using cs_packages.model;
using cs_packages.utils;

namespace cs_packages.vehicle
{
    public class DriftCounter : Events.Script
    {
        private static float modV;
        private static bool playerDrifting = false;
        private static float multiplier = 1.0f;
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
            if (Check.GetPlayerStatus(Check.PlayerStatus.Spawn))
            {
                Events.Tick += UpdateSpeedometer;
                //Дрифт счётчик
                //Events.Tick += UpdateSpeedometer;
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
        public void OnPlayerLeaveVehicle(Vehicle vehicle, int seatId)
        {
            if (Check.GetPlayerStatus(Check.PlayerStatus.Spawn))
            {
                Events.Tick -= UpdateSpeedometer;
                multiplier = 1;
                score = 0;
                playerDrifting = false;
                driftHTML.Active = false;
                //Events.Tick -= UpdateSpeedometer;
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
                driftHTML.Active = false;
               
                return; 
            }
            Vehicle vehicle = Player.LocalPlayer.Vehicle;


            //Browser.ExecuteFunctionEvent(speedometerHTML, "updateSpeedometer", new object[] { GetVehicleSpeed(vehicle), vehicle.Rpm, vehicle.Gear});
            OnDrift(vehicle);
        }

        private static void OnDrift(Vehicle vehicle)
        {
            float angle = Angle(vehicle);
            int timeLost = 5;
            DateTime tickTime = DateTime.UtcNow;
            if (angle > 0)
            {
                if (!driftHTML.Active)
                {
                    driftHTML.Active = true;
                }
                playerDrifting = true;
                lastTickTime = tickTime;

                score += (int)Math.Floor(angle * multiplier) / 10;

                if (score > 1000) multiplier = 1.1f;
                if (score > 4000) multiplier = 1.2f;
                if (score > 8000) multiplier = 1.3f;
                if (score > 12000) multiplier = 1.4f;
                if (score > 18000) multiplier = 1.5f;
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

            //RAGE.Game.UIText.Draw(totalscore.ToString(), new Point(1175, 560), 0.5f, Color.White, RAGE.Game.Font.ChaletComprimeCologne, false); // Вывоб общего числа очков
        }
        
        public static void StopDrift(int reason)
        {
            if (reason == 0)
            {
                ThisPlayer.Score += score;
                Api.Notify("~g~Time Out");
                UpdatePlayerScore(score);
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
            multiplier = 1.0f;
            score = 0;
            playerDrifting = false;
        }

        private static void UpdatePlayerScore(int score)
        { 
            Events.CallRemote("remote_UpdatePlayerDriftScore", score);//Отправляем на сервер
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

            float cosX = (sin * vehicle.GetVelocity().X + cos * vehicle.GetVelocity().Y) / Velocity(vehicle);
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
