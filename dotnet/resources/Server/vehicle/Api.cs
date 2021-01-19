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
            var vehModel = new Vehicles();
            try
            {
                MySqlCommand cmd = new MySqlCommand
                {
                    CommandText = "SELECT * FROM `vehicles` WHERE `Owner` = @ow AND `Id` = @id "
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

                Tuning.LoadTunning(carid);
                Tuning.ApplyTuning(vehModel.Veh, carid);
                
                Main.Veh.Add(vehModel.Id, vehModel);
            }
            catch(Exception ex)
            {
                NAPI.Util.ConsoleOutput($"[Load Vehicle] Ошибка при загрузке данных: {ex}");
            }
            

            
        }
        public void UnLoadVehicle(int carid)
        {
            Main.Veh[carid].Veh.Delete();
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
