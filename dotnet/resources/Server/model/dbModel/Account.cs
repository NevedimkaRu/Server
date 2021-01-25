using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Server.model
{
    public class Account : DB_Tables
    {
        
        public string Username { get; set; } = null;
        public string Password { get; set; } = null;

        public bool GetByUserName(string userName)
        {
            DataTable db = MySql.QueryRead($"select * from account where username = '{userName}'");
            if (db == null || db.Rows.Count == 0)
            {
                return false;
            }
            DataRow row = db.Rows[0];
            this.Id = Convert.ToInt32(row["Id"]);
            this.Username = Convert.ToString(row["UserName"]);
            this.Password = Convert.ToString(row["Password"]);

            return true;
        }

    }
}
