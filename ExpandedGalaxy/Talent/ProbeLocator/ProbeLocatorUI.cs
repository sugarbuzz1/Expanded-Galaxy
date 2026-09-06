using HarmonyLib;
using Talents.Framework;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLUIOutsideWorldUI), "UpdateKeenUIElements")]
    internal class ProbeLocatorUI
    {
        private static void Postfix(PLUIOutsideWorldUI __instance)
        {
            if (!(PLCameraSystem.Instance.GetModeString() == "SensorDish"))
                return;
            if (PLEncounterManager.Instance.PlayerShip == null || PLEncounterManager.Instance.PlayerShip.InWarp)
                return;
            if ((int)PLNetworkManager.Instance.LocalPlayer.Talents[TalentModManager.Instance.GetTalentIDFromName("Probe Specialty: Locator")] > 0)
            {
                foreach (PLProbePickup plProbePickup in UnityEngine.Object.FindObjectsOfType<PLProbePickup>())
                {
                    if (plProbePickup != null && !plProbePickup.PickedUp)
                        __instance.RequestKeenUIElement(plProbePickup.transform, "Research");
                }
            }
        }
    }
}
