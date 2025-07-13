using HarmonyLib;
using PulsarModLoader.Patches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Talents.Framework;
using UnityEngine;

namespace ExpandedGalaxy
{
    internal class Jetpack
    {
        private static float Reserve = 0f;
        internal static bool AdvancedJetPack = true;

        public static bool GetIsOnHomeShip(PLPawn pawn)
        {
            if (pawn.GetPlayer() != null && pawn.GetPlayer().GetPlayerID() == PLNetworkManager.Instance.LocalPlayer.GetPlayerID())
            {
                bool flag = false;
                if (pawn.MyCurrentTLI != null && pawn.MyCurrentTLI.MyShipInfo != null && pawn.MyCurrentTLI.MyShipInfo.ShipID == PLEncounterManager.Instance.PlayerShip.ShipID)
                    flag = true;
                if (AdvancedJetPack && pawn.GetPlayer() != null)
                {
                    if (pawn.GetPlayer().Talents[TalentModManager.Instance.GetTalentIDFromName("Jetpack Fuel Reserve")] > 0 && flag && !(pawn.MyController.JetpackFuel < 1f))
                        Reserve += 0.25f * Time.deltaTime;
                    Reserve = Mathf.Clamp(Reserve, 0f, 0.25f * pawn.GetPlayer().Talents[TalentModManager.Instance.GetTalentIDFromName("Jetpack Fuel Reserve")]);
                }
                return !AdvancedJetPack || flag;
            }
            return true;
        }

        public static float TryRechargeFromReserve(PLPlayerController playerController)
        {
            PLPawn pawn = playerController.MyPawn;
            if (pawn == null)
                return 0f;
            if (playerController.MyPawn.GetPlayer() == null || playerController.MyPawn.GetPlayer().GetPlayerID() != PLNetworkManager.Instance.LocalPlayer.GetPlayerID())
                return 0.25f;
            if (Reserve > 0f && (pawn.MyCurrentTLI != null && pawn.MyCurrentTLI.MyShipInfo == null || pawn.MyCurrentTLI.MyShipInfo.ShipID != PLEncounterManager.Instance.PlayerShip.ShipID))
            {
                if (playerController.JetpackFuel < 1f)
                {
                    Reserve -= 0.25f * Time.deltaTime;
                    Reserve = Mathf.Clamp(Reserve, 0f, 0.25f * pawn.GetPlayer().Talents[TalentModManager.Instance.GetTalentIDFromName("Jetpack Fuel Reserve")]);
                }
                return 0.25f;
            }
            else
            {
                if (playerController.JetpackFuel < 0.05f)
                    return 0.25f;
            }
            return 0f;
        }
    }

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
