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
        public delegate void OnPlayerDriftingDelegate(float angle, bool isCalled);
        public static event OnPlayerDriftingDelegate OnPlayerDrifting;

        private bool IsHandlerAttached = false;
        private bool IsPlayerDrifting = false;
        private float Multiplier = 1.0f;
        private DateTime LastTickTime;
        private int Score = 0;
        private int LastVehHealth;
        private HtmlWindow driftHTML;

        public DriftCounter()
        {
            Events.OnPlayerEnterVehicle += OnPlayerEnterVehicle;
            Events.OnPlayerLeaveVehicle += OnPlayerLeaveVehicle;
            Events.Add("trigger_ResetDriftScore", ResetPlayerDriftScoreFromServer);
            Events.Add("trigger_GetPlayerScore", GetPlayerScore);
        }

        public void GetPlayerScore(object[] args)
        {
            Events.CallRemote("remote_GetPlayerScore", Score);
        }

        private void OnPlayerEnterVehicle(Vehicle vehicle, int seatId)
        {
            Chat.Output("Enter");
            if (Check.GetPlayerStatus(Check.PlayerStatus.Spawn))
            {
                if(!IsHandlerAttached)
                {
                    Chat.Output("ХОБА");
                    OnPlayerDrifting += PlayerDrifting;

                    Events.Tick += UpdateSpeedometer;
                    IsHandlerAttached = true;
                }
                ThisPlayer.CurrentVehicle = vehicle;
                vehicle.SetRadioEnabled(false);
                LastVehHealth = vehicle.GetHealth();
                if(driftHTML == null)
                {
                    driftHTML = new HtmlWindow("package://statics/html/drift.html");
                    driftHTML.Active = false;
                }
            }
        }
        public void OnPlayerLeaveVehicle(Vehicle vehicle, int seatId)
        {
            Chat.Output("leave");
            if (Check.GetPlayerStatus(Check.PlayerStatus.Spawn))
            {
                Events.Tick -= UpdateSpeedometer;
                OnPlayerDrifting -= PlayerDrifting;
                IsHandlerAttached = false;
                Multiplier = 1;
                Score = 0;
                IsPlayerDrifting = false;
                driftHTML.Active = false;
                ThisPlayer.CurrentVehicle = null;
            }
        }

        public void UpdateSpeedometer(List<Events.TickNametagData> nametags)
        {
            if (Player.LocalPlayer.Vehicle == null) {
                ThisPlayer.CurrentVehicle = null;
                OnPlayerDrifting.Invoke(0, false);
                Events.Tick -= UpdateSpeedometer;
                OnPlayerDrifting -= PlayerDrifting;
                IsHandlerAttached = false;
                driftHTML.Active = false;
                return;
            }
            float angle = Angle(Player.LocalPlayer.Vehicle);
            OnPlayerDrifting.Invoke(angle, true);
        }

        private void StopDrift(int reason)
        {
            if (reason == 0)
            {
                ThisPlayer.Score += Score;
                Api.Notify("~g~Time Out");
                UpdatePlayerScore(Score);
            }
            else
            {
                Api.Notify("~r~Crash!");
                Browser.ExecuteFunctionEvent(driftHTML, "drifterror", new object[] {});
            }
            ResetPlayerDriftScore();
        }

        public void ResetPlayerDriftScore()
        {
            Multiplier = 1.0f;
            Score = 0;
            IsPlayerDrifting = false;
        }

        private void PlayerDrifting(float angle, bool isCalled)
        {
            Vehicle vehicle = Player.LocalPlayer.Vehicle;
            int timeLost = 5;
            DateTime tickTime = DateTime.UtcNow;
            if (angle > 0)
            {
                if (!driftHTML.Active)
                {
                    driftHTML.Active = true;
                }
                IsPlayerDrifting = true;
                LastTickTime = tickTime;
                Score += (int)Math.Floor(angle * Multiplier) / 10;

                if (Score > 1000) Multiplier = 1.1f;
                if (Score > 4000) Multiplier = 1.2f;
                if (Score > 8000) Multiplier = 1.3f;
                if (Score > 12000) Multiplier = 1.4f;
                if (Score > 18000) Multiplier = 1.5f;
            }

            if ((tickTime.Ticks - LastTickTime.Ticks) >= timeLost * 10000000)
            {
                if (IsPlayerDrifting)
                {
                    StopDrift(0);
                }
            }
            else Browser.ExecuteFunctionEvent(driftHTML, "driftScore", new object[] { Score.ToString(), Multiplier });
            if (vehicle.GetHealth() < LastVehHealth)
            {
                LastVehHealth = vehicle.GetHealth();
                if (IsPlayerDrifting)
                {
                    StopDrift(228);
                }
            }
            else Browser.ExecuteFunctionEvent(driftHTML, "driftScore", new object[] { Score.ToString(), Multiplier });
        }

        private void UpdatePlayerScore(int score)
        { 
            Events.CallRemote("remote_UpdatePlayerDriftScore", score);//Отправляем на сервер
        }

        public static int GetVehicleSpeed(Vehicle vehicle)
        {
            Vector3 velocity = vehicle.GetVelocity();
            int speed = (int)Math.Round(Math.Sqrt(velocity.X * velocity.X + velocity.Y * velocity.Y + velocity.Z * velocity.Z) * 3.6f);

            return speed;
        }
        public void ResetPlayerDriftScoreFromServer(object[] args)
        {
            Multiplier = 1;
            Score = 0;
            IsPlayerDrifting = false;
        }

        private float Velocity(Vehicle vehicle)
        {
            Vector3 velocity = vehicle.GetVelocity();
            return (float)Math.Sqrt(velocity.X * velocity.X + velocity.Y * velocity.Y);
        }

        private float Angle(Vehicle vehicle)
        {
            if (GetVehicleSpeed(vehicle) < 30) return 0;
            Vector3 rotation = vehicle.GetRotation(0);

            float sin = -(float)Math.Sin(Math.PI * rotation.Z / 180.0);
            float cos = (float)Math.Cos(Math.PI * rotation.Z / 180.0);

            float cosX = (sin * vehicle.GetVelocity().X + cos * vehicle.GetVelocity().Y) / Velocity(vehicle);
            if (cosX > 0.966 || cosX < 0) return 0;
            return (float)(Math.Acos(cosX) * (180.0 / Math.PI) * 0.5);
        }
    }
}
