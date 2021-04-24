using System;
using System.Collections.Generic;
using System.Text;
using cs_packages.model;
using RAGE;
namespace cs_packages.utils
{
    public class Check
    {
        public enum PlayerStatus
        {
            Spawn = 1,
            OpenChat = 2,
            OnEvent = 3,
        };
        public static bool GetPlayerStatus(PlayerStatus status)
        {
            if (ThisPlayer.IsSpawn == false) return false;
            switch (status)
            {
                case PlayerStatus.OpenChat:
                    {
                       return RAGE.Elements.Player.LocalPlayer.IsTypingInTextChat;
                    }
                case PlayerStatus.OnEvent:
                    {
                        return false;
                    }
            }

            return true;
        }
    }
}
