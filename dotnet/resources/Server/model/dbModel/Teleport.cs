using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;

namespace Server.model
{
    public class Teleport : DB_Tables
    {
        public string Name { get; set; }
        public string Discription { get; set; }
        public Vector3 Position { get; set; }
        public int Type { get; set; }
        public Vector3 ColShapePos { get; set; }
        public float ColShapeRange { get; set; }
        public float ColShapeHeight { get; set; }

        public ColShape _ColShape { get; set; }

        public Teleport(string Name, string Discription, Vector3 Position)
        {
            this.Name = Name;
            this.Discription = Discription;
            this.Position = Position;
        }

        public Teleport()
        { 
        
        }
    }
}
