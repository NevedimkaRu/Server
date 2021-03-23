using System;
using System.Collections.Generic;
using System.Text;
using RAGE;
using RAGE.Elements;
using RAGE.NUI;
using cs_packages.model;
namespace cs_packages.player
{
    public class ClanMenu : Events.Script
    {
        MenuPool menuPool;
        bool menuactive;

        public ClanMenu()
        {
            Events.Add("trigger_ShowClanMenu", ShowClanMenu);
        }

        /*Информация
         Игроки
         Ранги*/

        private void ShowClanMenu(object[] args)
        {
            ClanClient model = RAGE.Util.Json.Deserialize<ClanClient>(args[0].ToString());

            if (menuactive) return;
            menuactive = true;
            Chat.Activate(false);
            Chat.Show(false);

            menuPool = new MenuPool();
            ClanMembers clanleader = model.Members.Find(c => c.Rank == 0);
            var mainMenu = new UIMenu($"Клан {model.Title}", $"Лидер - {clanleader.Name}");
            mainMenu.MouseControlsEnabled = false;
            ClanRank _rank;
            menuPool.Add(mainMenu);

            //mainMenu.AddItem(new UIMenuItem("Информация"));
            UIMenu member = menuPool.AddSubMenu(mainMenu, "Участники");
            foreach(var members in model.Members)
            {    
                member.SetMenuData(members.CharacterId);

                ClanMembers _player = model.Members.Find(c => c.CharacterId == Convert.ToInt32(member.MenuData));
                _rank = model.Ranks.Find(r => r.Rank == _player.Rank);
                UIMenu memberaction = menuPool.AddSubMenu(member, $"{_player.Name} Ранг {_rank.RankTitle} ");

                memberaction.AddItem(new UIMenuItem("Выгнать из клана"));
                UIMenu changerank = menuPool.AddSubMenu(memberaction, "Изменить ранг");
                foreach(var _ranks in model.Ranks)
                {
                    UIMenuItem ranks = new UIMenuItem(_ranks.RankTitle);
                    ranks.SetItemData(_ranks.Rank);
                    changerank.AddItem(ranks);

                }
                changerank.OnItemSelect += (sender, item, index) =>
                {
                    _rank = model.Ranks.Find(r => r.Rank == Convert.ToInt32(item.ItemData));
                    Api.Notify($"Вы изменили игроку {_player.Name} ранг на {_rank.RankTitle}");
                };
            }
            UIMenu rank = menuPool.AddSubMenu(mainMenu, "Управление рангами");
            int totalranks = model.Ranks.Count; //4
            int count = 0;
            foreach (var _ranks1 in model.Ranks)
            {
                UIMenuItem ranks = new UIMenuItem(_ranks1.RankTitle);
                
                
                rank.AddItem(ranks);
                ranks.SetItemData(_ranks1.Rank);
                count++;
                if(count >= totalranks)
                {
                    UIMenuItem addrank = new UIMenuItem("Добавить ранг");
                    rank.AddItem(addrank);
                    ranks.SetItemData(-1);
                }
            }

            mainMenu.OnItemSelect += (sender, item, index) =>
            {
                if (index == 0)
                {
                    //todo
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
