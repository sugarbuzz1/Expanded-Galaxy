using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLPhaseTurret), "Fire")]
    internal class PhaseTurretRange
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> list = instructions.ToList();
            List<CodeInstruction> patchSequence = new List<CodeInstruction>()
                {
                    new CodeInstruction(OpCodes.Ldarg_0),
                    CodeInstruction.LoadField(typeof(PLTurret), "TurretRange"),
                    new CodeInstruction(OpCodes.Ldc_R4, 5f),
                    new CodeInstruction(OpCodes.Div),
                };
            list.RemoveAt(131);
            list.InsertRange(131, patchSequence.AsEnumerable<CodeInstruction>());
            list.RemoveAt(139);
            list.InsertRange(139, patchSequence.AsEnumerable<CodeInstruction>());

            return list.AsEnumerable<CodeInstruction>();
        }
    }
}
