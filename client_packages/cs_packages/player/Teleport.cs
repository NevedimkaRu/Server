using RAGE;
using RAGE.Elements;
using System;
using System.Collections.Generic;
using System.Text;

namespace cs_packages.player
{
    class Teleport : Events.Script
    {
        public Teleport()
        {
            Events.Add("trigger_Teleport", TeleportTo);
        }

        private void TeleportTo(object[] args)
        {
            Vector3 tpPos = (Vector3)args[0];
            int time = 3000;
            utils.Utils.SmoothTeleport(tpPos,0, time, true);
        }


    }
}
