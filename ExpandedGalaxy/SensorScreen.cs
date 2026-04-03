
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace ExpandedGalaxy
{
    internal class SensorScreen
    {
        [HarmonyPatch(typeof(PLScientistSensorScreen), "SetupUI")]
        internal class SetupSensorUI
        {
            private static void Postfix(PLScientistSensorScreen __instance)
            {
                Traverse traverse = Traverse.Create(__instance);
                UISprite InfoBoxBG = traverse.Field("InfoBoxBG").GetValue<UISprite>();
                for (int i = 0; i < 4; i++)
                {
                    string name = "compScanOption_" + i.ToString();
                    object[] params1 = new object[7]
                    {
                        name,
                        string.Empty,
                        new Vector3(-190f, 34f - 52f * i),
                        new Vector2(185f, 50f),
                        new Color(0.65f, 0.65f, 0.65f),
                        InfoBoxBG.transform,
                        UIWidget.Pivot.TopLeft
                    };
                    UISprite button = traverse.Method("CreateButton", new Type[7] { typeof(string), typeof(string), typeof(Vector3), typeof(Vector2), typeof(Color), typeof(Transform), typeof(UIWidget.Pivot) }).GetValue<UISprite>(params1);
                    button.depth += 10000;
                    button.GetComponentInChildren<UILabel>().depth += 10000;
                    button.GetComponentInChildren<UILabel>().fontSize /= 2;
                    button.GetComponentInChildren<UILabel>().width = 600;
                    button.GetComponentInChildren<UILabel>().overflowMethod = UILabel.Overflow.ResizeHeight;
                }
                for (int i = 0; i < 4; i++)
                {
                    string name = "compScanOption_" + (i + 4).ToString();
                    object[] params1 = new object[7]
                    {
                        name,
                        string.Empty,
                        new Vector3(5f, 34f - 52f * i),
                        new Vector2(185f, 50f),
                        new Color(0.65f, 0.65f, 0.65f),
                        InfoBoxBG.transform,
                        UIWidget.Pivot.TopLeft
                    };
                    UISprite button = traverse.Method("CreateButton", new Type[7] { typeof(string), typeof(string), typeof(Vector3), typeof(Vector2), typeof(Color), typeof(Transform), typeof(UIWidget.Pivot) }).GetValue<UISprite>(params1);
                    button.depth += 10000;
                    button.GetComponentInChildren<UILabel>().depth += 10000;
                    button.GetComponentInChildren<UILabel>().fontSize /= 2;
                    button.GetComponentInChildren<UILabel>().width = 600;
                    button.GetComponentInChildren<UILabel>().overflowMethod = UILabel.Overflow.ResizeHeight;
                }
                traverse.Field("InfoBoxText").GetValue<UILabel>().fontSize = 60;

                object[] params2 = new object[7]
                {
                        "compScanNext",
                        ">>",
                        new Vector3(140f, 138f),
                        new Vector2(50f, 50f),
                        new Color(0.65f, 0.65f, 0.65f),
                        InfoBoxBG.transform,
                        UIWidget.Pivot.TopLeft
                };
                UISprite button1 = traverse.Method("CreateButton", new Type[7] { typeof(string), typeof(string), typeof(Vector3), typeof(Vector2), typeof(Color), typeof(Transform), typeof(UIWidget.Pivot) }).GetValue<UISprite>(params2);
                button1.depth += 10000;
                button1.GetComponentInChildren<UILabel>().depth += 10000;

                params2 = new object[7]
                {
                        "compScanBack",
                        "<<",
                        new Vector3(140f, 86f),
                        new Vector2(50f, 50f),
                        new Color(0.65f, 0.65f, 0.65f),
                        InfoBoxBG.transform,
                        UIWidget.Pivot.TopLeft
                };
                button1 = traverse.Method("CreateButton", new Type[7] { typeof(string), typeof(string), typeof(Vector3), typeof(Vector2), typeof(Color), typeof(Transform), typeof(UIWidget.Pivot) }).GetValue<UISprite>(params2);
                button1.depth += 10000;
                button1.GetComponentInChildren<UILabel>().depth += 10000;
            }
        }

        [HarmonyPatch(typeof(PLScientistSensorScreen), "Update")]
        internal class SensorScreenUpdate
        {
            private static void Postfix(PLScientistSensorScreen __instance, ref bool ___IsInfoBoxActive, ref UISprite ___InfoBoxBG, ref UILabel ___InfoBoxText, ref UILabel ___InfoBoxTitle, ref UISprite ___InfoBoxCloseButton, ref bool ___InfoBox_IsTopMsg)
            {
                if (!__instance.UIIsSetup() || !__instance.LocalPlayerInSameLocation())
                    return;
                if (___IsInfoBoxActive)
                {
                    if (___InfoBoxText.text == string.Empty || ___InfoBoxText.text.ToLower().Contains("none"))
                    {
                        ___InfoBoxBG.height = 400;
                        float num = (float)___InfoBoxBG.height * 0.5f;
                        ___InfoBoxTitle.transform.localPosition = new Vector3(-180, num - 10f);
                        ___InfoBoxText.transform.localPosition = new Vector3(-160, num - 50f);
                        ___InfoBoxCloseButton.transform.localPosition = new Vector3(140f, num - 10f);
                        if (___InfoBox_IsTopMsg)
                            num = 215f;
                        ___InfoBoxBG.transform.localPosition = new Vector3(0f, num - (float)___InfoBoxBG.height * 0.5f);
                    }
                    else
                    {
                        ___InfoBoxBG.height = 50 + Mathf.RoundToInt((float) ___InfoBoxText.height * 0.25f);
                        float num = (float)___InfoBoxBG.height * 0.5f;
                        ___InfoBoxTitle.transform.localPosition = new Vector3(-180, num - 10f);
                        ___InfoBoxText.transform.localPosition = new Vector3(-180, num - 50f);
                        ___InfoBoxCloseButton.transform.localPosition = new Vector3(140f, num - 10f);
                        if (___InfoBox_IsTopMsg)
                            num = 215f;
                        ___InfoBoxBG.transform.localPosition = new Vector3(0f, num - (float)___InfoBoxBG.height * 0.5f);
                    }
                }
            }
        }

        [HarmonyPatch(typeof(PLScientistSensorScreen), "OnButtonClick")]
        internal class SensorScreenButtonClick
        {
            internal static List<PLShipComponent> cachedComponentScan = new List<PLShipComponent>();
            internal static int pageIndex = 0;
            private static bool Prefix(PLScientistSensorScreen __instance, UIWidget inButton, ref PLShipInfoBase ___TargetShip, ref bool ___IsInfoBoxActive, ref UISprite ___InfoBoxBG)
            {
                if (!(inButton != null && inButton.gameObject.activeSelf && inButton.gameObject.activeInHierarchy && (double)inButton.finalAlpha > 0.1))
                    return false;
                if (__instance.MyScreenHubBase.OptionalShipInfo.IsActiveScanInProgress())
                {
                    return true;
                }
                else
                {
                    if (inButton.name == null)
                        return false;
                    switch (inButton.name)
                    {
                        case "compScan_Reactor":
                            if (___IsInfoBoxActive)
                                break;
                            __instance.PlaySoundEventOnAllClonedScreens("play_ship_generic_internal_computer_ui_click");
                            if (___TargetShip == null)
                                break;
                            float num1 = 0.0f;
                            if (__instance.MyScreenHubBase.OptionalShipInfo != null)
                                num1 = ___TargetShip.MySensorObjectShip.GetDetectionSignal(Vector3.SqrMagnitude(___TargetShip.Exterior.transform.position - __instance.MyScreenHubBase.OptionalShipInfo.Exterior.transform.position), ___TargetShip.MyStats.EMSignature, __instance.MyScreenHubBase.OptionalShipInfo.MyStats.EMDetection, (PLShipInfoBase)__instance.MyScreenHubBase.OptionalShipInfo, (PLSensorObject)___TargetShip.MySensorObjectShip);
                            if ((double)num1 <= 10.0)
                                break;
                            cachedComponentScan.Clear();
                            pageIndex = 0;
                            cachedComponentScan.AddRange(___TargetShip.MyStats.GetComponentsOfType(ESlotType.E_COMP_REACTOR));
                            SetupComponentScanOptions(__instance, cachedComponentScan, pageIndex);
                            if (cachedComponentScan.Count > 0)
                                __instance.SetInfoBox("Reactor Scan", string.Empty, true);
                            else
                                __instance.SetInfoBox("Reactor Scan", "None\n", true);
                            break;
                        case "compScan_Cargo":
                            if (___IsInfoBoxActive)
                                break;
                            __instance.PlaySoundEventOnAllClonedScreens("play_ship_generic_internal_computer_ui_click");
                            if (___TargetShip == null)
                                break;
                            float num2 = 0.0f;
                            if (__instance.MyScreenHubBase.OptionalShipInfo != null)
                                num2 = ___TargetShip.MySensorObjectShip.GetDetectionSignal(Vector3.SqrMagnitude(___TargetShip.Exterior.transform.position - __instance.MyScreenHubBase.OptionalShipInfo.Exterior.transform.position), ___TargetShip.MyStats.EMSignature, __instance.MyScreenHubBase.OptionalShipInfo.MyStats.EMDetection, (PLShipInfoBase)__instance.MyScreenHubBase.OptionalShipInfo, (PLSensorObject)___TargetShip.MySensorObjectShip);
                            if ((double)num2 <= 10.0)
                                break;
                            cachedComponentScan.Clear();
                            pageIndex = 0;
                            cachedComponentScan.AddRange(___TargetShip.MyStats.GetComponentsOfType(ESlotType.E_COMP_CARGO, true));
                            SetupComponentScanOptions(__instance, cachedComponentScan, pageIndex);
                            if (cachedComponentScan.Count > 0)
                                __instance.SetInfoBox("Cargo Scan", string.Empty, true);
                            else
                                __instance.SetInfoBox("Cargo Scan", "None\n", true);
                            break;
                        case "compScan_Hull":
                            if (___IsInfoBoxActive)
                                break;
                            __instance.PlaySoundEventOnAllClonedScreens("play_ship_generic_internal_computer_ui_click");
                            if (___TargetShip == null)
                                break;
                            float num3 = 0.0f;
                            if (__instance.MyScreenHubBase.OptionalShipInfo != null)
                                num3 = ___TargetShip.MySensorObjectShip.GetDetectionSignal(Vector3.SqrMagnitude(___TargetShip.Exterior.transform.position - __instance.MyScreenHubBase.OptionalShipInfo.Exterior.transform.position), ___TargetShip.MyStats.EMSignature, __instance.MyScreenHubBase.OptionalShipInfo.MyStats.EMDetection, (PLShipInfoBase)__instance.MyScreenHubBase.OptionalShipInfo, (PLSensorObject)___TargetShip.MySensorObjectShip);
                            if ((double)num3 <= 10.0)
                                break;
                            cachedComponentScan.Clear();
                            pageIndex = 0;
                            cachedComponentScan.AddRange(___TargetShip.MyStats.GetComponentsOfType(ESlotType.E_COMP_HULL));
                            cachedComponentScan.AddRange(___TargetShip.MyStats.GetComponentsOfType(ESlotType.E_COMP_HULLPLATING));
                            SetupComponentScanOptions(__instance, cachedComponentScan, pageIndex);
                            if (cachedComponentScan.Count > 0)
                                __instance.SetInfoBox("Hull Scan", string.Empty, true);
                            else
                                __instance.SetInfoBox("Hull Scan", "None\n", true);
                            break;
                        case "compScan_Processor":
                            if (___IsInfoBoxActive)
                                break;
                            __instance.PlaySoundEventOnAllClonedScreens("play_ship_generic_internal_computer_ui_click");
                            if (___TargetShip == null)
                                break;
                            float num4 = 0.0f;
                            if (__instance.MyScreenHubBase.OptionalShipInfo != null)
                                num4 = ___TargetShip.MySensorObjectShip.GetDetectionSignal(Vector3.SqrMagnitude(___TargetShip.Exterior.transform.position - __instance.MyScreenHubBase.OptionalShipInfo.Exterior.transform.position), ___TargetShip.MyStats.EMSignature, __instance.MyScreenHubBase.OptionalShipInfo.MyStats.EMDetection, (PLShipInfoBase)__instance.MyScreenHubBase.OptionalShipInfo, (PLSensorObject)___TargetShip.MySensorObjectShip);
                            if ((double)num4 <= 10.0)
                                break;
                            cachedComponentScan.Clear();
                            pageIndex = 0;
                            cachedComponentScan.AddRange(___TargetShip.MyStats.GetComponentsOfType(ESlotType.E_COMP_CPU));
                            SetupComponentScanOptions(__instance, cachedComponentScan, pageIndex);
                            if (cachedComponentScan.Count > 0)
                                __instance.SetInfoBox("CPU Scan", string.Empty, true);
                            else
                                __instance.SetInfoBox("CPU Scan", "None\n", true);
                            break;
                        case "compScan_Shld":
                            if (___IsInfoBoxActive)
                                break;
                            __instance.PlaySoundEventOnAllClonedScreens("play_ship_generic_internal_computer_ui_click");
                            if (___TargetShip == null)
                                break;
                            float num5 = 0.0f;
                            if (__instance.MyScreenHubBase.OptionalShipInfo != null)
                                num5 = ___TargetShip.MySensorObjectShip.GetDetectionSignal(Vector3.SqrMagnitude(___TargetShip.Exterior.transform.position - __instance.MyScreenHubBase.OptionalShipInfo.Exterior.transform.position), ___TargetShip.MyStats.EMSignature, __instance.MyScreenHubBase.OptionalShipInfo.MyStats.EMDetection, (PLShipInfoBase)__instance.MyScreenHubBase.OptionalShipInfo, (PLSensorObject)___TargetShip.MySensorObjectShip);
                            if ((double)num5 <= 10.0)
                                break;
                            cachedComponentScan.Clear();
                            pageIndex = 0;
                            cachedComponentScan.AddRange(___TargetShip.MyStats.GetComponentsOfType(ESlotType.E_COMP_SHLD));
                            SetupComponentScanOptions(__instance, cachedComponentScan, pageIndex);
                            if (cachedComponentScan.Count > 0)
                                __instance.SetInfoBox("Shield Scan", string.Empty, true);
                            else
                                __instance.SetInfoBox("Shield Scan", "None\n", true);
                            break;
                        case "compScan_Misc":
                            if (___IsInfoBoxActive)
                                break;
                            __instance.PlaySoundEventOnAllClonedScreens("play_ship_generic_internal_computer_ui_click");
                            if (___TargetShip == null)
                                break;
                            float num6 = 0.0f;
                            if (__instance.MyScreenHubBase.OptionalShipInfo != null)
                                num6 = ___TargetShip.MySensorObjectShip.GetDetectionSignal(Vector3.SqrMagnitude(___TargetShip.Exterior.transform.position - __instance.MyScreenHubBase.OptionalShipInfo.Exterior.transform.position), ___TargetShip.MyStats.EMSignature, __instance.MyScreenHubBase.OptionalShipInfo.MyStats.EMDetection, (PLShipInfoBase)__instance.MyScreenHubBase.OptionalShipInfo, (PLSensorObject)___TargetShip.MySensorObjectShip);
                            if ((double)num6 <= 10.0)
                                break;
                            cachedComponentScan.Clear();
                            pageIndex = 0;
                            cachedComponentScan.AddRange(___TargetShip.MyStats.GetComponentsOfType(ESlotType.E_COMP_WARP));
                            cachedComponentScan.AddRange(___TargetShip.MyStats.GetComponentsOfType(ESlotType.E_COMP_SENS));
                            cachedComponentScan.AddRange(___TargetShip.MyStats.GetComponentsOfType(ESlotType.E_COMP_THRUSTER));
                            cachedComponentScan.AddRange(___TargetShip.MyStats.GetComponentsOfType(ESlotType.E_COMP_MANEUVER_THRUSTER));
                            cachedComponentScan.AddRange(___TargetShip.MyStats.GetComponentsOfType(ESlotType.E_COMP_INERTIA_THRUSTER));
                            SetupComponentScanOptions(__instance, cachedComponentScan, pageIndex);
                            if (cachedComponentScan.Count > 0)
                                __instance.SetInfoBox("Misc Scan", string.Empty, true);
                            else
                                __instance.SetInfoBox("Misc Scan", "None\n", true);
                            break;
                        case "compScan_Programs":
                            if (___IsInfoBoxActive)
                                break;
                            __instance.PlaySoundEventOnAllClonedScreens("play_ship_generic_internal_computer_ui_click");
                            if (___TargetShip == null)
                                break;
                            float num7 = 0.0f;
                            if (__instance.MyScreenHubBase.OptionalShipInfo != null)
                                num7 = ___TargetShip.MySensorObjectShip.GetDetectionSignal(Vector3.SqrMagnitude(___TargetShip.Exterior.transform.position - __instance.MyScreenHubBase.OptionalShipInfo.Exterior.transform.position), ___TargetShip.MyStats.EMSignature, __instance.MyScreenHubBase.OptionalShipInfo.MyStats.EMDetection, (PLShipInfoBase)__instance.MyScreenHubBase.OptionalShipInfo, (PLSensorObject)___TargetShip.MySensorObjectShip);
                            if ((double)num7 <= 10.0)
                                break;
                            cachedComponentScan.Clear();
                            pageIndex = 0;
                            cachedComponentScan.AddRange(___TargetShip.MyStats.GetComponentsOfType(ESlotType.E_COMP_PROGRAM));
                            SetupComponentScanOptions(__instance, cachedComponentScan, pageIndex);
                            if (cachedComponentScan.Count > 0)
                                __instance.SetInfoBox("Program Scan", string.Empty, true);
                            else
                                __instance.SetInfoBox("Program Scan", "None\n", true);
                            break;
                        case "compScan_Turret":
                            if (___IsInfoBoxActive)
                                break;
                            __instance.PlaySoundEventOnAllClonedScreens("play_ship_generic_internal_computer_ui_click");
                            if (___TargetShip == null)
                                break;
                            float num8 = 0.0f;
                            if (__instance.MyScreenHubBase.OptionalShipInfo != null)
                                num8 = ___TargetShip.MySensorObjectShip.GetDetectionSignal(Vector3.SqrMagnitude(___TargetShip.Exterior.transform.position - __instance.MyScreenHubBase.OptionalShipInfo.Exterior.transform.position), ___TargetShip.MyStats.EMSignature, __instance.MyScreenHubBase.OptionalShipInfo.MyStats.EMDetection, (PLShipInfoBase)__instance.MyScreenHubBase.OptionalShipInfo, (PLSensorObject)___TargetShip.MySensorObjectShip);
                            if ((double)num8 <= 10.0)
                                break;
                            cachedComponentScan.Clear();
                            pageIndex = 0;
                            cachedComponentScan.AddRange(___TargetShip.MyStats.GetComponentsOfType(ESlotType.E_COMP_MAINTURRET));
                            cachedComponentScan.AddRange(___TargetShip.MyStats.GetComponentsOfType(ESlotType.E_COMP_TURRET));
                            cachedComponentScan.AddRange(___TargetShip.MyStats.GetComponentsOfType(ESlotType.E_COMP_AUTO_TURRET));
                            cachedComponentScan.AddRange(___TargetShip.MyStats.GetComponentsOfType(ESlotType.E_COMP_TRACKERMISSILE));
                            cachedComponentScan.AddRange(___TargetShip.MyStats.GetComponentsOfType(ESlotType.E_COMP_NUCLEARDEVICE));
                            SetupComponentScanOptions(__instance, cachedComponentScan, pageIndex);
                            if (cachedComponentScan.Count > 0)
                                __instance.SetInfoBox("Weapon Scan", string.Empty, true);
                            else
                                __instance.SetInfoBox("Weapon Scan", "None\n", true);
                            break;
                        case "compScanOption_0":
                        case "compScanOption_1":
                        case "compScanOption_2":
                        case "compScanOption_3":
                        case "compScanOption_4":
                        case "compScanOption_5":
                        case "compScanOption_6":
                        case "compScanOption_7":
                            __instance.PlaySoundEventOnAllClonedScreens("play_ship_generic_internal_computer_ui_click");
                            string[] nameNumber = inButton.name.Split('_');
                            int scanOption = Int32.Parse(nameNumber[1]);
                            if (!(cachedComponentScan.Count > scanOption + pageIndex * 8))
                                break;
                            ToggleButtons(__instance);
                            PLShipComponent component = cachedComponentScan[scanOption + pageIndex * 8];
                            if (component == null)
                                break;
                            if (PLGlobal.GetColorBGForWare(component) != Color.white)
                                ___InfoBoxBG.color = PLGlobal.GetColorBGForWare(component);
                            StringBuilder builder = new StringBuilder();
                            builder.AppendLine();
                            builder.AppendLine(component.Desc);
                            builder.AppendLine();
                            builder.AppendLine();
                            string[] leftLines = component.GetStatLineLeft().Split('\n');
                            string[] rightLines = component.GetStatLineRight().Split('\n');
                            string[] extraLeftLines = component.GetExtraLineLeft().Split('\n');
                            string[] extraRightLines = component.GetExtraLineRight().Split('\n');
                            for (int i = 0; i < Math.Max(leftLines.Length, extraLeftLines.Length); i++)
                            {
                                string newLine;
                                if (i < leftLines.Length && leftLines[i] != string.Empty)
                                {
                                    double width = 0.0;
                                    foreach (char ch in leftLines[i].ToCharArray())
                                    {
                                        switch (ch)
                                        {
                                            case ' ':
                                            case '.':
                                            case ',':
                                                width += 0.5;
                                                break;
                                            default:
                                                width += 1.0;
                                                break;
                                        }
                                    }
                                    foreach (char ch in rightLines[i].ToCharArray())
                                    {
                                        switch (ch)
                                        {
                                            case ' ':
                                            case '.':
                                            case ',':
                                                width += 0.5;
                                                break;
                                            default:
                                                width += 1.0;
                                                break;
                                        }
                                    }
                                    newLine = leftLines[i];
                                    newLine += new string(' ', (int)((28.0 - width) * 2.0));
                                    newLine += rightLines[i];
                                }
                                else
                                    newLine = new string(' ', 56);
                                newLine += "     ";
                                if (i < extraLeftLines.Length && extraLeftLines[i] != string.Empty)
                                {
                                    double width = 0.0;
                                    foreach (char ch in extraLeftLines[i].ToCharArray())
                                    {
                                        switch (ch)
                                        {
                                            case ' ':
                                            case '.':
                                            case ',':
                                            case 't':
                                            case 'i':
                                            case 'I':
                                            case 'f':
                                            case 'j':
                                            case 'l':
                                            case 'L':
                                            case '1':
                                                width += 0.5;
                                                break;
                                            default:
                                                width += 1.0;
                                                break;
                                        }
                                    }
                                    foreach (char ch in extraRightLines[i].ToCharArray())
                                    {
                                        switch (ch)
                                        {
                                            case ' ':
                                            case '.':
                                            case ',':
                                            case 't':
                                            case 'i':
                                            case 'I':
                                            case 'f':
                                            case 'j':
                                            case 'l':
                                            case 'L':
                                            case '1':
                                                width += 0.5;
                                                break;
                                            default:
                                                width += 1.0;
                                                break;
                                        }
                                    }
                                    newLine += extraLeftLines[i];
                                    newLine += new string(' ', (int)((28.0 - width) * 2.0));
                                    newLine += extraRightLines[i];
                                }
                                builder.AppendLine(newLine);
                            }
                            __instance.SetInfoBox(component.Name, builder.ToString(), true);
                            break;
                        case "compScanNext":
                            __instance.PlaySoundEventOnAllClonedScreens("play_ship_generic_internal_computer_ui_click");
                            pageIndex++;
                            SetupComponentScanOptions(__instance, cachedComponentScan, pageIndex);
                            break;
                        case "compScanBack":
                            if (!(pageIndex > 0))
                                break;
                            __instance.PlaySoundEventOnAllClonedScreens("play_ship_generic_internal_computer_ui_click");
                            pageIndex--;
                            SetupComponentScanOptions(__instance, cachedComponentScan, pageIndex);
                            break;
                        case "CloseInfoBoxBtn":
                            ToggleButtons(__instance);
                            return true;
                        default:
                            ToggleButtons(__instance);
                            if (___IsInfoBoxActive)
                                return false;
                            return true;
                    }
                }
                return false;
            }

            private static void SetupComponentScanOptions(PLScientistSensorScreen screen, List<PLShipComponent> components, int pageIndex)
            {
                ToggleButtons(screen);
                Traverse traverse = Traverse.Create(screen);
                List<UIWidget> AllButtons = traverse.Field("AllButtons").GetValue<List<UIWidget>>();
                List<UIWidget> optionButtons = new List<UIWidget>();
                foreach (UIWidget button in AllButtons)
                {
                    if (button != null)
                    {
                        if (button.name.Contains("compScanOption"))
                            optionButtons.Add(button);
                        else if (button.name == "compScanNext")
                        {
                            if (components.Count > (pageIndex + 1) * 8)
                                button.gameObject.SetActive(true);
                            else
                                button.gameObject.SetActive(false);
                        }
                        else if (button.name == "compScanBack")
                        {
                            if (pageIndex > 0)
                                button.gameObject.SetActive(true);
                            else
                                button.gameObject.SetActive(false);
                        }
                        else
                        {
                            switch (button.name)
                            {
                                case "compScan_Reactor":
                                case "compScan_Cargo":
                                case "compScan_Hull":
                                case "compScan_Processor":
                                case "compScan_Shld":
                                case "compScan_Misc":
                                case "compScan_Programs":
                                case "compScan_Turret":
                                case "compScan_LF":
                                case "weaknessScanAttuneDmg":
                                case "weaknessScanAttuneDmg1":
                                case "weaknessScanAttuneDmg3":
                                case "weaknessScanCyberDef":
                                case "weaknessScanShieldWeakPoint":
                                case "weaknessScanReactor":
                                    button.gameObject.SetActive(false);
                                    break;                           
                            }
                        }
                    }
                }
                for (int i = 0; i < 8; i++)
                {
                    if (components.Count > i + pageIndex * 8)
                    {
                        optionButtons[i].GetComponentInChildren<UILabel>().text = components[i + pageIndex * 8].Name;
                        optionButtons[i].gameObject.SetActive(true);
                        Color color = PLGlobal.GetColorBGForWare(components[i + pageIndex * 8]) * 0.5f;
                        color.a = 1f;
                        optionButtons[i].color = color;
                        optionButtons[i].GetComponentInChildren<UILabel>().color = optionButtons[i].color;
                    }
                }
            }

            private static void ToggleButtons(PLScientistSensorScreen screen)
            {
                Traverse traverse = Traverse.Create(screen);
                List<UIWidget> AllButtons = traverse.Field("AllButtons").GetValue<List<UIWidget>>();
                UISprite InfoBoxBG = traverse.Field("InfoBoxBG").GetValue<UISprite>();
                InfoBoxBG.color = new Color(0.65f, 0.65f, 0.65f);
                foreach (UIWidget button in AllButtons)
                {
                    if (button.name.Contains("compScanOption"))
                    {
                        button.gameObject.SetActive(false);
                    }
                    else if (button.name == "compScanNext" || button.name == "compScanBack")
                        button.gameObject.SetActive(false);
                    else
                    {
                        switch (button.name)
                        {
                            case "compScan_Reactor":
                            case "compScan_Cargo":
                            case "compScan_Hull":
                            case "compScan_Processor":
                            case "compScan_Shld":
                            case "compScan_Misc":
                            case "compScan_Programs":
                            case "compScan_Turret":
                            case "compScan_LF":
                            case "weaknessScanAttuneDmg":
                            case "weaknessScanAttuneDmg1":
                            case "weaknessScanAttuneDmg3":
                            case "weaknessScanCyberDef":
                            case "weaknessScanShieldWeakPoint":
                            case "weaknessScanReactor":
                                button.gameObject.SetActive(true);
                                break;
                        }
                    }
                }
            }
        }

        [HarmonyPatch(typeof(PLScientistSensorScreen), "OnButtonMouseAway")]
        internal class SensorButtonMouseAway
        {
            private static bool Prefix(PLScientistSensorScreen __instance, UIWidget inButton, ref UIWidget ___LastHoverButton)
            {
                if (inButton.name.Contains("compScanOption"))
                {
                    int index = Int32.Parse(inButton.name.Split('_')[1]);
                    if (SensorScreenButtonClick.cachedComponentScan.Count > index + SensorScreenButtonClick.pageIndex * 8)
                    {
                        Color color = PLGlobal.GetColorBGForWare(SensorScreenButtonClick.cachedComponentScan[index + SensorScreenButtonClick.pageIndex * 8]) * 0.5f;
                        color.a = 1f;
                        inButton.color = color;
                        inButton.GetComponentInChildren<UILabel>().color = inButton.color;
                    }
                    if (!(inButton == ___LastHoverButton))
                        return false;
                    PLInGameUI.HidePersistantLargeTooltip();
                    ___LastHoverButton = null;
                    return false;
                }
                return true;
            }
        }
    }
}
