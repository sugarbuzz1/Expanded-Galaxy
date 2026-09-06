using ExpandedGalaxy.SensorDish;
using HarmonyLib;
using PulsarModLoader;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLScientistSensorScreen), "OnButtonClick")]
    internal class CacheRealPlayer
    {
        private static bool Prefix(PLScientistSensorScreen __instance, UIWidget inButton)
        {
            if (__instance.MyScreenHubBase != null && __instance.MyScreenHubBase.OptionalShipInfo != null && __instance.MyScreenHubBase.OptionalShipInfo)
            {
                PLShipComponent component = __instance.MyScreenHubBase.OptionalShipInfo.MyStats.GetShipComponent<PLShipComponent>(ESlotType.E_COMP_SENSORDISH);
                if (component != null || component.SubType != SensorDishModManager.Instance.GetSensorDishIDFromName("Ancient Sensor Dish"))
                    return true;
                AncientSensorDishMod.lastToInteract = PLNetworkManager.Instance.LocalPlayer;
                ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.CachePlayerRPC", PhotonTargets.Others, new object[1]
                {
                    (object) PLNetworkManager.Instance.LocalPlayer.GetPlayerID(),
                });
            }
            return true;
        }
    }
}
