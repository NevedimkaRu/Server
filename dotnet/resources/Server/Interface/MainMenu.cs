using GTANetworkAPI;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Interface
{
    class MainMenu : Script
    {
        [RemoteEvent("remote_PrepareMenuData")]
        public void PrepareMenuData(Player player)
        {
            string JsonData = getJsonMenuData(player);
            player.TriggerEvent("trigger_OpenMenuData", JsonData);
        }



        private string getJsonMenuData(Player player)
        {
            dynamic FinalData = new JObject();

            FinalData.test = "testString";

            //Машины
            FinalData.cars = new JArray() as dynamic;

            foreach (int carid in Main.Veh.Keys)
            {
                if (Main.Veh[carid].OwnerId == Main.Players1[player].Character.Id)
                {
                    if (Main.Veh[carid]._Veh != null)
                    {
                        dynamic carsData = new JObject();
                        carsData.carId = carid;
                        carsData.carName = Main.Veh[carid]._Veh.DisplayName;
                        FinalData.cars.Add(carsData);
                    }

                }
            }

            return FinalData.ToString();
        }
    }
}
