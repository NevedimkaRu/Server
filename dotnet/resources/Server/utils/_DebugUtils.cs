using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.utils
{
    class _DebugUtils : Script
    {

        [RemoteEvent("remote_login_debug")]
        public void remoteLogin(Player player, String login, string password, bool rememberMe)
        {
            account.Api.LoginAccount(player, login, password, rememberMe);
        }

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
