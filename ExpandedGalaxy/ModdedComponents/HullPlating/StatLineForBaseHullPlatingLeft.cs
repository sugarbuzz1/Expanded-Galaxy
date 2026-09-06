using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLWare), "GetStatLineLeft")]
    internal class StatLineForBaseHullPlatingLeft
    {
        private static bool Prefix(PLWare __instance, ref string __result)
        {
            PLHullPlating hullPlating = __instance as PLHullPlating;
            if (hullPlating != null && hullPlating.SubType == (int)EHullPlatingType.E_HULLPLATING_CCGE)
            {
                __result = PLLocalize.Localize("Armor") + "\n" + PLLocalize.Localize("Bottom Hit Dmg") + "\n" + PLLocalize.Localize("Mass") + "\n";
                return false;
            }
            return true;
        }
    }
}
