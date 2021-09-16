using RAGE;
using System;
using System.Collections.Generic;
using System.Text;

namespace cs_packages.Interface
{
    class Hud : Events.Script
    {
        private static bool isShowHud = true;
        private Hud()
        {
            Events.Add("vui_showHud", ChangeShowHud);
            Input.Bind(RAGE.Ui.VirtualKeys.F7, true, ToggleShowHud);
            Events.Add("trigger_setMoneyLevelExp", SetMoneyLevelExp);
            Events.Add("trigger_SetMoney", SetMoney);
            Events.Add("trigger_SetLevel", SetLevel);
            Events.Add("trigger_SetExp", SetExp);
        }

        private void SetMoneyLevelExp(object[] args)
        {
            int money = Convert.ToInt32(args[0]);
            int level = Convert.ToInt32(args[1]);
            int exp = Convert.ToInt32(args[2]);
            SetMoney(new object[] { money });
            SetLevel(new object[] { level });
            SetExp(new object[] { exp });
        }

        private void SetExp(object[] args)
        {
            int exp = Convert.ToInt32(args[0]);
            ChangeData("exp", exp.ToString());
        }

        private void SetLevel(object[] args)
        {
            int level = Convert.ToInt32(args[0]);
            ChangeData("level", level.ToString());
        }

        private void SetMoney(object[] args)
        {
            int money = Convert.ToInt32(args[0]);
            ChangeData("money", money.ToString());
        }

        private void ToggleShowHud()
        {
            ShowHud(!isShowHud);
        }

        private void ShowHud(bool v)
        {
            ChangeData("setShowHud", v.ToString().ToLower());
            Chat.Show(v);
        }

        private void ChangeShowHud(object[] args)
        {
            isShowHud = Convert.ToBoolean(args[0]);
        }

        private void ChangeData(string name, string value) 
        {
            Vui.VuiExec($"$store.dispatch('playerData/{name}', {value})");
        }
    }
}
