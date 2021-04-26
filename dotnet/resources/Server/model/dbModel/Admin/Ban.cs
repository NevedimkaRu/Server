using System;
using System.Collections.Generic;
using System.Text;

namespace Server.model
{
    public class Ban : DB_Tables
    {
        public int AccountId { get; set; }
        public DateTime BanDate { get; set; }
        public DateTime UnBanDate { get; set; }
        public bool Permanent { get; set; }
        public string Reason { get; set; }

    }
}
