using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using Server.account;

namespace Server.remoteEvents
{
    class Account : Script
    {
        [RemoteEvent("remote_login")]
        public async void Remote_LoginAccount(Player player, string login, string password, bool rememberMe)
        {
            await Api.LoginAccount(player, login, password, rememberMe);
        }

        [RemoteEvent("remote_register")]
        public async void Remote_RegisterAccount(Player player, string login, string password, string characterName)
        {
            await Api.CreateAccount(player, login, password, characterName);
        }
    }
}
