using HarmonyLib;
using PulsarModLoader.Content.Components.Hull;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLHull), "AddStats")]
    internal class HullFinalAddStats
    {
        private static void Postfix(PLShipComponent __instance)
        {
            if (!(__instance is PLHull))
                return;
            int index = __instance.SubType - HullModManager.Instance.VanillaHullMaxType;
            if (index <= -1 || index >= HullModManager.Instance.HullTypes.Count || __instance.ShipStats == null)
                return;
            HullModManager.Instance.HullTypes[index].AddStats(__instance);
        }
    }
}
