using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLPawn), "TakeFireDamage")]
    internal class NoFireDamage
    {
        private static bool Prefix(PLPawn __instance, ref float inDmg)
        {
            if (Exosuit.BetterExosuit)
            {
                if (__instance.GetPlayer() != null)
                {
                    if (__instance.GetPlayer().RaceID == 2 || __instance.GetExosuitIsActive())
                    {
                        if (__instance.GetPlayer().RaceID == 1)
                        {
                            inDmg *= 0.5f;
                            return true;
                        }
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
