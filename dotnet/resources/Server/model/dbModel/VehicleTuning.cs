﻿using GTANetworkAPI;

namespace Server.model
{
    public class VehicleTuning : DB_Tables
    {
        public int CarId { get; set; }

        public Color PrimaryColor { get; set; } = new Color(0, 0, 0);
        public Color SecondaryColor { get; set; } = new Color(0, 0, 0);
        public int ColorType { get; set; } = 0;
        public int Spoiler { get; set; } = -1; // 0
        public int FrontBumper { get; set; } = -1; // 1
        public int RearBumper { get; set; } = -1;// 2
        public int SideSkirt { get; set; } = -1;// 3
        public int Exhaust { get; set; } = -1;// 4
        public int Frame { get; set; } = -1;// 5
        public int Grille { get; set; } = -1;// 6
        public int Hood { get; set; } = -1;// 7
        public int Fender { get; set; } = -1;// 8
        public int RightFender { get; set; } = -1;
        public int Roof { get; set; } = -1;
        public int Engine { get; set; } = -1;
        public int Brakes { get; set; } = -1;
        public int Transmission { get; set; } = -1;
        public int Horns { get; set; } = -1;
        public int Suspension { get; set; } = -1;
        public int Armor { get; set; } = -1;// 16
        public int Turbo { get; set; } = -1;// 18
        public int Xenon { get; set; } = -1;// 22
        public int FrontWheels { get; set; } = -1;//23
        public int BackWheels  { get; set; } = -1;//24 Only for Motorcycles

        public int PlateHolders { get; set; } = -1;//25
        public int TrimDesign { get; set; } = -1;//27
        public int Ornaments { get; set; } = -1;//28
        public int DialDesign { get; set; } = -1;//30

        public int SteeringWheel { get; set; } = -1;//33
        public int ShiftLever { get; set; } = -1;//34
        public int Plaques { get; set; } = -1;//35
        public int Hydraulics { get; set; } = -1;//38
        public int Boost { get; set; } = -1;//40
        public int Livery { get; set; } = -1;//48
        public int Plate { get; set; } = 0;//53
        public int WindowTint { get; set; } = -1;//55
    }
}
