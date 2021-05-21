using GTANetworkAPI;
using Server.model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Server.admin
{
    public class Ban : Script
    {
        public static void BanPlayer(Player player, string reason, int days = 0)
        {
            model.Ban model = new model.Ban();
            model.AccountId = Main.Players1[player].Account.Id;
            model.BanDate = DateTime.UtcNow.AddHours(3);
            model.Reason = reason;
            if (days == 0)
            {
                model.Permanent = true;
            }
            else
            {
                model.Permanent = false;
                model.UnBanDate = DateTime.UtcNow.AddDays(days).AddHours(3);
            }
            
            model.Insert();
            player.Kick();
        }

        public static bool UnBanPlayer(int accountId)
        {
            DataTable dt = MySql.QueryRead($"SELECT * FROM `ban` WHERE `AccountId` = {accountId}");
            if (dt == null || dt.Rows.Count == 0)
            {
                return false;
            }
            MySql.Query($"DELETE FROM `ban` WHERE `AccountId` = {accountId}");
            return true;
        }

        public static string BanPlayer(int accid, string reason, int days = 0)
        {
            DataTable dt = MySql.QueryRead($"SELECT `Name` FROM `character` WHERE `AccountId` = {accid}");
            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }
            model.Ban model = new model.Ban();
            model.AccountId = accid;
            model.BanDate = DateTime.UtcNow.AddHours(3);
            model.Reason = reason;
            if (days == 0)
            {
                model.Permanent = true;
            }
            else
            {
                model.Permanent = false;
                model.UnBanDate = DateTime.UtcNow.AddDays(days).AddHours(3);
            }
            MySql.Query($"DELETE FROM `ban` WHERE `AccountId` = {accid}");
            model.Insert();
            return dt.Rows[0]["Name"].ToString();
        }

        public static async Task<model.Ban> CheckBanStatus(int id)
        {
            DataTable dt = await MySql.QueryReadAsync($"SELECT * FROM `ban` WHERE `AccountId` = {id}");
            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }
            foreach (DataRow row in dt.Rows)
            {
                model.Ban model = new model.Ban();
                model.Id = Convert.ToInt32(row["Id"]);
                model.AccountId = Convert.ToInt32(row["AccountId"]);
                model.BanDate = Convert.ToDateTime(row["BanDate"]);
                model.UnBanDate = Convert.ToDateTime(row["UnBanDate"]);
                model.Reason = Convert.ToString(row["Reason"]);
                model.Permanent = Convert.ToBoolean(row["Permanent"]);
                if (DateTime.UtcNow.AddHours(3).CompareTo(model.UnBanDate) >= 0 && !model.Permanent)
                {
                    model.Delete();
                    return null;
                }
                return model;
            }
            return null;
        }

        [Command("banl", GreedyArg = true)]
        public void cmd_BanList(Player player)
        {
            DataTable dt = MySql.QueryRead("SELECT * FROM `ban`");
            if (dt == null || dt.Rows.Count == 0)
            {
                return;
            }

            foreach (DataRow row in dt.Rows)
            {
                model.Ban model = new model.Ban();
                model.Id = Convert.ToInt32(row["Id"]);
                model.AccountId = Convert.ToInt32(row["AccountId"]);
                model.BanDate = Convert.ToDateTime(row["BanDate"]);
                model.UnBanDate = Convert.ToDateTime(row["UnBanDate"]);
                model.Reason = Convert.ToString(row["Reason"]);
                if(DateTime.UtcNow.CompareTo(model.UnBanDate) <= 0)
                {
                    player.SendChatMessage($"[Забанен]{model.AccountId} {model.BanDate} {model.UnBanDate} {model.Reason}");
                }
                else
                {
                    player.SendChatMessage($"[Разбанен]{model.AccountId} {model.BanDate} {model.UnBanDate} {model.Reason}");
                }
                
            }

        }
    }
}
