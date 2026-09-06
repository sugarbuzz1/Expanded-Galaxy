using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLMegaTurret), "Tick")]
    internal class TickPowerUsage
    {
        private static void Postfix(PLMegaTurret __instance)
        {
            switch (__instance.SubType)
            {
                case 0: //MAIN TURRET
                    __instance.CalculatedMaxPowerUsage_Watts = 6800f * __instance.LevelMultiplier(0.2f);
                    break;
                case 1: //CU LONG
                case 5: //MODIFIED CU LONG
                    __instance.CalculatedMaxPowerUsage_Watts = 11500f * __instance.LevelMultiplier(0.2f);
                    break;
                case 3: //RAPIDFIRE
                    __instance.CalculatedMaxPowerUsage_Watts = 11600f * __instance.LevelMultiplier(0.2f);
                    break;
                case 4: //FLASHFIRE
                    __instance.CalculatedMaxPowerUsage_Watts = 11600f * __instance.LevelMultiplier(0.1f);
                    break;
            }
        }
    }
}
