using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLVirus), "FinalLateAddStats")]
    internal class VirusEffect
    {
        private static bool Prefix(PLVirus __instance, ref PLShipStats inStats)
        {
            if (__instance.SubType == (int)EVirusType.RAND_LARGE)
            {
                switch ((int)__instance.SubTypeData)
                {
                    case 0:
                        inStats.TurretDamageFactor *= 0.7f;
                        inStats.ShieldsChargeRate = 0.0f;
                        break;
                    case 1:
                        inStats.HullArmor *= 0.5f;
                        inStats.OxygenRefillRate = 0.0f;
                        break;
                    case 2:
                        inStats.CyberDefenseRating = 0.0f;
                        inStats.TurretChargeFactor *= 0.7f;
                        break;
                    case 3:
                        inStats.EMDetection = 0.0f;
                        inStats.WarpChargeRate = 0.0f;
                        break;
                }
            }
            return true;
        }
    }
}

