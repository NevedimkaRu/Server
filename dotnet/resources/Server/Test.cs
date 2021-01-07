using System;
using GTANetworkAPI;

namespace Server
{
    public class Test : Script
    {
        [ServerEvent(Event.ResourceStart)]
        public void OnResourceStart()
        {
            NAPI.Util.ConsoleOutput("Всё ок");
        }
        [Command("veh")]
        public void Veh(Player player)
        {
            Vector3 player_pos = player.Position;
            NAPI.Vehicle.CreateVehicle(VehicleHash.Omnis, player_pos, 2f, new Color(0, 255, 100), new Color(0));
        }
        [Command("sv1", GreedyArg = true)]
        public void cmd_SetVelocytyX(Player player, string vlX, string vlY, string vlZ)
        {
            object[] vl = new object[3];
            vl[0] = vlX;
            vl[1] = vlY;
            vl[2] = vlZ;

            player.TriggerEvent("setVelocity", vl);
        }

    }
}
