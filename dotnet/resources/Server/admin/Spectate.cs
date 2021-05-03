using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.admin
{
    public class Spectate : Script
    {
        [Command("sp",GreedyArg = true)]
        public void cmd_Spectate(Player player, string playerid)
        {
            if (!Api.GetAccess(player, 1)) return;
            int id = Convert.ToInt32(playerid);
            SpectatePlayer(player, id);
        }
        public static void SpectatePlayer(Player player, int id)
        {
            if (id < 0 || id > NAPI.Server.GetMaxPlayers())
            {
                player.SendChatMessage("Игрок с таким Id не найден");
            }
            Player target = utils.Check.GetPlayerByID(id);
            if (target != null)
            {
                if (target != player)
                {
                    if (Main.Players1.ContainsKey(target))
                    {
                        if(Main.Players1[player].Admin._Spectate.Active == false)
                        { // Не сохраняем новые данные о позиции, если мы уже в режиме слежки
                            Main.Players1[player].Admin._Spectate.Position = player.Position;
                            Main.Players1[player].Admin._Spectate.Dimension = player.Dimension;
                        }
                        else NAPI.ClientEvent.TriggerClientEvent(player, "trigger_Spectate", null, false); // Если уже за кем-то SPшит и потом на другюго, то сначала deattach
                        player.SetSharedData("INVISIBLE", true); // Ваша переменная с Вашей системы инвизов, чтобы игроки не видели ника над головой
                        Main.Players1[player].Admin._Spectate.Active = true;
                        Main.Players1[player].Admin._Spectate.TargetId = target.Value;
                        player.Transparency = 0; // Сначала устанавливаем игроку полную прозрачность, а только потом телепортируем к игроку
                        player.Dimension = target.Dimension;
                        player.Position = new Vector3(target.Position.X, target.Position.Y, (target.Position.Z + 3)); // Сначала телепортируем к игроку, чтобы он загрузился
                        NAPI.Task.Run(() => {
                            player.TriggerEvent("trigger_Spectate", target, true);
                        },500);
                        //utils.Trigger.ClientEvent(player, "trigger_Spectate", target, true); // И только потом аттачим админа к игроку
                        player.SendChatMessage("Вы наблюдаете за " + target.Name + " [ID: " + target.Value + "].");
                    }
                    else player.SendChatMessage("Игрок под данным ID еще не авторизовался.");
                }
            }
            else player.SendChatMessage("Игрок под ID " + id + " отсутствует.");
        }
    }
}
