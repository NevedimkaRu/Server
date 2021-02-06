using System;
using System.Collections.Generic;
using System.Text;
using RAGE;
using RAGE.Elements;
using RAGE.NUI;
namespace cs_packages.player
{
    class GarageMenu : Events.Script
    {
        MenuPool menuPool;
        bool menuactive;

        public GarageMenu()
        {
            Events.Add("trigger_ShowGarageInfo", ShowHouseInfo);
            Events.Add("trigger_ShowGarageBuyMenu", ShowHouseBuyMenu);
        }

        private void ShowHouseBuyMenu(object[] args)
        {
            if (menuactive) return;
            menuactive = true;
            Chat.Activate(false);
            Chat.Show(false);
            int garageid = Convert.ToInt32(args[0]);
            int cost = Convert.ToInt32(args[1]);

            menuPool = new MenuPool();
            var mainMenu = new UIMenu("Покупка гаража", $"Гараж[{garageid}]");

            menuPool.Add(mainMenu);
            var buyitem = new UIMenuItem($"Купить гараж");
            buyitem.SetRightLabel($"~g~{cost}$");

            mainMenu.AddItem(buyitem);

            mainMenu.AddItem(new UIMenuItem("~r~Закрыть"));

            mainMenu.OnItemSelect += (sender, item, index) =>
            {
                if (index == 0)
                {
                    Events.CallRemote("remote_BuyGarage", garageid);
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

        private void ShowHouseInfo(object[] args)
        {
            if (menuactive) return;
            menuactive = true;
            Chat.Activate(false);
            Chat.Show(false);

            


            int garageid = Convert.ToInt32(args[0]);
            string owner = Convert.ToString(args[1]);
            bool isOwner = Convert.ToBoolean(args[2]);
            bool isClosed = Convert.ToBoolean(args[3]);

            menuPool = new MenuPool();
            var mainMenu = new UIMenu($"Гараж[{garageid}]", $"Владелец: {owner}");

            menuPool.Add(mainMenu);
            var entergarage = new UIMenuItem(isClosed ? "~r~Войти" : "~g~Войти");
            entergarage.SetItemData(0);
            mainMenu.AddItem(entergarage);
            UIMenuItem closegarage = new UIMenuItem(isClosed ? "Открыть гараж" : "Закрыть гараж");
            if (isOwner)
            {
                closegarage.SetItemData(1);
                mainMenu.AddItem(closegarage);
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
                    Events.CallRemote("remote_CloseGarage", garageid, isClosed);
                    closegarage.Text = isClosed ? "Открыть гараж" : "Закрыть гараж";
                    entergarage.Text = isClosed ? "~r~Войти" : "~g~Войти";
                    return;
                }
                else if (itemdata == 0)
                {
                    Events.CallRemote("remote_EnterGarage", garageid, 2);
                    Chat.Activate(true);
                    Chat.Show(true);
                    menuactive = false;
                    menuPool.CloseAllMenus();
                    Events.Tick -= DrawMenu;
                }
                else if (itemdata == 2)
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
    }
}
