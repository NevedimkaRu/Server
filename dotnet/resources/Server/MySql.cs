using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using MySqlConnector;
using GTANetworkAPI;
using System.Threading.Tasks;

namespace Server
{
    class MySql : Script
    {
        public const string connStr = "server=localhost;user=root;database=drift;port=3306;password=;";
        readonly static MySqlConnection conn = new MySqlConnection(connStr);
        public static bool Debug = true;

        public static void MySqlConnect()
        {
            try
            {
                conn.Open();
                NAPI.Util.ConsoleOutput("[Mysql]Подключено успешно");
            }
            catch (ArgumentException ex)
            {
                NAPI.Util.ConsoleOutput("[Mysql]Ошибка в строке подключения: " + ex.ToString());
            }
            catch(MySqlException me)
            {
                switch (me.Number)
                {
                    case 1042:
                        NAPI.Util.ConsoleOutput("[MySql]Unable to connect to any of the specified MySQL hosts");
                        break;
                    case 0:
                        NAPI.Util.ConsoleOutput("[MySql]Отказанно в доступе");
                        break;
                    default:
                        NAPI.Util.ConsoleOutput($"[MySql] Не удалось подключиться к бд, код ошибки: ({me.Number}) {me.Message}");
                        break;
                }
            }
            conn.Close();
            //Test();
            //TestExecute();
        }
        public static void Test()
        {
            /*ЗАПРОСЫ*/
            /*Query("INSERT INTO `accounts`(`Username`, `Password`) VALUES('Nagibator2',458)");//Простой запрос

            //Запрос с использование MySqlCommand
            string name = "Nagibator2";
            string pass = "1488528";
            MySqlCommand cmd = new MySqlCommand
            {
                CommandText = "UPDATE `accounts` SET `Password`= @pass WHERE `Username`= @name"
            };
            cmd.Parameters.AddWithValue("@pass", pass);
            cmd.Parameters.AddWithValue("@name", name);
            Query(cmd);

            //Запрос с последующим получение данных с бд
            var result = QueryRead($"SELECT Password FROM accounts WHERE Username='Nagibator2'");
            foreach(DataRow row in result.Rows)
            {
                //int pass = Convert.ToInt32(row["Password"]);//int
                string password = row["Password"].ToString();//string
                NAPI.Util.ConsoleOutput(password);
            }*/
            //TestExecute();
            

        }

        public static void Query(MySqlCommand command)
        {
            try
            {
                using (MySqlConnection connect = new MySqlConnection(connStr))
                {
                    connect.Open();
                    command.Connection = connect;

                    command.ExecuteNonQuery();
                    //
                    
                }
                if (Debug) NAPI.Util.ConsoleOutput($"[MySql Query] {command.CommandText}");
            }
            catch (Exception ex) 
            { 
                NAPI.Util.ConsoleOutput($"[MySql Query Error] {ex}"); 
            }
        }

        public static void Query(string command)
        {
            using (MySqlCommand cmd = new MySqlCommand(command))
            {
                Query(cmd);
            }
        }

        public static void Query(string command, Dictionary<string, object> parameters)
        {
            using (MySqlCommand cmd = new MySqlCommand(command))
            {
                foreach (string f in parameters.Keys)
                {
                    cmd.Parameters.AddWithValue("@" + f, parameters[f]);
                }
                Query(cmd);
            }
        }

        public static async Task QueryAsync(MySqlCommand command)
        {
            try
            {
                using (MySqlConnection connect = new MySqlConnection(connStr))
                {
                    await connect.OpenAsync();
                    command.Connection = connect;

                    await command.ExecuteNonQueryAsync();
                    //

                }
                if (Debug) NAPI.Util.ConsoleOutput($"[MySql Query] {command.CommandText}");
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput($"[MySql QueryAsync Error] {ex}");
            }
        }

        public static async Task QueryAsync(string command)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connStr))
                {
                    await connection.OpenAsync();

                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.Connection = connection;
                        cmd.CommandText = command;

                        await cmd.ExecuteNonQueryAsync();
                    }
                }
                if (Debug) NAPI.Util.ConsoleOutput($"[MySql QueryAsync] {command}");
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput($"[MySql QueryAsync Error] {ex}");
            }
        }

        /// <summary>
        /// Отправить запрос и считать ответ
        /// </summary>
        /// <param name="command">Передаем заранее составленную команду</param>
        /// <returns>Ответ базы данных в формате таблицы</returns>
        public static DataTable QueryRead(MySqlCommand command)
        {
            if (Debug) NAPI.Util.ConsoleOutput("[MySql QueryR] " + command.CommandText);
            using (MySqlConnection connect = new MySqlConnection(connStr))
            {
                connect.Open();

                command.Connection = connect;

                MySqlDataReader reader = command.ExecuteReader();
                DataTable result = new DataTable();
                result.Load(reader);

                return result;
            }
        }
        /// <summary>
        /// Отправить запрос и считать ответ
        /// </summary>
        /// <param name="command">Передаем команду в виде строки</param>
        /// <returns>Ответ базы данных в формате таблицы</returns>
        public static DataTable QueryRead(string command)
        {
            using (MySqlCommand cmd = new MySqlCommand(command))
            {

                return QueryRead(cmd);
            }
        }

        public static DataTable QueryRead(string command, Dictionary<string, object> parameters)
        {
            using (MySqlCommand cmd = new MySqlCommand(command))
            {
                foreach(string f in parameters.Keys)
                {
                    cmd.Parameters.AddWithValue("@" + f, parameters[f]);
                }
                return QueryRead(cmd);
            }
        }

        public static void TestQuery() // Пример создания нового поля в таблице. Аналогично обновление в полях.
        {
            string sql = "INSERT INTO `accounts`(`Username`, `Password`) VALUES ('Nico', '123')";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.ExecuteNonQuery();
        }

        public static int InsertReq(object[] args)
        {

            for (int i = 0; i < args.Length; i++ )
            {
               
            }
            return 1;
        }

        /*public static DataTable InsertReq(string command)
        {
            using (MySqlCommand cmd = new MySqlCommand(command))
            {
                return QueryRead(cmd);
            }
        }*/

        /*public static void TestExecute() // Пример получение данных с таблицы 
        {
            MySqlConnection connect = new MySqlConnection(connStr);
            connect.Open();
            string sql = "INSERT INTO `characters`( `Name`, `Money`, `Sex`, `Status`, `Score`) VALUES ('Name','999999','0','0','0'); Select last_insert_id()";
            MySqlCommand cmd = new MySqlCommand(sql);

            cmd.Connection = connect;

            MySqlDataReader rdr = cmd.ExecuteReader();
            
            NAPI.Util.ConsoleOutput(rdr.ToString());

            while (rdr.Read())
            {
                NAPI.Util.ConsoleOutput(rdr[0] + "ХУЙ");
            }
            rdr.Close(); 
        }*/
    }
}
            
