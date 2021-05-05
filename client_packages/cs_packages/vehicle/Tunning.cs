using System;
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
            Input.Bind(VirtualKeys.Home, true, SendIndexTuning);//todo убрать
            Input.Bind(VirtualKeys.H, true, RepairCar);
        }

        public class TuningComponents
        {
            public int Component { get; set; }
            public List<int> Indexes;
            public List<string> IndexesNames;
        }


        List<string> slotNames = new List<string>() 
        {
            "Spoiler",//0
            "Front Bumper",//1
            "Rear Bumper",//2
            "Side Skirts",//3
            "Exhaust",//4
            "Frame",//5
            "Grille",//6
            "Hood",//7
            "Fender",//8
            "Right Fender",//9
            "Roof",//10
            "Engine",//11
            "Brakes",//12
            "Transmission",//13
            "Horn",//14
            "Suspension",//15
            "Armor",//16
            "",//17
            "Turbo",//18
            "",//19
            "",//20
            "",//21
            "Xenon",//22
            "Front Wheels",//23
            "Back Wheels",//24
            "Plate Holders",//25
            "",//26
            "Trim Design",//27
            "Ornaments",//28
            "",//29
            "Dial Design",//30
            "",//31
            "",//32
            "Steering Wheels",//33
            "Shift Lever",//34
            "Plaques",//35
            "",//36
            "Speakers",//37
            "Hydraulics",//38
            "Engine Block",//39
            "Air Filters",//40
            "Strut Braces",//41
            "Arch Covers",//42
            "Aerials",//43
            "Exterior Trim",//44
            "Tank",//45
            "Windows",//46
            "",//47
            "Livery",//48
        };
        private void SendIndexTuning()
        {
            List<TuningComponents> components = new List<TuningComponents>();
            for (int i = 0; i <= 48; i++)
            {
                TuningComponents model = new TuningComponents();
                model.Component = i;
                int totalmods = Player.LocalPlayer.Vehicle.GetNumMods(i);
                if (totalmods == 0 && slotNames[i].Length == 0) continue;
                Chat.Output($"{i} {slotNames[i]}");
                model.Indexes = new List<int>();
                model.IndexesNames = new List<string>();
                for (int a = 0; a < totalmods; a++)
                {
                    model.Indexes.Add(a);
                    string lablename = RAGE.Game.Ui.GetLabelText(Player.LocalPlayer.Vehicle.GetModTextLabel(i, a));

                    model.IndexesNames.Add(lablename == "NULL" ? $"{slotNames[i]} {a}" : lablename);
                }
                components.Add(model);
            }
            Events.CallRemote("remote_SendIndexTuning", components);
        }

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
