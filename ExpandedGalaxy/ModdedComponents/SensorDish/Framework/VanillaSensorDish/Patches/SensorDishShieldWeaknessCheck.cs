using HarmonyLib;
using PulsarModLoader.Patches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace ExpandedGalaxy.SensorDish
{
    [HarmonyPatch(typeof(PLShipInfoBase), "TakeDamage")]
    internal class SensorDishShieldWeaknessCheck
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            Label failed = generator.DefineLabel();

            List<CodeInstruction> list = instructions.ToList();

            List<CodeInstruction> targetSequence = new List<CodeInstruction>() {
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Ldc_I4_2),
                    CodeInstruction.Call(typeof(PLShipInfoBase), "IsSensorWeaknessActive", new Type[1]
                    {
                        typeof(ESensorWeakness)
                    }),
                };
            List<CodeInstruction> patchSequence = new List<CodeInstruction>()
                {
                    new CodeInstruction(OpCodes.Ldc_I4_0),
                };

            List<CodeInstruction> list2 = HarmonyHelpers.PatchBySequence(list.AsEnumerable<CodeInstruction>(), targetSequence, patchSequence, HarmonyHelpers.PatchMode.REPLACE, HarmonyHelpers.CheckMode.NONNULL, false).ToList();

            List<CodeInstruction> targetSequence2 = new List<CodeInstruction>() {
                    new CodeInstruction(OpCodes.Ldarg_3),
                    new CodeInstruction(OpCodes.Ldloc_2),
                    new CodeInstruction(OpCodes.Ldloc_S),
                };
            List<CodeInstruction> patchSequence2 = new List<CodeInstruction>()
                {
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Ldc_I4_2),
                    new CodeInstruction(OpCodes.Ldc_I4, -1),
                    CodeInstruction.Call(typeof(SensorDishModManager), "IsSensorWeaknessActiveModded", new Type[3] {
                        typeof(PLShipInfoBase),
                        typeof(int),
                        typeof(int)
                    }),
                    new CodeInstruction(OpCodes.Brfalse_S, failed),
                    new CodeInstruction(OpCodes.Ldc_R4, 1.2f),
                    new CodeInstruction(OpCodes.Mul)
                };

            list2[513].labels.Add(failed);

            return HarmonyHelpers.PatchBySequence(list2.AsEnumerable<CodeInstruction>(), targetSequence2, patchSequence2, HarmonyHelpers.PatchMode.AFTER, HarmonyHelpers.CheckMode.NONNULL, false);
        }
    }
}
