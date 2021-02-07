using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;

namespace Server.model
{
    public class Traks : DB_Tables
    {
        public string Name { get; set; }
        public List<Vector3> Positions { get; set; }
        public float Rotation { get; set; }
        public int TimeLimit { get; set; }
        public int RewardScore { get; set; }
        public int Reward { get; set; }

        //temp
        public List<ColShape> _ColShapes = new List<ColShape>();

    }
}
