using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using GTANetworkAPI;
using Newtonsoft.Json;
using Server.model;
using Server.utils;

namespace Server.garage
{
    class Api : Script
    {
        //todo сделать продажу игроку, выгонять всех игроков из гаража при продаже.
        [ServerEvent(Event.ResourceStart)]
        public void OnResourceStart()
        {
            DataTable dt = MySql.QueryRead("SELECT garage.*, character.Name FROM `garage` Left join `character` on garage.CharacterId = character.Id");
            if (dt == null || dt.Rows.Count == 0)
            {
                return;
            }

            foreach (DataRow row in dt.Rows)
            {
                Garage model = new Garage();
                model.Id = Convert.ToInt32(row["Id"]);
                model.HouseId = Convert.ToInt32(row["HouseId"]);
                model.CharacterId = Convert.ToInt32(row["CharacterId"]);
                model.GarageType = Convert.ToInt32(row["GarageType"]);
                model.Cost = Convert.ToUInt32(row["Cost"]);
                model.Position = JsonConvert.DeserializeObject<Vector3>(row["Position"].ToString());
                model.Rotation = Convert.ToSingle(row["Rotation"]);
                model.Closed = Convert.ToBoolean(row["Closed"]);
                model._Owner = Convert.ToString(row["Name"]);

                model._Marker = NAPI.Marker.CreateMarker(36,
                       new Vector3(model.Position.X, model.Position.Y, model.Position.Z),
                       model.Position,
                       new Vector3(0, 0, 0),
                       1.0f,
                       new Color(207, 207, 207));
                if(model.HouseId != -1)
                {
                    model._TextLabel = NAPI.TextLabel.CreateTextLabel("Press \"E\" ", model.Position, 10.0f, 2.0f, 0, new Color(250, 250, 250));
                }
                else if(model.HouseId == -1 && model.CharacterId != -1)
                {   
                    model._TextLabel = NAPI.TextLabel.CreateTextLabel(
                        $"Гараж {model.Id}" +
                        $"\nВладелец \"{model._Owner}\" ", 
                        model.Position, 10.0f, 2.0f, 0, new Color(250, 250, 250));
                    model._Blip = NAPI.Blip.CreateBlip(357, model.Position,1.0f, 6);
                }
                else if(model.HouseId == -1 && model.CharacterId == -1)
                {
                    model._TextLabel = NAPI.TextLabel.CreateTextLabel(
                        $"Гараж {model.Id}. " +
                        $"\nМест для транспорта - {Main.GarageTypes[model.GarageType].VehiclePosition.Count}" +
                        $"\nСтоимость \"{model.Cost}\" ", 
                        model.Position, 10.0f, 2.0f, 0, new Color(250, 250, 250));
                    model._Blip = NAPI.Blip.CreateBlip(357, model.Position, 1.0f, 43);
                }
                Main.Garage.Add(model.Id, model);
            }
        }


        public static void RefreshGarage(int id)
        {
            if (Main.Garage[id]._Blip != null) Main.Garage[id]._Blip.Delete();
            if (Main.Garage[id]._Marker != null) Main.Garage[id]._Marker.Delete();
            if (Main.Garage[id]._TextLabel != null) Main.Garage[id]._TextLabel.Delete();
            if (Main.Garage[id]._Owner != null) Main.Garage[id]._Owner = null;

            if (Main.Garage[id].HouseId != -1)
            {
                Main.Garage[id]._TextLabel = NAPI.TextLabel.CreateTextLabel("Press \"E\" ", Main.Garage[id].Position, 10.0f, 2.0f, 0, new Color(250, 250, 250));
            }
            else if (Main.Garage[id].HouseId == -1 && Main.Garage[id].CharacterId != -1)
            {
                DataTable dtt = MySql.QueryRead("SELECT `Name` FROM `character` WHERE `Id` = '1'");
                Main.Garage[id]._Owner = Convert.ToString(dtt.Rows[0]["Name"]);
                Main.Garage[id]._TextLabel = NAPI.TextLabel.CreateTextLabel(
                    $"Гараж {id}" +
                    $"\nВладелец \"{Main.Garage[id]._Owner}\" ",
                    Main.Garage[id].Position, 10.0f, 2.0f, 0, new Color(250, 250, 250));
                Main.Garage[id]._Blip = NAPI.Blip.CreateBlip(357, Main.Garage[id].Position, 1.0f, 6);
                Main.Garage[id]._Marker = NAPI.Marker.CreateMarker(36,
                   new Vector3(Main.Garage[id].Position.X, Main.Garage[id].Position.Y, Main.Garage[id].Position.Z),
                   Main.Garage[id].Position,
                   new Vector3(0, 0, 0),
                   1.0f,
                   new Color(207, 207, 207));
            }
            else if (Main.Garage[id].HouseId == -1 && Main.Garage[id].CharacterId == -1)
            {
                Main.Garage[id]._TextLabel = NAPI.TextLabel.CreateTextLabel(
                    $"Гараж {id}. " +
                    $"\nМест для транспорта - {Main.GarageTypes[Main.Garage[id].GarageType].VehiclePosition.Count}" +
                    $"\nСтоимость \"{Main.Garage[id].Cost}\" ",
                    Main.Garage[id].Position, 10.0f, 2.0f, 0, new Color(250, 250, 250));
                Main.Garage[id]._Blip = NAPI.Blip.CreateBlip(357, Main.Garage[id].Position, 1.0f, 43);
                Main.Garage[id]._Marker = NAPI.Marker.CreateMarker(36,
                   new Vector3(Main.Garage[id].Position.X, Main.Garage[id].Position.Y, Main.Garage[id].Position.Z),
                   Main.Garage[id].Position,
                   new Vector3(0, 0, 0),
                   1.0f,
                   new Color(207, 207, 207));
            }

        }

        public static void CreateGarage(Vector3 position, float rotation, uint cost = 100000)
        {
            Garage garage = new Garage();
            garage.HouseId = -1;
            garage.CharacterId = -1;
            garage.GarageType = 0;
            garage.Position = position;
            garage.Rotation = rotation;
            garage.Cost = cost;

            int id = garage.Insert();

            Main.Garage.Add(id, garage);

            RefreshGarage(id);
        }

        public static void SellGarage(int garageid)
        {
            if (!Main.Garage.ContainsKey(garageid)) return;
            if (Main.Garage[garageid].HouseId != -1) return;
            Main.Garage[garageid].CharacterId = -1;
            Main.Garage[garageid]._Owner = null;
            Main.Garage[garageid].Update("CharacterId");

            foreach (var veh in Main.Veh)
            {
                if (veh.Value._Garage.GarageId == Main.Garage[garageid].Id)
                {
                    veh.Value._Garage.GarageId = -1;
                    veh.Value._Garage.GarageSlot = -1;
                    MySql.Query($"UPDATE `vehiclesgarage` SET `GarageId` = '{veh.Value._Garage.GarageId}', " +
                            $"`GarageSlot` = '{veh.Value._Garage.GarageSlot}' " +
                            $"WHERE `VehicleId` = '{veh.Value.Id}'");
                    veh.Value._Veh.Delete();
                }
            }        
            RefreshGarage(garageid);
        }
        public void PlayerBuyGarage(Player player, int garageid)
        {
            if (!Main.Garage.ContainsKey(garageid) || !Check.GetPlayerStatus(player, Check.PlayerStatus.Spawn)) 
                return;

            Main.Garage[garageid].CharacterId = Main.Players1[player].Character.Id;

            string textlable = $"Гараж[{Main.Garage[garageid].Id}]\nВладелец: {Main.Players1[player].Character.Name}";

            NAPI.TextLabel.SetTextLabelText(Main.Garage[garageid]._TextLabel, textlable);
            Main.Garage[garageid]._Marker.Delete();

            Main.Garage[garageid]._Marker = NAPI.Marker.CreateMarker(1,
               new Vector3(Main.Garage[garageid].Position.X, Main.Garage[garageid].Position.Y, Main.Garage[garageid].Position.Z - 1.0f),
               Main.Garage[garageid].Position,
               new Vector3(0, 0, 0),
               1.0f,
               new Color(207, 207, 207));

            Main.Garage[garageid].Update("CharacterId");
        }
        public static void OnPlayerPressEKey(Player player)
        {
            if (!Check.GetPlayerStatus(player, Check.PlayerStatus.Spawn) || player.Vehicle == null) return;

            foreach (var garage in Main.GarageTypes)
            {
                if (player.Position.DistanceTo(garage.Value.ExitPosition) < 2.0f && Main.Players1[player].GarageId != -1)//Выход из гаража на улицу(в транспорте)
                {
                    foreach (var veh in Main.Veh)
                    {
                        if(veh.Value._Veh != player.Vehicle)
                        {
                            continue;
                        }
                        if (veh.Value.OwnerId == Main.Players1[player].Character.Id && player.Vehicle.Handle == veh.Value._Veh.Handle)
                        {

                            veh.Value._Veh.Position = Main.Garage[Main.Players1[player].GarageId].Position;
                            veh.Value._Veh.Rotation = new Vector3(veh.Value._Veh.Rotation.X, veh.Value._Veh.Rotation.Y, Main.Garage[Main.Players1[player].GarageId].Rotation);
                            veh.Value._Veh.Dimension = 0;

                            player.Dimension = 0;
                            player.Position = Main.Garage[Main.Players1[player].GarageId].Position;
                            NAPI.Task.Run(() =>
                            {
                                player.SetIntoVehicle(veh.Value._Veh, 0);
                            });
                            Main.Players1[player].HouseId = -1;
                            Main.Players1[player].GarageId = -1;
                            return;
                        }
                    }
                }
            }
            foreach (var garage in Main.Garage)
            {
                if (player.Position.DistanceTo(garage.Value.Position) < 1.3f && Main.Players1[player].GarageId == -1)
                {
                    foreach(var veh in Main.Veh)
                    {
                        if(veh.Value._Veh == null) continue;
                        if(veh.Value.OwnerId == Main.Players1[player].Character.Id && player.Vehicle.Handle == veh.Value._Veh.Handle)
                        {
                            if(veh.Value._Garage.GarageId != garage.Value.Id)
                            {
                                player.SendChatMessage("Этот транспорт не привязан к этому гаражу");
                                return;
                            }
                            if (Main.GarageTypes[garage.Value.GarageType].Ipl != null)
                            {
                                utils.Other.RequestPlayerIpl(player, Main.GarageTypes[garage.Value.GarageType].Ipl);
                            }
                            player.Position = Main.GarageTypes[garage.Value.GarageType].ExitPosition;
                            player.Dimension = (uint)garage.Value.Id;
                            Main.Players1[player].GarageId = garage.Value.Id;
                            if(garage.Value.HouseId != -1) Main.Players1[player].HouseId = garage.Value.HouseId;
                            RespawnPlayerVehicle(veh.Value.Id);
                            return;
                        }
                    }
                }
            }
        }
        public static void RespawnPlayerVehicle( int carid)
        {
            try
            {
                if (!Main.Veh.ContainsKey(carid)) return;
                Main.Veh[carid]._Veh.Position = Main.GarageTypes[Main.Garage[Main.Veh[carid]._Garage.GarageId].GarageType].VehiclePosition[Main.Veh[carid]._Garage.GarageSlot].Position;
                Main.Veh[carid]._Veh.Rotation = new Vector3(Main.Veh[carid]._Veh.Rotation.X, Main.Veh[carid]._Veh.Rotation.Y, Main.GarageTypes[Main.Garage[Main.Veh[carid]._Garage.GarageId].GarageType].VehiclePosition[Main.Veh[carid]._Garage.GarageSlot].Rotation);
                Main.Veh[carid]._Veh.Dimension = (uint)Main.Veh[carid]._Garage.GarageId;
            }
            catch { }
        }
        public static void OnPlayerPressAltKey(Player player)
        {
            if (!Check.GetPlayerStatus(player, Check.PlayerStatus.Spawn) || player.Vehicle != null) return;
            foreach (var garage in Main.Garage)
            {
                if (player.Position.DistanceTo(Main.GarageTypes[garage.Value.GarageType].Position) < 1.3f && garage.Value.Id == player.Dimension)
                {
                    if (garage.Value.HouseId != -1)
                    {
                        player.Position = Main.HousesInteriors[Main.Houses[Main.Players1[player].HouseId].InteriorId].Position;
                        player.Dimension = (uint)Main.Players1[player].HouseId;
                        Main.Players1[player].GarageId = -1;
                        if (Main.HousesInteriors[Main.Houses[Main.Players1[player].HouseId].InteriorId].InteriorIpl != null)
                        {
                            utils.Other.RequestPlayerIpl(player, Main.HousesInteriors[Main.Houses[(int)player.Dimension].InteriorId].InteriorIpl);
                        }
                    }
                    else
                    {
                        player.Position = garage.Value.Position;
                        player.Dimension = 0;
                        Main.Players1[player].GarageId = -1;
                        if(Main.GarageTypes[garage.Value.GarageType].Ipl != null)
                        {
                            utils.Other.RemovePlayerIpl(player, Main.GarageTypes[garage.Value.GarageType].Ipl);
                        }
                    }
                }
                if(player.Position.DistanceTo(garage.Value.Position) < 1.3f)
                {
                    if(garage.Value.HouseId == -1)//Если гараж не привязан к дому
                    {
                        if(garage.Value.CharacterId == -1)//покупка гаража
                        {
                            player.TriggerEvent("trigger_ShowGarageBuyMenu", garage.Value.Id, garage.Value.Cost);
                            return;
                        }
                        else//информация о гараже
                        {
                            player.TriggerEvent("trigger_ShowGarageInfo",
                                garage.Value.Id,
                                garage.Value._Owner,
                                garage.Value.CharacterId == Main.Players1[player].Character.Id ? true : false,
                                garage.Value.Closed);
                            return;
                        }
                    }
                }
            }
        }

        [RemoteEvent("remote_EnterGarage")]
        public void Remote_EnterGarage(Player player, int houseid, int type)
        {
            if (type == 0)//Вход из дома в гараж(с улицы)
            {
                if (Main.Houses[houseid].Closed)
                {
                    player.SendChatMessage("Дом закрыт");
                }
                else
                {
                    foreach (var garage in Main.Garage)
                    {
                        if (garage.Value.HouseId == houseid)
                        {
                            player.Position = Main.GarageTypes[garage.Value.GarageType].Position;
                            player.Dimension = (uint)garage.Value.Id;
                            Main.Players1[player].HouseId = houseid;
                            Main.Players1[player].GarageId = garage.Value.Id;
                            if (Main.GarageTypes[garage.Value.GarageType].Ipl != null)
                            {
                                utils.Other.RequestPlayerIpl(player, Main.GarageTypes[garage.Value.GarageType].Ipl);
                            }
                        }
                    }
                }
            }
            if (type == 1)//Вход из дома в гараж
            {
                foreach (var garage in Main.Garage)
                {
                    if (garage.Value.HouseId == houseid)
                    {
                        player.Position = Main.GarageTypes[garage.Value.GarageType].Position;
                        player.Dimension = (uint)garage.Value.Id;
                        Main.Players1[player].HouseId = houseid;
                        Main.Players1[player].GarageId = garage.Value.Id;
                        if (Main.GarageTypes[garage.Value.GarageType].Ipl != null)
                        {
                            utils.Other.RequestPlayerIpl(player, Main.GarageTypes[garage.Value.GarageType].Ipl);
                        }
                    }
                }
            }
            if(type == 2)
            {
                if(Main.Garage.ContainsKey(houseid))
                {
                    player.Position = Main.GarageTypes[Main.Garage[houseid].GarageType].Position;
                    player.Dimension = (uint)Main.Garage[houseid].Id;
                    Main.Players1[player].GarageId = Main.Garage[houseid].Id;
                    if (Main.GarageTypes[Main.Garage[houseid].GarageType].Ipl != null)
                    {
                        utils.Other.RequestPlayerIpl(player, Main.GarageTypes[Main.Garage[houseid].GarageType].Ipl);
                    }
                }
            }
        }
        [RemoteEvent("remote_CloseGarage")]
        public void Remote_CloseHouse(Player player, int garageid, bool IsClosed)
        {
            Main.Garage[garageid].Closed = IsClosed;
        }
        [RemoteEvent("remote_BuyGarage")]
        public void Remote_BuyHouse(Player player, int garageid)
        {
            player.SendChatMessage($"Вы купили гараж[{garageid}] за {Main.Garage[garageid].Cost}");
            PlayerBuyGarage(player, garageid);
        }
        [Command("sellgarage", GreedyArg = true)]
        public void cmd_SellGarage(Player player, string garageid)
        {
            if (!Check.GetPlayerStatus(player, Check.PlayerStatus.Spawn)) return;
            if (!Main.Garage.ContainsKey(Convert.ToInt32(garageid)))
            {
                player.SendChatMessage("Гараж с таким id не существует.");
            }
            if (Main.Garage[Convert.ToInt32(garageid)].HouseId != -1)
            {
                player.SendChatMessage("Нельзя продать гараж, который привязан к дому");
            }
            SellGarage(Convert.ToInt32(garageid));
            player.SendChatMessage($"Вы продали гараж {garageid}");
        }
        [Command("creategarage", GreedyArg = true)]
        public void cmd_CreateGarage(Player player)
        {
            if (player.Vehicle == null)
            {
                player.SendChatMessage("Вы должны находиться в машине");
            }
            Vehicle vehicle = player.Vehicle;

            CreateGarage(vehicle.Position, vehicle.Rotation.Z);
        }
    }
}
