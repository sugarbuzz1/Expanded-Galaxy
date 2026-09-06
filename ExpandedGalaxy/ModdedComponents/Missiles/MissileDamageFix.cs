using HarmonyLib;
using PulsarModLoader.Patches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using UnityEngine;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLTurret), "ServerFireMissile")]
    internal class MissileDamageFix
    {
        internal static void HelperMethod(PLMissle missle, PLTrackerMissile trackerMissile)
        {
            missle.MyDamageType = trackerMissile.DamageType;
            if (PLNetworkManager.Instance.IsInternalBuild)
                Debug.Log("[ExGal] Missile damage corrected: " + missle.MyDamageType.ToString());
        }
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> list = instructions.ToList();

            List<CodeInstruction> targetSequence = new List<CodeInstruction>() {
                    new CodeInstruction(OpCodes.Ldc_I4_1),
                    new CodeInstruction(OpCodes.Sub),
                    new CodeInstruction(OpCodes.Conv_I2),
                    new CodeInstruction(OpCodes.Callvirt),
                };
            List<CodeInstruction> patchSequence = new List<CodeInstruction>()
                {
                    new CodeInstruction(OpCodes.Ldloc_0),
                    new CodeInstruction(OpCodes.Ldarg_1),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(MissileDamageFix), "HelperMethod", new Type[2] {typeof(PLMissle), typeof(PLTrackerMissile)}))
                };

            return HarmonyHelpers.PatchBySequence(list.AsEnumerable<CodeInstruction>(), targetSequence, patchSequence, HarmonyHelpers.PatchMode.AFTER, HarmonyHelpers.CheckMode.NONNULL, false);
        }
    }
}
