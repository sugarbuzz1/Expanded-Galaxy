
using HarmonyLib;
using System;
using UnityEngine;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLScientistSensorScreen), "OnButtonMouseAway")]
    internal class SensorButtonMouseAway
    {
        private static bool Prefix(PLScientistSensorScreen __instance, UIWidget inButton, ref UIWidget ___LastHoverButton)
        {
            if (inButton.name.Contains("compScanOption"))
            {
                int index = Int32.Parse(inButton.name.Split('_')[1]);
                if (SensorScreenButtonClick.cachedComponentScan.Count > index + SensorScreenButtonClick.pageIndex * 8)
                {
                    Color color = PLGlobal.GetColorBGForWare(SensorScreenButtonClick.cachedComponentScan[index + SensorScreenButtonClick.pageIndex * 8]) * 0.5f;
                    color.a = 1f;
                    inButton.color = color;
                    inButton.GetComponentInChildren<UILabel>().color = inButton.color;
                }
                if (!(inButton == ___LastHoverButton))
                    return false;
                PLInGameUI.HidePersistantLargeTooltip();
                ___LastHoverButton = null;
                return false;
            }
            return true;
        }
    }
}
