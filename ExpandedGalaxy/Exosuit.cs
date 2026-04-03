
using HarmonyLib;
using PulsarModLoader.Patches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace ExpandedGalaxy
{
    internal class Exosuit
    {
        internal static bool BetterExosuit = true;

        [HarmonyPatch(typeof(PLPlayerController), "IsSprinting")]
        internal class NoExosuitSprint
        {
            private static void Postfix(PLPlayerController __instance, ref bool __result)
            {
                if (BetterExosuit)
                {
                    if (__instance.MyPawn != null)
                    {
                        if (__instance.MyPawn.GetExosuitIsActive())
                            __result = false;
                        else if (__instance.MyPawn.GetPlayer() != null && __instance.MyPawn.GetPlayer().RaceID == 2)
                            __result = false;
                    }
                }
            }
        }

        [HarmonyPatch(typeof(PLCombatTarget), "TakeDamage")]
        internal class BetterExosuitDamage
        {
            private static bool Prefix(PLCombatTarget __instance, ref float inDmg, bool combat, int attackerCombatTargetID)
            {
                if (BetterExosuit && __instance is PLPawn pawn)
                {
                    if (pawn.GetExosuitIsActive())
                        inDmg *= 0.8f;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(PLPawn), "TakeFireDamage")]
        internal class NoFireDamage
        {
            private static bool Prefix(PLPawn __instance, ref float inDmg)
            {
                if (BetterExosuit)
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

        [HarmonyPatch(typeof(PLController), "Update")]
        internal class NoExosuitSlowdown
        {
            internal static float ExosuitSpeedMod() => BetterExosuit ? 1f : 0.66f;
            private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
            {
                List<CodeInstruction> list = instructions.ToList();

                List<CodeInstruction> targetSequence = new List<CodeInstruction>() {
                    new CodeInstruction(OpCodes.Ldc_R4, 0.66f)
                };
                List<CodeInstruction> patchSequence = new List<CodeInstruction>()
                {
                    CodeInstruction.Call(typeof(ExpandedGalaxy.Exosuit.NoExosuitSlowdown), "ExosuitSpeedMod")
                };

                return HarmonyHelpers.PatchBySequence(list.AsEnumerable<CodeInstruction>(), targetSequence, patchSequence, HarmonyHelpers.PatchMode.REPLACE, HarmonyHelpers.CheckMode.NONNULL, false);
            }
        }
    }
}
