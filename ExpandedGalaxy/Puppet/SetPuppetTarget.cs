using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLInGameUI), "SetShipAsTarget")]
    internal class SetPuppetTarget
    {
        private static void Postfix(PLSpaceTarget target)
        {
            if (!PhotonNetwork.isMasterClient)
                return;
            foreach (int shipID in Puppet.shipDatas.Keys)
            {
                PLShipInfoBase pLShipInfoBase = PLEncounterManager.Instance.GetShipFromID(shipID);
                if (pLShipInfoBase != null)
                    pLShipInfoBase.photonView.RPC("Captain_SetTargetShip", PhotonTargets.All, (object)target.SpaceTargetID);
            }
        }
    }
}
