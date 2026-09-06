using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLShipInfo), "OnProgramsCharged")]
    internal class NoAmmoRefill
    {
        private static bool Prefix() { return !Ammunition.DynamicAmmunition; }
    }
}
