using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using Newtonsoft.Json;

namespace Server.utils
{
    public class Parser
    {
        public static List<Vector3> ParseToListVector3(string list)
        {
            string[] vector = list.Split("|");
            List<Vector3> vectorList = new List<Vector3>();
            foreach (string s in vector)
            {
                Vector3 v = JsonConvert.DeserializeObject<Vector3>(s);
                vectorList.Add(v);
            }
            return vectorList;
        }
        public static string ParseFromListVector3(List<Vector3> list)
        {
            string[] vectors = new string[list.Count];
            int i = 0;
            foreach (Vector3 vector in list)
            {
                vectors[i] = JsonConvert.SerializeObject(vector);
                i++;
            }
            return String.Join('|', vectors);
        }
    }
}
