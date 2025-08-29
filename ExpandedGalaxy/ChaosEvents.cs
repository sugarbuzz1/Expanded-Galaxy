using HarmonyLib;
using PulsarModLoader.Patches;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;

namespace ExpandedGalaxy
{
    internal class ChaosEvents
    {
        [HarmonyPatch(typeof(PLServer), "Update")]
        internal class HandleChaosEvents
        {
            private static bool Prefix()
            {
                if (PLServer.Instance != null)
                    if (Mathf.FloorToInt((float)PLServer.Instance.ChaosLevel) > (int)PLServer.Instance.OldChaosLevel && (double)PLServer.Instance.GetProcessedChaosLevel() <= 6.0)
                    {
                        Traverse traverse = Traverse.Create(PLServer.Instance);
                        DeactivateAllChaosEvents();
                        PLRand rand = new PLRand(Mathf.FloorToInt(PLGlobal.Instance.Galaxy.Seed + PLServer.Instance.ChaosLevel + 1));
                        for (int i = 0; i < 100; i++)
                        {
                            EChaosEvent e = (EChaosEvent)rand.Next(0, 10);
                            if (CanActivateChaosEvent(e))
                            {
                                PLServer.Instance.SetChaosEventAsActive((int)e);
                                PLServer.Instance.OnChaosEventActivate(e);
                                break;
                            }
                        }
                    }
                return true;
            }
        }

        private static void DeactivateAllChaosEvents()
        {
            for (int i = 0; i < (int)EChaosEvent.MAX; i++)
            {
                if (PLServer.Instance.IsChaosEventActive((EChaosEvent)i))
                    OnChaosEventDeactivate((EChaosEvent)i);
                    
            }
            PLServer.Instance.ActiveChaosEvents = 0L;
        }

        [HarmonyPatch(typeof(PLServer), "OnChaosEventActivate")]
        internal class OnChaosEventActivatePatch
        {
            private static bool Prefix(PLServer __instance, EChaosEvent chaosEvent)
            {
                switch(chaosEvent)
                {
                    case EChaosEvent.E_INFECTION_BOOST:
                    case EChaosEvent.E_WD_OFFENSIVE:
                        return true;
                    case EChaosEvent.E_LONG_RANGE_PRICE_HIKE:
                        foreach (PLSectorInfo pLSectorInfo in PLGlobal.Instance.Galaxy.AllSectorInfos.Values)
                        {
                            if (pLSectorInfo.VisualIndication == ESectorVisualIndication.WARP_NETWORK_STATION && pLSectorInfo.MySPI.Faction == 0 && pLSectorInfo.IsPartOfLongRangeWarpNetwork)
                            {
                                pLSectorInfo.MySPI.Faction = 1;
                                pLSectorInfo.IsPartOfLongRangeWarpNetwork = false;
                            }
                            else if ((pLSectorInfo.VisualIndication == ESectorVisualIndication.COLONIAL_HUB || pLSectorInfo.VisualIndication == ESectorVisualIndication.FLUFFY_FACTORY_01 || pLSectorInfo.VisualIndication == ESectorVisualIndication.FLUFFY_FACTORY_02 || pLSectorInfo.VisualIndication == ESectorVisualIndication.FLUFFY_FACTORY_03) && pLSectorInfo.IsPartOfLongRangeWarpNetwork)
                                pLSectorInfo.IsPartOfLongRangeWarpNetwork = false;
                        }
                        break;
                }
                return false;
            }
        }

        private static void OnChaosEventDeactivate(EChaosEvent chaosEvent)
        {
            switch(chaosEvent)
            {
                case EChaosEvent.E_INFECTION_BOOST:
                    PLServer.Instance.FactionStrengthLevels[4] /= 4;
                    foreach(PLFactionInfo info in PLGlobal.Instance.Galaxy.AllFactions)
                    {
                        if (info.FactionID == 4)
                        {
                            info.FactionAI_Continuous_GalaxySpreadLimit /= 1.5f;
                            info.FactionAI_Continuous_GalaxySpreadFactor /= 1.85f;
                            break;
                        }
                    }
                    break;
                case EChaosEvent.E_WD_OFFENSIVE:
                    PLServer.Instance.FactionStrengthLevels[2] /= 4;
                    foreach (PLFactionInfo info in PLGlobal.Instance.Galaxy.AllFactions)
                    {
                        if (info.FactionID == 2)
                        {
                            info.FactionAI_Continuous_GalaxySpreadLimit /= 2f;
                            info.FactionAI_Continuous_GalaxySpreadFactor /= 6f;
                            break;
                        }
                    }
                    break;
                case EChaosEvent.E_LONG_RANGE_PRICE_HIKE:
                    foreach (PLSectorInfo pLSectorInfo in PLGlobal.Instance.Galaxy.AllSectorInfos.Values)
                    {
                        if (pLSectorInfo.VisualIndication == ESectorVisualIndication.WARP_NETWORK_STATION && pLSectorInfo.MySPI.Faction != 0 && !pLSectorInfo.IsPartOfLongRangeWarpNetwork)
                        {
                            pLSectorInfo.MySPI.Faction = 0;
                            pLSectorInfo.IsPartOfLongRangeWarpNetwork = true;
                        }
                        else if ((pLSectorInfo.VisualIndication == ESectorVisualIndication.COLONIAL_HUB || pLSectorInfo.VisualIndication == ESectorVisualIndication.FLUFFY_FACTORY_01 || pLSectorInfo.VisualIndication == ESectorVisualIndication.FLUFFY_FACTORY_02 || pLSectorInfo.VisualIndication == ESectorVisualIndication.FLUFFY_FACTORY_03) && !pLSectorInfo.IsPartOfLongRangeWarpNetwork)
                            pLSectorInfo.IsPartOfLongRangeWarpNetwork = true;
                    }
                    break;
            }
        }

        private static bool CanActivateChaosEvent(EChaosEvent chaosEvent)
        {
            if (PLServer.Instance == null)
                return false;
            switch (chaosEvent)
            {
                case EChaosEvent.E_FUEL_SHORTAGE:
                    return (double)(float)PLServer.Instance.ChaosLevel >= 2.0;
                case EChaosEvent.E_COOLANT_SHORTAGE:
                    return (double)(float)PLServer.Instance.ChaosLevel >= 1.0;
                case EChaosEvent.E_CALM:
                    return (double)(float)PLServer.Instance.ChaosLevel >= 7.0;
                case EChaosEvent.E_INFECTION_BOOST:
                    return (double)(float)PLServer.Instance.ChaosLevel >= 4.0;
                case EChaosEvent.E_WD_OFFENSIVE:
                    return (double)(float)PLServer.Instance.ChaosLevel >= 3.0;
                case EChaosEvent.E_SHOCK_DRONE_INVASION:
                    return (double)(float)PLServer.Instance.ChaosLevel >= 3.0;
                case EChaosEvent.E_DEATHSEEKER_DRONE_INVASION:
                    return (double)(float)PLServer.Instance.ChaosLevel >= 3.0;
                case EChaosEvent.E_PHASE_DRONE_INVASION:
                    return (double)(float)PLServer.Instance.ChaosLevel >= 1.0;
                case EChaosEvent.E_LONG_RANGE_PRICE_HIKE:
                    return (double)(float)PLServer.Instance.ChaosLevel >= 2.0;
                default:
                    return false;
            }
        }

        [HarmonyPatch(typeof(PLServer), "GetCoolantBasePrice")]
        internal class CoolantChaosEventPrice
        {
            private static void Postfix(ref int __result)
            {
                if (PLServer.Instance == null)
                    return;
                if (PLServer.Instance.IsChaosEventActive(EChaosEvent.E_COOLANT_SHORTAGE))
                    __result *= 10;
            }
        }

        [HarmonyPatch(typeof(PLServer), "GetFuelBasePrice")]
        internal class FuelChaosEventPrice
        {
            private static void Postfix(ref int __result)
            {
                if (PLServer.Instance == null)
                    return;
                if (PLServer.Instance.IsChaosEventActive(EChaosEvent.E_FUEL_SHORTAGE))
                    __result *= 5;
            }
        }

        [HarmonyPatch(typeof(PLServer), "CaptainBuy_Fuel")]
        internal class BuyFuelCap
        {
            private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                List<CodeInstruction> list = instructions.ToList();

                List<CodeInstruction> targetSequence = new List<CodeInstruction>() {
                    new CodeInstruction(OpCodes.Ldc_I4, 200),
                };
                List<CodeInstruction> patchSequence = new List<CodeInstruction>()
                {
                    new CodeInstruction(OpCodes.Ldc_I4, 50),
                };

                return HarmonyHelpers.PatchBySequence(list.AsEnumerable<CodeInstruction>(), targetSequence, patchSequence, HarmonyHelpers.PatchMode.REPLACE, HarmonyHelpers.CheckMode.NONNULL, false);
            }
        }

        //i made this with chatgpt because i am lazy
        [HarmonyPatch]
        public static class Patch_UpdateMainItems_ReplaceString
        {
            static MethodBase TargetMethod()
            {
                // Get the nested compiler-generated state machine for UpdateMainItems
                var outerType = typeof(PLItemShopMenu);
                foreach (var nested in outerType.GetNestedTypes(BindingFlags.NonPublic | BindingFlags.Instance))
                {
                    if (nested.Name.Contains("UpdateMainItems"))
                    {
                        return AccessTools.Method(nested, "MoveNext");
                    }
                }
                throw new System.Exception("Failed to locate state machine for UpdateMainItems");
            }

            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                foreach (var instruction in instructions)
                {
                    if (instruction.opcode == OpCodes.Ldstr && instruction.operand is string str && str == " / 200)")
                    {
                        instruction.operand = " / 50)";
                    }
                    yield return instruction;
                }
            }
        }
    }
}
