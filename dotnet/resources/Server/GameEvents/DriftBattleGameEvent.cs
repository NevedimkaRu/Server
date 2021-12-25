using GTANetworkAPI;
using Server.GameEvents.Abstract;
using Server.model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.GameEvents
{
    class DriftBattleGameEvent : BaseGameEvent
    {
        public override int _minPlayerForEvent { get; set; } = 0;

        MapGameEvent map = new MapGameEvent()
        {
            Name = "Доки",
            MaxTime = 120,
            PlayerPositions = new List<Vector3>()
                {
                    new Vector3(922.6224f, -3004.9324f, 5.2193694f),
                    new Vector3(931.85895f, -3010.9177f, 5.2198863f),
                    new Vector3(914.6646f, -3005.4727f, 5.21992f),
                    new Vector3(916.997f, -3010.421f, 5.219356f),
                    new Vector3(907.4528f, -3005.156f, 5.2199483f)
                },
            Rotation = -90f
        };

        public override string _eventName { get; set; } = "DriftBattle";

        public override void OnFinishRound()
        {
        }

        public override void OnInitEvent()
        {
        }

        public override void OnStartRound()
        {
            NAPI.Chat.SendChatMessageToAll($"OnStartRound: {GetEventName()}");
            NAPI.Util.ConsoleOutput($"OnStartRound: {GetEventName()}");

            List<Player> playerList = GetPlayerList();
            for (int i = 0; i < playerList.Count; i++)
            {
                playerList[i].Position = map.PlayerPositions[i];
                playerList[i].Rotation = new Vector3(0, 0, map.Rotation);
            }
        }

        public override void OnStopEvent()
        {
        }
    }
}
