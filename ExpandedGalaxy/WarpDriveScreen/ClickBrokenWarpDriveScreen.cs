using HarmonyLib;
using PulsarModLoader.Content.Components.WarpDrive;
using System.Collections.Generic;
using UnityEngine;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLWarpDriveScreen), "OnButtonClick")]
    internal class ClickBrokenWarpDriveScreen
    {
        internal static List<char> currentInput = new List<char>();

        private static bool Prefix(
          PLWarpDriveScreen __instance,
          UIWidget inButton,
          ref List<UILabel> ___AllLabels,
          ref List<UIWidget> ___AllButtons)
        {
            if ((UnityEngine.Object)__instance.MyScreenHubBase != (UnityEngine.Object)null && (UnityEngine.Object)__instance.MyScreenHubBase.OptionalShipInfo != (UnityEngine.Object)null && __instance.MyScreenHubBase.OptionalShipInfo.GetIsPlayerShip() && __instance.MyScreenHubBase.OptionalShipInfo.MyWarpDrive != null && __instance.MyScreenHubBase.OptionalShipInfo.MyWarpDrive.SubType == WarpDriveModManager.Instance.GetWarpDriveIDFromName("Broken Warp Drive"))
            {
                if (__instance.MyScreenHubBase.OptionalShipInfo.MyWarpDrive.Level == 0)
                    return false;
                if (inButton.name.Contains("inputOption"))
                {
                    if (currentInput.Count < 5)
                    {
                        int index1 = int.Parse(inButton.name.Split('_')[1]);
                        if (currentInput.Count == 0 && index1 > 19)
                            return false;
                        __instance.PlaySoundEventOnAllClonedScreens("play_ship_generic_internal_computer_ui_click");
                        currentInput.Add(StargatePuzzle.SubLetters[index1]);
                        string empty = string.Empty;
                        for (int index2 = 0; index2 < currentInput.Count; ++index2)
                        {
                            empty += currentInput[index2].ToString();
                            if (index2 + 1 < currentInput.Count)
                                empty += " ";
                        }
                        ___AllLabels[SetupBrokenDriveScreen.inputLabel].text = empty;
                    }
                    return false;
                }
                if (inButton.name == "backspace")
                {
                    if (currentInput.Count > 0)
                    {
                        __instance.PlaySoundEventOnAllClonedScreens("play_ship_generic_internal_computer_ui_keypad");
                        currentInput.RemoveAt(currentInput.Count - 1);
                        string empty = string.Empty;
                        for (int index = 0; index < currentInput.Count; ++index)
                        {
                            empty += currentInput[index].ToString();
                            if (index + 1 < currentInput.Count)
                                empty += " ";
                        }
                        ___AllLabels[SetupBrokenDriveScreen.inputLabel].text = empty;
                    }
                    return false;
                }
                if (inButton.name == "enter")
                {
                    __instance.PlaySoundEventOnAllClonedScreens("play_ship_generic_internal_computer_ui_click");
                    if (currentInput.Count < 5)
                    {
                        ___AllLabels[SetupBrokenDriveScreen.targetLabel].text = "TARGET: INVALID INPUT";
                        ___AllLabels[SetupBrokenDriveScreen.targetLabel].color = Color.red;
                        ___AllLabels[SetupBrokenDriveScreen.statusLabel].text = "STATUS:";
                    }
                    else
                    {
                        string empty = string.Empty;
                        for (int index = 0; index < currentInput.Count; ++index)
                            empty += currentInput[index].ToString();
                        Vector3 vector3 = StargatePuzzle.VectorFromCode(empty);
                        ___AllLabels[SetupBrokenDriveScreen.targetLabel].text = "TARGET: <" + vector3.x.ToString("0.####") + ", " + vector3.y.ToString("0.####") + ", " + vector3.z.ToString("0.####") + ">";
                        ___AllLabels[SetupBrokenDriveScreen.statusLabel].text = "STATUS: ERROR";
                        ___AllLabels[SetupBrokenDriveScreen.statusLabel].color = Color.red;
                    }
                    return false;
                }
                if (inButton.name == "reverse")
                {
                    __instance.PlaySoundEventOnAllClonedScreens("play_ship_generic_internal_computer_ui_click");
                    if (currentInput.Count > 0)
                    {
                        string code = string.Empty;
                        for (int index = 0; index < 5; ++index)
                            code = currentInput.Count <= index ? code + "§" : code + currentInput[index].ToString();
                        char[] charArray = StargatePuzzle.Solve(StargatePuzzle.VectorFromCode(code) * -1f).ToCharArray();
                        string empty = string.Empty;
                        for (int index = 0; index < currentInput.Count; ++index)
                        {
                            empty += charArray[index].ToString();
                            currentInput[index] = charArray[index];
                            if (index + 1 < currentInput.Count)
                                empty += " ";
                        }
                        ___AllLabels[SetupBrokenDriveScreen.inputLabel].text = empty;
                    }
                    return false;
                }
            }
            return true;
        }
    }
}