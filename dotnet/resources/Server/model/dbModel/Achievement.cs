using System;
using System.Collections.Generic;
using System.Text;

namespace Server.model
{
    public class Achievement : DB_Tables
    {
        public int AchievmentId { get; set; }
        
        public int CharacterId { get; set; }
        public Dictionary<int, int> Progress { get; set; } = new Dictionary<int, int>();
        public bool IsCompleted { get; set; } = false;
    }
}
