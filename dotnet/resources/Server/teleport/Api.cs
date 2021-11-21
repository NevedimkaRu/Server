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
        //todo Обнулять дрифт каунтер при телепорте
        public static void CreateTeleport(Player player, string name, string discription)
        {
            string player_pos = JsonConvert.SerializeObject(player.Position);
            Teleport model = new Teleport(name, discription, player.Position);

            MySql.Query($"INSERT INTO `teleport`(`Name`, `Discription`, `Position`) VALUES ('{model.Name}','{model.Discription}','{player_pos}')");

            player.SendChatMessage($"Вы создали телепорт:[{model.Id}] {model.Name} - {model.Discription} - {model.Position}");

             Main.Teleports.Add(model);

        }
        [ServerEvent(Event.ResourceStart)]
        public static void LoadTeleports()
        {
            DataTable dt = MySql.QueryRead("SELECT * FROM `teleport`");
            if (dt == null || dt.Rows.Count == 0)
            {
                return;
            }

            foreach (DataRow row in dt.Rows)
            {
                Teleport model = new Teleport();
                model.LoadByDataRow(row);
                if(model.ColShapePos != null)
                {
                    model._ColShape = NAPI.ColShape.CreateCylinderColShape(model.ColShapePos, model.ColShapeRange, model.ColShapeHeight,0);
                    model._ColShape.OnEntityEnterColShape += (shape, player) =>
                    {
                        if (utils.Check.GetPlayerStatus(player, utils.Check.PlayerStatus.Spawn))
                        {
                            Main.Players1[player].TeleportId = model.Id;
                            player.SendChatMessage($"Вы вошли в {model.Name}");
                        }
                    };                
                    model._ColShape.OnEntityExitColShape += (shape, player) =>
                    {
                        if (utils.Check.GetPlayerStatus(player, utils.Check.PlayerStatus.Spawn))
                        {
                            Main.Players1[player].TeleportId = -1;
                            player.SendChatMessage($"Вы вышли из {model.Name}");
                        }
                    };
                }
                Main.Teleports.Add(model);
            }
            NAPI.Util.ConsoleOutput("Teleports load");
        }
        public static bool SmoothTeleport(Player player, float x, float y, float z, float rot)
        {
            Vector3 position = new Vector3(x, y, z);
            if (player.Vehicle != null)
            {
                Vehicle veh = player.Vehicle;
                player.Vehicle.Rotation = new Vector3(player.Vehicle.Rotation.X, player.Vehicle.Rotation.Y, rot);
                player.Vehicle.Position = new Vector3(position.X, position.Y, position.Z + 1.5f);
                /*player.Position = position;
                player.Rotation = new Vector3(player.Rotation.X, player.Rotation.Y, rot);
                player.SetIntoVehicle(veh, 0);*/
            }
            else
            {
                player.Position = position;
                player.Rotation = new Vector3(player.Rotation.X, player.Rotation.Y, rot);
            }
            return true;
        }
        public static void PlayerTeleported(Player player)
        {
            if (!Main.Players1.ContainsKey(player)) return;
            Main.Players1[player].State = PlayerModel.States.Default;
            player.SendChatMessage("default");
        }

        public static void TeleportTo(Player player, int tpId)
        {
            if (Main.Players1[player].State == PlayerModel.States.Teleporting) return;
            Main.Players1[player].State = PlayerModel.States.Teleporting;
            player.SendChatMessage("teleporting");
            model.Teleport teleport = Main.Teleports.Find(c => c.Id == tpId);
            if (teleport != null)
            {
                utils.Trigger.ClientEvent(player, "trigger_Teleport", teleport.Position);
            }
        }
    }
}
