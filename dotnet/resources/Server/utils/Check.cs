using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.utils
{
    public class Check : Script
    {
        public enum PlayerStatus
        {
            Spawn = 1,
            OnEvent = 3,
            
        };
        public static bool GetPlayerStatus(Player player, PlayerStatus status)
        {
            if (!Main.Players1.ContainsKey(player)) return false;
            if (!Main.Players1[player].IsSpawn) return false; 
            switch (status)
            {
                    case PlayerStatus.Spawn:
                    {
                        return true;
                    }
                    case PlayerStatus.OnEvent:
                    {
                        return false;
                    }
            }

            return true;
        }

        public static Player GetPlayerByID(int id)
        {
            foreach (Player player in Main.Players1.Keys.ToList())
            {
                if (!Main.Players1.ContainsKey(player)) continue;
                if (player.Value == id) return player;
            }
            return null;
        }
    }
}
