using System;
using System.Collections.Generic;
using System.Text;
using Server.model;
using GTANetworkAPI;
using System.Data;
using MySqlConnector;
/*todo
 * Проверка на уничтожение транспорта, в идеале вообще запретить уничтожение
 * Удаление Dictionary при выходе игрока с сервера(Veh, VehicleTunings) и сам транспорт
 * Скопипиздить синхронизацию транспорта с RedAge
 */
namespace Server.vehicle
{
    public class Api : Script
    {
        public void AddVehicle(string player_name, string vehhash)
        {
            MySql.Query($"INSERT INTO `vehicles` (`Owner`, `ModelHash`) VALUES ('{player_name}','{vehhash}')");
        }
        public void DestroyCar(Player player, int carid)
        {
            Main.Veh[carid]._Veh.ResetSharedData("vehicleId");//Не знаю, надо ли удалять дату, учитывая то, что дальше машина удаляется
            Main.Veh[carid]._Veh.ResetSharedData("sh_Handling");

            Main.Veh[carid]._Veh.Delete();
            Main.Veh.Remove(carid);
            Main.VehicleTunings.Remove(carid);
            player.SendChatMessage("Destroy");
        }
        [Command("cs")]
        public void cmd_CarSpawn(Player player)
        {
            LoadPlayerVehice(player);
        }
        [Command("tc")]
        public void cmd_TeleportToCar(Player player, string carid)
        {
            player.Position = Main.Veh[Convert.ToInt32(carid)]._Veh.Position;
        }
        public static void LoadPlayerVehice(Player player)
        {
            if (!Main.Players1.ContainsKey(player)) return;
            DataTable dt = MySql.QueryRead($"SELECT * FROM `vehiclesgarage` JOIN `vehicles` ON vehicles.Id = vehiclesgarage.VehicleId WHERE vehicles.OwnerId = '{Main.Players1[player].Character.Id}'");
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
                    if (veh.Value._Garage.GarageSlot <= Main.GarageTypes[Main.Garage[veh.Value._Garage.GarageId].GarageType].VehiclePosition.Count - 1
                        && veh.Value._Garage.GarageId != -1)

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
                    else
                    {
                        Tuning.LoadTunning(veh.Value.Id);
                    }
                }
            }
        }
        public void LoadVehicle(Player player, int carid)//todo сделать проверку на наличие гаража для машины
        {
            if (Main.Veh.ContainsKey(carid))//Проверка на то, создана ли машина
            {
                if (Main.Veh[carid].OwnerId == Main.Players1[player].Character.Id && player.Vehicle == null)
                {
                    if (Main.Veh[carid]._Veh.Exists) Main.Veh[carid]._Veh.Delete();//Удаляем машину

                    Main.Veh[carid]._Veh.ResetSharedData("vehicleId");
                    Main.Veh[carid]._Veh.ResetSharedData("sh_Handling");

                    Main.Veh[carid]._Veh = NAPI.Vehicle.CreateVehicle((VehicleHash)NAPI.Util.GetHashKey(Main.Veh[carid].ModelHash), player.Position, 2f, new Color(0, 255, 100), new Color(0));//и заного создаём
                    Main.Veh[carid]._Veh.SetSharedData("sh_Handling", Main.Veh[carid].Handling);
                    Main.Veh[carid]._Veh.SetSharedData("vehicleId", Main.Veh[carid].Id);

                    NAPI.Task.Run(() =>
                    {
                        player.TriggerEvent("add_SetHandling", Main.Veh[carid]._Veh.Handle, Main.Veh[carid].Handling);//todo Сделать синхронизацию между всеми игроками
                    });
                    Tuning.ApplyTuning(Main.Veh[carid]._Veh, carid);
                    player.SetIntoVehicle(Main.Veh[carid]._Veh, 0);
                }
                return;//
            }
        }
        //Тестовые команды
        [Command("car",GreedyArg = true)]
        public void cmd_Car(Player player, string caridd)
        {
            int carid = Convert.ToInt32(caridd);
            LoadVehicle(player, carid);
        }

        [Command("addcar",GreedyArg = true)]
        public void cmd_AddCar(Player player, string player_name, string vehhash)
        {
            AddVehicle(player_name, vehhash);

        }
    }
}
