using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using Server.model;
using Server.account;
using Server.utils;

namespace Server
{
    class Main : Script
    {
        public static Dictionary<Player, Account> Players = new Dictionary<Player, Account>();
        public static Dictionary<Player, PlayerModel> Players1 = new Dictionary<Player, PlayerModel>();
        public static Dictionary<int, Vehicles> Veh = new Dictionary<int, Vehicles>();
        public static Dictionary<int, Teleport> Teleports = new Dictionary<int, Teleport>();
        public static Dictionary<int, VehicleTuning> VehicleTunings = new Dictionary<int, VehicleTuning>();

        public static Dictionary<int, House> Houses = new Dictionary<int, House>();
        public static Dictionary<int, Garage> Garage = new Dictionary<int, Garage>();

        public static List<HouseInterior> HousesInteriors = new List<HouseInterior>()
        {
            {new HouseInterior("apa_v_mp_h_01_a",new Vector3(-786.8663,315.7642,217.6385)) }
        };

        public static Dictionary<int, GarageType> GarageTypes = new Dictionary<int, GarageType>()
        {
            {0, new GarageType(0, null, new Vector3(173.2903, -1003.6, -99.65707), new Vector3(172.88823, -1005.8173, -98.99993), 
                new List<GarageVehiclePosition>()
                {
                       new GarageVehiclePosition(new Vector3(175.57777, -1003.6113, -99.681),-178.4336f),
                       new GarageVehiclePosition(new Vector3(171.66412, -1003.73596, -99.67906),-177.80727f)
                }) 
            }
        };

        public static List<Traks> Traks = new List<Traks>();

        public Main()
        {
            MySql.MySqlConnect();//Подключение к бд
        }

        [RemoteEvent("remote_PressEKey")]
        public void Remote_OnPlayerPressEKey(Player player)
        {
            garage.Api.OnPlayerPressEKey(player);
        }

        [RemoteEvent("remote_PressAlt")]
        public void Remote_OnPlayerPressAltKey(Player player)
        {
            house.Api.OnPlayerPressAltKey(player);
            garage.Api.OnPlayerPressAltKey(player);
        }        
    }
}
