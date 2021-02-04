using RAGE.Elements;
using System;
using System.Collections.Generic;
using System.Text;

namespace cs_packages.model
{
    public class Customize
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
    }
}
