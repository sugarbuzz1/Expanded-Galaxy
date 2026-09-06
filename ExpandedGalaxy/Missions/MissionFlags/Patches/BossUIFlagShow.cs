using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLShipInfoBase), "ShouldShowBossUI")]
    internal class BossUIFlagShow
    {
        private static void Postfix(PLShipInfoBase __instance, ref bool __result)
        {
            __result |= (BossFlag.AllBossFlags.Count > 0 && BossFlag.AllBossFlags[0].ShipStats != null && BossFlag.AllBossFlags[0].ShipStats.Ship == __instance);
        }
    }

}

