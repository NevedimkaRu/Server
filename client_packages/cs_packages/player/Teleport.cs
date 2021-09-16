using RAGE;
using RAGE.Elements;
using System;
using System.Collections.Generic;
using System.Text;

namespace cs_packages.player
{
    class Teleport : Events.Script
    {
        public Teleport()
        {
            Events.Add("trigger_Teleport", TeleportTo);
        }

        private void TeleportTo(object[] args)
        {
            Vector3 tpPos = (Vector3)args[0];
            int time = 3000;
            utils.Utils.SmoothTeleport(tpPos,0, time, true);
            /*RAGE.Task.Run(() => {
                if (RAGE.Elements.Player.LocalPlayer.Vehicle != null)
                {
                    RAGE.Elements.Player.LocalPlayer.TaskVehicleDriveWander(RAGE.Elements.Player.LocalPlayer.Vehicle.Handle, 30, 0);
                }

            }, delayTime: time + 200);*/
            //Events.Tick += Tick;
            /*RAGE.Task.Run(() => {
                RAGE.Elements.Player.LocalPlayer.ClearTasks();

            }, delayTime: 9500);*/
        }

        /*private void Tick(List<Events.TickNametagData> nametags)
        {
            if (!RAGE.Game.Streaming.IsPlayerSwitchInProgress())
            {
                RAGE.Elements.Player.LocalPlayer.ClearTasks();
                Events.Tick -= Tick;
            }
        }*/
    }
}
