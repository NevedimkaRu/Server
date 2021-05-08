using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using Server.model;

namespace Server.admin
{
    public class Report : Script
    {
        public static bool CreateReport(Player player, string text)
        {
            if(Main.Reports.Find(c => c.Player == player) != null)
            {
                return false;
            }
            model.Report report = new model.Report();
            report.Player = player;
            report.ReportTime = DateTime.UtcNow.AddHours(3);
            report.Text = text;
            Main.Reports.Add(report);
            return true;
        }
        public static bool DeleteReport(Player player)
        {
            model.Report report = Main.Reports.Find(c => c.Player == player);
            if(report != null)
            {
                Main.Reports.Remove(report);
                return true;
            }
            return false;
        }

        [Command("report",GreedyArg = true)]
        public void cmd_Report(Player player, string text)
        {
            if(CreateReport(player, text))
            {
                player.SendChatMessage($"Вы отправили репорт: '{text}'.");
            }
            else
            {
                player.SendChatMessage("У вас уже есть активный репорт. Дождитесь ответа или удалите его!");
            }
        }
        [Command("delrep")]
        public void cmd_DeleteReport(Player player)
        {
            if(DeleteReport(player))
            {
                player.SendChatMessage("Вы удалили свой репорт.");
            }
            else
            {
                player.SendChatMessage("У вас нету активных репортов.");
            }
        }
        [Command("rl")]
        public void cmd_ReportList(Player player)
        {
            foreach(var list in Main.Reports)
            {
                player.SendChatMessage($"{list.Text} - {list.ReportTime} - {Main.Players1[list.Player].Character.Name}[{list.Player.Value}]");
            }
        }
        [Command("ans", GreedyArg = true)]
        public void cmd_Ans(Player player, string id, string text)
        {
            int _id = Convert.ToInt32(id);
            Player target = utils.Check.GetPlayerByID(_id);
            model.Report report = Main.Reports.Find(c => c.Player == target);
            if(report == null)
            {
                player.SendChatMessage("Этот игрок не писал в репорт.");
                return;
            }
            Main.Reports.Remove(report);
            NAPI.Chat.SendChatMessageToPlayer(target, $"{text}. Администратор {player.Name}");
            player.SendChatMessage($"Вы ответили игроку {target.Name}: '{text}'");
        }
    }
}
