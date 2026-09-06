using HarmonyLib;
using PulsarModLoader.Content.Components.MissionShipComponent;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLMissionShipComponent), "GetShortDesc")]
    internal class AmmunitionShortDesc
    {
        private static void Postfix(PLMissionShipComponent __instance, ref string __result)
        {
            if (__instance.SubType == MissionShipComponentModManager.Instance.GetMissionShipComponentIDFromName("Ammunition Cache"))
                __result = "Ammunition".ToUpper();
        }
    }
}
