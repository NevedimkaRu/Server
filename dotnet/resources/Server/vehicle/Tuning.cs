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
                CommandText = "SELECT * FROM `vehicletuning` WHERE `carid` = @id "
            };
            cmd.Parameters.AddWithValue("@id", carid);

            DataTable result = MySql.QueryRead(cmd);

            foreach (DataRow row in result.Rows)
            {
                tuning.Id = Convert.ToInt32(row["Id"]);
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
        public static void SaveVehicleTuning(int vehicleId, int modeType, int modeIndex)
        {
            switch(modeType)
            {
                case 0:
                    Main.VehicleTunings[vehicleId].Spoiler = modeIndex;
                    Main.VehicleTunings[vehicleId].Update("Spoiler");
                    break;
                case 1:
                    Main.VehicleTunings[vehicleId].FrontBumper = modeIndex;
                    Main.VehicleTunings[vehicleId].Update("FrontBumper");
                    break;
                case 2: 
                    Main.VehicleTunings[vehicleId].RearBumper = modeIndex;
                    Main.VehicleTunings[vehicleId].Update("RearBumper");
                    break;
                case 3: 
                    Main.VehicleTunings[vehicleId].SideSkirt = modeIndex;
                    Main.VehicleTunings[vehicleId].Update("SideSkirt");
                    break;
                case 4: 
                    Main.VehicleTunings[vehicleId].Exhaust = modeIndex;
                    Main.VehicleTunings[vehicleId].Update("Exhaust");
                    break;
                case 5: 
                    Main.VehicleTunings[vehicleId].Frame = modeIndex;
                    Main.VehicleTunings[vehicleId].Update("Frame");
                    break;
                case 6: 
                    Main.VehicleTunings[vehicleId].Grille = modeIndex;
                    Main.VehicleTunings[vehicleId].Update("Grille");
                    break;
                case 7: 
                    Main.VehicleTunings[vehicleId].Hood = modeIndex;
                    Main.VehicleTunings[vehicleId].Update("Hood");
                    break;
                case 8: 
                    Main.VehicleTunings[vehicleId].Fender = modeIndex;
                    Main.VehicleTunings[vehicleId].Update("Fender");
                    break;
                case 9: 
                    Main.VehicleTunings[vehicleId].RightFender = modeIndex;
                    Main.VehicleTunings[vehicleId].Update("RightFender");
                    break;
                case 10: 
                    Main.VehicleTunings[vehicleId].Roof = modeIndex;
                    Main.VehicleTunings[vehicleId].Update("Roof");
                    break;
                case 11: 
                    Main.VehicleTunings[vehicleId].Engine = modeIndex;
                    Main.VehicleTunings[vehicleId].Update("Engine");
                    break;
                case 12: 
                    Main.VehicleTunings[vehicleId].Brakes = modeIndex;
                    Main.VehicleTunings[vehicleId].Update("Brakes");
                    break;
                case 13: 
                    Main.VehicleTunings[vehicleId].Transmission = modeIndex;
                    Main.VehicleTunings[vehicleId].Update("Transmission");
                    break;
                case 14: 
                    Main.VehicleTunings[vehicleId].Horns = modeIndex;
                    Main.VehicleTunings[vehicleId].Update("Horns");
                    break;
                case 15:
                    Main.VehicleTunings[vehicleId].Suspension = modeIndex;
                    Main.VehicleTunings[vehicleId].Update("Suspension");
                    break;
                case 16: 
                    Main.VehicleTunings[vehicleId].Armor = modeIndex;
                    Main.VehicleTunings[vehicleId].Update("Armor");
                    break;
                case 18: 
                    Main.VehicleTunings[vehicleId].Turbo = modeIndex;
                    Main.VehicleTunings[vehicleId].Update("Turbo");
                    break;
                case 22: 
                    Main.VehicleTunings[vehicleId].Xenon = modeIndex;
                    Main.VehicleTunings[vehicleId].Update("Xenon");
                    break;
                case 23: 
                    Main.VehicleTunings[vehicleId].FrontWheels = modeIndex;
                    Main.VehicleTunings[vehicleId].Update("FrontWheels");
                    break;
                case 24: 
                    Main.VehicleTunings[vehicleId].BackWheels = modeIndex;
                    Main.VehicleTunings[vehicleId].Update("BackWheels");
                    break;
                case 25: 
                    Main.VehicleTunings[vehicleId].PlateHolders = modeIndex;
                    Main.VehicleTunings[vehicleId].Update("PlateHolders");
                    break;
                case 27: 
                    Main.VehicleTunings[vehicleId].TrimDesign = modeIndex;
                    Main.VehicleTunings[vehicleId].Update("TrimDesign");
                    break;
                case 28: 
                    Main.VehicleTunings[vehicleId].Ornaments = modeIndex;
                    Main.VehicleTunings[vehicleId].Update("Ornaments");
                    break;
                case 30: 
                    Main.VehicleTunings[vehicleId].DialDesign = modeIndex;
                    Main.VehicleTunings[vehicleId].Update("DialDesign");
                    break;
                case 33: 
                    Main.VehicleTunings[vehicleId].SteeringWheel = modeIndex;
                    Main.VehicleTunings[vehicleId].Update("SteeringWheel");
                    break;
                case 34:
                    Main.VehicleTunings[vehicleId].ShiftLever = modeIndex;
                    Main.VehicleTunings[vehicleId].Update("ShiftLever");
                    break;
                case 35:
                    Main.VehicleTunings[vehicleId].Plaques = modeIndex;
                    Main.VehicleTunings[vehicleId].Update("Plaques");
                    break;
                case 38:
                    Main.VehicleTunings[vehicleId].Hydraulics = modeIndex;
                    Main.VehicleTunings[vehicleId].Update("Hydraulics");
                    break;
                case 40:
                    Main.VehicleTunings[vehicleId].Boost = modeIndex;
                    Main.VehicleTunings[vehicleId].Update("Boost");
                    break;
                case 48:
                    Main.VehicleTunings[vehicleId].Livery = modeIndex;
                    Main.VehicleTunings[vehicleId].Update("Livery");
                    break;
                case 53:
                    Main.VehicleTunings[vehicleId].Plate = modeIndex;
                    Main.VehicleTunings[vehicleId].Update("Plate");
                    break;
                case 55:
                    Main.VehicleTunings[vehicleId].WindowTint = modeIndex;
                    Main.VehicleTunings[vehicleId].Update("WindowTint");
                    break;
                default:
                    {
                        break;
                    }
            }
        }

        public static void ApplyTuning(Vehicle vehicle, int vehicleID)
        {
            if(!Main.VehicleTunings.ContainsKey(vehicleID)) return;
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
            vehicle.SetMod(11, Main.VehicleTunings[vehicleID].Engine);

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

        public void ApplyTuning(int carid)
        {
            if (!Main.VehicleTunings.ContainsKey(carid))
            {
                LoadTunning(carid);
            }
            ApplyTuning(Main.Veh[carid]._Veh, carid);
        }
        [RemoteEvent("remote_ApplyTunning")]
        public void remoteApplyTunning(Player player, object[] args)
        {
            int carid = Convert.ToInt32(args[0]);
            ApplyTuning(carid);
        }
        [RemoteEvent("remote_SetTunning")]
        public void SetTuning(Player player, object[] args)
        {
            int modeType = (int)args[0];
            int modeIndex = (int)args[1];

            Vehicle veh = player.Vehicle;

            veh.SetMod(modeType, modeIndex);
            if(veh.HasSharedData("vehicleId"))
            {
                SaveVehicleTuning(veh.GetSharedData<int>("vehicleId"), modeType, modeIndex);
            }
            if(Test.Debug)
            {
                player.SendChatMessage($"{modeType} - {modeIndex}");
            }        
        }
    }
}
