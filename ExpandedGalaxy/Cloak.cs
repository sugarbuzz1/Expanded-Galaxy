using HarmonyLib;
using PulsarModLoader;
using UnityEngine;

namespace ExpandedGalaxy
{
    internal class Cloak
    {
        [HarmonyPatch(typeof(PLSensorObjectShip), "GetIsCloaked")]
        private class Remove0EMSignature
        {
            private static void Postfix(ref bool __result)
            {
                __result = false;
            }
        }

        [HarmonyPatch(typeof(PLScientistComputerScreen), "Update")]
        private class ScientistScreenFix
        {
            private static void Postfix(
              PLScientistComputerScreen __instance,
              UILabel ___MainScreen_EMLabel,
              PLCachedFormatString<float> ___cMainScreen_EMLabel)
            {
                PLGlobal.SafeLabelSetText(___MainScreen_EMLabel, ___cMainScreen_EMLabel.ToString(__instance.MyScreenHubBase.OptionalShipInfo.MyStats.EMSignature));
            }
        }

        [HarmonyPatch(typeof(PLShipStats), "CalculateStats")]
        private class EMSignatureDecreaser
        {
            private static void Postfix(PLShipStats __instance)
            {
                PLCloakingSystem cloakingSystem = __instance.Ship.MyCloakingSystem;
                if (cloakingSystem != null && __instance.Ship.GetIsCloakingSystemActive())
                {
                    float EMReduction = 0f;
                    switch (cloakingSystem.SubType)
                    {
                        case (int)ECloakingSystemType.E_SYVASSI:
                            EMReduction = 4f * cloakingSystem.GetPowerPercentInput(); break;
                        default:
                            EMReduction = 3f * cloakingSystem.GetPowerPercentInput(); break;
                    }
                    __instance.EMSignature = Mathf.Clamp(__instance.EMSignature - EMReduction, 0f, __instance.EMSignature);
                }
            }
        }

        [HarmonyPatch(typeof(PLWare), "GetStatLineLeft")]
        internal class StatLineForCloakLeft
        {
            private static bool Prefix(PLWare __instance, ref string __result)
            {
                PLCloakingSystem cloakingSystem = __instance as PLCloakingSystem;
                if (cloakingSystem != null)
                {
                    if (cloakingSystem.SubType == (int)ECloakingSystemType.E_NORMAL)
                    {
                        __result = PLLocalize.Localize("EM Signature") + "\n" + PLLocalize.Localize("Turret Damage") + "\n";
                        return false;
                    }
                    else
                    {
                        __result = PLLocalize.Localize("EM Signature") + "\n" + PLLocalize.Localize("Charge Rate") + "\n";
                        return false;
                    }
                }
                return true;
            }
        }

        public static float subTypeDataParse(short subtypedata)
        {
            return subtypedata / 250f;
        }

        [HarmonyPatch(typeof(PLWare), "GetStatLineRight")]
        internal class StatLineForCloakRight
        {
            private static bool Prefix(PLWare __instance, ref string __result)
            {
                PLCloakingSystem cloakingSystem = __instance as PLCloakingSystem;
                if (cloakingSystem != null)
                {
                    if (cloakingSystem.SubType == (int)ECloakingSystemType.E_NORMAL)
                    {
                        __result = "-" + (3f * cloakingSystem.GetPowerPercentInput()).ToString("0.0") + "/" + (3f).ToString("0.0") + "\n" + "+" + subTypeDataParse(cloakingSystem.SubTypeData).ToString("0.0") + "/" + (20f + 2.5f * cloakingSystem.Level).ToString("0.0") + "\n";
                        return false;
                    }
                    else
                    {
                        __result = "-" + (4f * cloakingSystem.GetPowerPercentInput()).ToString("0.0") + "/" + (4f).ToString("0.0") + "\n" + "+" + subTypeDataParse(cloakingSystem.SubTypeData).ToString("0.0") + "/" + (20f + 2.5f * cloakingSystem.Level).ToString("0.0") + "\n";
                        return false;
                    }
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(PLCloakingSystem), "Tick")]
        internal class CloakTick
        {
            private static void Postfix(PLCloakingSystem __instance)
            {
                if (PhotonNetwork.isMasterClient)
                {

                    ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.UpdateSubTypeData", PhotonTargets.Others, new object[3]
                    {
                        (object) __instance.ShipStats.Ship.ShipID,
                        (object) __instance.NetID,
                        (object) __instance.SubTypeData,
                    });

                    if (__instance.SubTypeData < 0)
                        __instance.SubTypeData = 0;
                    if (__instance.ShipStats.Ship.GetIsCloakingSystemActive())
                    {
                        if (__instance.SubTypeData < (5000 + 625 * __instance.Level))
                            __instance.SubTypeData += 10;
                    }
                    else
                    {
                        if (__instance.SubTypeData > 0)
                            __instance.SubTypeData -= 10;
                    }
                }
            }
        }

        [HarmonyPatch(typeof(PLShipComponent), "AddStats")]
        internal class CloakAddStats
        {
            private static void Postfix(PLShipComponent __instance, PLShipStats inStats)
            {
                if (!(__instance is PLCloakingSystem))
                    return;
                if (__instance.SubType == (int)ECloakingSystemType.E_NORMAL)
                    inStats.TurretDamageFactor *= 1f + (subTypeDataParse(__instance.SubTypeData) / 100f);
            }
        }

        [HarmonyPatch(typeof(PLShipComponent), "FinalLateAddStats")]
        internal class CloakFinalLateAddStats
        {
            private static void Postfix(PLShipComponent __instance, PLShipStats inStats)
            {
                if (!(__instance is PLCloakingSystem))
                    return;
                if (__instance.SubType == (int)ECloakingSystemType.E_SYVASSI)
                {
                    inStats.ShieldsChargeRate *= 1f + (subTypeDataParse(__instance.SubTypeData) / 100f);
                    inStats.ShieldsChargeRateMax *= 1f + (subTypeDataParse(__instance.SubTypeData) / 100f);
                }
            }
        }
    }
}
