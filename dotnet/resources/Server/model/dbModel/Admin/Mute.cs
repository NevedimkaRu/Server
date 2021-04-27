using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Server.model
{
    public class Mute : DB_Tables
    {
        public int CharacterId { get; set; }
        public int Type { get; set; }
        public int TimeLeft { get; set; }
        public string Reason { get; set; }
        public string WhoMuted { get; set; }

        public const int CHAT = 0;
        public const int VOICE = 1;
        public const int REPORT = 2;
    }
}
