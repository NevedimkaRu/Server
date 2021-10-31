using RAGE;
using RAGE.Ui;
using RAGE.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Events.Add("vui_RemoteEvent", VuiCallRemote);
            Events.AddProc("vui_RemoteProc", VuiCallRemoteProc);
        }

        private async Task<object> VuiCallRemoteProc(object[] args)
        {
            Chat.Output("Пришло");
            string eventName = args[0].ToString();
            object[] p = args.Skip(1).ToArray();
            if (p.Length > 0)
            {
                return await Events.CallRemoteProc(eventName, p);
            }
            else 
            { 
                return await Events.CallRemoteProc(eventName);
            }
        }

        private void VuiCallRemote(object[] args)
        {
            Chat.Output("Пришло");
            string eventName = args[0].ToString();
            object[] p = args.Skip(1).ToArray();

            if (p.Length > 0)
            {
                Events.CallRemote(eventName, p);
            }
            else 
            { 
                Events.CallRemote(eventName, p);
            }
        }

        public static void CloseModals()
        {
            VuiModals("closeMenu()");
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
            index.ExecuteJs($"Vui.$q.notify('{text}')");
        }


        private void cmd(string cmd, Events.CancelEventArgs cancel)
        {
            string[] args = cmd.Split(new char[] { ' ' });
            string commandName = args[0].Trim(new char[] { '/' });

            if (commandName == "VUI")
            {
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
                //index.ExecuteJs("alert('asdasdasd')");
                index.ExecuteJs("VUI.$store.dispatch('playerData/initCars', [{title:'Sultan', number: '228'}])");
            }
            if (commandName == "proc") 
            {
            }
        }
    }
}
