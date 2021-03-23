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
            Shapetest.GetShapeTestResultEx(_result, ref hit, endPos, surfaceNormal, ref materialHash, ref entity);
            if (hit != 0) return true;
            return false;
        }
    }
}
