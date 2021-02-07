using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.model
{
    public class TracksRecords : DB_Tables
    {
        public int CharacterId { get; set; }
        public int TrackId { get; set; }
        public int Score { get; set; }
    }
}
