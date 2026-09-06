using CodeStage.AntiCheat.ObscuredTypes;
using PulsarModLoader;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace ExpandedGalaxy
{
    internal class PlanetCompPickups
    {
        internal static Dictionary<int, List<int>> PickupQueue = new Dictionary<int, List<int>>();

        public static async void AddCompToPlanet(PLPersistantEncounterInstance pei, int inHubID, int[] compHashes, bool replace = true)
        {
            await Task.Delay(100);
            while (PLNetworkManager.Instance.CurrentGame == null || !pei.GameInitWithHubID || PLEncounterManager.Instance.GetCPEI() != pei)
                await Task.Delay(100);
            await Task.Delay(100);
            if (PhotonNetwork.isMasterClient)
                ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.AddCompToPlanetRPC", PhotonTargets.Others, new object[4]
                {
                    pei.GetSectorID(),
                    inHubID,
                    compHashes,
                    replace
                });
            PLSectorInfo sector = PLServer.GetSectorWithID(inHubID);
            PLGamePlanet current = PLNetworkManager.Instance.CurrentGame as PLGamePlanet;
            Transform[] objects = current.PlanetRoot.GetComponentsInChildren<Transform>(true);
            List<Transform> cargoBaseTransforms = new List<Transform>();
            foreach (Transform obj in objects)
            {
                if (obj != null)
                {
                    if (obj.name.Contains("Cargo_Base_"))
                    {
                        if (obj.gameObject != null && obj.gameObject.activeSelf && !cargoBaseTransforms.Contains(obj) && !obj.gameObject.TryGetComponent<PLPickupComponent>(out PLPickupComponent component))
                        {
                            if (obj.parent != null)
                            {
                                Transform parent = obj.parent;
                                bool flag = false;
                                while (true)
                                {
                                    if (parent == null)
                                        break;
                                    if (parent.gameObject != null & !parent.gameObject.activeSelf)
                                    {
                                        flag = true;
                                        break;
                                    }
                                    parent = parent.parent;

                                }
                                if (flag)
                                    continue;
                            }
                            if (obj.gameObject.TryGetComponent<PLPickupRandomComponent>(out PLPickupRandomComponent component1))
                            {
                                if (replace)
                                {
                                    component1.PickedUp = true;
                                    component1.HasSetupVisualObject = true;
                                    if (PLGameStatic.Instance.m_AllPickupRandomComponents.Contains(component1))
                                        PLGameStatic.Instance.m_AllPickupRandomComponents.Remove(component1);
                                    if (pei.MyPersistantData.PickupRandomComponentPersistantData.ContainsKey((ObscuredInt)component1.PickupID))
                                        pei.MyPersistantData.PickupRandomComponentPersistantData.Remove((ObscuredInt)component1.PickupID);
                                    UnityEngine.Object.Destroy(component1);
                                    for (int i = 0; i < obj.childCount; i++)
                                    {
                                        UnityEngine.Object.Destroy(obj.GetChild(i).gameObject, 2f);
                                    }
                                    cargoBaseTransforms.Add(obj);
                                }
                            }
                            else
                            {
                                cargoBaseTransforms.Add(obj);
                            }
                        }
                    }
                }
            }
            await Task.Delay(100);
            if (!(cargoBaseTransforms.Count > 0))
            {
                PulsarModLoader.Utilities.Logger.Info(string.Format("Could not find open cargo base at sector {0}! ({1})", inHubID.ToString(), sector.VisualIndication.ToString()));
                return;
            }
            int minimumFreePickupID = 0;
            foreach (int key in pei.MyPersistantData.PickupRandomComponentPersistantData.Keys)
                if (key > minimumFreePickupID)
                    minimumFreePickupID = key;
            PLRand rand = new PLRand(inHubID + PLServer.Instance.GalaxySeed);
            int num = 0;
            while (cargoBaseTransforms.Count > 0 && compHashes.Length > num)
            {
                int num1 = rand.Next(0, cargoBaseTransforms.Count);
                if (compHashes[num] != -1)
                {

                    PLPickupRandomComponent pLPickupRandom = cargoBaseTransforms[num1].gameObject.AddComponent<PLPickupRandomComponent>();
                    pLPickupRandom.MyPEI = pei;
                    pLPickupRandom.PickedUp = false;
                    pLPickupRandom.RandComp = compHashes[num];
                    pLPickupRandom.RandCompSetup = true;
                    pLPickupRandom.m_InternalComp = PLShipComponent.CreateShipComponentFromHash(pLPickupRandom.RandComp);
                    PLShipComponent shipComponent = pLPickupRandom.m_InternalComp;
                    if (shipComponent != null && shipComponent.CargoVisualPrefabID > PLGlobal.Instance.CargoVisualPrefabs.Length)
                        shipComponent.CargoVisualPrefabID = 1;
                    pLPickupRandom.PickupID = ++minimumFreePickupID;
                    pei.MyPersistantData.PickupRandomComponentPersistantData.Add((ObscuredInt)pLPickupRandom.PickupID, (ObscuredBool)pLPickupRandom.PickedUp);
                    pLPickupRandom.MyInterior = pLPickupRandom.GetComponentInParent<PLInterior>();
                    PLGameStatic.Instance.m_AllPickupRandomComponents.Add(pLPickupRandom);
                }
                cargoBaseTransforms.Remove(cargoBaseTransforms[num1]);
                ++num;
            }
            if (compHashes.Length > num)
                PulsarModLoader.Utilities.Logger.Info(string.Format("Could not place {0} components at sector {1}! ({2})", (compHashes.Length - num).ToString(), inHubID.ToString(), sector.VisualIndication.ToString()));
        }
    }
}
