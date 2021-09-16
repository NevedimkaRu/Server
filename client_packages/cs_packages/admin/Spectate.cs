using RAGE;
using RAGE.Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace cs_packages.admin
{
    public class Spectate : Events.Script
    {
        public Spectate()
        {
            Events.Add("trigger_Spectate", SpectateMode);
            //Events.OnEntityStreamIn += OnEntityStreamIn;
            RAGE.Nametags.Enabled = false;
            Events.Tick += Tick;
        }

        private void Tick(List<Events.TickNametagData> nametags)
        {
            if (!utils.Check.GetPlayerStatus(utils.Check.PlayerStatus.Spawn)) return;
            foreach (Player player in Entities.Players.Streamed)
            {
                /* Variables */
                float _screenX = 0;
                float _screenY = 0;

                Vector3 position = player.GetBoneCoords(31086, 0, 0, 0);

                if (RAGE.Game.Graphics.GetScreenCoordFromWorldCoord(position.X, position.Y, position.Z + 0.60f, ref _screenX, ref _screenY))
                {
                    string title = Convert.ToString(player._GetSharedData<string>("sd_Title"));
                    if(title != null)
                    {
                        RAGE.NUI.UIResText.Draw($"{title}",
                            (int)(1920 * _screenX),
                            (int)((1080 * _screenY) + 30),
                            RAGE.Game.Font.HouseScript,
                            0.4f,
                            Color.White,
                            RAGE.NUI.UIResText.Alignment.Centered, true, true, 0);
                    }

                    RAGE.NUI.UIResText.Draw($"{player.Name}",
                        (int)(1920 * _screenX),
                        (int)(1080 * _screenY),
                        RAGE.Game.Font.ChaletLondon,
                        0.4f,
                        Color.White,
                        RAGE.NUI.UIResText.Alignment.Centered, true, true, 0);
                }
            }
        }

        private void SpectateMode(object[] args)
        {
            RAGE.Elements.Player target = (RAGE.Elements.Player)args[0];
            bool toggle = (bool)args[1];

            RAGE.Elements.Player.LocalPlayer.FreezePosition(toggle);
            RAGE.Elements.Player.LocalPlayer.SetInvincible(true);
            RAGE.Elements.Player.LocalPlayer.SetVisible(false, false);
            RAGE.Elements.Player.LocalPlayer.SetCollision(false, false);
            if (toggle)
            {
                if (target != null && target.Exists)
                {
                    RAGE.Task.Run(() => {
                        RAGE.Game.Entity.AttachEntityToEntity(RAGE.Elements.Player.LocalPlayer.Handle, target.Handle, -1, 0f, 0, 1, 0, 0, 0, true, false, false, false, 0, false);
                    },delayTime: 500);
                }
                else Events.CallRemote("remote_UnSpectate");
            }
        }
    }
}
