using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;

namespace Server.utils
{
    public class Other : Script
    {
        public static void RequestPlayerIpl(Player player, string ipl)
        {
            NAPI.Task.Run(() =>
            {
                player.TriggerEvent("trigger_RequestPlayerIpl", ipl);
            });
        }
        public static void RemovePlayerIpl(Player player, string ipl)
        {
            NAPI.Task.Run(() =>
            {
                player.TriggerEvent("trigger_RemovePlayerIpl", ipl);
            });
        }

        public static void PlayerFadeScreen(Player player, int durationOut, int durationIn, int delay)
        {
            if (!Main.Players1.ContainsKey(player)) return;
            Trigger.ClientEvent(player, "trigger_FadeScreen", durationOut, durationIn, delay);
        }
    }
}
