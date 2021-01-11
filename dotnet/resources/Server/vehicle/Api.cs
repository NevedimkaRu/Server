using System;
using System.Collections.Generic;
using System.Text;
using Server.model;
using GTANetworkAPI;
using System.Data;

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
            DataTable result = MySql.QueryRead($"SELECT * FROM `cars` WHERE `Owner` = '{Main.Players[player].Username}' , `Id` = '{carid}'");
            foreach(DataRow row in result.Rows)
            {
                vehModel.Id = Convert.ToInt32( row["Id"]);
                vehModel.ModelHash = Convert.ToString(row["ModelHash"]);
                vehModel.Owner = Convert.ToString(row["Owner"]);
            }
            
            Vector3 player_pos = player.Position;
            vehModel.Veh = NAPI.Vehicle.CreateVehicle(VehicleHash.Tampa, player_pos, 2f, new Color(0, 255, 100), new Color(0));
            Main.Vehicles.Add(vehModel.Id, vehModel);
        }
        public void UnLoadVehicle(Player player, int carid)
        {
            Main.Vehicles.Remove(carid);
            Main.Vehicles[carid].Veh.Delete();
        }
        [Command("car",GreedyArg = true)]
        public void cmd_Car(Player player, int carid)
        {
            if(Main.Vehicles.ContainsKey(carid))
            {
                UnLoadVehicle(player, carid);
                return;
            }
            LoadVehicle(player, carid);
        }

        [Command("addcar",GreedyArg = true)]
        public void cmd_AddCar(string player_name, string vehhash)
        {

        }
    }
}
