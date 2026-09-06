using PulsarModLoader;
using UnityEngine;

namespace ExpandedGalaxy
{
    internal class EMPPulse : ModMessage
    {
        public override void HandleRPC(object[] arguments, PhotonMessageInfo sender)
        {
            int inID = (int)arguments[0];
            float range = (float)arguments[1];
            PLShipStats stats = PLEncounterManager.Instance.GetShipFromID(inID).MyStats;
            PLShipInfo shipFromId = PLEncounterManager.Instance.GetShipFromID(inID) as PLShipInfo;
            UnityEngine.Object.Instantiate<GameObject>(PLGlobal.Instance.EMPExplosionPrefab, shipFromId.Exterior.transform.position, Quaternion.identity);
            if (PhotonNetwork.isMasterClient)
            {
                foreach (PLShipInfoBase plShipInfoBase in UnityEngine.Object.FindObjectsOfType(typeof(PLShipInfoBase)))
                {
                    if ((UnityEngine.Object)plShipInfoBase != (UnityEngine.Object)shipFromId && (plShipInfoBase.GetCurrentSensorPosition() - shipFromId.GetCurrentSensorPosition()).magnitude < (range / 5f))
                        plShipInfoBase.photonView.RPC("Overcharged", PhotonTargets.All);
                }
            }
        }
    }
}

