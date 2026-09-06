using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLPawnItem_SmugglersPistol), MethodType.Constructor)]
    internal class SmugglerPistolAmmo
    {
        private static void Postfix(PLPawnItem_SmugglersPistol __instance)
        {
            if (!Ammunition.DynamicAmmunition)
                return;
            __instance.UsesAmmo = true;
            __instance.AmmoMax = 48;
            __instance.AmmoCurrent = __instance.AmmoMax;
        }
    }
}
