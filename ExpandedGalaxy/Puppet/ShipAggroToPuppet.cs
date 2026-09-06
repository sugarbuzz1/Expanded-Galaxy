using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLShipInfoBase), "AddHostileShip")]
    internal class ShipAggroToPuppet
    {
        private static bool Prefix(PLShipInfoBase __instance, PLShipInfoBase inShip)
        {
            if (Puppet.shipDatas.ContainsKey(__instance.ShipID))
            {
                if (Puppet.shipDatas[__instance.ShipID] == inShip.ShipID)
                    return false;
            }
            else if (Puppet.shipDatas.ContainsKey(inShip.ShipID))
            {
                if (Puppet.shipDatas[inShip.ShipID] == __instance.ShipID)
                    return false;
            }
            return true;
        }
    }
}
