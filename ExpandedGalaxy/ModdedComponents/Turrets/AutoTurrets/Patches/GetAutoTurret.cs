using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLShipInfoBase), "GetTurretAtID")]
    internal class GetAutoTurret
    {
        private static void Postfix(PLShipInfoBase __instance, ref PLTurret __result, int inID)
        {
            if (__result != null)
                return;
            __result = __instance.GetAutoTurretAtID(inID);
        }
    }
}
