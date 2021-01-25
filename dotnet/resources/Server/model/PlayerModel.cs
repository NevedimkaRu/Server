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

        public bool IsSpawn { get; set; } = false;
    }
}
