using ExpandedGalaxy.SensorDish;
using HarmonyLib;
using PulsarModLoader.Patches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLTurret), "Tick")]
    internal class AncientSensorMissileLock
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            Label failed = generator.DefineLabel();
            Label succeed = generator.DefineLabel();
            List<CodeInstruction> list = instructions.ToList();

            List<CodeInstruction> targetSequence = new List<CodeInstruction>() {
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Ldarg_0),
                    CodeInstruction.LoadField(typeof(PLTurret), "LockedOnAmount"),
                    CodeInstruction.Call(typeof(UnityEngine.Time), "get_deltaTime"),
                };
            List<CodeInstruction> patchSequence = new List<CodeInstruction>()
                {
                    new CodeInstruction(OpCodes.Ldarg_0),
                    CodeInstruction.Call(typeof(AncientSensorMissileLock), "GetTurretMissileTarget", new Type[1]
                    {
                        typeof(PLTurret)
                    }),
                    new CodeInstruction(OpCodes.Ldc_I4_5),
                    new CodeInstruction(OpCodes.Ldc_I4_1),
                    CodeInstruction.Call(typeof(SensorDishModManager), "IsSensorWeaknessActiveModded", new Type[3] {
                        typeof(PLShipInfoBase),
                        typeof(int),
                        typeof(int)
                    }),
                    new CodeInstruction(OpCodes.Brfalse_S, failed),
                    new CodeInstruction(OpCodes.Ldc_R4, 1.35f),
                    new CodeInstruction(OpCodes.Br_S, succeed)
                };
            list[1406].labels.Add(failed);
            list[1407].labels.Add(succeed);

            return HarmonyHelpers.PatchBySequence(list.AsEnumerable<CodeInstruction>(), targetSequence, patchSequence, HarmonyHelpers.PatchMode.AFTER, HarmonyHelpers.CheckMode.NONNULL, false);
        }

        public static PLShipInfoBase GetTurretMissileTarget(PLTurret turret)
        {
            return turret.TargetedMissileLockShip;
        }
    }
}
