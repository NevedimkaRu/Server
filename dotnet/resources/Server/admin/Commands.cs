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
            if (lvl == 0)
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
            if (Main.Admins.Find(c => c.AccountId == id) != null)
            {
                player.SendChatMessage($"Аккаунт с ID {id} уже назначен администратором");
                return;
            }
            foreach (Player target in NAPI.Pools.GetAllPlayers())
            {
                if (Main.Players1[target].Account.Id == id)
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
                if (Main.Players1[target].Admin != null)
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
                if (pass == model.Password)
                {
                    player.SendChatMessage($"Вы успешно авторизировались как администратор {model.Lvl} уровня");
                    Main.Players1[player].Admin = model;
                    Main.Players1[player].Admin._IsLogin = true;
                }
            }
        }
        [Command("ban", GreedyArg = true)]
        public void cmd_Ban(Player player, string id, string days, string reason)
        {
            if (!Api.GetAccess(player, 1)) return;
            
            int _id = Convert.ToInt32(id);
            int _days = Convert.ToInt32(days);
            if (_id == player.Value)
            {
                player.SendChatMessage("Ты дурачок?");
                Api.SendAdminMessage($"{player.Name} попытался забанить себя.");
                return;
            }
            if (_days <= 0 || _days > 14)
            {
                player.SendChatMessage("Количество дней от 1 до 14");
                return;
            }
            foreach (Player target in NAPI.Pools.GetAllPlayers())
            {
                if (target.Value == _id)
                {
                    if (!utils.Check.GetPlayerStatus(target, utils.Check.PlayerStatus.Spawn))
                    {
                        player.SendChatMessage("Игрок не авторизировался");
                        return;
                    }
                    string dayys;
                    if (_days == 1) dayys = "день";
                    else if (_days >= 2 || _days <= 4) dayys = "дня";
                    else dayys = "дней";
                    NAPI.Chat.SendChatMessageToAll($"Администратор {Main.Players1[player].Character.Name} забанил игрока {Main.Players1[target].Character.Name} на {dayys}  по причине: {reason}");
                    Ban.BanPlayer(target, reason, _days);
                    return;
                }
            }
            player.SendChatMessage("Игрок не найден");
        }
        [Command("offban", GreedyArg = true)]
        public void cmd_OffLineBan(Player player, string accid, string days, string reason)
        {
            if (!Api.GetAccess(player, 1)) return;
            int _id = Convert.ToInt32(accid);
            int _days = Convert.ToInt32(days);
            if (_days <= 0 || _days > 14)
            {
                player.SendChatMessage("Количество дней от 1 до 14");
                return;
            }
            if (_id == Main.Players1[player].Account.Id)
            {
                player.SendChatMessage("Ты дурачок?");
                Api.SendAdminMessage($"{player.Name} попытался забанить себя.");
                return;
            }
            foreach (Player target in NAPI.Pools.GetAllPlayers())
            {
                if (utils.Check.GetPlayerStatus(target, utils.Check.PlayerStatus.Spawn))
                {
                    if (Main.Players1[target].Account.Id == _id)
                    {
                        player.SendChatMessage($"Игрок с ({_id})id аккаунта в сети: {Main.Players1[target].Character.Name}[{target.Value}]. Используйте /ban");
                    }
                }
            }
            string name = Ban.BanPlayer(_id, reason, _days);
            if (name == null)
            {
                player.SendChatMessage("Аккаунт с таким Id не найден");
                return;
            }
            string dayys;
            if (_days == 1) dayys = "день";
            else if (_days >= 2 || _days <= 4) dayys = "дня";
            else dayys = "дней";
            Api.SendAdminMessage($"Администратор {Main.Players1[player].Character.Name} забанил игрока {name} на {dayys} дня(ей) по причине: {reason}");
        }
        [Command("unban", GreedyArg = true)]
        public void cmd_UnBan(Player player, string accid)
        {
            int id = Convert.ToInt32(accid);
            if (!Api.GetAccess(player, 1)) return;
            if (Ban.UnBanPlayer(id))
            {
                Api.SendAdminMessage($"Администратор {player.Name} разбанил аккаунт ID[{id}]");
            }
            else
            {
                player.SendChatMessage($"У аккаунта с ID[{id}] нету бана или он не существует.");
            }
        }
        [Command("givescore")]
        public void cmd_GiveScore(Player player, int id, int score)
        {

            if (!Api.GetAccess(player, 1)) return;
            character.Api.GivePlayerExp(utils.Check.GetPlayerByID(id), score);
            Api.SendAdminMessage($"Администратор {Main.Players1[player].Character.Name} выдал игроку {Main.Players1[utils.Check.GetPlayerByID(id)].Character.Name}[{utils.Check.GetPlayerByID(id).Value}] {score} Exp");
        }
    }
}
