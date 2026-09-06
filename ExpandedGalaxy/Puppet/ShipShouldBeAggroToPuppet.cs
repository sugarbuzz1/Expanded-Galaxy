using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLShipInfoBase), "ShouldBeHostileToShip")]
    internal class ShipShouldBeAggroToPuppet
    {
        private static void Postfix(PLShipInfoBase __instance, PLShipInfoBase inShip, ref bool __result)
        {
            if (Puppet.shipDatas.ContainsKey(__instance.ShipID))
            {
                if (Puppet.shipDatas[__instance.ShipID] == inShip.ShipID)
                    __result = false;
            }
            else if (Puppet.shipDatas.ContainsKey(inShip.ShipID))
            {
                if (Puppet.shipDatas[inShip.ShipID] == __instance.ShipID)
                    __result = false;
            }
        }
    }
}
