using System;
using System.Collections.Generic;
using System.Text;

namespace Server.model
{
    public class Record : DB_Tables
    {
        public int CharacterId { get; set; }
        public int Score { get; set; }
        public int MapId { get; set; }
        public int Type { get; set; }
        public string VehicleHash { get; set; }
    }
}
