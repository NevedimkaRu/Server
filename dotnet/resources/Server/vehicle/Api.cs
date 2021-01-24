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
        public void LoadVehicle(Player player, int carid)
        {
            if (Main.Veh.ContainsKey(carid))//Проверка на то, создана ли машина
            {
                if (Main.Veh[carid].OwnerId == Main.Players[player].Id && player.Vehicle == null)
                {
                    Main.Veh[carid]._Veh.Delete();//Удаляем машину

                    Main.Veh[carid]._Veh.ResetSharedData("vehicleId");
                    Main.Veh[carid]._Veh.ResetSharedData("sh_Handling");

                    Main.Veh[carid]._Veh = NAPI.Vehicle.CreateVehicle((VehicleHash)NAPI.Util.GetHashKey(Main.Veh[carid].ModelHash), player.Position, 2f, new Color(0, 255, 100), new Color(0));//и заного создаём
                    Main.Veh[carid]._Veh.SetSharedData("sh_Handling", Main.Veh[carid].Handling);

                    NAPI.Task.Run(() =>
                    {
                        player.TriggerEvent("add_SetHandling", Main.Veh[carid]._Veh.Handle, Main.Veh[carid].Handling);//todo Сделать синхронизацию между всеми игроками
                    });
                    Main.Veh[carid]._Veh.SetSharedData("vehicleId", Main.Veh[carid].Id);
                    Tuning.ApplyTuning(Main.Veh[carid]._Veh, carid);
                    player.SetIntoVehicle(Main.Veh[carid]._Veh, 0);
                }
                return;//
            }

            Vehicles vehModel = new Vehicles();
            vehModel.SetId(carid);

            if (vehModel.OwnerId != Main.Players[player].Id)
            {
                player.SendChatMessage("Это не ваша машина");
                return;
            }

            foreach (var veh in Main.Veh)
            {
                if(veh.Value.OwnerId == Main.Players[player].Id)
                {
                    DestroyCar(player, veh.Key);
                    break;
                }
            }

            Vector3 player_pos = player.Position;
            vehModel._Veh = NAPI.Vehicle.CreateVehicle((VehicleHash)NAPI.Util.GetHashKey(vehModel.ModelHash), player_pos, 2f, new Color(0, 255, 100), new Color(0));
            Main.Veh.Add(vehModel.Id, vehModel);

            player.SetIntoVehicle(Main.Veh[carid]._Veh, 0);

            Tuning.LoadTunning(carid);

            Main.Veh[carid]._Veh.SetSharedData("sh_Handling", Main.Veh[carid].Handling);
            Main.Veh[carid]._Veh.SetSharedData("vehicleId", carid);
            NAPI.Task.Run(() =>
            {
                player.TriggerEvent("add_SetHandling", Main.Veh[carid]._Veh.Handle, Main.Veh[carid].Handling);//todo Сделать синхронизацию между всеми игроками
            });

            Tuning.ApplyTuning(Main.Veh[carid]._Veh, carid);
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
