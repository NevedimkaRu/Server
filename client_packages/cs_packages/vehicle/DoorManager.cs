using System;
using System.Collections.Generic;
using System.Text;
using RAGE;
using RAGE.NUI;
using RAGE.Game;
using System.Drawing;

namespace cs_packages.vehicle
{
    /*todo 
     Сделать синхронизацию
     У праворульных машин перепутан индекс правой и левой двери*/
    class DoorManager : Events.Script
    {
        MenuPool menuPool;
        bool menuactive;

        bool active = false;
        public DoorManager()
        {
            //Input.Bind(RAGE.Ui.VirtualKeys.X, true, ShowCamDirection);
            Input.Bind(RAGE.Ui.VirtualKeys.X, true, ChangeDoorState);
            Input.Bind(RAGE.Ui.VirtualKeys.B, true, ShowDoorPositions);//
            Events.OnEntityStreamIn += OnVehicleStreamIn;
            Events.Tick += Tick;
        }

        private void Tick(List<Events.TickNametagData> nametags)
        {
            if (RAGE.Elements.Player.LocalPlayer.Vehicle != null) return;
            if (!utils.Check.GetPlayerStatus(utils.Check.PlayerStatus.Spawn)) return;
            foreach (RAGE.Elements.Vehicle vehicle in RAGE.Elements.Entities.Vehicles.Streamed) 
            { 
                List<Vector3> doors = new List<Vector3>()
                {
                    Entity.GetWorldPositionOfEntityBone(vehicle.Handle,
                        Entity.GetEntityBoneIndexByName(vehicle.Handle, "handle_dside_f")),
                    Entity.GetWorldPositionOfEntityBone(vehicle.Handle,
                        Entity.GetEntityBoneIndexByName(vehicle.Handle, "handle_pside_f")),
                    Entity.GetWorldPositionOfEntityBone(vehicle.Handle,
                        Entity.GetEntityBoneIndexByName(vehicle.Handle, "handle_dside_r")),
                    Entity.GetWorldPositionOfEntityBone(vehicle.Handle,
                        Entity.GetEntityBoneIndexByName(vehicle.Handle, "handle_pside_r")),
                    Entity.GetWorldPositionOfEntityBone(vehicle.Handle, Entity.GetEntityBoneIndexByName(vehicle.Handle, "bonnet")),
                    Entity.GetWorldPositionOfEntityBone(vehicle.Handle, Entity.GetEntityBoneIndexByName(vehicle.Handle, "boot")),
                };
                for (int i = 0; i < doors.Count; i++)
                {
                    int tempX = 0;
                    int tempY = 0;
                    Graphics.GetActiveScreenResolution(ref tempX, ref tempY);
                    float centerX = tempX / 2;
                    float _screenX = 0;
                    float _screenY = 0;
                    Graphics.GetScreenCoordFromWorldCoord(doors[i].X, doors[i].Y, doors[i].Z, ref _screenX, ref _screenY);
                    float screenX = _screenX * 1920;
                    float screenY = _screenY * 1080;
                    float radiusX = 200;
                    float distance;
                    if (i == 4) distance = 2.2f;
                    else distance = 2.0f;
                    /*if (Math.Pow((screenX - centerX), 2) + Math.Pow((screenY - centerY), 2) < Math.Pow(radius, 2) 
                        && RAGE.Elements.Player.LocalPlayer.Position.DistanceTo(doors[i]) < distance)*/
                    if (screenX < centerX + radiusX && screenX > centerX - radiusX
                        && RAGE.Elements.Player.LocalPlayer.Position.DistanceTo(doors[i]) < distance)
                    {
                            
                        RAGE.NUI.UIResText.Draw($"X",
                            (int)(screenX),
                            (int)(screenY),
                            RAGE.Game.Font.ChaletLondon,
                            0.4f,
                            Color.White,
                            RAGE.NUI.UIResText.Alignment.Centered, true, true, 0);
                        break;
                    }

                }
            }
        }
        private void ChangeDoorState()
        {
            if (RAGE.Elements.Player.LocalPlayer.Vehicle != null) return;
            if (!utils.Check.GetPlayerStatus(utils.Check.PlayerStatus.Spawn)) return;
            foreach (RAGE.Elements.Vehicle vehicle in RAGE.Elements.Entities.Vehicles.Streamed)
            {
                List<Vector3> doors = new List<Vector3>()
                {
                    Entity.GetWorldPositionOfEntityBone(vehicle.Handle,
                        Entity.GetEntityBoneIndexByName(vehicle.Handle, "handle_dside_f")),
                    Entity.GetWorldPositionOfEntityBone(vehicle.Handle,
                        Entity.GetEntityBoneIndexByName(vehicle.Handle, "handle_pside_f")),
                    Entity.GetWorldPositionOfEntityBone(vehicle.Handle,
                        Entity.GetEntityBoneIndexByName(vehicle.Handle, "handle_dside_r")),
                    Entity.GetWorldPositionOfEntityBone(vehicle.Handle,
                        Entity.GetEntityBoneIndexByName(vehicle.Handle, "handle_pside_r")),
                    Entity.GetWorldPositionOfEntityBone(vehicle.Handle, Entity.GetEntityBoneIndexByName(vehicle.Handle, "bonnet")),
                    Entity.GetWorldPositionOfEntityBone(vehicle.Handle, Entity.GetEntityBoneIndexByName(vehicle.Handle, "boot")),
                };
                for (int i = 0; i < doors.Count; i++ )
                {
                    int tempX = 0;
                    int tempY = 0;
                    Graphics.GetActiveScreenResolution(ref tempX, ref tempY);
                    float centerX = tempX / 2;
                    float centerY = tempY / 2;
                    float _screenX = 0;
                    float _screenY = 0;
                    Graphics.GetScreenCoordFromWorldCoord(doors[i].X, doors[i].Y, doors[i].Z + 0.60f, ref _screenX, ref _screenY);
                    float screenX = _screenX * 1920;
                    float screenY = _screenY * 1080;
                    float radiusX = 200;
                    float radiusY = 700;
                    float distance;
                    if (i == 4) distance = 2.2f;
                    else distance = 2.0f;
                    /*if (Math.Pow((screenX - centerX), 2) + Math.Pow((screenY - centerY), 2) < Math.Pow(radius, 2) 
                        && RAGE.Elements.Player.LocalPlayer.Position.DistanceTo(doors[i]) < distance)*/
                    if(screenX < centerX + radiusX && screenX > centerX - radiusX 
                        && RAGE.Elements.Player.LocalPlayer.Position.DistanceTo(doors[i]) < distance)
                    {
                        if (Vehicle.GetVehicleDoorAngleRatio(vehicle.Handle, i) > 0.1)
                        {
                            Vehicle.SetVehicleDoorShut(vehicle.Handle, i, false);
                        }
                        else
                        {
                            Vehicle.SetVehicleDoorOpen(vehicle.Handle, i, false, false);
                        }
                        break;
                    }
                }
            }
        }

        private void ShowDoorPositions()
        {
            if (RAGE.Elements.Player.LocalPlayer.Vehicle == null) return;
            List<Vector3> doors = new List<Vector3>()
            {
                Entity.GetWorldPositionOfEntityBone(RAGE.Elements.Player.LocalPlayer.Vehicle.Handle, 
                    Entity.GetEntityBoneIndexByName(RAGE.Elements.Player.LocalPlayer.Vehicle.Handle, "handle_dside_f")),
                Entity.GetWorldPositionOfEntityBone(RAGE.Elements.Player.LocalPlayer.Vehicle.Handle, 
                    Entity.GetEntityBoneIndexByName(RAGE.Elements.Player.LocalPlayer.Vehicle.Handle, "handle_dside_r")),
                Entity.GetWorldPositionOfEntityBone(RAGE.Elements.Player.LocalPlayer.Vehicle.Handle,
                    Entity.GetEntityBoneIndexByName(RAGE.Elements.Player.LocalPlayer.Vehicle.Handle, "handle_pside_f")),
                Entity.GetWorldPositionOfEntityBone(RAGE.Elements.Player.LocalPlayer.Vehicle.Handle,
                    Entity.GetEntityBoneIndexByName(RAGE.Elements.Player.LocalPlayer.Vehicle.Handle, "handle_pside_r"))
            };
            foreach(var door in doors)
            {
                RAGE.Elements.TextLabel text = new RAGE.Elements.TextLabel(door, $"{door}", new RGBA(250, 250, 250), 5);
            }
        }

        private void OnVehicleStreamIn(RAGE.Elements.Entity entity)
        {
            if (entity.Type != RAGE.Elements.Type.Vehicle) return;
            RAGE.Elements.MapObject mapObject = new RAGE.Elements.MapObject(1302435108, new Vector3(), new Vector3());
            Entity.AttachEntityToEntity(mapObject.Handle, entity.Id, Entity.GetEntityBoneIndexByName(entity.Id, "door_dside_f"), 
                0,0,0,0,0,0,false,false,false,false,2,true);
        }

        internal static bool PointingAT(float distance, out int hit, out int entity, out Vector3 endPos, out int materialHash)
        {
            hit = 0;
            entity = -1;
            materialHash = -1;
            Vector3 _pos = Cam.GetGameplayCamCoord();
            Vector3 _dir = admin.NoClip.GetDirectionByRotation(Cam.GetGameplayCamRot(0));
            Vector3 _farAway = new Vector3((_dir.X * distance) + (_pos.X), (_dir.Y * distance) + (_pos.Y), (_dir.Z * distance) + (_pos.Z));
            int _result = Shapetest
                .StartShapeTestRay(_pos.X, _pos.Y, _pos.Z, _farAway.X, _farAway.Y, _farAway.Z, -1, 0, 7);
            endPos = new Vector3();
            Vector3 surfaceNormal = new Vector3();
            Shapetest.GetShapeTestResultEx(_result, ref hit, endPos, surfaceNormal,ref materialHash, ref entity);
            /*if (hit != 0) return true;
            return false;*/
            return true;
        }
    }
}
