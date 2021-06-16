using GTANetworkAPI;
using Newtonsoft.Json;
using Server.Interface;
using Server.model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Server.vehicle
{
    class Store : Script
    {
        [ServerEvent(Event.ResourceStart)]
        public void LoadVehicleStore()
        {
            DataTable dt = MySql.QueryRead("select * from vehiclestore");
            foreach (DataRow row in dt.Rows)
            {
                VehicleStore model = new VehicleStore();
                model.LoadByDataRow(row);
                Main.VehicleStore.Add(model);
            }
        }

        [RemoteEvent("remote_GetVehicleStore")]
        public void GetVehicleStore(Player player)
        {
            player.TriggerEvent("trigger_SendVehicleStore", Main.VehicleStore);
        }

        public void BuyVehicle(Player player, int VehicleStoreId, int garageId)
        {
            VehicleStore vs = Main.VehicleStore.Find(c => c.Id == VehicleStoreId);
            Character character = Main.Players1[player].Character;
            if (character.Level < vs.Level)
            {
                Vui.ErrorNotify(player, "Ваш уровень не соответствует уровню автомобиля");
                return;
            }
            if (character.Money < vs.Cost)
            {
                Vui.ErrorNotify(player, "У вас не достаточно средств");
                return;
            }

            Server.character.Api.GivePlayerMoney(player, -vs.Cost);
            int carid = Api.AddVehicle(character.Id, vs.Hash);
            Api.SetVehicleInGarage(player, carid, garageId);
        }

        [RemoteProc("remote_GetVehStoreData")]
        public string GetVehicleStoreData(Player player)
        {
            return JsonConvert.SerializeObject(Main.VehicleStore).ToString();
        }

        [RemoteEvent("remote_getFreeGarages")]
        public void GetFreeGarages(Player player, int vehicleStoreId) 
        {
            if (Main.VehicleStore.Find(c => c.Id == vehicleStoreId).Cost > Main.Players1[player].Character.Money)
            {
                player.TriggerEvent("trigger_ErrorVehicleStore", "Недостаточно средств");
                return;
            }
            List<Garage> garages = new List<Garage>();
            //todo Надо бы хранить гаражи игрока в модели PlayerModel а то чёт херня какая-то постоянно искать их во всём списке
            foreach (Garage g in Main.Garage.Values)
            {
                if (g.CharacterId == Main.Players1[player].Character.Id) 
                {
                    int count = Main.GarageTypes[Main.Garage[g.Id].GarageType].VehiclePosition.Count;

                    List<int> slots = new List<int>();

                    foreach (Vehicles veh in Main.Veh.Values)
                    {
                        if (veh._Garage.GarageId == g.Id)
                        {
                            slots.Add(veh._Garage.GarageSlot);
                            if (slots.Count >= count)
                            {
                                break;
                            }
                        }
                    }
                    if (slots.Count >= count) continue;
                    garages.Add(g);
                }
            }
            if (garages.Count == 0) 
            {
                player.TriggerEvent("trigger_ErrorVehicleStore", "В Ваших гаражах нету места для транспорта");
                return;
            }
            player.TriggerEvent("trigger_SendGaragesVS", garages);

        }

        [RemoteEvent("remote_BuyVehicle")]
        public void RemoteBuyVehicle(Player player, int VehicleStoreId, int GarageId)
        {
            BuyVehicle(player, VehicleStoreId, GarageId);
        }

        [Command("buycar", GreedyArg = true)]
        public void buycar(Player player, string vehId, string garageId)
        {
            BuyVehicle(player, Convert.ToInt32(vehId), Convert.ToInt32(garageId));
        }

    }
}
