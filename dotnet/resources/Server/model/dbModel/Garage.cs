using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.model
{
    class Garage : DB_Tables
    {
        public int HouseId { get; set; }
        public int CharacterId { get; set; }
        public int GarageType { get; set; }
        public uint Cost { get; set; }
        public Vector3 Position { get; set; }
        public float Rotation { get; set; }
        public bool Closed { get; set; } = false;
        public Marker _Marker { get; set; }
        public TextLabel _TextLabel { get; set; }
        public Blip _Blip { get; set; }
        public string _Owner { get; set; }
    }

    class GarageType
    {
        public string Ipl { get; set; } = null;
        public Vector3 Position { get; set; }//Позиция входа в дом / выхода на улицу
        public Vector3 ExitPosition { get;set; }//Выход из гаража(на машине)
        public List<GarageVehiclePosition> VehiclePosition { get; set; }
        public Marker Marker { get; set; }
        //public TextLabel TextLabel { get; set; }

        public GarageType(string Ipl, Vector3 Position, Vector3 ExitPosition, List<GarageVehiclePosition> VehiclePosition)
        {
            this.Ipl = Ipl;
            this.Position = Position;
            this.ExitPosition = ExitPosition;
            this.VehiclePosition = VehiclePosition;
            Marker =  NAPI.Marker.CreateMarker(1,
                       new Vector3(Position.X, Position.Y, Position.Z - 1.0f),
                       Position,
                       new Vector3(0, 0, 0),
                       1.0f,
                       new Color(207, 207, 207));

            /*TextLabel = */NAPI.TextLabel.CreateTextLabel("Нажмите \"Alt\" ", Position, 10.0f, 2.0f, 0, new Color(250, 250, 250));
            NAPI.TextLabel.CreateTextLabel("Нажмите \"E\"(в авто) ", ExitPosition, 10.0f, 2.0f, 0, new Color(250, 250, 250));
        }
    }

    class GarageVehiclePosition
    {
        public Vector3 Position { get; set; }
        public float Rotation { get; set; }
        public GarageVehiclePosition(Vector3 Position, float Rotation)
        {
            this.Position = Position;
            this.Rotation = Rotation;
        }
    }
}
