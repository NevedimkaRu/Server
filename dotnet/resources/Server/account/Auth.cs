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

        [ServerEvent(Event.PlayerDisconnected)]
        public void OnPlayerDisconnected(Player player)
        {
            var account_api = new Api();
            account_api.SaveAccount(player).Wait();
        }
        [Command("login", GreedyArg = true)]
        public void cmd_Login(Player player, string name, string password)
        {
            if (name.Length <= 0 || password.Length <= 0)
            {
                player.SendChatMessage("Введите Nick/Name и пароль.");
                return;
            }

            Api.LoginAccount(player, name, password);
        }
        [Command("register", GreedyArg = true)]
        public void cmd_Register(Player player, string name, string password)
        {
            if (name.Length <= 0 || password.Length <= 0)
            {
                player.SendChatMessage("Введите Nick/Name и пароль.");
                return;
            }

            Api.CreateAccount(player, name, password);
        }

        [Command("stats")]
        public void cmd_Stats(Player player)
        {
            player.SendChatMessage(
                $"Name:{Main.Players[player].Username} " +
                $"\n Password: {Main.Players[player].Password}" +
                $"\n DriftScore: {Main.Players[player].DriftScore}"
                );
        }
    }
}
