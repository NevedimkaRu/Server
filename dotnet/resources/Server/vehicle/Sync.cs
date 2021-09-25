using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.vehicle
{
    public class Sync : Script
    {
        public Sync()
        {

        }

        public enum SyncType : int
        {
            color = 1,
            door = 2
        }

        public const string VEHICLE_PRIMARY_COLOR = "vehicle_primary_color";
        public const string VEHICLE_SECONDARY_COLOR = "vehicle_secondary_color";
        public const string VEHICLE_COLOR_TYPE = "vehicle_color_type";

        public void ApplyVehicleSync(Vehicle vehicle)
        {
            foreach(Player player in NAPI.Pools.GetAllPlayers())
            {
                utils.Trigger.ClientEvent(player, "trigger_ApplyVehicleSync", vehicle);
            }
        }
        [Command("color")]
        public void CMD_SetVehicleColor(Player player, int r, int g, int b)
        {
            //if (player.Vehicle == null) return;
            Color color = new Color(r, g, b);
            if (player.Vehicle == null) return;
            player.Vehicle.SetSharedData(VEHICLE_PRIMARY_COLOR, color);
            player.SendChatMessage(color.ToString());
            Main.Veh[Main.Players1[player].CarId]._Tuning.PrimaryColor = color;
        }
        [Command("savecolor")]
        public void cmd_SaveColor(Player player)
        {
            if (player.Vehicle == null) return;
            Main.Veh[Main.Players1[player].CarId]._Tuning.Update();
        }
        [Command("type")]
        public void CMD_SetVehicleColorType(Player player, int type)
        {
            if (player.Vehicle == null) return;
            player.Vehicle.SetSharedData(VEHICLE_COLOR_TYPE, type);
            Main.Veh[Main.Players1[player].CarId]._Tuning.ColorType = type;
            //ApplyVehicleSync(player.Vehicle);
        }
        [ServerEvent(Event.EntityCreated)]
        public void OnEntityCreated(Entity entity)
        {
            if(entity.Type == EntityType.Vehicle)
            {
                entity.SetSharedData(VEHICLE_PRIMARY_COLOR, new Color(0, 0, 0));
                entity.SetSharedData(VEHICLE_COLOR_TYPE, 0);
            }
        }
    }
}
