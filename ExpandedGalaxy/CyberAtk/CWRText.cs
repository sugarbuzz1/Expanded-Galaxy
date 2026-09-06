using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLCPU), "GetStatLineRight")]
    internal class CWRText
    {
        private static void Postfix(PLCPU __instance, ref string __result)
        {
            if (__instance.CPUClass == ECPUClass.CYBERWARFARE_MODULE)
            {
                float value = 0.225f * __instance.LevelMultiplier(0.75f);
                __result = __instance.ShipStats == null || (bool)__instance.ShipStats.isPreview || __instance.InCargoSlot() ? value.ToString() : (value * __instance.GetPowerPercentInput()).ToString("0.0") + "/" + value.ToString("0.0") + "\n";
            }
        }
    }
}
