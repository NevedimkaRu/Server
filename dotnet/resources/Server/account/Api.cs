using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using GTANetworkAPI;
using MySqlConnector;
using Server.Interface;
using Server.model;

namespace Server.account
{
    //todo Сохранение аккаунтов и переписать под async
    public class Api
    {
        public static void CreateAccount(Player player, string name, string password, string characterName)
        {


            Account account = new Account();
            if (account.GetByUserName(name)) 
            {
                player.TriggerEvent("trigger_RegisterError", "Аккаунта с таким логином уже существует");
                return;
            }

            account.Username = name;
            account.Password = password;


            account.Insert();
            PlayerModel playerModel = new PlayerModel();
            playerModel.Account = account;
            Main.Players1.Add(player, playerModel);
            //todo Нужно сделать проверку на занятость ника именно здесь
            character.Api.CreateCharacter(player, characterName);
            player.TriggerEvent("trigger_FinishRegister");
        }
        public static void LoginAccount(Player player, string name, string password)
        {
            Account account = new Account();
            
            if(!account.GetByUserName(name))
            {
                player.TriggerEvent("trigger_AuthError", "Такой аккаунт не существует");
                return;
            }

            if (account.Password != password)
            {
                player.TriggerEvent("trigger_AuthError", "Неправильный логин/пароль");

                return;
            }
            player.SendChatMessage($"Вы успешно авторизировались как {name}");
            PlayerModel playerModel = new PlayerModel();
            playerModel.Account = account;
            Main.Players1.Add(player, playerModel);
            character.Api.LoadCharacter(player, account.Id);

            player.TriggerEvent("trigger_FinishAuth");
        }

        [ServerEvent(Event.PlayerConnected)]
        public void OnPlayerConnected(Player player)
        {
            player.SetSharedData("IsSpawn", false);
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
                cmd.Parameters.AddWithValue("@ds", Main.Players1[player].Character.DriftScore);

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
