using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLTurret), "ShouldAIFire")]
    internal class AIFireBase
    {
        private static bool Prefix(PLTurret __instance, bool operatedByBot, float heatOffset, float heatGeneratedOnFire, ref bool __result)
        {
            if (__instance.ShipStats != null && (UnityEngine.Object)__instance.ShipStats.Ship != (UnityEngine.Object)null)
            {
                if (__instance.ShipStats.Ship == PLEncounterManager.Instance.PlayerShip)
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

