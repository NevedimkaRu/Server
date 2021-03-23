using System;
using System.Collections.Generic;
using System.Text;
using RAGE;
using RAGE.NUI;
using RAGE.Game;

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
            Input.Bind(RAGE.Ui.VirtualKeys.X, true, ShowCamDirection);
            Input.Bind(RAGE.Ui.VirtualKeys.F13, true, ShowDoorPositions);//
            Events.OnEntityStreamIn += OnVehicleStreamIn;
        }

        private void ShowDoorPositions()
        {
            if (RAGE.Elements.Player.LocalPlayer.Vehicle == null) return;
            List<Vector3> doors = new List<Vector3>()
            {
                
                //RAGE.Elements.Player.LocalPlayer.Vehicle.GetEntryPositionOfDoor(0),
                Entity.GetWorldPositionOfEntityBone(RAGE.Elements.Player.LocalPlayer.Vehicle.Handle, 
                    Entity.GetEntityBoneIndexByName(RAGE.Elements.Player.LocalPlayer.Vehicle.Handle, "door_dside_f")),
                Entity.GetWorldPositionOfEntityBone(RAGE.Elements.Player.LocalPlayer.Vehicle.Handle, 
                    Entity.GetEntityBoneIndexByName(RAGE.Elements.Player.LocalPlayer.Vehicle.Handle, "door_dside_r")),
                Entity.GetWorldPositionOfEntityBone(RAGE.Elements.Player.LocalPlayer.Vehicle.Handle,
                    Entity.GetEntityBoneIndexByName(RAGE.Elements.Player.LocalPlayer.Vehicle.Handle, "door_pside_f")),
                Entity.GetWorldPositionOfEntityBone(RAGE.Elements.Player.LocalPlayer.Vehicle.Handle,
                    Entity.GetEntityBoneIndexByName(RAGE.Elements.Player.LocalPlayer.Vehicle.Handle, "door_pside_r"))
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

        private void ShowCamDirection()
        {
            if (PointingAT(20, out int hit, out int entity, out Vector3 endPos, out int materialHash))
            {
                //Chat.Output($" hit: {hit} | entity: {entity} | pos: {endPos.X} {endPos.Y} {endPos.Z}");
                //Chat.Output(Entity.IsEntityAVehicle(entity).ToString());
                if(Entity.IsEntityAVehicle(entity))
                {   
                    List<Vector3> doors = new List<Vector3>()
                    {
                        Entity.GetWorldPositionOfEntityBone(entity,
                            Entity.GetEntityBoneIndexByName(entity, "door_dside_f")),
                        Entity.GetWorldPositionOfEntityBone(entity,
                            Entity.GetEntityBoneIndexByName(entity, "door_pside_f")),
                        Entity.GetWorldPositionOfEntityBone(entity,
                            Entity.GetEntityBoneIndexByName(entity, "door_dside_r")),
                        Entity.GetWorldPositionOfEntityBone(entity,
                            Entity.GetEntityBoneIndexByName(entity, "door_pside_r")),
                        Entity.GetWorldPositionOfEntityBone(entity, Entity.GetEntityBoneIndexByName(entity, "bonnet")),
                        Entity.GetWorldPositionOfEntityBone(entity, Entity.GetEntityBoneIndexByName(entity, "boot")),
                    };
                    int i = 0;
                    float distance = 0;
                    /*Chat.Output(endPos.ToString());
                    Chat.Output(endPos.DistanceTo(doors[0]).ToString());
                    Chat.Output(RAGE.Elements.Player.LocalPlayer.Position.DistanceTo(doors[0]).ToString());*/
                    foreach (var door in doors)
                    {
                        //if(RAGE.Elements.Player.LocalPlayer.Position.DistanceTo(door) < 2)

                        if (i == 4) distance = 2.2f;
                        else distance = 2.0f;
                        if(endPos.DistanceTo2D(door) < 1.3 && RAGE.Elements.Player.LocalPlayer.Position.DistanceTo(door) < distance)
                        {
                            if(Vehicle.GetVehicleDoorAngleRatio(entity, i) > 0.1)
                            {
                                Vehicle.SetVehicleDoorShut(entity, i, false);
                            }
                            else
                            {
                                Vehicle.SetVehicleDoorOpen(entity, i, false, false);
                            }
                            i = 0;
                            //Chat.Output(Vehicle.GetEntryPositionOfDoor(entity, i).ToString());
                            
                            break;
                        }

                        i++;
                    }
                }
            }
        }

        internal static bool PointingAT(float distance, out int hit, out int entity, out Vector3 endPos, out int materialHash)
        {
            hit = 0;
            entity = -1;
            materialHash = -1;
            Vector3 _pos = Cam.GetGameplayCamCoord();
            Vector3 _dir = admin.NoClip.GetDirectionByRotation(Cam.GetGameplayCamRot(0));
            Vector3 _farAway = new Vector3((_dir.X * distance) + (_pos.X), (_dir.Y * distance) + (_pos.Y), (_dir.Z * distance) + (_pos.Z));
            int _result = Shapetest.StartShapeTestRay(_pos.X, _pos.Y, _pos.Z, _farAway.X, _farAway.Y, _farAway.Z, -1, 0, 7);
            endPos = new Vector3();
            Vector3 surfaceNormal = new Vector3();
            Shapetest.GetShapeTestResultEx(_result, ref hit, endPos, surfaceNormal,ref materialHash, ref entity);
            if (hit != 0) return true;
            return false;
        }
    }
}
