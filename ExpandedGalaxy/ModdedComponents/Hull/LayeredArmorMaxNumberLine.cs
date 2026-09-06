using HarmonyLib;
using PulsarModLoader.Content.Components.Hull;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLShipComponent), "GetExtraLineRight")]
    internal class LayeredArmorMaxNumberLine
    {
        private static bool Prefix(PLShipComponent __instance, ref string __result)
        {
            PLHull hull = __instance as PLHull;
            if (hull == null)
            {
                return true;
            }
            else
            {
                if (hull.SubType == 9)
                {
                    __result = "Level " + (hull.Level + 1).ToString() + "\n" + (500f * __instance.LevelMultiplier(0.15f)).ToString("0") + "\n";
                    return false;
                }
                if (hull.SubType == HullModManager.Instance.GetHullIDFromName("Juggernaut Hull"))
                {
                    __result = "Level " + (hull.Level + 1).ToString() + "\n" + "-20%" + "\n";
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
    }
}
