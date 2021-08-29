using System;
using System.Collections.Generic;
using System.Text;
using RAGE;
using RAGE.Elements;
namespace cs_packages.ad
{
    class Tuning : Events.Script
    {
        private bool IsStart = false;
        private int LastBump = -1;
        private int LastRearBump = -1;
        private int LastSpoiler = -1;
        public Tuning()
        {
            Events.OnPlayerCommand += cmd;
        }

        private void cmd(string cmd, Events.CancelEventArgs cancel)
        {
            string[] args = cmd.Split(new char[] { ' ' });
            string commandName = args[0].Trim(new char[] { '/' });

            if (commandName == "tun")
            {
                IsStart = !IsStart;
                if (Player.LocalPlayer.Vehicle == null) return;
                ChangeTuning(Player.LocalPlayer.Vehicle);
                RAGE.Task.Run(() => { ChangeTuning(Player.LocalPlayer.Vehicle); }, delayTime: 1000);
            }
        }

        public void ChangeTuning(Vehicle vehicle)
        {
            var rand = new Random();
            int spoiler = rand.Next(0, vehicle.GetNumMods(0));
            int bump = rand.Next(0, vehicle.GetNumMods(1));
            int rearbump = rand.Next(0, vehicle.GetNumMods(2));
            if (spoiler == LastSpoiler)
            {
                while(spoiler == LastSpoiler)
                {
                    spoiler = rand.Next(0, vehicle.GetNumMods(0));
                }
                LastSpoiler = spoiler;
            }
            vehicle.SetMod(0, spoiler, false);

            vehicle.SetMod(1, bump, false);
            if (bump == LastBump)
            {
                while (bump == LastBump)
                {
                    bump = rand.Next(0, vehicle.GetNumMods(1));
                }
                LastBump = bump;
            }

            vehicle.SetMod(2, rearbump, false);
            if (rearbump == LastRearBump)
            {
                while (rearbump == LastRearBump)
                {
                    rearbump = rand.Next(0, vehicle.GetNumMods(2));
                }
                LastRearBump = rearbump;
            }
            vehicle.SetMod(3,rand.Next(0, vehicle.GetNumMods(3)), false);
            vehicle.SetMod(4,rand.Next(0, vehicle.GetNumMods(4)), false);
            vehicle.SetMod(5,rand.Next(0, vehicle.GetNumMods(5)), false);
            vehicle.SetMod(6,rand.Next(0, vehicle.GetNumMods(6)), false);
            vehicle.SetMod(7,rand.Next(0, vehicle.GetNumMods(7)), false);
            vehicle.SetMod(9,rand.Next(0, vehicle.GetNumMods(9)), false);
            vehicle.SetMod(10,rand.Next(0, vehicle.GetNumMods(10)), false);
            vehicle.SetMod(23,rand.Next(0, vehicle.GetNumMods(23)), false);
            vehicle.SetMod(53,rand.Next(0, vehicle.GetNumMods(53)), false);
            vehicle.SetMod(33,rand.Next(0, vehicle.GetNumMods(33)), false);

            vehicle.SetColours(rand.Next(0, 159), rand.Next(0, 159));
            if(IsStart) RAGE.Task.Run(() => { ChangeTuning(Player.LocalPlayer.Vehicle); }, delayTime: 1000);
        }

    }
}
