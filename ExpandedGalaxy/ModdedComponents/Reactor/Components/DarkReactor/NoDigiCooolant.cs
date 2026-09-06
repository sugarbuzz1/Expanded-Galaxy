using HarmonyLib;
using PulsarModLoader.Content.Components.Reactor;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLWarpDriveProgram), "ExecuteBasedOnType")]
    internal class NoDigiCooolant
    {
        private static bool Prefix(PLWarpDriveProgram __instance)
        {
            if (__instance.ShipStats.GetShipComponent<PLReactor>(ESlotType.E_COMP_REACTOR) != null && __instance.ShipStats.GetShipComponent<PLReactor>(ESlotType.E_COMP_REACTOR).SubType == ReactorModManager.Instance.GetReactorIDFromName("Dark-Matter Reactor") && __instance.SubType == (int)EWarpDriveProgramType.DIG_COOLANT)
            {
                __instance.Level = 0;
                return false;
            }
            return true;
        }
    }
}
