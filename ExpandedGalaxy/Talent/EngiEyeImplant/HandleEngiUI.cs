using HarmonyLib;
using PulsarModLoader.Patches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Talents.Framework;
using UnityEngine;
using UnityEngine.UI;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLInGameUI), "Update")]
    internal class HandleEngiUI
    {
        internal static bool SetupUI = false;
        internal static Image[] SystemHealthOutline = new Image[4];
        internal static Image[] SystemHealthBG = new Image[4];
        internal static Image[] SystemHealthBar = new Image[4];
        internal static Image[] SystemIcon = new Image[4];
        internal static Image[] SystemFireIcon = new Image[4];

        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            Label succeed = generator.DefineLabel();
            List<CodeInstruction> list = instructions.ToList();

            List<CodeInstruction> targetSequence = new List<CodeInstruction>() {
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(PLInGameUI), "UpdateShipIndicators"))
                };
            List<CodeInstruction> patchSequence = new List<CodeInstruction>()
                {
                    new CodeInstruction(OpCodes.Ldarg_0),
                    CodeInstruction.Call(typeof(ExpandedGalaxy.HandleEngiUI), "HandleEngineerImplantUI", new Type[1] {
                        typeof(PLInGameUI)
                    }),
                    new CodeInstruction(OpCodes.Brtrue_S, succeed)
                };
            list[5694].labels.Add(succeed);

            return HarmonyHelpers.PatchBySequence(list.AsEnumerable<CodeInstruction>(), targetSequence, patchSequence, HarmonyHelpers.PatchMode.AFTER, HarmonyHelpers.CheckMode.NONNULL, false);
        }

        internal static bool HandleEngineerImplantUI(PLInGameUI pLInGameUI)
        {
            if (ShouldShowEngineerImplantUI())
            {
                if (!pLInGameUI.PilotingUIRoot.activeSelf)
                    pLInGameUI.PilotingUIRoot.SetActive(true);
                if (!HandleEngiUI.SetupUI)
                {
                    pLInGameUI.PilotAbilityRoot.SetActive(false);

                    pLInGameUI.PilotingSpeed.gameObject.SetActive(false);
                    pLInGameUI.PilotingSpeedBGCenter.gameObject.SetActive(false);
                    pLInGameUI.PilotingSpeedIndicator.gameObject.SetActive(false);
                    pLInGameUI.PilotingUIRoot.transform.Find("SpeedBG").gameObject.SetActive(false);

                    pLInGameUI.PilotingUIRoot.transform.Find("BoostBG").localPosition = new Vector3(0f, -238f, 0f);

                    for (int i = 0; i < 4; i++)
                    {
                        if (HandleEngiUI.SystemHealthOutline[i] == null)
                        {
                            Image image = UnityEngine.Object.Instantiate(pLInGameUI.BoostFill);
                            image.transform.SetParent(pLInGameUI.PilotingUIRoot.transform.Find("BoostBG"));
                            image.transform.localPosition = new Vector3(-24f + (16f * i), 23f, 0f);
                            image.rectTransform.sizeDelta = new Vector2(15f, 27f);
                            image.color = Color.white;
                            HandleEngiUI.SystemHealthOutline[i] = image;
                        }
                        else
                        {
                            HandleEngiUI.SystemHealthOutline[i].color = Color.white;
                            HandleEngiUI.SystemHealthOutline[i].gameObject.SetActive(true);
                        }
                        if (HandleEngiUI.SystemHealthBG[i] == null)
                        {
                            Image image = UnityEngine.Object.Instantiate(pLInGameUI.BoostFill);
                            image.transform.SetParent(pLInGameUI.PilotingUIRoot.transform.Find("BoostBG"));
                            image.transform.localPosition = new Vector3(-24f + (16f * i), 23f, 0f);
                            image.rectTransform.sizeDelta = new Vector2(12f, 24f);
                            image.color = Color.black;
                            HandleEngiUI.SystemHealthBG[i] = image;
                        }
                        else
                        {
                            HandleEngiUI.SystemHealthBG[i].gameObject.SetActive(true);
                        }
                        if (HandleEngiUI.SystemFireIcon[i] == null)
                        {
                            Image image = UnityEngine.Object.Instantiate(pLInGameUI.BoostFill);
                            Sprite sprite = Sprite.Create(PLGlobal.Instance.FireIcon, new Rect(0f, 0f, 128f, 128f), new Vector2(0.5f, 0.5f));
                            image.sprite = sprite;
                            image.transform.SetParent(pLInGameUI.PilotingUIRoot.transform.Find("BoostBG"));
                            image.transform.localPosition = new Vector3(-24f + (16f * i), 16f, 0f);
                            image.rectTransform.sizeDelta = new Vector2(12f, 12f);
                            image.color = new Color(1f, 0.6f, 0f, 1f);
                            image.gameObject.SetActive(false);
                            HandleEngiUI.SystemFireIcon[i] = image;
                        }
                        if (HandleEngiUI.SystemHealthBar[i] == null)
                        {
                            Image image = UnityEngine.Object.Instantiate(pLInGameUI.BoostFill);
                            image.transform.SetParent(pLInGameUI.PilotingUIRoot.transform.Find("BoostBG"));
                            image.transform.localPosition = new Vector3(-24f + (16f * i), 23f, 0f);
                            image.rectTransform.sizeDelta = new Vector2(12f, 24f);
                            image.color = Color.red;
                            HandleEngiUI.SystemHealthBar[i] = image;
                        }
                        else
                        {
                            HandleEngiUI.SystemHealthBar[i].transform.localPosition = new Vector3(-24f + (16f * i), 23f, 0f);
                            HandleEngiUI.SystemHealthBar[i].rectTransform.sizeDelta = new Vector2(12f, 24f);
                            HandleEngiUI.SystemHealthBar[i].gameObject.SetActive(true);
                        }
                        if (HandleEngiUI.SystemIcon[i] == null)
                        {
                            Image image = UnityEngine.Object.Instantiate(pLInGameUI.BoostFill);
                            Sprite sprite = Sprite.Create(PLGlobal.Instance.SystemIcon[i], new Rect(0f, 0f, 64f, 64f), new Vector2(0.5f, 0.5f));
                            image.sprite = sprite;
                            image.transform.SetParent(pLInGameUI.PilotingUIRoot.transform.Find("BoostBG"));
                            image.transform.localPosition = new Vector3(-24f + (16f * i), 45.5f, 0f);
                            image.rectTransform.sizeDelta = new Vector2(15f, 15f);
                            image.color = Color.white;
                            HandleEngiUI.SystemIcon[i] = image;
                        }
                        else
                        {
                            HandleEngiUI.SystemIcon[i].color = Color.white;
                            HandleEngiUI.SystemIcon[i].gameObject.SetActive(true);
                        }
                    }

                    pLInGameUI.BoostLabel.text = "COOLANT";
                    pLInGameUI.BoostLabel.color = Color.white;
                    pLInGameUI.BoostFillOutline.gameObject.SetActive(false);
                    pLInGameUI.BoostFill.color = new Color(0f, 0.6f, 0.3f, 1f);

                    pLInGameUI.PilotingControlMode.text = "Pump State:";
                    pLInGameUI.PilotingControlMode.transform.localPosition = new Vector3(-90.6f, -251.9f, 0f);
                    pLInGameUI.PilotingCameraMode.rectTransform.localPosition = new Vector3(-10.6f, -251.9f, 0f);
                    pLInGameUI.PilotingCameraMode.text = "OFF";

                    pLInGameUI.Piloting_ReactorPower.text = "STABILITY 100%";
                    HandleEngiUI.SetupUI = true;
                }

                PLShipInfo playership = PLEncounterManager.Instance.PlayerShip;
                pLInGameUI.BoostFill.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 130f * playership.ReactorCoolantLevelPercent);
                pLInGameUI.BoostFill.transform.localPosition = new Vector3(-65f + (65f * playership.ReactorCoolantLevelPercent), -0.0001f, 0f);

                float lerpSpeed = 0f;
                switch (playership.ReactorCoolingPumpState)
                {
                    case 0:
                        pLInGameUI.PilotingCameraMode.text = "OFF";
                        break;
                    case 1:
                        pLInGameUI.PilotingCameraMode.text = "LOW";
                        lerpSpeed = 3f;
                        break;
                    case 2:
                        pLInGameUI.PilotingCameraMode.text = "HIGH";
                        lerpSpeed = 12f;
                        break;
                }
                if (lerpSpeed == 0f)
                    pLInGameUI.BoostFill.color = new Color(0f, 0.6f, 0.3f, 1f);
                else
                    pLInGameUI.BoostFill.color = new Color(0f, 0.6f, 0.3f, 1f) * (float)(0.89999997615814209 + ((double)Mathf.Sin(Time.time * lerpSpeed) + 0.800000011920929) * 0.10000000149011612);

                pLInGameUI.PilotingSpeed.gameObject.SetActive(false);

                for (int i = 0; i < 4; i++)
                {
                    if (HandleEngiUI.SystemHealthBar[i] != null)
                    {
                        float num = 0f;
                        if (playership.GetSystemFromID(i) != null)
                            num = playership.GetSystemFromID(i).GetHealthRatio();
                        HandleEngiUI.SystemHealthBar[i].rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 24f * num);
                        HandleEngiUI.SystemHealthBar[i].transform.localPosition = new Vector3(-24f + (16f * i), 11f + (12f * num), 0f);
                    }
                    if (HandleEngiUI.SystemHealthOutline[i] != null)
                    {
                        if (playership.GetSystemFromID(i) != null && playership.GetSystemFromID(i).IsOnFire())
                        {
                            float t = Mathf.Clamp01((float)((double)Time.time * 1.5 % 1.25));
                            HandleEngiUI.SystemHealthOutline[i].color = Color.Lerp(new Color(1f, 0.6f, 0.0f, 1f), Color.white, t);
                        }
                        else
                            HandleEngiUI.SystemHealthOutline[i].color = Color.white;
                    }
                    if (HandleEngiUI.SystemIcon[i] != null)
                    {
                        if (playership.GetSystemFromID(i) != null && playership.GetSystemFromID(i).IsOnFire())
                        {
                            float t = Mathf.Clamp01((float)((double)Time.time * 1.5 % 1.25));
                            HandleEngiUI.SystemIcon[i].color = Color.Lerp(new Color(1f, 0.6f, 0.0f, 1f), Color.white, t);
                        }
                        else
                            HandleEngiUI.SystemIcon[i].color = Color.white;
                    }
                    if (HandleEngiUI.SystemFireIcon[i] != null)
                    {
                        if (playership.GetSystemFromID(i) != null && playership.GetSystemFromID(i).IsOnFire())
                        {
                            HandleEngiUI.SystemFireIcon[i].gameObject.SetActive(true);
                        }
                        else
                            HandleEngiUI.SystemFireIcon[i].gameObject.SetActive(false);
                    }
                }

                pLInGameUI.PilotingControlMode.text = "Pump State:";
                pLInGameUI.Piloting_ReactorTemp.text = "TEMP " + (100f * playership.MyStats.ReactorTempCurrent / playership.MyStats.ReactorTempMax).ToString("0") + "%";
                pLInGameUI.Piloting_ReactorPower.text = "STABILITY " + (100f * (1f - playership.CoreInstability)).ToString("0") + "%";
                return true;
            }
            else
            {
                if (HandleEngiUI.SetupUI)
                {
                    pLInGameUI.PilotingSpeedBGCenter.gameObject.SetActive(true);
                    pLInGameUI.PilotingSpeedIndicator.gameObject.SetActive(true);
                    pLInGameUI.PilotingUIRoot.transform.Find("SpeedBG").gameObject.SetActive(true);

                    pLInGameUI.PilotingUIRoot.transform.Find("BoostBG").localPosition = new Vector3(0f, -220f, 0f);

                    for (int i = 0; i < 4; i++)
                    {
                        if (HandleEngiUI.SystemHealthOutline[i] != null)
                            HandleEngiUI.SystemHealthOutline[i].gameObject.SetActive(false);
                        if (HandleEngiUI.SystemHealthBG[i] != null)
                            HandleEngiUI.SystemHealthBG[i].gameObject.SetActive(false);
                        if (HandleEngiUI.SystemHealthBar[i] != null)
                            HandleEngiUI.SystemHealthBar[i].gameObject.SetActive(false);
                        if (HandleEngiUI.SystemIcon[i] != null)
                            HandleEngiUI.SystemIcon[i].gameObject.SetActive(false);
                        if (HandleEngiUI.SystemFireIcon[i] != null)
                            HandleEngiUI.SystemFireIcon[i].gameObject.SetActive(false);

                        pLInGameUI.BoostLabel.text = "BOOST";
                        pLInGameUI.BoostLabel.color = Color.white;
                        pLInGameUI.BoostFillOutline.gameObject.SetActive(true);
                        pLInGameUI.BoostFill.color = PLGlobal.Instance.ClassColors[0];

                        pLInGameUI.PilotingControlMode.text = "Manual";
                        pLInGameUI.PilotingControlMode.transform.localPosition = new Vector3(-110.6f, -249.9f, 0f);
                        pLInGameUI.PilotingCameraMode.rectTransform.localPosition = new Vector3(-110.6f, -237.9f, 0f);
                        pLInGameUI.PilotingCameraMode.text = "Orbit";

                        pLInGameUI.Piloting_ReactorPower.text = "";
                        HandleEngiUI.SetupUI = false;
                    }
                }
                return false;
            }
        }

        private static bool ShouldShowEngineerImplantUI()
        {
            return PLGlobal.Instance.BottomInfoLabelStringTop == "" && PLNetworkManager.Instance.LocalPlayer != null && ((int)PLNetworkManager.Instance.LocalPlayer.Talents[TalentModManager.Instance.GetTalentIDFromName("Engineer Eye Implant")] > 0 || (PLNetworkManager.Instance.LocalPlayer.GetClassID() == 0 && PLServer.Instance.GetCachedFriendlyPlayerOfClass(4) != null && PLServer.Instance.GetCachedFriendlyPlayerOfClass(4).Talents[TalentModManager.Instance.GetTalentIDFromName("Engineer Eye Implant")] > 0)) && PLNetworkManager.Instance.LocalPlayer.GetPawn() != null && !PLNetworkManager.Instance.LocalPlayer.GetPawn().IsDead && PLCameraManager.Instance != null && PLCameraSystem.Instance != null && PLCameraSystem.Instance.CurrentCameraMode != null && PLCameraSystem.Instance.CurrentCameraMode.GetModeString() == "LocalPawn" && !PLNetworkManager.Instance.LocalPlayer.IsSittingInCaptainsChair() && PLNetworkManager.Instance.LocalPlayer.GetPawn().CurrentShip != null && PLNetworkManager.Instance.LocalPlayer.GetPawn().CurrentShip.GetIsPlayerShip();
        }
    }
}
