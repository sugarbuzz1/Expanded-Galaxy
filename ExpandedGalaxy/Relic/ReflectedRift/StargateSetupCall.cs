using HarmonyLib;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLPersistantEncounterInstance), "InitGame")]
    internal class StargateSetupCall
    {
        private static bool Prefix(PLPersistantEncounterInstance __instance, int inHubID)
        {
            if (__instance is PLWarpStationEncounter)
            {
                PLSectorInfo sector = PLServer.GetSectorWithID(inHubID);
                if (sector != null & sector.MySPI.Faction == 6)
                {
                    SetupStargateSector(__instance);
                }
            }
            return true;
        }

        private static async void SetupStargateSector(PLPersistantEncounterInstance pei)
        {
            while (PLNetworkManager.Instance.CurrentGame == null || !pei.GameInitWithHubID || PLEncounterManager.Instance.GetCPEI() != pei)
                await Task.Yield();
            Scene myScene = SceneManager.GetActiveScene();
            if (myScene == null)
                return;
            GameObject[] objects = myScene.GetRootGameObjects();
            for (int i = 0; i < objects[5].transform.GetChild(0).childCount; i++)
            {
                Transform transform = objects[5].transform.GetChild(0).GetChild(i);
                switch (transform.name)
                {
                    case "Flag_small":
                    case "MetalChair_01":
                    case "DecorativeCarpet_01":
                    case "CargoCrate_02-2":
                    case "CargoCrate_02-2 (1)":
                    case "CargoCrate_01-2":
                    case "CargoCrate_01-2 (1)":
                    case "CargoCrate_01-2 (2)":
                    case "Toilet_01":
                    case "Kictchen_Attachment_02":
                    case "Shower_02_Controls":
                    case "Shower_02_Top":
                    case "Shower_02_Base":
                    case "Bed_01":
                        transform.gameObject.SetActive(false);
                        break;
                }
                if (transform.name.Contains("Carrier_Exterior_Light_01"))
                {
                    for (int j = 0; j < transform.childCount; j++)
                    {
                        transform.GetChild(j).gameObject.SetActive(false);
                    }
                }
            }
            int num = 0;
            int num1 = 0;
            bool flag = false;
            while (!flag)
            {
                for (num = 0; num < PLScreenHubBase.GetAllScreenHubs().Count; num++)
                {
                    if (PLScreenHubBase.GetAllScreenHubs()[num].name == "WarpStation_Exterior" && PLScreenHubBase.GetAllScreenHubs()[num].AllScreens.Count > 0)
                    {
                        for (num1 = 0; num1 < PLScreenHubBase.GetAllScreenHubs()[num].AllScreens.Count; num1++)
                        {
                            if (PLScreenHubBase.GetAllScreenHubs()[num].AllScreens[num1] is PLWarpStationScreen)
                            {
                                flag = true;
                                break;
                            }
                        }
                    }
                    if (flag)
                        break;
                }
                await Task.Yield();
            }
            GameObject.Destroy(PLScreenHubBase.GetAllScreenHubs()[num].gameObject.GetComponent<PLHailTarget_WarpStation>());
            SetupStargateScreen.Setup((PLWarpStationScreen)PLScreenHubBase.GetAllScreenHubs()[num].AllScreens[0]);
        }
    }
}
