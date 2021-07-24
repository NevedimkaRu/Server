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
        public static void OnPlayerPressAltKey(Player player)
        {

        }
        public static void OnPlayerPressEKey(Player player)
        {
            foreach(var biz in Main.Biz.Values)
            {
                if(player.Position.DistanceTo(biz.Position) <= 10 && biz.Type == (int)Business._BizType.Tuning)
                {
                    if (player.Vehicle != null && player.Vehicle.HasData("vehicleId"))
                    {
                        if (player.Vehicle.GetData<int>("vehicleId") == Main.Players1[player].CarId)
                        {
                            //player.SendChatMessage("Всё четко");
                        }
                    }
                    break;
                }
            }
        }
    }
}
