using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.utils
{
    public class DimensionManager : Script
    {
        public enum Type
        {
            House,
            Garage,
            Event
        }

        private static Dictionary<Type, uint> Dimensions = new Dictionary<Type, uint>()
        {
            {Type.House, 10000 },
            {Type.Garage, 15000 },
            {Type.Event, 30000 },
        };

        public static void SetPlayerDimension(Player player, Type type, int id) =>
            player.Dimension = GetDimensionFromId(type, id);
        public static void SetEntityDimension(Entity entity, Type type, int id) =>
            entity.Dimension = GetDimensionFromId(type, id);

        public static int GetIdFromDimension(Type type, uint dim) 
        { 
            return (int)(Dimensions[type] - dim); 
        }

        public static uint GetDimensionFromId(Type type, int id)
        {
            return (Dimensions[type] + (uint)id);
        }
    }
}
