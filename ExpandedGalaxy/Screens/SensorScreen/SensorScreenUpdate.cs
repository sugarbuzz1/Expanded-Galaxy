
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLScientistSensorScreen), "Update")]
    internal class SensorScreenUpdate
    {
        internal static byte SensorMode = 0;
        private static void Postfix(PLScientistSensorScreen __instance, ref bool ___IsInfoBoxActive, ref UISprite ___InfoBoxBG, ref UILabel ___InfoBoxText, ref UILabel ___InfoBoxTitle, ref UISprite ___InfoBoxCloseButton, ref bool ___InfoBox_IsTopMsg, ref UIWidget[] ___MainScreen_ActiveScanRoots, ref UILabel[] ___MainScreen_ActiveScanNameLabels, ref PLShipInfoBase ___TargetShip, ref PLSensorObject ___TargetSensorObject, ref UILabel ___ShipInfoScreen_LeftInfoTextLabel, ref UILabel ___ShipInfoScreen_RightInfoTextLabel, ref UILabel[] ___MainScreen_ActiveScanTimeLabels, ref PLCachedFormatString<int> ___cMainScreen_ActiveScanTimeLabels0)
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
                    ___InfoBoxBG.height = 50 + Mathf.RoundToInt((float)___InfoBoxText.height * 0.25f);
                    float num = (float)___InfoBoxBG.height * 0.5f;
                    ___InfoBoxTitle.transform.localPosition = new Vector3(-180, num - 10f);
                    ___InfoBoxText.transform.localPosition = new Vector3(-180, num - 50f);
                    ___InfoBoxCloseButton.transform.localPosition = new Vector3(140f, num - 10f);
                    if (___InfoBox_IsTopMsg)
                        num = 215f;
                    ___InfoBoxBG.transform.localPosition = new Vector3(0f, num - (float)___InfoBoxBG.height * 0.5f);
                }
            }
            if (__instance.MyScreenHubBase.OptionalShipInfo == null || __instance.MyScreenHubBase.OptionalShipInfo.IsActiveScanInProgress())
                return;
            if (___TargetShip != null)
            {
                ___ShipInfoScreen_LeftInfoTextLabel.text = "MASS\n";
                ___ShipInfoScreen_RightInfoTextLabel.text = ___TargetShip.MyStats.Mass.ToString("0") + "\n";
                if (___TargetShip.MySensorObjectShip != null)
                {
                    switch (SensorMode)
                    {
                        case 0:
                            ___ShipInfoScreen_LeftInfoTextLabel.text += PLLocalize.Localize("EM SIG");
                            ___ShipInfoScreen_RightInfoTextLabel.text += ___TargetShip.MySensorObjectShip.EMSignature.ToString("0.0");
                            break;
                        case 1:
                            ___ShipInfoScreen_LeftInfoTextLabel.text += "LF SIG";
                            ___ShipInfoScreen_RightInfoTextLabel.text += (___TargetShip.MySensorObjectShip.LFSignature / 0.125f).ToString("0.0");
                            break;
                        case 2:
                            ___ShipInfoScreen_LeftInfoTextLabel.text += "NT SIG";
                            ___ShipInfoScreen_RightInfoTextLabel.text += ___TargetShip.MySensorObjectShip.QTSignature.ToString("0.0");
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}
