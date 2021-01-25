using GTANetworkAPI;
using Server.model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.character
{
    class Api
    {

        public static void CreateCharacter(Player player, string name) {
            Character model = new Character(Main.Players1[player].Account.Id, name);
            model.Insert();
            LoadCharacter(player, Main.Players1[player].Account.Id);
        }


        public static void LoadCharacter(Player player, int accountId)
        {
            Character character = new Character();
            if (character.getByAccountId(accountId))
            {
                player.SendChatMessage($"Здравствуйте, {character.Name}");
                Main.Players1[player].Character = character;
                Main.Players1[player].IsSpawn = true;
                player.Name = character.Name + "[" + player.Id + "]";
                player.SetSharedData("IsSpawn", true);
            }
            else 
            {
                player.SendChatMessage("Создайте персонажа");
            }
        }

    }
}
