using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLPawnItem_PhasePistol), MethodType.Constructor)]
    internal class PhasePistolAmmo
    {
        private static void Postfix(PLPawnItem_PhasePistol __instance)
        {
            if (!Ammunition.DynamicAmmunition)
                return;
            __instance.UsesAmmo = true;
            __instance.AmmoMax = 30;
            __instance.AmmoCurrent = __instance.AmmoMax;
        }
    }
}
