using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using GTANetworkAPI;
using Newtonsoft.Json;
using Server.model;
namespace Server.game
{

    public class DriftEvent : Script
    {
        static List<Vector3> Positions = new List<Vector3>
        {
            new Vector3(-398.9926, 806.9512, 222.63179),
            new Vector3(282.09293, 832.8769, 191.4665),
            new Vector3(312.68573, 1001.1451, 209.8595),
            new Vector3(-342.85498, 944.4129, 232.06987),
            new Vector3(-342.85498, 944.4129, 232.06987),
            new Vector3(-342.85498, 944.4129, 232.06987),
            new Vector3(-342.85498, 944.4129, 232.06987),
            new Vector3(-342.85498, 944.4129, 232.06987)
        };

        public static void ResourceStart()
        {
            DataTable dt = MySql.QueryRead("SELECT * FROM `traks`");
            if (dt == null || dt.Rows.Count == 0)
            {
                return;
            }

            Traks model = new Traks();

            foreach (DataRow row in dt.Rows)
            {
                model.Id = Convert.ToInt32(row["Id"]);
                model.Name = Convert.ToString(row["Name"]);
                model.Positions = utils.Parser.ParseToListVector3(Convert.ToString(row["Positions"]));
                //NAPI.Util.ConsoleOutput(model.Positions[0].ToString());
                model.Reward = Convert.ToInt32(row["Reward"]);
                model.RewardScore = Convert.ToInt32(row["RewardScore"]);
                model.TimeLimit = Convert.ToInt32(row["TimeLimit"]);

                ColShape colShape;
                for (int i = 0; i < model.Positions.Count; i++ )
                {
                    colShape = NAPI.ColShape.CreateSphereColShape(model.Positions[i], 10.0f);
                    model._ColShapes.Add(colShape);
                }
                Main.Traks.Add(model);
            }
            
        }
        [ServerEvent(Event.PlayerEnterColshape)]
        public void PlayerEnterColshape(ColShape colShape, Player player)
        {
            if (Main.Players1[player].Track == null)
            {
                
                foreach (Traks traks in Main.Traks)
                {
                    if (colShape == traks._ColShapes[0])
                    {
                        Main.Players1[player].Track = traks;
                        Main.Players1[player].CurrentTrackIndex = 0;
                        NAPI.Task.Run(() =>
                        {
                            player.TriggerEvent("trigger_SetTrackRoute", Main.Players1[player].Track.Positions[1], 1);
                            
                        });
                        NAPI.Task.Run(() =>
                        {
                            player.TriggerEvent("trigger_ResetDriftScore");//todo сделать оттельный счётчик
                        });

                        player.SendChatMessage("ПОГНАЛИ НАХУЙ!");

                        if (utils.Track.GetPlayerTrackRecordByTrackId(player, Main.Players1[player].Track.Id) == -1)
                        {
                            utils.Track.InsertTrackRecordByTrackId(player, traks.Id);
                        }

                        TracksRecords track;
                        track = utils.Track.GetTrackRecordsByTrackId(player, traks.Id);

                        
                        player.SendChatMessage($"Вам рекорд на страссе {traks.Name}:" +
                            $" {track.Score}");
                        break;
                    }
                }
                return;
            }
            if(Main.Players1[player].Track != null)
            {
                if(colShape == Main.Players1[player].Track._ColShapes[Main.Players1[player].Track._ColShapes.Count - 1] 
                    && Main.Players1[player].CurrentTrackIndex == Main.Players1[player].Track._ColShapes.Count - 1)
                {
                    player.TriggerEvent("trigger_SetTrackRoute", Main.Players1[player].Track.Positions[Main.Players1[player].Track._ColShapes.Count - 1], 999);

                    NAPI.Task.Run(() =>
                    {
                        player.TriggerEvent("trigger_GetPlayerScore");//Получаем очков за дрифт
                    });
                    
                    player.SendChatMessage("OMEDOTO KUZAIMASU!");
                    
                }
                else if(colShape == Main.Players1[player].Track._ColShapes[Main.Players1[player].CurrentTrackIndex + 1])
                {
                    Main.Players1[player].CurrentTrackIndex++;
                    player.SendChatMessage(Main.Players1[player].CurrentTrackIndex.ToString());
                    if(Main.Players1[player].CurrentTrackIndex == Main.Players1[player].Track._ColShapes.Count - 2)
                    {
                        NAPI.Task.Run(() =>
                        {
                            player.TriggerEvent("trigger_SetTrackRoute", Main.Players1[player].Track.Positions[Main.Players1[player].CurrentTrackIndex + 1], 38);
                        });
                        
                    }
                    else
                    {
                        NAPI.Task.Run(() =>
                        {
                            player.TriggerEvent("trigger_SetTrackRoute", Main.Players1[player].Track.Positions[Main.Players1[player].CurrentTrackIndex + 1], 1);
                        });
                            
                    }
                    
                }
            }
        }
        [RemoteEvent("remote_GetPlayerScore")]
        public void Remote_GetPlayerScore(Player player, int score)
        {
            int playerscore = utils.Track.GetPlayerTrackRecordByTrackId(player, Main.Players1[player].Track.Id);
            if (Main.Players1.ContainsKey(player))
            {
                if(playerscore == -1)
                {
                    player.SendChatMessage("Если вы видите это сообщение, то всё пошло по пизде.");
                }
                else if(playerscore > score)//Если рекодр больше набранных очков
                {
                    Main.Players1[player].Track = null;
                    Main.Players1[player].CurrentTrackIndex = -1;
                }
                else{
                    utils.Track.UpdateTrackRecordByTrackId(player, Main.Players1[player].Track.Id, score);
                    Main.Players1[player].Track = null;
                    Main.Players1[player].CurrentTrackIndex = -1;
                }
            }
        }
    }
}
