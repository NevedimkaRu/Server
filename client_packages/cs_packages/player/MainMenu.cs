using System;
using System.Collections.Generic;
using System.Text;
using RAGE;
using RAGE.NUI;

namespace cs_packages.player
{
    class MainMenu : RAGE.Events.Script
    {
        private MenuPool mainMenuPool;

        public void ShowMainMenu()
        {
            mainMenuPool = new MenuPool();
            var mainMenu = new UIMenu("Native UI", "~b~NATIVEUI SHOWCASE");

            mainMenuPool.Add(mainMenu);
            ShowPlayerCars(mainMenu);
            ShowTeleports(mainMenu);

        }

        public void ShowPlayerCars(UIMenu menu)
        {
            var submenu = mainMenuPool.AddSubMenu(menu, "Транспорт");
            for (int i = 0; i < 20; i++)
                submenu.AddItem(new UIMenuItem("Машина", "Описание"));
        }

        public void ShowTeleports(UIMenu menu)
        {
            var submenu = mainMenuPool.AddSubMenu(menu, "Телепорты");
            for (int i = 0; i < 20; i++)
                submenu.AddItem(new UIMenuItem("Телепорт", "Описание"));
        }

    }
}
