using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLPawnItem_SmugglersPistol), "CalcDamageDone")]
    internal class SmugglerPistolDamage
    {
        private static bool Prefix(PLPawnItem_SmugglersPistol __instance, ref float __result)
        {
            if (!Ammunition.DynamicAmmunition)
                return true;
            __result = (float)(12.0 + 5.0 * __instance.Level);
            return false;
        }
    }
}
