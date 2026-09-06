using HarmonyLib;
using UnityEngine;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLTurret), "Tick")]
    internal class FixTargetting
    {
        private static void Postfix(PLTurret __instance)
        {
            if (!__instance.IsEquipped || !(__instance.TurretInstance != null))
                return;
            Vector3 eulerAngles = __instance.targetWorldRot.eulerAngles;
            if (float.IsNaN(eulerAngles.x) || float.IsNaN(eulerAngles.y) || float.IsNaN(eulerAngles.z))
                __instance.targetWorldRot.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
        }
    }

}
