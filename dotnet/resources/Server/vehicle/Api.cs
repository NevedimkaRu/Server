using System;
using System.Collections.Generic;
using System.Text;
using Server.model;
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
            veh.Handling = 4;
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
            MySql.Query($"INSERT INTO `vehicletuning` (`CarId`) VALUES ('{veh.Id}')");
            Tuning.LoadTunning(veh.Id);
            return carid;
            //Handling.CreateDefaultHandling(veh.Id, 0);
        }
        public static async Task LoadPlayerVehice(Player player)
        {
            if (!Main.Players1.ContainsKey(player)) return;
            DataTable dt = await MySql.QueryReadAsync($"SELECT * FROM `vehicles` JOIN `vehiclesgarage` ON vehicles.Id = vehiclesgarage.VehicleId WHERE vehicles.OwnerId = '{Main.Players1[player].Character.Id}'");
            if (dt == null || dt.Rows.Count == 0)
            {
                return;
            }

            foreach (DataRow row in dt.Rows)
            {
                Vehicles model = new Vehicles();
                model.Id = Convert.ToInt32(row["Id"]);
                model.ModelHash = Convert.ToString(row["ModelHash"]);
                model.OwnerId = Convert.ToInt32(row["OwnerId"]);
                model.Handling = Convert.ToInt32(row["Handling"]);
                if(model.Handling > 2 && Main.Players1[player].Character.Vip == 0 || model.Handling > 5 || model.Handling < 0)
                {
                    model.Handling = 0;
                }
                VehiclesGarage garage = new VehiclesGarage();
                garage.Id = Convert.ToInt32(row["Id"]);
                garage.VehicleId = Convert.ToInt32(row["VehicleId"]);
                garage.GarageId = Convert.ToInt32(row["GarageId"]);
                garage.GarageSlot = Convert.ToInt32(row["GarageSlot"]);

                model._Garage = garage;

                if (!Main.Veh.ContainsKey(model.Id))
                {
                    Main.Veh.Add(model.Id, model);
                }
                Tuning.LoadTunning(model.Id);
                Handling.LoadVehicleHandling(model.Id);
            }

            foreach (var veh in Main.Veh)
            {
                if (veh.Value.OwnerId == Main.Players1[player].Character.Id)
                {
                    if (veh.Value._Garage.GarageId == -1) continue;
                    if (veh.Value._Garage.GarageSlot <= Main.GarageTypes[Main.Garage[veh.Value._Garage.GarageId].GarageType].VehiclePosition.Count - 1)
                    {
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
                    if (Main.Veh[carid]._HandlingData.Count == 0)
                    {
                        Handling.LoadVehicleHandling(carid, true);
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

                        int caridd = player.Vehicle.GetData<int>("CarId");
                        if (caridd == carid) return;
                        player.WarpOutOfVehicle();
                        if(Main.Veh.ContainsKey(caridd))
                        {
                            if (Main.Veh[carid].OwnerId == Main.Players1[player].Character.Id);
                            {
                                var vehpos = Main.GarageTypes[Main.Garage[Main.Veh[caridd]._Garage.GarageId].GarageType].VehiclePosition;
                                Main.Veh[caridd]._Veh.Delete();
                                Main.Veh[caridd]._Veh = null;
                                SpawnPlayerVehicle
                                    (
                                        Main.Veh[caridd].Id,
                                        new Vector3
                                        (
                                            vehpos[Main.Veh[caridd]._Garage.GarageSlot].Position.X,
                                            vehpos[Main.Veh[caridd]._Garage.GarageSlot].Position.Y,
                                            vehpos[Main.Veh[caridd]._Garage.GarageSlot].Position.Z
                                        ),
                                        vehpos[Main.Veh[caridd]._Garage.GarageSlot].Rotation,
                                        (uint)Main.Veh[caridd]._Garage.GarageId
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

        public void RemoveVehicle(int carid)
        {
            if (!Main.Veh.ContainsKey(carid)) return;
            if (Main.Veh[carid]._Garage != null)
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
            Main.Veh[carid].Delete();
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
