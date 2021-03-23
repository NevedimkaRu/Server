using System;
using System.Collections.Generic;
using System.Text;

namespace cs_packages.model
{
    public class Clan
    {
    }
    public class ClanClient
    {
        public string Title { get; set; }
        public ClanSettings Settings { get; set; }
        public List<ClanMembers> Members;
        public List<ClanRank> Ranks;
    }
    public class ClanRank
    {
        public int ClanId { get; set; }
        public int Rank { get; set; }
        public string RankTitle { get; set; }
    }
    public class ClanMember 
    {
        public int CharacterId { get; set; }
        public int ClanId { get; set; }
        public int Rank { get; set; } // 0 ранг - лидер
    }
    public class ClanMembers
    {
        public string Name { get; set; }
        public int CharacterId { get; set; }
        public int Rank { get; set; }
    }
    public class ClanSettings
    {

    }
}
