using GTANetworkAPI;
using Server.model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Server.character
{
    class Title : Script
    {
        [ServerEvent(Event.ResourceStart)]
        public void LoadCharactersTitles()
        {
            DataTable dt = MySql.QueryRead("SELECT * FROM `titles`");
            if (dt == null || dt.Rows.Count == 0)
            {
                return;
            }
            foreach (DataRow row in dt.Rows)
            {
                Titles model = new Titles();
                model.LoadByDataRow(row);
                Main.Titles.Add(model.TitleId, model);
            }
        }

        public static async Task<List<CharacterTitle>> LoadCharacterTitle(Player player)
        {
            DataTable dt = await MySql.QueryReadAsync($"SELECT * FROM `charactertitle` WHERE `CharacterId` = {Main.Players1[player].Character.Id}");
            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }

            List<CharacterTitle> list = new List<CharacterTitle>();

            foreach (DataRow row in dt.Rows)
            {
                CharacterTitle model = new CharacterTitle();
                model.LoadByDataRow(row);
                list.Add(model);
            }
            return list;
        }
        public static void GivePlayerTitle(Player player, int titleId)
        {
            CharacterTitle title = new CharacterTitle();
            title.CharacterId = Main.Players1[player].Character.Id;
            title.TitleId = titleId;
            title.Id = title.Insert();
            Main.Players1[player].Titles.Add(title);
        }
        public static void AddTitle(string title)
        {
            Titles model = new Titles();
            model.TitleId = Main.Titles.Keys.Count + 1;
            model.Title = title;
            model.Id = model.Insert();
            
            Main.Titles.Add(model.TitleId, model);
        }

        [Command("addtitle", GreedyArg = true)]
        public void cmd_AddTitle(Player player, string title)
        {
            AddTitle(title);
            player.SendChatMessage($"Вы добавили титул: '{title}'");
        }

        [Command("givetitle")]
        public void cmd_GivePlayerTitle(Player player, int playerid, int titleid)
        {
            CharacterTitle title = new CharacterTitle();
            title.CharacterId = Main.Players1[utils.Check.GetPlayerByID(playerid)].Character.Id;
            title.TitleId = titleid;
            title.Id = title.Insert();
            Main.Players1[utils.Check.GetPlayerByID(playerid)].Titles.Add(title);
        }
        [Command("settitle")]
        public void cmd_SetTitle(Player player, int titleid)
        {
            if (Main.Players1[player].Titles != null)
            {
                if (Main.Players1[player].Titles.Find(c => c.TitleId == titleid) != null)
                {
                    Main.Players1[player].Character.Title = titleid;
                    player.SetSharedData("sd_Title", Main.Titles[Main.Players1[player].Character.Title].Title);
                }
            }
        }
    }
}
