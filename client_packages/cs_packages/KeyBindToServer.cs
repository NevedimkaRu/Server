using System;
using System.Collections.Generic;
using System.Text;
using RAGE;
using RAGE.Elements;
namespace cs_packages
{
    class KeyBindToServer : Events.Script
    {
        bool freezeinput = false;
        public KeyBindToServer()
        {
            Input.Bind(0x12, true, PressedAlt);//alt
            Input.Bind(0x45, true, PressedE);//E
        }

        private void PressedE()
        {
            if (Player.LocalPlayer.Vehicle == null) return;
            if (!freezeinput)
            {
                Events.CallRemote("remote_PressEKey");
                freezeinput = true;
                Task.Run(FreezeKey, 3000);
            }
        }

        public void PressedAlt()
        {
            if (!freezeinput)
            {
                Events.CallRemote("remote_PressAlt");
                freezeinput = true;
                Task.Run(FreezeKey, 3000);
            }
        }

        public void FreezeKey()
        {
            freezeinput = false;
        }
    }
}
