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
        public void cmd_CreateVehicle(Player player)
        {
            Vector3 player_pos = player.Position;
            //NAPI.Vehicle.CreateVehicle(VehicleHash.Tampa, player_pos, 2f, new Color(0, 255, 100), new Color(0));
            /*NAPI.Vehicle.CreateVehicle(VehicleHash.Tampa2, player_pos, 2f, new Color(0, 255, 100), new Color(0));
            NAPI.Vehicle.CreateVehicle(VehicleHash.Tampa3, player_pos, 2f, new Color(0, 255, 100), new Color(0));*/
            player.SetIntoVehicle(NAPI.Vehicle.CreateVehicle(VehicleHash.Tampa3, player_pos, 2f, new Color(0, 255, 100), new Color(0)),0);
        }
        [Command("vehh", GreedyArg = true)]
        public void cmd_CreateVehicleHash(Player player, VehicleHash hash)
        {
            Vector3 player_pos = player.Position;
            player.SetIntoVehicle(NAPI.Vehicle.CreateVehicle(hash, player_pos, 2f, new Color(0, 255, 100), new Color(0)), 0);
        }
        [Command("vehhash")]
        public void cmd_Vehhash(Player player)
        {
            foreach(string names in Enum.GetNames(typeof(VehicleHash)))
            {
                player.SendChatMessage($"{names}");
            }
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
