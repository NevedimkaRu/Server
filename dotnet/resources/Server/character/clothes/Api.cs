using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GTANetworkAPI;
using Newtonsoft.Json.Linq;
using Server.model;

namespace Server.character.clothes
{
    public class Api : Script
    {
        public static string MaleClothesJson = "";
        public static string FemaleClothesJson = "";
        
        [ServerEvent(Event.ResourceStart)]
        public void OnResourceStart()
        {
            MaleClothesJson = CreateClothesJson(true).ToString();
            FemaleClothesJson = CreateClothesJson(false).ToString();
        }

        public static async Task<PlayerClothes> LoadPlayerClothes(Player player)
        {
            PlayerClothes playerClothes = new PlayerClothes();
            if(await playerClothes.LoadByOtherFieldAsync("CharacterId", Main.Players1[player].Character.Id.ToString()))
            {
                return playerClothes;
            }
            else
            {
                return null;
            }
        }
        public static void SetDefaultPlayerClothes(Player player, PlayerClothes clothes)
        {
            PlayerClothes playerClothes = clothes;

            SetPlayerClothes(player, Clothes.ClothesTypes.Masks, clothes.Mask, clothes.MaskTexture);
            SetPlayerClothes(player, Clothes.ClothesTypes.Tops, clothes.Top, clothes.TopTexture);
            if(Main.Players1[player].Clothes.Undershirt != -1)
            {
                SetPlayerClothes(player, Clothes.ClothesTypes.Undershirts, clothes.Undershirt, clothes.UndershirtTexture);
            }
            SetPlayerClothes(player, Clothes.ClothesTypes.Legs, clothes.Legs, clothes.LegsTexture);
            SetPlayerClothes(player, Clothes.ClothesTypes.Shoes, clothes.Shoes, clothes.LegsTexture);
        }
        public static PlayerClothes CreatePlayerDefaultClothes(Player player)
        {
            PlayerClothes playerClothes = new PlayerClothes();
            playerClothes.CharacterId = Main.Players1[player].Character.Id;
            playerClothes.Top = 4;
            playerClothes.TopTexture = 0;
            playerClothes.Undershirt = 8;
            playerClothes.UndershirtTexture = 0;
            playerClothes.Shoes = 1;
            playerClothes.ShoesTexture = 0;
            playerClothes.Legs = 10;
            playerClothes.LegsTexture = 0;
            playerClothes.Mask = -1;
            playerClothes.MaskTexture = -1;
            playerClothes.Insert();
            return playerClothes;
        }
        public JObject CreateUndershirtClothesJson(Player player)
        {
            bool gender = Main.Players1[player].Customization.gender;
            if (Dict.Tops[gender][Main.Players1[player].Clothes.Top] == null) return null;

            dynamic ClothesData = new JObject();
            ClothesData.Undershirt = new JArray() as dynamic;

            if (Dict.Tops[gender][Main.Players1[player].Clothes.Top].Undershirts == null) return null;

            foreach (var undershirt in Dict.Undershirts[gender])
            {
                if (Dict.Tops[gender][Main.Players1[player].Clothes.Top].Undershirts.Contains(undershirt.Key))
                {
                    dynamic UndershirtData = new JObject();

                    UndershirtData.id = undershirt.Key;
                    UndershirtData.textures = new JArray(undershirt.Value.Textures);
                    ClothesData.Undershirt.Add(UndershirtData);
                }
            }
            return ClothesData;
        }

        [Command("unjson")]
        public void CMD_CreateJsonUndershirtClothes(Player player)
        {
            player.SendChatMessage(CreateUndershirtClothesJson(player).ToString());
        }

        public JObject CreateClothesJson(bool gender)
        {
            dynamic ClothesData = new JObject();

            ClothesData.Masks = new JArray() as dynamic;
            ClothesData.Legs = new JArray() as dynamic;
            ClothesData.Shoes = new JArray() as dynamic;
            ClothesData.UpTops = new JArray() as dynamic;
            foreach(ClothesList masks in Dict.Masks)
            {
                dynamic maskData = new JObject();
                maskData.id = masks.Drawable;
                maskData.textures = new JArray(masks.Textures);
                ClothesData.Masks.Add(maskData);
            }
            foreach (ClothesList legs in Dict.Legs[gender])
            {
                dynamic legsData = new JObject();
                legsData.id = legs.Drawable;
                legsData.textures = new JArray(legs.Textures);
                ClothesData.Legs.Add(legsData);
            }
            foreach (ClothesList shoes in Dict.Shoes[gender])
            {
                dynamic shoesData = new JObject();
                shoesData.id = shoes.Drawable;
                shoesData.textures = new JArray(shoes.Textures);
                ClothesData.Shoes.Add(shoesData);
            }
            foreach (ClothesList tops in Dict.Tops[gender])
            {
                dynamic upTopsData = new JObject();

                upTopsData.id = tops.Drawable;
                upTopsData.textures = new JArray(tops.Textures);
                ClothesData.UpTops.Add(upTopsData);

                //ClothesData.Shoes.Add(shoesData);
            }
            return ClothesData;
        }



        public static bool SetPlayerClothes(Player player,Clothes.ClothesTypes clothesType, int clothes, int texture)
        {

            bool gender = Main.Players1[player].Customization.gender;
            if(clothesType == Clothes.ClothesTypes.Masks)
            {
                if(clothes == 0 || clothes == -1)
                {
                    Main.Players1[player].Clothes.Mask = -1;
                    Main.Players1[player].Clothes.MaskTexture = 0;
                    return true;
                }
                if (Dict.Masks[clothes] == null) return false;
                if (Dict.Masks[clothes].Drawable == -1 
                    || Dict.Masks[clothes].Textures.Contains(texture) == false) return false;
                Main.Players1[player].Clothes.Mask = clothes;
                Main.Players1[player].Clothes.MaskTexture = texture;

                player.SetClothes(1, clothes, texture);
                return true;
            }
            else if (clothesType == Clothes.ClothesTypes.Legs)
            {
                if (Dict.Legs[gender][clothes] == null) return false;
                if (Dict.Legs[gender][clothes].Drawable == -1 
                    || Dict.Legs[gender][clothes].Textures.Contains(texture) == false) return false;
                Main.Players1[player].Clothes.Legs = clothes;
                Main.Players1[player].Clothes.LegsTexture = texture;
                player.SetClothes(4, clothes, texture);
                return true;
            }
            else if (clothesType == Clothes.ClothesTypes.Shoes)
            {
                if (clothes == 34 || clothes == -1)
                {
                    Main.Players1[player].Clothes.Shoes = -1;
                    Main.Players1[player].Clothes.ShoesTexture = 0;
                    return true;
                }
                if (Dict.Shoes[gender][clothes] == null) return false;
                if (Dict.Shoes[gender][clothes].Drawable == -1
                    || Dict.Shoes[gender][clothes].Textures.Contains(texture) == false) return false;
                Main.Players1[player].Clothes.Shoes = clothes;
                Main.Players1[player].Clothes.ShoesTexture = texture;
                player.SetClothes(6, clothes, texture);
                return true;
            }
            else if(clothesType == Clothes.ClothesTypes.Undershirts)
            {
                if (!Dict.Undershirts[gender].ContainsKey(clothes))
                {
                    //todo сделать логирование
                    player.SendChatMessage("Такой одежды не существует");
                    return false;
                }
                List<int> underList = Dict.Tops[gender][Main.Players1[player].Clothes.Top].Undershirts;
                if (underList == null) return false;
                foreach(var under in underList)
                {
                    if(under == clothes)
                    {
                        player.SetClothes(8, 15, 0);
                        if (Main.TempClothes.ContainsKey(player))
                        {
                            Main.TempClothes[player].Undershirt = clothes;
                            Main.TempClothes[player].UndershirtTexture = texture;
                        }
                        player.SetClothes(8, clothes, texture);
                        return true;
                    }
                }
                player.SendChatMessage("Эта нижняя одежда не подходит для этого топа");
                return false;
            }
            else if(clothesType == Clothes.ClothesTypes.Tops)
            {
                if(clothes == 15 || clothes == -1)
                {
                    if (Main.TempClothes.ContainsKey(player))
                    {
                        Main.TempClothes[player].Top = -1;
                        Main.TempClothes[player].TopTexture = 0;
                        Main.TempClothes[player].Undershirt = -1;
                        Main.TempClothes[player].UndershirtTexture = 0;
                    }

                    player.SetClothes(11, 15, 0);
                    player.SetClothes(8, 15, 0);
                    player.SetClothes(3, Dict.CorrectTorso[gender][15], 0);
                    return false;
                }
                if (Dict.Tops[gender][clothes] == null) return false;

                player.SetClothes(11, 15, 0);
                player.SetClothes(8, 15, 0);

                if(Dict.Tops[gender][clothes].Undershirts != null)
                {
                    if (Dict.Tops[gender][clothes].Undershirts.Contains(Main.Players1[player].Clothes.Undershirt))
                    {
                        player.SetClothes(8, Main.Players1[player].Clothes.Undershirt, Main.Players1[player].Clothes.UndershirtTexture);
                        if (Main.TempClothes.ContainsKey(player))
                        {
                            Main.TempClothes[player].Undershirt = Main.Players1[player].Clothes.Undershirt;
                            Main.TempClothes[player].UndershirtTexture = Main.Players1[player].Clothes.UndershirtTexture;
                        }
                    }
                    else
                    {
                        player.SetClothes(8, Dict.Tops[gender][clothes].Undershirts[0], 0);
                        if (Main.TempClothes.ContainsKey(player))
                        {
                            Main.TempClothes[player].Undershirt = Dict.Tops[gender][clothes].Undershirts[0];
                            Main.TempClothes[player].UndershirtTexture = 0;
                        }
                    }
                }

                if (Main.TempClothes.ContainsKey(player))
                {
                    Main.TempClothes[player].Top = clothes;
                    Main.TempClothes[player].TopTexture = texture;
                }
                player.SetClothes(11, clothes, texture);
                player.SetClothes(3, Dict.CorrectTorso[gender][clothes], 0);
                return true;
            }
            return false;
        }

        public static void SendClothesStoreData(Player player)
        {
            int chunkSize = 26000;
            //int testi = 0;
            if (Main.Players1[player].Customization.gender)
            {
                //26 649 символов - максимальное колчиество символов в строке для передачи на клиент
                int i = 0;
                while (i < MaleClothesJson.Length)
                {
                    //player.SendChatMessage($"Передаётся порция {++testi}");
                    player.TriggerEvent("trigger_bigPortionClothesData", MaleClothesJson.Substring(i, i + chunkSize < MaleClothesJson.Length ? chunkSize : MaleClothesJson.Length - i));
                    i += chunkSize;
                }
            }
            else
            {
                int i = 0;
                while (i < FemaleClothesJson.Length)
                {
                    player.TriggerEvent("trigger_bigPortionClothesData", FemaleClothesJson.Substring(i, i + chunkSize < FemaleClothesJson.Length ? chunkSize : FemaleClothesJson.Length - i));
                    i += chunkSize;
                }
            }
        }

        [Command("sc")]
        public static void cmd_SetClothes(Player player,int clothestype, int drawable, int texture)
        {
            //player.SetClothes(clothestype, drawable, texture);
            SetPlayerClothes(player, (Clothes.ClothesTypes)clothestype, drawable, texture);
        }
    }
}
