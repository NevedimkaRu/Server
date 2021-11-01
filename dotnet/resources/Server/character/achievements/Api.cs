using GTANetworkAPI;
using Server.model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Server.character.achievements
{
    public class Api : Script
    {
        public static async Task<Dictionary<int, Achievement>> LoadPlayerAchievementsAsync(Player player)
        {
            DataTable dt = await MySql.QueryReadAsync($"SELECT * FROM `achievement` WHERE `CharacterId` = {Main.Players1[player].Character.Id}");
            if (dt == null || dt.Rows.Count == 0)
            {
                return new Dictionary<int, Achievement>();
            }

            Dictionary<int, Achievement> ach = new Dictionary<int, Achievement>();

            foreach (DataRow row in dt.Rows)
            {
                Achievement model = new Achievement();
                model.LoadByDataRow(row);
                ach.Add(model.AchievmentId, model);
            }
            return ach;
        }

        public static void GivePlayerAchProgress(Player player, int achievement, int task, int progress)
        {
            if (!Main.Players1.ContainsKey(player)) return;
            if (!Main.Players1[player].IsSpawn) return;
            if (Main.Players1[player].Achievement.ContainsKey(achievement))
            {
                if (Main.Players1[player].Achievement[achievement].IsCompleted) return;
                if (!Main.Players1[player].Achievement[achievement].Progress.ContainsKey(task)) 
                    Main.Players1[player].Achievement[achievement].Progress.Add(task, 0);
                else if (Main.Players1[player].Achievement[achievement].Progress[task] 
                    >= Main.Achievment[achievement].Tasks[task].Progress) return;//Если уже выполнена задача
                if ((Main.Players1[player].Achievement[achievement].Progress[task] + progress) >= Main.Achievment[achievement].Tasks[task].Progress)
                {
                    Main.Players1[player].Achievement[achievement].Progress[task] = Main.Achievment[achievement].Tasks[task].Progress;
                    player.SendChatMessage($"Вы выполнили задачу: \"{Main.Achievment[achievement].Tasks[task].Name}\"");
                    if(Main.Achievment[achievement].Tasks.Count > 1)
                    {
                        int completedTasks = 0;
                        for (int i = 0; i < Main.Achievment[achievement].Tasks.Count; i++)
                        {
                            if(Main.Players1[player].Achievement[achievement].Progress[task] >= Main.Achievment[achievement].Tasks[i].Progress)
                            {
                                completedTasks++;
                            }
                        }
                        if(completedTasks == Main.Achievment[achievement].Tasks.Count)
                        {
                            Main.Players1[player].Achievement[achievement].IsCompleted = true;
                            Main.Players1[player].Achievement[achievement].UpdateAsync();
                            player.SendChatMessage("Вы выполнили достижение: " + Main.Achievment[achievement].Name);//todo изменить на уведомление
                            GivePlayerAchReward(player, achievement);
                        }
                    }
                    else
                    {
                        Main.Players1[player].Achievement[achievement].IsCompleted = true;
                        Main.Players1[player].Achievement[achievement].UpdateAsync();
                        player.SendChatMessage("Вы выполнили достижение: " + Main.Achievment[achievement].Name);
                        GivePlayerAchReward(player, achievement);
                    }
                }
                else Main.Players1[player].Achievement[achievement].Progress[task] += progress;

            }
            else
            {
                Achievement ach = new Achievement();
                ach.CharacterId = Main.Players1[player].Character.Id;
                ach.AchievmentId = achievement;
                if (progress >= Main.Achievment[achievement].Tasks[task].Progress)
                {
                    player.SendChatMessage($"Вы выполнили задачу: \"{Main.Achievment[achievement].Tasks[task].Name}\"");
                    ach.Progress[task] = Main.Achievment[achievement].Tasks[task].Progress;
                    if (Main.Achievment[achievement].Tasks.Count == 1)
                    {
                        ach.IsCompleted = true;
                        player.SendChatMessage("Вы выполнили достижение: " + Main.Achievment[achievement].Name);
                        GivePlayerAchReward(player, achievement);
                    }
                }
                else
                {
                    ach.Progress.Add(task, progress);
                }
                ach.Id = ach.Insert();
                Main.Players1[player].Achievement.Add(achievement, ach);
            }
        }
        private static void GivePlayerAchReward(Player player, int achievement)
        {
            string rewardText = "Вы получили в награду: ";
            foreach(var reward in Main.Achievment[achievement].Rewards)
            {
                
                switch (reward.Type)
                {
                    case AchievementsRewards.RewardType.Money:
                        {
                            character.Api.GivePlayerMoney(player, reward.Ammount);
                            rewardText += $"{reward.Ammount}$";
                            break;
                        }
                    case AchievementsRewards.RewardType.Vehicle:
                        {
                            vehicle.Api.AddVehicle(Main.Players1[player].Character.Id, reward.Name);
                            rewardText += $"Автомобиль {reward.Name}";
                            break;
                        }
                    case AchievementsRewards.RewardType.Title:
                        {
                            character.Title.GivePlayerTitle(player, Convert.ToInt32(reward.Ammount));
                            rewardText += $"Титул: \"{Main.Titles[reward.Ammount].Title}\" ";
                            break;
                        }
                }
            }
        }
    }
}
