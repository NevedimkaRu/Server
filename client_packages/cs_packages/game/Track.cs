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
        
        //Checkpoint checkpoint;
        private Track()
        {
            Events.Add("trigger_SetTrackRoute", SetTrackRoute);
            Events.OnPlayerEnterCheckpoint += OnPlayerEnterCheckpoint;
        }

        private void OnPlayerEnterCheckpoint(Checkpoint checkpoint, Events.CancelEventArgs cancel)
        {
            
        }

        public void SetTrackRoute(object[] args)
        {
            uint id = Convert.ToUInt32(args[1]);
            Vector3 Position = (Vector3)args[0];


            if (id == 999)
            {
                blip.Destroy();
                return;
            }

            if(blip != null)
            {
                blip.Destroy();
                blip = new Blip(id, Position);//38
                blip.SetRoute(true);
            }
            else
            {
                blip = new Blip(id, Position);//38
                blip.SetRoute(true);
            }
            Chat.Output("CLASSNO");
        }
    }
}
