using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using Newtonsoft.Json.Linq;
using Server.character.clothes;
using Server.model;

namespace Server.remoteEvents
{
    class Clothes : Script
    {
        [RemoteEvent("remote_SetCloth")]
        public void Remote_SetCloth(Player player, int clothType, int clothId, int texture)
        {
            PlayerClothes model = new PlayerClothes();
            model.Top = Main.Players1[player].Clothes.Top;
            model.TopTexture = Main.Players1[player].Clothes.TopTexture;
            model.Undershirt = Main.Players1[player].Clothes.Undershirt;
            model.UndershirtTexture = Main.Players1[player].Clothes.UndershirtTexture;
            model.Mask = Main.Players1[player].Clothes.Mask;
            model.MaskTexture = Main.Players1[player].Clothes.MaskTexture;
            model.Legs = Main.Players1[player].Clothes.Legs;
            model.LegsTexture = Main.Players1[player].Clothes.LegsTexture;
            model.Shoes = Main.Players1[player].Clothes.Shoes;
            model.ShoesTexture = Main.Players1[player].Clothes.ShoesTexture;

            if (!Main.TempClothes.ContainsKey(player))
                Main.TempClothes.Add(player, model);

            if (!Api.SetPlayerClothes(player, (model.Clothes.ClothesTypes)clothType, clothId, texture))
            {
                player.TriggerEvent("trigger_buyClothesError", "Несовместимость одежды");
            }
        }
        [RemoteEvent("remote_DiscardCloth")]
        public void Remote_DiscardCloth(Player player)
        {
            Api.SetDefaultPlayerClothes(player, Main.Players1[player].Clothes);
        }        
        [RemoteEvent("remote_CloseClothesStore")]
        public void Remote_CloseClothesStore(Player player)
        {
            //Api.SetDefaultPlayerClothes(player, Main.Players1[player].Clothes);
            if (Main.TempClothes.ContainsKey(player)) Main.TempClothes.Remove(player);
        }
        [RemoteEvent("remote_BuyCloth")]
        public void Remote_BuyCloth(Player player)
        {
            if (Main.TempClothes.ContainsKey(player))
            {
                Main.Players1[player].Clothes = Main.TempClothes[player];
                Main.TempClothes.Remove(player);
                utils.Trigger.ClientEvent(player, "trigger_buyClothesSuccess");
            }
        }

        [RemoteProc("remote_GetDownTops")]
        public string Remote_GetDownTops(Player player) {
            JObject json = new Api().CreateUndershirtClothesJson(player);
            if (json == null) 
            {
                return "null";
            }
            return json.ToString();
        }
    }
}
