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
        TextLabel text;
        List<Player> nearbyPlayers = new List<Player>();
        public Spectate()
        {
            Events.Add("trigger_Spectate", SpectateMode);
            //Events.OnEntityStreamIn += OnEntityStreamIn;
            //Events.Tick += Tick;
            RAGE.Nametags.Enabled = true;
        }

        private void Tick(List<Events.TickNametagData> nametags)
        {
            if (!utils.Check.GetPlayerStatus(utils.Check.PlayerStatus.Spawn)) return;
            Chat.Output("1");
            foreach (Player player in Entities.Players.Streamed)
            {
                /* Variables */
                float _screenX = 0;
                float _screenY = 0;

                Vector3 position = player.GetBoneCoords(31086, 0, 0, 0);

                if (RAGE.Game.Graphics.GetScreenCoordFromWorldCoord(position.X, position.Y, position.Z + 0.60f, ref _screenX, ref _screenY))
                {
                    RAGE.NUI.UIResText.Draw($"{player.Name}", 
                        (int)(1920 * _screenX),
                        (int)(1080 * _screenY), 
                        RAGE.Game.Font.Pricedown, 
                        0.4f,
                        Color.White,
                        RAGE.NUI.UIResText.Alignment.Centered, true, true, 0);
                }
            }
        }

        private void OnEntityStreamIn(Entity entity)
        {
            /*if(entity.Type == RAGE.Elements.Type.Player)
            {
                RAGE.Elements.Player player = (Player)entity;
                bool IsInvisible = (bool)player._GetSharedData<bool>("INVISIBLE");
                if (IsInvisible)
                {
                    player.SetCollision(false, false);
                    player.Name = "";
                }
                nearbyPlayers.Add(player);
                Chat.Output($"{player.Name}");

            }*/
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
