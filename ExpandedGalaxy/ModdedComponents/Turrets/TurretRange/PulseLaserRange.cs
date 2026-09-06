using HarmonyLib;
using PulsarModLoader.Patches;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLTurret), "UpdateQuickShowVisuals")]
    internal class PulseLaserRange
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> list = instructions.ToList();

            List<CodeInstruction> targetSequence = new List<CodeInstruction>() {
                    new CodeInstruction(OpCodes.Ldc_R4, 20000f),
                    new CodeInstruction(OpCodes.Stloc_S),
                };
            List<CodeInstruction> patchSequence = new List<CodeInstruction>()
                {
                    new CodeInstruction(OpCodes.Ldarg_0),
                    CodeInstruction.LoadField(typeof(PLTurret), "TurretRange"),
                    new CodeInstruction(OpCodes.Ldc_R4, 5f),
                    new CodeInstruction(OpCodes.Div),
                    new CodeInstruction(OpCodes.Stloc_S, 12)
                };
            patchSequence[0].labels.Add(list[263].labels[0]);

            return HarmonyHelpers.PatchBySequence(list.AsEnumerable<CodeInstruction>(), targetSequence, patchSequence, HarmonyHelpers.PatchMode.REPLACE, HarmonyHelpers.CheckMode.NONNULL, false);
        }
    }
}
