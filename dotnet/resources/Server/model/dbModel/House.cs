using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.model
{
    public class House : DB_Tables
    {
        public int CharacterId { get; set; } = -1;
        public Vector3 Position { get; set; }
        public int InteriorId { get; set; }
        public int Cost { get; set; }
        public bool Closed { get; set; }
        public uint _Dimension { get; set; }
        public string _Owner { get; set; }
        public Marker _Marker { get; set; }
        public TextLabel _TextLabel { get; set; }
        public Blip _Blip { get; set; }

    }

    public class HouseInterior
    {
        public string InteriorIpl { get; set; }
        public Vector3 Position { get; set; }

        public Marker _Marker { get; set; }
        public TextLabel _TextLabel { get; set; }
        public HouseInterior(string InteriorIpl, Vector3 Position)
        {
            this.InteriorIpl = InteriorIpl;
            this.Position = Position;

            _Marker = NAPI.Marker.CreateMarker(1,
               new Vector3(Position.X, Position.Y, Position.Z - 1.0f),
               Position,
               new Vector3(0, 0, 0),
               1.0f,
               new Color(207, 207, 207));

            _TextLabel = NAPI.TextLabel.CreateTextLabel("Выход\nнажмите \'alt\'", Position, 10.0f, 2.0f, 0, new Color(250, 250, 250));
        }

        public static void RequestIpl(string IplName)
        {
            if(IplName != null) NAPI.World.RequestIpl(IplName);
            if (Test.Debug) NAPI.Util.ConsoleOutput($"[House Interior Ipl] {IplName}");
        }
    }
}
