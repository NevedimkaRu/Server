using GTANetworkAPI;
using Server.constants;
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
            Main.Players1[player].IsSpawn = true;
        }

        public static async Task LoadCharacter(Player player, int accountId)
        {
            Character character = new Character();
            if (await character.LoadByOtherFieldAsync("AccountId", $"{accountId}"))
            {
                Main.Players1[player].Character = character;

                player.Name = character.Name + "[" + player.Id + "]";
                game.DriftTrack.LoadPlayerTrackScore(player);
                await vehicle.Api.LoadPlayerVehice(player);
                customization.Api.LoadCustomization(player, character.Id);
                Main.Players1[player].Clothes = await clothes.Api.LoadPlayerClothes(player);
                if(Main.Players1[player].Clothes == null)
                {
                    Main.Players1[player].Clothes = clothes.Api.CreatePlayerDefaultClothes(player);
                    clothes.Api.SetDefaultPlayerClothes(player, Main.Players1[player].Clothes);
                }
                else
                {
                    clothes.Api.SetDefaultPlayerClothes(player, Main.Players1[player].Clothes);
                }
                Main.Players1[player].Records = await Record.LoadPlayerRecords(player);
                Main.Players1[player].Titles = await Title.LoadCharacterTitle(player);
                Main.Players1[player].Achievement = await achievements.Api.LoadPlayerAchievementsAsync(player);
                if (Main.Players1[player].Titles != null)
                {
                    if(Main.Players1[player].Titles.Find(c => c.TitleId == Main.Players1[player].Character.Title) != null)
                    {
                        player.SetSharedData(SharedData.TITLE,Main.Titles[Main.Players1[player].Character.Title].Title);
                    }
                }
                Main.Players1[player].Mute = await admin.Mute.CheckMuteStatus(character.Id);
                if (Main.Players1[player].Mute != null)
                {
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
        public static void TakePlayerMoney(Player player, int money)
        {
            if (!utils.Check.GetPlayerStatus(player, utils.Check.PlayerStatus.Spawn)) return;
            Main.Players1[player].Character.Money -= money;
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
