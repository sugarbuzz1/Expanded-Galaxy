using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLShipInfoBase), "VirusAttemptSuccessful")]
    internal class NewVirusCalc
    {
        private static void Postfix(PLShipInfoBase __instance, PLVirus inVirus, ref bool __result)
        {
            if (__instance.MyStats != null)
            {
                if (!inVirus.AttemptCounterMap.ContainsKey(__instance.ShipID) || inVirus.AttemptCounterMap[__instance.ShipID] < 3)
                {
                    __result = false;
                    return;
                }
                if ((double)1.0 + inVirus.Sender.MyStats.CyberAttackRating + CyberAtk.GetVirusCyberAtkModifier(inVirus.SubType) > (double)__instance.MyStats.CyberDefenseRating)
                {
                    __result = true;
                    return;
                }
            }
            __result = false;
        }
    }
}
