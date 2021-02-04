using GTANetworkAPI;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Server.model
{
    public class Customization : DB_Tables
    {
        public int characterId { get; set; }
        public bool gender { get; set; }
        public int father { get; set; }
        public int mother { get; set; }
        public float similar { get; set; } 
        public float skin { get; set; }
        //внешность
        public int hair { get; set; }
        public int eyebrows { get; set; }
        public int beard { get; set; }
        public int hairColor { get; set; }
        public int eyeColor { get; set; }
        //Лицо
        public float noseWidth { get; set; }
        public float noseHeight { get; set; } 
        public float noseTipLength { get; set; } 
        public float noseDepth { get; set; } 
        public float noseTipHeight { get; set; } 
        public float noseBroke { get; set; } 
        public float eyebrowHeight { get; set; } 
        public float eyebrowDepth { get; set; } 
        public float cheekboneHeight { get; set; }
        public float cheekboneWidth { get; set; } 
        public float cheekDepth { get; set; }
        public float eyeScale { get; set; }
        public float lipThickness { get; set; }
        public float jawWidth { get; set; }
        public float sjawShapekin { get; set; }
        public float chinHeight { get; set; }
        public float chinDepth { get; set; }
        public float chinWidth { get; set; }
        public float chinIndent { get; set; } 
        public float neck { get; set; }

        public void ParseClientData(JObject param)
        {
            foreach (var obj in GetType().GetProperties())
            {
                string fld = obj.Name;
                if (fld != "Id" && fld != "characterId")
                {
                    switch (GetType().GetProperty(fld).PropertyType.Name)
                    {
                        case "Boolean": obj.SetValue(this, bool.Parse(param[fld].ToString())); break;
                        case "Int32": obj.SetValue(this, int.Parse(param[fld].ToString())); break;
                        case "Single": obj.SetValue(this, float.Parse(param[fld].ToString())); break;
                        default: break;
                    }
                }
            }
        }

        public void SetToPlayer(Player player)
        {
            List<string> features = new List<string>
            {"noseWidth", "noseHeight", "noseTipLength", "noseDepth", "noseTipHeight", "noseBroke", "eyebrowHeight", "eyebrowDepth",
                "cheekboneHeight", "cheekboneWidth", "cheekDepth", "eyeScale", "lipThickness", "jawWidth", "sjawShapekin", "chinHeight",
                "chinDepth", "chinWidth", "chinIndent", "neck" };
            if (this.gender)
            {
                NAPI.Player.SetPlayerSkin(player, NAPI.Util.GetHashKey("mp_m_freemode_01"));
            }
            else {
                NAPI.Player.SetPlayerSkin(player, NAPI.Util.GetHashKey("mp_f_freemode_01"));
            }
            HeadBlend hb = new HeadBlend()
            {
                ShapeFirst = (byte) mother,
                ShapeSecond = (byte)father,
                ShapeThird = 0,

                SkinFirst = (byte) mother,
                SkinSecond = (byte) father,
                SkinThird = 0,

                ShapeMix = (byte) similar,
                SkinMix = (byte) skin,
                ThirdMix = 0.0f,
            };
            NAPI.Player.SetPlayerHeadBlend(player, hb);
            foreach (string fld in features)
            { 
                NAPI.Player.SetPlayerFaceFeature(player, features.IndexOf(fld), (float) this.GetType().GetProperty(fld).GetValue(this));
            }
            NAPI.Player.SetPlayerClothes(player, 2, this.hair, 0);
            NAPI.Player.SetPlayerHairColor(player, (byte) this.hairColor, 0);
            NAPI.Player.SetPlayerHeadOverlay(player, 1, new HeadOverlay() { Index = (byte) this.beard, Color = (byte)this.hairColor, Opacity = 1f });
            NAPI.Player.SetPlayerHeadOverlay(player, 2, new HeadOverlay() { Index = (byte) this.eyebrows, Color = (byte)this.hairColor, Opacity = 1f });
          
            NAPI.Player.SetPlayerEyeColor(player, (byte) this.eyeColor);
        }

        public bool getByCharacterId(int characterId)
        {
            DataTable db = MySql.QueryRead($"select * from `customization` where characterId = '{characterId}'");
            if (db == null || db.Rows.Count == 0)
            {
                return false;
            }
            DataRow row = db.Rows[0];
            foreach (var obj in GetType().GetProperties())
            {
                string fld = obj.Name;
                if (GetType().GetProperty(fld).PropertyType.Name == "Boolean")
                {
                    obj.SetValue(this, Convert.ToBoolean(row[fld]));
                } else
                {
                    obj.SetValue(this, row[fld]);
                }
            }

            return true;
        }
    }
}
