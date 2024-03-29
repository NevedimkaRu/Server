﻿using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.model
{
    public class GameEventConstants
    {
        //DRIFT EVETS
        public const int DRIFT_MAX_PLAYER = 5;
        public const int DRIFT_MIN_PLAYER = 2;
        public const int DRIFT_REWARD_1 = 5000;
        public const int DRIFT_REWARD_2 = 2500;
        public const int DRIFT_REWARD_3 = 1000;
        public const int GAME_EVENT_TYPE_DRIFT = 1;
    }

    public class GameEvent 
    {
        public String Name { get; set; }
        public int MinPlayers { get; set; }
        public int MaxPlayers { get; set; }
        public int GameType { get; set; }
        public MapGameEvent _Map { get; set; }
        public List<Player> Players = new List<Player>();
        public Dictionary<Player, int> CarIds = new Dictionary<Player, int>();
        public List<Player> ReadyPlayers = new List<Player>();
        public Dictionary<Player, int> Scores = new Dictionary<Player, int>();
        public uint Dimension = 10000;

        public bool IsActive { get; set; }

        public void RemovePlayer(Player player)
        {
            if (this.Players.Contains(player))
            {
                this.Players.Remove(player);
                this.CarIds.Remove(player);
                if (this.ReadyPlayers.Contains(player))
                {
                    this.ReadyPlayers.Remove(player);
                }
                if (this.Scores.ContainsKey(player))
                {
                    this.Scores.Remove(player);
                }
            }
        }
    }

    public class MapGameEvent
    {
        public String Name { get; set; }
        public ColShape CShape { get; set; }
        public int MaxTime { get; set; }
        public List<Vector3> PlayerPositions { get; set; }
        public float Rotation { get; set; }
            
        
    }
}
