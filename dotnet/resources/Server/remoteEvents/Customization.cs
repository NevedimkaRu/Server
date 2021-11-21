using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using Server.customization;

namespace Server.remoteEvents
{
    class Customization : Script
    {
        [RemoteEvent("remote_SaveCustomization")]
        public void Remote_SaveCustomization(Player player, object[] args)
        {
            Api.SaveCustomization(player, args);
        }

        [RemoteEvent("remote_GetCharacterCostumize")]
        public void Remote_GetCharacterCusumize(Player player)
        {
            Api.GetCharacterCusumize(player);
        }
    }
}
