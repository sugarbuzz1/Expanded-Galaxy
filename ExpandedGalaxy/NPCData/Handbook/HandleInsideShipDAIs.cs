using HarmonyLib;
using PulsarModLoader.Patches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLGameStatic), "Update")]
    internal class HandleInsideShipDAIs
    {
        internal static bool HelperMethod()
        {
            if (PLAbyssShipInfo.Instance != null)
                return true;
            PLPawn pawn = PLNetworkManager.Instance.MyLocalPawn;
            bool flag = pawn.CurrentShip != null;
            bool flag1 = false;
            if (flag && NPCData.InsideShipDAIs.Count > 0)
            {
                foreach (PLDialogueActorInstance pLDialogueActorInstance in NPCData.InsideShipDAIs)
                {
                    if (pLDialogueActorInstance != null)
                    {
                        if (pawn.MyCurrentTLI == pLDialogueActorInstance.TLIInParent && (pawn.transform.position - pLDialogueActorInstance.transform.position).sqrMagnitude < pLDialogueActorInstance.MaxRange * pLDialogueActorInstance.MaxRange)
                        {
                            flag1 = true;
                            break;
                        }
                    }
                }
            }
            return !flag || flag1;
        }
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> list = instructions.ToList();

            List<CodeInstruction> targetSequence = new List<CodeInstruction>() {
                    new CodeInstruction(OpCodes.Ldsfld, AccessTools.Field(typeof(PLNetworkManager), "Instance")),
                    new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(PLNetworkManager), "MyLocalPawn")),
                    new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(PLCombatTarget), "CurrentShip")),
                    new CodeInstruction(OpCodes.Ldnull),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(UnityEngine.Object), "op_Equality", new Type[] { typeof(UnityEngine.Object), typeof(UnityEngine.Object) })),
                    new CodeInstruction(OpCodes.Brtrue_S),
                    new CodeInstruction(OpCodes.Ldsfld, AccessTools.Field(typeof(PLAbyssShipInfo), "Instance")),
                    new CodeInstruction(OpCodes.Ldnull),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(UnityEngine.Object), "op_Inequality", new Type[] { typeof(UnityEngine.Object), typeof(UnityEngine.Object) })),
                };
            List<CodeInstruction> patchSequence = new List<CodeInstruction>()
                {
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(HandleInsideShipDAIs), "HelperMethod"))
                };

            return HarmonyHelpers.PatchBySequence(list.AsEnumerable<CodeInstruction>(), targetSequence, patchSequence, HarmonyHelpers.PatchMode.REPLACE, HarmonyHelpers.CheckMode.NONNULL, false);
        }
    }
}
