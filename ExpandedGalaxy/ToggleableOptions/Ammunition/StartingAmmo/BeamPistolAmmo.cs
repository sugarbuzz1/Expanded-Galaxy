using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLPawnItem_PierceLaserPistol), MethodType.Constructor)]
    internal class BeamPistolAmmo
    {
        private static void Postfix(PLPawnItem_PierceLaserPistol __instance)
        {
            if (!Ammunition.DynamicAmmunition)
                return;
            __instance.AmmoMax = 30;
            __instance.AmmoCurrent = __instance.AmmoMax;
        }
    }
}
