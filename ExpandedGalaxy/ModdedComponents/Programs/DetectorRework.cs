using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLSensorObject), "InternalDetectionSignatureIsDetected")]
    internal class DetectorRework
    {
        private static void Postfix(PLSensorObject __instance, ref PLSensorObjectCacheData socd, float sqDist, float signature, float detection, PLShipInfoBase detectorShip)
        {
            if (!(__instance is PLSensorObjectShip))
                return;
            socd.IsDetected |= Programs.HasActiveProgramOfType(detectorShip, 15);
        }
    }
}

