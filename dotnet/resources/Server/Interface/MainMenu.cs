using GTANetworkAPI;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using Server.utils;

namespace Server.Interface
{
    class MainMenu : Script
    {
        [RemoteEvent("remote_PrepareMenuData")]
        public void PrepareMenuData(Player player)
        {
            if (!Check.GetPlayerStatus(player, Check.PlayerStatus.Spawn)) return;
            string JsonData = getJsonMenuData(player);
            player.TriggerEvent("trigger_OpenMenuData", JsonData);
        }

        private string getJsonMenuData(Player player)
        {
            
            dynamic FinalData = new JObject();

            FinalData.test = FinalData["testString"];

            //Машины
            FinalData.cars = new JArray() as dynamic;

            foreach(var car in Main.Veh)
            {
                if(car.Value.OwnerId == Main.Players1[player].Character.Id)
                {
                    dynamic carsData = new JObject();
                    carsData.carId = car.Value.Id;
                    carsData.carName = NAPI.Vehicle.GetVehicleDisplayName((VehicleHash)NAPI.Util.GetHashKey(car.Value.ModelHash));
                    FinalData.cars.Add(carsData);
                }
            }

            //Дома
            FinalData.houses = new JArray() as dynamic;
            foreach (KeyValuePair<int, model.House> house in Main.Houses)
            {
                if(house.Value.CharacterId == Main.Players1[player].Character.Id)
                {
                    dynamic housesData = new JObject();
                    housesData.houseId = house.Value.Id;
                    housesData.houseClosed = house.Value.Closed;
                    FinalData.houses.Add(housesData);
                    break;
                }
            }

            //Гаражи
            FinalData.garages = new JArray() as dynamic;
            foreach(var garage in Main.Garage)
            {
                if(garage.Value.CharacterId == Main.Players1[player].Character.Id)
                {
                    dynamic garagesData = new JObject();
                    garagesData.garageId = garage.Value.Id;
                    garagesData.garageHouseId = garage.Value.HouseId;//Если != -1, то гараж привязан к дому
                    garagesData.garageSlotCount = Main.GarageTypes[garage.Value.GarageType].VehiclePosition.Count;//Кол-во слотов в гараже
                    FinalData.garages.Add(garagesData);
                }
            }

            //Телепорты
            FinalData.teleports = new JArray() as dynamic;
            foreach(var teleport in Main.Teleports)
            {
                dynamic teleportData = new JObject();
                teleportData.tpId = teleport.Value.Id;
                teleportData.tpName = teleport.Value.Name;
                teleportData.tpDiscription = teleport.Value.Discription;
                teleportData.tpType = teleport.Value.Type;

                FinalData.teleports.Add(teleportData);
            }

            //Клан
            /*FinalData.playerclan = new JArray() as dynamic;
            foreach(var clan in Main.Clans)
            {
                if(clan.Value.Id == Main.Players1[player].Clan.Id)
                {
                    dynamic playerClanData = new JObject();
                    playerClanData.clanId = Main.Players1[player].Clan.Id;
                    playerClanData.clanTitle = clan.Value.Title;
                    //playerClanData.clanMembers = clan.Value._Members;
                    playerClanData.clanRanks = clan.Value._Ranks;
                }
            }*/

            return FinalData.ToString();
        }
    }
}
