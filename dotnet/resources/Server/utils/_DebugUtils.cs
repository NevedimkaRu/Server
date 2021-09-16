using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.utils
{
    class _DebugUtils : Script
    {
        [RemoteEvent("remote_initDriftevent")]
        public void remoteInitDriftevent(Player player)
        {
            game.GameEvents.Api.initDrifEvent();
        }

        [RemoteEvent("remote_driftEvent")]
        public void remoteDriftEvent(Player player, int carid)
        {
            game.GameEvents.Api.ToDriftEvent(player, carid);
        }

    }
}
