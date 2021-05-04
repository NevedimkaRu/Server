using System;
using System.Collections.Generic;
using System.Text;

namespace Server.model
{
    public class Admin : DB_Tables
    {
        public int AccountId { get; set; }
        public int Lvl { get; set; }
        public string Password { get; set; } = null;
        public bool _IsLogin { get; set; } = false;
        public string _CharName { get; set; }
        public Spectate _Spectate = new Spectate();
    }
}
