using System;
using System.Collections.Generic;
using System.Text;

namespace Server.model
{
    public class Account : DB_Tables
    {
        
        public string Username { get; set; } = null;
        public string Password { get; set; } = null;

        //temp
        public bool IsLogged { get; set; } = false;

    }
}
