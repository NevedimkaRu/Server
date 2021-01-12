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
            MySql.Query($"INSERT INTO `cars` (`Owner`, `ModelHash`) VALUES ('{player_name}','{vehhash}')");
        }
        public void LoadVehicle(Player player, int carid)
        {
            var vehModel = new VehicleModel();

            try
            {
                MySqlCommand cmd = new MySqlCommand
                {
                    CommandText = "SELECT * FROM `cars` WHERE `Owner` = @ow AND `Id` = @id "
                };
                cmd.Parameters.AddWithValue("@ow", Main.Players[player].Username);
                cmd.Parameters.AddWithValue("@id", carid);

                DataTable result = MySql.QueryRead(cmd);
                
                foreach (DataRow row in result.Rows)
                {
                    vehModel.ModelHash = Convert.ToString(row["ModelHash"]);
                }
                vehModel.Owner = Main.Players[player].Username;
                vehModel.Id = carid;
                Vector3 player_pos = player.Position;
                vehModel.Veh = NAPI.Vehicle.CreateVehicle((VehicleHash)NAPI.Util.GetHashKey(vehModel.ModelHash), player_pos, 2f, new Color(0, 255, 100), new Color(0));
                Main.Vehicles.Add(vehModel.Id, vehModel);
            }
            catch(Exception ex)
            {
                NAPI.Util.ConsoleOutput($"[Load Vehicle] Ошибка при загрузке данных: {ex}");
            }
            

            
        }
        public void UnLoadVehicle(int carid)
        {
            Main.Vehicles.Remove(carid);
            Main.Vehicles[carid].Veh.Delete();
        }
        //Тестовые команды
        [Command("car",GreedyArg = true)]
        public void cmd_Car(Player player, string caridd)
        {
            int carid = Convert.ToInt32(caridd);
            if(Main.Vehicles.ContainsKey(carid))
            {
                UnLoadVehicle(carid);
                return;
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
