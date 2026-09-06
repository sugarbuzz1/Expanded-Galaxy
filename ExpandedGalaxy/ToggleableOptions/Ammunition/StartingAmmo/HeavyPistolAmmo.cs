using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLPawnItem_HeavyPistol), MethodType.Constructor)]
    internal class HeavyPistolAmmo
    {
        private static void Postfix(PLPawnItem_HeavyPistol __instance)
        {
            if (!Ammunition.DynamicAmmunition)
                return;
            __instance.AmmoMax = 20;
            __instance.AmmoCurrent = __instance.AmmoMax;
        }
    }
}
