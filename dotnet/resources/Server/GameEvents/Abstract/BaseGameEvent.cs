using GTANetworkAPI;
using Server.GameEvents.Interface;
using System;
using System.Collections.Generic;
using System.Timers;

namespace Server.GameEvents.Abstract
{
    abstract class BaseGameEvent : IGameEvent
    {
        public abstract String _eventName { get; set; }
        //Статус эвента (-1 - эвент не активен; 0 - В ожидании; 1 - раунд в процессе)
        public virtual int _gameEventStatus { get; set; } = -1;
        //количество милисекунд через которые происходит перепроверка условий для старта евента(на пример минимальное количество участников);
        public virtual int _rechekConditionToStartEvent { get; set; } = 1000;
        //Максимальное количество милисекунд отведённое на раунд;
        public virtual int _maxTimeForRound { get; set; } = 2000;
        // Максимальное количество участников для эвента;
        public virtual int _maxPlayerForEvent { get; set; } = 5;
        // Минимальное количество участников для эвента;
        public virtual int _minPlayerForEvent { get; set; } = 1;
        // перезапускать ли евент после окончания раунда;
        public virtual bool _needToReset { get; set; } = true;
        //Время паузы после раунда
        public virtual int _timeAfterRound { get; set; } = 1000;
        private Timer _timerToFinishRound;

        private List<Player> _participants = new List<Player>();
        Dictionary<Player, int> _participantsVehs = new Dictionary<Player, int>();


        public void AddPlayer(Player player, int carId)
        {
            if (Main.Veh.ContainsKey(carId))
            {
                _participants.Add(player);
                _participantsVehs.Add(player, carId);
            }
            
            
        }

        public bool ContaintPlayer(Player player)
        {
            return _participants.Contains(player);
        }
        public void FinishEventRound()
        {
            NAPI.Util.ConsoleOutput($"FinishEventRound: {GetEventName()}");

            _gameEventStatus = 0;
            _timerToFinishRound.Dispose();
            _timerToFinishRound.Enabled = false;
            _timerToFinishRound.Stop();
            OnFinishRound();
            NAPI.Task.Run(ResetEvent, delayTime: _timeAfterRound);
        }

        public string GetEventName()
        {
            return _eventName;
        }

        public int GetGameEventStatus()
        {
            return _gameEventStatus;
        }

        public int GetMaxPlayers()
        {
            return _maxPlayerForEvent;
        }

        public int GetMinPlayers()
        {
            return _minPlayerForEvent;
        }

        public List<Player> GetPlayerList()
        {
            return _participants;
        }

        public int GetPLayersCount()
        {
            return _participants.Count;
        }

        public bool IfResetEvent()
        {
            return _needToReset;
        }

        public void InitEvent()
        {
            NAPI.Util.ConsoleOutput($"InitEvent: {GetEventName()}");
            _gameEventStatus = 0;
            OnInitEvent();
            CheckConditionalsToStart();
        }

        public bool RemovePlayer(Player player)
        {
            _participantsVehs.Remove(player);
            return _participants.Remove(player);

        }

        public void ResetEvent()
        {
            NAPI.Util.ConsoleOutput($"ResetEvent: {GetEventName()}");

            if (!IfResetEvent())
            {
                StopEvent();
            }
            else
            {
                CheckConditionalsToStart();
            }
        }

        public void SetReset(bool needReset)
        {
            _needToReset = needReset;
        }
        public void StartEventRound()
        {
            NAPI.Util.ConsoleOutput($"StartEventRound: {GetEventName()}");
            _gameEventStatus = 1;
            OnStartRound();
            _timerToFinishRound = new Timer(_maxTimeForRound);
            _timerToFinishRound.Elapsed += this.TimerToFinishRoundElapsed;
            _timerToFinishRound.AutoReset = false;
            _timerToFinishRound.Enabled = true;
            _timerToFinishRound.Start();
        }

        public void StopEvent()
        {
            _gameEventStatus = -1;
            OnStopEvent();
        }

        public abstract void OnFinishRound();

        public abstract void OnStartRound();

        public abstract void OnInitEvent();


        public abstract void OnStopEvent();


        private int GetPlayerVehId(Player player)
        {
            return _participantsVehs[player];
        }
        private Vehicle GetPlayerVeh(Player player)
        {
            if (Main.Veh.ContainsKey(GetPlayerVehId(player)))
            {
                return Main.Veh[GetPlayerVehId(player)]._Veh;
            }
            else 
            {
                return null;
            }
        }

        private void TimerToFinishRoundElapsed(System.Object source, ElapsedEventArgs e)
        {
            FinishEventRound();
        }

        private void CheckConditionalsToStart()
        {
            NAPI.Util.ConsoleOutput($"CheckConditionalsToStart: {GetEventName()};\n_minPlayerToStartRound: {GetMinPlayers()}; _currentPlayerCount: {GetPLayersCount()};");
            if (GetPLayersCount() >= GetMinPlayers())
            {
                StartEventRound();
            }
            else
            {
                NAPI.Task.Run(CheckConditionalsToStart, delayTime: _rechekConditionToStartEvent);
            }
        }
    }
}
