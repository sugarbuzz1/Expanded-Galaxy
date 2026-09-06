using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLPawnItem_HandShotgun), MethodType.Constructor)]
    internal class SplitshotAmmo
    {
        private static void Postfix(PLPawnItem_HandShotgun __instance)
        {
            if (!Ammunition.DynamicAmmunition)
                return;
            __instance.AmmoMax = 12;
            __instance.AmmoCurrent = __instance.AmmoMax;
        }
    }
}
