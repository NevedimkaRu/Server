using System;
using System.Collections.Generic;
using System.Text;
using RAGE;
using cs_packages.Interface;
namespace cs_packages.player
{
    public class Business : Events.Script
    {
        public Business()
        {
            Events.Add("trigger_OpenBusinessMenu", OpenBusinessMenu);
        }

        private void OpenBusinessMenu(object[] args)
        {
            int bizType = Convert.ToInt32(args[0]);

            switch(bizType)
            {
                case 0://clothes
                    {
                        ClothesStore.OpenCSMenu();
                        break;
                    }
                case 1://tun
                    {
                        VehicleTuningMenu.OpenVTMenu();
                        break;
                    }
                case 2://Автосалон
                    {
                        VehicleStore.OpenVSMenu();
                        break;
                    }
            }
        }
    }
}
