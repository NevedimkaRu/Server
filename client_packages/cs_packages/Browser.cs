using System;
using System.Collections.Generic;
using System.Text;
using RAGE.Ui;
using RAGE;

namespace cs_packages
{
    class Browser : Events.Script
    {
        public static void ExecuteFunctionEvent(HtmlWindow htmlwindow, string function, object[] args)
        {
            string input = string.Empty;
            object[] arguments = args;
            foreach (object arg in arguments)
            {
                input += input.Length > 0 ? (", '" + arg.ToString() + "'") : ("'" + arg.ToString() + "'");
            }
            htmlwindow.ExecuteJs(function + "(" + input + ");");
        }

        public static void Show(HtmlWindow htmlwindow, bool cursor = true)
        {
            htmlwindow.Active = true;
            Cursor.Visible = cursor == true ? true : false;
        }

        public static void Close(HtmlWindow htmlwindow)
        {
            RAGE.Chat.Output("метод close");
            htmlwindow.Active = false;
            Cursor.Visible = false;

        }
    }
}
