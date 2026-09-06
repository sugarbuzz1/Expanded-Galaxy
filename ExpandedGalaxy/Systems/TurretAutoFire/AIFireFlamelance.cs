using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLFlamelanceTurret), "ShouldAIFire")]
    internal class AIFireFlamelance
    {
        private static bool Prefix(PLFlamelanceTurret __instance, bool operatedByBot, float heatOffset, float heatGenOnFire, ref bool __result)
        {
            if (__instance.ShipStats != null && (UnityEngine.Object)__instance.ShipStats.Ship != (UnityEngine.Object)null)
            {
                if (__instance.ShipStats.Ship.ShipID == PLEncounterManager.Instance.PlayerShip.ShipID)
                {
                    if (!__instance.ShipStats.Ship.IsAuxSystemActive(4))
                    {
                        __result = false;
                        return false;
                    }
                }
            }
            return true;
        }
    }
}

