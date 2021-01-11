using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using Server.model;
using Server.account;

namespace Server
{
    class Main : Script
    {
        public static Dictionary<Player, AccountModel> Players = new Dictionary<Player, AccountModel>();
        public static Dictionary<int, VehicleModel> Vehicles = new Dictionary<int, VehicleModel>();

        public Main()
        {
            MySql.MySqlConnect();//Подключение к бд
            
        }
    }
}
