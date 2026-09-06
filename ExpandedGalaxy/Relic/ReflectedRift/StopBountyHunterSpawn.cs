using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLServer), "UpdateBountyHunter")]
    internal class StopBountyHunterSpawn
    {
        private static bool Prefix(PLServer __instance)
        {
            return !ReflectedRift.inRift;
        }
    }
}
