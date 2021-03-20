using System;
using System.Collections.Generic;
using System.Text;
using Server.model;
using GTANetworkAPI;
using System.Data;
using MySqlConnector;
using Newtonsoft.Json;
/*todo
* Проверка на уничтожение транспорта, в идеале вообще запретить уничтожение
* Удаление Dictionary при выходе игрока с сервера(Veh, VehicleTunings) и сам транспорт
* Скопипиздить синхронизацию транспорта с RedAge
*/
namespace Server.vehicle
{
    public class Api : Script
    {
        public void AddVehicle(int charid, string vehhash)
        {
            Vehicles veh = new Vehicles();
            veh.ModelHash = vehhash;
            veh.OwnerId = charid;
            veh.Handling = 4;
            int id = veh.Insert();
            veh.Id = id;
            
            VehiclesGarage garage = new VehiclesGarage();
            garage.GarageId = -1;
            garage.GarageSlot = -1;
            garage.VehicleId = veh.Id;
            garage.Insert();

            veh._Garage = garage;
            Main.Veh.Add(id, veh);
            MySql.Query($"INSERT INTO `vehicletuning` (`CarId`) VALUES ('{veh.Id}')");
            Tuning.LoadTunning(veh.Id);
        }
        public void DestroyCar(Player player, int carid)
        {
            Main.Veh[carid]._Veh.Delete();
            Main.Veh.Remove(carid);
            Main.VehicleTunings.Remove(carid);
            player.SendChatMessage("Destroy");
        }
        public static void LoadPlayerVehice(Player player)
        {
            if (!Main.Players1.ContainsKey(player)) return;
            DataTable dt = MySql.QueryRead($"SELECT * FROM `vehicles` JOIN `vehiclesgarage` ON vehicles.Id = vehiclesgarage.VehicleId WHERE vehicles.OwnerId = '{Main.Players1[player].Character.Id}'");
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
                
                VehiclesGarage garage = new VehiclesGarage();
                garage.VehicleId = Convert.ToInt32(row["VehicleId"]);
                garage.GarageId = Convert.ToInt32(row["GarageId"]);
                garage.GarageSlot = Convert.ToInt32(row["GarageSlot"]);

                model._Garage = garage;

                if (!Main.Veh.ContainsKey(model.Id))
                {
                    Main.Veh.Add(model.Id, model);
                }
            }

            foreach(var veh in Main.Veh)
            {
                if(veh.Value.OwnerId == Main.Players1[player].Character.Id)
                {
                    if (veh.Value._Garage.GarageId == -1) continue;
                    if (veh.Value._Garage.GarageSlot <= Main.GarageTypes[Main.Garage[veh.Value._Garage.GarageId].GarageType].VehiclePosition.Count - 1)
                    {
                        var vehpos = Main.GarageTypes[Main.Garage[veh.Value._Garage.GarageId].GarageType].VehiclePosition;
                        veh.Value._Veh = NAPI.Vehicle.CreateVehicle(
                            NAPI.Util.GetHashKey(veh.Value.ModelHash),
                            new Vector3(vehpos[veh.Value._Garage.GarageSlot].Position.X, vehpos[veh.Value._Garage.GarageSlot].Position.Y, vehpos[veh.Value._Garage.GarageSlot].Position.Z),
                            vehpos[veh.Value._Garage.GarageSlot].Rotation,
                            0,
                            0,
                            "NOMER",
                            255,
                            false,
                            true,
                            (uint)veh.Value._Garage.GarageId);
                        NAPI.Task.Run(() =>
                        {
                            player.TriggerEvent("add_SetHandling", veh.Value._Veh.Handle, veh.Value.Handling);//todo Сделать синхронизацию между всеми игроками
                        });
                        Tuning.LoadTunning(veh.Value.Id);
                        Tuning.ApplyTuning(Main.Veh[veh.Value.Id]._Veh, veh.Value.Id);
                    }
                    else//Если транспорт находиться на слоте, которого нету гараже
                    {
                        player.SendChatMessage($"Транспорт {veh.Value.ModelHash} отправился в резерв, т.к находился в занятом гараже [{veh.Value._Garage.GarageId}]");
                        veh.Value._Garage.GarageId = -1;
                        veh.Value._Garage.GarageSlot = -1;
                        MySql.Query($"UPDATE `vehiclesgarage` SET `GarageId` = '-1', " +
                                $"`GarageSlot` = '-1'" +
                                $"WHERE `VehicleId` = '{veh.Value.Id}'");

                        //Tuning.LoadTunning(veh.Value.Id);//Заранее загружаем тюнинг, если вдруг он переместит транспорт в гараж на свободный слот
                        //todo возможно стоит не загружать ему тюнинг
                    }
                }
            }
        }
        public static void LoadVehicle(Player player, int carid)//todo сделать проверку на наличие гаража для машины
        {
            if (Main.Veh.ContainsKey(carid))//Проверка на то, существует ли машина
            {
                if(Main.Veh[carid]._Garage.GarageId == -1)
                {
                    player.SendChatMessage("Транспорт находится в резерве. Поставьте его в гараж, чтобы телепортировать его к себе");
                    return;
                } 
                if (Main.Veh[carid].OwnerId == Main.Players1[player].Character.Id && player.Vehicle == null)
                {
                    LoadVehicleInPos(player, carid, player.Position, player.Rotation.Z);
                    player.SetIntoVehicle(Main.Veh[carid]._Veh, 0);
                }
            }
        }
        public void SetVehicleInGarage(Player player, int carid, int garageid)
        {
            if(Main.Veh.ContainsKey(carid) && Main.Garage.ContainsKey(garageid))
            {
                if(Main.Veh[carid].OwnerId != Main.Players1[player].Character.Id)
                {
                    player.SendChatMessage("Это не ваш транспорт");
                    return;
                }
                if(Main.Garage[garageid].CharacterId != -1 && Main.Garage[garageid].CharacterId != Main.Players1[player].Character.Id)
                {
                    player.SendChatMessage("Это не ваш гараж");
                    return;
                }
                if(Main.Garage[garageid].HouseId != -1 && Main.Houses.ContainsKey(Main.Garage[garageid].HouseId))
                {
                    if(Main.Houses[Main.Garage[garageid].HouseId].CharacterId != Main.Players1[player].Character.Id)
                    {
                        player.SendChatMessage("Это не ваш гараж");
                        return;
                    }
                }
                if(Main.Garage[garageid].CharacterId == -1 && Main.Garage[garageid].HouseId == -1)
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
                        if(slots.Count >= count)
                        {
                            player.SendChatMessage("В гараже нету свободного места");
                            return;
                        }
                    }
                }
                int slot = 0;
                for(int i = 0; i < count; i++)
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

                var vehpos = Main.GarageTypes[Main.Garage[garageid].GarageType].VehiclePosition;
                Main.Veh[carid]._Veh = NAPI.Vehicle.CreateVehicle(
                    NAPI.Util.GetHashKey(Main.Veh[carid].ModelHash),
                    new Vector3(vehpos[Main.Veh[carid]._Garage.GarageSlot].Position.X, vehpos[Main.Veh[carid]._Garage.GarageSlot].Position.Y, vehpos[Main.Veh[carid]._Garage.GarageSlot].Position.Z),
                    vehpos[Main.Veh[carid]._Garage.GarageSlot].Rotation,
                    0,
                    0,
                    "NOMER",
                    255,
                    false,
                    true,
                    (uint)Main.Garage[garageid].Id);
                NAPI.Task.Run(() =>
                {
                    player.TriggerEvent("add_SetHandling", Main.Veh[carid]._Veh.Handle, Main.Veh[carid].Handling);//todo Сделать синхронизацию между всеми игроками
                });
                if(!Main.VehicleTunings.ContainsKey(carid)) Tuning.LoadTunning(Main.Veh[carid].Id);
                Tuning.ApplyTuning(Main.Veh[Main.Veh[carid].Id]._Veh, Main.Veh[carid].Id);
            }
            else
            {
                player.SendChatMessage($"Транспорт {carid} не существует");
            }
        }

        public static void LoadVehicleInPos(Player player, int carid, Vector3 pos, float rotatoin = 0f)
        {
            if (Main.Veh[carid]._Veh != null) Main.Veh[carid]._Veh.Delete();//Удаляем машину

            Main.Veh[carid]._Veh = NAPI.Vehicle.CreateVehicle(NAPI.Util.GetHashKey(Main.Veh[carid].ModelHash), pos, rotatoin, 0, 0);//и заного создаём
            Main.Veh[carid]._Veh.SetSharedData("sh_Handling", Main.Veh[carid].Handling);
            Main.Veh[carid]._Veh.SetSharedData("vehicleId", carid);
            NAPI.Chat.SendChatMessageToPlayer(player, "На сервере " + carid);

            NAPI.Task.Run(() =>
            {
                player.TriggerEvent("add_SetHandling", Main.Veh[carid]._Veh.Handle, Main.Veh[carid].Handling);//todo Сделать синхронизацию между всеми игроками
            });
            //if (!Main.VehicleTunings.ContainsKey(carid))
            //{
            //    Tuning.LoadTunning(carid);
            //}
            //Tuning.ApplyTuning(Main.Veh[carid]._Veh, carid);
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
            AddVehicle(Convert.ToInt32(charid), vehhash);
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
