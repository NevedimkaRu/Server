using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.vehicle
{
    class Tunning : Script
    {
        [RemoteEvent("remote_SetTunning")]
        public void SetTunning(Player player, object[] args)
        {
            int modeType = (int)args[0];
            int modeIndex = (int)args[1];

            player.Vehicle.SetMod(modeType, modeIndex);
            if(Test.Debug)
            {
                player.SendChatMessage($"{modeType} - {modeIndex}");
            }
            
        }
    }
}
