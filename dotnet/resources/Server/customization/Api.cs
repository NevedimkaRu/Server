using GTANetworkAPI;
using Newtonsoft.Json.Linq;
using Server.model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.customization
{
    class Api : Script
    {

        [RemoteEvent("remote_SaveCustomization")]
        public void SaveCustomization(Player player, object[] args)
        {
            String str = args[0].ToString();
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
                model.Insert();
            }
            model.SetToPlayer(player);
        }

        public static void LoadCustomization(Player player, int characterId)
        {
            Customization model = new Customization();
            model.LoadByOtherId("CharacterId", characterId);
            model.SetToPlayer(player);
        }

        [Command("hair", GreedyArg = true)]
        public void SetHair(Player player, string p1, string p2)
        {
            byte i = byte.Parse(p1);
            byte k = byte.Parse(p2);
            NAPI.Player.SetPlayerHeadOverlay(player, 1, new HeadOverlay() { Index = i, Color = k, Opacity = 1f });
        }
    }
}
