
using CodeStage.AntiCheat.ObscuredTypes;
using HarmonyLib;
using PulsarModLoader;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExpandedGalaxy
{
    internal class PlanetObjectSetupManager
    {
        private static PlanetObjectSetupManager instance;
        private List<int> allocatedViewIDs;
        private bool isSetupWithIDs;

        public PlanetObjectSetupManager()
        {
            allocatedViewIDs = new List<int>();
            isSetupWithIDs = false;
        }

        public static PlanetObjectSetupManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new PlanetObjectSetupManager();
                return instance;
            }
        }

        public void OnNewGame()
        {
            allocatedViewIDs.Clear();
        }

        public void ClearViewIDs()
        {
            allocatedViewIDs.Clear();
            isSetupWithIDs = false;
        }

        public bool IsSetupWithIDs()
        {
            return isSetupWithIDs;
        }

        public int[] GetViewIDs()
        {
            return allocatedViewIDs.ToArray();
        }

        public void RemoveID(int viewID)
        {
            int num = allocatedViewIDs.IndexOf(viewID);
            if (num != -1)
            {
                allocatedViewIDs[num] = -1;
            }
        }

        public void SetIDs(int[] iDs)
        {
            allocatedViewIDs.AddRange(iDs);
            isSetupWithIDs = true;
        }

        public void SetupDPOWithViewIDs(int levelId, Transform[] gameObjects)
        {
            int num = 0;
            if (levelId == 85)
            {
                foreach (Transform transform in gameObjects)
                {
                    transform.gameObject.AddComponent<PLDamageablePlanetObject>();
                    PLDamageablePlanetObject pLDamageablePlanetObject = transform.GetComponent<PLDamageablePlanetObject>();
                    pLDamageablePlanetObject.ShowDamageVisualEffectOnHit = false;
                    pLDamageablePlanetObject.MaxHealth = (ObscuredFloat)40f;
                    pLDamageablePlanetObject.Health = (ObscuredFloat)40f;
                    pLDamageablePlanetObject.Destroyable = true;
                    pLDamageablePlanetObject.MyCollisionSpheres = new PLPawnCollisionSphere[0];
                    transform.GetComponentInChildren<MeshCollider>().gameObject.AddComponent<PLDamageablePlanetObject_Collider>();
                    pLDamageablePlanetObject.ScriptName = "ExGal_ReflectedRift_Finsh";
                    PhotonView view = transform.gameObject.AddComponent<PhotonView>();
                    if (PhotonNetwork.isMasterClient)
                    {
                        view.viewID = PhotonNetwork.AllocateViewID();
                        allocatedViewIDs.Add(view.viewID);
                    }
                    else
                    {
                        if (allocatedViewIDs[num] == -1)
                        {
                            GameObject.Destroy(transform.gameObject);
                            continue;
                        }
                        view.viewID = allocatedViewIDs[num];
                        ++num;
                    }
                    view.ObservedComponents = new List<Component>
                    {
                        pLDamageablePlanetObject
                    };
                }
                isSetupWithIDs = true;
            }
            else if (levelId == 95)
            {
                Debug.Log("1");
                if (PLServer.Instance.HasActiveMissionWithID(8000018) || PLMissionObjective.IDIsCompleted("ExGal_TheMap_Hidden_UnlockDoor"))
                {
                    Debug.Log("2");
                    foreach (Transform transform in gameObjects)
                    {
                        Debug.Log("3");
                        transform.gameObject.AddComponent<PLDamageablePlanetObject>();
                        PLDamageablePlanetObject pLDamageablePlanetObject = transform.GetComponent<PLDamageablePlanetObject>();
                        pLDamageablePlanetObject.ShowDamageVisualEffectOnHit = false;
                        pLDamageablePlanetObject.MaxHealth = (ObscuredFloat)10f;
                        pLDamageablePlanetObject.Health = (ObscuredFloat)10f;
                        pLDamageablePlanetObject.Destroyable = true;
                        pLDamageablePlanetObject.MyCollisionSpheres = new PLPawnCollisionSphere[0];
                        pLDamageablePlanetObject.UniqueNameForSaving = "ExGal_BurrowDoorBlocker";
                        if (PhotonNetwork.isMasterClient)
                        {
                            Debug.Log("4");
                            PLDPOPersistantData data = PLEncounterManager.Instance.GetCPEI().MyPersistantData.GetDPOPersistantData("ExGal_BurrowDoorBlocker");
                            if (data == null)
                            {
                                data = new PLDPOPersistantData();
                                data.DestroyedMsgSent = (ObscuredBool)false;
                                data.RepairedMsgSent = (ObscuredBool)false;
                                data.Health = pLDamageablePlanetObject.Health;
                                PLEncounterManager.Instance.GetCPEI().MyPersistantData.DPOPersistantData.Add((ObscuredString)"ExGal_BurrowDoorBlocker", data);
                            }
                            pLDamageablePlanetObject.SetPersistantData(data);
                        }
                        PLDamageablePlanetObject_Collider collider = transform.GetComponentInParent<MeshCollider>().gameObject.AddComponent<PLDamageablePlanetObject_Collider>();
                        collider.MyDPO = pLDamageablePlanetObject;
                        pLDamageablePlanetObject.ScriptName = "ExGal_TheMap_Hidden_UnlockDoor";
                        PhotonView view = transform.gameObject.AddComponent<PhotonView>();
                        Debug.Log("5");
                        if (PhotonNetwork.isMasterClient)
                        {
                            Debug.Log("6");
                            view.viewID = PhotonNetwork.AllocateViewID();
                            allocatedViewIDs.Add(view.viewID);
                        }
                        else
                        {
                            Debug.Log("7");
                            if (allocatedViewIDs[num] == -1)
                            {
                                Debug.Log("8");
                                GameObject.Destroy(transform.gameObject);
                                continue;
                            }
                            view.viewID = allocatedViewIDs[num];
                            ++num;
                        }
                        Debug.Log("9");
                        view.ObservedComponents = new List<Component>
                    {
                        pLDamageablePlanetObject
                    };
                    }
                }
                isSetupWithIDs = true;
            }
            if (PhotonNetwork.isMasterClient)
            {
                List<object> obj = new List<object>();
                foreach (int num1 in allocatedViewIDs)
                    obj.Add((object)num1);
                ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.ServerSendViewIDs", PhotonTargets.Others, obj.ToArray());
            }
        }
    }
}
