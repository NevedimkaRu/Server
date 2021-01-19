using System;
using System.Collections.Generic;
using System.Text;
using RAGE.Game;

namespace cs_packages
{
    public class Api
    {
        public static void Notify(string text)
        {
            Ui.SetNotificationTextEntry("STRING");
            Ui.AddTextComponentSubstringPlayerName(text);
            Ui.DrawNotification(false, false);
        }
    }
}
