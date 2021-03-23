using System;
using System.Collections.Generic;
using System.Text;
using RAGE;
using RAGE.Elements;
using cs_packages.utils;

namespace cs_packages.house
{
    public class Furniture : Events.Script
    {
        private bool furnitureActive = false;
        private TextLabel text;
        private MapObject obj;
        private float distance = 6;
        public Furniture()
        {
            Input.Bind(RAGE.Ui.VirtualKeys.B, true, TestFurniture);
            
        }

        private void TestFurniture()
        {
            if (!furnitureActive)
            {
                Vector3 playerPos = Player.LocalPlayer.Position;
                playerPos.X += 1f;
                obj = new MapObject(/*1072616162*/ RAGE.Util.Joaat.Hash("prop_logpile_06b"), playerPos, new Vector3(0, 0, 0));
                
                obj.Dimension = Player.LocalPlayer.Dimension;
                text = new TextLabel(Player.LocalPlayer.Position, $"{RAGE.Util.Joaat.Hash("prop_barrier_work01a")}\nPos:{obj.Position}\nRot:{obj.GetRotation(2)}\n{obj.HasBeenBroken()}", new RGBA(250, 250, 250), 10);
                Events.Tick += Tick;
            }
            else
            {
                obj.SetCollision(true, true);
                //obj.SetActivatePhysicsAsSoonAsItIsUnfrozen(true);
                distance = 6f;
                Events.Tick -= Tick; 
            }
            furnitureActive = !furnitureActive;
        }

        private void Tick(List<Events.TickNametagData> nametags)
        {
            
            //Utils.PointingAT(20, out int hit, out int entity, out objPostition, out int materialHash);
            Vector3 _pos = RAGE.Game.Cam.GetGameplayCamCoord();
            Vector3 _dir = admin.NoClip.GetDirectionByRotation(RAGE.Game.Cam.GetGameplayCamRot(0));
            //Vector3 _farAway = new Vector3((_dir.X * distance) + (_pos.X), (_dir.Y * distance) + (_pos.Y), (_dir.Z * distance) + (_pos.Z));
            
            Vector3 _farAway = new Vector3((_dir.X * distance) + (_pos.X), (_dir.Y * distance) + (_pos.Y), Player.LocalPlayer.Position.Z - 1.0f);
            Vector3 rotation = obj.GetRotation(2);
            obj.Position = _farAway;
            obj.PlaceOnGroundProperly();
            obj.FreezePosition(false);
            text.Position = new Vector3(obj.Position.X,obj.Position.Y,obj.Position.Z + 1f);
            text.Text = $"{RAGE.Util.Joaat.Hash("prop_barrier_work01a")}\nPos:{obj.Position}\nRot:{obj.GetRotation(2)}\n{distance}";
            //RAGE.Game.Entity.SetEntityCoords(obj, _farAway.X, _farAway.Y, _farAway.Z, false, false, false, false); 
            if(Input.IsDown(RAGE.Ui.VirtualKeys.Q))
            {
                
                obj.SetRotation(rotation.X, rotation.Y, rotation.Z + 1.0f, 2, false);
            }
            if (Input.IsDown(RAGE.Ui.VirtualKeys.E))
            {
                obj.SetRotation(rotation.X, rotation.Y, rotation.Z - 1.0f, 2, false);
            }
            if (Input.IsDown(RAGE.Ui.VirtualKeys.Up))
            {
                //if (distance > 10) distance = 10f;
                distance += 0.05f;
            }
            if (Input.IsDown(RAGE.Ui.VirtualKeys.Down))
            {
                if (distance < 6) distance = 6f;
                distance -= 0.05f;
            }
            if(obj.Position.Z > Player.LocalPlayer.Position.Z + 2.0f || obj.Position.Z < Player.LocalPlayer.Position.Z - 2.0f) Chat.Output("Объект за картой");
            // MapObject a = new MapObject(072616162,new Vector3(objPostition.X, objPostition.Y, objPostition.Z), new Vector3());

        }
    }
}
