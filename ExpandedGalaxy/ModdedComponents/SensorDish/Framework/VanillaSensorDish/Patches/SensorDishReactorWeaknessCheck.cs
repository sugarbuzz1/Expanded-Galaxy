using HarmonyLib;
using PulsarModLoader.Patches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace ExpandedGalaxy.SensorDish
{
    [HarmonyPatch(typeof(PLShipStats), "CalculateStats")]
    internal class SensorDishReactorWeaknessCheck
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> list = instructions.ToList();

            List<CodeInstruction> targetSequence = new List<CodeInstruction>() {
                    CodeInstruction.Call(typeof(PLShipStats), "get_Ship"),
                    new CodeInstruction(OpCodes.Ldc_I4_5),
                    new CodeInstruction(list[965]),
                };
            List<CodeInstruction> patchSequence = new List<CodeInstruction>()
                {
                    CodeInstruction.Call(typeof(PLShipStats), "get_Ship"),
                    new CodeInstruction(OpCodes.Ldc_I4_5),
                    new CodeInstruction(OpCodes.Ldc_I4, -1),
                    CodeInstruction.Call(typeof(SensorDishModManager), "IsSensorWeaknessActiveModded", new Type[3] {
                        typeof(PLShipInfoBase),
                        typeof(int),
                        typeof(int)
                    }),
                };
            return HarmonyHelpers.PatchBySequence(list.AsEnumerable<CodeInstruction>(), targetSequence, patchSequence, HarmonyHelpers.PatchMode.REPLACE, HarmonyHelpers.CheckMode.NONNULL, false);
        }
    }
}
