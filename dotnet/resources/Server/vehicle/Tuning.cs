using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using MySqlConnector;
using System.Data;
using Server.model;
namespace Server.vehicle
{
    class Tuning : Script
    {
        public static void LoadTunning(int carid)
        {
            var tuning = new VehicleTuning();

            MySqlCommand cmd = new MySqlCommand
            {
                CommandText = "SELECT * FROM `vehiclestuning` WHERE `carid` = @id "
            };
            cmd.Parameters.AddWithValue("@id", carid);

            DataTable result = MySql.QueryRead(cmd);

            foreach (DataRow row in result.Rows)
            {
                tuning.Id = Convert.ToInt32(row["Id"]);
                tuning.Spoiler = Convert.ToInt32(row["Spoiler"]);
                tuning.FrontBumper = Convert.ToInt32(row["FrontBumper"]);
                tuning.RearBumper = Convert.ToInt32(row["RearBumper"]);
                Main.VehicleTunings.Add(carid, tuning);
            }
        }
        public static void UpdateTuning(int carid)
        {

        }

        public static void ApplyTuning(Vehicle vehicle, int carid)
        {
            vehicle.SetMod(0, Main.VehicleTunings[carid].Spoiler);
            vehicle.SetMod(1, Main.VehicleTunings[carid].FrontBumper);
            vehicle.SetMod(2, Main.VehicleTunings[carid].RearBumper);

        }

        [RemoteEvent("remote_SetTunning")]
        public void SetTunning(Player player, object[] args)
        {
            int modeType = (int)args[0];
            int modeIndex = (int)args[1];

            player.Vehicle.SetMod(modeType, modeIndex);
            if(Test.Debug)
            {
                player.SendChatMessage($"{modeType} - {modeIndex}");
            }
            
        }

       
    }
}
