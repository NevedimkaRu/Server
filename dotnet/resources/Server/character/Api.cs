using GTANetworkAPI;
using Server.model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server.character
{
    class Api : Script
    {

        public static async Task CreateCharacter(Player player, string name) {
            Character model = new Character(Main.Players1[player].Account.Id, name);
            model.Insert();
            await LoadCharacter(player, Main.Players1[player].Account.Id);
        }

        public static async Task LoadCharacter(Player player, int accountId)
        {
            Character character = new Character();
            if (await character.getByAccountIdAsync(accountId))
            {
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

                /*ClanMember clan = new ClanMember();
                if(clan.LoadByOtherId("CharacterId", Main.Players1[player].Character.Id))
                { 
                    Main.Players1[player].Clan = clan;
                }*/

                player.Name = character.Name + "[" + player.Id + "]";
                await vehicle.Api.LoadPlayerVehice(player);
                customization.Api.LoadCustomization(player, character.Id);
                List<model.Mute> muteModel = await admin.Mute.CheckMuteStatus(character.Id);
                if (muteModel != null)
                {
                    Main.Players1[player].Mute = muteModel;
                    Main.Players1[player].MuteTimer = new Timer(admin.Mute.tc, player, 0, 1000);
                }

                player.TriggerEvent("trigger_setMoneyLevelExp", character.Money, character.Level, character.Exp);

            }
            else 
            {
                player.SendChatMessage("Создайте персонажа");
            }
        }

        public static void GivePlayerExp(Player player, int exp)
        {
            if (!utils.Check.GetPlayerStatus(player, utils.Check.PlayerStatus.Spawn)) return;
            if (Main.Players1[player].Character.Level == 30) return;
            Main.Players1[player].Character.Exp += exp;
            int needexp = (Main.Players1[player].Character.Level + 1) * 20000;
            if(Main.Players1[player].Character.Exp >= needexp)
            {
                if(Main.Players1[player].Character.Level == 29)
                {
                    player.SendChatMessage("Подзравляем, вы дошли до максимального уровня персонажа!");
                }
                Main.Players1[player].Character.Level += 1;
                Main.Players1[player].Character.Exp = Main.Players1[player].Character.Exp - needexp;
                player.SendChatMessage($"Вы перешли на {Main.Players1[player].Character.Level} уровень.");
                player.TriggerEvent("trigger_SetLevel", Main.Players1[player].Character.Level);
            }
            player.TriggerEvent("trigger_SetExp", Main.Players1[player].Character.Exp);
        }

        public static void GivePlayerMoney(Player player, int money)
        {
            if (!utils.Check.GetPlayerStatus(player, utils.Check.PlayerStatus.Spawn)) return;
            Main.Players1[player].Character.Money += money;
            player.TriggerEvent("trigger_SetMoney", Main.Players1[player].Character.Money);
        }
        public static void GivePlayerDriftScore(Player player, int score)
        {
            if (!utils.Check.GetPlayerStatus(player, utils.Check.PlayerStatus.Spawn)) return;
            Main.Players1[player].Character.DriftScore += score;
        }
        [Command("stats")]
        public void cmd_Stats(Player player)
        {
            player.SendChatMessage(
                $"Name: {Main.Players1[player].Character.Name} " +
                $"DriftScore: {Main.Players1[player].Character.DriftScore} " +
                $"Money: {Main.Players1[player].Character.Money} " +
                $"Level: {Main.Players1[player].Character.Level} ({Main.Players1[player].Character.Exp}/{(Main.Players1[player].Character.Level + 1) * 20000})"
                );
        }

    }
}
