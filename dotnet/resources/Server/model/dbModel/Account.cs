﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Server.model
{
    public class Account : DB_Tables
    {
        
        public string Username { get; set; } = null;
        public string Password { get; set; } = null;
    }
}
