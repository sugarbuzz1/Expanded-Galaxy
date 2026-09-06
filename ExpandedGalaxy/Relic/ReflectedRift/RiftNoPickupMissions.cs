using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLServer), "AttemptToAddPickupMission")]
    internal class RiftNoPickupMissions
    {
        private static bool Prefix()
        {
            return !ReflectedRift.inRift;
        }
    }
}
