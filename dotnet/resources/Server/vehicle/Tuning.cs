using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using MySqlConnector;
using System.Data;
using Server.model;
namespace Server.vehicle
{
    class Tuning : Script
    {
        public static void LoadTunning(int carid)
        {
            var tuning = new VehicleTuning();

            MySqlCommand cmd = new MySqlCommand
            {
                CommandText = "SELECT * FROM `vehiclestuning` WHERE `carid` = @id "
            };
            cmd.Parameters.AddWithValue("@id", carid);

            DataTable result = MySql.QueryRead(cmd);

            foreach (DataRow row in result.Rows)
            {
                tuning.Spoiler = Convert.ToInt32(row["Spoiler"]);
                tuning.FrontBumper = Convert.ToInt32(row["FrontBumper"]);
                tuning.RearBumper = Convert.ToInt32(row["RearBumper"]);
                tuning.SideSkirt = Convert.ToInt32(row["SideSkirt"]);// 3
                tuning.Exhaust = Convert.ToInt32(row["Exhaust"]);// 4
                tuning.Frame = Convert.ToInt32(row["Frame"]);// 5
                tuning.Grille = Convert.ToInt32(row["Grille"]);// 6
                tuning.Hood = Convert.ToInt32(row["Hood"]);// 7
                tuning.Fender = Convert.ToInt32(row["Fender"]);// 8
                tuning.RightFender = Convert.ToInt32(row["RightFender"]);
                tuning.Roof = Convert.ToInt32(row["Roof"]);
                tuning.Engine = Convert.ToInt32(row["Engine"]);
                tuning.Brakes = Convert.ToInt32(row["Brakes"]);
                tuning.Transmission = Convert.ToInt32(row["Transmission"]);
                tuning.Horns = Convert.ToInt32(row["Horns"]);
                tuning.Suspension = Convert.ToInt32(row["Suspension"]);
                tuning.Armor = Convert.ToInt32(row["Armor"]);// 16
                tuning.Turbo = Convert.ToInt32(row["Turbo"]);// 18
                tuning.Xenon = Convert.ToInt32(row["Xenon"]);// 22
                tuning.FrontWheels = Convert.ToInt32(row["FrontWheels"]);//23
                tuning.BackWheels = Convert.ToInt32(row["BackWheels"]);//24 Only for Motorcycles
                tuning.PlateHolders = Convert.ToInt32(row["PlateHolders"]);//25
                tuning.TrimDesign = Convert.ToInt32(row["TrimDesign"]);//27
                tuning.Ornaments = Convert.ToInt32(row["Ornaments"]);//28
                tuning.DialDesign = Convert.ToInt32(row["DialDesign"]);//30
                tuning.SteeringWheel = Convert.ToInt32(row["SteeringWheel"]);//33
                tuning.ShiftLever = Convert.ToInt32(row["ShiftLever"]);//34
                tuning.Plaques = Convert.ToInt32(row["Plaques"]);//35
                tuning.Hydraulics = Convert.ToInt32(row["Hydraulics"]);//38
                tuning.Boost = Convert.ToInt32(row["Boost"]);//40
                tuning.Livery = Convert.ToInt32(row["Livery"]);//48
                tuning.Plate = Convert.ToInt32(row["Plate"]); //53
                tuning.WindowTint = Convert.ToInt32(row["WindowTint"]);//55
                Main.VehicleTunings.Add(carid, tuning);
            }
        }
        public static void UpdateTuning(int carid)
        {

        }

        public static void ApplyTuning(Vehicle vehicle, int vehicleID)
        {
            vehicle.SetMod(0, Main.VehicleTunings[vehicleID].Spoiler);
            vehicle.SetMod(1, Main.VehicleTunings[vehicleID].FrontBumper);
            vehicle.SetMod(2, Main.VehicleTunings[vehicleID].RearBumper);

            vehicle.SetMod(3, Main.VehicleTunings[vehicleID].SideSkirt);// 3
            vehicle.SetMod(4, Main.VehicleTunings[vehicleID].Exhaust);// 4
            vehicle.SetMod(5, Main.VehicleTunings[vehicleID].Frame);// 5
            vehicle.SetMod(6, Main.VehicleTunings[vehicleID].Grille);// 6
            vehicle.SetMod(7, Main.VehicleTunings[vehicleID].Hood);// 7
            vehicle.SetMod(8, Main.VehicleTunings[vehicleID].Fender);// 8
            vehicle.SetMod(9, Main.VehicleTunings[vehicleID].RightFender);
            vehicle.SetMod(10, Main.VehicleTunings[vehicleID].Roof);
            NAPI.Task.Run(() => //toto КОСТЫЛЬ
            {
                vehicle.SetMod(11, Main.VehicleTunings[vehicleID].Engine);
            }, delayTime: 2000);
            

            vehicle.SetMod(12, Main.VehicleTunings[vehicleID].Brakes);
            vehicle.SetMod(13, Main.VehicleTunings[vehicleID].Transmission);
            vehicle.SetMod(14, Main.VehicleTunings[vehicleID].Horns);
            vehicle.SetMod(15, Main.VehicleTunings[vehicleID].Suspension);
            vehicle.SetMod(16, Main.VehicleTunings[vehicleID].Armor);// 16
            vehicle.SetMod(18, Main.VehicleTunings[vehicleID].Turbo);// 18
            vehicle.SetMod(22, Main.VehicleTunings[vehicleID].Xenon);// 22
            vehicle.SetMod(23, Main.VehicleTunings[vehicleID].FrontWheels);//23
            vehicle.SetMod(24, Main.VehicleTunings[vehicleID].BackWheels);//24 Only for Motorcycles

            vehicle.SetMod(25, Main.VehicleTunings[vehicleID].PlateHolders);//25
            vehicle.SetMod(27, Main.VehicleTunings[vehicleID].TrimDesign);//27
            vehicle.SetMod(28, Main.VehicleTunings[vehicleID].Ornaments);//28
            vehicle.SetMod(30, Main.VehicleTunings[vehicleID].DialDesign);//30

            vehicle.SetMod(33, Main.VehicleTunings[vehicleID].SteeringWheel);//33
            vehicle.SetMod(34, Main.VehicleTunings[vehicleID].ShiftLever);//34
            vehicle.SetMod(35, Main.VehicleTunings[vehicleID].Plaques);//35
            vehicle.SetMod(38, Main.VehicleTunings[vehicleID].Hydraulics);//38
            vehicle.SetMod(40, Main.VehicleTunings[vehicleID].Boost);//40
            vehicle.SetMod(48, Main.VehicleTunings[vehicleID].Livery);//48
            vehicle.SetMod(53, Main.VehicleTunings[vehicleID].Plate);//53
            vehicle.SetMod(55, Main.VehicleTunings[vehicleID].WindowTint);//55

        }

        [RemoteEvent("remote_SetTunning")]
        public void SetTunning(Player player, object[] args)
        {
            int modeType = (int)args[0];
            int modeIndex = (int)args[1];

            player.Vehicle.SetMod(modeType, modeIndex);
            if(Test.Debug)
            {
                player.SendChatMessage($"{modeType} - {modeIndex}");
            }
            
        }

       
    }
}
