using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLPickupRandomComponent), "SetupRandComp")]
    internal class SetupRandCompFix
    {
        private static bool Prefix(PLPickupRandomComponent __instance)
        {
            return !__instance.RandCompSetup;
        }
    }
}
