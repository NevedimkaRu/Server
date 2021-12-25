﻿using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
namespace Server.remoteEvents
{
    class DriftTrack : Script
    {

        [RemoteEvent("remote_GetPlayerScore")]
        public void Remote_GetPlayerScore(Player player, int score)
        {
            game.DriftTrack.SetPlayerTrackScore(player, score);
        }

        [RemoteEvent("remote_TimeLost")]
        public void Remote_TimeLost(Player player)
        {
            game.DriftTrack.TimeLost(player);
        }
    }
}
