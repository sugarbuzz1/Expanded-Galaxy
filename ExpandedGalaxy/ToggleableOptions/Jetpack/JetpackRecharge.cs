using HarmonyLib;
using PulsarModLoader.Patches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLPlayerController), "HandleMovement")]
    internal class JetpackRecharge
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            Label failed = generator.DefineLabel();
            Label succeed = generator.DefineLabel();
            List<CodeInstruction> list = instructions.ToList();

            List<CodeInstruction> targetSequence = new List<CodeInstruction>() {
                    new CodeInstruction(OpCodes.Ldc_R4, 0.25f)
                };
            List<CodeInstruction> patchSequence = new List<CodeInstruction>()
                {
                    new CodeInstruction(OpCodes.Ldarg_0),
                    CodeInstruction.LoadField(typeof(PLController), "MyPawn"),
                    CodeInstruction.Call(typeof(ExpandedGalaxy.Jetpack), "GetIsOnHomeShip", new Type[1]
                    {
                        typeof(PLPawn)
                    }),
                    new CodeInstruction(OpCodes.Brtrue_S, failed),
                    new CodeInstruction(OpCodes.Ldarg_0),
                    CodeInstruction.Call(typeof(ExpandedGalaxy.Jetpack), "TryRechargeFromReserve", new Type[1]
                    {
                        typeof(PLPlayerController)
                    }),
                    new CodeInstruction(OpCodes.Br, succeed)
                };
            list[40].labels.Add(failed);
            list[41].labels.Add(succeed);

            return HarmonyHelpers.PatchBySequence(list.AsEnumerable<CodeInstruction>(), targetSequence, patchSequence, HarmonyHelpers.PatchMode.BEFORE, HarmonyHelpers.CheckMode.NONNULL, false);
        }
    }
}
