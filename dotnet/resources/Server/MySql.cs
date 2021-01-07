using System;
using System.Collections.Generic;
using System.Text;
using MySqlConnector;
using GTANetworkAPI;



namespace Server
{
    class MySql : Script
    {
        const string connStr = "server=localhost;user=root;database=drift;port=3306;password=";
        MySqlConnection conn = new MySqlConnection(connStr);
        [ServerEvent(Event.ResourceStart)]
        public void MySqlConnect()
        {
            conn.Open();
            try
            {
                NAPI.Util.ConsoleOutput("[Mysql]Подключено успешно");
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput("[Mysql]Не удалось подключиться к бд. Код ошибки: " + ex.ToString());
            }
            //TestQuery();
            TestExecute();
        }
        
        public void TestQuery() // Пример создания нового поля в таблице. Аналогично обновление в полях.
        {
            string sql = "INSERT INTO `accounts`(`Username`, `Password`) VALUES ('Nico', '123')";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.ExecuteNonQuery();
        }

        public void TestExecute() // Пример получение данных с таблицы 
        {
            string sql = "SELECT Username, Password FROM accounts";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                NAPI.Util.ConsoleOutput(rdr[0] + " -- " + rdr[1]);
            }
            rdr.Close(); 
        }
    }
}
            
