using HarmonyLib;
using System.Collections.Generic;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLShipInfoBase), "Update")]
    internal class CorruptionControls
    {
        private static void Postfix(PLShipInfoBase __instance)
        {
            if (!(PLServer.Instance != null))
                return;
            if (PLNetworkManager.Instance.LocalPlayer == null || PLNetworkManager.Instance.MyLocalPawn == null)
                return;
            if (!(__instance != null && __instance.MyStats != null))
                return;
            bool flag = false;
            if (__instance.GetCurrentShipControllerPlayerID() == PLNetworkManager.Instance.LocalPlayer.GetPlayerID())
            {
                List<PLShipComponent> components = __instance.MyStats.GetComponentsOfType(ESlotType.E_COMP_VIRUS);
                foreach (PLShipComponent component in components)
                {
                    PLVirus virus = component as PLVirus;
                    if (virus != null && virus.SubType == (int)EVirusType.CORRUPTION)
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag)
                {
                    PLInput.Instance.EInputActionNameToString[54] = "flight_left";
                    PLInput.Instance.EInputActionNameToString[53] = "flight_right";
                    PLInput.Instance.EInputActionNameToString[56] = "flight_forward";
                    PLInput.Instance.EInputActionNameToString[55] = "flight_back";
                    PLInput.Instance.EInputActionNameToString[59] = "flight_roll_right";
                    PLInput.Instance.EInputActionNameToString[60] = "flight_roll_left";
                    PLInput.Instance.EInputActionNameToString[63] = "full_throttle";
                    PLInput.Instance.EInputActionNameToString[62] = "full_reverse_throttle";
                }
                else
                {
                    PLInput.Instance.EInputActionNameToString[54] = "flight_back";
                    PLInput.Instance.EInputActionNameToString[53] = "flight_forward";
                    PLInput.Instance.EInputActionNameToString[56] = "flight_left";
                    PLInput.Instance.EInputActionNameToString[55] = "flight_right";
                    PLInput.Instance.EInputActionNameToString[59] = "flight_roll_left";
                    PLInput.Instance.EInputActionNameToString[60] = "flight_roll_right";
                    PLInput.Instance.EInputActionNameToString[63] = "full_reverse_throttle";
                    PLInput.Instance.EInputActionNameToString[62] = "full_throttle";
                }
            }
        }
    }
}

