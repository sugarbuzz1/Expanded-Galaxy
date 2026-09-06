using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLPawnItem_BurstPistol), MethodType.Constructor)]
    internal class BurstRifleAmmo
    {
        private static void Postfix(PLPawnItem_BurstPistol __instance)
        {
            if (!Ammunition.DynamicAmmunition)
                return;
            __instance.AmmoMax = 60;
            __instance.AmmoCurrent = __instance.AmmoMax;
        }
    }
}
