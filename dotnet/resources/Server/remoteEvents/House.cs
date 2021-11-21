using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using Server.house;
namespace Server.remoteEvents
{
    class House : Script
    {
        [RemoteEvent("remote_ExitHouse")]
        public void Remote_ExitHouse(Player player)
        {
            Api.PlayerExitHouse(player);
        }

        [RemoteEvent("remote_CloseHouse")]
        public void Remote_CloseHouse(Player player, int houseid, bool IsClosed)
        {
            Main.Houses[houseid].Closed = IsClosed;
        }
        [RemoteEvent("remote_BuyHouse")]
        public void Remote_BuyHouse(Player player, int houseid)
        {
            Api.PlayerBuyHouse(player, houseid);
        }

        [RemoteEvent("remote_EnterHouse")]
        public void Remote_EnterHouse(Player player, int houseid)
        {
            Api.PlayerEnterHouse(player, houseid);
        }
    }
}
