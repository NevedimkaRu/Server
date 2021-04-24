﻿using GTANetworkAPI;
using Server.model;
using System;
using System.Collections.Generic;
using System.Data;
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

                DataTable dt;
                dt = MySql.QueryRead("SELECT * FROM `tracksrecords`");
                
                if (dt != null || dt.Rows.Count != 0)
                {
                    TracksRecords model = new TracksRecords();

                    foreach (DataRow row in dt.Rows)
                    {
                        model.Id = Convert.ToInt32(row["Id"]);
                        model.CharacterId = Convert.ToInt32(row["CharacterId"]);
                        model.Score = Convert.ToInt32(row["Score"]);
                        model.TrackId = Convert.ToInt32(row["TrackId"]);
                        if(model.CharacterId == Main.Players1[player].Character.Id)
                        {
                            Main.Players1[player].TracksRecords.Add(model);
                        }
                    }
                }

                ClanMember clan = new ClanMember();
                if(clan.LoadByOtherId("CharacterId", Main.Players1[player].Character.Id))
                { 
                    Main.Players1[player].Clan = clan;
                    //Main.Clans[Main.Players1[player].Clan.ClanId]._Members.Add(Main.Players1[player]);
                }


                
                player.Name = character.Name + "[" + player.Id + "]";
                vehicle.Api.LoadPlayerVehice(player);
                customization.Api.LoadCustomization(player, character.Id);
            }
            else 
            {
                player.SendChatMessage("Создайте персонажа");
            }
        }

    }
}
