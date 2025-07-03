using HarmonyLib;
using PulsarModLoader.Content.Components.InertiaThruster;
using PulsarModLoader.Patches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace ExpandedGalaxy
{
    internal class Thrusters
    {
        private class StabilizerMod : InertiaThrusterMod
        {
            public override string Name => "Integrated Stabilizer Thruster";

            public override string Description => "Inertia thruster with an automatic rotation control system that eliminates turret and missile knockback";

            public override float MaxOutput => 0.37f;

            public override float MaxPowerUsage_Watts => 4000f;

            public override int MarketPrice => 8000;
        }

        public static bool hasInertiaThrusterOfName(PLShipStats stats, string name)
        {
            bool flag = false;
            foreach (PLInertiaThruster thruster in stats.GetComponentsOfType(ESlotType.E_COMP_INERTIA_THRUSTER))
            {
                if (thruster.Name == name)
                    flag = true;
            }
            return flag;
        }

        [HarmonyPatch(typeof(PLMegaTurret), "ChargeComplete")]
        internal class StabilizerInertiaTurret
        {
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
                    new CodeInstruction(OpCodes.Ldstr, "Integrated Stabilizer Thruster"),
                    CodeInstruction.Call(typeof(ExpandedGalaxy.Thrusters), "hasInertiaThrusterOfName", new Type[2] {
                        typeof(PLShipStats),
                        typeof(string)
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

        [HarmonyPatch(typeof(PLMissle), "Explode")]
        internal class StabilizerInertiaMissile
        {
            private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
            {
                Label failed = generator.DefineLabel();
                Label succeed = generator.DefineLabel();
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
                    CodeInstruction.LoadField(typeof(PLShipInfoBase), "MyStats"),
                    new CodeInstruction(OpCodes.Ldstr, "Integrated Stabilizer Thruster"),
                    CodeInstruction.Call(typeof(ExpandedGalaxy.Thrusters), "hasInertiaThrusterOfName", new Type[2] {
                        typeof(PLShipStats),
                        typeof(string)
                    }),
                    new CodeInstruction(OpCodes.Brfalse_S, failed),
                    new CodeInstruction(OpCodes.Ldc_R4, 0f),
                    new CodeInstruction(OpCodes.Br_S, succeed)
                };
                list[127].labels.Add(failed);
                list[128].labels.Add(succeed);

                return HarmonyHelpers.PatchBySequence(list.AsEnumerable<CodeInstruction>(), targetSequence, patchSequence, HarmonyHelpers.PatchMode.AFTER, HarmonyHelpers.CheckMode.NONNULL, false);
            }
        }
    }
}
