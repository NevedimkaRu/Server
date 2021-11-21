using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using Server.vehicle;

namespace Server.remoteEvents
{
    class Vehicle : Script
    {
        [RemoteEvent("remote_UpdatePlayerDriftScore")]
        public void Remote_UpdatePlayerDriftScore(Player player, int score)
        {
            Api.UpdatePlayerDriftScore(player, score);
        }
        [RemoteEvent("remote_RepairCar")]
        public void Remote_RepairCar(Player player, object[] args)
        {
            Api.RepairCar(player, args);
        }
        [RemoteEvent("remote_SpawnPlayerCar")]
        public void Remote_SpawnPlayerVehicle(Player player, int carid)
        {
            Api.TeleportPlayerVehicleToPlayer(player, carid);
        }

        [RemoteEvent("remote_ChangeCarsSlots")]
        public void Remote_ChangeCarsSlots(Player player, string data)
        {
            Api.ChangeCarsSlots(player, data);
        }
    }
}
