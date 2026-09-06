using HarmonyLib;
using UnityEngine;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLDeathseekerCommanderDrone), "Update")]
    internal class DeathwardenDischarge
    {
        private static void Postfix(PLDeathseekerCommanderDrone __instance)
        {
            if (__instance.HasBeenDestroyed || PLEncounterManager.Instance.PlayerShip.Get_IsInWarpMode())
                return;
            foreach (PLShipInfoBase plShipInfoBase in UnityEngine.Object.FindObjectsOfType(typeof(PLShipInfoBase)))
            {
                if ((UnityEngine.Object)plShipInfoBase != (UnityEngine.Object)__instance && (plShipInfoBase.GetCurrentSensorPosition() - __instance.GetCurrentSensorPosition()).magnitude < (8000f / 5f))
                    plShipInfoBase.DischargeAmount += 0.08f * Time.deltaTime;
            }
        }
    }
}
