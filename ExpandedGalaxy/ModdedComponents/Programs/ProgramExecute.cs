using HarmonyLib;
using PulsarModLoader.Content.Components.Hull;
using System.Collections;
using UnityEngine;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLWarpDriveProgram), "ExecuteBasedOnType")]
    internal class ProgramExecute
    {
        internal static int FramesToWait = 10;
        private static bool Prefix(PLWarpDriveProgram __instance)
        {
            if (__instance.SubType == (int)EWarpDriveProgramType.BARRAGE)
            {
                __instance.Level = 0;
                foreach (PLShipComponent component in __instance.ShipStats.AllComponents)
                {
                    if (component is PLTurret)
                    {
                        PLTurret turret = (PLTurret)component;
                        if (!turret.IsOverheated)
                        {
                            turret.Heat -= 0.5f;
                            turret.Heat = Mathf.Clamp01(turret.Heat);
                        }
                    }
                }
                return false;
            }
            if (__instance.SubType != (int)EWarpDriveProgramType.EXTENDED_SHIELDS)
                return true;
            __instance.Level = 0;
            if (__instance.GetActiveTimerAlpha() > 0f && __instance.GetActiveTimerAlpha() < 1f)
                return true;
            if (__instance.ShipStats.Ship.MyShieldGenerator != null)
            {
                if (__instance.ShipStats.Ship.MyHull != null && __instance.ShipStats.Ship.MyHull.SubType == HullModManager.Instance.GetHullIDFromName("Juggernaut Hull"))
                    return true;
                __instance.ShipStats.Ship.StartCoroutine(LateChargeShields(__instance.ShipStats.Ship));
            }
            return true;
        }

        private static IEnumerator LateChargeShields(PLShipInfoBase shipInfoBase)
        {
            if (shipInfoBase == null || shipInfoBase.MyShieldGenerator == null)
                yield break;
            float shieldAmount = shipInfoBase.MyShieldGenerator.Current + 99f;
            shieldAmount = Mathf.Clamp(shieldAmount, 0f, shipInfoBase.MyStats.ShieldsMax + 100f);
            for (int i = 0; i < FramesToWait; i++)
                yield return new WaitForEndOfFrame();
            if (shipInfoBase != null && shipInfoBase.MyShieldGenerator != null)
            {
                do
                {
                    shipInfoBase.MyShieldGenerator.Current += 100f;
                    shipInfoBase.MyShieldGenerator.Current = Mathf.Clamp(shipInfoBase.MyShieldGenerator.Current, 0f, shipInfoBase.MyStats.ShieldsMax);
                    yield return new WaitForEndOfFrame();
                }
                while (shipInfoBase.MyShieldGenerator.Current < shieldAmount);
            }
        }
    }
}

