using HarmonyLib;
using UnityEngine;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLWarpDriveProgram), "FinalLateAddStats")]
    internal class OverdriveBuff
    {
        private static void Postfix(PLWarpDriveProgram __instance, PLShipStats inStats, ref float ___Overcharge_LastActivationTime, ref float ___Overcharge_ActiveTime)
        {
            if ((double)Time.time - (double)___Overcharge_LastActivationTime < (double)___Overcharge_ActiveTime)
            {
                if (inStats.Ship.CoreInstability > 1.0f)
                    inStats.Ship.CoreInstability = 1.0f;
            }
        }
    }
}

