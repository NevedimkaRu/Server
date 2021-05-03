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

        public bool getByAccountId(int accountId)
        {
            DataTable db = MySql.QueryRead($"select * from `character` where AccountId = '{accountId}'");
            if (db == null || db.Rows.Count == 0)
            {
                return false;
            }
            DataRow row = db.Rows[0];
            this.Id = Convert.ToInt32(row["Id"]);
            this.AccountId = Convert.ToInt32(row["AccountId"]);
            this.Name = Convert.ToString(row["Name"]);
            this.DriftScore = Convert.ToInt32(row["DriftScore"]);
            this.Money = Convert.ToInt32(row["Money"]);
            this.Level = Convert.ToInt32(row["Level"]);

            return true;
        }
        public async Task<bool> getByAccountIdAsync(int accountId)
        {
            DataTable db = await MySql.QueryReadAsync($"select * from `character` where AccountId = '{accountId}'");
            if (db == null || db.Rows.Count == 0)
            {
                return false;
            }
            DataRow row = db.Rows[0];
            this.Id = Convert.ToInt32(row["Id"]);
            this.AccountId = Convert.ToInt32(row["AccountId"]);
            this.Name = Convert.ToString(row["Name"]);
            this.DriftScore = Convert.ToInt32(row["DriftScore"]);
            this.Money = Convert.ToInt32(row["Money"]);
            this.Level = Convert.ToInt32(row["Level"]);

            return true;
        }
    }
}
