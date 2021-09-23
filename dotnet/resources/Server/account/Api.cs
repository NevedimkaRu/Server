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
        [ServerEvent(Event.ResourceStart)]
        public void LoadRememberAccounts() 
        {
            DataTable dt = MySql.QueryRead($"select SocialClubId from account where RememberMe = 1");
            foreach (DataRow row in dt.Rows)
            {
                Main.RememberAccounts.Add(Convert.ToUInt64(row["SocialClubId"]));
            }
        }

        [ServerEvent(Event.PlayerConnected)]
        public async void fillLoginData(Player player)
        {
            if (Main.RememberAccounts.Contains(player.SocialClubId))
            {
                DataTable dt = await MySql.QueryReadAsync($"select * from account where SocialClubId = {player.SocialClubId} and RememberMe = 1");
                if (dt.Rows.Count > 0)
                {
                    Account acc = new Account();
                    acc.LoadByDataRow(dt.Rows[0]);
                    utils.Trigger.ClientEvent(player, "trigger_FillLoginData", acc.Username, acc.Password);
                    //player.TriggerEvent("trigger_FillLoginData", acc.Username, acc.Password);
                    return;
                }
            }
            utils.Trigger.ClientEvent(player, "trigger_StartAuth");

           //player.TriggerEvent("trigger_StartAuth");

        }
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
            account.SocialClubId = player.SocialClubId;
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

            SendClientData(player);
        }
        public static async Task LoginAccount(Player player, string name, string password, bool rememberMe)
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
                foreach (var acc in Main.Players1)
                {
                    if (acc.Value.Account.Id == account.Id)
                    {
                        player.TriggerEvent("trigger_AuthError", "Аккаунт с таким логином уже авторизирован");
                    }
                }
                Ban banModel = new Ban();
                banModel = await admin.Ban.CheckBanStatus(account.Id);
                if(banModel != null)
                {
                    if(banModel.Permanent) player.SendChatMessage($"Ваш аккаунт забанен навсегда. По причине: {banModel.Reason}");
                    else player.SendChatMessage($"Ваш аккаунт забанен до {banModel.UnBanDate}. По причине: {banModel.Reason}");
                    player.Kick();
                    return;
                }
                account.LastIp = player.Address;
                account.Update("LastIp");
                PlayerModel playerModel = new PlayerModel();
                playerModel.Account = account;
                Main.Players1.Add(player, playerModel);
                await character.Api.LoadCharacter(player, account.Id);
                Main.Players1[player].IsSpawn = true;
                utils.Trigger.ClientEvent(player, "trigger_FinishAuth");
                player.SendChatMessage($"Вы успешно авторизировались как {name}");
                //player.TriggerEvent("trigger_FinishAuth");

                if (rememberMe)
                {
                    if (!Main.RememberAccounts.Contains(account.SocialClubId))
                    {
                        Main.RememberAccounts.Add(account.SocialClubId);
                        account.RememberMe = true;
                        await account.UpdateAsync("RememberMe");
                    }
                    else
                    {
                        await MySql.QueryAsync($"update account set RememberMe = 0 where SocialClubId = {player.SocialClubId} and RememberMe = 1; update account set RememberMe = 1 where id = {account.Id}");
                    }
                }
                else 
                {
                    if (Main.RememberAccounts.Contains(player.SocialClubId))
                    {
                        await MySql.QueryAsync($"update account set RememberMe = 0 where SocialClubId = {player.SocialClubId} and RememberMe = 1;");
                        Main.RememberAccounts.Remove(player.SocialClubId);
                    }
                }

                SendClientData(player);
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
            if (!Main.Players1.ContainsKey(player) || !Main.Players1[player].IsSpawn) return;
            
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

        private static void SendClientData(Player player)
        {
            //Отправка каких либо данных на клиент

            //Json файл магазина одежды
            character.clothes.Api.SendClothesStoreData(player);
        }
    }
}
