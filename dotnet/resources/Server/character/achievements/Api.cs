using GTANetworkAPI;
using Server.model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.character.achievements
{
    public class Api : Script
    {
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
                            player.SendChatMessage("Вы выполнили достижение: " + Main.Achievment[achievement].Name);//todo изменить на уведомление
                            GivePlayerAchReward(player, achievement);
                        }
                    }
                    else
                    {
                        Main.Players1[player].Achievement[achievement].IsCompleted = true;
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
                    ach.Progress[task] = Main.Achievment[achievement].Tasks[task].Progress;
                    if (Main.Achievment[achievement].Tasks.Count == 1)
                    {
                        ach.IsCompleted = true;
                        ach.Id = ach.Insert();
                        player.SendChatMessage("Вы выпорнили достижение: " + Main.Achievment[achievement].Name);
                        GivePlayerAchReward(player, achievement);
                    }
                }
                else
                {
                    ach.Id = ach.Insert();
                    ach.Progress.Add(task, progress);
                }
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
