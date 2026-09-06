using HarmonyLib;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLPersistantEncounterInstance), "InitGame")]
    internal class MiningHubSetupCall
    {
        private static async void SetupMiningHubPlanet(PLPersistantEncounterInstance pei, bool visited)
        {
            await Task.Delay(100);
            while (PLNetworkManager.Instance.CurrentGame == null || !pei.GameInitWithHubID || PLEncounterManager.Instance.GetCPEI() != pei)
                await Task.Yield();
            PLGamePlanet current = PLNetworkManager.Instance.CurrentGame as PLGamePlanet;
            if (current == null)
            {
                PulsarModLoader.Utilities.Logger.Info(string.Format("Could not find planet at sector {0}! ({1})", PLServer.GetCurrentSector().ID.ToString(), PLServer.GetCurrentSector().VisualIndication.ToString()));
                return;
            }
            Transform[] objects = current.PlanetRoot.GetComponentsInChildren<Transform>(true);
            foreach (Transform obj in objects)
            {
                if (obj != null)
                {
                    if (obj.gameObject != null)
                    {
                        switch (obj.name)
                        {
                            case "LargeDebris_01":
                            case "LargeDebris_01_Deco":
                                obj.gameObject.SetActive(true);
                                break;
                            case "Factory_Structure_01":
                                obj.gameObject.SetActive(false);
                                break;
                            case "LogScreen1":
                                obj.transform.localPosition = new Vector3(18.8f, -21.4f, -24f);
                                obj.transform.rotation = Quaternion.Euler(0f, 45f, 0f);
                                obj.gameObject.SetActive(true);
                                break;
                        }
                        if (!visited)
                        {
                            if (obj.name.Contains("HG_77328") || obj.name.Contains("CreepingLung_01"))
                            {
                                if (obj.gameObject.TryGetComponent<PLPickupObject>(out PLPickupObject component))
                                    component.PickedUp = false;
                            }
                        }
                    }
                }
            }
            GameObject volumeObj = new GameObject();
            volumeObj.transform.position = new Vector3(132.5f, -188f, -29f);
            volumeObj.transform.SetParent(current.PlanetRoot.transform);
            PLObjectiveVolume volume = volumeObj.AddComponent<PLObjectiveVolume>();
            volume.Dimensions = new Vector3(3f, 1f, 4f);
            volume.VolumeName = "ExGal_MiningDrone_Volume";
        }
        private static bool Prefix(PLPersistantEncounterInstance __instance, int inHubID)
        {
            if (__instance is PLLavaPlanet2Encounter)
            {
                PLSectorInfo sector = PLServer.GetSectorWithID(inHubID);
                if (sector == null) return true;
                SetupMiningHubPlanet(__instance, sector.Visited);
                if (!sector.Visited && PhotonNetwork.isMasterClient)
                {
                    int[] hashes = new int[4]
                    {
                            (int)new PLScrapCargo(9).getHash(),
                            (int)new PLScrapCargo(9).getHash(),
                            (int)new PLScrapCargo(9).getHash(),
                            (int)Relic.GenerateRelic(PLServer.Instance.GalaxySeed).getHash(),
                    };
                    PlanetCompPickups.PickupQueue.Add(inHubID, hashes.ToList());
                }
            }
            return true;
        }
    }
}
