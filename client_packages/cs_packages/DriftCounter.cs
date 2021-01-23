using System;
using System.Collections.Generic;
using System.Drawing;
using RAGE;
using RAGE.Ui;
//using RAGE.Game;
using RAGE.Elements;

namespace cs_packages
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

        private DriftCounter()
        {
            Events.Add("setVelocity", SetVelocity); 
            Events.OnPlayerEnterVehicle += OnPlayerEnterVehicle;
            Events.OnPlayerLeaveVehicle += OnPlayerLeaveVehicle;
            Events.OnPlayerCommand += OnPlayerCommand;
            Events.Tick += OnTick;
        }
        private void OnPlayerCommand(string cmd, Events.CancelEventArgs cancel)
        {
            string[] args = cmd.Split(new char[] { ' ' } );
            string commandName = args[0].Trim(new char[] { '/' });

            if (commandName == "anwh")
            {
                if (args[1] == null) {
                    Chat.Output("Using /anwh float");
                    return;
                }
                float anglewheel = float.Parse(args[1]);
                if(anglewheel < 0 || anglewheel > 2)
                {
                    Chat.Output("Не больше 2 и меньше 0");
                    return;
                }
                Player.LocalPlayer.Vehicle.SetHandling("fSteeringLock", anglewheel);
                                
            }            
            if (commandName == "curve")
            {
                if (args[1] == null) {
                    Chat.Output("Using /curve float");
                    return;
                }
                float angle = float.Parse(args[1]);
                Player.LocalPlayer.Vehicle.SetHandling("fTractionCurveMax", angle);               
            }
            if (commandName == "handling")
            {
                string handlename = args[1];
                float angle = float.Parse(args[2]);
                Player.LocalPlayer.Vehicle.SetHandling(handlename, angle);               
            }
        }
        private void OnPlayerEnterVehicle(Vehicle vehicle, int seatId)
        {
            /*int handlingId = Convert.ToInt32(vehicle.GetSharedData("sh_Handling"));
            Chat.Output("Enter Vehicle");
            if(handlingId != 0)
            {
                test.handling.SetHandling(vehicle, handlingId);
            }*/
            //Chat.Output($"client: carid - {vehicle.Id} remoteid - {vehicle.RemoteId}");
            
            driftHTML = new HtmlWindow("package://statics/html/drift.html");
            vehHealth = vehicle.GetHealth();
            driftHTML.Active = false;
            totalscore = (int)Player.LocalPlayer.GetSharedData("PLAYER_SCORE");
        }
        public void OnPlayerLeaveVehicle(Vehicle vehicle, int seatId)
        {
            Chat.Output("Leave Vehicle");
            driftHTML.Active = false;
            multiplier = 1;
            score = 0;
            playerDrifting = false;
        }
        public void SetVelocity(object[] args)
        {
            Player player = Player.LocalPlayer;
            int x;
            x = Convert.ToInt32(args[0]);
            int y;
            y = Convert.ToInt32(args[1]);
            int z;
            z = Convert.ToInt32(args[2]);

            player.Vehicle.SetVelocity((float)x, (float)y, (float)z);
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



        private void OnTick(List<Events.TickNametagData> nametags)
        {
            //if (Globals.playerLogged && Player.LocalPlayer.Vehicle != null)
            //{
                UpdateSpeedometer(); 
            //}
            /*if (RAGE.Input.IsDown(0x38) || RAGE.Input.IsDown(0x46) || RAGE.Input.IsDown(0x86))
            {
                Chat.Output("EEEEEEE");
            }*/
        }

        public static void UpdateSpeedometer()
        {
            if (Player.LocalPlayer.Vehicle == null) { return; }
            Vehicle vehicle = Player.LocalPlayer.Vehicle;

            Vector3 currentPosition = vehicle.Position;

            RAGE.Game.UIText.Draw(GetVehicleSpeed(vehicle).ToString(), new Point(1175, 650), 0.75f, Color.White, RAGE.Game.Font.ChaletComprimeCologne, false);

            OnDrift(vehicle);
        }

        private static void OnDrift(Vehicle vehicle)
        {
            Events.CallRemote("GetVehicleRotation", vehicle);
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

        private static void StopDrift(int reason)
        {
            if (reason == 0)
            {
                Chat.Output("Time Out"); totalscore += score;
                //RAGE.Game.Ui.SetNotificationTextEntry("Time Out");
                RAGE.Game.Ui.DrawNotificationAward("Timeout", "Time out", 0, 0, "Time out 1");
                //RAGE.Game.Ui.DrawNotification(true, false);
                UpdatePlayerScore(totalscore);
            }
            else
            {
                Chat.Output("Crash!");
                //RAGE.Game.Ui.SetNotificationTextEntry("Crash");
                RAGE.Game.Ui.DrawNotificationAward("", "Crash", 0, 109, "");
                //RAGE.Game.Ui.DrawNotification(true, true);
            }
                //if(reason == 0) { Browser.ExecuteFunctionEvent(driftHTML, "driftError", new object[] { score.ToString() }); }
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
    }
}
