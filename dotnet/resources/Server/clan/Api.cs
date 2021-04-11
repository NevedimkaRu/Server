using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Server.model;

namespace Server.clan
{
    public class Api : Script
    {
        [ServerEvent(Event.ResourceStart)]
        public void LoadClans()
        {
            DataTable dt = MySql.QueryRead("SELECT clan.*, clanrank.* FROM `clan` Left join `clanrank` on clan.Id = clanrank.ClanId");
            if (dt == null || dt.Rows.Count == 0)
            {
                return;
            }

            foreach (DataRow row in dt.Rows)
            {
                Clan model = new Clan();
                model.Id = Convert.ToInt32(row["Id"]);
                model.Title = Convert.ToString(row["Title"]);

                ClanRank rank = new ClanRank();
                rank.Rank = Convert.ToInt32(row["Rank"]);
                rank.RankTitle = Convert.ToString(row["RankTitle"]);

                if (Main.Clans.ContainsKey(model.Id))
                {
                    Main.Clans[model.Id]._Ranks.Add(rank);
                    continue;
                }
                Main.Clans.Add(model.Id, model);
                Main.Clans[model.Id]._Ranks.Add(rank);
                GetClanMembers(model.Id);
            }
            
        }

        public static List<ClanMembers> GetClanMembers(int clanid)
        {
            DataTable dt = MySql.QueryRead($"select character.Name, clanmember.CharacterId, clanmember.Rank from clanmember join `character` on character.Id = clanmember.CharacterId where ClanId = {clanid}");
            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }

            List<ClanMembers> members = new List<ClanMembers>();


            foreach (DataRow row in dt.Rows)
            {
                ClanMembers clan = new ClanMembers();
                clan.CharacterId = Convert.ToInt32(row["CharacterId"]);
                clan.Rank = Convert.ToInt32(row["Rank"]);
                clan.Name = Convert.ToString(row["Name"]);
                
                members.Add(clan);

            }

            return members;
        }

        public void AddClanRank(int clanid, int rankLvl, string rankTitile)
        {
            ClanRank rank = new ClanRank();
            rank.ClanId = clanid;
            rank.Rank = rankLvl;
            rank.RankTitle = rankTitile;
            rank.Insert();
            Main.Clans[clanid]._Ranks.Add(rank);
        }

        [Command("addrank", GreedyArg = true)]
        public void cmd_AddRank(Player player, string rank, string title)
        {
            if(Main.Players1[player].Clan.Rank != 0) 
            {
                player.SendChatMessage("Только лидер клана может добавлять ранги");
                return;
            }
            AddClanRank(Main.Players1[player].Clan.Id, Convert.ToInt32(rank), title);
            player.SendChatMessage($"Вы добавили в клан {Main.Clans[Main.Players1[player].Clan.ClanId].Title} новый ранг: {title}");
        }

        [Command("ranks", GreedyArg = true)]
        public void cmd_RankList(Player player)
        {
            if (Main.Players1[player].Clan == null) return;
            foreach(ClanRank rank in Main.Clans[Main.Players1[player].Clan.ClanId]._Ranks)
            {
                player.SendChatMessage($"ID[{rank.Rank}] {rank.RankTitle}");
            }
        }

        [Command("dim")]
        public void cmd_Dimension(Player player)
        {
            player.SendChatMessage($"Dimension: {player.Dimension}");
        }
    }
}
