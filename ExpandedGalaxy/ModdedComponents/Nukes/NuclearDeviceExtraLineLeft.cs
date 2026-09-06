using HarmonyLib;
using PulsarModLoader.Content.Components.NuclearDevice;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLNuclearDevice), "GetExtraLineLeft")]
    internal class NuclearDeviceExtraLineLeft
    {
        private static bool Prefix(PLNuclearDevice __instance, ref string __result)
        {
            if (__instance.SubType < NuclearDeviceModManager.Instance.VanillaNuclearDeviceMaxType && __instance.SubType != 5 && __instance.SubType != 3 && !(__instance is PLBiscuitBombComponent))
            {
                __result = "-\n" + "Dmg Boost\n" + PLLocalize.Localize("Intimidation");
                return false;
            }
            return true;
        }
    }
}
