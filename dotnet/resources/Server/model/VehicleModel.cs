using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;

namespace Server.model
{
    public class VehicleModel
    {
        public int Id { get; set; }
        public string ModelHash { get; set; }
        public string Owner { get; set; }
        public Vehicle Veh { get; set; }

    }
}
