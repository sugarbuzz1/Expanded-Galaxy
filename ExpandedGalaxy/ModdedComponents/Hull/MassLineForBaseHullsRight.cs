using HarmonyLib;
using PulsarModLoader.Content.Components.Hull;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLHull), "GetStatLineRight")]
    internal class MassLineForBaseHullsRight
    {
        private static bool Prefix(PLHull __instance, ref string __result)
        {
            if (__instance.SubType < HullModManager.Instance.VanillaHullMaxType)
            {
                if (Hull.GetBaseHullMass(__instance.SubType, __instance.Level) == 0f)
                    return true;
                __result = (__instance.Max * __instance.LevelMultiplier(0.2f)).ToString("0") + "\n" + (__instance.Armor * 250f * __instance.LevelMultiplier(0.15f)).ToString("0") + "\n" + (Hull.GetBaseHullMass(__instance.SubType, __instance.Level)).ToString("0") + "\n";
                return false;
            }
            return true;
        }
    }
}
