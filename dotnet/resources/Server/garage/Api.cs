using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using GTANetworkAPI;
using Newtonsoft.Json;
using Server.model;

namespace Server.garage
{
    class Api : Script
    {
        [ServerEvent(Event.ResourceStart)]
        public void OnResourceStart()
        {
            DataTable dt = MySql.QueryRead("SELECT * FROM `garage`");
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
                    DataTable dtt = MySql.QueryRead("SELECT `Name` FROM `character` WHERE `Id` = '1'");
                    string charname = Convert.ToString(dtt.Rows[0]["Name"]);
                    model._TextLabel = NAPI.TextLabel.CreateTextLabel(
                        $"Гараж {model.Id}" +
                        $"\nВладелец \"{charname}\" ", 
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
        public void CreateGarage(Player player, Vector3 Position)
        {

        }
        public static void OnPlayerPressEKey(Player player)
        {
            if (!Main.Players1.ContainsKey(player)) return;
            if (!Main.Players1[player].IsSpawn || player.Vehicle == null) return;

            foreach (var garage in Main.GarageTypes)
            {
                if (player.Position.DistanceTo(garage.Value.ExitPosition) < 1.3f && Main.Players1[player].GarageId != -1)
                {
                    foreach (var veh in Main.Veh)
                    {
                        if (veh.Value.OwnerId == Main.Players1[player].Character.Id && player.Vehicle.Handle == veh.Value._Veh.Handle)
                        {

                            veh.Value._Veh.Position = Main.Garage[Main.Players1[player].GarageId].Position;
                            veh.Value._Veh.Rotation = new Vector3(veh.Value._Veh.Rotation.X, veh.Value._Veh.Rotation.Y, Main.Garage[Main.Players1[player].GarageId].Rotation);
                            veh.Value._Veh.Dimension = 0;
                            player.Position = Main.Garage[Main.Players1[player].GarageId].Position;
                            player.Dimension = 0;
                            player.SetIntoVehicle(veh.Value._Veh, 0);
                            Main.Players1[player].HouseId = -1;
                            Main.Players1[player].GarageId = -1;
                            NAPI.Task.Run(() =>
                            {
                                player.TriggerEvent("add_SetHandling", veh.Value._Veh.Handle, veh.Value.Handling);//todo Сделать синхронизацию между всеми игроками
                            });
                            NAPI.Task.Run(() => //toto КОСТЫЛЬ
                            {
                                veh.Value._Veh.SetMod(11, Main.VehicleTunings[veh.Value.Id].Engine);
                            }, delayTime: 2000);
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
                            Main.Players1[player].HouseId = garage.Value.HouseId;
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
            if (!Main.Players1.ContainsKey(player)) return;
            if (!Main.Players1[player].IsSpawn || player.Vehicle != null) return;
            foreach (var garage in Main.Garage)
            {
                if (player.Position.DistanceTo(Main.GarageTypes[garage.Value.GarageType].Position) < 1.3f)
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
        }
    }
}
