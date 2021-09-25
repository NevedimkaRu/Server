using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using MySqlConnector;
using System.Data;
using Server.model;
using System.Threading.Tasks;
using Newtonsoft.Json;

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
                //tuning.LoadByDataRow(row);
                tuning.Id = Convert.ToInt32(row["Id"]);
                tuning.CarId = Convert.ToInt32(row["CarId"]);

                tuning.PrimaryColor = JsonConvert.DeserializeObject<Color>(row["PrimaryColor"].ToString());
                tuning.SecondaryColor = JsonConvert.DeserializeObject<Color>(row["SecondaryColor"].ToString());
                tuning.ColorType = Convert.ToInt32(row["ColorType"]);
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
                Main.Veh[carid]._Tuning = tuning;
            }
        }
        public static void SaveVehicleTuning(int vehicleId, int modeType, int modeIndex)
        {
            switch(modeType)
            {
                case 0:
                    Main.Veh[vehicleId]._Tuning.Spoiler = modeIndex;
                    Main.Veh[vehicleId]._Tuning.Update("Spoiler");
                    break;
                case 1:
                    Main.Veh[vehicleId]._Tuning.FrontBumper = modeIndex;
                    Main.Veh[vehicleId]._Tuning.Update("FrontBumper");
                    break;
                case 2: 
                    Main.Veh[vehicleId]._Tuning.RearBumper = modeIndex;
                    Main.Veh[vehicleId]._Tuning.Update("RearBumper");
                    break;
                case 3: 
                    Main.Veh[vehicleId]._Tuning.SideSkirt = modeIndex;
                    Main.Veh[vehicleId]._Tuning.Update("SideSkirt");
                    break;
                case 4: 
                    Main.Veh[vehicleId]._Tuning.Exhaust = modeIndex;
                    Main.Veh[vehicleId]._Tuning.Update("Exhaust");
                    break;
                case 5: 
                    Main.Veh[vehicleId]._Tuning.Frame = modeIndex;
                    Main.Veh[vehicleId]._Tuning.Update("Frame");
                    break;
                case 6: 
                    Main.Veh[vehicleId]._Tuning.Grille = modeIndex;
                    Main.Veh[vehicleId]._Tuning.Update("Grille");
                    break;
                case 7: 
                    Main.Veh[vehicleId]._Tuning.Hood = modeIndex;
                    Main.Veh[vehicleId]._Tuning.Update("Hood");
                    break;
                case 8: 
                    Main.Veh[vehicleId]._Tuning.Fender = modeIndex;
                    Main.Veh[vehicleId]._Tuning.Update("Fender");
                    break;
                case 9: 
                    Main.Veh[vehicleId]._Tuning.RightFender = modeIndex;
                    Main.Veh[vehicleId]._Tuning.Update("RightFender");
                    break;
                case 10: 
                    Main.Veh[vehicleId]._Tuning.Roof = modeIndex;
                    Main.Veh[vehicleId]._Tuning.Update("Roof");
                    break;
                case 11: 
                    Main.Veh[vehicleId]._Tuning.Engine = modeIndex;
                    Main.Veh[vehicleId]._Tuning.Update("Engine");
                    break;
                case 12: 
                    Main.Veh[vehicleId]._Tuning.Brakes = modeIndex;
                    Main.Veh[vehicleId]._Tuning.Update("Brakes");
                    break;
                case 13: 
                    Main.Veh[vehicleId]._Tuning.Transmission = modeIndex;
                    Main.Veh[vehicleId]._Tuning.Update("Transmission");
                    break;
                case 14: 
                    Main.Veh[vehicleId]._Tuning.Horns = modeIndex;
                    Main.Veh[vehicleId]._Tuning.Update("Horns");
                    break;
                case 15:
                    Main.Veh[vehicleId]._Tuning.Suspension = modeIndex;
                    Main.Veh[vehicleId]._Tuning.Update("Suspension");
                    break;
                case 16: 
                    Main.Veh[vehicleId]._Tuning.Armor = modeIndex;
                    Main.Veh[vehicleId]._Tuning.Update("Armor");
                    break;
                case 18: 
                    Main.Veh[vehicleId]._Tuning.Turbo = modeIndex;
                    Main.Veh[vehicleId]._Tuning.Update("Turbo");
                    break;
                case 22: 
                    Main.Veh[vehicleId]._Tuning.Xenon = modeIndex;
                    Main.Veh[vehicleId]._Tuning.Update("Xenon");
                    break;
                case 23: 
                    Main.Veh[vehicleId]._Tuning.FrontWheels = modeIndex;
                    Main.Veh[vehicleId]._Tuning.Update("FrontWheels");
                    break;
                case 24: 
                    Main.Veh[vehicleId]._Tuning.BackWheels = modeIndex;
                    Main.Veh[vehicleId]._Tuning.Update("BackWheels");
                    break;
                case 25: 
                    Main.Veh[vehicleId]._Tuning.PlateHolders = modeIndex;
                    Main.Veh[vehicleId]._Tuning.Update("PlateHolders");
                    break;
                case 27: 
                    Main.Veh[vehicleId]._Tuning.TrimDesign = modeIndex;
                    Main.Veh[vehicleId]._Tuning.Update("TrimDesign");
                    break;
                case 28: 
                    Main.Veh[vehicleId]._Tuning.Ornaments = modeIndex;
                    Main.Veh[vehicleId]._Tuning.Update("Ornaments");
                    break;
                case 30: 
                    Main.Veh[vehicleId]._Tuning.DialDesign = modeIndex;
                    Main.Veh[vehicleId]._Tuning.Update("DialDesign");
                    break;
                case 33: 
                    Main.Veh[vehicleId]._Tuning.SteeringWheel = modeIndex;
                    Main.Veh[vehicleId]._Tuning.Update("SteeringWheel");
                    break;
                case 34:
                    Main.Veh[vehicleId]._Tuning.ShiftLever = modeIndex;
                    Main.Veh[vehicleId]._Tuning.Update("ShiftLever");
                    break;
                case 35:
                    Main.Veh[vehicleId]._Tuning.Plaques = modeIndex;
                    Main.Veh[vehicleId]._Tuning.Update("Plaques");
                    break;
                case 38:
                    Main.Veh[vehicleId]._Tuning.Hydraulics = modeIndex;
                    Main.Veh[vehicleId]._Tuning.Update("Hydraulics");
                    break;
                case 40:
                    Main.Veh[vehicleId]._Tuning.Boost = modeIndex;
                    Main.Veh[vehicleId]._Tuning.Update("Boost");
                    break;
                case 48:
                    Main.Veh[vehicleId]._Tuning.Livery = modeIndex;
                    Main.Veh[vehicleId]._Tuning.Update("Livery");
                    break;
                case 53:
                    Main.Veh[vehicleId]._Tuning.Plate = modeIndex;
                    Main.Veh[vehicleId]._Tuning.Update("Plate");
                    break;
                case 55:
                    Main.Veh[vehicleId]._Tuning.WindowTint = modeIndex;
                    Main.Veh[vehicleId]._Tuning.Update("WindowTint");
                    break;
                default:
                    {
                        break;
                    }
            }
        }

        public static void ApplyTuning(Vehicle vehicle, int vehicleId)
        {
            vehicle.SetSharedData(Sync.VEHICLE_PRIMARY_COLOR, Main.Veh[vehicleId]._Tuning.PrimaryColor);
            vehicle.SetSharedData(Sync.VEHICLE_SECONDARY_COLOR, Main.Veh[vehicleId]._Tuning.SecondaryColor);
            vehicle.SetSharedData(Sync.VEHICLE_COLOR_TYPE, Main.Veh[vehicleId]._Tuning.ColorType);

            vehicle.SetMod(0, Main.Veh[vehicleId]._Tuning.Spoiler);
            vehicle.SetMod(1, Main.Veh[vehicleId]._Tuning.FrontBumper);
            vehicle.SetMod(2, Main.Veh[vehicleId]._Tuning.RearBumper);

            vehicle.SetMod(3, Main.Veh[vehicleId]._Tuning.SideSkirt);// 3
            vehicle.SetMod(4, Main.Veh[vehicleId]._Tuning.Exhaust);// 4
            vehicle.SetMod(5, Main.Veh[vehicleId]._Tuning.Frame);// 5
            vehicle.SetMod(6, Main.Veh[vehicleId]._Tuning.Grille);// 6
            vehicle.SetMod(7, Main.Veh[vehicleId]._Tuning.Hood);// 7
            vehicle.SetMod(8, Main.Veh[vehicleId]._Tuning.Fender);// 8
            vehicle.SetMod(9, Main.Veh[vehicleId]._Tuning.RightFender);
            vehicle.SetMod(10, Main.Veh[vehicleId]._Tuning.Roof);
            vehicle.SetMod(11, Main.Veh[vehicleId]._Tuning.Engine);

            vehicle.SetMod(12, Main.Veh[vehicleId]._Tuning.Brakes);
            vehicle.SetMod(13, Main.Veh[vehicleId]._Tuning.Transmission);
            vehicle.SetMod(14, Main.Veh[vehicleId]._Tuning.Horns);
            vehicle.SetMod(15, Main.Veh[vehicleId]._Tuning.Suspension);
            vehicle.SetMod(16, Main.Veh[vehicleId]._Tuning.Armor);// 16
            vehicle.SetMod(18, Main.Veh[vehicleId]._Tuning.Turbo);// 18
            vehicle.SetMod(22, Main.Veh[vehicleId]._Tuning.Xenon);// 22
            vehicle.SetMod(23, Main.Veh[vehicleId]._Tuning.FrontWheels);//23
            vehicle.SetMod(24, Main.Veh[vehicleId]._Tuning.BackWheels);//24 Only for Motorcycles

            vehicle.SetMod(25, Main.Veh[vehicleId]._Tuning.PlateHolders);//25
            vehicle.SetMod(27, Main.Veh[vehicleId]._Tuning.TrimDesign);//27
            vehicle.SetMod(28, Main.Veh[vehicleId]._Tuning.Ornaments);//28
            vehicle.SetMod(30, Main.Veh[vehicleId]._Tuning.DialDesign);//30

            vehicle.SetMod(33, Main.Veh[vehicleId]._Tuning.SteeringWheel);//33
            vehicle.SetMod(34, Main.Veh[vehicleId]._Tuning.ShiftLever);//34
            vehicle.SetMod(35, Main.Veh[vehicleId]._Tuning.Plaques);//35
            vehicle.SetMod(38, Main.Veh[vehicleId]._Tuning.Hydraulics);//38
            vehicle.SetMod(40, Main.Veh[vehicleId]._Tuning.Boost);//40
            vehicle.SetMod(48, Main.Veh[vehicleId]._Tuning.Livery);//48
            vehicle.SetMod(53, Main.Veh[vehicleId]._Tuning.Plate);//53
            vehicle.SetMod(55, Main.Veh[vehicleId]._Tuning.WindowTint);//55

        }

        public void BuyTuning(Player player, int modeType, int modeIndex) 
        {
            Vehicle veh = player.Vehicle;
            
            if(!Main.Veh.ContainsKey(Main.Players1[player].CarId))
            {
                if (Test.Debug)
                {
                    player.SendChatMessage("Dictionary Veh doesn't have such key");
                }
                player.TriggerEvent("trigger_buyTuningError", "Тупая ошибка в Dictionary Main.Veh");
                return;
            }
            if(Main.Veh[Main.Players1[player].CarId]._Veh != player.Vehicle) 
            {
                if (Test.Debug) 
                {
                    player.SendChatMessage("Debug: Main.Veh.[Main.Players1[player].CarId] != player.Vehicle");
                }
                return;
            }
            int tuningCost = Main.VehicleTuningsCost.Find(c => c.Component == modeType && c.Index == modeIndex && c.ModelHash == player.Vehicle.Model).Cost;
            if (Main.Players1[player].Character.Money < tuningCost)
            {
                player.TriggerEvent("trigger_buyTuningError", "Недостаточно средств");
                return;
            }

            veh.SetMod(modeType, modeIndex);

            SaveVehicleTuning(Main.Players1[player].CarId, modeType, modeIndex);
            character.Api.GivePlayerMoney(player, -tuningCost);
            player.TriggerEvent("trigger_buyTuningSuccess");
            
        }

        [RemoteEvent("remote_BuyTuning")]
        public void RemoteBuyTuningComponent(Player player, object[] args)
        {
            int modeType = (int)args[0];
            int modeIndex = (int)args[1];
            BuyTuning(player, modeType, modeIndex);
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

        [RemoteProc("remote_GetVehTuningStoreData")]
        public string GetVehTuningStoreData(Player player)
        {
                Dictionary<int, List<VehicleTuningDict>> data = GetVehicleTuningComponents(player.Vehicle.Model);
                return JsonConvert.SerializeObject(data);
        }

        public static Dictionary<int, List<VehicleTuningDict>> GetVehicleTuningComponents(uint VehHash)
        {
            Dictionary<int,List< VehicleTuningDict>> components = new Dictionary<int, List<VehicleTuningDict>>();
            int lastComponent = 0;
            List<VehicleTuningDict> list = new List<VehicleTuningDict>();
            foreach (VehicleTuningCost vtCost in Main.VehicleTuningsCost)
            {
                if (vtCost.ModelHash != VehHash) continue;
                VehicleTuningDict tuning = new VehicleTuningDict();
                int component = Convert.ToInt32(vtCost.Component);
                if (lastComponent != component)
                {
                    components.Add(lastComponent, list);
                    lastComponent = component;
                    list = new List<VehicleTuningDict>();
                }
                tuning.Index = vtCost.Index;
                tuning.Cost = vtCost.Cost;
                tuning.Name = vtCost.IndexName;
                list.Add(tuning);
                NAPI.Util.ConsoleOutput($"{component} - {tuning.Index} - {tuning.Cost}");
            }
            return components;
        }

    }
}
