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
        [ServerEvent(Event.ResourceStart)]
        public static void LoadTunningCost()
        {

            DataTable result = MySql.QueryRead("SELECT * FROM `vehicletuningcost`");

            foreach (DataRow row in result.Rows)
            {
                VehicleTuningCost tuningcost = new VehicleTuningCost();
                tuningcost.LoadByDataRow(row);
                Main.VehicleTuningsCost.Add(tuningcost);
            }
        }
        public class TuningComponents
        {
            public int Component { get; set; }
            public List<int> Indexes;
            public List<string> IndexesNames;
        }
        [RemoteEvent("remote_SendIndexTuning")]
        public void remote_SendIndexTuning(Player player, object[] args)
        {
            List<TuningComponents> components = new List<TuningComponents>();

            components = JsonConvert.DeserializeObject<List<TuningComponents>>(Convert.ToString(args[0]));
            for (int i = 0; i <= 55; i++)
            {
                if (i == 9 || i == 11 || i == 12 || i == 13 || i == 15 || i == 16 || i == 18 || i == 24) continue;
                TuningComponents component = new TuningComponents();
                component = components.Find(c => c.Component == i);
                if (component == null) continue;
                if (component.Indexes.Count == 0) continue;
                for (int a = 0; a < component.Indexes.Count; a++)
                {
                    VehicleTuningCost model = new VehicleTuningCost();
                    uint vehmodel = player.Vehicle.Model;

                    model.ModelHash = vehmodel;
                    model.Index = a;
                    model.Component = i;
                    model.Cost = a * 1000;
                    model.IndexName = component.IndexesNames[a];
                    model.Insert();
                }
            }
        }
    }
}
