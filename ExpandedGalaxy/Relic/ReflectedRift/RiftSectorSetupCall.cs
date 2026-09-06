using HarmonyLib;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLPersistantEncounterInstance), "InitGame")]
    internal class RiftSectorSetupCall
    {
        private static bool Prefix(PLPersistantEncounterInstance __instance, int inHubID)
        {
            if (__instance is PLPlanetLevelEncounter && (int)__instance.LevelID == 80)
            {
                PLSectorInfo sector = PLServer.GetSectorWithID(inHubID);
                if (sector != null && sector.MySPI.Faction == 6)
                {
                    SetupRiftSector(__instance);
                }
            }
            return true;
        }

        private static async void SetupRiftSector(PLPersistantEncounterInstance pei)
        {
            while (PLNetworkManager.Instance.CurrentGame == null || !pei.GameInitWithHubID || PLEncounterManager.Instance.GetCPEI() != pei)
                await Task.Yield();
            Scene myScene = SceneManager.GetActiveScene();
            if (myScene == null)
                return;
            GameObject[] objects = myScene.GetRootGameObjects();
            for (int i = 0; i < objects[0].transform.childCount; i++)
            {
                Transform transform = objects[0].transform.GetChild(i);
                if (transform.name.Contains("ProbePickup"))
                    transform.gameObject.GetComponent<PLProbePickup>().PickedUp = true;
                else if (transform.name != "Particle System" && transform.name != "Rift_Animation_01" && transform.name != "Rift_Animation_02" && transform.name != "InteriorLight" && transform.name != "ExteriorLight")
                    transform.gameObject.SetActive(false);
            }
            for (int i = 0; i < PLTeleportationLocationInstance.GetAllTLIs().Count; i++)
            {
                if (PLTeleportationLocationInstance.GetAllTLIs()[i] != null && PLTeleportationLocationInstance.GetAllTLIs()[i].transform.name == "PLGamePlanet")
                {
                    UnityEngine.Object.Destroy(PLTeleportationLocationInstance.GetAllTLIs()[i]);
                    break;
                }
            }
        }
    }
}
