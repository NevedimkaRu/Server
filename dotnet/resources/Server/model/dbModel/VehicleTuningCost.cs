using System;
using System.Collections.Generic;
using System.Text;

namespace Server.model
{
    public class VehicleTuningCost : DB_Tables
    {
        public uint ModelHash { get; set; }
        public int Component { get; set; }
        public int Index { get; set; }
        public int Cost { get; set; }
    }
}
