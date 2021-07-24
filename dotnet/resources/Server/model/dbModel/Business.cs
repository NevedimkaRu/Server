using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.model
{
    public class Business : DB_Tables
    {
        public enum _BizType : Int32
        {
            Clothes = 0,
            Tuning = 1,
            Vehicle = 2
        }

        public int Type { get; set; }
        public Vector3 Position { get; set; }
        public Marker _Marker { get; set; }
        public TextLabel _TextLabel { get; set; }
        public Blip _Blip { get; set; }
        public Business()
        {

        }
        public void InitBusiness()
        {

            List<string> Types = new List<string>()
            {
                {"Магазин одежды" },
                {"Автосалон" },
                {"Тюнинг" }
            };

            List<int> Blips = new List<int>()
            {
                {73},
                {523},
                {446}
            };

            _Marker = NAPI.Marker.CreateMarker(1,
               new Vector3(Position.X, Position.Y, Position.Z - 1.0f),
               Position,
               new Vector3(0, 0, 0),
               1.0f,
               new Color(207, 207, 207));
            string key = (int)Type == 1 ? "E" : "alt";
            _TextLabel = NAPI.TextLabel.CreateTextLabel($"{Types[(int)Type]}\nНажмите \'{key}\'", Position, 10.0f, 2.0f, 0, new Color(250, 250, 250));

            _Blip = NAPI.Blip.CreateBlip(Blips[(int)Type], Position, 1.0f, 0,name: Types[(int)Type]);
        }
    }
}
