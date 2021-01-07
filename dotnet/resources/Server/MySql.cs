using System;
using System.Collections.Generic;
using System.Text;
using MySqlConnector;
using GTANetworkAPI;



namespace Server
{
    class MySql : Script
    {
        [ServerEvent(Event.ResourceStart)]
        public void MySqlConnect()
        {
            const string connStr = "server=localhost;user=root;database=drift;port=3306;password=";
            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                NAPI.Util.ConsoleOutput("Connecting to MySQL...");
                conn.Open();

                string sql = "SELECT Username, Password FROM accounts";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    NAPI.Util.ConsoleOutput(rdr[0] + " -- " + rdr[1]);
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput(ex.ToString());
            }
        }  
    }
}
