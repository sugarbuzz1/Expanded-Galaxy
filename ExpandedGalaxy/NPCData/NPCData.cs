using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExpandedGalaxy
{
    internal class NPCData
    {
        public static GameObject HandbookPrefab = null;
        internal static List<PLDialogueActorInstance> InsideShipDAIs = new List<PLDialogueActorInstance>();

        private static IEnumerator PlaceGuidebookRoutine(PLServer instance)
        {
            List<int> ProcessedShipIDs = new List<int>();
            while (instance != null)
            {
                if (PLEncounterManager.Instance != null && PLEncounterManager.Instance.PlayerShip != null)
                {
                    PLShipInfo pLShipInfo = PLEncounterManager.Instance.PlayerShip;
                    if (!ProcessedShipIDs.Contains(pLShipInfo.ShipID))
                    {
                        ProcessedShipIDs.Add(pLShipInfo.ShipID);
                        GameObject GuidebookGO = GameObject.Instantiate(HandbookPrefab);
                        UnityEngine.Object.DontDestroyOnLoad(GuidebookGO);
                        GuidebookGO.layer = 11;
                        GuidebookGO.name = "Guidebook_ExGal";
                        PLDialogueActorInstance pLDialogueActorInstance = GuidebookGO.AddComponent<PLDialogueActorInstance>();
                        pLDialogueActorInstance.ActorName = "ExGal_Guidebook";
                        pLDialogueActorInstance.DisplayName = "ExpandedGalaxy Handbook";
                        pLDialogueActorInstance.WillBuyBiscuits = false;
                        pLDialogueActorInstance.DisplayTextHeight = 0f;
                        pLDialogueActorInstance.MaxRange = 1.5f;
                        pLDialogueActorInstance.DialogueCameraShouldBeUsed = false;
                        pLDialogueActorInstance.InteractionText = "Open";
                        pLDialogueActorInstance.TLIInParent = pLShipInfo.MyTLI;
                        bool flag = false;
                        switch (pLShipInfo.ShipTypeID)
                        {
                            case EShipType.E_INTREPID:
                                GuidebookGO.transform.SetParent(pLShipInfo.InteriorStatic.transform.GetChild(8));
                                GuidebookGO.transform.localPosition = new Vector3(12.3f, 5.275f, 5.4f);
                                GuidebookGO.transform.rotation = Quaternion.Euler(0, 180, 180);
                                break;
                            case EShipType.E_WDCRUISER:
                                GuidebookGO.transform.SetParent(pLShipInfo.InteriorStatic.transform.GetChild(12));
                                GuidebookGO.transform.localPosition = new Vector3(6.7f, -1.342f, -5.1f);
                                GuidebookGO.transform.rotation = Quaternion.Euler(0, 250, 180);
                                break;
                            case EShipType.E_CARRIER:
                                GuidebookGO.transform.SetParent(pLShipInfo.InteriorStatic.transform.GetChild(4));
                                GuidebookGO.transform.localPosition = new Vector3(-6.7f, -0.797f, -9.2f);
                                GuidebookGO.transform.rotation = Quaternion.Euler(0, 280, 180);
                                break;
                            case EShipType.E_STARGAZER:
                                GuidebookGO.transform.SetParent(pLShipInfo.InteriorStatic.transform.GetChild(2));
                                GuidebookGO.transform.localPosition = new Vector3(-16.6f, -4.17f, -21.6f);
                                GuidebookGO.transform.rotation = Quaternion.Euler(0, 305, 180);
                                break;
                            case EShipType.E_ROLAND:
                                GuidebookGO.transform.SetParent(pLShipInfo.InteriorStatic.transform.GetChild(7));
                                GuidebookGO.transform.localPosition = new Vector3(-4.4f, 13.96f, -25.8f);
                                GuidebookGO.transform.rotation = Quaternion.Euler(0, 32, 180);
                                break;
                            case EShipType.E_DESTROYER:
                                GuidebookGO.transform.SetParent(pLShipInfo.InteriorStatic.transform.GetChild(3).GetChild(138));
                                GuidebookGO.transform.localPosition = new Vector3(2.8f, 1.04f, 20.7f);
                                GuidebookGO.transform.rotation = Quaternion.Euler(0, 180, 180);
                                break;
                            case EShipType.E_OUTRIDER:
                                GuidebookGO.transform.SetParent(pLShipInfo.InteriorStatic.transform.GetChild(2).GetChild(1));
                                GuidebookGO.transform.localPosition = new Vector3(-0.5f, 0.095f, -9.3f);
                                GuidebookGO.transform.rotation = Quaternion.Euler(0, 332, 180);
                                break;
                            case EShipType.E_FLUFFY_DELIVERY:
                                GuidebookGO.transform.SetParent(pLShipInfo.InteriorStatic.transform.GetChild(17));
                                GuidebookGO.transform.localPosition = new Vector3(-1.5f, -0.8f, -5.6f);
                                GuidebookGO.transform.rotation = Quaternion.Euler(0, 0, 180);
                                break;
                            case EShipType.E_ANNIHILATOR:
                                GuidebookGO.transform.SetParent(pLShipInfo.InteriorStatic.transform.GetChild(24));
                                GuidebookGO.transform.localPosition = new Vector3(-9.9f, 0.82f, -1);
                                GuidebookGO.transform.rotation = Quaternion.Euler(0, 305, 180);
                                break;
                            case EShipType.E_CIVILIAN_STARTING_SHIP:
                                GuidebookGO.transform.SetParent(pLShipInfo.InteriorStatic.transform.GetChild(11));
                                GuidebookGO.transform.localPosition = new Vector3(5.3f, -2.59f, -18.8f);
                                GuidebookGO.transform.rotation = Quaternion.Euler(0, 345, 180);
                                break;
                            case EShipType.OLDWARS_HUMAN:
                                GuidebookGO.transform.SetParent(pLShipInfo.InteriorStatic.transform.GetChild(11));
                                GuidebookGO.transform.localPosition = new Vector3(-16.2f, 16.21f, -23.71f);
                                GuidebookGO.transform.rotation = Quaternion.Euler(0, 337, 180);
                                break;
                            case EShipType.OLDWARS_SYLVASSI:
                                GuidebookGO.transform.SetParent(pLShipInfo.InteriorStatic.transform.GetChild(13));
                                GuidebookGO.transform.localPosition = new Vector3(6.9f, 11.46f, -10.3f);
                                GuidebookGO.transform.rotation = Quaternion.Euler(0, 270, 180);
                                break;
                            case EShipType.E_POLYTECH_SHIP:
                                GuidebookGO.transform.SetParent(pLShipInfo.InteriorStatic.transform);
                                GuidebookGO.transform.localPosition = new Vector3(3.6f, -18.45f, 21.9f);
                                GuidebookGO.transform.rotation = Quaternion.Euler(0, 180, 180);
                                break;
                            case EShipType.E_FLUFFY_TWO:
                                GuidebookGO.transform.SetParent(pLShipInfo.InteriorStatic.transform.GetChild(0));
                                GuidebookGO.transform.localPosition = new Vector3(20.5f, 2.005f, 1.9f);
                                GuidebookGO.transform.rotation = Quaternion.Euler(0, 270, 180);
                                break;
                            case EShipType.E_ABYSS_PLAYERSHIP:
                                GuidebookGO.transform.SetParent(pLShipInfo.InteriorStatic.transform.GetChild(23).GetChild(7));
                                GuidebookGO.transform.localPosition = new Vector3(-4.1f, 7.489f, -11.1f);
                                GuidebookGO.transform.rotation = Quaternion.Euler(0, 180, 180.5f);
                                break;
                            default:
                                GameObject.Destroy(GuidebookGO);
                                flag = true;
                                break;
                        }
                        if (!flag)
                            InsideShipDAIs.Add(pLDialogueActorInstance);
                    }
                    if (pLShipInfo.InWarp)
                    {
                        ProcessedShipIDs.RemoveAll(v => v != pLShipInfo.ShipID);
                        InsideShipDAIs.RemoveAll(x => !(x != null));
                    }
                }
                yield return new WaitForSeconds(1);
            }
        }

        internal static void ServerStartGuidebookCoroutine(PLServer __instance)
        {
            __instance.StartCoroutine(PlaceGuidebookRoutine(__instance));
        }
    }
}
