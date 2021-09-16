using System;
using System.Collections.Generic;
using RAGE;
using RAGE.Game;
using System.Text;

namespace cs_packages.utils
{
    public class Utils
    {
        public static List<object> GetFloatList(float begin, float end, float step)
        {
            List<object> floatList = new List<object>();
            for (float i = begin; i <= end; i += step)
            {
                floatList.Add(Math.Round(i, 1));
            }
            return floatList;
        }

        public static List<object> GetIntList(int begin, int end, int step)
        {
            List<object> list = new List<object>();
            for (int i = begin; i <= end; i += step)
            {
                list.Add(i);
            }
            return list;
        }
        /*internal static bool PointingAT(float distance, out int hit, out int entity, out Vector3 endPos, out int materialHash)
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
            Shapetest.GetShapeTestResultEx(_result, ref hit, endPos, surfaceNormal, ref materialHash, ref entity);
            if (hit != 0) return true;
            return false;
        }*/

        public static Vector3 GetNormalizedVector(Vector3 vector)
        {

            float mag = (float)Math.Sqrt(
                vector.X * vector.X + vector.Y * vector.Y + vector.Z * vector.Z
                );

            vector.X = vector.X / mag;
            vector.Y = vector.Y / mag;
            vector.Z = vector.Z / mag;
            return vector;
        }

        public static Vector3 GetCrossProduct(Vector3 v1, Vector3 v2)
        {
            Vector3 vector = new Vector3(0, 0, 0);
            vector.X = v1.Y * v2.Z - v1.Z * v2.Y;
            vector.Y = v1.Z * v2.X - v1.X * v2.Z;
            vector.Z = v1.X * v2.Y - v1.Y * v2.X;
            return vector;
        }
        public static Vector3 GetDirectionByRotation(Vector3 rotation)
        {
            float num = rotation.Z * 0.0174532924f;
            float num2 = rotation.X * 0.0174532924f;
            float num3 = MathF.Abs(MathF.Cos(num2));
            return new Vector3 { X = -MathF.Sin(num) * num3, Y = MathF.Cos(num) * num3, Z = MathF.Sin(num2) };
        }

        public static async void SmoothTeleport(Vector3 position, float rot, int duration, bool drive)
        {
            RAGE.Game.Streaming.SwitchOutPlayer(RAGE.Elements.Player.LocalPlayer.Handle, 0, 1);
            
            RAGE.Task.Run(async () =>
            {
                if((bool)await Events.CallRemoteProc("remote_SmoothTeleport", position.X, position.Y, position.Z, rot))
                {
                    if(drive && RAGE.Elements.Player.LocalPlayer.Vehicle != null)
                    {
                        RAGE.Elements.Player.LocalPlayer.Vehicle.FreezePosition(true);
                        RAGE.Task.Run(() => { RAGE.Elements.Player.LocalPlayer.Vehicle.FreezePosition(false); }, delayTime: 200);
                        RAGE.Elements.Player.LocalPlayer.TaskVehicleDriveWander(RAGE.Elements.Player.LocalPlayer.Vehicle.Handle, 30, 0);
                        Events.Tick += Tick;
                    }
                    RAGE.Game.Streaming.Unknown._0xD8295AF639FD9CB8(RAGE.Elements.Player.LocalPlayer.Handle);

                }
                /*if(RAGE.Elements.Player.LocalPlayer.Vehicle != null)
                {
                    RAGE.Elements.Vehicle veh = RAGE.Elements.Player.LocalPlayer.Vehicle;
                    veh.Position = position;
                    veh.Position.Z = veh.Position.Z + 1;
                    RAGE.Elements.Player.LocalPlayer.Position = position;
                    RAGE.Task.Run(() => { RAGE.Elements.Player.LocalPlayer.SetIntoVehicle(veh.Handle, -1); });
                }
                else
                {
                    RAGE.Elements.Player.LocalPlayer.Position = position;

                }*/
               
            }, delayTime: duration);
        }
        private static void Tick(List<Events.TickNametagData> nametags)
        {
            if (!RAGE.Game.Streaming.IsPlayerSwitchInProgress())
            {
                RAGE.Elements.Player.LocalPlayer.ClearTasks();
                Events.Tick -= Tick;
            }
        }
    }
}
