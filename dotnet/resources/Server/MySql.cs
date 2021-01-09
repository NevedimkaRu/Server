﻿using System;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using System.Text;
using MySqlConnector;
using GTANetworkAPI;



namespace Server
{
    class MySql : Script
    {
        private const string connStr = "server=localhost;user=root;database=drift;port=3306;password=;";
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
            Test();
        }
        public static void Test()
        {
            /*ЗАПРОСЫ*/
            Query("INSERT INTO `accounts`(`Username`, `Password`) VALUES('Nagibator2',458)");//Простой запрос

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
                string pass = row["Password"].ToString();//string
                NAPI.Util.ConsoleOutput(pass);
            }

            
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

        public static async void QueryAsync(MySqlCommand command)
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
                NAPI.Util.ConsoleOutput($"[MySql Query Error] {ex}");
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

                DbDataReader reader = command.ExecuteReader();
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

        public static void TestQuery() // Пример создания нового поля в таблице. Аналогично обновление в полях.
        {
            string sql = "INSERT INTO `accounts`(`Username`, `Password`) VALUES ('Nico', '123')";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.ExecuteNonQuery();
        }

        public static void TestExecute() // Пример получение данных с таблицы 
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
            
