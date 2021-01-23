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
            Main.Veh[carid]._Veh.Delete();
            Main.Veh.Remove(carid);
            Main.VehicleTunings.Remove(carid);
            player.SendChatMessage("Destroy");
        }
        public void LoadVehicle(Player player, int carid)
        {
            if(Main.Veh.ContainsKey(carid)) return;

            Vehicles vehModel = new Vehicles();
            foreach (var veh in Main.Veh)
            {
                if(veh.Value.OwnerId == Main.Players[player].Id)
                {
                    DestroyCar(player, veh.Key);
                    break;
                }
            }

            try
            {
                vehModel.SetId(carid);

                if(vehModel.OwnerId != Main.Players[player].Id)
                {
                    player.SendChatMessage("Это не ваша машина");
                    return;
                }

                Vector3 player_pos = player.Position;
                vehModel._Veh = NAPI.Vehicle.CreateVehicle((VehicleHash)NAPI.Util.GetHashKey(vehModel.ModelHash), player_pos, 2f, new Color(0, 255, 100), new Color(0));
                Main.Veh.Add(vehModel.Id, vehModel);

                player.SetIntoVehicle(Main.Veh[carid]._Veh, 0);

                Tuning.LoadTunning(carid);

                vehModel._Veh.SetSharedData("sh_Handling", vehModel.Handling);
                NAPI.Task.Run(() =>
                {
                    player.TriggerEvent("add_SetHandling", Main.Veh[carid]._Veh.Handle, Main.Veh[carid].Handling);//todo Сделать синхронизацию между всеми игроками
                });


                Tuning.ApplyTuning(vehModel._Veh, carid);
            }
            catch(Exception ex)
            {
                NAPI.Util.ConsoleOutput($"[Load Vehicle] Ошибка при загрузке данных: {ex}");
            }
        }
        public void UnLoadVehicle(int carid)
        {
            Main.Veh[carid]._Veh.Delete();
            Main.Veh.Remove(carid);
        }
        //Тестовые команды
        [Command("car",GreedyArg = true)]
        public void cmd_Car(Player player, string caridd)
        {
            int carid = Convert.ToInt32(caridd);
            if(Main.Veh.ContainsKey(carid))
            {
                UnLoadVehicle(carid);
            }
            LoadVehicle(player, carid);
        }

        [Command("addcar",GreedyArg = true)]
        public void cmd_AddCar(Player player, string player_name, string vehhash)
        {
            AddVehicle(player_name, vehhash);

        }
    }
}
