using HarmonyLib;
using PulsarModLoader.Content.Components.InertiaThruster;
using PulsarModLoader.Patches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLMegaTurret), "ChargeComplete")]
    internal class StabilizerInertiaTurret
    {
        internal static bool HelperMethod(PLShipStats pLShipStats)
        {
            bool flag = false;
            foreach (PLInertiaThruster thruster in pLShipStats.GetComponentsOfType(ESlotType.E_COMP_INERTIA_THRUSTER))
            {
                if (thruster.SubType == InertiaThrusterModManager.Instance.GetInertiaThrusterIDFromName("Integrated Stabilizer Thruster"))
                {
                    flag = true;
                    break;
                }
            }
            return flag;
        }

        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            Label failed = generator.DefineLabel();
            Label succeed = generator.DefineLabel();
            List<CodeInstruction> list = instructions.ToList();

            List<CodeInstruction> targetSequence = new List<CodeInstruction>() {
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Call),
                    new CodeInstruction(OpCodes.Callvirt),
                    new CodeInstruction(OpCodes.Ldfld),
                    new CodeInstruction(OpCodes.Callvirt)
                };
            List<CodeInstruction> patchSequence = new List<CodeInstruction>()
                {
                    new CodeInstruction(OpCodes.Ldarg_0),
                    CodeInstruction.Call(typeof(PLMegaTurret), "get_ShipStats"),
                    CodeInstruction.Call(typeof(StabilizerInertiaTurret), "HelperMethod", new Type[1] {
                        typeof(PLShipStats),
                    }),
                    new CodeInstruction(OpCodes.Brfalse_S, failed),
                    new CodeInstruction(OpCodes.Ldc_R4, 0f),
                    new CodeInstruction(OpCodes.Br_S, succeed)
                };
            list[14].labels.Add(failed);
            list[15].labels.Add(succeed);

            return HarmonyHelpers.PatchBySequence(list.AsEnumerable<CodeInstruction>(), targetSequence, patchSequence, HarmonyHelpers.PatchMode.AFTER, HarmonyHelpers.CheckMode.NONNULL, false);
        }
    }
}
