using System;
using System.Collections.Generic;
using System.Text;

namespace Server.model
{
    public class Clan : DB_Tables
    {
        public string Title { get; set; }
        public ClanSettings _Settings { get; set; }
        public List<ClanMembers> _Members = new List<ClanMembers>();
        public List<ClanRank> _Ranks = new List<ClanRank>();
    }

    public class ClanSettings : DB_Tables
    {

    }

    public class ClanMembers
    {
        public string Name { get; set; }
        public int CharacterId { get; set; }
        public int Rank { get; set; }
    }

    public class ClanMember : DB_Tables
    {
        public int CharacterId { get; set; }
        public int ClanId { get; set; }
        public int Rank { get; set; } // 0 ранг - лидер
    }
    public class ClanRank : DB_Tables
    {
        public int ClanId { get; set; }
        public int Rank { get; set; }
        public string RankTitle { get; set; }
    }
}
