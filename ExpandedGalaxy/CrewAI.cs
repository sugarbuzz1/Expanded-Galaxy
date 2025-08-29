using HarmonyLib;
using PulsarModLoader;
using PulsarModLoader.Patches;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using UnityEngine;

namespace ExpandedGalaxy
{
    internal class CrewAI
    {
        [HarmonyPatch(typeof(PLScientistSensorScreen), "OptimizeForBot")]
        internal class BetterSensorBot
        {
            private static bool Prefix(PLScientistSensorScreen __instance, PLBot inBot)
            {
                if (!((Object)inBot != (Object)null) || !((Object)inBot.PlayerOwner != (Object)null) || !((Object)inBot.PlayerOwner.GetPawn() != (Object)null) || !((Object)inBot.PlayerOwner.GetPawn().CurrentShip != (Object)null) || !((Object)__instance.MyScreenHubBase.OptionalShipInfo != (Object)null))
                    return false;
                if (__instance.MyScreenHubBase.OptionalShipInfo.MySensorDish != null)
                    inBot.LastInRangeForSensorDishTime = Time.time;
                Traverse traverse = Traverse.Create(__instance);
                if ((double)Time.time - (double)traverse.Field("LastTimeBot_Activated_SensorSweep").GetValue<float>() > 8.0 && (double)Time.time - (double)traverse.Field("LastTimeBot_TriedToActivate_SensorSweep").GetValue<float>() > 1.0)
                {
                    traverse.Field("LastTimeBot_TriedToActivate_SensorSweep").SetValue(Time.time);
                    int num = inBot.PlayerOwner.GetPriorityLevel(18);
                    if (inBot.PlayerOwner.ActiveSubPriority != null && inBot.PlayerOwner.ActiveSubPriority.PriID == 62)
                        num = 5;
                    if (Random.Range(0, 100) < num * num)
                    {
                        traverse.Field("LastTimeBot_Activated_SensorSweep").SetValue(Time.time);
                        PLServer.Instance.photonView.RPC("StartActiveScan", PhotonTargets.All, (object)__instance.MyScreenHubBase.OptionalShipInfo.ShipID, (object)0);
                    }
                }
                if (!((Object)__instance.MyScreenHubBase.OptionalShipInfo.TargetShip != (Object)null) || (double)__instance.MyScreenHubBase.OptionalShipInfo.TargetShip.MySensorObjectShip.GetDetectionSignal(Vector3.SqrMagnitude(__instance.MyScreenHubBase.OptionalShipInfo.TargetShip.Exterior.transform.position - __instance.MyScreenHubBase.OptionalShipInfo.Exterior.transform.position), __instance.MyScreenHubBase.OptionalShipInfo.TargetShip.MyStats.EMSignature, __instance.MyScreenHubBase.OptionalShipInfo.MyStats.EMDetection, (PLShipInfoBase)__instance.MyScreenHubBase.OptionalShipInfo, (PLSensorObject)__instance.MyScreenHubBase.OptionalShipInfo.TargetShip.MySensorObjectShip) < 18.0 || !((Object)__instance.MyScreenHubBase.OptionalShipInfo.TargetShip != (Object)__instance.MyScreenHubBase.OptionalShipInfo) || (double)Time.time - (double)__instance.MyScreenHubBase.OptionalShipInfo.TargetShip.LastRecievedWeaknessTime <= 32.0)
                    return false;
                if (__instance.MyScreenHubBase.OptionalShipInfo.MySensorDish == null || __instance.MyScreenHubBase.OptionalShipInfo.MySensorDish.SubType == 0)
                {
                    if (inBot.PlayerOwner.GetPriorityLevel(20) > 0 && __instance.MyScreenHubBase.OptionalShipInfo.MyStats.CyberAttackRating > __instance.MyScreenHubBase.OptionalShipInfo.TargetShip.MyStats.CyberDefenseRating * 0.6f && HasUseableVirus(__instance.MyScreenHubBase.OptionalShipInfo))
                    {
                        __instance.MyScreenHubBase.OptionalShipInfo.TargetShip.photonView.RPC("AddSensorWeakness", PhotonTargets.All, 3, PLServer.Instance.GetEstimatedServerMs(), 0);
                    }
                    else if (!SensorDish.IsSensorWeaknessActiveReal(__instance.MyScreenHubBase.OptionalShipInfo.TargetShip, 5, 0) && __instance.MyScreenHubBase.OptionalShipInfo.TargetShip.MyStats.ReactorTempCurrent / __instance.MyScreenHubBase.OptionalShipInfo.TargetShip.MyStats.ReactorTempMax > 0.9f)
                    {
                        __instance.MyScreenHubBase.OptionalShipInfo.TargetShip.photonView.RPC("AddSensorWeakness", PhotonTargets.All, 5, PLServer.Instance.GetEstimatedServerMs(), 0);
                    }
                    else if (__instance.MyScreenHubBase.OptionalShipInfo.TargetShip.MyShieldGenerator != null && __instance.MyScreenHubBase.OptionalShipInfo.TargetShip.MyShieldGenerator.MinIntegrityForBubble * 2f <= __instance.MyScreenHubBase.OptionalShipInfo.TargetShip.MyShieldGenerator.Current && (double)__instance.MyScreenHubBase.OptionalShipInfo.TargetShip.MyStats.ShieldsMax != 0.0)
                    {
                        __instance.MyScreenHubBase.OptionalShipInfo.TargetShip.photonView.RPC("AddSensorWeakness", PhotonTargets.All, 2, PLServer.Instance.GetEstimatedServerMs(), 0);
                    }
                    else if (__instance.MyScreenHubBase.OptionalShipInfo.TargetShip.GetCombatLevel() > __instance.MyScreenHubBase.OptionalShipInfo.GetCombatLevel() && (float)__instance.MyScreenHubBase.OptionalShipInfo.TargetShip.WeaponsSystem.Health > 5f)
                    {
                        __instance.MyScreenHubBase.OptionalShipInfo.TargetShip.photonView.RPC("AddSensorWeakness", PhotonTargets.All, 0, PLServer.Instance.GetEstimatedServerMs(), 1);
                    }
                    else if ((float)__instance.MyScreenHubBase.OptionalShipInfo.TargetShip.EngineeringSystem.Health > 5f)
                    {
                        __instance.MyScreenHubBase.OptionalShipInfo.TargetShip.photonView.RPC("AddSensorWeakness", PhotonTargets.All, 0, PLServer.Instance.GetEstimatedServerMs(), 0);
                    }
                    else if ((float)__instance.MyScreenHubBase.OptionalShipInfo.TargetShip.WeaponsSystem.Health > 5f)
                    {
                        __instance.MyScreenHubBase.OptionalShipInfo.TargetShip.photonView.RPC("AddSensorWeakness", PhotonTargets.All, 0, PLServer.Instance.GetEstimatedServerMs(), 1);
                    }
                    else
                    {
                        __instance.MyScreenHubBase.OptionalShipInfo.TargetShip.photonView.RPC("AddSensorWeakness", PhotonTargets.All, 0, PLServer.Instance.GetEstimatedServerMs(), 3);
                    }
                }
                else if (__instance.MyScreenHubBase.OptionalShipInfo.MySensorDish.SubType == 1)
                {
                    if (__instance.MyScreenHubBase.OptionalShipInfo.MySensorDish.SubTypeData == 0 && HasShipToTakeover(__instance.MyScreenHubBase.OptionalShipInfo, out PLShipInfoBase strongestShip))
                    {
                        strongestShip.photonView.RPC("AddSensorWeakness", PhotonTargets.All, 3, PLServer.Instance.GetEstimatedServerMs(), 1);
                    }
                    else if (__instance.MyScreenHubBase.OptionalShipInfo.TargetShip.MyShieldGenerator != null && __instance.MyScreenHubBase.OptionalShipInfo.TargetShip.MyShieldGenerator.MinIntegrityForBubble * 2f <= __instance.MyScreenHubBase.OptionalShipInfo.TargetShip.MyShieldGenerator.Current && (double)__instance.MyScreenHubBase.OptionalShipInfo.TargetShip.MyStats.ShieldsMax != 0.0)
                    {
                        __instance.MyScreenHubBase.OptionalShipInfo.TargetShip.photonView.RPC("AddSensorWeakness", PhotonTargets.All, 5, PLServer.Instance.GetEstimatedServerMs(), 1);
                    }
                    else if (inBot.PlayerOwner.GetPriorityLevel(20) > 0 && __instance.MyScreenHubBase.OptionalShipInfo.MyStats.CyberAttackRating < __instance.MyScreenHubBase.OptionalShipInfo.TargetShip.MyStats.CyberDefenseRating && HasUseableVirus(__instance.MyScreenHubBase.OptionalShipInfo))
                    {
                        __instance.MyScreenHubBase.OptionalShipInfo.TargetShip.photonView.RPC("AddSensorWeakness", PhotonTargets.All, 0, PLServer.Instance.GetEstimatedServerMs(), 3);
                    }
                    else if (__instance.MyScreenHubBase.OptionalShipInfo.TargetShip.GetCombatLevel() > __instance.MyScreenHubBase.OptionalShipInfo.GetCombatLevel() && (float)__instance.MyScreenHubBase.OptionalShipInfo.TargetShip.WeaponsSystem.Health > 5f)
                    {
                        __instance.MyScreenHubBase.OptionalShipInfo.TargetShip.photonView.RPC("AddSensorWeakness", PhotonTargets.All, 0, PLServer.Instance.GetEstimatedServerMs(), 1);
                    }
                    else if ((float)__instance.MyScreenHubBase.OptionalShipInfo.TargetShip.EngineeringSystem.Health > 5f)
                    {
                        __instance.MyScreenHubBase.OptionalShipInfo.TargetShip.photonView.RPC("AddSensorWeakness", PhotonTargets.All, 0, PLServer.Instance.GetEstimatedServerMs(), 0);
                    }
                    else
                    {
                        __instance.MyScreenHubBase.OptionalShipInfo.TargetShip.photonView.RPC("AddSensorWeakness", PhotonTargets.All, 2, PLServer.Instance.GetEstimatedServerMs(), 1);
                    }
                }
                if (SensorDish.screenInfos.ContainsKey(__instance))
                {
                    SensorDish.lastToInteract = inBot.PlayerOwner;
                    ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.cachePlayer", PhotonTargets.Others, new object[1]
                    {
                    (object) inBot.PlayerOwner.GetPlayerID(),
                    });
                }
                return false;
            }
        }

        private static bool HasUseableVirus(PLShipInfoBase inShipInfo)
        {
            bool flag = false;
            foreach (PLWarpDriveProgram program in inShipInfo.MyStats.GetComponentsOfType(ESlotType.E_COMP_PROGRAM))
            {
                if (program.IsVirus && program.Level >= program.MaxLevelCharges)
                {
                    flag = true;
                    break;
                }
            }
            return flag;
        }

        private static bool HasShipToTakeover(PLShipInfoBase inHomeShip, out PLShipInfoBase strongestShip) 
        {
            strongestShip = null;
            foreach (PLShipInfoBase shipInfoBase in UnityEngine.Object.FindObjectsOfType<PLShipInfoBase>())
            {
                if (shipInfoBase != null && shipInfoBase.ShipID != inHomeShip.ShipID && (inHomeShip.TargetShip == null || inHomeShip.TargetShip.ShipID != shipInfoBase.ShipID) && !shipInfoBase.HasBeenDestroyed && shipInfoBase.IsDrone && inHomeShip.ShouldBeHostileToShip(shipInfoBase, false, false, false))
                {
                    switch (shipInfoBase.ShipTypeID)
                    {
                        case EShipType.E_WDDRONE1:
                        case EShipType.E_WDDRONE2:
                        case EShipType.E_WDDRONE3:
                        case EShipType.E_DEATHSEEKER_DRONE:
                        case EShipType.E_SHOCK_DRONE:
                        case EShipType.E_PHASE_DRONE:
                        case EShipType.E_UNSEEN_FIGHTER:
                            if (strongestShip == null)
                                strongestShip = shipInfoBase;
                            else
                                if (shipInfoBase.GetCombatLevel() > strongestShip.GetCombatLevel())
                                    strongestShip = shipInfoBase;
                            break;
                    }
                }
            }
            return strongestShip != null;
        }

        [HarmonyPatch(typeof(PLCustomPawn), "BeforeUpdate")]
        internal class Purple
        {
            private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
            {
                Label failed = generator.DefineLabel();
                Label succeed = generator.DefineLabel();
                List<CodeInstruction> list = instructions.ToList();

                List<CodeInstruction> targetSequence = new List<CodeInstruction>() {
                    new CodeInstruction(OpCodes.Ldfld),
                    new CodeInstruction(OpCodes.Ldc_I4_0),
                    new CodeInstruction(OpCodes.Ldelem_Ref),
                    new CodeInstruction(OpCodes.Ldstr, "_ClassColor")
                };
                List<CodeInstruction> patchSequence = new List<CodeInstruction>()
                {
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(PLCustomPawn), "MyPawn")),
                    new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(PLCombatTarget), "GetPlayer")),
                    new CodeInstruction(OpCodes.Ldc_I4_0),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(PLPlayer), "GetPlayerName", new System.Type[1] {typeof(bool)})),
                    new CodeInstruction(OpCodes.Ldstr, "sugarbuzz1"),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(string), "op_Equality", new System.Type[2] {typeof(string), typeof(string)})),
                    new CodeInstruction(OpCodes.Brfalse, failed),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(ExpandedGalaxy.Relic), "getRelicColor")),
                    new CodeInstruction(OpCodes.Br, succeed)
                };
                list[671].labels.Add(failed);
                list[678].labels.Add(succeed);

                return HarmonyHelpers.PatchBySequence(list.AsEnumerable<CodeInstruction>(), targetSequence, patchSequence, HarmonyHelpers.PatchMode.AFTER, HarmonyHelpers.CheckMode.NONNULL, true);
            }
        }
    }
}
