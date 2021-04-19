using System;
using System.Collections.Generic;
using System.Text;
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
            if (!(bool)RAGE.Elements.Player.LocalPlayer._GetSharedData<bool>("IsSpawn")) return false;
            switch (status)
            {
                case PlayerStatus.Spawn:
                    {
                        return true;
                    }
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
