using System;
using System.Collections.Generic;
using System.Text;
using RAGE;
using RAGE.NUI;
using RAGE.Elements;

namespace cs_packages.player
{
    public class HouseMenu : Events.Script
    {
        MenuPool menuPool;
        bool menuactive;
        
        public HouseMenu()
        {
            Events.Add("trigger_ShowHouseInfo", ShowHouseInfo);
            Events.Add("trigger_ShowExitHouseInfo", ShowExitHouseInfo);
            Events.Add("trigger_ShowHouseBuyMenu", ShowHouseBuyMenu);
            Events.Add("trigger_RequestPlayerIpl", RequestPlayerIpl);
            
        }

        private void RequestPlayerIpl(object[] args)
        {
            string ipl = Convert.ToString(args[0]);

            RAGE.Game.Streaming.RequestIpl(ipl);
        }


        private void ShowExitHouseInfo(object[] args)
        {
            if (menuactive) return;
            menuactive = true;
            Chat.Activate(false);
            Chat.Show(false);

            bool hasGarage = Convert.ToBoolean(args[0]);

            menuPool = new MenuPool();
            var mainMenu = new UIMenu("Выход", "Выберите куда вы хотите попасть");

            UIMenuItem exit = new UIMenuItem("Улица");
            exit.SetItemData(0);
            mainMenu.AddItem(exit);

            UIMenuItem garage = new UIMenuItem("Гараж");
            if(hasGarage)
            {
                garage.SetItemData(1);
                mainMenu.AddItem(garage);
            }

            UIMenuItem close = new UIMenuItem("Закрыть");
            close.SetItemData(2);
            mainMenu.AddItem(close);

            mainMenu.OnItemSelect += (sender, item, index) =>
            {

                int itemdata = Convert.ToInt32(item.ItemData);
                if (itemdata == 0)
                {
                    Chat.Activate(true);
                    Chat.Show(true);
                    menuactive = false;
                    Events.Tick -= DrawMenu;

                    Events.CallRemote("remote_ExitHouse");
                    return;
                }
                if(itemdata == 1)
                {
                    Chat.Activate(true);
                    Chat.Show(true);
                    menuactive = false;
                    Events.Tick -= DrawMenu;

                    Events.CallRemote("remote_EnterGarage",Player.LocalPlayer.Dimension,1);
                }
                if(itemdata == 2)
                {
                    Chat.Activate(true);
                    Chat.Show(true);
                    menuactive = false;
                    Events.Tick -= DrawMenu;
                }
            };
            menuPool.Add(mainMenu);

            menuPool.RefreshIndex();
            Events.Tick += DrawMenu;
            mainMenu.Visible = true;

            mainMenu.OnMenuClose += (sender) =>
            {
                Chat.Activate(true);
                Chat.Show(true);
                menuactive = false;
                Events.Tick -= DrawMenu;
            };
        }
        private void ShowHouseBuyMenu(object[] args)
        {
            if (menuactive) return;
            menuactive = true;
            Chat.Activate(false);
            Chat.Show(false);
            int houseid = Convert.ToInt32(args[0]);  
            int cost = Convert.ToInt32(args[1]);

            menuPool = new MenuPool();
            var mainMenu = new UIMenu("Покупка дома", $"Дом[{houseid}]");   

            menuPool.Add(mainMenu);
            var buyitem = new UIMenuItem($"Купить дом");
            buyitem.SetRightLabel($"~g~{cost}$");

            mainMenu.AddItem(buyitem);

            mainMenu.AddItem(new UIMenuItem("~r~Закрыть"));
            
            mainMenu.OnItemSelect += (sender, item, index) =>
            {
                if(index == 0)
                {
                    Events.CallRemote("remote_BuyHouse", houseid);
                    Chat.Activate(true);
                    Chat.Show(true);
                    menuactive = false;
                    menuPool.CloseAllMenus();
                    Events.Tick -= DrawMenu;
                }
                else
                {
                    Chat.Activate(true);
                    Chat.Show(true);
                    menuactive = false;
                    menuPool.CloseAllMenus();
                    Events.Tick -= DrawMenu;
                }
            };

            menuPool.RefreshIndex();
            Events.Tick += DrawMenu;
            mainMenu.Visible = true;

            mainMenu.OnMenuClose += (sender) =>
            {
                Chat.Activate(true);
                Chat.Show(true);
                menuactive = false;
                Events.Tick -= DrawMenu;
            };
        }

        private void DrawMenu(List<Events.TickNametagData> nametags)
        {
            if (menuactive)
            {
                menuPool.ProcessMenus();
            }
        }

        private void ShowHouseInfo(object[] args)
        {
            if (menuactive) return;
            menuactive = true;
            Chat.Activate(false);
            Chat.Show(false);
            int houseid = Convert.ToInt32(args[0]);
            string owner = Convert.ToString(args[1]);
            bool isOwner = Convert.ToBoolean(args[2]);
            bool isClosed = Convert.ToBoolean(args[3]);
            bool hasGarage = Convert.ToBoolean(args[4]);

            menuPool = new MenuPool();
            var mainMenu = new UIMenu($"Дом[{houseid}]", $"Владелец: {owner}");

            menuPool.Add(mainMenu);
            var enterhouse = new UIMenuItem(isClosed ? "~r~Войти" : "~g~Войти");
            enterhouse.SetItemData(0);
            mainMenu.AddItem(enterhouse);
            UIMenuItem closehouse = new UIMenuItem(isClosed ? "Открыть дом" : "Закрыть дом");
            UIMenuItem garagehouse = new UIMenuItem("Войти в гараж");
            if (isOwner)
            {
                closehouse.SetItemData(1);
                mainMenu.AddItem(closehouse);
            }              
            if (isOwner)
            {
                garagehouse.SetItemData(3);
                mainMenu.AddItem(garagehouse);
            }            

            var close = new UIMenuItem("~r~Закрыть");
            close.SetItemData(2);
            mainMenu.AddItem(close);

            mainMenu.OnItemSelect += (sender, item, index) =>
            {
                int itemdata = Convert.ToInt32(item.ItemData);
                if (itemdata == 1)
                {
                    isClosed = !isClosed;
                    Events.CallRemote("remote_CloseHouse", houseid, isClosed);
                    closehouse.Text = isClosed ? "Открыть дом" : "Закрыть дом";
                    enterhouse.Text = isClosed ? "~r~Войти" : "~g~Войти";
                    return;
                }
                else if (itemdata == 0)
                {
                    Events.CallRemote("remote_EnterHouse", houseid);
                    Chat.Activate(true);
                    Chat.Show(true);
                    menuactive = false;
                    menuPool.CloseAllMenus();
                    Events.Tick -= DrawMenu;
                }
                else if (itemdata == 3)
                {
                    Events.CallRemote("remote_EnterGarage", houseid, 0);
                    Chat.Activate(true);
                    Chat.Show(true);
                    menuactive = false;
                    menuPool.CloseAllMenus();
                    Events.Tick -= DrawMenu;
                }
                else if(itemdata == 2)
                {
                    Chat.Activate(true);
                    Chat.Show(true);
                    menuactive = false;
                    menuPool.CloseAllMenus();
                    Events.Tick -= DrawMenu;
                }
            };

            menuPool.RefreshIndex();
            Events.Tick += DrawMenu;
            mainMenu.Visible = true;

            mainMenu.OnMenuClose += (sender) =>
            {
                Chat.Activate(true);
                Chat.Show(true);
                menuactive = false;
                Events.Tick -= DrawMenu;
            };
        }
    }
}
