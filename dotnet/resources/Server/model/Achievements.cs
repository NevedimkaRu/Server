using System;
using System.Collections.Generic;
using System.Text;

namespace Server.model
{
    public class Achievements
    {
        public string Name { get; }
        public List<AchievementsTasks> Tasks;
        public List<AchievementsRewards> Rewards;

        public Achievements(string Name, /*string Discription,*/ List<AchievementsTasks> Tasks, List<AchievementsRewards> Rewards)
        {
            this.Name = Name;
            this.Tasks = Tasks;
            this.Rewards = Rewards;
        }
    }
    public class AchievementsTasks
    {
        public string Name { get; set; }
        public int Progress { get; set; }

        public AchievementsTasks(string Name, int Progress)
        {
            this.Name = Name;
            this.Progress = Progress;
        }
    }

    public class AchievementsRewards
    {
        public enum RewardType 
        { 
            Money,
            Vehicle,
            Title
        };

        public RewardType Type { get; set; }
        public string Name { get; set; }
        public int Ammount { get; set; }
        public AchievementsRewards(RewardType Type, string Name)
        {
            this.Type = Type;
            this.Name = Name;
            this.Ammount = 10000;
        }
        public AchievementsRewards(RewardType Type, int Ammount)
        {
            this.Type = Type;
            this.Name = null;
            this.Ammount = Ammount;
        }
    }
}
