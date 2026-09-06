using HarmonyLib;
using UnityEngine;

namespace ExpandedGalaxy.SensorDish
{
    [HarmonyPatch(typeof(PLScientistSensorScreen), "Update")]
    internal class UpdateSensorScreenPatch
    {
        private static bool Prefix(PLScientistSensorScreen __instance)
        {
            if (!__instance.UIIsSetup() || !__instance.LocalPlayerInSameLocation())
                return true;
            if (__instance.MyScreenHubBase.OptionalShipInfo != null)
            {
                int sensorDishSubtype = 0;
                if (__instance.MyScreenHubBase.OptionalShipInfo.MyStats.GetShipComponent<PLSensorDish>(ESlotType.E_COMP_SENSORDISH) != null)
                    sensorDishSubtype = __instance.MyScreenHubBase.OptionalShipInfo.MyStats.GetShipComponent<PLSensorDish>(ESlotType.E_COMP_SENSORDISH).SubType;
                int screenAbilityMode = 0;
                if (SensorDishModManager.Instance.ScreenInfoData.ContainsKey(__instance.MyScreenHubBase.OptionalShipInfo.ShipID))
                    screenAbilityMode = SensorDishModManager.Instance.ScreenInfoData[__instance.MyScreenHubBase.OptionalShipInfo.ShipID];
                else
                    SensorDishModManager.Instance.ScreenInfoData[__instance.MyScreenHubBase.OptionalShipInfo.ShipID] = 0;
                if (screenAbilityMode != sensorDishSubtype)
                {
                    GameObject.Destroy(__instance.weaknessScanCyberDefBG.gameObject);
                    GameObject.Destroy(__instance.weaknessScanShieldsWeakPointBG.gameObject);
                    GameObject.Destroy(__instance.weaknessScanReactorWeakPointBG.gameObject);
                    GameObject.Destroy(__instance.weaknessScanCyberDef.gameObject);
                    GameObject.Destroy(__instance.weaknessScanShieldsWeakPoint.gameObject);
                    GameObject.Destroy(__instance.weaknessScanReactorWeakPoint.gameObject);

                    __instance.AllStylizedElements.RemoveAll(item => item == null);
                    __instance.AllLabels.RemoveAll(item => item == null);
                    __instance.AllSprites.RemoveAll(item => item == null);
                    __instance.AllButtons.RemoveAll(item => item == null);

                    UITexture[] buttonTextures = SensorDishModManager.Instance.CreateButtonsFromSubType(__instance, sensorDishSubtype);

                    __instance.weaknessScanCyberDefBG = buttonTextures[0];
                    __instance.weaknessScanShieldsWeakPointBG = buttonTextures[1];
                    __instance.weaknessScanReactorWeakPointBG = buttonTextures[2];
                    __instance.weaknessScanCyberDef = buttonTextures[3];
                    __instance.weaknessScanShieldsWeakPoint = buttonTextures[4];
                    __instance.weaknessScanReactorWeakPoint = buttonTextures[5];
                    SensorDishModManager.Instance.ScreenInfoData[__instance.MyScreenHubBase.OptionalShipInfo.ShipID] = sensorDishSubtype;
                }
            }
            return true;
        }
    }
}
