﻿using System;
using System.Collections.Generic;
using System.Text;
using RAGE;
using RAGE.Elements;
using RAGE.NUI;
using RAGE.Ui;

namespace cs_packages.vehicle
{
    class Tunning : Events.Script
    {
        private bool menuactive = false;
        public Tunning()
        {
            Input.Bind(VirtualKeys.F4, true, ShowTunningMenu);//f4
            Input.Bind(VirtualKeys.Home, true, SendIndexTuning);
            Input.Bind(VirtualKeys.H, true, RepairCar);
        }

        private void SendIndexTuning()
        {
            List<int> indexes = new List<int>();
            for(int i = 0; i <= 75; i++)
            {
                int totalmods = Player.LocalPlayer.Vehicle.GetNumMods(i);
                indexes.Add(totalmods);

            }
            Events.CallRemote("remote_SendIndexTuning", indexes);
        }

        List<string> slotNames = new List<string>() 
        {
            "Spoiler",
            "Front Bumper",
            "Rear Bumper",
            "Side Skirts",
            "Exhaust",
            "Rollcage",
            "Grille",
            "Bonnet",
            "Fenders and Arches",
            "Fenders",
            "Roof",
            "Engine",
            "Brakes",
            "Transmission",
            "Horn",
            "Suspension",
            "Armor",
            "",
            "Turbo",
            "",
            "",
            "",
            "Headlights",
            "Front Wheels",
            "Back Wheels",
            "Plate Holders",
            "Vanity Plates",
            "Interior Trim",
            "Ornaments",
            "Interior Dash",
            "Dials",
            "Door Speakers",
            "Leather Seats",
            "Steering Wheels",
            "Column Shifters",
            "Plaques",
            "ICE",
            "Speakers",
            "Hydraulics",
            "Engine Block",
            "Air Filters",
            "Strut Braces",
            "Arch Covers",
            "Aerials",
            "Exterior Trim",
            "Tank",
            "Windows",
            "",
            "Livery"
        };


        private MenuPool menuPool;

        

        public void ShowTunningMenu()
        {
            if (menuactive) return;
            menuactive = true;
            if (Player.LocalPlayer.Vehicle == null)
            {
                Api.Notify("Вы должны находиться в транспорте");
                return;
            }
            menuPool = new MenuPool();
            var mainMenu = new UIMenu("Тюнинг", "ТЮНИНГ");

            menuPool.Add(mainMenu);

            for (int i = 0; i < slotNames.Count; i++)
            {
                int totalmods = Player.LocalPlayer.Vehicle.GetNumMods(i);
                if (totalmods > 0 && slotNames[i].Length > 0)
                {
                    var submenu = menuPool.AddSubMenu(mainMenu, slotNames[i].ToString());
                    
                    submenu.OnItemSelect += (sender, item, index) =>
                    {
                        Events.CallRemote("remote_SetTunning", sender.MenuData, index);
                    };
                    for (int modIndex = 0; modIndex < totalmods; modIndex++)
                    {
                        string lablename = RAGE.Game.Ui.GetLabelText(Player.LocalPlayer.Vehicle.GetModTextLabel(i, modIndex));
                        var newitem = new UIMenuItem(lablename == "NULL" ? $"{slotNames[i]} {modIndex}" : lablename, "Описание");
                        submenu.SetMenuData(i);
                        submenu.AddItem(newitem);
                        Chat.Output("");
                    }
                }
            }
            menuPool.RefreshIndex();
            Events.Tick += DrawMenu;
            mainMenu.Visible = true;

            mainMenu.OnMenuClose += (sender) =>
            {
                menuactive = false;
                Events.Tick -= DrawMenu;
            };
        }
        private void DrawMenu(List<Events.TickNametagData> nametags)
        {
            if(menuactive)
            {
                menuPool.ProcessMenus();
            }
            
        }

        public void RepairCar()
        {
            if (Player.LocalPlayer.Vehicle != null)
            {
                Events.CallRemote("remote_RepairCar", Player.LocalPlayer.Vehicle);
            }
        }
    }
}
