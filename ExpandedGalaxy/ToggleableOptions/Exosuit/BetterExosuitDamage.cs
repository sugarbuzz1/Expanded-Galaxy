using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLCombatTarget), "TakeDamage")]
    internal class BetterExosuitDamage
    {
        private static bool Prefix(PLCombatTarget __instance, ref float inDmg, bool combat, int attackerCombatTargetID)
        {
            if (Exosuit.BetterExosuit && __instance is PLPawn pawn)
            {
                if (pawn.GetExosuitIsActive() || pawn.GetPlayer() != null && pawn.GetPlayer().RaceID == 2)
                    inDmg *= 0.5f;
            }
            return true;
        }
    }
}
