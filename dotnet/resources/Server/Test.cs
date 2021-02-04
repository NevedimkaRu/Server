using System;
using System.IO;
using GTANetworkAPI;

namespace Server
{
    public class Test : Script
    {
        public const bool Debug = true;

        [ServerEvent(Event.ResourceStart)]
        public void OnResourceStart()
        {
            NAPI.Util.ConsoleOutput("Всё ок!");
            game.DriftEvent.ResourceStart();
        }
        [Command("veh")]
        public void cmd_CreateVehicle(Player player)
        {
            if (!Debug) return;
            Vector3 player_pos = player.Position;
            player.SetIntoVehicle(NAPI.Vehicle.CreateVehicle(VehicleHash.Elegy2, player_pos, 2f, new Color(0, 255, 100), new Color(0),"МУР",255,false,true,player.Dimension),0);
        }
        [Command("vehh", GreedyArg = true)]
        public void cmd_CreateVehicleHash(Player player, VehicleHash hash)
        {
            if (!Debug) return;
            Vector3 player_pos = player.Position;
            player.SetIntoVehicle(NAPI.Vehicle.CreateVehicle(hash, player_pos, 2f, new Color(0, 255, 100), new Color(0)), 0);
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
        [Command("sv1", GreedyArg = true)]
        public void cmd_SetVelocytyX(Player player, string vlX, string vlY, string vlZ)
        {
            if (!Debug) return;
            object[] vl = new object[3];
            vl[0] = vlX;
            vl[1] = vlY;
            vl[2] = vlZ;

            player.TriggerEvent("setVelocity", vl);
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
        [Command("createtp",GreedyArg = true)]
        public void cmd_CreateTeleport(Player player, string name, string discription)
        {
            if (!Debug) return;
            teleport.Api.CreateTeleport(player, name, discription);
        }
        [Command("tp", GreedyArg = true)]
        public void cmd_Teleport(Player player, string teleportid)
        {
            int id = Convert.ToInt32(teleportid);
            if(Main.Teleports.ContainsKey(id))
            {
                player.Position = (Main.Teleports[id].Position);
            }
        }
        [Command("tl")]
        public void cmd_TeleportList(Player player)
        {
            if (!Debug) return;
            for (int a = 1; a != Main.Teleports.Count + 1; a++)
            {
                player.SendChatMessage($"[{Main.Teleports[a].Id}]{Main.Teleports[a].Name} - {Main.Teleports[a].Discription}");
            }
        }
        [Command("settime")]
        public static void cmd_setTime(Player player, int hours, int minutes, int seconds)
        {
            if (!Debug) return;
            NAPI.World.SetTime(hours, minutes, seconds);
        }
        [Command("rain")]
        public static void CMD_setWeather1(Player player, byte weather)
        {
            if (!Debug) return;
            NAPI.World.SetWeather("RAIN");
        }
        [Command("smog")]
        public static void CMD_setWeather2(Player player, byte weather)
        {
            if (!Debug) return;
            NAPI.World.SetWeather("SMOG");
        }
        [Command("clear")]
        public static void CMD_setWeather3(Player player, byte weather)
        {
            if (!Debug) return;
            NAPI.World.SetWeather("CLEAR");
        }
        [Command("mod", GreedyArg = true)]
        public static void cmd_SetMod(Player player, string modeType, string modeIndex)
        {
            if(player.Vehicle != null)
            {
                player.Vehicle.SetMod(Convert.ToInt32(modeType), Convert.ToInt32(modeIndex));
                player.SendChatMessage($"{modeType} - {modeIndex}");
            }
        }

        [Command("ipl", GreedyArg = true)]
        public static void cmd_RequestIpl(Player player, string ipl)
        {
            NAPI.World.RequestIpl(ipl);
        }
    }
}
