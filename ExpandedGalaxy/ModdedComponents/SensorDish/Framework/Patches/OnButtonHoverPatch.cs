using HarmonyLib;

namespace ExpandedGalaxy.SensorDish
{
    [HarmonyPatch(typeof(PLScientistSensorScreen), "OnButtonHover")]
    internal class OnButtonHoverPatch
    {
        private static void Postfix(PLScientistSensorScreen __instance, UIWidget inButton)
        {
            if (!(inButton.name == "weaknessScanCyberDef" || inButton.name == "weaknessScanReactor" || inButton.name == "weaknessScanShieldWeakPoint"))
                return;
            if (__instance.MyScreenHubBase == null || __instance.MyScreenHubBase.OptionalShipInfo == null)
                return;
            int subType = 0;
            int abilityType = 0;
            if (SensorDishModManager.Instance.ScreenInfoData.ContainsKey(__instance.MyScreenHubBase.OptionalShipInfo.ShipID))
                subType = SensorDishModManager.Instance.ScreenInfoData[__instance.MyScreenHubBase.OptionalShipInfo.ShipID];
            switch (inButton.name)
            {
                case "weaknessScanCyberDef":
                    abilityType = 0;
                    break;
                case "weaknessScanShieldWeakPoint":
                    abilityType = 1;
                    break;
                case "weaknessScanReactor":
                    abilityType = 2;
                    break;
            }
            PLInGameUI.SetTooltipLargeText(SensorDishModManager.Instance.SensorDishTypes[subType].SensorDishAbilities[abilityType].Name, SensorDishModManager.Instance.SensorDishTypes[subType].SensorDishAbilities[abilityType].Description, false, 0.0f);
        }
    }
}
