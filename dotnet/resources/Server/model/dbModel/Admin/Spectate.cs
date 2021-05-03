using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.model
{
    public class Spectate
    {
        public bool Active { get; set; } = false;
        public int TargetId { get; set; }
        public Vector3 Position { get; set; }
        public uint Dimension { get; set; }
    }
}
