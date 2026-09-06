using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLShipComponent), "FinalLateAddStats")]
    internal class CloakFinalLateAddStats
    {
        private static void Postfix(PLShipComponent __instance, PLShipStats inStats)
        {
            if (!(__instance is PLCloakingSystem))
                return;
            if (__instance.SubType == (int)ECloakingSystemType.E_SYVASSI)
            {
                inStats.ShieldsChargeRate *= 1f + (Cloak.SubTypeDataParse(__instance.SubTypeData) / 100f);
                inStats.ShieldsChargeRateMax *= 1f + (Cloak.SubTypeDataParse(__instance.SubTypeData) / 100f);
            }
        }
    }
}
