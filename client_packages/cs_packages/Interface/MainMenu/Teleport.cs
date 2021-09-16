using RAGE;
using System;
using System.Collections.Generic;
using System.Text;

namespace cs_packages.Interface.MainMenu
{
    class Teleport : Events.Script
    {
        Teleport()
        {
            Events.Add("vui_teleportTo", TeleportTo);
        }

        private void TeleportTo(object[] args)
        {
            int tpId = Convert.ToInt32(args[0]);
            Events.CallRemote("remote_TeleportTo", tpId);
        }
    }
}
