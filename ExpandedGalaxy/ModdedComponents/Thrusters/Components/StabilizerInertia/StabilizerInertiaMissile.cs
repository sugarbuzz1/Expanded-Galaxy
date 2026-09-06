using HarmonyLib;
using PulsarModLoader.Content.Components.InertiaThruster;
using PulsarModLoader.Patches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLMissle), "Explode")]
    internal class StabilizerInertiaMissile
    {
        internal static float HelperMethod(PLMissle missle, PLShipInfoBase target)
        {
            bool flag = false;
            foreach (PLInertiaThruster thruster in target.MyStats.GetComponentsOfType(ESlotType.E_COMP_INERTIA_THRUSTER))
            {
                if (thruster.SubType == InertiaThrusterModManager.Instance.GetInertiaThrusterIDFromName("Integrated Stabilizer Thruster"))
                {
                    flag = true;
                    break;
                }
            }
            return flag ? 0f : missle.MaxDamage;
        }
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> list = instructions.ToList();

            List<CodeInstruction> targetSequence = new List<CodeInstruction>() {
                    new CodeInstruction(OpCodes.Ldloc_3),
                    new CodeInstruction(OpCodes.Ldfld),
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Ldfld),
                };
            List<CodeInstruction> patchSequence = new List<CodeInstruction>()
                {
                    new CodeInstruction(OpCodes.Ldloc_3),
                    new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(PLShipInfoBase), "ExteriorRigidbody")),
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Ldloc_3),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(StabilizerInertiaMissile), "HelperMethod", new Type[2] {typeof(PLMissle), typeof(PLShipInfoBase)}))
                };

            return HarmonyHelpers.PatchBySequence(list.AsEnumerable<CodeInstruction>(), targetSequence, patchSequence, HarmonyHelpers.PatchMode.AFTER, HarmonyHelpers.CheckMode.NONNULL, false);
        }
    }
}
