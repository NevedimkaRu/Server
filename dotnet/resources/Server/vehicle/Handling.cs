using GTANetworkAPI;
using Newtonsoft.Json;
using Server.model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Server.vehicle
{
    public class Handling : Script
    {
        private static float fInitialDriveMaxFlatVel = 3.6f;
        private static float fBrakeBiasFront = 2;
        private static float fSteeringLock = 0.017453292f;
        private static float fTractionCurveLateral = 0.017453292f;
        private static float fTractionBiasFront = 2;
        private static float fSuspensionCompDamp = 10;
        private static float fSuspensionReboundDamp = 10;
        private static float fSuspensionBiasFront = 2;
        private static float fAntiRollBarBiasFront = 2;

        public static void CreateDefaultHandling(int carid, int slot)
        {
            VehicleHandling model = new VehicleHandling();
            model.CarId = carid;
            model.Slot = slot;
            model.fInitialDragCoeff = 15.50000f;
            model.vecCentreOfMassOffset = new Vector3(0.0f, 0.0f, 0.0f);
            model.vecInertiaMultiplier = new Vector3(1.200000f, 1.200000f, 1.600000f);
            model.fDriveBiasFront = 0;
            model.nInitialDriveGears = 6;
            model.fInitialDriveForce = 1.9f;
            model.fDriveInertia = 1.0f;
            model.fClutchChangeRateScaleUpShift = 1.6f;
            model.fClutchChangeRateScaleDownShift = 1.6f;
            model.fInitialDriveMaxFlatVel = 160.0f / fInitialDriveMaxFlatVel;
            model.fBrakeForce = 4.85f;
            model.fBrakeBiasFront = 0.67f * fBrakeBiasFront;
            model.fHandBrakeForce = 3.5f;
            model.fSteeringLock = 52.0f * fSteeringLock;
            model.fTractionCurveMax = 0.95f;
            model.fTractionCurveMin = 1.300f;
            model.fTractionCurveLateral = 24.5f * fTractionCurveLateral;
            model.fTractionSpringDeltaMax = 0.15f;
            model.fLowSpeedTractionLossMult = 1.0f;
            model.fTractionBiasFront = 0.45f * fTractionBiasFront;
            model.fTractionLossMult = 1.0f;
            model.fSuspensionForce = 2.5f;
            model.fSuspensionCompDamp = 2.6f / fSuspensionCompDamp;
            model.fSuspensionReboundDamp = 3.0f / fSuspensionReboundDamp;
            model.fSuspensionRaise = -0.0f;
            model.fSuspensionBiasFront = 0.5f * fSuspensionBiasFront;
            model.Insert();
            if(Main.Veh.ContainsKey(carid))
            {
                Main.Veh[carid]._HandlingData.Add(model);
            }
        }
        public static void FillAllHandlingSlots(int carid)
        {
            if (!Main.Veh.ContainsKey(carid)) return;
            for (int i = 0; i < 2; i++)
            {
                if (Main.Veh[carid]._HandlingData.Find(c => c.Slot == i) != null) continue;
                VehicleHandling model = new VehicleHandling();
                model.CarId = carid;
                model.Slot = i;
                model.fInitialDragCoeff = 15.50000f;
                model.vecCentreOfMassOffset = new Vector3(0.0f, 0.0f, 0.0f);
                model.vecInertiaMultiplier = new Vector3(1.200000f, 1.200000f, 1.600000f);
                model.fDriveBiasFront = 0;
                model.nInitialDriveGears = 6;
                model.fInitialDriveForce = 1.9f;
                model.fDriveInertia = 1.0f;
                model.fClutchChangeRateScaleUpShift = 1.6f;
                model.fClutchChangeRateScaleDownShift = 1.6f;
                model.fInitialDriveMaxFlatVel = 160.0f / fInitialDriveMaxFlatVel;
                model.fBrakeForce = 4.85f;
                model.fBrakeBiasFront = 0.67f * fBrakeBiasFront;
                model.fHandBrakeForce = 3.5f;
                model.fSteeringLock = 52.0f * fSteeringLock;
                model.fTractionCurveMax = 0.95f;
                model.fTractionCurveMin = 1.300f;
                model.fTractionCurveLateral = 24.5f * fTractionCurveLateral;
                model.fTractionSpringDeltaMax = 0.15f;
                model.fLowSpeedTractionLossMult = 1.0f;
                model.fTractionBiasFront = 0.45f * fTractionBiasFront;
                model.fTractionLossMult = 1.0f;
                model.fSuspensionForce = 2.5f;
                model.fSuspensionCompDamp = 2.6f / fSuspensionCompDamp;
                model.fSuspensionReboundDamp = 3.0f / fSuspensionReboundDamp;
                model.fSuspensionRaise = -0.0f;
                model.fSuspensionBiasFront = 0.5f * fSuspensionBiasFront;
                Main.Veh[carid]._HandlingData.Add(model);
            }
        }
        public void UpdateVehicleHandling(int carid, VehicleHandling handlingModel, int slot)
        {
            VehicleHandling vehicleHandling = Main.Veh[carid]._HandlingData.Find(c => c.Slot == slot);
            vehicleHandling.Delete();
            Main.Veh[carid]._HandlingData.Remove(vehicleHandling);
            
            Main.Veh[carid]._HandlingData.Add(handlingModel);
            handlingModel.Insert();
        }
        [Command("ch", GreedyArg = true)]
        public void cmd_CreateDefaultHandling(Player player, string carid, string slot)
        {
            CreateDefaultHandling(Convert.ToInt32(carid), Convert.ToInt32(slot));
            player.SendChatMessage($"Стандартный handling для {carid}, слот {slot}");
        }

        [RemoteEvent("remote_SetHandling")]
        public void SetHandling(Player player, object[] args)
        {
            VehicleHandling model = JsonConvert.DeserializeObject<VehicleHandling>(args[0].ToString());
            player.Vehicle.SetSharedData("sd_Handling1", model);
        }
    }
}
