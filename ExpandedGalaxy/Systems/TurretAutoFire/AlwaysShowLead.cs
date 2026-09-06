using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLUIOutsideWorldUI), "Update")]
    internal class AlwaysShowLead
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> list = instructions.ToList<CodeInstruction>();
            list[99].opcode = OpCodes.Nop;
            list[100].opcode = OpCodes.Nop;
            list[101].opcode = OpCodes.Nop;
            list[102].opcode = OpCodes.Nop;
            list[103].opcode = OpCodes.Nop;
            list[104].opcode = OpCodes.Ldc_I4_1;
            list[104].operand = (object)null;
            return list.AsEnumerable<CodeInstruction>();
        }
    }
}

