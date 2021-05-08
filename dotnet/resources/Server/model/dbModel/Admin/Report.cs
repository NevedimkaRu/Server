using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.model
{
    public class Report
    {
        public string Text { get; set; }
        public DateTime ReportTime { get; set; }
        public Player Player { get; set; }
    }
}
