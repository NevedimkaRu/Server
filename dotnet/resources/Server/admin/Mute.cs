using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server.admin
{
    public class Mute : Script
    {
        public static TimerCallback tc = new TimerCallback(MuteTimerCallback);
        public static async Task<List<model.Mute>> CheckMuteStatus(int characterid)
        {
            List<model.Mute> muteList = new List<model.Mute>();
            DataTable dt = await MySql.QueryReadAsync($"SELECT * FROM `mute` WHERE `CharacterId` = {characterid}");
            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }
            foreach (DataRow row in dt.Rows)
            {
                model.Mute model = new model.Mute();
                model.Id = Convert.ToInt32(row["Id"]);
                model.CharacterId = Convert.ToInt32(row["CharacterId"]);
                model.TimeLeft = Convert.ToInt32(row["TimeLeft"]);
                model.Type = Convert.ToInt32(row["Type"]);
                model.Reason = Convert.ToString(row["Reason"]);
                model.WhoMuted = Convert.ToString(row["WhoMuted"]);
                if (model.TimeLeft <= 0)
                {
                    model.Delete();
                    continue;
                }
                muteList.Add(model);
            }
            return muteList;
        }
        public static void MutePlayer(Player player, Player target, int minutes, int type, string reason)
        {
            model.Mute model = new model.Mute();
            model.CharacterId = Main.Players1[target].Character.Id;
            model.TimeLeft = minutes;
            model.Type = type;
            model.Reason = reason;
            model.WhoMuted = Main.Players1[player].Character.Name;
            model.Insert();
            
            if(Main.Players1[target].MuteTimer == null) Main.Players1[target].MuteTimer = new Timer(tc, target, 0, 1000);

            Main.Players1[target].Mute.Add(model);
        }

        [Command("mute", GreedyArg = true)]
        public void cmd_Mute(Player player, string id, string type, string minutes, string reason)
        {
            if (!Api.GetAccess(player, 1)) return;

            int _id = Convert.ToInt32(id);
            int _minutes = Convert.ToInt32(minutes);
            int _type = Convert.ToInt32(type);
            /*if (_minutes < 5 || _minutes > 60)
            {
                player.SendChatMessage("Количество минут от 5 до 60");
                return;
            }*/
            foreach (Player target in NAPI.Pools.GetAllPlayers())
            {
                if (target.Value == _id)
                {
                    if (!utils.Check.GetPlayerStatus(target, utils.Check.PlayerStatus.Spawn))
                    {
                        player.SendChatMessage("Игрок не авторизировался");
                        return;
                    }
                    string types;
                    if (_type == model.Mute.CHAT) types = "текстовый чат";
                    else if (_type == model.Mute.REPORT) types = "репорт";
                    else types = "голосовой чат";
                    NAPI.Chat.SendChatMessageToAll($"Администратор {Main.Players1[player].Character.Name} заблокировал игроку {Main.Players1[target].Character.Name} {types} на {_minutes} минут по причине: {reason}");
                    MutePlayer(player, target, _minutes, _type, reason);
                    return;
                }
            }
            player.SendChatMessage("Игрок не найден");
        }

        [ServerEvent(Event.ChatMessage)]
        public void OnPlayerChatMessage(Player player, string message)
        {
            if (!utils.Check.GetPlayerStatus(player, utils.Check.PlayerStatus.Spawn)) return;
            model.Mute muteModel = Main.Players1[player].Mute.Find(c => c.Type == model.Mute.CHAT);
            if (muteModel != null)
            {
                message = "EBAL";
                player.SendChatMessage($"У вас заблокирован доступ к чату. Осталось: {muteModel.TimeLeft}");
                return;
            }
            else
            {
                NAPI.Chat.SendChatMessageToAll($"{Main.Players1[player].Character.Name}[{player.Value}]: {message}");
            }
        }
        private static void MuteTimerCallback(object state)
        {
            Player player = (Player)state;
            if (!utils.Check.GetPlayerStatus(player, utils.Check.PlayerStatus.Spawn)) return;
            if(Main.Players1[player].Mute == null || Main.Players1[player].Mute.Count == 0)
            {
                Main.Players1[player].MuteTimer.Dispose();
                Main.Players1[player].MuteTimer = null;
                return;
            }
            foreach(model.Mute muteList in Main.Players1[player].Mute)
            {
                muteList.TimeLeft--;
                if (muteList.TimeLeft <= 0)
                {
                    
                    Main.Players1[player].Mute.Remove(muteList);
                    muteList.Delete();
                    string reason;
                    if (muteList.Type == model.Mute.CHAT) reason = "текстового чата";
                    else if (muteList.Type == model.Mute.REPORT) reason = "репорта";
                    else reason = "голосового чата";
                    player.SendChatMessage($"Истекло время отключения {reason}. Теперь вы снова можете им пользоваться.");
                    if (Main.Players1[player].Mute.Count == 0)
                    {
                        Main.Players1[player].MuteTimer.Dispose();
                        Main.Players1[player].MuteTimer = null;
                    }
                    break;
                }
            }
        }
    }
}
