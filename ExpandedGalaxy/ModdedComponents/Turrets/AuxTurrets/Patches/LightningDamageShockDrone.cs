using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLLightningTurret), "Tick")]
    internal class LightningDamageShockDrone
    {
        private static void Postfix(PLLightningTurret __instance)
        {
            if (__instance.IsEquipped && __instance.m_Damage != 65f && __instance.ShipStats.Ship != null && __instance.ShipStats.Ship.ShipTypeID == EShipType.E_SHOCK_DRONE)
                __instance.m_Damage = 65f;
        }
    }
}
