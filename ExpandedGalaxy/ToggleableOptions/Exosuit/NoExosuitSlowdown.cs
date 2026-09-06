using HarmonyLib;
using PulsarModLoader.Patches;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLController), "Update")]
    internal class NoExosuitSlowdown
    {
        internal static float ExosuitSpeedMod() => Exosuit.BetterExosuit ? 1f : 0.66f;
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> list = instructions.ToList();

            List<CodeInstruction> targetSequence = new List<CodeInstruction>() {
                new CodeInstruction(OpCodes.Ldc_R4, 0.66f)
            };
            List<CodeInstruction> patchSequence = new List<CodeInstruction>()
            {
                CodeInstruction.Call(typeof(NoExosuitSlowdown), "ExosuitSpeedMod")
            };

            return HarmonyHelpers.PatchBySequence(list.AsEnumerable<CodeInstruction>(), targetSequence, patchSequence, HarmonyHelpers.PatchMode.REPLACE, HarmonyHelpers.CheckMode.NONNULL, false);
        }
    }
}
