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
        public static Dictionary<Player, Account> Players = new Dictionary<Player, Account>();
        public static Dictionary<int, Vehicles> Veh = new Dictionary<int, Vehicles>();
        public static Dictionary<int, Teleport> Teleports = new Dictionary<int, Teleport>();

        public Main()
        {
            MySql.MySqlConnect();//Подключение к бд
            
        }
    }
}
