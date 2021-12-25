using cs_packages.constants;
using RAGE;
using RAGE.Elements;
using System;
using System.Collections.Generic;
using System.Text;

namespace cs_packages.vehicle
{
    public class Sync : Events.Script
    {

        public const string VEHICLE_PRIMARY_COLOR = "vehicle_primary_color";
        public const string VEHICLE_SECONDARY_COLOR = "vehicle_secondary_color";
        public const string VEHICLE_COLOR_TYPE = "vehicle_color_type";
        public Sync()
        {
            Events.OnEntityStreamIn += EntityStreamIn;
            Events.Add("trigger_ApplyVehicleSync", TriggerSync);
            Events.AddDataHandler(VEHICLE_PRIMARY_COLOR, aaa);
            Events.AddDataHandler(VEHICLE_COLOR_TYPE, aaa);
        }

        private void aaa(Entity entity, object arg, object oldArg)
        {
            if (entity.Type == RAGE.Elements.Type.Vehicle)
            {
                Vehicle vehicle = (Vehicle)entity;
                ApplyVehicleSync(vehicle);
            }
        }

        private void TriggerSync(object[] args)
        {
            Vehicle veh = (Vehicle)args[0];
            ApplyVehicleSync(veh);
            //Chat.Output("TriggerSync");
            
        }

        private void EntityStreamIn(Entity entity)
        {
            if(entity.Type == RAGE.Elements.Type.Vehicle)
            {
                Vehicle vehicle = (Vehicle)entity;
                ApplyVehicleSync(vehicle);
            }
        }
        
        public void ApplyVehicleSync(Vehicle vehicle)
        {
            Color color = RAGE.Util.Json.Deserialize<Color>(vehicle.GetSharedData(SharedData.VEHICLE_PRIMARY_COLOR).ToString());

            int colortype = (int)vehicle._GetSharedData<int>(SharedData.VEHICLE_COLOR_TYPE);

            vehicle.SetModColor1(colortype, 0, 0);
            vehicle.SetCustomPrimaryColour(color.Red, color.Green, color.Blue);
        }

        public class Color
        {
            public int Red;
            public int Green;
            public int Blue;
            public int Alpha;

            public Color(int Red, int Green, int Blue, int Alpha)
            {
                this.Red = Red;
                this.Green = Green;
                this.Blue = Blue;
                this.Alpha = Alpha;
            }
        }
    }
}
