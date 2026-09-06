using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLPawnItem_HeldBeamPistol_WithHealing), MethodType.Constructor)]
    internal class HealBeamNotWeapon
    {
        private static void Postfix(PLPawnItem_HeldBeamPistol_WithHealing __instance)
        {
            __instance.MyUtilityType = EItemUtilityType.E_NONE;
        }
    }
}
