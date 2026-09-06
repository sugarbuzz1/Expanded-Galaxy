using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLVirus), "ShouldApplyRandSmallEffectID")]
    internal class RandSmallFix
    {
        private static bool Prefix(PLVirus __instance, int inID, ref bool __result)
        {
            if (__instance.SubType != 14 || __instance.NetID == -1)
            {
                __result = false;
                return false;
            }
            __result = (int)__instance.SubTypeData == inID;
            return false;
        }
    }
}

