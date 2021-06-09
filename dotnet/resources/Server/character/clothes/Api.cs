using System;
using System.Collections.Generic;
using System.Text;
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
            int toptype = Dict.ClothesDict[Clothes.ClothesTypes.Tops].ClothesList[true][clothes].Type;
            bool gender = Main.Players1[player].Customization.gender;
            if(clothesType == Clothes.ClothesTypes.Masks)
            {
                if(clothes == 0 || clothes == -1)
                {
                    Main.Players1[player].Clothes.Mask = -1;
                    Main.Players1[player].Clothes.MaskTexture = 0;
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
            else if(clothesType == Clothes.ClothesTypes.Tops)
            {
                if(clothes == 15 || clothes == -1)
                {
                    Main.Players1[player].Clothes.Top = -1;
                    Main.Players1[player].Clothes.TopTexture = 0;
                    Main.Players1[player].Clothes.Undershirt = -1;
                    Main.Players1[player].Clothes.TopTexture = 0;
                    player.SetClothes(11, 15, 0);
                    player.SetClothes(8, 15, 0);
                    player.SetClothes(3, Dict.CorrectTorso[gender][15], 0);
                    return true;
                }
                if (Dict.ClothesDict[Clothes.ClothesTypes.Tops].ClothesList[gender][clothes] == null) return false;
                if (toptype != -1)
                {
                    if (Dict.ClothesDict[Clothes.ClothesTypes.Tops].ClothesList[gender][clothes].Drawable == -1 ||
                            Dict.ClothesDict[Clothes.ClothesTypes.Tops].ClothesList[gender][clothes].Textures.Contains(texture) == false) return false;

                    if (Dict.UndershirtsDict[gender].ContainsKey(Main.Players1[player].Clothes.Undershirt) 
                        && Dict.UndershirtsDict[gender][Main.Players1[player].Clothes.Undershirt].Drawables.ContainsKey(toptype))
                    {
                        player.SetClothes(11, 15, 0);
                        player.SetClothes(8, 15, 0);
                    
                        player.SetClothes(8, Dict.UndershirtsDict[gender][Main.Players1[player].Clothes.Undershirt].Drawables[toptype], Main.Players1[player].Clothes.UndershirtTexture);
                        Main.Players1[player].Clothes.Top = clothes;
                        Main.Players1[player].Clothes.TopTexture = texture;
                        player.SetClothes(11, clothes, texture);
                        player.SetClothes(3, Dict.CorrectTorso[gender][clothes], 0);
                        return true;
                    }
                    else
                    {
                        player.SendChatMessage("Эта футболка/рубашка не подходит к данному топу");
                        return false;
                    }
                }
                else
                {
                    player.SetClothes(11, 15, 0);
                    player.SetClothes(8, 15, 0);
                    if (Main.Players1[player].Clothes.Top != -1)
                    {
                        int toptype1 = Dict.ClothesDict[Clothes.ClothesTypes.Tops].ClothesList[gender][Main.Players1[player].Clothes.Top].Type;
                        if (Dict.UndershirtsDict[Main.Players1[player].Customization.gender].ContainsKey(clothes))
                        {
                            if(Dict.UndershirtsDict[Main.Players1[player].Customization.gender][clothes].Drawables.ContainsKey(toptype1))
                            {
                                Main.Players1[player].Clothes.Undershirt = clothes;
                                Main.Players1[player].Clothes.UndershirtTexture = texture;

                                player.SetClothes(8, Dict.UndershirtsDict[Main.Players1[player].Customization.gender][Main.Players1[player].Clothes.Undershirt].Drawables[toptype1], 
                                    Main.Players1[player].Clothes.UndershirtTexture);
                                player.SetClothes(11, Main.Players1[player].Clothes.Top, 0);
                                player.SetClothes(3, Dict.CorrectTorso[gender][Main.Players1[player].Clothes.Top], 0);
                                return true;
                            }
                        }
                    }
                    if (Dict.UndershirtsDict[Main.Players1[player].Customization.gender].ContainsKey(clothes))
                    {
                        Main.Players1[player].Clothes.Undershirt = clothes;
                        Main.Players1[player].Clothes.UndershirtTexture = texture;
                    }
                    else
                    {
                        Main.Players1[player].Clothes.Top = clothes;
                        Main.Players1[player].Clothes.TopTexture = texture;
                        Main.Players1[player].Clothes.Undershirt = -1;
                        Main.Players1[player].Clothes.UndershirtTexture = 0;
                    }
                    player.SetClothes(11, clothes, texture);
                    player.SetClothes(3, Dict.CorrectTorso[gender][clothes], 0);
                    return true;
                }
            }
            return false;
        }
        [Command("sc")]
        public static void cmd_SetClothes(Player player,int clothestype, int drawable, int texture)
        {
            SetPlayerClothes(player, (Clothes.ClothesTypes)clothestype, drawable, texture);
        }
    }
}
