using HarmonyLib;
using PulsarModLoader.Content.Components.Reactor;
using PulsarModLoader.Patches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using UnityEngine;

namespace ExpandedGalaxy
{
    internal class Reactors
    {
        private class DarkReactor : ReactorMod
        {
            public override string Name => "Dark-Matter Reactor";

            public override string Description => "This reactor creates power at a scale unlike any other. Interestingly, it seems to have no reaction to ANY form of coolant.";

            public override int MarketPrice => 50500;

            public override float EnergyOutputMax => 46000f;

            public override float MaxTemp => 4050f;

            public override float EmergencyCooldownTime => 300f;

            public override float EnergySignatureAmount => 9f;

            public override float HeatOutput => 1f;

            public override void Tick(PLShipComponent InComp)
            {
                if (!InComp.IsEquipped || InComp.ShipStats == null || !(InComp.ShipStats.Ship is PLShipInfo) || !((UnityEngine.Object)(InComp.ShipStats.Ship as PLShipInfo).ReactorInstance != (UnityEngine.Object)null))
                    return;
                PLReactor plReactor = InComp as PLReactor;
                float num = Mathf.Clamp01(plReactor.ShipStats.ReactorTempCurrent * plReactor.ShipStats.ReactorTotalUsagePercent / plReactor.ShipStats.ReactorTempMax);
                float num1 = Mathf.Clamp01(plReactor.ShipStats.ReactorTotalUsagePercent / 0.75f);
                float num2 = 1f - Mathf.Clamp01(0.5f * num + 0.5f * num1);
                Color color = new Color(85f * num2 / 255f, 0f, 255f * num2 / 255f);
                Color color1 = new Color((35f + 50f * num2) / 255f, 0f, 1f);

                if (InComp.ShipStats.Ship is PLShipInfo)
                {
                    foreach (Light componentsInChild in (InComp.ShipStats.Ship as PLShipInfo).ReactorInstance.GetComponentsInChildren<Light>())
                        componentsInChild.color = color1;
                    foreach (ParticleSystem componentsInChild in (InComp.ShipStats.Ship as PLShipInfo).ReactorInstance.GetComponentsInChildren<ParticleSystem>())
                    {
                        if (componentsInChild != (InComp.ShipStats.Ship as PLShipInfo).ReactorInstance.CenterParticles && componentsInChild != (InComp.ShipStats.Ship as PLShipInfo).ReactorInstance.WhiteParticles && componentsInChild != (InComp.ShipStats.Ship as PLShipInfo).ReactorInstance.FlareAltParticles && componentsInChild != (InComp.ShipStats.Ship as PLShipInfo).ReactorInstance.FlareParticles && componentsInChild != (InComp.ShipStats.Ship as PLShipInfo).ReactorInstance.VerticalFlareParticles)
                            componentsInChild.startColor = color;
                    }

                }
            }
        }

        [HarmonyPatch(typeof(PLReactor), "ShipUpdate")]
        internal class noCoolant
        {
            private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
            {
                Label failed = generator.DefineLabel();
                List<CodeInstruction> list = instructions.ToList();

                List<CodeInstruction> targetSequence = new List<CodeInstruction>() {
                    new CodeInstruction(OpCodes.Mul),
                    new CodeInstruction(OpCodes.Ldloc_2),
                    new CodeInstruction(OpCodes.Mul),
                    new CodeInstruction(OpCodes.Ldloc_3)
                };
                List<CodeInstruction> patchSequence = new List<CodeInstruction>()
                {
                    new CodeInstruction(OpCodes.Ldarg_1),
                    new CodeInstruction(OpCodes.Brfalse_S, failed),
                    new CodeInstruction(OpCodes.Ldarg_1),
                    CodeInstruction.Call(typeof(PLWare), "get_Name"),
                    new CodeInstruction(OpCodes.Ldstr, "Dark-Matter Reactor"),
                    CodeInstruction.Call(typeof(System.String), "op_Equality", new Type[2] {
                        typeof(string),
                        typeof(string)
                    }),
                    new CodeInstruction(OpCodes.Brfalse, failed),
                    new CodeInstruction(OpCodes.Ldc_R4, 0f),
                    new CodeInstruction(OpCodes.Stloc_1)
                };
                list[120].labels.Add(failed);

                return HarmonyHelpers.PatchBySequence(list.AsEnumerable<CodeInstruction>(), targetSequence, patchSequence, HarmonyHelpers.PatchMode.AFTER, HarmonyHelpers.CheckMode.ALWAYS, false);
            }
        }

        [HarmonyPatch(typeof(PLWarpDriveProgram), "ExecuteBasedOnType")]
        internal class noDigiCooolant
        {
            private static bool Prefix(PLWarpDriveProgram __instance)
            {
                if (__instance.ShipStats.GetShipComponent<PLReactor>(ESlotType.E_COMP_REACTOR) != null && __instance.ShipStats.GetShipComponent<PLReactor>(ESlotType.E_COMP_REACTOR).SubType == ReactorModManager.Instance.GetReactorIDFromName("Dark-Matter Reactor") && __instance.SubType == (int)EWarpDriveProgramType.DIG_COOLANT)
                {
                    __instance.Level = 0;
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(PLReactor), "Equip")]
        internal class ResetColor
        {
            private static void Postfix(PLReactor __instance)
            {
                if (__instance.ShipStats == null && __instance.ShipStats.Ship == null)
                    return;
                if (__instance.ShipStats.Ship is PLShipInfo)
                {
                    if ((__instance.ShipStats.Ship as PLShipInfo).ReactorInstance == null)
                        return;
                    foreach (Light componentsInChild in (__instance.ShipStats.Ship as PLShipInfo).ReactorInstance.GetComponentsInChildren<Light>())
                        componentsInChild.color = Color.white;
                    foreach (ParticleSystem componentsInChild in (__instance.ShipStats.Ship as PLShipInfo).ReactorInstance.GetComponentsInChildren<ParticleSystem>())
                        componentsInChild.startColor = Color.white;
                }
            }
        }

        [HarmonyPatch(typeof(PLReactorInstance), "Update")]
        internal class CustomColors
        {
            private static void Postfix(PLReactorInstance __instance, ref float ___SmoothTotalUsagePercent)
            {
                if (!((UnityEngine.Object)__instance.MyShipInfo != (UnityEngine.Object)null) || !((UnityEngine.Object)PLServer.Instance != (UnityEngine.Object)null))
                    return;
                if (!(__instance.MyShipInfo.MyStats.GetShipComponent<PLReactor>(ESlotType.E_COMP_REACTOR) != null && __instance.MyShipInfo.MyStats.GetShipComponent<PLReactor>(ESlotType.E_COMP_REACTOR).SubType == ReactorModManager.Instance.GetReactorIDFromName("Dark-Matter Reactor")))
                    return;
                float num = Mathf.Clamp01(__instance.MyShipInfo.CoreInstability / 0.75f);
                float num1 = Mathf.Clamp01(__instance.MyShipInfo.MyStats.ReactorTotalUsagePercent / 0.75f);
                float num2 = Mathf.Clamp01(0.5f * num + 0.5f * num1);
                if ((UnityEngine.Object)__instance.WhiteParticles != (UnityEngine.Object)null)
                {
                    __instance.WhiteParticles.emissionRate = Mathf.Clamp01(___SmoothTotalUsagePercent) * 150f;
                }
                if ((UnityEngine.Object)__instance.FlareParticles != (UnityEngine.Object)null)
                {
                    __instance.FlareParticles.emissionRate = (float)(5.0 + (double)Mathf.Clamp01(num2 - 0.2f) * 8.0);
                    __instance.FlareParticles.startColor = new Color((35f + 50f * num2) / 255f, 0f, 1f);
                    __instance.FlareParticles.startSize = Mathf.Clamp01(num2 - 0.2f) * 14f;
                }
                if ((UnityEngine.Object)__instance.FlareAltParticles != (UnityEngine.Object)null)
                {
                    __instance.FlareAltParticles.emissionRate = (float)(5.0 + (double)Mathf.Clamp01(num2 - 0.2f) * 4.0);
                    __instance.FlareAltParticles.startColor = new Color(35f / 255f, 0f, 1f);
                    __instance.FlareAltParticles.startSize = Mathf.Clamp01(num2 - 0.2f) * 18f;
                }
                if ((UnityEngine.Object)__instance.VerticalFlareParticles != (UnityEngine.Object)null)
                {
                    __instance.VerticalFlareParticles.emissionRate = (float)(5.0 + (double)Mathf.Clamp01(num2 - 0.5f) * 12.0);
                    __instance.VerticalFlareParticles.startColor = new Color(45f / 255f, 0f, 1f);
                    __instance.VerticalFlareParticles.startSize = Mathf.Clamp01(num2 - 0.5f) * 12f;
                }
                if (!((UnityEngine.Object)__instance.CenterParticles != (UnityEngine.Object)null))
                    return;
                __instance.CenterParticles.startColor = new Color(45f / 255f, 0f, 1f, num2 + 0.2f);
                __instance.CenterParticles.emissionRate = (float)(50.0 + (double)Mathf.Clamp01(num2 - 0.2f) * 150.0);
                __instance.CenterParticles.startSize = (float)(0.15000000596046448 + (double)num2 * 0.20000000298023224);
            }
        }

        private class MiningDroneReactor : ReactorMod
        {
            public override string Name => "Extraction Drone Reactor";

            public override string Description => "If you're reading this, I fucked up :(";

            public override int MarketPrice => 50500;

            public override float EnergyOutputMax => 50000f;

            public override float MaxTemp => 4500f;

            public override float EmergencyCooldownTime => 5f;

            public override float EnergySignatureAmount => 24f;

            public override bool CanBeDroppedOnShipDeath => false;
        }
    }
}
