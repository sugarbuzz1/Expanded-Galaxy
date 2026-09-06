using HarmonyLib;
using UnityEngine;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLPersistantPlanetEncounterInstance), "Update")]
    internal class SendToRift
    {
        private static void Postfix(PLPersistantPlanetEncounterInstance __instance)
        {
            if (!ReflectedRift.inRift && PhotonNetwork.isMasterClient && __instance is PLPlanetLevelEncounter && (int)__instance.LevelID == 80 && PLEncounterManager.Instance != null && PLEncounterManager.Instance.PlayerShip != null && !PLEncounterManager.Instance.PlayerShip.InWarp)
            {
                if (PLServer.GetCurrentSector() != null && PLServer.GetCurrentSector().MySPI.Faction == 6)
                {
                    float f = Vector3.SqrMagnitude(PLEncounterManager.Instance.PlayerShip.Exterior.transform.position - new Vector3(347.6f, 136.5f, -62.5f));
                    if ((double)f < 1225.0)
                    {
                        PLSectorInfo sectorInfo = PLGlobal.Instance.Galaxy.GetSectorOfVisualIndication((ESectorVisualIndication)145);
                        if (sectorInfo != null)
                        {

                            PLServer.Instance.photonView.RPC("NetworkBeginWarp", PhotonTargets.All, (object)PLEncounterManager.Instance.PlayerShip.ShipID, (object)sectorInfo.ID, (object)PLServer.Instance.GetEstimatedServerMs(), (object)-1);
                            PLMissionObjective_Custom.OnCustomObjEvent("ExGal_ReflectedRift_Enter");
                            ReflectedRift.SetRiftData(0, true);
                            PLServer.Instance.IsReflection = !PLServer.Instance.IsReflection;
                        }
                    }
                }
            }
        }
    }
}
