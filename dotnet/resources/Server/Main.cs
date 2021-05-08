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
        public static List<Teleport> Teleports = new List<Teleport>();
        public static List<VehicleTuningCost> VehicleTuningsCost = new List<VehicleTuningCost>();
        public static List<VehicleStore> VehicleStore = new List<VehicleStore>();
        public static Dictionary<int, Titles> Titles = new Dictionary<int, Titles>();

        public static List<Admin> Admins = new List<Admin>();
        public static List<Report> Reports = new List<Report>();

        public static Dictionary<int, House> Houses = new Dictionary<int, House>();
        public static Dictionary<int, Garage> Garage = new Dictionary<int, Garage>();

        public static Dictionary<int, Clan> Clans = new Dictionary<int, Clan>();
        public static Dictionary<int, ClanRank> ClanRanks = new Dictionary<int, ClanRank>();

        public static List<HouseInterior> HousesInteriors = new List<HouseInterior>() // Интерьеры домов и пикап выхода
        {
            {new HouseInterior("apa_v_mp_h_01_a",new Vector3(-786.8663,315.7642,217.6385)) }
        };

        public static Dictionary<int, GarageType> GarageTypes = new Dictionary<int, GarageType>() // Интерьеры гаражей и различные координаты(транспорт, пикап выхода и тд)
        {
            {0, new GarageType(null, new Vector3(173.2903, -1003.6, -99.65707), new Vector3(172.88823, -1005.8173, -98.99993), 
                new List<GarageVehiclePosition>()
                {
                       new GarageVehiclePosition(new Vector3(175.57777, -1003.6113, -99.681),-178.4336f),
                       new GarageVehiclePosition(new Vector3(171.66412, -1003.73596, -99.67906),-177.80727f)
                }) 
            },
            {1, new GarageType(null, new Vector3(197.8153, -1002.293, -99.65749), new Vector3(197.8153, -1002.293, -99.65749),
                new List<GarageVehiclePosition>()
                {
                       new GarageVehiclePosition(new Vector3(193.0595, -998.85315, -99.68073),-179.30615f),
                       new GarageVehiclePosition(new Vector3(196.57741, -998.9872, -99.68111),-178.6532f),
                       new GarageVehiclePosition(new Vector3(200.02084, -998.96313, -99.68071),-179.25778f),
                       new GarageVehiclePosition(new Vector3(203.76367, -998.8757, -99.68072),-179.14012f)
                }) 
            }
        };

        public static List<Traks> Traks = new List<Traks>();
        public Main()
        {
            MySql.MySqlConnect();//Подключение к бд
            NAPI.Server.SetGlobalServerChat(false);
        }
        [ServerEvent(Event.ChatMessage)]
        public void ASQWEASD(Player player, string message)
        {

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
