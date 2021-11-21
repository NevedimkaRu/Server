using GTANetworkAPI;
using Newtonsoft.Json;
using Server.model;
using Server.utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Server.house
{
    public class Api : Script
    {
        //todo сделать оплату дома, продажу игроку, выгонять всех игроков из дома при его продаже, проверку на деньги
        public Api()
        {

        }

        [ServerEvent(Event.ResourceStart)]
        public void OnResourceStart()
        {
            DataTable dt = MySql.QueryRead("SELECT house.*, character.Name FROM `house` Left join `character` on house.CharacterId = character.Id");
            if (dt == null || dt.Rows.Count == 0)
            {
                return;
            }

            House model = new House();


            foreach (DataRow row in dt.Rows)
            {
                model.Id = Convert.ToInt32(row["Id"]);
                model.CharacterId = Convert.ToInt32(row["CharacterId"]);

                model.Position = JsonConvert.DeserializeObject<Vector3>(row["Position"].ToString());
                model.InteriorId = Convert.ToInt32(row["InteriorId"]);
                model.Cost = Convert.ToInt32(row["Cost"]);
                model.Closed = Convert.ToBoolean(row["Closed"]);
                model._Owner = Convert.ToString(row["Name"]);
                model._Dimension = (uint)model.Id;
                string textlable;

                if (model.CharacterId != -1)
                {
                    textlable = $"Дом[{model.Id}]\nВладелец: {model._Owner}";
                    model._Marker = NAPI.Marker.CreateMarker(1,
                       new Vector3(model.Position.X, model.Position.Y, model.Position.Z - 1.0f),
                       model.Position,
                       new Vector3(0, 0, 0),
                       1.0f,
                       new Color(207, 207, 207));
                    model._Blip = NAPI.Blip.CreateBlip(411,model.Position,1.0f,6, name:"Занятый дом");
                    NAPI.Blip.SetBlipShortRange(model._Blip, true);
                }
                else
                {
                    textlable = $"Дом[{model.Id}] на продажу\nСтоимость:{model.Cost}";
                    model._Marker = NAPI.Marker.CreateMarker(29,
                       new Vector3(model.Position.X, model.Position.Y, model.Position.Z),
                       model.Position,
                       new Vector3(0, 0, 0),
                       1.0f,
                       new Color(207, 207, 207));
                    model._Blip = NAPI.Blip.CreateBlip(374, model.Position, 1.0f, 43, name:"Дом на продажу");
                    NAPI.Blip.SetBlipShortRange(model._Blip, true);
                }

                model._TextLabel = NAPI.TextLabel.CreateTextLabel(textlable, model.Position, 10.0f, 2.0f, 0, new Color(250,250,250));
                

                Main.Houses.Add(model.Id, model);
            }
        }
        public static void OnPlayerPressAltKey(Player player)
        {
            if (!Check.GetPlayerStatus(player, Check.PlayerStatus.Spawn) || player.Vehicle != null) return;
            foreach (var house in Main.Houses)
            {
                if (player.Position.DistanceTo(house.Value.Position) < 1.3f)
                {
                    bool _garage = false;
                    foreach(var garage in Main.Garage)
                    {
                        if(garage.Value.HouseId == house.Value.Id)
                        {
                            _garage = true;
                            break;
                        }
                    }
                    if (house.Value.CharacterId != -1)//Если дом куплен
                    {
                        player.TriggerEvent("trigger_ShowHouseInfo",
                            house.Value.Id,
                            house.Value._Owner,
                            house.Value.CharacterId == Main.Players1[player].Character.Id ? true : false,
                            house.Value.Closed,
                            _garage);
                        return;
                    }
                    else
                    {
                        player.TriggerEvent("trigger_ShowHouseBuyMenu", house.Value.Id, house.Value.Cost);
                        return;
                    }
                }
            }
            foreach(var exit in Main.HousesInteriors)
            {
                if (player.Position.DistanceTo(exit.Position) < 1.3f)
                {
                    bool _garage = false;
                    foreach (var garage in Main.Garage)
                    {
                        if (garage.Value.HouseId == Main.Houses[Main.Players1[player].HouseId].Id)
                        {
                            _garage = true;
                            break;
                        }
                    }
                    player.TriggerEvent("trigger_ShowExitHouseInfo", _garage);
                }
            }
        }
        public static void PlayerExitHouse(Player player)
        {
            foreach (var exit in Main.HousesInteriors)
            {
                if (player.Position.DistanceTo(exit.Position) < 1.3f)
                {
                    player.Position = Main.Houses[Main.Players1[player].HouseId].Position;
                    utils.Other.RemovePlayerIpl(player, exit.InteriorIpl);
                    Main.Players1[player].HouseId = -1;
                    if (Main.Players1[player].GarageId != -1) Main.Players1[player].GarageId = -1;//На всякий случай
                    player.Dimension = 0;
                }
            }
        }
        public static void PlayerEnterHouse(Player player, int houseid)
        {
            if (Main.Houses[houseid].Closed)
            {
                player.SendChatMessage("Дом закрыт");
            }
            else
            {
                player.Position = Main.HousesInteriors[Main.Houses[houseid].InteriorId].Position;
                player.Dimension = Main.Houses[houseid]._Dimension;
                Main.Players1[player].HouseId = houseid;
                utils.Other.RequestPlayerIpl(player, Main.HousesInteriors[Main.Houses[houseid].InteriorId].InteriorIpl);
            }
        }

        public static void PlayerBuyHouse(Player player, int houseid)
        {
            if (!Main.Houses.ContainsKey(houseid) || Check.GetPlayerStatus(player, Check.PlayerStatus.Spawn)) return;
            player.SendChatMessage($"Вы купили дом[{houseid}] за {Main.Houses[houseid].Cost}");
            Main.Houses[houseid].CharacterId = Main.Players1[player].Character.Id;

            string textlable = $"Дом[{Main.Houses[houseid].Id}]\nВладелец: {Main.Players1[player].Character.Name}";

            NAPI.TextLabel.SetTextLabelText(Main.Houses[houseid]._TextLabel, textlable);
            Main.Houses[houseid]._Marker.Delete();

            Main.Houses[houseid]._Marker = NAPI.Marker.CreateMarker(1,
               new Vector3(Main.Houses[houseid].Position.X, Main.Houses[houseid].Position.Y, Main.Houses[houseid].Position.Z -1.0f),
               Main.Houses[houseid].Position,
               new Vector3(0, 0, 0),
               1.0f,
               new Color(207, 207, 207));

            Main.Houses[houseid].Update("CharacterId");
        }
        public void ClearHouseInfo(int houseid)
        {
            Main.Houses[houseid].CharacterId = -1;
            Main.Houses[houseid].Closed = false;
            string textlable = $"Дом[{Main.Houses[houseid].Id}] на продажу\nСтоимость:{Main.Houses[houseid].Cost}";

            NAPI.TextLabel.SetTextLabelText(Main.Houses[houseid]._TextLabel, textlable);
            Main.Houses[houseid]._Marker.Delete();

            Main.Houses[houseid]._Marker = NAPI.Marker.CreateMarker(29,
               new Vector3(Main.Houses[houseid].Position.X, Main.Houses[houseid].Position.Y, Main.Houses[houseid].Position.Z),
               Main.Houses[houseid].Position,
               new Vector3(0, 0, 0),
               1.0f,
               new Color(207, 207, 207));

            foreach(var garage in Main.Garage)
            {
                if(garage.Value.HouseId == houseid)
                {
                    foreach(var veh in Main.Veh)
                    {
                        if(veh.Value._Garage.GarageId == garage.Value.Id)
                        {
                            veh.Value._Garage.GarageId = -1;
                            veh.Value._Garage.GarageSlot = -1;
                            MySql.Query($"UPDATE `vehiclesgarage` SET `GarageId` = '{veh.Value._Garage.GarageId}', " +
                                $"`GarageSlot` = '{veh.Value._Garage.GarageSlot}' " +
                                $"WHERE `VehicleId` = '{veh.Value.Id}'");
                            veh.Value._Veh.Delete();
                        }
                    }
                    break;
                }
            }

            Main.Houses[houseid].Update("CharacterId");
            Main.Houses[houseid].Update("Closed");
        }

        [Command("buyhouse", GreedyArg = true)]
        public void cmd_BuyHouse(Player player, string houseid)
        {
            PlayerBuyHouse(player, Convert.ToInt32(houseid));
        }

        [Command("sellhouse", GreedyArg = true)]
        public void cmd_SellHouse(Player player, string _houseid)
        {
            int houseid = Convert.ToInt32(_houseid);

            ClearHouseInfo(houseid);
        }
    }
}
