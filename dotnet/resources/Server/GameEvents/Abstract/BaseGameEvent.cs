using GTANetworkAPI;
using Server.GameEvents.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.GameEvents.Abstract
{
    abstract class BaseGameEvent : IGameEvent,
    {
        private String _eventName = "BaseEvent";
        //Статус эвента (-1 - эвент не активен; 0 - В ожидании; 1 - раунд в процессе)
        private int _gameEventStatus = -1;
        //количество милисекунд через которые происходит перепроверка условий для старта евента(на пример минимальное количество участников);
        private int _rechekConditionToStartEvent = 5000;
        //Максимальное количество милисекунд отведённое на раунд;
        private int _maxTimeForround = 120000;
        // Максимальное количество участников для эвента;
        private int _maxPlayerForEvent = 5;
        // Минимальное количество участников для эвента;
        private int _minPlayerForEvent = 2;
        // перезапускать ли евент после окончания раунда;
        private bool _needToReset = true;
        //Время паузы после раунда
        private int _timeAfterRound = 10000;

        private List<Player> _participants = new List<Player>();


        public void AddPlayer(Player player)
        {
            _participants.Add(Player);
        }

        public bool ContaintPlayer(Player player)
        {
            return _participants.Contains(player);
        }
        public void FinishEventRound()
        {
            _gameEventStatus = 0;
            NAPI.Task.Run(ResetEvent, delayTime: _timeAfterRound);
        }

        public bool GetEventName()
        {
            return _eventName;
        }

        public int GetGameEventStatus()
        {
            return _gameEventStatus;
        }

        public int GetMaxPlayers()
        {
            return _maxPlayerForEvent
        }

        public int GetMinPlayers()
        {
            return _minPlayerForEvent
        }

        public List<Player> GetPlayerList()
        {
            return _participants;
        }

        public int GetPLayersCount()
        {
            return _participants.Count();
        }

        public bool IfResetEvent()
        {
            return _needToReset;
        }

        public void InitEvent()
        {
            _gameEventStatus = 0;
        }

        public bool RemovePlayer(Player player)
        {
            return _participants.Remove(player);
        }

        public void ResetEvent()
        {
            if (!IfResetEvent) return;
            CheckConditionalsToStart()
        }

        public void SetReset(bool needReset)
        {
            _needToReset = needReset;
        }
        public void StartEventRound()
        {
            _gameEventStatus = 1;
        }

        public void StopEvent()
        {
            _gameEventStatus = -1;
        }

        private void CheckConditionalsToStart() 
        {
            if (GetPLayersCount() >= GetMinPlayers()) 
            {
                StartEvent();
            }
            NAPI.Task.Run(CheckConditionalsToStart(), delayTime: _rechekConditionToStartEvent);

        }
    }
}
