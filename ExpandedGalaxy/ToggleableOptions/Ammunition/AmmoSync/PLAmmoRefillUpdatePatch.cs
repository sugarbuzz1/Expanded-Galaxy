using HarmonyLib;
using PulsarModLoader.Patches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLAmmoRefill), "Update")]
    internal class PLAmmoRefillUpdatePatch
    {
        internal static bool ShouldRefillPlayer(PLAmmoRefill ammoRefill, PLPlayer player)
        {
            if (player == null)
                return false;
            if (ammoRefill.MyTLI != null && ammoRefill.MyTLI.MyShipInfo != null && (ammoRefill.MyTLI.MyShipInfo.TeamID == -1 || ammoRefill.MyTLI.MyShipInfo.TeamID == player.TeamID))
                return true;
            if (ammoRefill is PLAmmoRefill_Arena)
                return true;
            return false;
        }

        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> list = instructions.ToList();

            List<CodeInstruction> targetSequence = new List<CodeInstruction>() {
                    new CodeInstruction(OpCodes.Ldloc_2),
                    new CodeInstruction(OpCodes.Ldnull),
                    new CodeInstruction(OpCodes.Call),
                };
            List<CodeInstruction> patchSequence = new List<CodeInstruction>()
                {
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Ldloc_2),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(PLAmmoRefillUpdatePatch), "ShouldRefillPlayer", new Type[2] {typeof(PLAmmoRefill), typeof(PLPlayer)})),
                };

            return HarmonyHelpers.PatchBySequence(list.AsEnumerable<CodeInstruction>(), targetSequence, patchSequence, HarmonyHelpers.PatchMode.REPLACE, HarmonyHelpers.CheckMode.NONNULL, false);
        }
    }
}
