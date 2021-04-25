using GTANetworkAPI;
using System;
using Server.model;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;

namespace Server.admin
{
    class Commands : Script
    {
        //todo не забыть включить проверку на ники
        [Command("makeadmin", GreedyArg = true)]
        public void cmd_MakeAdmin(Player player, string accountId, string adminLvl)
        {
            if (!utils.Check.GetPlayerStatus(player, utils.Check.PlayerStatus.Spawn)) return;
            /*if(Main.Players1[player].Account.Username != "Mario" || Main.Players1[player].Account.Username != "Rico")
            {
                return;
            }*/
            int id = Convert.ToInt32(accountId);
            int lvl = Convert.ToInt32(adminLvl);
            if(lvl == 0)
            {
                foreach (Player target in NAPI.Pools.GetAllPlayers())
                {
                    Api.RemoveAdmin(id);
                    if (Main.Players1[target].Account.Id == id)
                    {
                        player.SendChatMessage($"Вы сняли игрока {Main.Players1[target].Character.Name} с должности администратора");
                        target.SendChatMessage($"{Main.Players1[player].Character.Name} снял вас с должности администратора");
                        return;
                    }
                }
                player.SendChatMessage($"(offline)Вы сняли игрока {Main.Admins.Find(c => c.AccountId == id)._CharName} c должности администратора");
                Api.RemoveAdmin(id);
                return;
            }
            if(Main.Admins.Find(c => c.AccountId == id) != null)
            {
                player.SendChatMessage($"Аккаунт с ID {id} уже назначен администратором");
                return;
            }
            foreach(Player target in NAPI.Pools.GetAllPlayers())
            {
                if(Main.Players1[target].Account.Id == id)
                {
                    target.SendChatMessage($"Администратор {Main.Players1[player].Character.Name} назначил вас администратором {lvl} уровня");
                    target.SendChatMessage($"Придумайте пароль для доступа к администраторским функциям используя команду /areg ");
                    player.SendChatMessage($"Вы назначили игрока {Main.Players1[target].Character.Name} администратором {lvl} уровня");
                    Api.AddAdmin(id, lvl, Main.Players1[target].Character.Name);
                    return;
                }
            }
            Api.AddAdmin(id, lvl);
            player.SendChatMessage($"Вы назначили игрока {Main.Admins.Find(c => c.AccountId == id)._CharName} администратором {lvl} уровня");
        }

        [Command("admins")]
        public void cmd_Admins(Player player)
        {
            string admins = "";
            foreach (Player target in NAPI.Pools.GetAllPlayers())
            {
                if(Main.Players1[target].Admin != null)
                {
                    admins += $"{Main.Players1[target].Character.Name} : {Main.Players1[target].Admin.Lvl} | ";
                }
            }
            player.SendChatMessage(admins);
        }
        [Command("areg", GreedyArg = true)]
        public void cmd_Aregister(Player player, string pass)
        {
            Admin model = Main.Admins.Find(c => c.AccountId == Main.Players1[player].Account.Id);
            if (model != null)
            {
                if (model.Password != null)
                {
                    player.SendChatMessage("У вас уже установлен пароль");
                    return;
                }
                if (pass.Length < 5 || pass.Length > 11)
                {
                    player.SendChatMessage("Длина пароля должена быть от 5 до 11 символов");
                    return;
                }
                model.Password = pass;
                model.Update("Password");
                player.SendChatMessage("Вы установили пароль, теперь авторизируйтесь с помощью /alogin");
            }
        }
        [Command("alogin", GreedyArg = true)]
        public void cmd_Alogin(Player player, string pass)
        {
            
            if (Main.Players1[player].Admin != null)
            {
                if (Main.Players1[player].Admin._IsLogin) return;
                return;
            }
            if (pass.Length < 5 || pass.Length > 11) return;
            Admin model = Main.Admins.Find(c => c.AccountId == Main.Players1[player].Account.Id);
            if (model != null)
            {
                if(pass == model.Password)
                {
                    player.SendChatMessage($"Вы успешно авторизировались как администратор {model.Lvl} уровня");
                    Main.Players1[player].Admin = model;
                    Main.Players1[player].Admin._IsLogin = true;
                }
            }
        }
    }
}
