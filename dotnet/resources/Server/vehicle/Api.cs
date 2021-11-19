using System;
using System.Collections.Generic;
using System.Text;
using Server.model;
using Server.character;
using GTANetworkAPI;
using System.Data;
using MySqlConnector;
using Newtonsoft.Json;
using Server.utils;
using System.Threading.Tasks;
/*todo
* Удаление Dictionary при выходе игрока с сервера(Veh, VehicleTunings) и сам транспорт
* Скопипиздить синхронизацию транспорта с RedAge
*/
namespace Server.vehicle
{
    public class Api : Script
    {
        
        public static int AddVehicle(int charid, string vehhash)
        {
            Vehicles veh = new Vehicles();
            veh.ModelHash = vehhash;
            veh.OwnerId = charid;
            veh.Handling = 0;
            int id = veh.Insert();
            veh.Id = id;
            int carid = veh.Id;

            VehiclesGarage garage = new VehiclesGarage();
            garage.GarageId = -1;
            garage.GarageSlot = -1;
            garage.VehicleId = veh.Id;
            garage.Id = garage.Insert();
            
            veh._Garage = garage;

            VehicleTuning tuning = new VehicleTuning();
            tuning.CarId = veh.Id;
            tuning.Id = tuning.Insert();

            veh._Tuning = tuning;

            Main.Veh.Add(id, veh);
            Main.Veh[id]._Tuning = tuning;
            //vehicle.Handling.FillAllHandlingSlots(id);
            //Tuning.LoadTunning(veh.Id);
            return carid;
        }
        public static async Task LoadPlayerVehice(Player player)
        {
            if (!Main.Players1.ContainsKey(player)) return;
            DataTable dt = await MySql.QueryReadAsync($"SELECT * FROM `vehicles` " +
                $"JOIN `vehiclesgarage` ON vehicles.Id = vehiclesgarage.VehicleId " +
                $"JOIN `vehicletuning` ON vehicles.Id = vehicletuning.CarId " +
                $"LEFT JOIN `vehiclehandling` ON vehicles.Id = vehiclehandling.CarId " +
                $"WHERE vehicles.OwnerId = '{Main.Players1[player].Character.Id}'");
            if (dt == null || dt.Rows.Count == 0)
            {
                return;
            }
            int prevCarid = -1;
            foreach (DataRow row in dt.Rows)
            {
                if (prevCarid != Convert.ToInt32(row["Id"]) && prevCarid != -1)
                {
                    vehicle.Handling.FillAllHandlingSlots(prevCarid);
                }
                Vehicles model = new Vehicles();
                model.Id = Convert.ToInt32(row["Id"]);

                model.ModelHash = Convert.ToString(row["ModelHash"]);
                model.OwnerId = Convert.ToInt32(row["OwnerId"]);
                model.Handling = Convert.ToInt32(row["Handling"]);
                if (model.Handling > 2 && Main.Players1[player].Character.Vip == 0 || model.Handling > 5 || model.Handling < 0)
                {
                    model.Handling = 0;
                }
                
                VehiclesGarage garage = new VehiclesGarage();
                garage.Id = Convert.ToInt32(row["Id1"]);
                garage.VehicleId = Convert.ToInt32(row["VehicleId"]);
                garage.GarageId = Convert.ToInt32(row["GarageId"]);
                garage.GarageSlot = Convert.ToInt32(row["GarageSlot"]);

                model._Garage = garage;
                
                VehicleTuning tuning = new VehicleTuning();

                tuning.Id = Convert.ToInt32(row["Id2"]);
                tuning.CarId = model.Id;
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

                model._Tuning = tuning;

                VehicleHandling handling = null;
                if (!(row["Slot"] is DBNull))
                {
                    handling = new VehicleHandling();
                    handling.Id = Convert.ToInt32(row["Id3"]);
                    handling.CarId = Convert.ToInt32(row["CarId"]);
                    handling.Slot = Convert.ToInt32(row["Slot"]);
                    handling.fInitialDragCoeff = Convert.ToSingle(row["fInitialDragCoeff"]);
                    handling.vecCentreOfMassOffset = JsonConvert.DeserializeObject<Vector3>(row["vecCentreOfMassOffset"].ToString());
                    handling.vecInertiaMultiplier = JsonConvert.DeserializeObject<Vector3>(row["vecInertiaMultiplier"].ToString());
                    handling.fDriveBiasFront = Convert.ToSingle(row["fDriveBiasFront"]);
                    handling.nInitialDriveGears = Convert.ToInt32(row["nInitialDriveGears"]);
                    handling.fInitialDriveForce = Convert.ToSingle(row["fInitialDriveForce"]);
                    handling.fDriveInertia = Convert.ToSingle(row["fDriveInertia"]);
                    handling.fClutchChangeRateScaleUpShift = Convert.ToSingle(row["fClutchChangeRateScaleUpShift"]);
                    handling.fClutchChangeRateScaleDownShift = Convert.ToSingle(row["fClutchChangeRateScaleDownShift"]);
                    handling.fInitialDriveMaxFlatVel = Convert.ToSingle(row["fInitialDriveMaxFlatVel"]);
                    handling.fBrakeForce = Convert.ToSingle(row["fBrakeForce"]);
                    handling.fBrakeBiasFront = Convert.ToSingle(row["fBrakeBiasFront"]);
                    handling.fHandBrakeForce = Convert.ToSingle(row["fHandBrakeForce"]);
                    handling.fSteeringLock = Convert.ToSingle(row["fSteeringLock"]);
                    handling.fTractionCurveMax = Convert.ToSingle(row["fTractionCurveMax"]);
                    handling.fTractionCurveMin = Convert.ToSingle(row["fTractionCurveMin"]);
                    handling.fTractionCurveLateral = Convert.ToSingle(row["fTractionCurveLateral"]);
                    handling.fTractionSpringDeltaMax = Convert.ToSingle(row["fTractionSpringDeltaMax"]);
                    handling.fLowSpeedTractionLossMult = Convert.ToSingle(row["fLowSpeedTractionLossMult"]);
                    handling.fTractionBiasFront = Convert.ToSingle(row["fTractionBiasFront"]);
                    handling.fTractionLossMult = Convert.ToSingle(row["fTractionLossMult"]);
                    handling.fSuspensionForce = Convert.ToSingle(row["fSuspensionForce"]);
                    handling.fSuspensionCompDamp = Convert.ToSingle(row["fSuspensionCompDamp"]);
                    handling.fSuspensionReboundDamp = Convert.ToSingle(row["fSuspensionReboundDamp"]);
                    handling.fSuspensionRaise = Convert.ToSingle(row["fSuspensionRaise"]);
                    handling.fSuspensionBiasFront = Convert.ToSingle(row["fSuspensionBiasFront"]);
                }
                if (!Main.Veh.ContainsKey(model.Id))
                {
                    if(handling != null) model._HandlingData.Add(handling);

                    Main.Veh.Add(model.Id, model);
                }
                else
                {
                    if (handling != null) Main.Veh[model.Id]._HandlingData.Add(handling);
                }
                prevCarid = model.Id;
            }

            foreach (var veh in Main.Veh)
            {
                if (veh.Value.OwnerId == Main.Players1[player].Character.Id)
                {
                    if (veh.Value._Garage.GarageId == -1) continue;
                    if (veh.Value._Garage.GarageSlot <= Main.GarageTypes[Main.Garage[veh.Value._Garage.GarageId].GarageType].VehiclePosition.Count - 1 && Main.Garage.ContainsKey(veh.Value._Garage.GarageId))
                    {//todo Сделать проверку на занятый слот
                        var vehpos = Main.GarageTypes[Main.Garage[veh.Value._Garage.GarageId].GarageType].VehiclePosition;
                        SpawnPlayerVehicle(
                            veh.Value.Id,
                            new Vector3
                            (
                                vehpos[veh.Value._Garage.GarageSlot].Position.X,
                                vehpos[veh.Value._Garage.GarageSlot].Position.Y,
                                vehpos[veh.Value._Garage.GarageSlot].Position.Z
                            ),
                            vehpos[veh.Value._Garage.GarageSlot].Rotation,
                            (uint)veh.Value._Garage.GarageId);
                    }
                    else//Если транспорт находиться на слоте, которого нету гараже
                    {
                        player.SendChatMessage($"Транспорт {veh.Value.ModelHash} отправился в резерв, т.к находился в занятом гараже [{veh.Value._Garage.GarageId}]");
                        veh.Value._Garage.GarageId = -1;
                        veh.Value._Garage.GarageSlot = -1;
                        MySql.Query($"UPDATE `vehiclesgarage` SET `GarageId` = '-1', " +
                                $"`GarageSlot` = '-1'" +
                                $"WHERE `VehicleId` = '{veh.Value.Id}'");
                    }
                }
            }
        }
        [Command("carinfo", GreedyArg = true)]
        public void CMD_CarInfo(Player player, string carid)
        {
            if (!Main.Veh.ContainsKey(Convert.ToInt32(carid))) return;
            Vehicle veh = Main.Veh[Convert.ToInt32(carid)]._Veh;
            player.SendChatMessage($"Pos:{veh.Position} Dim: {veh.Dimension} Model:{veh.Model}");
        }
        public static void SpawnPlayerVehicle(int carid, Vector3 position, float rotation, uint dimension)
        {
            if (!Main.Veh.ContainsKey(carid)) return;
            if (Main.Veh[carid]._Veh != null)
            {
                Main.Veh[carid]._Veh.Position = position;
                Main.Veh[carid]._Veh.Rotation = new Vector3(Main.Veh[carid]._Veh.Rotation.X, Main.Veh[carid]._Veh.Rotation.Y, rotation);
                Main.Veh[carid]._Veh.Dimension = dimension;

                if (Main.Veh[carid]._Tuning == null) Tuning.LoadTunning(Main.Veh[carid].Id);
                Tuning.ApplyTuning(Main.Veh[carid]._Veh, carid);
                Main.Veh[carid]._Veh.SetData<int>("CarId", carid);
                Main.Veh[carid]._Veh.SetSharedData("CarId1", carid);
                if (Main.Veh[carid]._HandlingData.Count == 0 || Main.Veh[carid]._HandlingData == null)
                {
                    Handling.FillAllHandlingSlots(carid);//todo затестить
                }
                Main.Veh[carid]._Veh.SetSharedData("sd_Handling1", Main.Veh[carid]._HandlingData.Find(c => c.Slot == Main.Veh[carid].Handling));
                Main.Veh[carid]._Veh.SetSharedData("sd_EngineMod", Main.Veh[carid]._Tuning.Engine);
            }
            else
            {
                NAPI.Task.Run(() =>
                {
                    Main.Veh[carid]._Veh = NAPI.Vehicle.CreateVehicle(
                        NAPI.Util.GetHashKey(Main.Veh[carid].ModelHash),
                        new Vector3(position.X, position.Y, position.Z),
                        rotation,
                        0,
                        0,
                        "NOMER",
                        255,
                        false,
                        true,
                        dimension);
                    if (Main.Veh[carid]._Tuning == null) Tuning.LoadTunning(Main.Veh[carid].Id);
                    Tuning.ApplyTuning(Main.Veh[carid]._Veh, carid);
                    Main.Veh[carid]._Veh.SetData<int>("CarId", carid);
                    Main.Veh[carid]._Veh.SetSharedData("CarId1", carid);
                    if (Main.Veh[carid]._HandlingData.Count == 0 || Main.Veh[carid]._HandlingData == null)
                    {
                        Handling.FillAllHandlingSlots(carid);//todo затестить
                    }
                    Main.Veh[carid]._Veh.SetSharedData("sd_Handling1", Main.Veh[carid]._HandlingData.Find(c => c.Slot == Main.Veh[carid].Handling));
                    Main.Veh[carid]._Veh.SetSharedData("sd_EngineMod", Main.Veh[carid]._Tuning.Engine);
                });
            }
        }
        public void LoadVehicle(Player player, int carid)
        {
            if (Main.Veh.ContainsKey(carid))//Проверка на то, существует ли машина
            {
                if (player.Dimension != 0)
                {
                    player.SendChatMessage("Транспорт нельзя заспавнить в интерьере/другом мире");
                    return;
                }
                if (Main.Veh[carid]._Garage.GarageId == -1)
                {
                    player.SendChatMessage("Транспорт находится в резерве. Поставьте его в гараж, чтобы телепортировать его к себе");
                    return;
                }

                if (Main.Veh[carid].OwnerId == Main.Players1[player].Character.Id)
                {
                    if (Main.Players1[player].CarId != -1 && player.Vehicle == null)
                    {
                        if (Main.Veh.ContainsKey(Main.Players1[player].CarId))
                        {
                            if (player.Vehicle != null)
                            {

                            }
                            var vehpos = Main.GarageTypes[Main.Garage[Main.Veh[Main.Players1[player].CarId]._Garage.GarageId].GarageType].VehiclePosition;
                            SpawnPlayerVehicle
                            (
                                Main.Veh[Main.Players1[player].CarId].Id,
                                new Vector3
                                (
                                    vehpos[Main.Veh[Main.Players1[player].CarId]._Garage.GarageSlot].Position.X,
                                    vehpos[Main.Veh[Main.Players1[player].CarId]._Garage.GarageSlot].Position.Y,
                                    vehpos[Main.Veh[Main.Players1[player].CarId]._Garage.GarageSlot].Position.Z
                                ),
                                vehpos[Main.Veh[Main.Players1[player].CarId]._Garage.GarageSlot].Rotation,
                                (uint)Main.Veh[Main.Players1[player].CarId]._Garage.GarageId
                            );
                        }
                    }
                    if (player.Vehicle != null)
                    {
                        /*int caridd = player.Vehicle.GetData<int>("CarId");
                        if (caridd == carid) return;*/
                        
                        int playCarId = Main.Players1[player].CarId;
                        //player.SendChatMessage(playCarId.ToString());
                        if (carid == playCarId) return;
                        uint playerDim = player.Dimension;
                        player.WarpOutOfVehicle();
                        if(Main.Veh.ContainsKey(playCarId))
                        {
                            if (Main.Veh[carid].OwnerId == Main.Players1[player].Character.Id)
                            {
                                /*if(Main.Veh[playCarId]._Veh != player.Vehicle)
                                {
                                    player.SendChatMessage("Нельзя заспавнить свой транспорт в чужом транспорте.");
                                }*/
                                var vehpos = Main.GarageTypes[Main.Garage[Main.Veh[playCarId]._Garage.GarageId].GarageType].VehiclePosition;
                                Main.Veh[playCarId]._Veh.Delete();
                                Main.Veh[playCarId]._Veh = null;
                                SpawnPlayerVehicle
                                    (
                                        Main.Veh[playCarId].Id,
                                        new Vector3
                                        (
                                            vehpos[Main.Veh[playCarId]._Garage.GarageSlot].Position.X,
                                            vehpos[Main.Veh[playCarId]._Garage.GarageSlot].Position.Y,
                                            vehpos[Main.Veh[playCarId]._Garage.GarageSlot].Position.Z
                                        ),
                                        vehpos[Main.Veh[playCarId]._Garage.GarageSlot].Rotation,
                                        (uint)Main.Veh[playCarId]._Garage.GarageId
                                    );
                            }
                        }
                    }
                    SpawnPlayerVehicle(carid, player.Position, player.Rotation.Z, player.Dimension);
                    Main.Players1[player].CarId = carid;
                    player.SetIntoVehicle(Main.Veh[carid]._Veh, 0);

                }
            }
        }

        public async void RemoveVehicle(int carid)
        {
            if (!Main.Veh.ContainsKey(carid)) return;
            await MySql.QueryAsync($"DELETE FROM `vehicles` WHERE `Id` = '{carid}';" +
                $" DELETE FROM `vehiclehandling` WHERE `CarId` = '{carid}';" +
                $" DELETE FROM `vehicletuning` WHERE `CarId` = '{carid}';" +
                $" DELETE FROM `vehiclesgarage` WHERE `VehicleId` = '{carid}'");
            /*if (Main.Veh[carid]._Garage != null)
                Main.Veh[carid]._Garage.Delete();
            if(Main.Veh[carid]._HandlingData.Count != 0)
            {
                foreach(VehicleHandling model in Main.Veh[carid]._HandlingData)
                {
                    model.Delete();
                }
            }
            if(Main.Veh[carid]._Tuning != null) Main.Veh[carid]._Tuning.Delete();
            if(Main.Veh[carid]._Veh != null) Main.Veh[carid]._Veh.Delete();
            Main.Veh[carid].Delete();*/
            Main.Veh.Remove(carid);
        }

        [Command("sellcar")]
        public void cmd_SellCar(Player player, int carid)
        {
            if (!Main.Veh.ContainsKey(carid)) return;
            if (Main.Veh[carid].OwnerId != Main.Players1[player].Character.Id) return;
            RemoveVehicle(carid);
            player.SendChatMessage("Вы продали машину");
        }

        public static void SetVehicleInGarage(Player player, int carid, int garageid)
        {
            if (Main.Veh.ContainsKey(carid) && Main.Garage.ContainsKey(garageid))
            {
                if (Main.Veh[carid].OwnerId != Main.Players1[player].Character.Id)
                {
                    player.SendChatMessage("Это не ваш транспорт");
                    return;
                }
                if (Main.Garage[garageid].CharacterId != -1 && Main.Garage[garageid].CharacterId != Main.Players1[player].Character.Id)
                {
                    player.SendChatMessage("Это не ваш гараж");
                    return;
                }
                if (Main.Garage[garageid].HouseId != -1 && Main.Houses.ContainsKey(Main.Garage[garageid].HouseId))
                {
                    if (Main.Houses[Main.Garage[garageid].HouseId].CharacterId != Main.Players1[player].Character.Id)
                    {
                        player.SendChatMessage("Это не ваш гараж");
                        return;
                    }
                }
                if (Main.Garage[garageid].CharacterId == -1 && Main.Garage[garageid].HouseId == -1)
                {
                    player.SendChatMessage("Это не ваш гараж");
                    return;
                }
                int count = Main.GarageTypes[Main.Garage[garageid].GarageType].VehiclePosition.Count;
                List<int> slots = new List<int>();

                foreach (var veh in Main.Veh)
                {
                    if (veh.Value._Garage.GarageId == garageid)
                    {
                        slots.Add(veh.Value._Garage.GarageSlot);
                        if (slots.Count >= count)
                        {
                            player.SendChatMessage("В гараже нету свободного места");
                            return;
                        }
                    }
                }
                int slot = 0;
                for (int i = 0; i < count; i++)
                {
                    if (!slots.Contains(i))
                    {
                        slot = i;
                        break;
                    }
                }
                player.SendChatMessage($"Вы поставили транспорт {Main.Veh[carid].ModelHash} в гараж {Main.Garage[garageid].Id} слот {slot}");
                Main.Veh[carid]._Garage.GarageId = garageid;
                Main.Veh[carid]._Garage.GarageSlot = slot;
                Main.Veh[carid]._Garage.Update();
                var vehpos = Main.GarageTypes[Main.Garage[garageid].GarageType].VehiclePosition;
                SpawnPlayerVehicle
                (
                    Main.Veh[carid].Id,
                    new Vector3
                    (
                        vehpos[Main.Veh[carid]._Garage.GarageSlot].Position.X,
                        vehpos[Main.Veh[carid]._Garage.GarageSlot].Position.Y,
                        vehpos[Main.Veh[carid]._Garage.GarageSlot].Position.Z
                    ),
                    vehpos[Main.Veh[carid]._Garage.GarageSlot].Rotation,
                    (uint)Main.Veh[carid]._Garage.GarageId
                );
            }
            else
            {
                player.SendChatMessage($"Транспорт {carid} не существует");
            }
        }

        [ServerEvent(Event.VehicleDamage)]
        public void OnVehicleDamage(Vehicle vehicle, float bodyHealthLose, float engineHealthLoss)
        {
            if (vehicle.Health < 100) vehicle.Repair();//todo протестировать
        }

        [ServerEvent(Event.VehicleDeath)]
        public void OnVehicleDeath(Vehicle vehicle)//todo протестировать
        {
            if(vehicle.HasData("CarId"))
            {
                int carid = Convert.ToInt32(vehicle.GetData<int>("CarId"));
                var vehpos = Main.GarageTypes[Main.Garage[Main.Veh[carid]._Garage.GarageId].GarageType].VehiclePosition;
                SpawnPlayerVehicle
                (
                    Main.Veh[carid].Id,
                    new Vector3
                    (
                        vehpos[Main.Veh[carid]._Garage.GarageSlot].Position.X,
                        vehpos[Main.Veh[carid]._Garage.GarageSlot].Position.Y,
                        vehpos[Main.Veh[carid]._Garage.GarageSlot].Position.Z
                    ),
                    vehpos[Main.Veh[carid]._Garage.GarageSlot].Rotation,
                    (uint)Main.Veh[carid]._Garage.GarageId
                );
            }
        }

        [RemoteEvent("remote_UpdatePlayerDriftScore")]
        public void UpdatePlayerDriftScore(Player player, int score)
        {
            if (Main.Players1[player].CarId != -1)
            {
                if (player.Vehicle != Main.Veh[Main.Players1[player].CarId]._Veh)
                {
                    return;
                }
                character.Api.GivePlayerExp(player, score);
                character.Api.GivePlayerMoney(player, score);
                character.Api.GivePlayerDriftScore(player, score);
                if (Main.Players1[player].TeleportId != -1)
                {
                    character.Record.CheckPlayerMapRecord(player, Main.Players1[player].TeleportId, score);
                }
                Main.Players1[player].Character.Update("Level,Exp,Money");
                character.achievements.Api.GivePlayerAchProgress(player, 0, 0, score);
                character.achievements.Api.GivePlayerAchProgress(player, 0, 1, score);
                
            }
        }
        [RemoteEvent("remote_RepairCar")]
        public void RepairCar(Player player, object[] args)
        {
            Vehicle veh = (Vehicle)args[0];
            if (veh != null)
            {
                NAPI.Vehicle.RepairVehicle(veh);
            }
        }
        [RemoteEvent("remote_SpawnPlayerCar")]
        public void Remote_SpawnPlayerVehicle(Player player, int carid)
        {
            LoadVehicle(player, carid);
        }

        private class CarSlotModel
        {
            public int carId;
            public int slotId;
            public int garageId;
        }


        [RemoteEvent("remote_ChangeCarsSlots")]
        public async void Remote_ChangeCarsSlots(Player player, string data)
        {
            List<CarSlotModel> carList = new List<CarSlotModel>();

            carList = JsonConvert.DeserializeObject<List<CarSlotModel>>(data);
            MySqlConnection conn = new MySqlConnection(MySql.connStr);
            StringBuilder command = new StringBuilder();
            foreach (var car in carList)
            {
                if (Main.Veh[car.carId]._Garage.GarageId == car.garageId && Main.Veh[car.carId]._Garage.GarageSlot == car.slotId) continue;
                Main.Veh[car.carId]._Garage.GarageId = car.garageId;
                Main.Veh[car.carId]._Garage.GarageSlot = car.slotId;
                command.Append($"UPDATE `vehiclesgarage` SET GarageId = {car.garageId}, GarageSlot = {car.slotId} WHERE VehicleId = {car.carId};");
                //Main.Veh[car.carId]._Garage.Update();
                if(Main.Players1[player].CarId != car.carId) SpawnVehicleInGarage(car.carId);
            }
            await MySql.QueryAsync(command.ToString());
        }

        public void SpawnVehicleInGarage(int carid)
        {
            if (!Main.Veh.ContainsKey(carid)) return;
            var vehpos = Main.GarageTypes[Main.Garage[Main.Veh[carid]._Garage.GarageId].GarageType].VehiclePosition;
            SpawnPlayerVehicle
            (
                Main.Veh[carid].Id,
                new Vector3
                (
                    vehpos[Main.Veh[carid]._Garage.GarageSlot].Position.X,
                    vehpos[Main.Veh[carid]._Garage.GarageSlot].Position.Y,
                    vehpos[Main.Veh[carid]._Garage.GarageSlot].Position.Z
                ),
                vehpos[Main.Veh[carid]._Garage.GarageSlot].Rotation,
                (uint)Main.Veh[carid]._Garage.GarageId
            );
        }


        //Тестовые команды
        [Command("car",GreedyArg = true)]
        public void cmd_Car(Player player, string caridd)
        {
            if (!Main.Players1.ContainsKey(player)) return;
            int carid = Convert.ToInt32(caridd);
            LoadVehicle(player, carid);
        }

        [Command("addcar",GreedyArg = true)]
        public void cmd_AddCar(Player player, string charid, string vehhash)
        {
            int carid = AddVehicle(Convert.ToInt32(charid), vehhash);
            player.SendChatMessage($"Вы создали транспорт {vehhash}[{carid}]");
        }
        [Command("tc")]
        public void cmd_TeleportToCar(Player player, string carid)
        {
            player.Position = Main.Veh[Convert.ToInt32(carid)]._Veh.Position;
        }
        [Command("setingarage", GreedyArg = true)]
        public void cmd_SetVehicleInGarage(Player player, string carid, string garageid)
        {
            SetVehicleInGarage(player, Convert.ToInt32(carid), Convert.ToInt32(garageid));
        }



    }
}
