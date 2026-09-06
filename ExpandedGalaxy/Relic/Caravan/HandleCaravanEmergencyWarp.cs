using HarmonyLib;
using System.Linq;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLShipInfoBase), "Ship_WarpOutNow")]
    internal class HandleCaravanEmergencyWarp
    {
        private static bool Prefix(PLShipInfoBase __instance)
        {
            if (__instance.PersistantShipInfo == null || __instance.PersistantShipInfo.SelectedActorID != "ExGal_RelicCaravan")
                return true;
            if (PLMissionObjective.IDIsCompleted("ExGal_TheMap_Hidden_TalkCaravan"))
                return false;
            PLSectorInfo currentSector = PLServer.GetCurrentSector();
            if (currentSector.Neighbors.Count > 0)
            {
                PLServer.Instance.photonView.RPC("WarpOutEffect", PhotonTargets.All, (object)__instance.Exterior.transform.position, (object)__instance.Exterior.transform.rotation);
                RelicCaravan.CaravanCurrentSector = currentSector.Neighbors[UnityEngine.Random.Range(0, currentSector.Neighbors.Count)].ID;
                RelicCaravan.CaravanPath.Clear();
                RelicCaravan.CaravanPathIndex = 0;
                RelicCaravan.CaravanUpdateTime = PLServer.Instance.GetEstimatedServerMs() + 60000;
            }
            else
            {
                if (RelicCaravan.CaravanPath.Count > RelicCaravan.CaravanPathIndex + 1)
                {
                    PLServer.Instance.photonView.RPC("WarpOutEffect", PhotonTargets.All, (object)__instance.Exterior.transform.position, (object)__instance.Exterior.transform.rotation);
                    RelicCaravan.CaravanCurrentSector = RelicCaravan.CaravanPath[RelicCaravan.CaravanPathIndex + 1].ID;
                    RelicCaravan.CaravanPathIndex++;
                    RelicCaravan.CaravanUpdateTime = PLServer.Instance.GetEstimatedServerMs() + 60000;
                }
                else
                    return false;
            }
            PhotonNetwork.Destroy(__instance.ShipRoot);
            return false;
        }
    }
}
