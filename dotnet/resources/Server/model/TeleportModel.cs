using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;

namespace Server.model
{
    public class TeleportModel
    {

        public int Id { get; set; } = -1;
        public string Name { get; set; } = "Name";
        public string Discription { get; set; } = "Discription";
        public Vector3 Position { get; set; }
        
        public TeleportModel(string Name, string Discription, Vector3 Position)
        {
            this.Name = Name;
            this.Discription = Discription;
            this.Position = Position;
        }
        public TeleportModel(int Id, string Name, string Discription, Vector3 Position)
        {
            this.Id = Id;
            this.Name = Name;
            this.Discription = Discription;
            this.Position = Position;
        }
        public TeleportModel() { }
    }
}
