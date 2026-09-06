using HarmonyLib;
using PulsarModLoader.Content.Components.CaptainsChair;
using PulsarModLoader.Content.Components.CPU;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLPlayer), "Update")]
    internal class PawnAbilityHandler
    {
        private static void Postfix(PLPlayer __instance)
        {
            if ((UnityEngine.Object)PLServer.Instance != (UnityEngine.Object)null)
            {
                if (PLNetworkManager.Instance != (UnityEngine.Object)null && PLNetworkManager.Instance.LocalPlayer != null && PLNetworkManager.Instance.LocalPlayer != __instance)
                    return;
                ShowAbilityText.setBottomInfoText("");
                if ((UnityEngine.Object)PLNetworkManager.Instance != (UnityEngine.Object)null && (UnityEngine.Object)PLNetworkManager.Instance.MyLocalPawn != (UnityEngine.Object)null && (UnityEngine.Object)PLNetworkManager.Instance.MyLocalPawn.MyController != (UnityEngine.Object)null && PLCameraSystem.Instance.CurrentCameraMode != null && PLCameraSystem.Instance.CurrentCameraMode.GetModeString() == "LocalPawn" && (UnityEngine.Object)PLNetworkManager.Instance.LocalPlayer != (UnityEngine.Object)null)
                {
                    if (PLNetworkManager.Instance.MyLocalPawn.CurrentShip != null)
                    {

                        PLShipInfo ship = __instance.GetPawn().CurrentShip;
                        if (__instance.IsSittingInCaptainsChair())
                        {
                            if (ship.MyStats.GetShipComponent<PLCaptainsChair>(ESlotType.E_COMP_CAPTAINS_CHAIR) != null && ship.MyStats.GetShipComponent<PLCaptainsChair>(ESlotType.E_COMP_CAPTAINS_CHAIR).SubType == CaptainsChairModManager.Instance.GetCaptainsChairIDFromName("Seat of the Surveyor"))
                            {
                                PLCaptainsChair chair = ship.MyStats.GetShipComponent<PLCaptainsChair>(ESlotType.E_COMP_CAPTAINS_CHAIR);
                                if (chair.SubTypeData == 0)
                                    ShowAbilityText.setBottomInfoText("<color=yellow>[" + PLInput.Instance.GetPrimaryKeyStringForAction("ExpandedGalaxy.Ability") + "] Launch Drone</color>");
                                else if (chair.SubTypeData > 0)
                                    ShowAbilityText.setBottomInfoText("<color=yellow>[" + PLInput.Instance.GetPrimaryKeyStringForAction("ExpandedGalaxy.Ability") + "] Control Drone</color>");
                            }
                        }
                        else
                        {
                            PLCPU plcpu = null;
                            foreach (PLShipComponent component in ship.MyStats.GetComponentsOfType(ESlotType.E_COMP_CPU))
                            {
                                plcpu = component as PLCPU;
                                if (plcpu != null && plcpu.SubType == CPUModManager.Instance.GetCPUIDFromName("Sylvassi Shield Charger"))
                                    break;
                            }
                            if (plcpu != null)
                            {
                                if (plcpu.SubType == CPUModManager.Instance.GetCPUIDFromName("Sylvassi Shield Charger"))
                                {
                                    if (!(PLNetworkManager.Instance.LocalPlayer.GetClassID() == 0 || PLNetworkManager.Instance.LocalPlayer.GetClassID() == 2))
                                        return;
                                    if (plcpu.SubTypeData == 0)
                                    {
                                        ShowAbilityText.setBottomInfoText("<color=yellow>[" + PLInput.Instance.GetPrimaryKeyStringForAction("ExpandedGalaxy.Ability") + "] Enable Shield Charger</color>");
                                    }
                                    else
                                    {
                                        ShowAbilityText.setBottomInfoText("<color=yellow>[" + PLInput.Instance.GetPrimaryKeyStringForAction("ExpandedGalaxy.Ability") + "] Disable Shield Charger</color>");
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}

