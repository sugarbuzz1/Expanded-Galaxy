using HarmonyLib;
using UnityEngine;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLCorruptedDroneShipInfo), "Start")]
    internal class SentryStart
    {
        private static void Postfix(PLCorruptedDroneShipInfo __instance, ref float ___Server_LastEMPBlastTime)
        {
            ___Server_LastEMPBlastTime = Time.time;
        }
    }
}
