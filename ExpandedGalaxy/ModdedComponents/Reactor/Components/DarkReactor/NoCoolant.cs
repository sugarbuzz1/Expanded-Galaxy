using HarmonyLib;
using PulsarModLoader.Patches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLReactor), "ShipUpdate")]
    internal class NoCoolant
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            Label failed = generator.DefineLabel();
            List<CodeInstruction> list = instructions.ToList();

            List<CodeInstruction> targetSequence = new List<CodeInstruction>() {
                    new CodeInstruction(OpCodes.Mul),
                    new CodeInstruction(OpCodes.Ldloc_2),
                    new CodeInstruction(OpCodes.Mul),
                    new CodeInstruction(OpCodes.Ldloc_3)
                };
            List<CodeInstruction> patchSequence = new List<CodeInstruction>()
                {
                    new CodeInstruction(OpCodes.Ldarg_1),
                    new CodeInstruction(OpCodes.Brfalse_S, failed),
                    new CodeInstruction(OpCodes.Ldarg_1),
                    CodeInstruction.Call(typeof(PLWare), "get_Name"),
                    new CodeInstruction(OpCodes.Ldstr, "Dark-Matter Reactor"),
                    CodeInstruction.Call(typeof(System.String), "op_Equality", new Type[2] {
                        typeof(string),
                        typeof(string)
                    }),
                    new CodeInstruction(OpCodes.Brfalse, failed),
                    new CodeInstruction(OpCodes.Ldc_R4, 0f),
                    new CodeInstruction(OpCodes.Stloc_1)
                };
            list[120].labels.Add(failed);

            return HarmonyHelpers.PatchBySequence(list.AsEnumerable<CodeInstruction>(), targetSequence, patchSequence, HarmonyHelpers.PatchMode.AFTER, HarmonyHelpers.CheckMode.ALWAYS, false);
        }
    }
}
