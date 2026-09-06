using HarmonyLib;
using PulsarModLoader;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLPersistantEncounterInstance), "InitGame")]
    internal class GreyPlainsSetupCall
    {
        private static bool Prefix(PLPersistantEncounterInstance __instance, int inHubID)
        {
            if (__instance is PLPlanetLevelEncounter && (int)__instance.LevelID == 85 && PLGlobal.Instance.Galaxy.AllSectorInfos.ContainsKey(inHubID) && PLGlobal.Instance.Galaxy.AllSectorInfos[inHubID] != null && PLGlobal.Instance.Galaxy.AllSectorInfos[inHubID].MySPI != null && PLGlobal.Instance.Galaxy.AllSectorInfos[inHubID].MySPI.Faction == 6)
            {
                PlanetObjectSetupManager.Instance.ClearViewIDs();
                SetupGreyPlainsSector(__instance);
                if (PhotonNetwork.isMasterClient && ReflectedRift.inRift)
                {
                    int[] compHash = new int[1]
                    {
                                (int)Relic.GenerateRelic((int)PLServer.Instance.GalaxySeed).getHash(),
                    };
                    PlanetCompPickups.PickupQueue.Add(inHubID, compHash.ToList());
                }
            }
            return true;
        }

        private static async void SetupGreyPlainsSector(PLPersistantEncounterInstance pei)
        {
            while (PLNetworkManager.Instance.CurrentGame == null || !pei.GameInitWithHubID || PLEncounterManager.Instance.GetCPEI() != pei)
                await Task.Yield();
            Scene myScene = SceneManager.GetActiveScene();
            if (myScene == null)
                return;
            GameObject[] objects = myScene.GetRootGameObjects();
            List<Transform> objectsToSetup = new List<Transform>();
            for (int i = 0; i < objects[2].transform.childCount; i++)
            {
                Transform transform = objects[2].transform.GetChild(i);
                switch (transform.name)
                {
                    case "Lounge_Bed2-2":
                        transform.gameObject.SetActive(false);
                        break;
                    case "DehydratedSandwichPickup":
                        GameObject.Destroy(transform.gameObject);
                        break;
                    case "SmallStation_01":
                        transform.GetChild(13).GetChild(5).GetChild(0).gameObject.SetActive(false);
                        transform.GetChild(13).GetChild(5).GetChild(1).gameObject.SetActive(true);

                        GameObject.Destroy(transform.GetChild(13).GetChild(8).gameObject);
                        GameObject.Destroy(transform.GetChild(13).GetChild(9).gameObject);
                        GameObject.Destroy(transform.GetChild(13).GetChild(10).gameObject);
                        GameObject.Destroy(transform.GetChild(13).GetChild(12).gameObject);
                        break;
                }

                if (transform.name.Contains("CU_Weeler"))
                {
                    objectsToSetup.Add(transform);
                }
            }
            if (objectsToSetup.Count > 0)
            {
                if (PhotonNetwork.isMasterClient)
                {
                    PlanetObjectSetupManager.Instance.SetupDPOWithViewIDs(85, objectsToSetup.ToArray());
                }
                else
                {
                    ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.ClientRequestViewIDs", PhotonTargets.MasterClient, new object[0]);
                    while (!PlanetObjectSetupManager.Instance.IsSetupWithIDs())
                        await Task.Yield();
                    PlanetObjectSetupManager.Instance.SetupDPOWithViewIDs(85, objectsToSetup.ToArray());
                }
            }
        }
    }
}
