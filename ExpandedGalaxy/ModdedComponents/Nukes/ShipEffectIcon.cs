using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLInGameUI), "UpdateAllRightShips")]
    internal class ShipEffectIcon
    {
        private static void Postfix(PLInGameUI __instance, PLShipInfo currentShip)
        {
            foreach (PLInGameUI.DisplayShipRightInfo info in __instance.AllDisplayRightShipRightInfos)
            {
                if (info.Root != null)
                {
                    bool flag = false;
                    for (int i = info.Root.transform.childCount - 1; i >= 0; i--)
                    {
                        if (info.Root.transform.GetChild(i).name == "StatusEffect")
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (!flag)
                    {
                        GameObject obj = new GameObject("StatusEffect", new System.Type[1]
                        {
                                typeof(RawImage)
                        });
                        obj.transform.SetParent(info.Root.transform);
                        obj.transform.localPosition = new Vector3(-75f * info.Scale, 0f, 0f);
                        obj.transform.localRotation = Quaternion.identity;
                        obj.transform.localScale = info.SpaceTarget is PLDamageableSpaceObject ? new Vector3(0.2f, 0.2f, 0.2f) : new Vector3(0.3f, 0.3f, 0.3f);
                        obj.GetComponent<RawImage>().GetComponent<RectTransform>().anchoredPosition3D = obj.GetComponent<RawImage>().transform.localPosition;
                        obj.GetComponent<RawImage>().texture = PLGlobal.Instance.ClassIcons[3];
                        obj.GetComponent<RawImage>().color = PLGlobal.Instance.ClassColors[3];
                        obj.GetComponent<RawImage>().raycastTarget = false;
                        obj.SetActive(false);
                    }
                    for (int i = info.Root.transform.childCount - 1; i >= 0; i--)
                    {
                        if (info.Root.transform.GetChild(i).name == "StatusEffect")
                        {
                            if (info.SpaceTarget != null && Nukes.AllStatusDatas.ContainsKey(info.SpaceTarget.SpaceTargetID))
                            {
                                info.Root.transform.GetChild(i).gameObject.SetActive(true);
                                if (info.TimeWithoutDetection < 0.1f + Time.deltaTime)
                                {
                                    info.Root.transform.GetChild(i).gameObject.GetComponent<RawImage>().color = PLGlobal.Instance.ClassColors[3];
                                }
                                else
                                {
                                    info.Root.transform.GetChild(i).gameObject.GetComponent<RawImage>().color = PLGlobal.Instance.ClassColors[3] * 0.5f;
                                }
                            }
                            else
                                info.Root.transform.GetChild(i).gameObject.SetActive(false);
                            break;
                        }
                    }
                }
            }
        }
    }
}
