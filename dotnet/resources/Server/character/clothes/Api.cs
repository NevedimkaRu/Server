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
                SetPlayerClothes(player, Clothes.ClothesTypes.Tops, clothes.Undershirt, clothes.UndershirtTexture);
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
        public JObject CreateClothesJson(bool gender)
        {
            dynamic ClothesData = new JObject();

            ClothesData.Masks = new JArray() as dynamic;
            ClothesData.Legs = new JArray() as dynamic;
            ClothesData.Shoes = new JArray() as dynamic;
            ClothesData.UpTops = new JArray() as dynamic;
            ClothesData.DownTops = new JArray() as dynamic;
            foreach(ClothesList masks in Dict.ClothesDict[Clothes.ClothesTypes.Masks].ClothesList[true])
            {
                dynamic maskData = new JObject();
                maskData.id = masks.Drawable;
                maskData.textures = new JArray(masks.Textures);
                ClothesData.Masks.Add(maskData);
            }
            foreach (ClothesList legs in Dict.ClothesDict[Clothes.ClothesTypes.Legs].ClothesList[gender])
            {
                dynamic legsData = new JObject();
                legsData.id = legs.Drawable;
                legsData.textures = new JArray(legs.Textures);
                ClothesData.Legs.Add(legsData);
            }
            foreach (ClothesList shoes in Dict.ClothesDict[Clothes.ClothesTypes.Shoes].ClothesList[gender])
            {
                dynamic shoesData = new JObject();
                shoesData.id = shoes.Drawable;
                shoesData.textures = new JArray(shoes.Textures);
                ClothesData.Shoes.Add(shoesData);
            }
            foreach (ClothesList tops in Dict.ClothesDict[Clothes.ClothesTypes.Tops].ClothesList[gender])
            {
                dynamic upTopsData = new JObject();
                dynamic downTopsData = new JObject();

                if(Dict.UndershirtsDict[gender].ContainsKey(tops.Drawable))
                {
                    downTopsData.id = tops.Drawable;
                    downTopsData.textures = new JArray(tops.Textures);
                    ClothesData.DownTops.Add(downTopsData);
                }
                else
                {
                    upTopsData.id = tops.Drawable;
                    upTopsData.textures = new JArray(tops.Textures);
                    upTopsData.type = tops.Type;
                    ClothesData.UpTops.Add(upTopsData);
                }

                //ClothesData.Shoes.Add(shoesData);
            }

            return ClothesData;
        }



        public static bool SetPlayerClothes(Player player,Clothes.ClothesTypes clothesType, int clothes, int texture)
        {
            int toptype = -1;
            if (clothesType == Clothes.ClothesTypes.Tops)
                toptype = Dict.ClothesDict[Clothes.ClothesTypes.Tops].ClothesList[true][clothes].Type;

            bool gender = Main.Players1[player].Customization.gender;
            if(clothesType == Clothes.ClothesTypes.Masks)
            {
                if(clothes == 0 || clothes == -1)
                {
                    Main.Players1[player].Clothes.Mask = -1;
                    Main.Players1[player].Clothes.MaskTexture = 0;
                    return true;
                }
                if (Dict.ClothesDict[Clothes.ClothesTypes.Masks].ClothesList[true][clothes] == null) return false;
                if (Dict.ClothesDict[Clothes.ClothesTypes.Masks].ClothesList[true][clothes].Drawable == -1 
                    || Dict.ClothesDict[Clothes.ClothesTypes.Masks].ClothesList[true][clothes].Textures.Contains(texture) == false) return false;
                Main.Players1[player].Clothes.Mask = clothes;
                Main.Players1[player].Clothes.MaskTexture = texture;

                player.SetClothes(1, clothes, texture);
                return true;
            }
            else if (clothesType == Clothes.ClothesTypes.Legs)
            {
                if (Dict.ClothesDict[Clothes.ClothesTypes.Legs].ClothesList[gender][clothes] == null) return false;
                if (Dict.ClothesDict[Clothes.ClothesTypes.Legs].ClothesList[gender][clothes].Drawable == -1 
                    || Dict.ClothesDict[Clothes.ClothesTypes.Legs].ClothesList[gender][clothes].Textures.Contains(texture) == false) return false;
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
                if (Dict.ClothesDict[Clothes.ClothesTypes.Shoes].ClothesList[gender][clothes] == null) return false;
                if (Dict.ClothesDict[Clothes.ClothesTypes.Shoes].ClothesList[gender][clothes].Drawable == -1
                    || Dict.ClothesDict[Clothes.ClothesTypes.Shoes].ClothesList[gender][clothes].Textures.Contains(texture) == false) return false;
                Main.Players1[player].Clothes.Shoes = clothes;
                Main.Players1[player].Clothes.ShoesTexture = texture;
                player.SetClothes(6, clothes, texture);
                return true;
            }
            else if(clothesType == Clothes.ClothesTypes.Tops)
            {
                if(clothes == 15 || clothes == -1)
                {
                    /*Main.Players1[player].Clothes.Top = -1;
                    Main.Players1[player].Clothes.TopTexture = 0;
                    Main.Players1[player].Clothes.Undershirt = -1;
                    Main.Players1[player].Clothes.UndershirtTexture = 0;*/
                    player.SetClothes(11, 15, 0);
                    player.SetClothes(8, 15, 0);
                    player.SetClothes(3, Dict.CorrectTorso[gender][15], 0);
                    return false;
                }
                if (Dict.ClothesDict[Clothes.ClothesTypes.Tops].ClothesList[gender][clothes] == null) return false;
                if (toptype != -1)
                {
                    if (Dict.ClothesDict[Clothes.ClothesTypes.Tops].ClothesList[gender][clothes].Drawable == -1 ||
                            Dict.ClothesDict[Clothes.ClothesTypes.Tops].ClothesList[gender][clothes].Textures.Contains(texture) == false) return false;

                    /*if(Main.Players1[player].Clothes.Undershirt == -1)
                    {
                        foreach(var cloth in Dict.UndershirtsDict[gender])
                        {
                            if (cloth.Value.Drawables.ContainsKey(toptype))
                            {
                                Main.Players1[player].Clothes.Undershirt = cloth.Key;
                                Main.Players1[player].Clothes.UndershirtTexture = 0;
                            }
                        }
                        
                    }*/

                    if (Dict.UndershirtsDict[gender].ContainsKey(Main.Players1[player].Clothes.Undershirt) 
                        && Dict.UndershirtsDict[gender][Main.Players1[player].Clothes.Undershirt].Drawables.ContainsKey(toptype))
                    {
                        player.SetClothes(11, 15, 0);
                        player.SetClothes(8, 15, 0);
                    
                        player.SetClothes(8, Dict.UndershirtsDict[gender][Main.Players1[player].Clothes.Undershirt].Drawables[toptype], Main.Players1[player].Clothes.UndershirtTexture);
                        /*Main.Players1[player].Clothes.Top = clothes;
                        Main.Players1[player].Clothes.TopTexture = texture;*/
                        player.SetClothes(11, clothes, texture);
                        player.SetClothes(3, Dict.CorrectTorso[gender][clothes], 0);
                        return true;
                    }
                    else
                    {
                        player.SetClothes(11, 15, 0);
                        player.SetClothes(8, 15, 0);
                        foreach(var under in Dict.UndershirtsDict[gender])
                        {
                            if (under.Value.Drawables.ContainsKey(toptype))
                            {
                                player.SetClothes(8, Dict.UndershirtsDict[gender][under.Key].Drawables[toptype], 0);
                                break;
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
                }
                else
                {
                    
                    if (Main.Players1[player].Clothes.Top != -1)
                    {
                        int toptype1 = Dict.ClothesDict[Clothes.ClothesTypes.Tops].ClothesList[gender][Main.Players1[player].Clothes.Top].Type;
                        if (Dict.UndershirtsDict[Main.Players1[player].Customization.gender].ContainsKey(clothes))
                        {
                            if(Dict.UndershirtsDict[Main.Players1[player].Customization.gender][clothes].Drawables.ContainsKey(toptype1))
                            {
                                if (Main.TempClothes.ContainsKey(player))
                                {
                                    Main.TempClothes[player].Undershirt = clothes;
                                    Main.TempClothes[player].UndershirtTexture = texture;
                                }

                                player.SetClothes(8, Dict.UndershirtsDict[Main.Players1[player].Customization.gender][clothes].Drawables[toptype1], 
                                    texture);
                                player.SetClothes(11, Main.Players1[player].Clothes.Top, Main.Players1[player].Clothes.TopTexture);
                                player.SetClothes(3, Dict.CorrectTorso[gender][Main.Players1[player].Clothes.Top], 0);
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                    if (Main.TempClothes.ContainsKey(player))
                    {
                        if (Dict.UndershirtsDict[Main.Players1[player].Customization.gender].ContainsKey(clothes))
                        {
                            Main.TempClothes[player].Undershirt = clothes;
                            Main.TempClothes[player].UndershirtTexture = texture;
                        }
                        else
                        {
                            Main.TempClothes[player].Top = clothes;
                            Main.TempClothes[player].TopTexture = texture;
                            Main.TempClothes[player].Undershirt = -1;
                            Main.TempClothes[player].UndershirtTexture = 0;
                        }
                    }
                    player.SetClothes(11, 15, 0);
                    player.SetClothes(8, 15, 0);
                    player.SetClothes(11, clothes, texture);
                    player.SetClothes(3, Dict.CorrectTorso[gender][clothes], 0);
                    return true;
                }
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
                    player.TriggerEvent("trigger_bigPortionClothesData", MaleClothesJson.Substring(i, i + chunkSize - 1 < MaleClothesJson.Length ? chunkSize - 1: MaleClothesJson.Length - i));
                    i += chunkSize;
                }
            }
            else
            {
                int i = 0;
                while (i < FemaleClothesJson.Length)
                {
                    player.TriggerEvent("trigger_bigPortionClothesData", FemaleClothesJson.Substring(i, i + chunkSize - 1 < FemaleClothesJson.Length ? chunkSize - 1: FemaleClothesJson.Length - i));
                    i += chunkSize;
                }
            }
        }

        [Command("sc")]
        public static void cmd_SetClothes(Player player,int clothestype, int drawable, int texture)
        {
            player.SetClothes(clothestype, drawable, texture);
            //SetPlayerClothes(player, (Clothes.ClothesTypes)clothestype, drawable, texture);
        }
    }
}
