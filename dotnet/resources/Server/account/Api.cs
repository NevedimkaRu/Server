﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using GTANetworkAPI;
using MySqlConnector;
using Server.model;

namespace Server.account
{
    public class Api
    {
        public static void CreateAccount(Player player, string name, string password)
        {


            var model = new Account
            {
                Password = password,
                Username = name,
                DriftScore = 0
            };

            model.Insert();

            //MySql.Query($"INSERT INTO `accounts` (`Username`, `Password`) VALUES ('{name}','{password}')");
            Main.Players.Add(player, model);
        }
        public static void LoginAccount(Player player, string name, string password)
        {
            DataTable result = MySql.QueryRead($"SELECT Password FROM account WHERE Username='{name}'");

            if(result == null || result.Rows.Count == 0)
            {
                player.SendChatMessage("Такой аккаунт не существует.");
                return;
            }
            DataRow row = result.Rows[0];
            string pass = Convert.ToString(row[0]);
            if (pass != password)
            {
                player.SendChatMessage("Неправильный логин/пароль");
                return;
            }
            player.SendChatMessage($"Вы успешно авторизировались как {name}");

            LoadAccount(player, name);
        }
        public static void LoadAccount(Player player, string name)
        {
            DataTable result = MySql.QueryRead($"SELECT * FROM account WHERE Username='{name}'");
            
            var model = new Account();
            model.Username = name;
            foreach(DataRow row in result.Rows)
            {
                model.DriftScore = Convert.ToInt32(row["DriftScore"]);
            }
            Main.Players.Add(player, model);
        }
        public async Task<bool> SaveAccount(Player player)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand
                {
                    CommandText = "UPDATE `accounts` SET " +
                    "`DriftScore`= @ds " +
                    "WHERE `Username`= @name"
                };
                cmd.Parameters.AddWithValue("@ds", Main.Players[player].DriftScore);

                await MySql.QueryAsync(cmd);
                return true;
            }
            catch(Exception ex) 
            { 
                NAPI.Util.ConsoleOutput($"[SaveAccount Exept] {ex.ToString()}");
                return false;
            }
        }
    }
}
