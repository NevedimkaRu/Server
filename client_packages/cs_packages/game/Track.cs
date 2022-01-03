using System;
using System.Collections.Generic;
using System.Text;

using RAGE;
using RAGE.Elements;

namespace cs_packages.game
{
    class Track : Events.Script
    {
        private static Blip blip;
        private static int totaltime = -1;
        private static bool activeTimer = false;
        private static int Score;

        private static Checkpoint checkpoint;
        private Track()
        {
            Events.Add("trigger_SetTrackRoute", SetTrackRoute);
            
        }

        private static void TimeLost()
        {
            if(activeTimer != false)
            {
                totaltime--;
                if (totaltime != 0)
                {
                    Task.Run(TimeLost, 1000);
                }
                if (totaltime <= 0)
                {
                    Events.CallRemote("remote_TimeLost");
                    blip.Destroy();
                    checkpoint.Destroy();
                    Score = 0;
                    Chat.Output("Время вышло");

                    vehicle.DriftCounter.OnPlayerDrifting -= PlayerDrifting;
                }
            }
        }

        public static void SetTrackRoute(object[] args)
        {
            Vector3 Position = (Vector3)args[0];
            Vector3 NextPosition = (Vector3)args[3];
            uint id = Convert.ToUInt32(args[1]);
            int time = Convert.ToInt32(args[2]);
            int firstcp = Convert.ToInt32(args[4]);
            if(firstcp == 1)
            {
                vehicle.DriftCounter.OnPlayerDrifting += PlayerDrifting;
            }
            if (firstcp == 2 && Player.LocalPlayer.Vehicle != null)
            {
                vehicle.DriftCounter.OnPlayerDrifting -= PlayerDrifting;
                Events.CallRemote("remote_GetPlayerTrackScore", Score);//todo сделать валидацию
                Score = 0;
            }
            if (time != 0)
            {
                activeTimer = true;
                totaltime = time;
                Task.Run(TimeLost, 1000);
            }
            if(time == -1)
            {
                activeTimer = false;
                totaltime = -1;
            }

            if (id == 999)
            {
                
                blip.Destroy();
                checkpoint.Destroy();
                Score = 0;
                return;
            }

            if(blip != null)
            {
                blip.Destroy();
                checkpoint.Destroy();
                blip = new Blip(id, Position);

                blip.SetRoute(true);

                if (id == 38)
                {
                    checkpoint = new Checkpoint(40, Position, 2.0f, Position, new RGBA(255, 60, 60));
                }
                else
                {
                    checkpoint = new Checkpoint(18, Position, 2.0f, NextPosition, new RGBA(207, 207, 207));
                }
            }
            else
            {
                checkpoint = new Checkpoint(18, Position, 2.0f, NextPosition, new RGBA(207, 207, 207));
                blip = new Blip(id, Position);
                blip.SetRoute(true);
            }
        }

        private static void PlayerDrifting(float angle, bool isCalled)
        {
            if(isCalled == false)
            {
                vehicle.DriftCounter.OnPlayerDrifting -= PlayerDrifting;
                Score = 0;
            }

            Score += (int)Math.Floor(angle / 10);
            Chat.Output(Score.ToString());
        }
    }
}
