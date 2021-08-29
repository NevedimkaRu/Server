using RAGE.Elements;
using System;
using System.Collections.Generic;
using System.Text;

namespace cs_packages.model
{
    public static class ThisPlayer
    {
        public static int Score { get; set; }
        public static bool IsSpawn { get; set; } = false;
        public static Vehicle CurrentVehicle = null;
    }
}
