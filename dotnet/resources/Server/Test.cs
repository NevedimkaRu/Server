using System;
using System.IO;
using System.Collections.Generic;
using GTANetworkAPI;
using Newtonsoft.Json;
using System.Data;
using Server.model;

namespace Server
{
    public class Test : Script
    {
        public const bool Debug = true;
        Vehicle curVeh = null;

        [ServerEvent(Event.ResourceStart)]
        public void OnResourceStart()
        {
            NAPI.Util.ConsoleOutput("Всё ок!");
            /*ColShape colShape = NAPI.ColShape.Create3DColShape(
                new Vector3(428.24582f, 1196.2463f, 300.75458f),
                new Vector3(100, 100, 100));
            
            //ColShape colShape = NAPI.ColShape.Create2DColShape(-428.24582f, 1196.2463f, 100, 100);
            colShape.OnEntityEnterColShape += (shape, player) =>
            {
                player.SendChatMessage("Enter");
            };
            colShape.OnEntityExitColShape += (shape, player) =>
            {
                player.SendChatMessage("Exit");
            };*/
        }
        [Command("veh")]
        public void cmd_CreateVehicle(Player player)
        {
            if (!Debug) return;
            Vector3 player_pos = player.Position;
            player.SetIntoVehicle(NAPI.Vehicle.CreateVehicle(VehicleHash.Elegy2, player_pos, 2f, new Color(0, 255, 100), new Color(0), "МУР", 255, false, true, player.Dimension), 0);
        }
        Vehicle vehhh;
        
        [Command("test")]
        public void cmd_Test(Player player)
        {
            Vector3 player_pos = player.Position;
            Vector3 secondpos = player.Position;
            secondpos.X -= 10.95f;
            secondpos.Y -= 15.25f;
            var dir =  player_pos - secondpos;
            var angle = -Math.Atan2(dir.Y, dir.X) * (180.0f / 3.1416f);
            vehhh = NAPI.Vehicle.CreateVehicle(VehicleHash.Elegy2, secondpos, Convert.ToSingle(angle), new Color(0, 255, 100), new Color(0), "МУР", 255, false, true, player.Dimension);
        }
        [ServerEvent(Event.Update)]
        public void OnUpdate()
        {
            if(vehhh != null)
            {
                Vector3 dir =  vehhh.Position - utils.Check.GetPlayerByID(0).Position;
                double angle = -Math.Atan2(dir.X, dir.Y) * (180 / 3.1416f) + 180;
                vehhh.Rotation = new Vector3(0,0, angle);
            }
        }
        [Command("vehh", GreedyArg = true)]
        public void cmd_CreateVehicleHash(Player player, VehicleHash hash)
        {
            if (!Debug) return;
            Vector3 player_pos = player.Position;
            player.SetIntoVehicle(NAPI.Vehicle.CreateVehicle(hash, player_pos, 2f, new Color(0, 255, 100), new Color(0)), 0);
        }
        [Command("vehhh", GreedyArg = true)]
        public void cmd_CreateVehicle1(Player player, string hasn)
        {
            if(curVeh != null)
            {
                curVeh.Delete();
            }
            var hash = NAPI.Util.GetHashKey(hasn);
            //Vector3 player_pos = new Vector3(-2039.3885f, -369.91492f, 47.552048f);
            //float rot = -67.184616f;
            Vector3 player_pos = new Vector3(-375.3224f, -126.40259f, 37.954063f);
            float rot = 62.35309f;
            curVeh = NAPI.Vehicle.CreateVehicle(hash, player_pos, rot, 0, 0);
            curVeh.PrimaryColor = 13;
            curVeh.SecondaryColor = 13;
            //player.SetIntoVehicle(NAPI.Vehicle.CreateVehicle(hash, player_pos, 2f, 0, 0), 0);
        }
        [Command("vehhash")]
        public void cmd_Vehhash(Player player)
        {
            if (!Debug) return;
            foreach (string names in Enum.GetNames(typeof(VehicleHash)))
            {
                player.SendChatMessage($"{names}");
            }
        }
        [Command("sv1")]
        public void cmd_SetVelocytyX(Player player, int vlX, int vlY)
        {
            if (!Debug) return;

            player.TriggerEvent("trigger_SetVelocity", vlX, vlY);
        }

        [Command("save", "Use /save [Position Name]", GreedyArg = true)]
        public void cmd_SavePosition(Player player, string PosName = "No Set")
        {
            if (!Debug) return;
            var pos = (player.IsInVehicle) ? player.Vehicle.Position : player.Position;
            var rot = (player.IsInVehicle) ? player.Vehicle.Rotation : player.Rotation;

            using (var stream = File.AppendText("SavePos.txt"))
            {
                if (player.IsInVehicle)
                {
                    NAPI.Notification.SendNotificationToPlayer(player, "~g~In car ~w~postion saved with name ~r~" + PosName, true);
                    stream.WriteLine("IN VEH || " + PosName + ":" + pos.X + ", " + pos.Y + ", " + pos.Z + "    Rot:    " + rot.Z);
                    stream.Close();
                }
                else
                {
                    NAPI.Notification.SendNotificationToPlayer(player, "~g~On foot ~w~position saved with name ~r~" + PosName, true);
                    stream.WriteLine("ON FOOT|| " + PosName + ":" + pos.X + ", " + pos.Y + ", " + pos.Z + "    Rot:    " + rot.Z);
                    stream.Close();
                }
            }
        }
        [Command("rot")]
        public void cmd_Rot(Player player)
        {
            player.SendChatMessage(player.Vehicle.Rotation.ToString());
        }
        [Command("createtp", GreedyArg = true)]
        public void cmd_CreateTeleport(Player player, string name, string discription)
        {
            if (!Debug) return;
            teleport.Api.CreateTeleport(player, name, discription);
        }
        [Command("tp", GreedyArg = true)]
        public void cmd_Teleport(Player player, string teleportid)
        {
            int id = Convert.ToInt32(teleportid);
            model.Teleport teleport = Main.Teleports.Find(c => c.Id == id);
            if (teleport != null)
            {
                utils.Trigger.ClientEvent(player, "trigger_Teleport", teleport.Position);
            }
        }
        [Command("settime")]
        public static void cmd_setTime(Player player, int hours, int minutes, int seconds)
        {
            if (!Debug) return;
            NAPI.World.SetTime(hours, minutes, seconds);
        }
        [Command("rain")]
        public static void CMD_setWeather1(Player player)
        {
            if (!Debug) return;
            NAPI.World.SetWeather("RAIN");
        }
        [Command("smog")]
        public static void CMD_setWeather2(Player player)
        {
            if (!Debug) return;
            NAPI.World.SetWeather("SMOG");
        }
        [Command("clear")]
        public static void CMD_setWeather3(Player player)
        {
            if (!Debug) return;
            NAPI.World.SetWeather("CLEAR");
        }

        [Command("weather")]
        public static void CMD_setWeatherCustom(Player player, string weather)
        {
            if (!Debug) return;
            NAPI.World.SetWeather(weather);
        }

        [Command("mod", GreedyArg = true)]
        public static void cmd_SetMod(Player player, string modeType, string modeIndex)
        {
            try
            {
                if (player.Vehicle != null)
                {
                    player.Vehicle.SetMod(Convert.ToInt32(modeType), Convert.ToInt32(modeIndex));
                    player.SendChatMessage($"{modeType} - {modeIndex}");
                }

            }
            catch(Exception ex)
            {

            }
        }

        [Command("ipl", GreedyArg = true)]
        public static void cmd_RequestIpl(Player player, string ipl)
        {
            //NAPI.World.RequestIpl(ipl);
            
            Server.utils.Other.RequestPlayerIpl(player, ipl);
        }
        [Command("cc")]
        public static void cmd_ClearChat(Player player)
        {
            for (int i = 0; i < 20; i++)
            {
                NAPI.Chat.SendChatMessageToAll(null);
            }
        }

        [Command("setcolor", GreedyArg = true)]
        public static void cmd_SetColor(Player player, string primary, string second)
        {
            if (player.Vehicle == null) return;
            player.Vehicle.PrimaryColor = Convert.ToInt32(primary);
            player.Vehicle.SecondaryColor = Convert.ToInt32(second);
        }
        [Command("setclothes", GreedyArg = true)]
        public static void cmd_SetClothes(Player player, string slot, string drawable, string texture)
        {
            Dictionary<int, ComponentVariation> clothDictionary = new Dictionary<int, ComponentVariation>();
            clothDictionary.Add(Convert.ToInt32(slot), new ComponentVariation { Drawable = Convert.ToInt32(drawable), Texture = Convert.ToInt32(texture) });
            //NAPI.Player.SetPlayerClothes(player, clothDictionary);
            player.SetClothes(clothDictionary);
            player.SendChatMessage(NAPI.Util.GetHashKey("radmir_clothes").ToString());
        }
        [RemoteEvent("SaveCam")]
        public static void SaveCam(Player player, string position, string rotation)
        {
            string pos = position.Replace(",", ".");
            string rot = rotation.Replace(",", ".");
            using (var stream = File.AppendText("SaveCam.txt"))
            {
                NAPI.Notification.SendNotificationToPlayer(player, "~g~Камера сохранена~r~", true);
                stream.WriteLine($"Pos: {pos} - Rot: {rot}");
                stream.Close();
            }
        }

        [Command("point")]
        public void cmd_SetVelocytyX(Player player, int playerId)
        {
            Player p = utils.Check.GetPlayerByID(playerId);
            if (p.Vehicle != null)
            {
                player.TriggerEvent("trigger_pointCamAtVeh", p.Vehicle.Handle);
            }
        }

        [Command("tpc")]
        public void cmd_TPC(Player player)
        {
            if (Main.Players1[player].CarId != 0 && Main.Veh.ContainsKey(Main.Players1[player].CarId))
            {
                player.SetIntoVehicle(Main.Veh[Main.Players1[player].CarId]._Veh, 0);
            }
        }


        [Command("goto")]
        public void cmd_Goto(Player player, int playerId)
        {
            Player p = utils.Check.GetPlayerByID(playerId);
            player.Position = p.Position;
        }
        [Command("scol")]
        public void cmd_SetVehicleColor(Player player, int r, int g, int b)
        {
            if (player.Vehicle == null) return;
            player.Vehicle.CustomPrimaryColor = new Color(r, g, b);
        }

        [Command("scolt")]
        public void cmd_SetVehicleColorType(Player player, int type)
        {
            //int a = Convert.ToInt32(new Color(10, 10, 10));
            player.Vehicle.PrimaryPaint = new VehiclePaint(type);
        }
    }
}
