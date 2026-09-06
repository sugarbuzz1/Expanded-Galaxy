using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLShipInfoBase), "Captain_SetTargetShip")]
    internal class CaptainSetPuppetTarget
    {
        private static void Postfix(PLShipInfoBase __instance, int inShipID)
        {
            foreach (int shipID in Puppet.shipDatas.Keys)
            {
                if (Puppet.shipDatas[shipID] == __instance.ShipID)
                {
                    PLShipInfoBase pLShipInfoBase = PLEncounterManager.Instance.GetShipFromID(shipID);
                    if (pLShipInfoBase != null && PhotonNetwork.isMasterClient)
                        pLShipInfoBase.photonView.RPC("Captain_SetTargetShip", PhotonTargets.All, (object)inShipID);
                }
            }
        }
    }
}
