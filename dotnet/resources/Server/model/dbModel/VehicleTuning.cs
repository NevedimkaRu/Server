using System;
using System.Collections.Generic;
using System.Text;

namespace Server.model
{
    public class VehicleTuning : DB_Tables
    {
        public int CarId { get; set; }

        public int Spoiler { get; set; } = -1; // 0
        public int FrontBumper { get; set; } = -1; // 1
        public int RearBumper { get; set; } = -1;// 2
        public int SideSkirt { get; set; } = -1;// 3
        public int Exhaust { get; set; }// 4
        public int Frame { get; set; }// 5
        public int Grille { get; set; }// 6
        public int Hood { get; set; }// 7
        public int Fender { get; set; }// 8
        public int RightFender { get; set; }
        public int Roof { get; set; }
        public int Engine { get; set; }
        public int Brakes { get; set; }
        public int Transmission { get; set; }
        public int Horns { get; set; }
        public int Suspension { get; set; }
        public int Armor { get; set; }// 16
        public int Turbo { get; set; }// 18
        public int Xenon { get; set; }// 22
        public int FrontWheels { get; set; }//23
        public int BackWheels  { get; set; }//24 Only for Motorcycles

        public int PlateHolders { get; set; }//25
        public int TrimDesign { get; set; }//27
        public int Ornaments { get; set; }//28
        public int DialDesign { get; set; }//30

        public int SteeringWheel { get; set; }//33
        public int ShiftLever { get; set; }//34
        public int Plaques { get; set; }//35
        public int Hydraulics { get; set; }//38
        public int Boost { get; set; }//40
        public int Livery { get; set; }//48
        public int Plate { get; set; } = 0;//53
        public int WindowTint { get; set; }//55
        

        enum VehicleId
        {
            CarId = 0,
            Spoiler = 10,
        }
    }
}
