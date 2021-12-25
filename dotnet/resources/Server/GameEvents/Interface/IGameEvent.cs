using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.GameEvents.Interface
{
    interface IGameEvent
    {

        public string GetEventName();
        public void InitEvent();
        public void StartEventRound();
        public void FinishEventRound();
        public void ResetEvent();
        public void StopEvent();
        public void AddPlayer(Player player, int carId);
        public bool RemovePlayer(Player player);
        public bool ContaintPlayer(Player player);
        public List<Player> GetPlayerList();
        public int GetGameEventStatus();
        public int GetPLayersCount();
        public int GetMaxPlayers();
        public int GetMinPlayers();
        public bool IfResetEvent();
        public void SetReset(bool needReset);


    }
}
