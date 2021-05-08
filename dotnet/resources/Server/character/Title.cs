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
    }
}
