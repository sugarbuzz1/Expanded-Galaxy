using HarmonyLib;
using PulsarModLoader.Content.Components.NuclearDevice;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLNuclearDevice), "GetExtraLineRight")]
    internal class NuclearDeviceExtraLineRight
    {
        private static bool Prefix(PLNuclearDevice __instance, ref string __result)
        {
            if (__instance.SubType < NuclearDeviceModManager.Instance.VanillaNuclearDeviceMaxType && __instance.SubType != 5 && __instance.SubType != 3 && !(__instance is PLBiscuitBombComponent))
            {
                int num = 25;
                switch (__instance.SubType)
                {
                    case 0:
                        num = 35;
                        break;
                    case 1:
                        num = 40;
                        break;
                    case 2:
                        num = 30;
                        break;
                    case 4:
                        num = 45;
                        break;
                }
                __result = "Level " + (__instance.Level + 1).ToString() + "\n" + num.ToString("0") + "%" + "\n+" + __instance.IntimidationBonus.ToString("0") + "%";
                return false;
            }
            return true;
        }
    }
}
