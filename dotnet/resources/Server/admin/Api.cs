using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Server.model;

namespace Server.admin
{
    public class Api : Script
    {
        [ServerEvent(Event.ResourceStart)]
        public void LoadAdmins()
        {
            DataTable dt = MySql.QueryRead(
                "SELECT admin. * , character.Name " +
                "FROM `admin` " +
                "JOIN `character` ON admin.AccountId = `character`.AccountId");
            if (dt == null || dt.Rows.Count == 0)
            {
                return;
            }

            foreach (DataRow row in dt.Rows)
            {
                Admin model = new Admin();
                model.Id = Convert.ToInt32(row["Id"]);
                model.AccountId = Convert.ToInt32(row["AccountId"]);
                model.Lvl = Convert.ToInt32(row["Lvl"]);
                model.Password = Convert.ToString(row["Password"]);
                model._CharName = Convert.ToString(row["Name"]);
                Main.Admins.Add(model);
            }
        }

        public static void AddAdmin(int id, int lvl, string name = null)
        {
            DataTable dt = MySql.QueryRead($"select name from `character` where AccountId = {id}");

            Admin model = new Admin();
            model.AccountId = id;
            model.Lvl = lvl;
            model._CharName = dt.Rows[0]["Name"].ToString();
            if(name != null)
            {
                model._CharName = name;
            }
            model.Insert();
            Main.Admins.Add(model);
        }
        public static void RemoveAdmin(int id)
        {
            Admin model = Main.Admins.Find(c => c.AccountId == id);
            Main.Admins.Remove(model);
            model.Delete();
        }

        public static bool GetAccess(Player player, int lvl)
        {
            if(!utils.Check.GetPlayerStatus(player, utils.Check.PlayerStatus.Spawn)) return false;
            if(Main.Players1[player].Admin == null) return false;
            if(!Main.Players1[player].Admin._IsLogin) return false;
            if(Main.Players1[player].Admin.Lvl < lvl) return false;

            return true;
        }

        public static void SendAdminMessage(string text)
        {
            foreach(Player player in NAPI.Pools.GetAllPlayers())
            {
                if(GetAccess(player, 1))
                {
                    player.SendChatMessage($"[A]{text}");
                }
            }
        }
    }
}
