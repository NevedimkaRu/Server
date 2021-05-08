using System;
using System.Collections.Generic;
using System.Text;

namespace Server.model
{
    public class CharacterTitle : DB_Tables
    {
        public int CharacterId { get; set; }
        public int TitleId { get; set; }
    }

    public class Titles : DB_Tables
    {
        public int TitleId { get; set; }
        public string Title { get; set; }
        public int Type { get; set; }
    }
}
