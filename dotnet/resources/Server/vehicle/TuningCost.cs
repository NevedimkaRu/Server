using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using MySqlConnector;
using System.Data;
using Server.model;
using Newtonsoft.Json;
namespace Server.vehicle
{
    public class TuningCost : Script
    {
        public static void LoadTunningCost()
        {
            VehicleTuningCost tuningcost = new VehicleTuningCost();

            DataTable result = MySql.QueryRead("SELECT * FROM `vehicletuningcost`");

            foreach (DataRow row in result.Rows)
            {
                tuningcost.Id = Convert.ToInt32(row["Id"]);
                tuningcost.ModelHash = Convert.ToUInt32(row["ModelHash"]);
                tuningcost.Component = Convert.ToInt32(row["Component"]);
                tuningcost.Index = Convert.ToInt32(row["Index"]);
                tuningcost.Cost = Convert.ToInt32(row["Cost"]);


                Main.VehicleTuningsCost.Add(tuningcost);
            }
        }
        [RemoteEvent("remote_SendIndexTuning")]
        public void remote_SendIndexTuning(Player player, object[] args)
        {
            List<int> indexes = new List<int>();
            

            indexes = JsonConvert.DeserializeObject<List<int>>(Convert.ToString(args[0]));
            for(int i = 0; i <=75;i++)
            {
                if (indexes[i] == 0) continue;
                for(int a = indexes[i]; a >= 1; a--)
                {
                    VehicleTuningCost model = new VehicleTuningCost();
                    uint vehmodel = player.Vehicle.Model;

                    model.ModelHash = vehmodel;
                    model.Index = a;
                    model.Component = i;
                    model.Cost = a * 1000;
                    Main.VehicleTuningsCost.Add(model);
                    model.Insert();
                }
            }
            //ClanClient model = Deserialize<ClanClient>(args[0].ToString());

        }
    }
}
