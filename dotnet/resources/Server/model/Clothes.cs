using System;
using System.Collections.Generic;
using System.Text;

namespace Server.model
{
    public class Clothes
    {
        public enum ClothesTypes
        {
            Masks = 1,
            Legs = 4,
            Shoes = 6,
            Accessories = 7,
            Tops = 11
        }

        public Dictionary<bool, List<ClothesList>> ClothesList;
        public Clothes(Dictionary<bool, List<ClothesList>> ClothesList)
        {
            this.ClothesList = ClothesList;
        }
    }
    public class ClothesList
    {
        public int Drawable { get; set; }
        public List<int> Textures { get; set; }
        public int Palette { get; set; }
        public int Type { get; set; } = -1;
        public ClothesList(int Drawable, List<int> Textures, int Palette, int Type = -1)
        {
            this.Drawable = Drawable;
            this.Textures = Textures;
            this.Palette = Palette;
            this.Type = Type;
        }
    }
    public class Undershirts
    {
        public Dictionary<int,int> Drawables { get; set; }//key - type, value - drawable

        public Undershirts(Dictionary<int, int> Drawables)
        {
            this.Drawables = Drawables;
        }
    }

    public class PlayerClothes
    {
        public int Top { get; set; } = -1;
        public int TopTexture { get; set; }
        public int Undershirt { get; set; } = -1;
        public int UndershirtTexture { get; set; }

        public int Mask { get; set; } = -1;
        public int MaskTexture { get; set; }

        public int Legs { get; set; } = -1;
        public int LegsTexture { get; set; }

        public int Shoes { get; set; }
        public int ShoesTexture { get; set; }
    }

}
