using HarmonyLib;
using PulsarModLoader.Content.Components.Hull;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLShipComponent), "GetExtraLineLeft")]
    internal class LayeredArmorMaxLine
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
                    __result = "-\n" + PLLocalize.Localize("Armor (Max)") + "\n";
                    return false;
                }
                if (hull.SubType == HullModManager.Instance.GetHullIDFromName("Juggernaut Hull"))
                {
                    __result = "-\n" + PLLocalize.Localize("Incoming Dmg") + "\n";
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
