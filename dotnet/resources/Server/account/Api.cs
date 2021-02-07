using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using GTANetworkAPI;
using MySqlConnector;
using Server.model;

namespace Server.account
{
    //todo Сохранение аккаунтов
    public class Api
    {
        public static void CreateAccount(Player player, string name, string password)
        {


            PlayerModel model = new PlayerModel();

            model.Account.Username = name;
            model.Account.Password = password;


            model.Account.Insert();

            LoginAccount(player, name, password);

        }
        public static void LoginAccount(Player player, string name, string password)
        {
            Account account = new Account();
            
            if(!account.GetByUserName(name))
            {
                player.SendChatMessage("Такой аккаунт не существует.");
                return;
            }

            if (account.Password != password)
            {
                player.SendChatMessage("Неправильный логин/пароль");
                return;
            }
            player.SendChatMessage($"Вы успешно авторизировались как {name}");
            PlayerModel playerModel = new PlayerModel();
            playerModel.Account = account;
            Main.Players1.Add(player, playerModel);
            character.Api.LoadCharacter(player, account.Id);
            
        }

        /*public async Task<bool> SaveAccount(Player player) 
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
        }*/
    }
}
