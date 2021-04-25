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
        public const string connStr = "server=triniti.ru-hoster.com;user=justaAxT;database=justaAxT;port=3306;password=D8C34ykzq5;";
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

        /// <summary>
        /// Асинхронная версия Read
        /// </summary>
        /// <param name="command">Передаем заранее составленную команду</param>
        /// <returns>Ответ базы данных в формате таблицы</returns>
        public static async Task<DataTable> QueryReadAsync(MySqlCommand command)
        {
            if (Debug) NAPI.Util.ConsoleOutput($"[MySql QueryReadAsync] {command}");
            using (MySqlConnection connection = new MySqlConnection(connStr))
            {
                await connection.OpenAsync();

                command.Connection = connection;

                MySqlDataReader reader = await command.ExecuteReaderAsync();
                DataTable result = new DataTable();
                result.Load(reader);

                return result;
            }
        }
        /// <summary>
        /// Асинхронная версия Read
        /// </summary>
        /// <param name="command">Передаем заранее составленную команду</param>
        /// <returns>Ответ базы данных в формате таблицы</returns>
        public static async Task<DataTable> QueryReadAsync(string command)
        {
            using (MySqlCommand cmd = new MySqlCommand(command))
            {
                return await QueryReadAsync(cmd);
            }
        }
    }
}
            
