using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLShipComponent), "AddStats")]
    internal class CloakAddStats
    {
        private static void Postfix(PLShipComponent __instance, PLShipStats inStats)
        {
            if (!(__instance is PLCloakingSystem))
                return;
            if (__instance.SubType == (int)ECloakingSystemType.E_NORMAL)
                inStats.TurretDamageFactor *= 1f + (Cloak.SubTypeDataParse(__instance.SubTypeData) / 100f);
        }
    }
}
