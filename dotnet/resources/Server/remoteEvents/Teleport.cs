using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using Server.teleport;

namespace Server.remoteEvents
{
    class Teleport : Script
    { 
        [RemoteProc("remote_SmoothTeleport")]
        public bool Remote_SmoothTeleport(Player player, float x, float y, float z, float rot)
        {
            return Api.SmoothTeleport(player, x, y, z, rot);
        }
        [RemoteEvent("remote_PlayerTeleported")]
        public void Remote_PlayerTeleported(Player player)
        {
            Api.PlayerTeleported(player);
        }
        [RemoteEvent("remote_TeleportTo")]
        public void Remote_TeleportTo(Player player, int tpId)
        {
            Api.TeleportTo(player, tpId);
        }
    }
}
