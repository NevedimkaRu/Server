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
            Undershirts = 8,
            Tops = 11
        }
    }
    public class ClothesList
    {
        public int Drawable { get; }
        public List<int> Textures { get; }
        public int SubUndershirtType { get; } = -1;
        public int Palette { get; }
        public int Type { get; } = -1;

        public List<int> Undershirts { get; } = null;
        public ClothesList(int Drawable, List<int> Textures, int Palette, List<int> Underhisrts = null)
        {
            this.Drawable = Drawable;
            this.Textures = Textures;
            this.Palette = Palette;
            this.Undershirts = Underhisrts;
        }
    }

    public class UnderShirts
    {
        public List<int> Textures { get; set; }
        public UnderShirts(List<int> Textures)
        {
            this.Textures = Textures;
        }
    }

    public class PlayerClothes : DB_Tables
    {
        public int CharacterId { get; set; }

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
