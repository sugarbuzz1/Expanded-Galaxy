using HarmonyLib;
using PulsarModLoader.Patches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Talents.Framework;
using UnityEngine.UI;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLInGameUI), "Update")]
    internal class JetpackFuelReserveUI
    {
        internal static void HelperMethod(PLInGameUI pLInGameUI)
        {
            if ((int)PLNetworkManager.Instance.MyLocalPawn.GetPlayer().Talents[TalentModManager.Instance.GetTalentIDFromName("Jetpack Fuel Reserve")] > 0)
            {
                PLInGameUI.SafeSetImageFillAmount(pLInGameUI.FuelFill, Jetpack.Reserve * 0.25f);
                PLInGameUI.SafeSetImageFillAmount(pLInGameUI.FuelFillOut, PLNetworkManager.Instance.MyLocalPawn.MyController.JetpackFuel * 0.25f);
            }
        }

        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> list = instructions.ToList();

            List<CodeInstruction> targetSequence = new List<CodeInstruction>()
            {
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Ldfld),
                new CodeInstruction(OpCodes.Ldsfld),
                new CodeInstruction(OpCodes.Ldfld),
                new CodeInstruction(OpCodes.Ldfld),
                new CodeInstruction(OpCodes.Callvirt),
                new CodeInstruction(OpCodes.Ldc_R4, 0.25f),
                new CodeInstruction(OpCodes.Mul),
                CodeInstruction.Call(typeof(PLInGameUI), "SafeSetImageFillAmount", new Type[2]
                    {
                        typeof(Image),
                        typeof(float)
                    }),
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Ldfld),
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Ldfld),
                new CodeInstruction(OpCodes.Callvirt),
                CodeInstruction.Call(typeof(PLInGameUI), "SafeSetImageFillAmount", new Type[2]
                    {
                        typeof(Image),
                        typeof(float)
                    }),
            };
            List<CodeInstruction> patchSequence = new List<CodeInstruction>()
                {
                    new CodeInstruction(OpCodes.Ldarg_0),
                    CodeInstruction.Call(typeof(ExpandedGalaxy.JetpackFuelReserveUI), "HelperMethod", new Type[1]
                    {
                        typeof(PLInGameUI)
                    }),
                };
            return HarmonyHelpers.PatchBySequence(list.AsEnumerable<CodeInstruction>(), targetSequence, patchSequence, HarmonyHelpers.PatchMode.AFTER, HarmonyHelpers.CheckMode.NONNULL, false);
        }
    }
}
