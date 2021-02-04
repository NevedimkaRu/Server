using System;
using System.Collections.Generic;
using System.Text;

using RAGE;
using RAGE.Elements;

namespace cs_packages.game
{
    class Track : Events.Script
    {
        Blip blip;
        private int totaltime = -1;
        private bool activeTimer = false;

        Checkpoint checkpoint;
        private Track()
        {
            Events.Add("trigger_SetTrackRoute", SetTrackRoute);
            
        }

        private void TimeLost()
        {
            if(activeTimer != false)
            {
                totaltime--;
                if (totaltime != 0)
                {
                    Task.Run(TimeLost, 1000);
                    Chat.Output(totaltime.ToString());
                }
                if (totaltime <= 0)
                {
                    Events.CallRemote("remote_TimeLost");
                    blip.Destroy();
                    checkpoint.Destroy();
                    Chat.Output("Время вышло");
                }
            }
        }

        public void SetTrackRoute(object[] args)
        {
            Vector3 Position = (Vector3)args[0];
            Vector3 NextPosition = (Vector3)args[3];
            uint id = Convert.ToUInt32(args[1]);
            int time = Convert.ToInt32(args[2]);
            if(time != 0)
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
    }
}
