using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLPawnItem_HeldBeamPistol_WithHealing), MethodType.Constructor)]
    internal class HealingBeamRifleAmmo
    {
        private static void Postfix(PLPawnItem_HeldBeamPistol_WithHealing __instance)
        {
            if (!Ammunition.DynamicAmmunition)
                return;
            __instance.AmmoMax = 100;
            __instance.AmmoCurrent = __instance.AmmoMax;
        }
    }
}
