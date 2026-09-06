using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLTurret), "Tick")]
    internal class PuppetTurretTarget
    {
        private static bool Prefix(PLTurret __instance)
        {
            if (__instance.ShipStats == null || __instance.ShipStats.Ship == null)
                return true;
            if (Puppet.shipDatas.ContainsKey(__instance.ShipStats.Ship.ShipID))
            {
                PLShipInfoBase masterShip = PLEncounterManager.Instance.GetShipFromID(Puppet.shipDatas[__instance.ShipStats.Ship.ShipID]);
                if (masterShip == null)
                    return true;
                if (__instance.SpaceTargetIDsTargeted.Contains(masterShip.SpaceTargetID))
                    __instance.SpaceTargetIDsTargeted.Remove(masterShip.SpaceTargetID);
            }
            return true;
        }
    }
}
