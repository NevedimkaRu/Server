using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;

namespace Server
{
    class Main : Script
    {
        public Main()
        {
            MySql.MySqlConnect();//Подключение к бд

        }
    }
}
