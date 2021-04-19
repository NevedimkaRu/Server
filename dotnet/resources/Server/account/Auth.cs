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
        public void LoginAccount(Player player, string login, string password) 
        {
            Api.LoginAccount(player, login, password);
        }

        [RemoteEvent("remote_register")]
        public void RegisterAccount(Player player, string login, string password, string characterName) 
        {
            Api.CreateAccount(player, login, password, characterName);
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

            //Api.CreateAccount(player, name, password);
        }

        [Command("createcharacter", GreedyArg = true)]
        public void cmd_CreateCharacter(Player player, string name)
        {
            if (name.Length <= 4) 
            {
                player.SendChatMessage("Имя персонажа должно состять минимум из 4 символов");
                return;
            }
            character.Api.CreateCharacter(player, name);
        }
        
        [Command("stats")]
        public void cmd_Stats(Player player)
        {
            player.SendChatMessage(
                $"UserName:{Main.Players1[player].Account.Username} - {player.Id}" +
                $"\n Password: {Main.Players1[player].Account.Password}"  +
                $"\n Name: {Main.Players1[player].Character.Name}"  +
                $"\n DriftScore: {Main.Players1[player].Character.DriftScore}"  +
                $"\n Money: {Main.Players1[player].Character.Money}"  +
                $"\n Level: {Main.Players1[player].Character.Level}"
                );
        }
    }
}
