using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLCorruptedDroneShipInfo), "EMPBlast")]
    internal class SentryEMP
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> list = instructions.ToList();

            list[28].operand = 600f;
            list[78].operand = 10000000f;
            return list.AsEnumerable<CodeInstruction>();
        }
    }
}
