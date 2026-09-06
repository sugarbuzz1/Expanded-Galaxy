using HarmonyLib;
using PulsarModLoader.Patches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLTurret), "ServerFireMissile")]
    internal class MissileTag
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            Label failed = generator.DefineLabel();
            List<CodeInstruction> list = instructions.ToList();

            List<CodeInstruction> targetSequence = new List<CodeInstruction>() {
                    new CodeInstruction(OpCodes.Ldloc_0),
                    new CodeInstruction(OpCodes.Ldc_R4, 30f),
                    new CodeInstruction(OpCodes.Ldarg_1),
                    new CodeInstruction(OpCodes.Ldfld),
                    new CodeInstruction(OpCodes.Mul),
                    new CodeInstruction(OpCodes.Stfld)
                };
            List<CodeInstruction> patchSequence = new List<CodeInstruction>()
                {
                    new CodeInstruction(OpCodes.Ldarg_1),
                    CodeInstruction.Call(typeof(PLWare), "get_Name"),
                    new CodeInstruction(OpCodes.Ldstr, "Seeker Missile"),
                    CodeInstruction.Call(typeof(System.String), "op_Equality", new Type[2] {
                        typeof(string),
                        typeof(string)
                    }),
                    new CodeInstruction(OpCodes.Brfalse, failed),
                    new CodeInstruction(OpCodes.Ldloc_0),
                    new CodeInstruction(OpCodes.Ldloc_0),
                    CodeInstruction.Call(typeof(UnityEngine.Object), "get_name"),
                    new CodeInstruction(OpCodes.Ldstr, " (seeker)"),
                    CodeInstruction.Call(typeof(string), "Concat", new Type[2]
                    {
                        typeof(string),
                        typeof(string)
                    }),
                    CodeInstruction.Call(typeof(UnityEngine.Object), "set_name", new Type[1]
                    {
                        typeof (string),
                    }),
                };
            list[55].labels.Add(failed);
            return HarmonyHelpers.PatchBySequence(list.AsEnumerable<CodeInstruction>(), targetSequence, patchSequence, HarmonyHelpers.PatchMode.AFTER, HarmonyHelpers.CheckMode.NONNULL, false);
        }
    }
}
