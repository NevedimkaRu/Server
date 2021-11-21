using GTANetworkAPI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Server.model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Server.customization
{
    class Api : Script
    {
        public static void SaveCustomization(Player player, object[] args)
        {
            string str = args[0].ToString();
            JObject obj = JObject.Parse(str);
            Customization model = new Customization();
            if (model.LoadByOtherId("CharacterId", Main.Players1[player].Character.Id))
            {
                model.ParseClientData(obj);
                model.Update();
            }
            else
            {
                model.ParseClientData(obj);
                model.characterId = Main.Players1[player].Character.Id;
                model.Insert();
            }
            Main.Players1[player].Customization = model;
            model.SetToPlayer(player);
        }

        public static void GetCharacterCusumize(Player player)
        {
            Customization model = new Customization();
            if (model.LoadByOtherId("CharacterId", Main.Players1[player].Character.Id))
            {
                player.TriggerEvent("trigger_setCharacterCostumize", model);
            }
        }

        public static void LoadCustomization(Player player, int characterId)
        {
            Customization model = new Customization();
            model.LoadByOtherId("CharacterId", characterId);
            Main.Players1[player].Customization = model;
            model.SetToPlayer(player);
        }
    }
}
