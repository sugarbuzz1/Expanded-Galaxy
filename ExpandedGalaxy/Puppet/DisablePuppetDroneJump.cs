using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLShipInfoBase), "Ship_WarpOutNow")]
    internal class DisablePuppetDroneJump
    {
        private static bool Prefix(PLShipInfoBase __instance)
        {
            if (Puppet.shipDatas.ContainsKey(__instance.ShipID))
                return false;
            return true;
        }
    }
}
