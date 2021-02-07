using GTANetworkAPI;
using Server.model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.game.GameEvents
{
    class Api : Script
    {

        public static GameEvent GE = new GameEvent();
        public static List<MapGameEvent> MapList = new List<MapGameEvent>();


        public static void ResourceStart()
        {
            LoadMaps();
            LoadGameEvents();
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

        public static void LoadMaps()
        {
            //ColShape cs = NAPI.ColShape.Create3DColShape(new Vector3(826.4169f, -2946.8687f, -20.906034f), new Vector3(1202.3827f, -3069.0847f, 30.8862033f), 0);
            ColShape cs = NAPI.ColShape.CreateSphereColShape(new Vector3(1000f, - 3000f, 5), 250f);
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
                SendMessageToGameEventMember("Мероприятие начнётся через 10 секунд, если хотите отказатся введите /outevent");
                NAPI.Task.Run(ActivateGameEvent, delayTime: 10000);
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
                    Player player = GE.Players[i];
                    player.Position = GE._Map.PlayerPositions[i];
                    Vehicle veh = NAPI.Vehicle.CreateVehicle(VehicleHash.Elegy2, GE._Map.PlayerPositions[i], GE._Map.Rotation, new Color(0, 255, 100), new Color(0));
                    NAPI.Task.Run(() => 
                    {
                        player.SetIntoVehicle(veh, 0);
                    }, delayTime: 1000 );
                }
            }
        }


        [Command("initdriftevent")]
        public void cmd_initDriftEvent(Player player)
        {
            NAPI.Chat.SendChatMessageToAll("Началось мероприятие " + GE.Name + " Введите /driftevent чтобы принять участие");
            GE.IsActive = true;
        }

        [Command("driftevent")]
        public void cmd_drifrevent(Player player)
        {
            if (GE.IsActive)
            {

                if (GE.Players.Count <= GE.MaxPlayers)
                {
                    GE.Players.Add(player);
                    NAPI.Chat.SendChatMessageToPlayer(player, "Вы согласились принять участие в " + GE.Name + ", дожидаемся остальных игроков.");
                    NAPI.Chat.SendChatMessageToPlayer(player, "Количество участников: " + (GE.Players.Count));
                    NAPI.Chat.SendChatMessageToPlayer(player, "Минимально необходимое количество участников: " + (GE.MinPlayers));
                    TryToStartEvent();
                }
                else {
                    NAPI.Chat.SendChatMessageToPlayer(player, "Мест пока нету, как и системы очереди, но вы держитесь, она пилится");
                }
            }
        }

        [Command("outevent")]
        public void cmd_outevent(Player player)
        {
            if (!GE.IsActive)
            {
                NAPI.Chat.SendChatMessageToPlayer(player, "Вы не являетесь участником мероприятия");
            }
            else {
                GE.Players.Remove(player);
                NAPI.Chat.SendChatMessageToPlayer(player, "Вы отказались от участия");
            }
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
