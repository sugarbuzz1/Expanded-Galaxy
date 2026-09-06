using HarmonyLib;
using PulsarModLoader.Patches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using UnityEngine;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLScientistComputerScreen), "Update")]
    internal class ProgramFGColor
    {
        internal static Color GetFGColorForProgram(PLWarpDriveProgram program)
        {
            if (program.Experimental)
                return new Color(0.7f, 0.7f, 0.1f, 1f);
            else if (program.Contraband)
                return new Color(0.8f, 0f, 0f, 1f);
            else if (Relic.GetIsRelic(program))
                return Relic.GetRelicColor();
            return new Color(0.65f, 0.65f, 0.65f, 1f);
        }
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> list = instructions.ToList();
            LocalBuilder index1 = null;
            List<Label> label = new List<Label>();

            for (int i = 0; i < list.Count; i++)
            {
                CodeInstruction codeInstruction = list[i];
                if (codeInstruction.opcode == OpCodes.Ldloc_S && codeInstruction.operand is LocalBuilder lb1 && lb1.LocalIndex == 40)
                {
                    index1 = lb1;
                    if (codeInstruction.labels.Count > 0 && i > 1200)
                        label.AddRange(codeInstruction.labels);
                }
                if (index1 != null && label.Count > 0)
                    break;
            }

            List<CodeInstruction> targetSequence = new List<CodeInstruction>() {
                    new CodeInstruction(OpCodes.Ldloc_S, index1),
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(PLUIScreen), "UI_White")),
                };
            List<CodeInstruction> patchSequence = new List<CodeInstruction>()
                {
                    new CodeInstruction(OpCodes.Ldloc_S, (byte)40),
                    new CodeInstruction(OpCodes.Ldloc_S, (byte)43),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(ProgramFGColor), "GetFGColorForProgram", new Type[1] {typeof(PLWarpDriveProgram)})),
                };

            List<CodeInstruction> list2 = HarmonyHelpers.PatchBySequence(list.AsEnumerable<CodeInstruction>(), targetSequence, patchSequence, HarmonyHelpers.PatchMode.REPLACE, HarmonyHelpers.CheckMode.NONNULL, false).ToList<CodeInstruction>();
            patchSequence[0].labels.AddRange(label);
            return HarmonyHelpers.PatchBySequence(list2.AsEnumerable<CodeInstruction>(), targetSequence, patchSequence, HarmonyHelpers.PatchMode.REPLACE, HarmonyHelpers.CheckMode.NONNULL, false);
        }

        private static void Postfix(PLScientistComputerScreen __instance, ref UILabel ___Status_EMDet, ref UILabel ___MainScreen_EMLabel, ref Dictionary<string, UIWidget> ___UIElements)
        {
            if (!__instance.UIIsSetup() || __instance.MyScreenHubBase == null || __instance.MyScreenHubBase.OptionalShipInfo == null)
                return;
            switch (SensorScreenUpdate.SensorMode)
            {
                case 0:
                    ((UILabel)___UIElements["Status_EMDetTitle"]).text = "EM Detection";
                    ((UILabel)___UIElements["MainScreen_EMLabelTop"]).text = "EM Signature";
                    ___Status_EMDet.text = __instance.MyScreenHubBase.OptionalShipInfo.MyStats.EMDetection.ToString("0.0");
                    ___MainScreen_EMLabel.text = __instance.MyScreenHubBase.OptionalShipInfo.MyStats.EMSignature.ToString("0.0");
                    break;
                case 1:
                    ((UILabel)___UIElements["Status_EMDetTitle"]).text = "LF Detection";
                    ((UILabel)___UIElements["MainScreen_EMLabelTop"]).text = "LF Signature";
                    ___Status_EMDet.text = __instance.MyScreenHubBase.OptionalShipInfo.MyStats.LFDetection.ToString("0.0");
                    ___MainScreen_EMLabel.text = (__instance.MyScreenHubBase.OptionalShipInfo.MyStats.LFSignature / 0.125f).ToString("0.0");
                    break;
                case 2:
                    ((UILabel)___UIElements["Status_EMDetTitle"]).text = "NT Detection";
                    ((UILabel)___UIElements["MainScreen_EMLabelTop"]).text = "NT Signature";
                    ___Status_EMDet.text = __instance.MyScreenHubBase.OptionalShipInfo.MyStats.RADetection.ToString("0.0");
                    ___MainScreen_EMLabel.text = __instance.MyScreenHubBase.OptionalShipInfo.MyStats.QTSignature.ToString("0.0");
                    break;
                case 3:
                    ((UILabel)___UIElements["Status_EMDetTitle"]).text = "GV Detection";
                    ((UILabel)___UIElements["MainScreen_EMLabelTop"]).text = "Mass";
                    ___Status_EMDet.text = __instance.MyScreenHubBase.OptionalShipInfo.MyStats.QTDetection.ToString("0.0");
                    ___MainScreen_EMLabel.text = __instance.MyScreenHubBase.OptionalShipInfo.MyStats.Mass.ToString("0");
                    break;
            }
        }
    }
}

