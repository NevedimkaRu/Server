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
            Teleport model = new Teleport(name, discription, player.Position);

            MySql.Query($"INSERT INTO `teleport`(`Name`, `Discription`, `Position`) VALUES ('{model.Name}','{model.Discription}','{player_pos}')");

            player.SendChatMessage($"Вы создали телепорт:[{model.Id}] {model.Name} - {model.Discription} - {model.Position}");

            LoadTeleports();

        }

        /*[ServerEvent(Event.ResourceStart)]
        public static void CreateTeleportTest()
        {
            Teleport model = new Teleport();
            model.Name = "test";
            model.Discription = "test23213";

            model.SetId(5);
            model.Discription = "test2";
            model.Name = "Порт ЛС2 тест";
            model.Update("Name,Discription");
            model.SetId(5);

            NAPI.Util.ConsoleOutput("================================");
            NAPI.Util.ConsoleOutput(model.Id.ToString() + " | " + model.Name + " | " + model.Discription);
            NAPI.Util.ConsoleOutput("================================");
        }*/

        public static void LoadTeleports()
        {
            var tp = new Teleport();

            DataTable dt = MySql.QueryRead("SELECT * FROM `teleport`");
            if (dt == null || dt.Rows.Count == 0)
            {
                return;
            }

            foreach (DataRow row in dt.Rows)
            {
                int teleportid = Convert.ToInt32(row["Id"]);
                Teleport model = new Teleport(teleportid,
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
