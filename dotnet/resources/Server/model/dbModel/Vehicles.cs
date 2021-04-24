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
        public List<VehicleHandling> _HandlingData = new List<VehicleHandling>();
        public VehiclesGarage _Garage { get; set; }
        

        //temp
        public Vehicle _Veh { get; set; }
        public ColShape _ColShape { get; set; }
    }

    public class VehiclesGarage : DB_Tables
    {
        public int VehicleId { get; set; }
        public int GarageId { get; set; }
        public int GarageSlot { get; set; }
    }

}
