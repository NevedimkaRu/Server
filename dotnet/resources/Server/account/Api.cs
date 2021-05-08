using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
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
            if (await account.LoadByOtherFieldAsync("Username",name))
            {
                player.TriggerEvent("trigger_RegisterError", "Аккаунт с таким логином уже существует");
                return;
            }

            account.Username = name;
            account.Password = password;
            account.SociaClubId = player.SocialClubId;
            account.RegisterIp = player.Address;
            account.LastIp = player.Address;
            account.RegisterDate = DateTime.UtcNow.AddHours(3);

            account.Insert();
            PlayerModel playerModel = new PlayerModel();
            playerModel.Account = account;
            Main.Players1.Add(player, playerModel);
            //todo Нужно сделать проверку на занятость ника именно здесь
            await character.Api.CreateCharacter(player, characterName);
            player.TriggerEvent("trigger_FinishRegister");
        }
        public static async Task LoginAccount(Player player, string name, string password)
        {
            Account account = new Account();
            //if (await account.GetByUserNameAsync(name))
            if (await account.LoadByOtherFieldAsync("Username", name))
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
                account.LastIp = player.Address;
                account.Update("LastIp");
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
            if(Main.Players1[player].Mute != null)
            {
                foreach(model.Mute muteModel in Main.Players1[player].Mute)
                {
                    muteModel.Update("TimeLeft");
                    Main.Players1[player].MuteTimer.Dispose();
                }
            }
            foreach(var veh in Main.Veh.Values)
            {
                if(veh.OwnerId == Main.Players1[player].Character.Id)
                {
                    veh._Veh.Delete();
                    Main.Veh.Remove(veh.Id);
                }
            }
            admin.Report.DeleteReport(player);
            Main.Players1.Remove(player);
        }
    }
}
