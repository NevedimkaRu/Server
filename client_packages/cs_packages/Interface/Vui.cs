using RAGE;
using RAGE.Ui;
using RAGE.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace cs_packages.Interface
{
    class Vui : Events.Script
    {
        public static HtmlWindow index = new HtmlWindow("package://statics/VUI/index.html");

        public Vui()
        {
            index.Active = true;
            Events.OnPlayerCommand += cmd;
            Events.Add("trigger_ErrorNotify", ErrorNotify);
        }

        private void ErrorNotify(object[] args)
        {
            Vui.Notify(args[0].ToString());
        }

        public static void VuiModals(string func)
        {
            index.ExecuteJs("VuiModals." + func);
        }

        public static void VuiExec(string func)
        {
            index.ExecuteJs("Vui." + func);
        }
        public static void Notify(string text)
        {
            Chat.Output("ВЫёбывается");
            index.ExecuteJs($"Vui.$q.notify('{text}')");
        }


        private void cmd(string cmd, Events.CancelEventArgs cancel)
        {
            string[] args = cmd.Split(new char[] { ' ' });
            string commandName = args[0].Trim(new char[] { '/' });

            if (commandName == "VUI")
            {
                Chat.Output("1");
                index.ExecuteJs("console.log(mp)");
            }
            if (commandName == "unblur")
            {
                RAGE.Game.Graphics.TransitionFromBlurred(0);
            }
            if (commandName == "reloadt")
            {
                index.Reload(true);
            }
            if (commandName == "reloadf")
            {
                index.Reload(false);
            }
            if (commandName == "VUI0")
            {
                Chat.Output("2");
                //index.ExecuteJs("alert('asdasdasd')");
                index.ExecuteJs("VUI.$store.dispatch('playerData/initCars', [{title:'Sultan', number: '228'}])");
            }
        }
    }
}
