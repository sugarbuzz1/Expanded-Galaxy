using HarmonyLib;
using PulsarModLoader.Patches;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLTurret), "UpdateQuickShowVisuals")]
    internal class PulseLaserDmgPatch
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> list = instructions.ToList();

            List<CodeInstruction> targetSequence = new List<CodeInstruction>() {
                    new CodeInstruction(OpCodes.Ldc_R4, 60f)
                };
            List<CodeInstruction> patchSequence = new List<CodeInstruction>()
                {
                    new CodeInstruction(OpCodes.Ldc_R4, 30f)
                };
            List<CodeInstruction> list2 = HarmonyHelpers.PatchBySequence(list.AsEnumerable<CodeInstruction>(), targetSequence, patchSequence, HarmonyHelpers.PatchMode.REPLACE, HarmonyHelpers.CheckMode.NONNULL, false).ToList();

            List<CodeInstruction> targetSequence2 = new List<CodeInstruction>() {
                    new CodeInstruction(OpCodes.Ldc_R4, 0.2f)
                };
            List<CodeInstruction> patchSequence2 = new List<CodeInstruction>()
                {
                    new CodeInstruction(OpCodes.Ldc_R4, 0.1f)
                };
            foreach (CodeInstruction instruction in list2)
            {
                if (instruction.opcode == OpCodes.Ldc_R4 && instruction.operand is float && (float)instruction.operand == 0.2f)
                {
                    patchSequence2[0].labels.AddRange(instruction.labels);
                    break;
                }
            }
            List<CodeInstruction> list3 = HarmonyHelpers.PatchBySequence(list2.AsEnumerable<CodeInstruction>(), targetSequence2, patchSequence2, HarmonyHelpers.PatchMode.REPLACE, HarmonyHelpers.CheckMode.NONNULL, false).ToList();
            List<CodeInstruction> patchSequence3 = new List<CodeInstruction>()
                {
                    new CodeInstruction(OpCodes.Ldc_R4, 0.1f)
                };

            List<CodeInstruction> list4 = HarmonyHelpers.PatchBySequence(list3.AsEnumerable<CodeInstruction>(), targetSequence2, patchSequence3, HarmonyHelpers.PatchMode.REPLACE, HarmonyHelpers.CheckMode.NONNULL, false).ToList();
            List<CodeInstruction> targetSequence4 = new List<CodeInstruction>() {
                    new CodeInstruction(OpCodes.Ldc_R4, 0.5f)
                };
            List<CodeInstruction> patchSequence4 = new List<CodeInstruction>()
                {
                    new CodeInstruction(OpCodes.Ldc_R4, 0.8f)
                };

            return HarmonyHelpers.PatchBySequence(list4.AsEnumerable<CodeInstruction>(), targetSequence4, patchSequence4, HarmonyHelpers.PatchMode.REPLACE, HarmonyHelpers.CheckMode.NONNULL, false);
        }
    }
}
