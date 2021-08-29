using RAGE;
using RAGE.Elements;
using RAGE.Util;
using System;
using System.Collections.Generic;
using System.Text;
using cs_packages.model;
using RAGE.NUI;

namespace cs_packages.game.GameEvents
{
    class DriftEvents : Events.Script
    {

        const int TIME_TO_WARMUP = 10;

        private DriftEvents()
        {
            Events.OnPlayerEnterVehicle += OnPlayerEnterVehicle;
            Events.OnPlayerCommand += OnPlayerCommand;
            Events.Add("startWarmUp", StartWarmUp);
            Events.Add("startDriftBattle", StartDriftBattle);
        }

        private static Vector3 StartPostiton;
        private static Vehicle Veh;
        private static Vector3 StartRotation;

        private void OnPlayerEnterVehicle(Vehicle vehicle, int seatId)
        {
            Chat.Output(Player.LocalPlayer.Vehicle.Handle.ToString());
        }

        private void OnPlayerCommand(string cmd, Events.CancelEventArgs cancel)
        {
            string[] args = cmd.Split(new char[] { ' ' });
            string commandName = args[0].Trim(new char[] { '/' });
            int cd = 5;

            if (commandName == "incar")
            {
                int carid = Convert.ToInt32(args[1]);
                Chat.Output("Попытка сесть в машину " + carid);
                Player.LocalPlayer.SetIntoVehicle(carid, -1);
            }

        }

        private void StartWarmUp(object[] args)
        {
            Vector3 pos = (Vector3)args[0];
            Vehicle veh = (Vehicle)args[1];
            //int handlingId = Convert.ToInt32(args[2]);
            Player.LocalPlayer.Position = pos;
            if (veh != null)
            {
                if (Player.LocalPlayer.Position.DistanceTo(veh.Position) < 5) 
                {
                    StartPostiton = veh.Position;
                    StartRotation = veh.GetRotation(1);
                    Player.LocalPlayer.SetIntoVehicle(veh.Handle, -1);
                    //RAGE.Game.Pad.DisableControlAction(1, 75, true);
                    veh.SetRadioEnabled(false);
                    //vehicle.Handling.SetHandlingChain(veh, handlingId);
                    Player.LocalPlayer.FreezePosition(true);
                    //if (Player.LocalPlayer.Vehicle != null) Player.LocalPlayer.Vehicle.FreezePosition(true);
                    Events.CallRemote("remote_readyDriftBattle");
                }
                Events.OnEntityStreamIn += (Entity ent) => {
                    if (ent == veh)
                    {
                        StartPostiton = veh.Position;
                        StartRotation = veh.GetRotation(1);
                        Player.LocalPlayer.SetIntoVehicle(veh.Handle, -1);
                        //RAGE.Game.Pad.DisableControlAction(1, 75, true);
                        veh.SetRadioEnabled(false);
                        //vehicle.Handling.SetHandlingChain(veh, handlingId);
                        Player.LocalPlayer.FreezePosition(true);
                        //if(Player.LocalPlayer.Vehicle != null) Player.LocalPlayer.Vehicle.FreezePosition(true);
                        Events.CallRemote("remote_readyDriftBattle");
                    }
                };
            }
            else {
                Chat.Output("Нету машины");
            }
            Chat.Output("Разминка продлится 30 секунд");
        }
        private void StartDriftBattle(object[] args)
        {
            Chat.Output("Start on CLient");
            ThisPlayer.Score = 0;
            Player.LocalPlayer.SetCoordsKeepVehicle(StartPostiton.X, StartPostiton.Y, StartPostiton.Z);
            Player.LocalPlayer.Vehicle.SetRotation(StartRotation.X, StartRotation.Y, StartRotation.Z, 1, true);
            utils.GameUtils.freezePlayer(5);
            Task.Run(FinishEvent, delayTime: 30000);
        }
        public void FinishEvent()
        {
            Chat.Output("Finish on client");

            //Player.LocalPlayer.FreezePosition(true);
            //if (Player.LocalPlayer.Vehicle != null) Player.LocalPlayer.Vehicle.FreezePosition(true);
            Events.CallRemote("remote_sendScore", ThisPlayer.Score);
        }
    }
}
