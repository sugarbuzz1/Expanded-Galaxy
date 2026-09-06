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
    internal class BurrowSetupCall
    {
        private static bool Prefix(PLPersistantEncounterInstance __instance, int inHubID)
        {
            if (__instance is PLDesertHubEncounter)
            {
                PlanetObjectSetupManager.Instance.ClearViewIDs();
                SetupBurrow(__instance);
            }
            return true;
        }

        private static async void SetupBurrow(PLPersistantEncounterInstance pei)
        {
            while (PLNetworkManager.Instance.CurrentGame == null || !pei.GameInitWithHubID || PLEncounterManager.Instance.GetCPEI() != pei)
                await Task.Yield();
            Scene myScene = SceneManager.GetActiveScene();
            if (myScene == null)
                return;
            GameObject[] objects = myScene.GetRootGameObjects();
            List<Transform> objectsToSetup = new List<Transform>();
            objects[1].transform.GetChild(133).gameObject.SetActive(true);
            objects[1].transform.GetChild(71).position = new Vector3(82, -89, 10);

            GameObject door1GO = new GameObject();
            BlockedInteriorDoor door1PLID = door1GO.AddComponent<BlockedInteriorDoor>();
            door1PLID.BlockingScript = "ExGal_TheMap_Hidden_UnlockDoor";
            door1PLID.VisibleName = "Sanctum";
            door1PLID.IsEntrance = true;
            door1PLID.MyInterior = objects[1].transform.GetChild(114).gameObject.GetComponent<PLInterior>();
            door1GO.transform.SetParent(objects[1].transform.GetChild(114).GetChild(11).GetChild(1));
            door1GO.transform.position = new Vector3(201.3f, -158.2f, 8.8f);
            door1GO.transform.rotation = Quaternion.Euler(0, 90.5f, 0);

            GameObject door2GO = new GameObject();
            PLInteriorDoor door2PLID = door2GO.AddComponent<PLInteriorDoor>();
            door2PLID.VisibleName = "Structure";
            door2PLID.IsEntrance = true;
            door2PLID.MyInterior = objects[1].transform.GetChild(114).gameObject.GetComponent<PLInterior>();
            door2GO.transform.SetParent(objects[1].transform.GetChild(133).GetChild(0));
            door2GO.transform.position = new Vector3(11.5f, -130.5f, -22.5f);
            door2GO.transform.rotation = Quaternion.Euler(0, 121.105f, 0);

            door1PLID.TargetDoor = door2PLID;
            door2PLID.TargetDoor = door1PLID;

            List<Light> lightsToAdd = new List<Light>
                {
                    objects[1].transform.GetChild(133).GetChild(3).GetComponent<Light>(),
                    objects[1].transform.GetChild(133).GetChild(4).GetComponent<Light>(),
                    objects[1].transform.GetChild(133).GetChild(6).GetComponent<Light>()
                };
            while (objects[1].transform.GetChild(114).gameObject.GetComponent<PLInterior>().Lights.Length != 47)
                await Task.Yield();
            List<Light> tempLight = objects[1].transform.GetChild(114).gameObject.GetComponent<PLInterior>().Lights.ToList();
            tempLight.AddRange(lightsToAdd);
            objects[1].transform.GetChild(114).gameObject.GetComponent<PLInterior>().Lights = tempLight.ToArray();

            GameObject entityGO = new GameObject();
            entityGO.transform.SetParent(objects[1].transform.GetChild(133));
            entityGO.transform.localPosition = new Vector3(0, -85.4f, 0);
            PLDialogueActorInstance pLDialogueActorInstance = entityGO.AddComponent<PLDialogueActorInstance>();
            pLDialogueActorInstance.ActorName = "ExGal_MysteriousEntity";
            pLDialogueActorInstance.DisplayName = "Mysterious Entity";
            pLDialogueActorInstance.InteractionText = "Interact with";

            objectsToSetup.Add(objects[1].transform.GetChild(114).GetChild(11).GetChild(1).GetChild(0));

            if (objectsToSetup.Count > 0)
            {
                if (PhotonNetwork.isMasterClient)
                {
                    PlanetObjectSetupManager.Instance.SetupDPOWithViewIDs(95, objectsToSetup.ToArray());
                }
                else
                {
                    ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.ClientRequestViewIDs", PhotonTargets.MasterClient, new object[0]);
                    while (!PlanetObjectSetupManager.Instance.IsSetupWithIDs() || !(PLServer.Instance != null))
                        await Task.Yield();
                    PlanetObjectSetupManager.Instance.SetupDPOWithViewIDs(95, objectsToSetup.ToArray());
                }
            }
        }
    }
}
