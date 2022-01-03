using System;
using System.Data;
using GTANetworkAPI;
using Server.model;
using Server.utils;

namespace Server.game
{
    //todo Интерфейс, на 0 чекпоинте сделать 3d текст и можно чекпоинт, который показывает направление, проверка на выход из транспорта
    public class DriftTrack : Script
    {
        [ServerEvent(Event.ResourceStart)]
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
                model.LoadByDataRow(row);
                model._Blip = NAPI.Blip.CreateBlip(611, model.Positions[0], 1.0f, 0, name: $"Дрифт зона: {model.Name}");
                NAPI.Blip.SetBlipShortRange(model._Blip, true);
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
            if (!Check.GetPlayerStatus(player, Check.PlayerStatus.Spawn)) return;
            if (player.Vehicle == null) return;
            if (Main.Players1[player].Track == null)
            {
                foreach (Traks traks in Main.Traks)
                {
                    if (colShape == traks._ColShapes[0])
                    {
                        if (Math.Abs(player.Vehicle.Rotation.Z) > Math.Abs(traks.Rotation) + 90 || Math.Abs(player.Vehicle.Rotation.Z) < Math.Abs(traks.Rotation) - 90) return;
                        Main.Players1[player].Track = traks;
                        Main.Players1[player].CurrentTrackIndex = 0;
                        NAPI.Task.Run(() =>
                        {
                            player.TriggerEvent("trigger_SetTrackRoute", Main.Players1[player].Track.Positions[1],
                                1, 
                                Main.Players1[player].Track.TimeLimit, 
                                Main.Players1[player].Track.Positions[2],
                                1);
                        });
                        int score = character.Record.GetPlayerMapRecord(player, 1, traks.Id);
                        if (score != 0)
                        {
                            player.SendChatMessage($"Ваш рекорд на трассе {traks.Name}: {score}");
                        }                        
                        
                        break;
                    }
                }
                return;
            }
            if(Main.Players1[player].Track != null)
            {
                if(colShape == Main.Players1[player].Track._ColShapes[Main.Players1[player].Track._ColShapes.Count - 1] 
                    && Main.Players1[player].CurrentTrackIndex == Main.Players1[player].Track._ColShapes.Count - 2)
                {
                    player.TriggerEvent("trigger_SetTrackRoute", 
                        Main.Players1[player].Track.Positions[Main.Players1[player].Track._ColShapes.Count - 1], 
                        999, 
                        -1,
                        Main.Players1[player].Track.Positions[Main.Players1[player].Track._ColShapes.Count - 1],
                        2);
                    
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
                            player.TriggerEvent("trigger_SetTrackRoute", 
                                Main.Players1[player].Track.Positions[Main.Players1[player].CurrentTrackIndex + 1], 
                                38,
                                0,
                                Main.Players1[player].Track.Positions[Main.Players1[player].CurrentTrackIndex + 1],
                                0);
                        });
                        
                    }
                    else
                    {
                        NAPI.Task.Run(() =>
                        {
                            player.TriggerEvent("trigger_SetTrackRoute", 
                                Main.Players1[player].Track.Positions[Main.Players1[player].CurrentTrackIndex + 1], 
                                1,
                                0,
                                Main.Players1[player].Track.Positions[Main.Players1[player].CurrentTrackIndex + 2],
                                0);
                        });
                            
                    }
                    
                }
            }
        }
        public static void SetPlayerTrackRecord(Player player, int score)
        {
            character.Record.CheckPlayerMapRecord(player, Main.Players1[player].Track.Id, 1, score);
            Main.Players1[player].Track = null;
            Main.Players1[player].CurrentTrackIndex = -1;
        }

        public static void TimeLost(Player player)
        {
            Main.Players1[player].Track = null;
            Main.Players1[player].CurrentTrackIndex = -1;
        }
    }
}
