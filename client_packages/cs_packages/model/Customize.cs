using RAGE.Elements;
using System;
using System.Collections.Generic;
using System.Text;

namespace cs_packages.model
{
    class Customize
    {
        //генетика
        public bool gender { get; set; } = true;
        public int father { get; set; } = 0;
        public int mother { get; set; } = 21;
        public float similar { get; set; } = 0.5f;
        public float skin { get; set; } = 0.5f;
        //внешность
        public int hair { get; set; } = 0;
        public int eyebrows { get; set; } = 0;
        public int beard { get; set; } = 0;
        public int hairColor { get; set; } = 0;
        public int eyeColor { get; set; } = 0;
        //Лицо
        public float noseWidth { get; set; } = 0f;
        public float noseHeight { get; set; } = 0f;
        public float noseTipLength { get; set; } = 0f;
        public float noseDepth { get; set; } = 0f;
        public float noseTipHeight { get; set; } = 0f;
        public float noseBroke { get; set; } = 0f;
        public float eyebrowHeight { get; set; } = 0f;
        public float eyebrowDepth { get; set; } = 0f;
        public float cheekboneHeight { get; set; } = 0f;
        public float cheekboneWidth { get; set; } = 0f;
        public float cheekDepth { get; set; } = 0f;
        public float eyeScale { get; set; } = 0f;
        public float lipThickness { get; set; } = 0f;
        public float jawWidth { get; set; } = 0f;
        public float sjawShapekin { get; set; } = 0f;
        public float chinHeight { get; set; } = 0f;
        public float chinDepth { get; set; } = 0f;
        public float chinWidth { get; set; } = 0f;
        public float chinIndent { get; set; } = 0f;
        public float neck { get; set; } = 0f;

        public void UpdateCharacterParents()
        {
            Player.LocalPlayer.SetHeadBlendData(
                this.mother,
                this.father,
                0,

                this.mother,
                this.father,
                0,

                this.similar,
                this.skin,
                0.0f,

                true
            );
        }
        public void UpdateCharacterHair()
        {
            int currentGender = (gender) ? 0 : 1;
            // hair
            Player.LocalPlayer.SetComponentVariation(2, this.hair, 0, 0);
            Player.LocalPlayer.SetHairColor(this.hairColor, 0);

            // appearance colors
            Player.LocalPlayer.SetHeadOverlayColor(2, 1, this.hairColor, 100); // eyebrow
            Player.LocalPlayer.SetHeadOverlayColor(1, 1, this.hairColor, 100); // beard
            Player.LocalPlayer.SetHeadOverlayColor(10, 1, this.hairColor, 100); // chesthair

            // eye color
            Player.LocalPlayer.SetEyeColor(this.eyeColor);
        }

        public void Update(string f)
        {
            switch (f)
            {
                case "similar":
                    UpdateCharacterParents();
                    return;
                case "skin":
                    UpdateCharacterParents();
                    return;
                case "father":
                    UpdateCharacterParents();
                    return;
                case "mother":
                    UpdateCharacterParents();
                    return;
                case "hairM":
                    UpdateCharacterHair();
                    return;
                case "hairF":
                    UpdateCharacterHair();
                    return;
                case "eyebrowsM": Player.LocalPlayer.SetHeadOverlay(2, (int) this.GetType().GetProperty(f).GetValue(this), 100); return;
                case "eyebrowsF": Player.LocalPlayer.SetHeadOverlay(2, (int)this.GetType().GetProperty(f).GetValue(this), 100); return;
                case "beard": Player.LocalPlayer.SetHeadOverlay(1, (int)this.GetType().GetProperty(f).GetValue(this), 100); return;
                case "hairColor":
                    UpdateCharacterHair();
                    return;
                case "eyeColor":
                    UpdateCharacterHair();
                    return;
                case "noseWidth": Player.LocalPlayer.SetFaceFeature(0, (float) this.GetType().GetProperty(f).GetValue(this)); return;
                case "noseHeight": Player.LocalPlayer.SetFaceFeature(1, (float)this.GetType().GetProperty(f).GetValue(this));  return;
                case "noseTipLength": Player.LocalPlayer.SetFaceFeature(2, (float)this.GetType().GetProperty(f).GetValue(this)); return;
                case "noseDepth": Player.LocalPlayer.SetFaceFeature(3, (float)this.GetType().GetProperty(f).GetValue(this));  return;
                case "noseTipHeight": Player.LocalPlayer.SetFaceFeature(4, (float)this.GetType().GetProperty(f).GetValue(this)); return;
                case "noseBroke": Player.LocalPlayer.SetFaceFeature(5, (float)this.GetType().GetProperty(f).GetValue(this)); return;
                case "eyebrowHeight": Player.LocalPlayer.SetFaceFeature(6, (float)this.GetType().GetProperty(f).GetValue(this));return;
                case "eyebrowDepth": Player.LocalPlayer.SetFaceFeature(7, (float)this.GetType().GetProperty(f).GetValue(this)); return;
                case "cheekboneHeight": Player.LocalPlayer.SetFaceFeature(8, (float)this.GetType().GetProperty(f).GetValue(this)); return;
                case "cheekboneWidth": Player.LocalPlayer.SetFaceFeature(9, (float) this.GetType().GetProperty(f).GetValue(this)); return;
                case "cheekDepth": Player.LocalPlayer.SetFaceFeature(10, (float)this.GetType().GetProperty(f).GetValue(this)); return;
                case "eyeScale": Player.LocalPlayer.SetFaceFeature(11, (float)this.GetType().GetProperty(f).GetValue(this)); return;
                case "lipThickness": Player.LocalPlayer.SetFaceFeature(12, (float)this.GetType().GetProperty(f).GetValue(this)); return;
                case "jawWidth": Player.LocalPlayer.SetFaceFeature(13, (float)this.GetType().GetProperty(f).GetValue(this)); return;
                case "jawShape": Player.LocalPlayer.SetFaceFeature(14, (float)this.GetType().GetProperty(f).GetValue(this)); return;
                case "chinHeight": Player.LocalPlayer.SetFaceFeature(15, (float)this.GetType().GetProperty(f).GetValue(this)); return;
                case "chinDepth": Player.LocalPlayer.SetFaceFeature(16, (float)this.GetType().GetProperty(f).GetValue(this)); return;
                case "chinWidth": Player.LocalPlayer.SetFaceFeature(17, (float)this.GetType().GetProperty(f).GetValue(this)); return;
                case "chinIndent": Player.LocalPlayer.SetFaceFeature(18, (float)this.GetType().GetProperty(f).GetValue(this)); return;
                case "neck": Player.LocalPlayer.SetFaceFeature(19, (float)this.GetType().GetProperty(f).GetValue(this)); return;
            }
        }
    }
}
