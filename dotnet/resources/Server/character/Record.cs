using GTANetworkAPI;
using Server.model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Server.character
{
    public class Record : Script
    {
        public static async Task<List<model.Record>> LoadPlayerRecords(Player player)
        {
            DataTable dt = await MySql.QueryReadAsync($"SELECT * FROM `record` WHERE `CharacterId` = {Main.Players1[player].Character.Id}");
            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }
            List<model.Record> records = new List<model.Record>();
            foreach (DataRow row in dt.Rows)
            {
                model.Record record = new model.Record();
                record.LoadByDataRow(row);
                records.Add(record);
            }
            return records;
        }
        [Command("rlist")]
        public void cmd_ShowPlayerRecords(Player player)
        {
            if(Main.Players1[player].Records != null)
            {
                foreach(model.Record record in Main.Players1[player].Records)
                {
                    VehicleStore name = Main.VehicleStore.Find(c => c.Hash == record.VehicleHash);
                    string vehname = name == null ? NAPI.Vehicle.GetVehicleDisplayName((VehicleHash)NAPI.Util.GetHashKey(record.VehicleHash)) : name.Title;
                    player.SendChatMessage($"{record.Score} - {Main.Teleports.Find(c => c.Id == record.MapId).Name} - {vehname}");
                }
            }
        }
        public static void CheckPlayerMapRecord(Player player, int mapId, int score)
        {
            if(Main.Players1[player].Records == null)
            {
                model.Record record = new model.Record();
                record.CharacterId = Main.Players1[player].Character.Id;
                record.MapId = mapId;
                record.Score = score;
                record.Type = 0;
                if (Main.Veh.ContainsKey(Main.Players1[player].CarId))
                {
                    record.VehicleHash = Main.Veh[Main.Players1[player].CarId].ModelHash;
                }
                record.Id = record.Insert();
                Main.Players1[player].Records = new List<model.Record>();
                Main.Players1[player].Records.Add(record);
                player.SendChatMessage($"Вы установили новый личный рекорд {score} на трассе {Main.Teleports.Find(c => c.Id == mapId).Name}");
            }
            else
            {
                model.Record record = Main.Players1[player].Records.Find(c => c.MapId == mapId);
                if(record == null)
                {
                    record = new model.Record();
                    record.CharacterId = Main.Players1[player].Character.Id;
                    record.MapId = mapId;
                    record.Score = score;
                    record.Type = 0;
                    if (Main.Veh.ContainsKey(Main.Players1[player].CarId))
                    {
                        record.VehicleHash = Main.Veh[Main.Players1[player].CarId].ModelHash;
                    }
                    record.Id = record.Insert();
                    Main.Players1[player].Records.Add(record);
                    player.SendChatMessage($"Вы установили новый личный рекорд {score} на трассе {Main.Teleports.Find(c => c.Id == mapId).Name}");
                }
                else
                {
                    if(record.Score < score)
                    {
                        record.Score = score;
                        if(Main.Veh.ContainsKey(Main.Players1[player].CarId))
                        {
                            record.VehicleHash = Main.Veh[Main.Players1[player].CarId].ModelHash;
                        }
                        record.Update("Score,VehicleHash");
                        player.SendChatMessage($"Вы установили новый личный рекорд {score} на трассе {Main.Teleports.Find(c => c.Id == mapId).Name}"); 
                    }
                }

            }
        }
    }
}
