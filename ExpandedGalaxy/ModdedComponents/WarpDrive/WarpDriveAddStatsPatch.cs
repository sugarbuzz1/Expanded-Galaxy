using HarmonyLib;
using UnityEngine;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLWarpDrive), "AddStats")]
    internal class WarpDriveAddStatsPatch
    {
        private static bool Prefix(PLWarpDrive __instance, PLShipStats inStats)
        {
            inStats.WarpChargeRate += __instance.ChargeSpeed * __instance.LevelMultiplier(0.25f) * __instance.GetPowerPercentInput();
            inStats.WarpRange += (float)((double)__instance.WarpRange * (double)__instance.LevelMultiplier(0.2f) + (double)Mathf.Clamp((int)__instance.SubTypeData, 0, 5) * 0.05);
            inStats.EMSignature += __instance.EnergySignatureAmt * __instance.GetPowerPercentInput();
            return false;
        }
    }
}
