using HarmonyLib;
using PulsarModLoader.Patches;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLCPU), "AddStats")]
    internal class CPUAddStatsPatch
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> list = instructions.ToList();

            List<CodeInstruction> targetSequence2 = new List<CodeInstruction>() {
                    new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(PLShipStats), "get_CyberAttackRating")),
                    new CodeInstruction(OpCodes.Ldc_R4),
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Ldc_R4),
                    new CodeInstruction(OpCodes.Ldc_R4),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(PLShipComponent), "LevelMultiplier")),
                    new CodeInstruction(OpCodes.Mul),
                };
            List<CodeInstruction> patchSequence2 = new List<CodeInstruction>()
                {
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(PLPoweredShipComponent), "GetPowerPercentInput")),
                    new CodeInstruction(OpCodes.Mul),
                };

            return HarmonyHelpers.PatchBySequence(list.AsEnumerable<CodeInstruction>(), targetSequence2, patchSequence2, HarmonyHelpers.PatchMode.AFTER, HarmonyHelpers.CheckMode.NONNULL, false);
        }
    }
}
