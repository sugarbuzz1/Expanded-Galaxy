using HarmonyLib;
using System;
using System.Collections.Generic;
using UnityEngine;
using static ExpandedGalaxy.Relic;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLWarpStationScreen), "OnButtonClick")]
    internal class ClickStargateScreen
    {
        internal static List<char> currentInput = new List<char>();
        private static bool Prefix(PLWarpStationScreen __instance, UIWidget inButton, ref List<UILabel> ___AllLabels, ref List<UIWidget> ___AllButtons)
        {
            if (PLServer.GetCurrentSector() != null && PLServer.GetCurrentSector().VisualIndication == ESectorVisualIndication.WARP_NETWORK_STATION && PLServer.GetCurrentSector().MySPI != null && PLServer.GetCurrentSector().MySPI.Faction == 6)
            {
                if (inButton.name.Contains("inputOption") && currentInput.Count < 5)
                {
                    int chIndex = Int32.Parse(inButton.name.Split('_')[1]);
                    if (currentInput.Count == 0 && chIndex > 19)
                        return false;
                    __instance.PlaySoundEventOnAllClonedScreens("play_ship_generic_internal_computer_ui_click");
                    currentInput.Add(StargatePuzzle.SubLetters[chIndex]);
                    string input = string.Empty;
                    for (int i = 0; i < currentInput.Count; i++)
                    {
                        input += currentInput[i];
                        if (i + 1 < currentInput.Count)
                            input += " ";
                    }
                    ___AllLabels[SetupStargateScreen.inputLabel].text = input;
                }
                else if (inButton.name == "backspace" && currentInput.Count > 0)
                {
                    __instance.PlaySoundEventOnAllClonedScreens("play_ship_generic_internal_computer_ui_keypad");
                    currentInput.RemoveAt(currentInput.Count - 1);
                    string input = string.Empty;
                    for (int i = 0; i < currentInput.Count; i++)
                    {
                        input += currentInput[i];
                        if (i + 1 < currentInput.Count)
                            input += " ";
                    }
                    ___AllLabels[SetupStargateScreen.inputLabel].text = input;
                }
                else if (inButton.name == "enter")
                {
                    __instance.PlaySoundEventOnAllClonedScreens("play_ship_generic_internal_computer_ui_click");
                    if (currentInput.Count < 5)
                    {
                        ___AllLabels[SetupStargateScreen.targetLabel].text = "TARGET: INVALID INPUT";
                        ___AllLabels[SetupStargateScreen.targetLabel].color = Color.red;
                        ___AllLabels[SetupStargateScreen.statusLabel].text = "STATUS:";
                        __instance.MyWarpStation.photonView.RPC("SetTargetedSectorID", PhotonTargets.MasterClient, (object)-1, (object)true);
                    }
                    else
                    {
                        string input = string.Empty;
                        for (int i = 0; i < currentInput.Count; i++)
                        {
                            input += currentInput[i];
                        }
                        Vector3 vector3 = StargatePuzzle.VectorFromCode(input);
                        ___AllLabels[SetupStargateScreen.targetLabel].text = "TARGET: <" + vector3.x.ToString("0.####") + ", " + vector3.y.ToString("0.####") + ", " + vector3.z.ToString("0.####") + ">";
                        if (input == SetupStargateScreen.currentSolution)
                        {
                            ___AllLabels[SetupStargateScreen.statusLabel].text = "STATUS: VALID PATH FOUND";
                            ___AllLabels[SetupStargateScreen.statusLabel].color = Color.green;
                            PLSectorInfo info = PLGlobal.Instance.Galaxy.GetSectorOfVisualIndication(ESectorVisualIndication.GREY_PLAINS);
                            if (info != null)
                                __instance.MyWarpStation.photonView.RPC("SetTargetedSectorID", PhotonTargets.MasterClient, (object)info.ID, (object)true);
                        }
                        else
                        {
                            ___AllLabels[SetupStargateScreen.statusLabel].text = "STATUS: INVALID PATH";
                            ___AllLabels[SetupStargateScreen.statusLabel].color = Color.red;
                            __instance.MyWarpStation.photonView.RPC("SetTargetedSectorID", PhotonTargets.MasterClient, (object)-1, (object)true);
                        }
                    }
                }
                return false;
            }
            return true;
        }
    }
}
