using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using GTANetworkAPI;
using Newtonsoft.Json;
using Server.model;

namespace Server.teleport
{
    public class Api : Script
    {
        public static void CreateTeleport(Player player, string name, string discription)
        {
            string player_pos = JsonConvert.SerializeObject(player.Position);
            TeleportModel model = new TeleportModel(name, discription, player.Position);

            MySql.Query($"INSERT INTO `teleports`(`Name`, `Discription`, `Position`) VALUES ('{model.Name}','{model.Discription}','{player_pos}')");

            player.SendChatMessage($"Вы создали телепорт:[{model.Id}] {model.Name} - {model.Discription} - {model.Position}");

            LoadTeleports();

        }
        public static void LoadTeleports()
        {
            DataTable dt = MySql.QueryRead("SELECT * FROM `teleports`");
            if (dt == null || dt.Rows.Count == 0)
            {
                return;
            }

            foreach (DataRow row in dt.Rows)
            {
                int teleportid = Convert.ToInt32(row["Id"]);
                TeleportModel model = new TeleportModel(teleportid,
                    Convert.ToString(row["Name"]),
                    Convert.ToString(row["Discription"]),
                    JsonConvert.DeserializeObject<Vector3>(row["Position"].ToString()));

                
                if(!Main.Teleports.ContainsKey(teleportid)) Main.Teleports.Add(teleportid, model);
                NAPI.Util.ConsoleOutput($"{model.Id} - {model.Name} - {model.Discription} - {model.Position}");
            }
            NAPI.Util.ConsoleOutput("Teleports load");
        }

        [ServerEvent(Event.ResourceStart)]
        public static void onResourceStart()
        {
            LoadTeleports();
        }
    }
}
