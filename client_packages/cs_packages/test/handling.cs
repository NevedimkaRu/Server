using System;
using System.Collections.Generic;
using System.Text;
using RAGE;
using RAGE.Elements;
using RAGE.NUI;

namespace cs_packages.test
{
    class handling : Events.Script
    {
        private float fInitialDriveMaxFlatVel = (float)3.6;
        private float fBrakeBiasFront = 2;
        private float fSteeringLock = (float)0.017453292;
        private float fTractionCurveLateral = (float)0.017453292;
        private float fTractionBiasFront = 2;
        private float fSuspensionCompDamp = 10;
        private float fSuspensionReboundDamp = 10;
        private float fSuspensionBiasFront = 2;
        private float fAntiRollBarBiasFront = 2;

        public handling()
        {
            Events.OnPlayerCommand += OnPlayerCommand;
        }

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
            if(commandName == "drift1")
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
            if(commandName == "drift2")
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
        }
    }
}
