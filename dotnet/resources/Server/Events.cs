using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
namespace Server
{
    public class Events : Script
    {
        public delegate void OnPlayerPressActionKeyDelegate(Player player);
        public static event OnPlayerPressActionKeyDelegate OnPlayerPressAltKey;
        public static event OnPlayerPressActionKeyDelegate OnPlayerPressEKey;
        public enum Keys : int
        {
            Alt = 0,
            E = 1
        }
        [RemoteEvent("remote_OnPressActionKey")]
        public void PressActionKey(Player player, int key)
        {
            if((Keys)key == Keys.Alt)
            {
                OnPlayerPressAltKey.Invoke(player);
            }           
            if((Keys)key == Keys.E)
            {
                OnPlayerPressEKey.Invoke(player);
            }
        }
        
        [RemoteEvent("remote_PressAlt")]
        public void Remote_OnPlayerPressAltKey(Player player)
        {
            house.Api.OnPlayerPressAltKey(player);
        }
    }
}
