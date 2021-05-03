using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using Server;
using Server.account;

namespace Server.account
{
    class Auth : Script
    {

        /*[ServerEvent(Event.PlayerDisconnected)]
        public void OnPlayerDisconnected(Player player)
        {
            var account_api = new Api();
            account_api.SaveAccount(player).Wait();
        }*/



        [RemoteEvent("remote_login")]
        public async void LoginAccount(Player player, string login, string password) 
        {
             await Api.LoginAccount(player, login, password);
        }

        [RemoteEvent("remote_register")]
        public async void RegisterAccount(Player player, string login, string password, string characterName) 
        {
            await Api.CreateAccount(player, login, password, characterName);
        }
       
    }
}
