using GTANetworkAPI;
using Server.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.game.GameEvents
{
    class Api : Script
    {

        public static GameEvent GE = new GameEvent();
        public static List<MapGameEvent> MapList = new List<MapGameEvent>();

        [ServerEvent(Event.ResourceStart)]
        public static void ResourceStart()
        {
            LoadMaps();
            LoadGameEvents();
            //LoadColShapes();
        }

        //[ServerEvent(Event.PlayerEnterColshape)]
        //public void PlayerEnterColshape(ColShape colShape, Player player)
        //{
        //    foreach (MapGameEvent map in Main.MapGameEvents)
        //    {
        //        if (colShape == map.CShape)
        //        {
        //            NAPI.Chat.SendChatMessageToPlayer(player, "Вы вошли в зону " + map.Name);
        //            Main.Players1[player].CurrentMapGameEvent = map;
        //        }
        //    }
        //}

        //[ServerEvent(Event.PlayerExitColshape)]
        //public void PlayerExitColshape(ColShape colShape, Player player)
        //{
        //    if (colShape == Main.Players1[player].CurrentMapGameEvent.CShape)
        //    {
        //        NAPI.Chat.SendChatMessageToPlayer(player, "Вы вышли из зоны " + Main.Players1[player].CurrentMapGameEvent.Name);
        //        Main.Players1[player].CurrentMapGameEvent = null;
        //    }
        //}


        public static void LoadGameEvents()
        {
            GameEvent ge = new GameEvent()
            {
                GameType = GameEventConstants.GAME_EVENT_TYPE_DRIFT,
                MinPlayers = GameEventConstants.DRIFT_MIN_PLAYER,
                MaxPlayers = GameEventConstants.DRIFT_MAX_PLAYER,
                Name = "ДрифтБатл в доках",
                _Map = MapList[0],
                IsActive = false
            };
            GE = ge;
        }


        public static void LoadColShapes()
        {
            ColShape cs = NAPI.ColShape.Create2DColShape(825.0543f, -3069.2056f, 376.9623f, 124.0965f, 0);
            cs.OnEntityEnterColShape += (shape, player) =>
            {
                NAPI.Chat.SendChatMessageToPlayer(player, "Enter ColShape");
            };
            cs.OnEntityExitColShape += (shape, player) =>
            {
                NAPI.Chat.SendChatMessageToPlayer(player, "Leave ColShape");
            };
        }

        public static void LoadMaps()
        {
            //ColShape cs = NAPI.ColShape.Create3DColShape(new Vector3(826.4169f, -2946.8687f, -20.906034f), new Vector3(1202.3827f, -3069.0847f, 30.8862033f), 0);
            ColShape cs = NAPI.ColShape.CreateSphereColShape(new Vector3(1000f, -3000f, 5), 250f);
            cs.OnEntityEnterColShape += (shape, player) =>
            {
                NAPI.Chat.SendChatMessageToPlayer(player, "Вы вошли в зону ");
            };
            cs.OnEntityExitColShape += (shape, player) =>
            {
                NAPI.Chat.SendChatMessageToPlayer(player, "Вы вышли из зоны");
            };

            //ColShape cs = NAPI.ColShape.Create2DColShape(826.4169f, -2946.8687f, 375.9658f, -122.216f, 0);
            MapGameEvent map = new MapGameEvent()
            {
                Name = "Доки",
                MaxTime = 120,
                CShape = cs,
                PlayerPositions = new List<Vector3>()
                {
                    new Vector3(922.6224f, -3004.9324f, 5.2193694f),
                    new Vector3(931.85895f, -3010.9177f, 5.2198863f),
                    new Vector3(914.6646f, -3005.4727f, 5.21992f),
                    new Vector3(916.997f, -3010.421f, 5.219356f),
                    new Vector3(907.4528f, -3005.156f, 5.2199483f)
                },
                Rotation = -90f
            };

            MapList.Add(map);
        }

        public static void TryToStartEvent()
        {
            if (!GE.IsActive) return;
            if (GE.Players.Count >= GE.MinPlayers)
            {
                SendMessageToGameEventMember("Мероприятие начнётся через 3 секунд, если хотите отказатся введите /outevent");
                NAPI.Task.Run(ActivateGameEvent, delayTime: 3000);
            }
        }

        public static void ActivateGameEvent()
        {
            if (GE.Players.Count < GE.MinPlayers)
            {
                SendMessageToGameEventMember("Мероприятие отменено из-за нехватки участников");
                GE.Players.Clear();
                GE.IsActive = false;
            }
            else
            {
                NAPI.Chat.SendChatMessageToAll("Погнали Ёпта");
                for (int i = 0; i < GE.Players.Count; i++)
                {
                    int index = i;
                    int carid = GE.CarIds[GE.Players[index]];
                    if (Main.Veh[carid]._Veh != null) Main.Veh[carid]._Veh.Delete();
                    Main.Veh[carid]._Veh = NAPI.Vehicle.CreateVehicle(NAPI.Util.GetHashKey(Main.Veh[carid].ModelHash), GE._Map.PlayerPositions[index], GE._Map.Rotation, 0, 0);
                    //vehicle.Api.LoadVehicleInPos(GE.Players[index], carid, GE._Map.PlayerPositions[index], GE._Map.Rotation);
                    Main.Veh[carid]._Veh.SetSharedData("vehicleId", Main.Veh[carid].Id);
                }
                for (int i = 0; i < GE.Players.Count; i++)
                {
                    int index = i;
                    NAPI.Task.Run(() =>
                    {
                        GE.Players[index].TriggerEvent("startWarmUp", GE._Map.PlayerPositions[index], Main.Veh[GE.CarIds[GE.Players[index]]]._Veh.Handle, Main.Veh[GE.CarIds[GE.Players[index]]].Handling);
                    });
                }
                NAPI.Task.Run(() =>
                {
                    StartGameEvent();
                }, delayTime: 30000);
                
            }
        }

        private static void StartForPlayer(int i)
        {
            GE.Players[i].Position = GE._Map.PlayerPositions[i];
            NAPI.Task.Run(() => {
                GE.Players[i].SetIntoVehicle(Main.Veh[GE.CarIds[GE.Players[i]]]._Veh, 0);
            });
            NAPI.Task.Run(() =>
            {
                GE.Players[i].TriggerEvent("freezeCountdown", 10);
            });
        }

        public static void initDrifEvent()
        {
            NAPI.Chat.SendChatMessageToAll("Началось мероприятие " + GE.Name + " Введите /driftevent чтобы принять участие");
            GE.IsActive = true;
        }

        public static void ToDriftEvent(Player player, int CarId)
        {
            if (GE.IsActive)
            {

                if (GE.Players.Count <= GE.MaxPlayers)
                {
                    if (Main.Veh[CarId].OwnerId != Main.Players1[player].Character.Id)
                    {
                        NAPI.Chat.SendChatMessageToPlayer(player, "Это не ваша машина");
                        return;
                    }
                    Vehicle veh = Main.Veh[CarId]._Veh;
                    if (veh == null)
                    {
                        NAPI.Chat.SendChatMessageToPlayer(player, "Эта машина в резерве");
                        return;
                    }
                    GE.Players.Add(player);
                    GE.CarIds.Add(player, CarId);
                    NAPI.Chat.SendChatMessageToPlayer(player, "Вы согласились принять участие в " + GE.Name + ", дожидаемся остальных игроков.");
                    NAPI.Chat.SendChatMessageToPlayer(player, "Количество участников: " + (GE.Players.Count));
                    NAPI.Chat.SendChatMessageToPlayer(player, "Минимально необходимое количество участников: " + (GE.MinPlayers));
                    TryToStartEvent();
                }
                else
                {
                    NAPI.Chat.SendChatMessageToPlayer(player, "Мест пока нету, как и системы очереди, но вы держитесь, она пилится");
                }
            }
        }

        public void ResetEvent()
        {
            NAPI.Chat.SendChatMessageToAll("reset event");
            GE.ReadyPlayers.Clear();
            GE.Scores.Clear();
            TryToStartEvent();
        }

        public static void StartGameEvent()
        {
            foreach (Player player in GE.Players)
            {
                if (!GE.ReadyPlayers.Contains(player)) {
                    PlayerOutEvent(player);
                }    
            }

            foreach (Player player in GE.Players)
            {
                NAPI.Chat.SendChatMessageToPlayer(player, "Starting event");
                NAPI.Task.Run(() =>
                {
                    player.TriggerEvent("startDriftBattle");
                });
            }
           
        }

        public void InitFinishEvent() 
        {
            List<string> results = new List<string>();
            int place = 1;
            foreach (KeyValuePair<Player, int> score in GE.Scores.OrderByDescending(key => key.Value))
            {
                results.Add((place++) + " место: " + score.Key.Name + " - " + score.Value + " очков");
            }
            foreach (Player player in GE.Players)
            {
                NAPI.Chat.SendChatMessageToPlayer(player, "Результаты:");
                foreach (string s in results)
                {
                    NAPI.Chat.SendChatMessageToPlayer(player, s);
                }
            }

            NAPI.Task.Run(() => {
                ResetEvent();
            }, delayTime: 10000);
        }

        public static void PlayerOutEvent(Player player)
        {
            if (!GE.IsActive)
            {
                NAPI.Chat.SendChatMessageToPlayer(player, "Вы не являетесь участником мероприятия");
            }
            else
            {
                GE.RemovePlayer(player);
                NAPI.Chat.SendChatMessageToPlayer(player, "Вы отказались от участия");
            }
        }

        [RemoteEvent("remote_readyDriftBattle")]
        public void ReadyToEvent(Player player, object[] args)
        {
            GE.ReadyPlayers.Add(player);
        }

        [RemoteEvent("remote_sendScore")]
        public void SendScore(Player player, object[] args)
        {
            NAPI.Chat.SendChatMessageToPlayer(player, "Score sended");
            int score = Convert.ToInt32(args[0]);
            GE.Scores.Add(player, score);
            if (GE.Scores.Count == GE.Players.Count)
            {
                InitFinishEvent();
            }
        }

        [Command("initdriftevent")]
        public void cmd_initDriftEvent(Player player)
        {
            initDrifEvent();
        }

        [Command("driftevent", GreedyArg = true)]
        public void cmd_drifrevent(Player player, string carId)
        {
            ToDriftEvent(player, int.Parse(carId));
        }

        [Command("outevent")]
        public void cmd_outevent(Player player)
        {
            PlayerOutEvent(player);
        }

        [Command("goto", GreedyArg = true)]
        public void cmd_goto(Player player, string x, string y, string z)
        {
            float X = float.Parse(x);
            float Y = float.Parse(y);
            float Z = float.Parse(z);
            player.Position = new Vector3(X, Y, Z);
        }

        [Command("getmycoord")]
        public void cmd_getmycoord(Player player)
        {
            NAPI.Chat.SendChatMessageToPlayer(player, "Ваши координаты: X: " + player.Position.X + "Y: " + player.Position.Y + "Z: " + player.Position.Z);
        }

        public static void SendMessageToGameEventMember(string msg)
        {
            foreach (Player player in GE.Players)
            {
                NAPI.Chat.SendChatMessageToPlayer(player, msg);
            }
        }
    }
}
