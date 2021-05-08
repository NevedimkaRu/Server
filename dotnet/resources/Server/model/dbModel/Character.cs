using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Server.model
{
    public class Character : DB_Tables
    {
        public int AccountId { get; set; }
        public string Name { get; set; }
        public int DriftScore { get; set; }
        public int Money { get; set; }
        public int Level { get; set; }
        public int Exp { get; set; }
        public int Vip { get; set; }
        public int Title { get; set; }
        
        public Character()
        { 
        }
        public Character(int accountId, string name)
        {
            this.AccountId = accountId;
            this.Name = name;
            this.DriftScore = 0;
            this.Money = 0;
            this.Level = 0;
        }
    }
}
