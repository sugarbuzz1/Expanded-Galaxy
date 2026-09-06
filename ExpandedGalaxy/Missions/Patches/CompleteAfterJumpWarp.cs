using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLMissionObjective_CompleteWithinJumpCount), "OnShipWarp")]
    internal class CompleteAfterJumpWarp
    {
        private static void Postfix()
        {
            PLMissionObjective_CompleteAfterJumpCount.OnShipWarp();
        }
    }

}

