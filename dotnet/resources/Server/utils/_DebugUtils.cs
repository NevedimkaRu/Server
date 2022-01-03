using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.utils
{
    class _DebugUtils : Script
    {

        GameEvents.DriftBattleGameEvent db = new GameEvents.DriftBattleGameEvent();

        [RemoteEvent("remote_initDriftevent")]
        public void remoteInitDriftevent(Player player)
        {
            db.InitEvent();
        }

        [RemoteEvent("remote_driftEvent")]
        public void remoteDriftEvent(Player player, int carid)
        {
            db.AddPlayer(player, carid);
        }

    }
}
