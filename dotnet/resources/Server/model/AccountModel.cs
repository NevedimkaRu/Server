using System;
using System.Collections.Generic;
using System.Text;

namespace Server.model
{
    public class AccountModel
    {

        //test
        public int Id { get; set; } = -1;
        public string Username { get; set; } = null;
        public string Password { get; set; } = null;
        public int DriftScore { get; set; } = 0;

    }
}
