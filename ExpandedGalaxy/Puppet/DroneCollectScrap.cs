using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLSpaceScrap), "Update")]
    internal class DroneCollectScrap
    {
        private static void Postfix(PLSpaceScrap __instance)
        {
            if (__instance.Collected)
                return;
            foreach (PLShipInfoBase plShipInfoBase in PLEncounterManager.Instance.AllShips.Values)
            {
                if (plShipInfoBase != null && Puppet.shipDatas.ContainsKey(plShipInfoBase.ShipID) && plShipInfoBase.ExteriorTransformCached != null)
                {
                    float sqrMagnitude = (plShipInfoBase.ExteriorTransformCached.position - __instance.transform.position).sqrMagnitude;
                    if (PhotonNetwork.isMasterClient && (double)sqrMagnitude < 6400.0)
                        __instance.OnCollect();
                }
            }
        }
    }
}
