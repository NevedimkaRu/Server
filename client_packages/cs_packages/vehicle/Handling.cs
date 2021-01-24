using System;
using System.Collections.Generic;
using System.Text;
using RAGE;
using RAGE.Elements;
using RAGE.NUI;

namespace cs_packages.test
{
    public class Handling : Events.Script
    {
        private static float fInitialDriveMaxFlatVel = (float)3.6;
        private static float fBrakeBiasFront = 2;
        private static float fSteeringLock = (float)0.017453292;
        private static float fTractionCurveLateral = (float)0.017453292;
        private static float fTractionBiasFront = 2;
        private static float fSuspensionCompDamp = 10;
        private static float fSuspensionReboundDamp = 10;
        private static float fSuspensionBiasFront = 2;
        private static float fAntiRollBarBiasFront = 2;

        private Handling()
        {
            Events.OnPlayerCommand += OnPlayerCommand;
            Events.Add("add_SetHandling", SetHandlingFromServer);
        }

        private void SetHandlingFromServer(object[] args)
        {
            Vehicle veh = (Vehicle)args[0];
            int id = Convert.ToInt32(args[1]);

            if (id == 0) return;
            switch (id)
            {
                case 1:
                    {
                        veh.SetHandling("fInitialDragCoeff", (float)6.220000);

                        veh.SetHandling("vecCentreOfMassOffset", new Vector3((float)0.0, (float)0.0, (float)0.0));
                        veh.SetHandling("vecInertiaMultiplier", new Vector3((float)1.200000, (float)1.200000, (float)1.600000));

                        veh.SetHandling("fDriveBiasFront", (float)0.0000);
                        veh.SetHandling("nInitialDriveGears", 3);
                        veh.SetHandling("fInitialDriveForce", (float)5.430000);
                        veh.SetHandling("fDriveInertia", (float)1.300000);
                        veh.SetHandling("fClutchChangeRateScaleUpShift", (float)1.600000);
                        veh.SetHandling("fClutchChangeRateScaleDownShift", (float)1.600000);
                        veh.SetHandling("fInitialDriveMaxFlatVel", (float)160.000000 / fInitialDriveMaxFlatVel);

                        veh.SetHandling("fBrakeForce", (float)1.500000);
                        veh.SetHandling("fBrakeBiasFront", (float)0.550000 * fBrakeBiasFront);
                        veh.SetHandling("fHandBrakeForce", (float)1.200000);
                        veh.SetHandling("fSteeringLock", (float)63.000000 * fSteeringLock);

                        veh.SetHandling("fTractionCurveMax", (float)0.900000);
                        veh.SetHandling("fTractionCurveMin", (float)1.400000);
                        veh.SetHandling("fTractionCurveLateral", (float)22.500000 * fTractionCurveLateral);
                        veh.SetHandling("fTractionSpringDeltaMax", (float)0.150000);

                        veh.SetHandling("fLowSpeedTractionLossMult", (float)1.000000);
                        veh.SetHandling("fCamberStiffnesss", (float)0.000000);
                        veh.SetHandling("fTractionBiasFront", (float)0.500000 * fTractionBiasFront);
                        veh.SetHandling("fTractionLossMult", (float)1.000000);
                        veh.SetHandling("fSuspensionForce", (float)3.000000);
                        veh.SetHandling("fSuspensionCompDamp", (float)1.800000 / fSuspensionCompDamp);
                        veh.SetHandling("fSuspensionReboundDamp", (float)2.800000 / fSuspensionReboundDamp);
                        veh.SetHandling("fSuspensionUpperLimit", (float)0.100000);
                        veh.SetHandling("fSuspensionLowerLimit", (float)-0.160000);
                        veh.SetHandling("fSuspensionRaise", (float)-0.04000);
                        veh.SetHandling("fSuspensionBiasFront", (float)0.500000 * fSuspensionBiasFront);

                        veh.SetHandling("fAntiRollBarForce", (float)0.500000);
                        veh.SetHandling("fAntiRollBarBiasFront", (float)0.470000 * fAntiRollBarBiasFront);
                        veh.SetHandling("fRollCentreHeightFront", (float)0.200000);
                        veh.SetHandling("fRollCentreHeightRear", (float)0.250000);
                        break;
                    }
                case 2:
                    {
                        veh.SetHandling("fInitialDragCoeff", (float)15.5);

                        veh.SetHandling("vecCentreOfMassOffset", new Vector3((float)0.0, (float)0.0, (float)0.0));
                        veh.SetHandling("vecInertiaMultiplier", new Vector3((float)1.0, (float)1.0, (float)1.0));

                        veh.SetHandling("fDriveBiasFront", (float)0.0000);
                        veh.SetHandling("nInitialDriveGears", 6);
                        veh.SetHandling("fInitialDriveForce", (float)1.900000);
                        veh.SetHandling("fDriveInertia", (float)1.0000);
                        veh.SetHandling("fClutchChangeRateScaleUpShift", (float)5.00000);
                        veh.SetHandling("fClutchChangeRateScaleDownShift", (float)5.00000);
                        veh.SetHandling("fInitialDriveMaxFlatVel", (float)200.000000 / fInitialDriveMaxFlatVel);
                        veh.SetHandling("fBrakeForce", (float)3.500000);
                        veh.SetHandling("fBrakeBiasFront", (float)0.670000 * fBrakeBiasFront);
                        veh.SetHandling("fHandBrakeForce", (float)3.500000);
                        veh.SetHandling("fSteeringLock", (float)57.000000 * fSteeringLock);

                        veh.SetHandling("fTractionCurveMax", (float)1.000000);
                        veh.SetHandling("fTractionCurveMin", (float)1.450000);
                        veh.SetHandling("fTractionCurveLateral", (float)35.000000 * fTractionCurveLateral);
                        veh.SetHandling("fTractionSpringDeltaMax", (float)0.150000);

                        veh.SetHandling("fLowSpeedTractionLossMult", (float)0.500000);
                        veh.SetHandling("fCamberStiffnesss", (float)0.000000);
                        veh.SetHandling("fTractionBiasFront", (float)0.450000 * fTractionBiasFront);
                        veh.SetHandling("fTractionLossMult", (float)1.000000);
                        veh.SetHandling("fSuspensionForce", (float)2.800000);
                        veh.SetHandling("fSuspensionCompDamp", (float)1.400000 / fSuspensionCompDamp);
                        veh.SetHandling("fSuspensionReboundDamp", (float)2.200000 / fSuspensionReboundDamp);
                        veh.SetHandling("fSuspensionUpperLimit", (float)0.060000);
                        veh.SetHandling("fSuspensionLowerLimit", (float)-0.050000);
                        veh.SetHandling("fSuspensionRaise", (float)-0.00000);
                        veh.SetHandling("fSuspensionBiasFront", (float)0.500000 * fSuspensionBiasFront);
                        break;
                    }
                case 3:
                    {
                        veh.SetHandling("fInitialDragCoeff", (float)8.0);

                        veh.SetHandling("vecCentreOfMassOffset", new Vector3((float)0.0, (float)-0.1, (float)0.0));
                        veh.SetHandling("vecInertiaMultiplier", new Vector3((float)1.2, (float)1.6, (float)1.9));

                        veh.SetHandling("fDriveBiasFront", (float)0.0000);
                        veh.SetHandling("nInitialDriveGears", 6);
                        veh.SetHandling("fInitialDriveForce", (float)3.584000);
                        veh.SetHandling("fDriveInertia", (float)1.0000);

                        veh.SetHandling("fClutchChangeRateScaleUpShift", (float)3.00000);
                        veh.SetHandling("fClutchChangeRateScaleDownShift", (float)3.00000);
                        veh.SetHandling("fInitialDriveMaxFlatVel", (float)380.1999970000 / fInitialDriveMaxFlatVel);
                        veh.SetHandling("fBrakeForce", (float)0.9000);
                        veh.SetHandling("fBrakeBiasFront", (float)0.450000 * fBrakeBiasFront);
                        veh.SetHandling("fHandBrakeForce", (float)0.800000);
                        veh.SetHandling("fSteeringLock", (float)58.000000 * fSteeringLock);

                        veh.SetHandling("fTractionCurveMax", (float)0.2300000);
                        veh.SetHandling("fTractionCurveMin", (float)1.050000);
                        veh.SetHandling("fTractionCurveLateral", (float)25.500000 * fTractionCurveLateral);
                        veh.SetHandling("fTractionSpringDeltaMax", (float)0.050000);

                        veh.SetHandling("fLowSpeedTractionLossMult", (float)1.000000);
                        veh.SetHandling("fCamberStiffnesss", (float)0.000000);
                        veh.SetHandling("fTractionBiasFront", (float)0.4840000 * fTractionBiasFront);
                        veh.SetHandling("fTractionLossMult", (float)1.000000);
                        veh.SetHandling("fSuspensionForce", (float)2.450000);
                        veh.SetHandling("fSuspensionCompDamp", (float)1.400000 / fSuspensionCompDamp);
                        veh.SetHandling("fSuspensionReboundDamp", (float)3.100000 / fSuspensionReboundDamp);
                        veh.SetHandling("fSuspensionUpperLimit", (float)0.100000);
                        veh.SetHandling("fSuspensionLowerLimit", (float)-0.080000);
                        veh.SetHandling("fSuspensionRaise", (float)-0.00000);
                        veh.SetHandling("fSuspensionBiasFront", (float)0.4800000 * fSuspensionBiasFront);

                        veh.SetHandling("fAntiRollBarForce", (float)0.700000);
                        veh.SetHandling("fAntiRollBarBiasFront", (float)0.650000 * fAntiRollBarBiasFront);
                        veh.SetHandling("fRollCentreHeightFront", (float)0.2300000);
                        veh.SetHandling("fRollCentreHeightRear", (float)0.180000);
                        break;
                    }
                case 4:
                    {
                        veh.SetHandling("fInitialDragCoeff", (float)15.50000);

                        veh.SetHandling("vecCentreOfMassOffset", new Vector3((float)0.0, (float)0.0, (float)0.0));
                        veh.SetHandling("vecInertiaMultiplier", new Vector3((float)1.200000, (float)1.200000, (float)1.600000));

                        veh.SetHandling("fDriveBiasFront", (float)0.0000);
                        veh.SetHandling("nInitialDriveGears", 6);
                        veh.SetHandling("fInitialDriveForce", (float)1.900);
                        veh.SetHandling("fDriveInertia", (float)1.000);
                        veh.SetHandling("fClutchChangeRateScaleUpShift", (float)1.600000);
                        veh.SetHandling("fClutchChangeRateScaleDownShift", (float)1.600000);
                        veh.SetHandling("fInitialDriveMaxFlatVel", (float)160.000000 / fInitialDriveMaxFlatVel);

                        veh.SetHandling("fBrakeForce", (float)4.8500000);
                        veh.SetHandling("fBrakeBiasFront", (float)0.670000 * fBrakeBiasFront);
                        veh.SetHandling("fHandBrakeForce", (float)3.500000);
                        veh.SetHandling("fSteeringLock", (float)52.000000 * fSteeringLock);

                        veh.SetHandling("fTractionCurveMax", (float)0.9500);
                        veh.SetHandling("fTractionCurveMin", (float)1.300);
                        veh.SetHandling("fTractionCurveLateral", (float)24.5000 * fTractionCurveLateral);
                        veh.SetHandling("fTractionSpringDeltaMax", (float)0.150000);

                        veh.SetHandling("fLowSpeedTractionLossMult", (float)1.000000);
                        veh.SetHandling("fCamberStiffnesss", (float)0.000000);
                        veh.SetHandling("fTractionBiasFront", (float)0.450000 * fTractionBiasFront);
                        veh.SetHandling("fTractionLossMult", (float)1.000000);
                        veh.SetHandling("fSuspensionForce", (float)2.500000);
                        veh.SetHandling("fSuspensionCompDamp", (float)2.600000 / fSuspensionCompDamp);
                        veh.SetHandling("fSuspensionReboundDamp", (float)3.00000 / fSuspensionReboundDamp);
                        veh.SetHandling("fSuspensionUpperLimit", (float)0.100000);
                        veh.SetHandling("fSuspensionLowerLimit", (float)-0.10000);
                        veh.SetHandling("fSuspensionRaise", (float)-0.00000);
                        veh.SetHandling("fSuspensionBiasFront", (float)0.500000 * fSuspensionBiasFront);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        /*public static void SetHandling(Vehicle veh, int id)
        {
            if (id == 0) return;
            switch (id)
            {
                case 1:
                    {
                        veh.SetHandling("fInitialDragCoeff", (float)6.220000);
                        
                        veh.SetHandling("vecCentreOfMassOffset", new Vector3((float)0.0, (float)0.0, (float)0.0));
                        veh.SetHandling("vecInertiaMultiplier", new Vector3((float)1.200000, (float)1.200000, (float)1.600000));
                        
                        veh.SetHandling("fDriveBiasFront", (float)0.0000);
                        veh.SetHandling("nInitialDriveGears", 3);
                        veh.SetHandling("fInitialDriveForce", (float)5.430000);
                        veh.SetHandling("fDriveInertia", (float)1.300000);
                        veh.SetHandling("fClutchChangeRateScaleUpShift", (float)1.600000);
                        veh.SetHandling("fClutchChangeRateScaleDownShift", (float)1.600000);
                        veh.SetHandling("fInitialDriveMaxFlatVel", (float)160.000000 / fInitialDriveMaxFlatVel);
                        
                        veh.SetHandling("fBrakeForce", (float)1.500000);
                        veh.SetHandling("fBrakeBiasFront", (float)0.550000 * fBrakeBiasFront);
                        veh.SetHandling("fHandBrakeForce", (float)1.200000);
                        veh.SetHandling("fSteeringLock", (float)63.000000 * fSteeringLock);
                        
                        veh.SetHandling("fTractionCurveMax", (float)0.900000);
                        veh.SetHandling("fTractionCurveMin", (float)1.400000);
                        veh.SetHandling("fTractionCurveLateral", (float)22.500000 * fTractionCurveLateral);
                        veh.SetHandling("fTractionSpringDeltaMax", (float)0.150000);
                        
                        veh.SetHandling("fLowSpeedTractionLossMult", (float)1.000000);
                        veh.SetHandling("fCamberStiffnesss", (float)0.000000);
                        veh.SetHandling("fTractionBiasFront", (float)0.500000 * fTractionBiasFront);
                        veh.SetHandling("fTractionLossMult", (float)1.000000);
                        veh.SetHandling("fSuspensionForce", (float)3.000000);
                        veh.SetHandling("fSuspensionCompDamp", (float)1.800000 / fSuspensionCompDamp);
                        veh.SetHandling("fSuspensionReboundDamp", (float)2.800000 / fSuspensionReboundDamp);
                        veh.SetHandling("fSuspensionUpperLimit", (float)0.100000);
                        veh.SetHandling("fSuspensionLowerLimit", (float)-0.160000);
                        veh.SetHandling("fSuspensionRaise", (float)-0.04000);
                        veh.SetHandling("fSuspensionBiasFront", (float)0.500000 * fSuspensionBiasFront);
                        
                        veh.SetHandling("fAntiRollBarForce", (float)0.500000);
                        veh.SetHandling("fAntiRollBarBiasFront", (float)0.470000 * fAntiRollBarBiasFront);
                        veh.SetHandling("fRollCentreHeightFront", (float)0.200000);
                        veh.SetHandling("fRollCentreHeightRear", (float)0.250000);
                        break;
                    }
                case 2:
                    {

                        veh.SetHandling("fInitialDragCoeff", (float)15.5);
                        
                        veh.SetHandling("vecCentreOfMassOffset", new Vector3((float)0.0, (float)0.0, (float)0.0));
                        veh.SetHandling("vecInertiaMultiplier", new Vector3((float)1.0, (float)1.0, (float)1.0));
                        
                        veh.SetHandling("fDriveBiasFront", (float)0.0000);
                        veh.SetHandling("nInitialDriveGears", 6);
                        veh.SetHandling("fInitialDriveForce", (float)1.900000);
                        veh.SetHandling("fDriveInertia", (float)1.0000);
                        veh.SetHandling("fClutchChangeRateScaleUpShift", (float)5.00000);
                        veh.SetHandling("fClutchChangeRateScaleDownShift", (float)5.00000);
                        veh.SetHandling("fInitialDriveMaxFlatVel", (float)200.000000 / fInitialDriveMaxFlatVel);
                        veh.SetHandling("fBrakeForce", (float)3.500000);
                        veh.SetHandling("fBrakeBiasFront", (float)0.670000 * fBrakeBiasFront);
                        veh.SetHandling("fHandBrakeForce", (float)3.500000);
                        veh.SetHandling("fSteeringLock", (float)57.000000 * fSteeringLock);
                        
                        veh.SetHandling("fTractionCurveMax", (float)1.000000);
                        veh.SetHandling("fTractionCurveMin", (float)1.450000);
                        veh.SetHandling("fTractionCurveLateral", (float)35.000000 * fTractionCurveLateral);
                        veh.SetHandling("fTractionSpringDeltaMax", (float)0.150000);
                        
                        veh.SetHandling("fLowSpeedTractionLossMult", (float)0.500000);
                        veh.SetHandling("fCamberStiffnesss", (float)0.000000);
                        veh.SetHandling("fTractionBiasFront", (float)0.450000 * fTractionBiasFront);
                        veh.SetHandling("fTractionLossMult", (float)1.000000);
                        veh.SetHandling("fSuspensionForce", (float)2.800000);
                        veh.SetHandling("fSuspensionCompDamp", (float)1.400000 / fSuspensionCompDamp);
                        veh.SetHandling("fSuspensionReboundDamp", (float)2.200000 / fSuspensionReboundDamp);
                        veh.SetHandling("fSuspensionUpperLimit", (float)0.060000);
                        veh.SetHandling("fSuspensionLowerLimit", (float)-0.050000);
                        veh.SetHandling("fSuspensionRaise", (float)-0.00000);
                        veh.SetHandling("fSuspensionBiasFront", (float)0.500000 * fSuspensionBiasFront);
                        break;
                    }
                case 3:
                    {
                        veh.SetHandling("fInitialDragCoeff", (float)8.0);
                        
                        veh.SetHandling("vecCentreOfMassOffset", new Vector3((float)0.0, (float)-0.1, (float)0.0));
                        veh.SetHandling("vecInertiaMultiplier", new Vector3((float)1.2, (float)1.6, (float)1.9));
                        
                        veh.SetHandling("fDriveBiasFront", (float)0.0000);
                        veh.SetHandling("nInitialDriveGears", 6);
                        veh.SetHandling("fInitialDriveForce", (float)3.584000);
                        veh.SetHandling("fDriveInertia", (float)1.0000);
                        
                        veh.SetHandling("fClutchChangeRateScaleUpShift", (float)3.00000);
                        veh.SetHandling("fClutchChangeRateScaleDownShift", (float)3.00000);
                        veh.SetHandling("fInitialDriveMaxFlatVel", (float)380.1999970000 / fInitialDriveMaxFlatVel);
                        veh.SetHandling("fBrakeForce", (float)0.9000);
                        veh.SetHandling("fBrakeBiasFront", (float)0.450000 * fBrakeBiasFront);
                        veh.SetHandling("fHandBrakeForce", (float)0.800000);
                        veh.SetHandling("fSteeringLock", (float)58.000000 * fSteeringLock);
                        
                        veh.SetHandling("fTractionCurveMax", (float)0.2300000);
                        veh.SetHandling("fTractionCurveMin", (float)1.050000);
                        veh.SetHandling("fTractionCurveLateral", (float)25.500000 * fTractionCurveLateral);
                        veh.SetHandling("fTractionSpringDeltaMax", (float)0.050000);
                        
                        veh.SetHandling("fLowSpeedTractionLossMult", (float)1.000000);
                        veh.SetHandling("fCamberStiffnesss", (float)0.000000);
                        veh.SetHandling("fTractionBiasFront", (float)0.4840000 * fTractionBiasFront);
                        veh.SetHandling("fTractionLossMult", (float)1.000000);
                        veh.SetHandling("fSuspensionForce", (float)2.450000);
                        veh.SetHandling("fSuspensionCompDamp", (float)1.400000 / fSuspensionCompDamp);
                        veh.SetHandling("fSuspensionReboundDamp", (float)3.100000 / fSuspensionReboundDamp);
                        veh.SetHandling("fSuspensionUpperLimit", (float)0.100000);
                        veh.SetHandling("fSuspensionLowerLimit", (float)-0.080000);
                        veh.SetHandling("fSuspensionRaise", (float)-0.00000);
                        veh.SetHandling("fSuspensionBiasFront", (float)0.4800000 * fSuspensionBiasFront);
                        
                        veh.SetHandling("fAntiRollBarForce", (float)0.700000);
                        veh.SetHandling("fAntiRollBarBiasFront", (float)0.650000 * fAntiRollBarBiasFront);
                        veh.SetHandling("fRollCentreHeightFront", (float)0.2300000);
                        veh.SetHandling("fRollCentreHeightRear", (float)0.180000);
                        break;
                    }
                case 4:
                    {
                        veh.SetHandling("fInitialDragCoeff", (float)15.50000);
                        
                        veh.SetHandling("vecCentreOfMassOffset", new Vector3((float)0.0, (float)0.0, (float)0.0));
                        veh.SetHandling("vecInertiaMultiplier", new Vector3((float)1.200000, (float)1.200000, (float)1.600000));
                        
                        veh.SetHandling("fDriveBiasFront", (float)0.0000);
                        veh.SetHandling("nInitialDriveGears", 6);
                        veh.SetHandling("fInitialDriveForce", (float)1.900);
                        veh.SetHandling("fDriveInertia", (float)1.000);
                        veh.SetHandling("fClutchChangeRateScaleUpShift", (float)1.600000);
                        veh.SetHandling("fClutchChangeRateScaleDownShift", (float)1.600000);
                        veh.SetHandling("fInitialDriveMaxFlatVel", (float)160.000000 / fInitialDriveMaxFlatVel);
                        
                        veh.SetHandling("fBrakeForce", (float)4.8500000);
                        veh.SetHandling("fBrakeBiasFront", (float)0.670000 * fBrakeBiasFront);
                        veh.SetHandling("fHandBrakeForce", (float)3.500000);
                        veh.SetHandling("fSteeringLock", (float)52.000000 * fSteeringLock);
                        
                        veh.SetHandling("fTractionCurveMax", (float)0.9500);
                        veh.SetHandling("fTractionCurveMin", (float)1.300);
                        veh.SetHandling("fTractionCurveLateral", (float)24.5000 * fTractionCurveLateral);
                        veh.SetHandling("fTractionSpringDeltaMax", (float)0.150000);
                        
                        veh.SetHandling("fLowSpeedTractionLossMult", (float)1.000000);
                        veh.SetHandling("fCamberStiffnesss", (float)0.000000);
                        veh.SetHandling("fTractionBiasFront", (float)0.450000 * fTractionBiasFront);
                        veh.SetHandling("fTractionLossMult", (float)1.000000);
                        veh.SetHandling("fSuspensionForce", (float)2.500000);
                        veh.SetHandling("fSuspensionCompDamp", (float)2.600000 / fSuspensionCompDamp);
                        veh.SetHandling("fSuspensionReboundDamp", (float)3.00000 / fSuspensionReboundDamp);
                        veh.SetHandling("fSuspensionUpperLimit", (float)0.100000);
                        veh.SetHandling("fSuspensionLowerLimit", (float)-0.10000);
                        veh.SetHandling("fSuspensionRaise", (float)-0.00000);
                        veh.SetHandling("fSuspensionBiasFront", (float)0.500000 * fSuspensionBiasFront);
                        break;
                    }
                default:
                    {
                        break;
                    }
                  
            }
        }*/

        public void OnPlayerCommand(string cmd, Events.CancelEventArgs cancel)
        {
            string[] args = cmd.Split(new char[] { ' ' });
            string commandName = args[0].Trim(new char[] { '/' });
            if (commandName == "drift")
            {
                Player.LocalPlayer.Vehicle.SetHandling("fInitialDragCoeff", (float)6.220000);

                Player.LocalPlayer.Vehicle.SetHandling("vecCentreOfMassOffset", new Vector3((float)0.0, (float)0.0, (float)0.0));
                Player.LocalPlayer.Vehicle.SetHandling("vecInertiaMultiplier", new Vector3((float)1.200000, (float)1.200000, (float)1.600000));

                Player.LocalPlayer.Vehicle.SetHandling("fDriveBiasFront", (float)0.0000);
                Player.LocalPlayer.Vehicle.SetHandling("nInitialDriveGears", 3);
                Player.LocalPlayer.Vehicle.SetHandling("fInitialDriveForce", (float)5.430000);
                Player.LocalPlayer.Vehicle.SetHandling("fDriveInertia", (float)1.300000);
                Player.LocalPlayer.Vehicle.SetHandling("fClutchChangeRateScaleUpShift", (float)1.600000);
                Player.LocalPlayer.Vehicle.SetHandling("fClutchChangeRateScaleDownShift", (float)1.600000);
                //Player.LocalPlayer.Vehicle.SetHandling("fInitialDriveMaxFlatVel", (float)160.000000 / fInitialDriveMaxFlatVel);

                Player.LocalPlayer.Vehicle.SetHandling("fBrakeForce", (float)1.500000);
                Player.LocalPlayer.Vehicle.SetHandling("fBrakeBiasFront", (float)0.550000 * fBrakeBiasFront);
                Player.LocalPlayer.Vehicle.SetHandling("fHandBrakeForce", (float)1.200000);
                Player.LocalPlayer.Vehicle.SetHandling("fSteeringLock", (float)63.000000 * fSteeringLock);

                Player.LocalPlayer.Vehicle.SetHandling("fTractionCurveMax", (float)0.900000);
                Player.LocalPlayer.Vehicle.SetHandling("fTractionCurveMin", (float)1.400000);
                Player.LocalPlayer.Vehicle.SetHandling("fTractionCurveLateral", (float)22.500000 * fTractionCurveLateral);
                Player.LocalPlayer.Vehicle.SetHandling("fTractionSpringDeltaMax", (float)0.150000);

                Player.LocalPlayer.Vehicle.SetHandling("fLowSpeedTractionLossMult", (float)1.000000);
                Player.LocalPlayer.Vehicle.SetHandling("fCamberStiffnesss", (float)0.000000);
                Player.LocalPlayer.Vehicle.SetHandling("fTractionBiasFront", (float)0.500000 * fTractionBiasFront);
                Player.LocalPlayer.Vehicle.SetHandling("fTractionLossMult", (float)1.000000);
                Player.LocalPlayer.Vehicle.SetHandling("fSuspensionForce", (float)3.000000);
                Player.LocalPlayer.Vehicle.SetHandling("fSuspensionCompDamp", (float)1.800000 / fSuspensionCompDamp);
                Player.LocalPlayer.Vehicle.SetHandling("fSuspensionReboundDamp", (float)2.800000 / fSuspensionReboundDamp);
                Player.LocalPlayer.Vehicle.SetHandling("fSuspensionUpperLimit", (float)0.100000);
                Player.LocalPlayer.Vehicle.SetHandling("fSuspensionLowerLimit", (float)-0.160000);
                Player.LocalPlayer.Vehicle.SetHandling("fSuspensionRaise", (float)-0.04000);
                Player.LocalPlayer.Vehicle.SetHandling("fSuspensionBiasFront", (float)0.500000 * fSuspensionBiasFront);

                Player.LocalPlayer.Vehicle.SetHandling("fAntiRollBarForce", (float)0.500000);
                Player.LocalPlayer.Vehicle.SetHandling("fAntiRollBarBiasFront", (float)0.470000 * fAntiRollBarBiasFront);
                Player.LocalPlayer.Vehicle.SetHandling("fRollCentreHeightFront", (float)0.200000);
                Player.LocalPlayer.Vehicle.SetHandling("fRollCentreHeightRear", (float)0.250000);

            }
            if (commandName == "drift1")
            {
                Player.LocalPlayer.Vehicle.SetHandling("fInitialDragCoeff", (float)15.5);

                Player.LocalPlayer.Vehicle.SetHandling("vecCentreOfMassOffset", new Vector3((float)0.0, (float)0.0, (float)0.0));
                Player.LocalPlayer.Vehicle.SetHandling("vecInertiaMultiplier", new Vector3((float)1.0, (float)1.0, (float)1.0));

                Player.LocalPlayer.Vehicle.SetHandling("fDriveBiasFront", (float)0.0000);
                Player.LocalPlayer.Vehicle.SetHandling("nInitialDriveGears", 6);
                Player.LocalPlayer.Vehicle.SetHandling("fInitialDriveForce", (float)1.900000);
                Player.LocalPlayer.Vehicle.SetHandling("fDriveInertia", (float)1.0000);
                Player.LocalPlayer.Vehicle.SetHandling("fClutchChangeRateScaleUpShift", (float)5.00000);
                Player.LocalPlayer.Vehicle.SetHandling("fClutchChangeRateScaleDownShift", (float)5.00000);
                Player.LocalPlayer.Vehicle.SetHandling("fInitialDriveMaxFlatVel", (float)200.000000 / fInitialDriveMaxFlatVel);
                Player.LocalPlayer.Vehicle.SetHandling("fBrakeForce", (float)3.500000);
                Player.LocalPlayer.Vehicle.SetHandling("fBrakeBiasFront", (float)0.670000 * fBrakeBiasFront);
                Player.LocalPlayer.Vehicle.SetHandling("fHandBrakeForce", (float)3.500000);
                Player.LocalPlayer.Vehicle.SetHandling("fSteeringLock", (float)57.000000 * fSteeringLock);

                Player.LocalPlayer.Vehicle.SetHandling("fTractionCurveMax", (float)1.000000);
                Player.LocalPlayer.Vehicle.SetHandling("fTractionCurveMin", (float)1.450000);
                Player.LocalPlayer.Vehicle.SetHandling("fTractionCurveLateral", (float)35.000000 * fTractionCurveLateral);
                Player.LocalPlayer.Vehicle.SetHandling("fTractionSpringDeltaMax", (float)0.150000);

                Player.LocalPlayer.Vehicle.SetHandling("fLowSpeedTractionLossMult", (float)0.500000);
                Player.LocalPlayer.Vehicle.SetHandling("fCamberStiffnesss", (float)0.000000);
                Player.LocalPlayer.Vehicle.SetHandling("fTractionBiasFront", (float)0.450000 * fTractionBiasFront);
                Player.LocalPlayer.Vehicle.SetHandling("fTractionLossMult", (float)1.000000);
                Player.LocalPlayer.Vehicle.SetHandling("fSuspensionForce", (float)2.800000);
                Player.LocalPlayer.Vehicle.SetHandling("fSuspensionCompDamp", (float)1.400000 / fSuspensionCompDamp);
                Player.LocalPlayer.Vehicle.SetHandling("fSuspensionReboundDamp", (float)2.200000 / fSuspensionReboundDamp);
                Player.LocalPlayer.Vehicle.SetHandling("fSuspensionUpperLimit", (float)0.060000);
                Player.LocalPlayer.Vehicle.SetHandling("fSuspensionLowerLimit", (float)-0.050000);
                Player.LocalPlayer.Vehicle.SetHandling("fSuspensionRaise", (float)-0.00000);
                Player.LocalPlayer.Vehicle.SetHandling("fSuspensionBiasFront", (float)0.500000 * fSuspensionBiasFront);
            }
            if (commandName == "drift2")
            {
                Player.LocalPlayer.Vehicle.SetHandling("fInitialDragCoeff", (float)8.0);

                Player.LocalPlayer.Vehicle.SetHandling("vecCentreOfMassOffset", new Vector3((float)0.0, (float)-0.1, (float)0.0));
                Player.LocalPlayer.Vehicle.SetHandling("vecInertiaMultiplier", new Vector3((float)1.2, (float)1.6, (float)1.9));

                Player.LocalPlayer.Vehicle.SetHandling("fDriveBiasFront", (float)0.0000);
                Player.LocalPlayer.Vehicle.SetHandling("nInitialDriveGears", 6);
                Player.LocalPlayer.Vehicle.SetHandling("fInitialDriveForce", (float)3.584000);
                Player.LocalPlayer.Vehicle.SetHandling("fDriveInertia", (float)1.0000);

                Player.LocalPlayer.Vehicle.SetHandling("fClutchChangeRateScaleUpShift", (float)3.00000);
                Player.LocalPlayer.Vehicle.SetHandling("fClutchChangeRateScaleDownShift", (float)3.00000);
                Player.LocalPlayer.Vehicle.SetHandling("fInitialDriveMaxFlatVel", (float)380.1999970000 / fInitialDriveMaxFlatVel);
                Player.LocalPlayer.Vehicle.SetHandling("fBrakeForce", (float)0.9000);
                Player.LocalPlayer.Vehicle.SetHandling("fBrakeBiasFront", (float)0.450000 * fBrakeBiasFront);
                Player.LocalPlayer.Vehicle.SetHandling("fHandBrakeForce", (float)0.800000);
                Player.LocalPlayer.Vehicle.SetHandling("fSteeringLock", (float)58.000000 * fSteeringLock);

                Player.LocalPlayer.Vehicle.SetHandling("fTractionCurveMax", (float)0.2300000);
                Player.LocalPlayer.Vehicle.SetHandling("fTractionCurveMin", (float)1.050000);
                Player.LocalPlayer.Vehicle.SetHandling("fTractionCurveLateral", (float)25.500000 * fTractionCurveLateral);
                Player.LocalPlayer.Vehicle.SetHandling("fTractionSpringDeltaMax", (float)0.050000);

                Player.LocalPlayer.Vehicle.SetHandling("fLowSpeedTractionLossMult", (float)1.000000);
                Player.LocalPlayer.Vehicle.SetHandling("fCamberStiffnesss", (float)0.000000);
                Player.LocalPlayer.Vehicle.SetHandling("fTractionBiasFront", (float)0.4840000 * fTractionBiasFront);
                Player.LocalPlayer.Vehicle.SetHandling("fTractionLossMult", (float)1.000000);
                Player.LocalPlayer.Vehicle.SetHandling("fSuspensionForce", (float)2.450000);
                Player.LocalPlayer.Vehicle.SetHandling("fSuspensionCompDamp", (float)1.400000 / fSuspensionCompDamp);
                Player.LocalPlayer.Vehicle.SetHandling("fSuspensionReboundDamp", (float)3.100000 / fSuspensionReboundDamp);
                Player.LocalPlayer.Vehicle.SetHandling("fSuspensionUpperLimit", (float)0.100000);
                Player.LocalPlayer.Vehicle.SetHandling("fSuspensionLowerLimit", (float)-0.080000);
                Player.LocalPlayer.Vehicle.SetHandling("fSuspensionRaise", (float)-0.00000);
                Player.LocalPlayer.Vehicle.SetHandling("fSuspensionBiasFront", (float)0.4800000 * fSuspensionBiasFront);

                Player.LocalPlayer.Vehicle.SetHandling("fAntiRollBarForce", (float)0.700000);
                Player.LocalPlayer.Vehicle.SetHandling("fAntiRollBarBiasFront", (float)0.650000 * fAntiRollBarBiasFront);
                Player.LocalPlayer.Vehicle.SetHandling("fRollCentreHeightFront", (float)0.2300000);
                Player.LocalPlayer.Vehicle.SetHandling("fRollCentreHeightRear", (float)0.180000);
            }
            if (commandName == "drift3")
            {
                Player.LocalPlayer.Vehicle.SetHandling("fInitialDragCoeff", (float)15.50000);

                Player.LocalPlayer.Vehicle.SetHandling("vecCentreOfMassOffset", new Vector3((float)0.0, (float)0.0, (float)0.0));
                Player.LocalPlayer.Vehicle.SetHandling("vecInertiaMultiplier", new Vector3((float)1.200000, (float)1.200000, (float)1.600000));

                Player.LocalPlayer.Vehicle.SetHandling("fDriveBiasFront", (float)0.0000);
                Player.LocalPlayer.Vehicle.SetHandling("nInitialDriveGears", 6);
                Player.LocalPlayer.Vehicle.SetHandling("fInitialDriveForce", (float)1.900);
                Player.LocalPlayer.Vehicle.SetHandling("fDriveInertia", (float)1.000);
                Player.LocalPlayer.Vehicle.SetHandling("fClutchChangeRateScaleUpShift", (float)1.600000);
                Player.LocalPlayer.Vehicle.SetHandling("fClutchChangeRateScaleDownShift", (float)1.600000);
                Player.LocalPlayer.Vehicle.SetHandling("fInitialDriveMaxFlatVel", (float)160.000000 / fInitialDriveMaxFlatVel);

                Player.LocalPlayer.Vehicle.SetHandling("fBrakeForce", (float)4.8500000);
                Player.LocalPlayer.Vehicle.SetHandling("fBrakeBiasFront", (float)0.670000 * fBrakeBiasFront);
                Player.LocalPlayer.Vehicle.SetHandling("fHandBrakeForce", (float)3.500000);
                Player.LocalPlayer.Vehicle.SetHandling("fSteeringLock", (float)52.000000 * fSteeringLock);

                Player.LocalPlayer.Vehicle.SetHandling("fTractionCurveMax", (float)0.9500);
                Player.LocalPlayer.Vehicle.SetHandling("fTractionCurveMin", (float)1.300);
                Player.LocalPlayer.Vehicle.SetHandling("fTractionCurveLateral", (float)24.5000 * fTractionCurveLateral);
                Player.LocalPlayer.Vehicle.SetHandling("fTractionSpringDeltaMax", (float)0.150000);

                Player.LocalPlayer.Vehicle.SetHandling("fLowSpeedTractionLossMult", (float)1.000000);
                Player.LocalPlayer.Vehicle.SetHandling("fCamberStiffnesss", (float)0.000000);
                Player.LocalPlayer.Vehicle.SetHandling("fTractionBiasFront", (float)0.450000 * fTractionBiasFront);
                Player.LocalPlayer.Vehicle.SetHandling("fTractionLossMult", (float)1.000000);
                Player.LocalPlayer.Vehicle.SetHandling("fSuspensionForce", (float)2.500000);
                Player.LocalPlayer.Vehicle.SetHandling("fSuspensionCompDamp", (float)2.600000 / fSuspensionCompDamp);
                Player.LocalPlayer.Vehicle.SetHandling("fSuspensionReboundDamp", (float)3.00000 / fSuspensionReboundDamp);
                Player.LocalPlayer.Vehicle.SetHandling("fSuspensionUpperLimit", (float)0.100000);
                Player.LocalPlayer.Vehicle.SetHandling("fSuspensionLowerLimit", (float)-0.10000);
                Player.LocalPlayer.Vehicle.SetHandling("fSuspensionRaise", (float)-0.00000);
                Player.LocalPlayer.Vehicle.SetHandling("fSuspensionBiasFront", (float)0.500000 * fSuspensionBiasFront);

                /*Player.LocalPlayer.Vehicle.SetHandling("fAntiRollBarForce", (float)0.500000);
                Player.LocalPlayer.Vehicle.SetHandling("fAntiRollBarBiasFront", (float)0.470000 * fAntiRollBarBiasFront);
                Player.LocalPlayer.Vehicle.SetHandling("fRollCentreHeightFront", (float)0.200000);
                Player.LocalPlayer.Vehicle.SetHandling("fRollCentreHeightRear", (float)0.250000);*/

            }
            if (commandName == "HUI")
            {
                Chat.Output("HUI");
            }
        }
    }
}
