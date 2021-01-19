using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;

namespace Server.model
{
    public class Vehicles : DB_Tables
    {
        public string ModelHash { get; set; }
        public string Owner { get; set; }
        public Vehicle Veh { get; set; }

    }
}
