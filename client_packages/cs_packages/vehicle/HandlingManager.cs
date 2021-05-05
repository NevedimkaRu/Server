using RAGE;
using RAGE.NUI;
using cs_packages.utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace cs_packages.vehicle
{
    public class HandlingManager : Events.Script
    {
        List<string> handlingNames = new List<string>() 
        {
            "fInitialDragCoeff",
            "fDriveBiasFront", 
            "nInitialDriveGears", 
            "fInitialDriveForce", 
            "fDriveInertia", 
            "fClutchChangeRateScaleUpShift", 
            "fClutchChangeRateScaleDownShift", 
            "fInitialDriveMaxFlatVel", 
            "fBrakeForce", 
            "fBrakeBiasFront", 
            "fHandBrakeForce", 
            "fSteeringLock", 
            "fTractionCurveMax", 
            "fTractionCurveMin", 
            "fTractionCurveLateral", 
            "fTractionSpringDeltaMax",
            "fLowSpeedTractionLossMult",
            "fCamberStiffnesss", 
            "fTractionBiasFront", 
            "fTractionLossMult", 
            "fSuspensionForce", 
            "fSuspensionCompDamp", 
            "fSuspensionReboundDamp", 
            "fSuspensionUpperLimit", 
            "fSuspensionLowerLimit", 
            "fSuspensionRaise", 
            "fSuspensionBiasFront", 
            "fAntiRollBarForce", 
            "fAntiRollBarBiasFront",
            "fRollCentreHeightFront", 
            "fRollCentreHeightRear"
        };

        private bool menuactive = false;
        private MenuPool menuPool;
        public HandlingManager()
        {
            //Input.Bind(RAGE.Ui.VirtualKeys.F7, true, ShowHandlingMenu);//f4
        }

        private void ShowHandlingMenu()
        {
            if (menuactive) return;
            menuactive = true;
            Chat.Activate(false);
            Chat.Show(false);
            menuPool = new MenuPool();
            var mainMenu = new UIMenu("Handling", "");
            menuPool.Add(mainMenu);
            
            for(int i = 0; i < handlingNames.Count; i++)
            {
                UIMenuItem handle = new UIMenuItem(handlingNames[i], "");
                handle.SetRightLabel(Convert.ToString(RAGE.Elements.Player.LocalPlayer.Vehicle.GetHandlingFloat(handlingNames[i])));
                mainMenu.AddItem(handle);

                /*handle.SetItemData(handlingNames[i]);
                mainMenu.OnListChange += (sender, item, index) =>
                {
                    RAGE.Elements.Player.LocalPlayer.Vehicle.SetHandling(Convert.ToString(item.ItemData), Convert.ToSingle(item.IndexToItem(index)));

                };*/
                /*
                UIMenuListItem handle = new UIMenuListItem(handlingNames[i], Utils.GetFloatList(0.01f, 2, 0.1f), 0);
                mainMenu.AddItem(handle);
                
                handle.SetItemData(handlingNames[i]);
                mainMenu.OnListChange += (sender, item, index) =>
                {
                    RAGE.Elements.Player.LocalPlayer.Vehicle.SetHandling(Convert.ToString(item.ItemData), Convert.ToSingle(item.IndexToItem(index)));

                };*/
            }
            UIMenuItem handle1 = new UIMenuItem("vecCentreOfMassOffset", "");
            handle1.SetRightLabel(Convert.ToString(RAGE.Elements.Player.LocalPlayer.Vehicle.GetHandlingVector("vecCentreOfMassOffset")));
            mainMenu.AddItem(handle1);
            UIMenuItem handle2 = new UIMenuItem("vecInertiaMultiplier", "");
            handle2.SetRightLabel(Convert.ToString(RAGE.Elements.Player.LocalPlayer.Vehicle.GetHandlingVector("vecInertiaMultiplier")));
            mainMenu.AddItem(handle2);
            menuPool.RefreshIndex();
            Events.Tick += DrawMenu;
            mainMenu.Visible = true;

            mainMenu.OnMenuClose += (sender) =>
            {
                Chat.Activate(true);
                Chat.Show(true);
                menuactive = false;
                Events.Tick -= DrawMenu;
            };
        }

        private void DrawMenu(List<Events.TickNametagData> nametags)
        {
            if (menuactive)
            {
                menuPool.ProcessMenus();
            }
        }
    }
}
