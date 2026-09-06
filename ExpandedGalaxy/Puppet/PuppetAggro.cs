using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLShipInfoBase), "Update")]
    internal class PuppetAggro
    {
        private static bool Prefix(PLShipInfoBase __instance)
        {
            if (__instance == null)
                return true;
            if (PLEncounterManager.Instance == null)
                return true;
            if (Puppet.shipDatas.ContainsKey(__instance.ShipID))
            {
                PLShipInfoBase masterShip = PLEncounterManager.Instance.GetShipFromID(Puppet.shipDatas[__instance.ShipID]);
                if (masterShip == null)
                    return true;
                if (__instance.HostileShips != null && __instance.HostileShips.Contains(masterShip.ShipID))
                    __instance.HostileShips.Remove(masterShip.ShipID);
                if (masterShip.TargetShip != null && PhotonNetwork.isMasterClient)
                    __instance.photonView.RPC("Captain_SetTargetShip", PhotonTargets.All, (object)masterShip.TargetShip.ShipID);
            }
            return true;
        }

        private static void Postfix(PLShipInfoBase __instance)
        {
            if (__instance == null)
                return;
            if (PLEncounterManager.Instance == null)
                return;
            if (Puppet.shipDatas.ContainsKey(__instance.ShipID))
            {
                PLShipInfoBase masterShip = PLEncounterManager.Instance.GetShipFromID(Puppet.shipDatas[__instance.ShipID]);
                if (masterShip == null)
                    return;
                if (__instance.HostileShips != null && __instance.HostileShips.Contains(masterShip.ShipID))
                    __instance.HostileShips.Remove(masterShip.ShipID);
                if (masterShip.TargetShip != null && PhotonNetwork.isMasterClient)
                    __instance.photonView.RPC("Captain_SetTargetShip", PhotonTargets.All, (object)masterShip.TargetShip.ShipID);
            }
        }
    }
}
