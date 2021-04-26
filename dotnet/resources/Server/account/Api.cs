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
    public class Api : Script
    {
        public static async Task CreateAccount(Player player, string name, string password, string characterName)
        {
            Account account = new Account();
            if (await account.GetByUserNameAsync(name)) 
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
        public static async Task LoginAccount(Player player, string name, string password)
        {
            Account account = new Account();

            if (await account.GetByUserNameAsync(name))
            {
                if (account.Password != password)
                {
                    player.TriggerEvent("trigger_AuthError", "Неправильный логин/пароль");
                    return;
                }
                Ban banModel = new Ban();
                banModel = await admin.Ban.CheckBanStatus(account.Id);
                if(banModel != null)
                {
                    player.SendChatMessage($"Ваш аккаунт забанен до {banModel.UnBanDate}. По причине: {banModel.Reason}");
                    player.Kick();
                    return;
                }
                PlayerModel playerModel = new PlayerModel();
                playerModel.Account = account;
                Main.Players1.Add(player, playerModel);
                await character.Api.LoadCharacter(player, account.Id);

                utils.Trigger.ClientEvent(player, "trigger_FinishAuth");
                player.SendChatMessage($"Вы успешно авторизировались как {name}");
                //player.TriggerEvent("trigger_FinishAuth");
                return;
            }
            else
            {
                player.TriggerEvent("trigger_AuthError", "Такой аккаунт не существует");
                return;
            }
        }
        public static void SaveAccount(Player player)
        {
            Main.Players1[player].Character.Update();
        }

        [ServerEvent(Event.PlayerDisconnected)]
        public void OnPlayerDisconnected(Player player, DisconnectionType type, string reason)
        {
            if (!Main.Players1.ContainsKey(player)) return;
            SaveAccount(player);
            Main.Players1.Remove(player);
        }
    }
}
