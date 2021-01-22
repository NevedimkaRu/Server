using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;

namespace Server.model
{
    public class Vehicles : DB_Tables
    {
        public string ModelHash { get; set; }
        public int OwnerId { get; set; }
        public int Handling { get; set; }

        //temp
        public Vehicle _Veh { get; set; }

    }
}
