using HarmonyLib;
using PulsarModLoader.Content.Components.Hull;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLHull), "GetStatLineLeft")]
    internal class MassLineForBaseHullsLeft
    {
        private static bool Prefix(PLHull __instance, ref string __result)
        {
            if (__instance.SubType < HullModManager.Instance.VanillaHullMaxType)
            {
                if (Hull.GetBaseHullMass(__instance.SubType, __instance.Level) == 0f)
                    return true;
                __result = PLLocalize.Localize("Integrity") + "\n" + PLLocalize.Localize("Armor") + "\n" + PLLocalize.Localize("Mass") + "\n";
                return false;
            }
            return true;
        }
    }
}
