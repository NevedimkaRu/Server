using System;
using System.Collections.Generic;
using System.Text;
using Server.model;
using GTANetworkAPI;
using System.Data;
using MySqlConnector;

namespace Server.vehicle
{
    public class Api : Script
    {
        public void AddVehicle(string player_name, string vehhash)
        {
            MySql.Query($"INSERT INTO `vehicles` (`Owner`, `ModelHash`) VALUES ('{player_name}','{vehhash}')");
        }
        public void LoadVehicle(Player player, int carid)
        {
            Vehicles vehModel = new Vehicles();
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

                player.SetIntoVehicle(vehModel._Veh, 0);

                Tuning.LoadTunning(carid);
                Tuning.ApplyTuning(vehModel._Veh, carid);
                
                Main.Veh.Add(vehModel.Id, vehModel);

                object[] obj = new object[3];
                obj[0] = vehModel._Veh.Id;
                obj[1] = vehModel._Veh.Handle.Value;
                obj[2] = vehModel.Handling;

                player.TriggerEvent("add_SetHandling", obj);

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
