using HarmonyLib;
using PulsarModLoader.Patches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLGameStatic), "Update")]
    internal class ShowAbilityText
    {

        private static string bottomInfo = "";
        private static string bottomInput = "";

        public static void setBottomInfoText(string infoText, string inputText = "")
        {
            bottomInfo = infoText;
            bottomInput = inputText;
        }
        private static void Postfix(PLGameStatic __instance)
        {
            if (PLNetworkManager.Instance.MyLocalPawn != null && PLNetworkManager.Instance.LocalPlayer != null)
            {
                Traverse traverse = Traverse.Create(PLGlobal.Instance);
                if (PLCameraSystem.Instance.CurrentCameraMode != null && (PLCameraSystem.Instance.CurrentCameraMode.GetModeString() == "Pilot" || PLCameraSystem.Instance.CurrentCameraMode.GetModeString() == "SensorDish"))
                {
                    traverse.Field("m_BottomInfoLabelString").SetValue("");
                    traverse.Field("m_BottomInfoLabelString_InputAction").SetValue("");
                    traverse.Field("m_BottomInfoLabelStringTop").SetValue("");
                    traverse.Field("m_BottomInfoLabelStringTop_InputAction").SetValue("");
                }
                else
                {
                    if (PLGlobal.Instance.BottomInfoLabelString == "")
                    {
                        traverse.Field("m_BottomInfoLabelString").SetValue(bottomInfo);
                        traverse.Field("m_BottomInfoLabelString_InputAction").SetValue(bottomInput);
                    }
                }
            }
        }

        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            Label failed = generator.DefineLabel();
            Label failed2 = generator.DefineLabel();
            List<CodeInstruction> list = instructions.ToList();

            List<CodeInstruction> targetSequence = new List<CodeInstruction>() {
                    CodeInstruction.LoadField(typeof(CargoObjectDisplay), "DisplayedItem"),
                    new CodeInstruction(OpCodes.Callvirt),
                    new CodeInstruction(OpCodes.Ldc_I4_S, (sbyte)21),
                    new CodeInstruction(OpCodes.Bne_Un)
                };
            List<CodeInstruction> patchSequence = new List<CodeInstruction>()
                {
                    CodeInstruction.Call(typeof(ExpandedGalaxy.Systems), "IsCargoScrappable", new Type[1] {
                        typeof(CargoObjectDisplay)
                    }),
                    new CodeInstruction(OpCodes.Brfalse_S, failed),
                };
            list[4998].labels.Add(failed);
            list[5772].labels.Add(failed2);

            List<CodeInstruction> list2 = HarmonyHelpers.PatchBySequence(list.AsEnumerable<CodeInstruction>(), targetSequence, patchSequence, HarmonyHelpers.PatchMode.REPLACE, HarmonyHelpers.CheckMode.NONNULL, false).ToList();

            List<CodeInstruction> targetSequence2 = new List<CodeInstruction>()
                {
                    new CodeInstruction(OpCodes.Ldstr, "Leave Captain's Chair"),
                    new CodeInstruction(OpCodes.Ldc_I4_0),
                    new CodeInstruction(OpCodes.Call),
                    new CodeInstruction(OpCodes.Ldstr, ""),
                    new CodeInstruction(OpCodes.Ldstr, "activate_station"),
                    new CodeInstruction(OpCodes.Callvirt),
                    new CodeInstruction(OpCodes.Ldsfld),
                    new CodeInstruction(OpCodes.Ldc_I4_S),
                    new CodeInstruction(OpCodes.Callvirt),
                    new CodeInstruction(OpCodes.Brfalse)
                };
            List<CodeInstruction> patchSequence2 = new List<CodeInstruction>()
                {
                    CodeInstruction.LoadField(typeof(PLNetworkManager), "Instance"),
                    CodeInstruction.LoadField(typeof(PLNetworkManager), "LocalPlayer"),
                    new CodeInstruction(OpCodes.Ldnull),
                    CodeInstruction.Call(typeof(UnityEngine.Object), "op_Inequality", new Type[2]
                    {
                        typeof(UnityEngine.Object),
                        typeof(UnityEngine.Object)
                    }),
                    new CodeInstruction(OpCodes.Brfalse, failed2),
                    CodeInstruction.LoadField(typeof(PLNetworkManager), "Instance"),
                    CodeInstruction.LoadField(typeof(PLNetworkManager), "LocalPlayer"),
                    new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(PLPlayer), "GetPlayerID")),
                    CodeInstruction.Call(typeof(ExpandedGalaxy.Systems), "IsPlayerPiloting", new Type[1]
                    {
                        typeof(int)
                    }),
                    new CodeInstruction(OpCodes.Brtrue, failed2)
                };

            return HarmonyHelpers.PatchBySequence(list2.AsEnumerable<CodeInstruction>(), targetSequence2, patchSequence2, HarmonyHelpers.PatchMode.AFTER, HarmonyHelpers.CheckMode.NONNULL, false);
        }
    }
}

