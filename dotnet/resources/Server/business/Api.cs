using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Server.model;

namespace Server.business
{
    public class Api : Script
    {
        public Api()
        {
            Server.Events.OnPlayerPressEKey += Events_OnPlayerPressEKey;
        }

        private void Events_OnPlayerPressEKey(Player player)
        {
            foreach (var biz in Main.Biz.Values)
            {
                if (player.Position.DistanceTo(biz.Position) <= 20 && biz.Type == (int)Business._BizType.Tuning)
                {
                    if (player.Vehicle != null && player.Vehicle.HasData("CarId"))
                    {
                        if (player.Vehicle.GetData<int>("CarId") == Main.Players1[player].CarId)
                        {
                            utils.Trigger.ClientEvent(player, "trigger_OpenBusinessMenu", 1);
                        }
                    }
                    break;
                }
                if (player.Position.DistanceTo(biz.Position) <= 10 && biz.Type == (int)Business._BizType.Clothes)
                {
                    if (player.Vehicle == null)
                    {
                        utils.Trigger.ClientEvent(player, "trigger_OpenBusinessMenu", 0);
                    }
                    break;
                }
                if (player.Position.DistanceTo(biz.Position) <= 10 && biz.Type == (int)Business._BizType.Vehicle)
                {
                    if (player.Vehicle == null)
                    {
                        utils.Trigger.ClientEvent(player, "trigger_OpenBusinessMenu", 2);
                    }
                    break;
                }
            }
        }
        [ServerEvent(Event.ResourceStart)]
        public void OnResourceStart()
        {
            DataTable dt = MySql.QueryRead("select * from business");

            foreach(DataRow row in dt.Rows)
            {
                Business business = new Business();
                business.LoadByDataRow(row);
                business.InitBusiness();
                Main.Biz.Add(business.Id, business);
            }
        }
    }
}
