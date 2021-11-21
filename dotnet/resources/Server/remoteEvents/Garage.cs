using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using Server.garage;

namespace Server.remoteEvents
{
    class Garage : Script
    {
        [RemoteEvent("remote_EnterGarage")]
        public void Remote_EnterGarage(Player player, int garageid, int type)
        {
            Api.PlayerEnterGarage(player, garageid, type);
        }
        [RemoteEvent("remote_CloseGarage")]
        public void Remote_CloseGarage(Player player, int garageid, bool IsClosed)
        {
            Main.Garage[garageid].Closed = IsClosed;
        }
        [RemoteEvent("remote_BuyGarage")]
        public void Remote_BuyGarage(Player player, int garageid)
        {
            Api.PlayerBuyGarage(player, garageid);
        }
    }
}
