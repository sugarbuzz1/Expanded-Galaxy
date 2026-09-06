using HarmonyLib;
using PulsarModLoader.Patches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLMissle), "FixedUpdate")]
    internal class SeekerProjFix
    {
        public static bool IsSeekerProj(PLProjectile projectile)
        {
            if (projectile.OwnerShipID != -1 && projectile.TurretID != -1)
            {
                PLShipInfoBase ship = PLEncounterManager.Instance.GetShipFromID(projectile.OwnerShipID);
                PLTurret turret = ship.GetTurretAtID(projectile.TurretID);
                if (turret is SeekerTurret)
                    return true;
            }
            return false;
        }
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            Label failed = generator.DefineLabel();
            Label succeed = generator.DefineLabel();

            List<CodeInstruction> list = instructions.ToList();

            List<CodeInstruction> targetSequence = new List<CodeInstruction>() {
                    new CodeInstruction(OpCodes.Ldc_R4, 15f),
                };
            List<CodeInstruction> patchSequence = new List<CodeInstruction>()
                {
                    new CodeInstruction(OpCodes.Ldarg_0),
                    CodeInstruction.Call(typeof(SeekerProjFix), "IsSeekerProj", new Type[1] {typeof(PLProjectile)}),
                    new CodeInstruction(OpCodes.Brfalse_S, failed),
                    new CodeInstruction(OpCodes.Ldc_R4, 100f),
                    new CodeInstruction(OpCodes.Br_S, succeed),
                    new CodeInstruction(OpCodes.Ldc_R4, 15f),
                    new CodeInstruction(OpCodes.Nop)
                };
            patchSequence[patchSequence.Count - 1].labels.Add(succeed);
            patchSequence[patchSequence.Count - 2].labels.Add(failed);
            return HarmonyHelpers.PatchBySequence(list.AsEnumerable<CodeInstruction>(), targetSequence, patchSequence, HarmonyHelpers.PatchMode.REPLACE, HarmonyHelpers.CheckMode.NONNULL, false);
        }
    }
}
