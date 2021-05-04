using System;
using System.Collections.Generic;
using System.Text;

namespace Server.model
{
    class VehicleStore : DB_Tables
    {
        public string Hash { get; set; }
        public string Title { get; set; }
        public int Cost { get; set; }
        public int Level {get;set;}
    }
}
