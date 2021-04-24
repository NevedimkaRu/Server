﻿using GTANetworkAPI;
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
                model.characterId = Main.Players1[player].Character.Id;
                model.Insert();
            }
            model.SetToPlayer(player);
        }

        [RemoteEvent("remote_GetCharacterCostumize")]
        public void GetCharacterCusumize(Player player) {
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
            model.SetToPlayer(player);
        }
    }
}
