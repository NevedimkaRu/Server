
using RAGE;
using RAGE.Elements;
using System;
using System.Collections.Generic;
using System.Text;

namespace cs_packages.utils
{
    class GameUtils : Events.Script
    {
        private GameUtils()
        {
            Events.OnPlayerCommand += OnPlayerCommand;
            Events.Add("freezeCountdown", freezeCountdown);
        }

        //int countdown = 0;



        private void OnPlayerCommand(string cmd, Events.CancelEventArgs cancel)
        {
            string[] args = cmd.Split(new char[] { ' ' });
            string commandName = args[0].Trim(new char[] { '/' });
            int cd = 5;

            if (commandName == "countdown")
            {
                Player.LocalPlayer.FreezePosition(true);
                Player.LocalPlayer.Vehicle.FreezePosition(true);
                CountDown(cd);
            }
            if (commandName == "unfreeze")
            {
                Player.LocalPlayer.FreezePosition(false);
                Player.LocalPlayer.Vehicle.FreezePosition(false);
            }

        }

        private static void freezeCountdown(object[] args)
        {
            int cd = Convert.ToInt32(args[0]);
            //Player.LocalPlayer.FreezePosition(true);
            if (Player.LocalPlayer.Vehicle != null)
            {
                Player.LocalPlayer.Vehicle.FreezePosition(true);
            }
            CountDown(cd);

        }

        public static void freezePlayer(int time)
        {
            Player.LocalPlayer.FreezePosition(true);
            if (Player.LocalPlayer.Vehicle != null)
            {
                Player.LocalPlayer.Vehicle.FreezePosition(true);
            }
            CountDown(time);
        }

        private static void CountDown(int cd)
        {
            Chat.Output(cd.ToString());
            if (cd-- <= 0)
            {
                Player.LocalPlayer.FreezePosition(false);
                if (Player.LocalPlayer.Vehicle != null)
                {
                    Player.LocalPlayer.Vehicle.FreezePosition(false);
                }
            }
            else
            {
                Task.Run(() => CountDown(cd), 1000);
            }
        }

    }
}
