using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Interface
{
    class Vui : Script
    {
        public static void ErrorNotify(Player player, string errorText)
        {
            player.TriggerEvent("trigger_ErrorNotify", errorText);
        }
    }
}
