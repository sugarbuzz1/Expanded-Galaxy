using HarmonyLib;
using PulsarModLoader;
using PulsarModLoader.Patches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using UnityEngine;

namespace ExpandedGalaxy
{
    public struct CrewLogData
    {
        public string Text;
        public float timeStamp;
        public int optionalSectorID;
        public Color optionalColor;
        public int specialData;
    }

    public class MapPin
    {
        public string Name;
        public int LogIndex;
        public int Priority;
        public Color Color;
        public GameObject PinObject;
    }

    public struct CrewLogScreenObjects
    {
        public UITexture CrewLogButton;
        public UITexture StatusButton;
        public UISprite LogPanel;
        public UILabel SectorLabel;
        public UILabel TimeLabel;
        public List<UISprite> LogButtons;
        public List<UITexture> LogColors;
        public UISprite BackButton;
        public UISprite NextButton;
        public UISprite NewLogButton;
        public UIPanel LogInfoClippingPanel;
        public UIWidget LogInfoPanel;
        public UISprite LogInfoBox;
        public UILabel LogInfoBoxLabel;
        public UILabel LogInfoBoxText;
        public UISprite LogInfoBoxClose;
        public UISprite LogInfoBoxCreate;
        public UISprite LogInfoButtonDel;
        public UISprite LogInfoBoxSectorButton;
        public UITexture LogInfoSectorColor;
        public UISprite LogInfoBoxWrite;
        public List<UITexture> LogInfoKeypadButtons;
        public List<UILabel> LogInfoKeypadLabels;
        public List<UIWidget> SpecialLogObjects;
    }
    internal class CrewLog
    {
        [HarmonyPatch(typeof(PLCaptainScreen), "SetupUI")]
        internal class SetupCaptainScreen
        {
            private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                List<CodeInstruction> list = instructions.ToList();

                List<CodeInstruction> targetSequence = new List<CodeInstruction>() {
                    new CodeInstruction(OpCodes.Ldc_R4, -60f),
                };
                List<CodeInstruction> patchSequence = new List<CodeInstruction>()
                {
                    new CodeInstruction(OpCodes.Ldc_R4, 32f),
                    new CodeInstruction(OpCodes.Sub)

                };
                list = HarmonyHelpers.PatchBySequence(list.AsEnumerable<CodeInstruction>(), targetSequence, patchSequence, HarmonyHelpers.PatchMode.AFTER, HarmonyHelpers.CheckMode.NONNULL, false).ToList();

                List<CodeInstruction> targetSequence2 = new List<CodeInstruction>() {
                    new CodeInstruction(OpCodes.Ldc_R4, 64f),
                };
                List<CodeInstruction> patchSequence2 = new List<CodeInstruction>()
                {
                    new CodeInstruction(OpCodes.Ldc_R4, 32f),

                };

                return HarmonyHelpers.PatchBySequence(list.AsEnumerable<CodeInstruction>(), targetSequence2, patchSequence2, HarmonyHelpers.PatchMode.REPLACE, HarmonyHelpers.CheckMode.NONNULL, false);
            }

            private static void Postfix(PLCaptainScreen __instance)
            {
                CrewLogManager.Instance.SetupScreen(__instance);
            }
        }

        [HarmonyPatch(typeof(PLCaptainScreen), "OnButtonClick")]
        internal class CaptainScreenButton
        {
            private static void Postfix(PLCaptainScreen __instance, UIWidget inButton, ref UISprite ___StatusPanel, ref UISprite ___EnemyStatusPanel)
            {
                if (!CrewLogManager.Instance.HasScreenObjects(__instance))
                    return;
                CrewLogScreenObjects screenObjects = CrewLogManager.Instance.GetObjectsForScreen(__instance);
                if (inButton.name == "CLBtn" && !screenObjects.LogPanel.gameObject.activeSelf)
                {
                    if (__instance.MyScreenHubBase.OptionalShipInfo != null && !__instance.MyScreenHubBase.OptionalShipInfo.GetIsPlayerShip())
                        return;
                    screenObjects.LogPanel.gameObject.SetActive(true);
                    CrewLogManager.Instance.UpdateAllPins();
                    __instance.mouseUpFrame = false;
                    ___StatusPanel.gameObject.SetActive(false);
                    ___EnemyStatusPanel.gameObject.SetActive(false);
                    __instance.PlaySoundEventOnAllClonedScreens("play_ship_generic_internal_computer_ui_click");
                }
                else if (inButton.name == "StatusBtn" && screenObjects.LogPanel.gameObject.activeSelf && !screenObjects.LogInfoPanel.gameObject.activeSelf)
                {
                    screenObjects.LogPanel.gameObject.SetActive(false);
                    CrewLogManager.Instance.UpdateAllPins();
                    __instance.mouseUpFrame = false;
                    ___StatusPanel.gameObject.SetActive(true);
                    ___EnemyStatusPanel.gameObject.SetActive(true);
                    __instance.PlaySoundEventOnAllClonedScreens("play_ship_generic_internal_computer_ui_click");
                }
                else if (inButton.name == "NewBtn" && !screenObjects.LogInfoPanel.gameObject.activeSelf)
                {
                    if ((bool)PLServer.Instance.CrewPurchaseLimitsEnabled)
                    {
                        if (PLNetworkManager.Instance.LocalPlayer != null && PLNetworkManager.Instance.LocalPlayer.GetClassID() != 0)
                            PLTabMenu.Instance.TimedErrorMsg = "You do not have permission to add logs at this time!";
                        return;
                    }
                    CrewLogManager.Instance.LogIndex = int.MinValue;
                    screenObjects.LogInfoPanel.gameObject.SetActive(true);
                    screenObjects.LogInfoBoxLabel.text = "New Log";
                    screenObjects.LogInfoBoxText.text = "";
                    screenObjects.LogInfoBoxText.fontSize = 84;
                    screenObjects.LogInfoBoxText.width = 720;
                    CrewLogData data = new CrewLogData
                    {
                        optionalSectorID = -1,
                        timeStamp = (float)PLServer.Instance.Playtime,
                        optionalColor = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f)),
                        Text = string.Empty,
                        specialData = -1
                    };
                    screenObjects.LogInfoBoxSectorButton.gameObject.SetActive(false);
                    screenObjects.LogInfoBoxSectorButton.GetComponentInChildren<UILabel>().text = string.Empty;
                    screenObjects.LogInfoBoxCreate.gameObject.SetActive(true);
                    screenObjects.LogInfoButtonDel.gameObject.SetActive(false);
                    screenObjects.LogInfoSectorColor.color = data.optionalColor;
                    CrewLogManager.Instance.HideKeyPad(__instance, false);
                    CrewLogManager.Instance.TempData = data;
                    __instance.StartCoroutine(CrewLogManager.Instance.ToggleLogButtons(__instance));
                    __instance.PlaySoundEventOnAllClonedScreens("play_ship_generic_internal_computer_ui_click");
                }
                else if (inButton.name == "LogInfoCloseBtn")
                {
                    CrewLogManager.Instance.LogIndex = -1;
                    screenObjects.LogInfoPanel.gameObject.SetActive(false);
                    CrewLogManager.Instance.HideKeyPad(__instance);
                    if (screenObjects.SpecialLogObjects.Count > 0)
                    {
                        for (int i = 0; i < screenObjects.SpecialLogObjects.Count; i++)
                        {
                            UnityEngine.Object.Destroy(screenObjects.SpecialLogObjects[i].gameObject);
                        }
                        screenObjects.SpecialLogObjects.Clear();
                    }
                    __instance.StartCoroutine(CrewLogManager.Instance.ToggleLogButtons(__instance, false));
                    __instance.PlaySoundEventOnAllClonedScreens("play_ship_generic_internal_computer_ui_click");
                }
                else if (inButton.name == "LogInfoCreateBtn")
                {
                    CrewLogManager.Instance.LogIndex = -1;
                    if (PhotonNetwork.isMasterClient)
                    {
                        CrewLogManager.Instance.AddLog(CrewLogManager.Instance.TempData);
                        List<object> sendArgumentList = new List<object>();
                        int logCount = CrewLogManager.Instance.GetLogs().Count;
                        sendArgumentList.Add(logCount);
                        foreach (CrewLogData sendLogData in CrewLogManager.Instance.GetLogs())
                        {
                            sendArgumentList.Add(sendLogData.Text);
                            sendArgumentList.Add(sendLogData.timeStamp);
                            sendArgumentList.Add(sendLogData.optionalSectorID);
                            sendArgumentList.Add(sendLogData.optionalColor.r);
                            sendArgumentList.Add(sendLogData.optionalColor.g);
                            sendArgumentList.Add(sendLogData.optionalColor.b);
                            sendArgumentList.Add(sendLogData.optionalColor.a);
                            sendArgumentList.Add(sendLogData.specialData);
                        }
                        ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.ServerSendLog", PhotonTargets.Others, sendArgumentList.ToArray());
                    }
                    else
                    {
                        ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.ClientSendLog", PhotonTargets.MasterClient, new object[7]
                        {
                            CrewLogManager.Instance.TempData.Text,
                            CrewLogManager.Instance.TempData.timeStamp,
                            CrewLogManager.Instance.TempData.optionalSectorID,
                            CrewLogManager.Instance.TempData.optionalColor.r,
                            CrewLogManager.Instance.TempData.optionalColor.g,
                            CrewLogManager.Instance.TempData.optionalColor.b,
                            CrewLogManager.Instance.TempData.optionalColor.a
                        });
                    }
                    screenObjects.LogInfoPanel.gameObject.SetActive(false);
                    CrewLogManager.Instance.HideKeyPad(__instance);
                    __instance.StartCoroutine(CrewLogManager.Instance.ToggleLogButtons(__instance, false));
                    __instance.PlaySoundEventOnAllClonedScreens("play_ship_generic_internal_computer_ui_click");
                }
                else if (inButton.name == "LogInfoDelBtn")
                {
                    if (PLNetworkManager.Instance.LocalPlayer != null && PLNetworkManager.Instance.LocalPlayer.GetClassID() != 0)
                    {
                        PLTabMenu.Instance.TimedErrorMsg = "Only the captain can delete logs!";
                        return;
                    }
                    if (CrewLogManager.Instance.LogIndex > -1)
                    {
                        CrewLogManager.Instance.RemoveLog(CrewLogManager.Instance.LogIndex);
                        List<object> sendArgumentList = new List<object>();
                        int logCount = CrewLogManager.Instance.GetLogs().Count;
                        sendArgumentList.Add(logCount);
                        foreach (CrewLogData sendLogData in CrewLogManager.Instance.GetLogs())
                        {
                            sendArgumentList.Add(sendLogData.Text);
                            sendArgumentList.Add(sendLogData.timeStamp);
                            sendArgumentList.Add(sendLogData.optionalSectorID);
                            sendArgumentList.Add(sendLogData.optionalColor.r);
                            sendArgumentList.Add(sendLogData.optionalColor.g);
                            sendArgumentList.Add(sendLogData.optionalColor.b);
                            sendArgumentList.Add(sendLogData.optionalColor.a);
                            sendArgumentList.Add(sendLogData.specialData);
                        }
                        ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.ServerSendLog", PhotonTargets.Others, sendArgumentList.ToArray());
                    }
                    CrewLogManager.Instance.LogIndex = -1;
                    screenObjects.LogInfoPanel.gameObject.SetActive(false);
                    CrewLogManager.Instance.HideKeyPad(__instance);
                    __instance.StartCoroutine(CrewLogManager.Instance.ToggleLogButtons(__instance, false));
                    __instance.PlaySoundEventOnAllClonedScreens("play_ship_generic_internal_computer_ui_click");
                }
                else if (inButton.name.Contains("LogBtn") && !screenObjects.LogInfoPanel.gameObject.activeSelf)
                {
                    CrewLogManager.Instance.LogIndex = int.Parse(inButton.name.Remove(0, 6)) - 1 + (10 * CrewLogManager.Instance.ShowLogIndex);
                    CrewLogManager.Instance.TempData = CrewLogManager.Instance.GetLogs()[CrewLogManager.Instance.LogIndex];
                    screenObjects.LogInfoPanel.gameObject.SetActive(true);
                    screenObjects.LogInfoBoxLabel.text = "Log #" + (CrewLogManager.Instance.LogIndex + 1).ToString();
                    screenObjects.LogInfoBoxSectorButton.gameObject.SetActive(false);
                    screenObjects.LogInfoSectorColor.color = CrewLogManager.Instance.TempData.optionalColor;
                    screenObjects.LogInfoBoxCreate.gameObject.SetActive(false);
                    CrewLogManager.Instance.HideKeyPad(__instance);
                    __instance.StartCoroutine(CrewLogManager.Instance.ToggleLogButtons(__instance));
                    __instance.PlaySoundEventOnAllClonedScreens("play_ship_generic_internal_computer_ui_click");
                    if (CrewLogManager.Instance.TempData.specialData == -1)
                    {
                        screenObjects.LogInfoButtonDel.gameObject.SetActive(true);
                        screenObjects.LogInfoBoxText.fontSize = 84;
                        screenObjects.LogInfoBoxText.width = 720;
                        screenObjects.LogInfoBoxText.text = CrewLogManager.Instance.TempData.Text;
                        screenObjects.LogInfoBoxSectorButton.GetComponentInChildren<UILabel>().text = CrewLogManager.Instance.TempData.optionalSectorID.ToString();                      
                    }
                    else
                    {
                        CrewLogManager.Instance.SetupSpecialLog(__instance, CrewLogManager.Instance.TempData.specialData);
                    }
                }
                else if (inButton.name == "LogInfoSectorBtn")
                {
                    if (CrewLogManager.Instance.TempData.optionalSectorID > -1)
                    {
                        if (PLGlobal.Instance.Galaxy != null && PLGlobal.Instance.Galaxy.AllSectorInfos.ContainsKey(CrewLogManager.Instance.TempData.optionalSectorID))
                        {
                            PLStarmap.Instance.OpenStarmapToSector(PLGlobal.Instance.Galaxy.AllSectorInfos[CrewLogManager.Instance.TempData.optionalSectorID]);
                            __instance.PlaySoundEventOnAllClonedScreens("play_ship_generic_internal_computer_ui_click");
                        }
                    }
                }
                else if (inButton.name == "LogInfoWriteBtn")
                {
                    __instance.PlaySoundEventOnAllClonedScreens("play_ship_generic_internal_computer_ui_click");
                    if (PLNetworkManager.Instance != null)
                    {
                        PLNetworkManager.Instance.IsTyping = true;
                        PLNetworkManager.Instance.CurrentChatText = "/exgal log ";
                    }
                }
                else if (inButton.name.Contains("KeypadBtn_") && screenObjects.LogInfoPanel.gameObject.activeSelf)
                {
                    int keypadPressed = int.Parse(inButton.name.Remove(0, 10));
                    __instance.PlaySoundEventOnAllClonedScreens("play_ship_generic_internal_computer_ui_keypad");
                    string sectorString = CrewLogManager.Instance.GetObjectsForScreen(__instance).LogInfoBoxSectorButton.GetComponentInChildren<UILabel>().text;
                    if (sectorString.Length < 6 || keypadPressed == 3 || keypadPressed == 11)
                    {
                        switch (keypadPressed)
                        {
                            case 0:
                                sectorString += "1";
                                break;
                            case 1:
                                sectorString += "4";
                                break;
                            case 2:
                                sectorString += "7";
                                break;
                            case 3:
                                if (sectorString.Length > 1)
                                    sectorString = sectorString.Substring(0, sectorString.Length - 1);
                                else
                                    sectorString = "-1";
                                break;
                            case 4:
                                sectorString += "2";
                                break;
                            case 5:
                                sectorString += "5";
                                break;
                            case 6:
                                sectorString += "8";
                                break;
                            case 7:
                                sectorString += "0";
                                break;
                            case 8:
                                sectorString += "3";
                                break;
                            case 9:
                                sectorString += "6";
                                break;
                            case 10:
                                sectorString += "9";
                                break;
                            default:
                                if (PLServer.GetCurrentSector() != null)
                                    sectorString = PLServer.GetCurrentSector().ID.ToString();
                                break;
                        }
                    }
                    CrewLogManager.Instance.GetObjectsForScreen(__instance).LogInfoBoxSectorButton.GetComponentInChildren<UILabel>().text = sectorString != "-1" ? sectorString : string.Empty;
                    if (sectorString == "-1")
                        CrewLogManager.Instance.GetObjectsForScreen(__instance).LogInfoBoxSectorButton.gameObject.SetActive(false);
                    else
                        CrewLogManager.Instance.GetObjectsForScreen(__instance).LogInfoBoxSectorButton.gameObject.SetActive(true);
                    CrewLogData logData = CrewLogManager.Instance.TempData;
                    try
                    {
                        logData.optionalSectorID = int.Parse(sectorString);
                    }
                    catch
                    {
                        logData.optionalSectorID = -1;
                    }
                    CrewLogManager.Instance.TempData = logData;
                }
                else if (inButton.name == "NextBtn" && !screenObjects.LogInfoPanel.gameObject.activeSelf)
                {
                    ++CrewLogManager.Instance.ShowLogIndex;
                    __instance.PlaySoundEventOnAllClonedScreens("play_ship_generic_internal_computer_ui_keypad");
                }
                else if (inButton.name == "BackBtn" && !screenObjects.LogInfoPanel.gameObject.activeSelf)
                {
                    --CrewLogManager.Instance.ShowLogIndex;
                    __instance.PlaySoundEventOnAllClonedScreens("play_ship_generic_internal_computer_ui_keypad");
                }
            }
        }

        [HarmonyPatch(typeof(PLCaptainScreen), "Update")]
        internal class CaptainScreenUpdate
        {
            private static void Postfix(PLCaptainScreen __instance, ref UISprite ___StatusPanel, ref UISprite ___EnemyStatusPanel)
            {
                if (__instance.MyRootPanel == null || !__instance.LocalPlayerInSameLocation() || !CrewLogManager.Instance.HasScreenObjects(__instance))
                    return;
                if (__instance.MyScreenHubBase.OptionalShipInfo != null)
                {
                    if (!__instance.MyScreenHubBase.OptionalShipInfo.GetIsPlayerShip())
                    {
                        if (CrewLogManager.Instance.GetObjectsForScreen(__instance).LogInfoPanel.gameObject.activeSelf)
                            CrewLogManager.Instance.GetObjectsForScreen(__instance).LogInfoPanel.gameObject.SetActive(false);
                        if (CrewLogManager.Instance.GetObjectsForScreen(__instance).LogPanel.gameObject.activeSelf)
                        {
                            CrewLogManager.Instance.GetObjectsForScreen(__instance).LogPanel.gameObject.SetActive(false);
                            ___StatusPanel.gameObject.SetActive(true);
                            ___EnemyStatusPanel.gameObject.SetActive(true);
                        }
                    }
                }
                if (!CrewLogManager.Instance.GetObjectsForScreen(__instance).LogPanel.gameObject.activeSelf)
                    return;
                CrewLogScreenObjects screenObjects = CrewLogManager.Instance.GetObjectsForScreen(__instance);
                if (PLServer.GetCurrentSector() != null)
                    screenObjects.SectorLabel.text = "Sector: " + PLServer.GetCurrentSector().ID;
                else
                    screenObjects.SectorLabel.text = "Sector: N/A";
                if (PLServer.Instance != null)
                    screenObjects.TimeLabel.text = "Time: " + CrewLogManager.FormatPlaytime(PLServer.Instance.Playtime);
                int count = CrewLogManager.Instance.GetLogs().Count;
                for (int i = 0; i < 10; i++)
                {
                    int index = i + 10 * CrewLogManager.Instance.ShowLogIndex;
                    if (index < count)
                    {
                        screenObjects.LogButtons[i].GetComponentInChildren<UILabel>().text = CrewLogManager.FormatPlaytime(CrewLogManager.Instance.GetLogs()[index].timeStamp);
                        screenObjects.LogButtons[i].gameObject.SetActive(true);
                        screenObjects.LogColors[i].color = CrewLogManager.Instance.GetLogs()[index].optionalColor;
                        screenObjects.LogColors[i].gameObject.SetActive(true);

                    }
                    else
                    {
                        screenObjects.LogButtons[i].gameObject.SetActive(false);
                        screenObjects.LogColors[i].gameObject.SetActive(false);
                    }
                }
                if (count > 10 + CrewLogManager.Instance.ShowLogIndex * 10)
                {
                    screenObjects.NextButton.gameObject.SetActive(true);
                }
                else
                {
                    screenObjects.NextButton.gameObject.SetActive(false);
                    if (count < CrewLogManager.Instance.ShowLogIndex * 10)
                        --CrewLogManager.Instance.ShowLogIndex;
                }
                if (CrewLogManager.Instance.ShowLogIndex > 0)
                    screenObjects.BackButton.gameObject.SetActive(true);
                else
                    screenObjects.BackButton.gameObject.SetActive(false);
                if (CrewLogManager.Instance.GetObjectsForScreen(__instance).LogInfoPanel.gameObject.activeSelf)
                {
                    if (CrewLogManager.Instance.TempData.optionalSectorID != -1)
                        screenObjects.LogInfoBoxSectorButton.gameObject.SetActive(true);
                    if (CrewLogManager.Instance.LogIndex == int.MinValue)
                        screenObjects.LogInfoBoxText.text = CrewLogManager.Instance.TempData.Text;
                }
            }
        }

        internal class ClientSendLog : ModMessage
        {
            public override void HandleRPC(object[] arguments, PhotonMessageInfo sender)
            {
                CrewLogData logData = new CrewLogData()
                {
                    Text = (string)arguments[0],
                    timeStamp = (float)arguments[1],
                    optionalSectorID = (int)arguments[2],
                    optionalColor = new Color((float)arguments[3], (float)arguments[4], (float)arguments[5], (float)arguments[6]),
                    specialData = -1
                };
                CrewLogManager.Instance.AddLog(logData);
                PLPlayer player = Systems.GetPlayerFromPhotonPlayer(sender.sender);
                if (player != null)
                    PLServer.Instance.AddNotificationLocalize("[PL] added a crew log", player.GetPlayerID(), (PLServer.Instance.GetEstimatedServerMs() + 6000), false);

                List<object> sendArgumentList = new List<object>();
                int logCount = CrewLogManager.Instance.GetLogs().Count;
                sendArgumentList.Add(logCount);
                foreach (CrewLogData sendLogData in CrewLogManager.Instance.GetLogs())
                {
                    sendArgumentList.Add(sendLogData.Text);
                    sendArgumentList.Add(sendLogData.timeStamp);
                    sendArgumentList.Add(sendLogData.optionalSectorID);
                    sendArgumentList.Add(sendLogData.optionalColor.r);
                    sendArgumentList.Add(sendLogData.optionalColor.g);
                    sendArgumentList.Add(sendLogData.optionalColor.b);
                    sendArgumentList.Add(sendLogData.optionalColor.a);
                    sendArgumentList.Add(sendLogData.specialData);
                }
                ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.ServerSendLog", PhotonTargets.Others, sendArgumentList.ToArray());
            }
        }

        internal class ServerSendLog : ModMessage
        {
            public override void HandleRPC(object[] arguments, PhotonMessageInfo sender)
            {
                try
                {
                    int logCount = (int)arguments[0];
                    Debug.Log("[ExGal] Log Count Recieved: " + logCount);
                    Debug.Log("[ExGal] Argument Count Recieved: " + arguments.Length);
                    CrewLogManager.Instance.ClearLogs();
                    int index = 1;
                    for (int i = 0; i < logCount; i++)
                    {
                        CrewLogData logData = new CrewLogData()
                        {
                            Text = (string)arguments[index++],
                            timeStamp = (float)arguments[index++],
                            optionalSectorID = (int)arguments[index++],
                            optionalColor = new Color((float)arguments[index++], (float)arguments[index++], (float)arguments[index++], (float)arguments[index++]),
                            specialData = (int)arguments[index++]
                        };
                        Debug.Log("[ExGal] Log Added With Sector ID: " + logData.optionalSectorID);
                        CrewLogManager.Instance.AddLog(logData);
                    }
                }
                catch (Exception e)
                {
                    Debug.Log("[ExGal] Exception in ServerSendLog:");
                    Debug.Log(e.ToString());
                    Debug.Log("------------------------------");
                    Debug.Log(e.Source);
                    Debug.Log("------------------------------");
                    Debug.Log(e.Message);
                    Debug.Log("------------------------------");
                    Debug.Log(e.StackTrace);
                    Debug.Log("[ExGal] End of Exception Info");
                }
            }
        }

        internal static void NewPlayerSendLogs(PhotonPlayer newPhotonPlayer, string inPlayerName)
        {
            if (!newPhotonPlayer.IsMasterClient)
            {
                List<object> sendArgumentList = new List<object>();
                int logCount = CrewLogManager.Instance.GetLogs().Count;
                sendArgumentList.Add(logCount);
                foreach (CrewLogData sendLogData in CrewLogManager.Instance.GetLogs())
                {
                    sendArgumentList.Add(sendLogData.Text);
                    sendArgumentList.Add(sendLogData.timeStamp);
                    sendArgumentList.Add(sendLogData.optionalSectorID);
                    sendArgumentList.Add(sendLogData.optionalColor.r);
                    sendArgumentList.Add(sendLogData.optionalColor.g);
                    sendArgumentList.Add(sendLogData.optionalColor.b);
                    sendArgumentList.Add(sendLogData.optionalColor.a);
                    sendArgumentList.Add(sendLogData.specialData);
                }
                ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.ServerSendLog", newPhotonPlayer, sendArgumentList.ToArray());
            }
        }
    }
}