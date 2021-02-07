using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.model
{
    public class PlayerModel
    {
        public Account Account { get; set; }
        public Character Character { get; set; }
        public Customization Customization { get; set; }
        public List<TracksRecords> TracksRecords = new List<TracksRecords>();

        public bool IsSpawn { get; set; } = false;
        public Traks Track { get; set; }
        public int CurrentTrackIndex { get; set; } = -1;

        public MapGameEvent CurrentMapGameEvent { get; set; } = null;


    }
}
