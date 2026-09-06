using Behave.Runtime;
using HarmonyLib;
using OculusSampleFramework;
using Pathfinding;
using PulsarModLoader.Patches;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using UnityEngine;
using Tree = Behave.Runtime.Tree;

namespace ExpandedGalaxy
{
    internal class CrewAI
    {
        [HarmonyPatch(typeof(PLCustomPawn), "BeforeUpdate")]
        internal class Purple
        {
            private static IEnumerable<CodeInstruction> Transpiler(
              IEnumerable<CodeInstruction> instructions,
              ILGenerator generator)
            {
                Label operand1 = generator.DefineLabel();
                Label operand2 = generator.DefineLabel();
                List<CodeInstruction> list = instructions.ToList<CodeInstruction>();
                List<CodeInstruction> targetSequence = new List<CodeInstruction>()
        {
          new CodeInstruction(OpCodes.Ldfld),
          new CodeInstruction(OpCodes.Ldc_I4_0),
          new CodeInstruction(OpCodes.Ldelem_Ref),
          new CodeInstruction(OpCodes.Ldstr, (object) "_ClassColor")
        };
                List<CodeInstruction> patchSequence = new List<CodeInstruction>()
        {
          new CodeInstruction(OpCodes.Ldarg_0),
          new CodeInstruction(OpCodes.Ldfld, (object) AccessTools.Field(typeof (PLCustomPawn), "MyPawn")),
          new CodeInstruction(OpCodes.Callvirt, (object) AccessTools.Method(typeof (PLCombatTarget), "GetPlayer")),
          new CodeInstruction(OpCodes.Ldc_I4_0),
          new CodeInstruction(OpCodes.Call, (object) AccessTools.Method(typeof (PLPlayer), "GetPlayerName", new System.Type[1]
          {
            typeof (bool)
          })),
          new CodeInstruction(OpCodes.Ldstr, (object) "sugarbuzz1"),
          new CodeInstruction(OpCodes.Call, (object) AccessTools.Method(typeof (string), "op_Equality", new System.Type[2]
          {
            typeof (string),
            typeof (string)
          })),
          new CodeInstruction(OpCodes.Brfalse, (object) operand1),
          new CodeInstruction(OpCodes.Call, (object) AccessTools.Method(typeof (Relic), "GetRelicColor")),
          new CodeInstruction(OpCodes.Br, (object) operand2)
        };
                list[671].labels.Add(operand1);
                list[678].labels.Add(operand2);
                return HarmonyHelpers.PatchBySequence(list.AsEnumerable<CodeInstruction>(), (IEnumerable<CodeInstruction>)targetSequence, (IEnumerable<CodeInstruction>)patchSequence, checkMode: HarmonyHelpers.CheckMode.NONNULL);
            }
        }

        [HarmonyPatch(typeof(PLBot), "GetNearestEnemyTargetTransform")]
        internal class SystemInstanceBotTarget
        {
            private static bool Prefix(
              PLBot __instance,
              ref PLCombatTarget outTarget,
              float rangeMultiplier,
              ref Transform __result,
              ref PLBotController ___MyBotController)
            {
                float num = Mathf.Pow(140f * rangeMultiplier, 2f);
                Transform transform = (Transform)null;
                foreach (PLCombatTarget allCombatTarget in PLGameStatic.Instance.AllCombatTargets)
                {
                    if ((UnityEngine.Object)allCombatTarget != (UnityEngine.Object)null && (UnityEngine.Object)__instance.PlayerOwner != (UnityEngine.Object)null && (UnityEngine.Object)__instance.PlayerOwner.GetPawn() != (UnityEngine.Object)null && !(bool)allCombatTarget.IsDead && allCombatTarget.ShouldShowInHUD() && allCombatTarget.gameObject.activeInHierarchy)
                    {
                        PLPawn plPawn = allCombatTarget as PLPawn;
                        PLGroundTurret plGroundTurret = allCombatTarget as PLGroundTurret;
                        bool flag = false;
                        if ((UnityEngine.Object)plGroundTurret != (UnityEngine.Object)null)
                            flag = false;
                        else if ((UnityEngine.Object)plPawn == (UnityEngine.Object)null || !plPawn.PreviewPawn && !(bool)plPawn.Cloaked)
                            flag = true;
                        if (flag && (UnityEngine.Object)allCombatTarget.CurrentShip == (UnityEngine.Object)__instance.PlayerOwner.GetPawn().CurrentShip && (UnityEngine.Object)allCombatTarget.MyInterior == (UnityEngine.Object)__instance.PlayerOwner.GetPawn().MyInterior && (UnityEngine.Object)allCombatTarget.MyCurrentTLI == (UnityEngine.Object)__instance.PlayerOwner.MyCurrentTLI && (0 | (!((UnityEngine.Object)allCombatTarget.GetPlayer() == (UnityEngine.Object)null) || __instance.PlayerOwner.GetClassID() == -1 ? 0 : ((int)__instance.PlayerOwner.TeamID == 0 ? 1 : 0)) | (!((UnityEngine.Object)allCombatTarget.GetPlayer() != (UnityEngine.Object)null) || !((UnityEngine.Object)allCombatTarget.GetPlayer() != (UnityEngine.Object)__instance.PlayerOwner) ? 0 : ((int)allCombatTarget.GetPlayer().TeamID != (int)__instance.PlayerOwner.TeamID ? 1 : 0))) != 0)
                        {
                            float sqrMagnitude = (allCombatTarget.transform.position - __instance.PlayerOwner.GetPawn().transform.position).sqrMagnitude;
                            if ((UnityEngine.Object)allCombatTarget == (UnityEngine.Object)__instance.HighPriorityTarget)
                                sqrMagnitude *= 0.2f;
                            if ((double)sqrMagnitude < (double)num && ((UnityEngine.Object)allCombatTarget == (UnityEngine.Object)__instance.HighPriorityTarget || __instance.PlayerOwner.GetPawn().HadRecentLOSSuccessToTarget(allCombatTarget)))
                            {
                                num = sqrMagnitude;
                                transform = allCombatTarget.transform;
                                outTarget = allCombatTarget;
                            }
                        }
                    }
                }
                if ((int)__instance.PlayerOwner.TeamID == 0)
                {
                    foreach (PLGroundTurret allGroundTurret in PLGameStatic.Instance.AllGroundTurrets)
                    {
                        if ((UnityEngine.Object)allGroundTurret != (UnityEngine.Object)null && (UnityEngine.Object)__instance.PlayerOwner != (UnityEngine.Object)null && (UnityEngine.Object)__instance.PlayerOwner.GetPawn() != (UnityEngine.Object)null && !(bool)allGroundTurret.IsDead && allGroundTurret.ShouldShowInHUD() && (UnityEngine.Object)allGroundTurret.Target != (UnityEngine.Object)null && allGroundTurret.Target.GetIsFriendly() && ((UnityEngine.Object)allGroundTurret.GetPlayer() == (UnityEngine.Object)null && (UnityEngine.Object)allGroundTurret.CurrentShip == (UnityEngine.Object)__instance.PlayerOwner.GetPawn().CurrentShip && (UnityEngine.Object)allGroundTurret.MyInterior == (UnityEngine.Object)__instance.PlayerOwner.GetPawn().MyInterior || (UnityEngine.Object)allGroundTurret.GetPlayer() != (UnityEngine.Object)null && (UnityEngine.Object)allGroundTurret.GetPlayer() != (UnityEngine.Object)__instance.PlayerOwner && (int)allGroundTurret.GetPlayer().TeamID != (int)__instance.PlayerOwner.TeamID && allGroundTurret.GetPlayer().SubHubID == __instance.PlayerOwner.SubHubID))
                        {
                            float sqrMagnitude = (allGroundTurret.transform.position - __instance.PlayerOwner.GetPawn().transform.position).sqrMagnitude;
                            if ((double)sqrMagnitude < (double)num && (double)sqrMagnitude > 1.0 && __instance.PlayerOwner.GetPawn().HadRecentLOSSuccessToTarget((PLCombatTarget)allGroundTurret))
                            {
                                num = sqrMagnitude;
                                transform = allGroundTurret.transform;
                                outTarget = (PLCombatTarget)allGroundTurret;
                            }
                        }
                    }
                    foreach (PLFBVent allFbVent in PLGameStatic.Instance.AllFBVents)
                    {
                        if ((UnityEngine.Object)allFbVent != (UnityEngine.Object)null && (UnityEngine.Object)__instance.PlayerOwner != (UnityEngine.Object)null && (UnityEngine.Object)__instance.PlayerOwner.GetPawn() != (UnityEngine.Object)null && !(bool)allFbVent.IsDead && allFbVent.ShouldTakeDamage() && ((UnityEngine.Object)allFbVent.GetPlayer() == (UnityEngine.Object)null && (UnityEngine.Object)allFbVent.CurrentShip == (UnityEngine.Object)__instance.PlayerOwner.GetPawn().CurrentShip && (UnityEngine.Object)allFbVent.MyInterior == (UnityEngine.Object)__instance.PlayerOwner.GetPawn().MyInterior || (UnityEngine.Object)allFbVent.GetPlayer() != (UnityEngine.Object)null && (UnityEngine.Object)allFbVent.GetPlayer() != (UnityEngine.Object)__instance.PlayerOwner && (int)allFbVent.GetPlayer().TeamID != (int)__instance.PlayerOwner.TeamID && allFbVent.GetPlayer().SubHubID == __instance.PlayerOwner.SubHubID))
                        {
                            float sqrMagnitude = (allFbVent.transform.position - __instance.PlayerOwner.GetPawn().transform.position).sqrMagnitude;
                            if ((double)sqrMagnitude < (double)num && (double)sqrMagnitude > 1.0 && __instance.PlayerOwner.GetPawn().HadRecentLOSSuccessToTarget((PLCombatTarget)allFbVent))
                            {
                                num = sqrMagnitude;
                                transform = allFbVent.transform;
                                outTarget = (PLCombatTarget)allFbVent;
                            }
                        }
                    }
                }
                if ((UnityEngine.Object)transform == (UnityEngine.Object)null && (UnityEngine.Object)___MyBotController != (UnityEngine.Object)null && (UnityEngine.Object)___MyBotController.MyPawn != (UnityEngine.Object)null && (UnityEngine.Object)__instance.PlayerOwner != (UnityEngine.Object)null && (UnityEngine.Object)___MyBotController.MyPawn.CurrentShip != (UnityEngine.Object)null && ___MyBotController.MyPawn.CurrentShip.TeamID != (int)__instance.PlayerOwner.TeamID)
                {
                    foreach (PLSystemInstance repairableSystemInstance in ___MyBotController.MyPawn.CurrentShip.RepairableSystemInstances)
                    {
                        if ((UnityEngine.Object)repairableSystemInstance != (UnityEngine.Object)null)
                        {
                            float sqrMagnitude = (repairableSystemInstance.transform.position - ___MyBotController.MyPawn.transform.position).sqrMagnitude;
                            if ((double)sqrMagnitude < (double)num && (double)sqrMagnitude > 1.0 && (double)repairableSystemInstance.MySystem.GetHealthRatio() > 0.05 && !repairableSystemInstance.MySystem.IsOnFire())
                            {
                                num = sqrMagnitude;
                                transform = repairableSystemInstance.transform;
                                outTarget = (PLCombatTarget)null;
                            }
                        }
                    }
                }
                if ((UnityEngine.Object)transform == (UnityEngine.Object)null && (UnityEngine.Object)__instance.HighPriorityTarget != (UnityEngine.Object)null)
                {
                    transform = __instance.HighPriorityTarget.transform;
                    outTarget = __instance.HighPriorityTarget;
                }
                __result = transform;
                return false;
            }
        }

        [HarmonyPatch(typeof(PLBot), "TickFindInvaderAction")]
        internal class BotFindInvaderActionPatch
        {
            private static bool Prefix(
              PLBot __instance,
              Tree sender,
              ref PLBotController ___MyBotController,
              ref float ___LastInitTime,
              ref bool ___IsSimpleCombatBot,
              ref BehaveResult __result)
            {
                if ((UnityEngine.Object)___MyBotController == (UnityEngine.Object)null || (UnityEngine.Object)__instance.PlayerOwner == (UnityEngine.Object)null || (UnityEngine.Object)__instance.PlayerOwner.GetPawn() == (UnityEngine.Object)null)
                {
                    __result = BehaveResult.Success;
                    return false;
                }
                PLCombatTarget outTarget = (PLCombatTarget)null;
                Transform enemyTargetTransform = __instance.GetNearestEnemyTargetTransform(ref outTarget);
                if ((UnityEngine.Object)enemyTargetTransform != (UnityEngine.Object)null && (UnityEngine.Object)___MyBotController != (UnityEngine.Object)null)
                {
                    PLPathfinderGraphEntity pgEforPlayer = PLPathfinder.GetInstance().GetPGEforPlayer(__instance.PlayerOwner);
                    Vector3 vector3_1;
                    if (pgEforPlayer != null)
                    {
                        for (int index = 0; index < 8; ++index)
                        {
                            Vector3 position1 = enemyTargetTransform.position;
                            vector3_1 = Vector3.Scale(UnityEngine.Random.onUnitSphere, new Vector3(1f, 0.05f, 1f));
                            Vector3 vector3_2 = vector3_1.normalized * UnityEngine.Random.Range(3f, 8f) * ((UnityEngine.Object)outTarget != (UnityEngine.Object)null ? outTarget.CombatRangeModifier : 1f);
                            Vector3 position2 = position1 + vector3_2;
                            NNInfoInternal nearest = pgEforPlayer.Graph.GetNearest(position2, PLBot.GetContraintForPGE(ref __instance.myPGEConstraint, pgEforPlayer));
                            if (nearest.node != null && !pgEforPlayer.Graph.Linecast(enemyTargetTransform.position, nearest.clampedPosition) && (double)Vector3.SqrMagnitude(nearest.clampedPosition - enemyTargetTransform.position) > 9.0 & ((double)UnityEngine.Random.value < 0.10000000149011612 | (double)Vector3.SqrMagnitude(nearest.clampedPosition - __instance.PlayerOwner.GetPawn().transform.position) < (double)Vector3.SqrMagnitude(__instance.PlayerOwner.GetPawn().transform.position - enemyTargetTransform.position)))
                            {
                                __instance.AI_TargetPos = nearest.clampedPosition;
                                __instance.AI_TargetPos_Raw = __instance.AI_TargetPos;
                                __instance.EnablePathing = true;
                                break;
                            }
                        }
                    }
                    if ((UnityEngine.Object)outTarget != (UnityEngine.Object)null && (UnityEngine.Object)__instance.PlayerOwner != (UnityEngine.Object)null && (int)__instance.PlayerOwner.RaceID == 1 && (UnityEngine.Object)__instance.PlayerOwner.GetPawn() != (UnityEngine.Object)null && !(bool)__instance.PlayerOwner.GetPawn().Cloaked && (double)Time.time - (double)___MyBotController.LastCloakedActivatedTime > 120.0 && (double)(float)__instance.PlayerOwner.GetPawn().Health / (double)(float)__instance.PlayerOwner.GetPawn().MaxHealth < 0.5 && (double)Time.time - (double)__instance.PlayerOwner.GetPawn().LastCombatDamageTakenTime < 2.0)
                        __instance.PlayerOwner.GetPawn().AttemptToCloak();
                    PLPriorityMetadata_FloatRange priorityMetadata = __instance.PlayerOwner.GetPriorityMetadata<PLPriorityMetadata_FloatRange>(EAIPriorityListDisplayed.REPEL_INVADERS, 0);
                    int index1 = (UnityEngine.Object)outTarget != (UnityEngine.Object)null ? UnityEngine.Random.Range(0, outTarget.MyCollisionSpheres.Length) : 0;
                    if ((UnityEngine.Object)___MyBotController != (UnityEngine.Object)null && (UnityEngine.Object)___MyBotController.MyPawn != (UnityEngine.Object)null)
                    {
                        if ((UnityEngine.Object)enemyTargetTransform != (UnityEngine.Object)null)
                        {
                            Vector3 normalized;
                            if ((UnityEngine.Object)___MyBotController.MyPawn.PawnCamera != (UnityEngine.Object)null && (UnityEngine.Object)outTarget != (UnityEngine.Object)null && outTarget.MyCollisionSpheres.Length > index1)
                            {
                                vector3_1 = outTarget.MyCollisionSpheres[index1].transform.position - ___MyBotController.MyPawn.PawnCamera.position;
                                normalized = vector3_1.normalized;
                            }
                            else
                            {
                                vector3_1 = enemyTargetTransform.position - ___MyBotController.MyPawn.PawnCamera.position;
                                normalized = vector3_1.normalized;
                            }
                            if ((UnityEngine.Object)outTarget != (UnityEngine.Object)null && (UnityEngine.Object)__instance.PlayerOwner.GetPawn() != (UnityEngine.Object)null && __instance.PlayerOwner.GetPawn().HadRecentLOSSuccessToTarget(outTarget))
                            {
                                float num1 = Vector3.Dot(___MyBotController.MyPawn.VerticalMouseLook.transform.forward, normalized);
                                float num2 = Vector3.SqrMagnitude(enemyTargetTransform.position - ___MyBotController.transform.position);
                                if (priorityMetadata != null)
                                    num2 *= 1f + Mathf.Clamp01(priorityMetadata.Data * 0.02f);
                                ___MyBotController.AI_ShouldUseActiveItem = (double)num2 >= 2.0 ? ((double)num2 >= 25.0 ? (double)num1 > 0.800000011920929 : (double)num1 > 0.60000002384185791) : (double)num1 > 0.20000000298023224;
                            }
                            else if ((UnityEngine.Object)outTarget == (UnityEngine.Object)null && (double)Vector3.SqrMagnitude(enemyTargetTransform.position - ___MyBotController.transform.position) < 125.0)
                            {
                                RaycastHit hitInfo;
                                Physics.Linecast(___MyBotController.transform.position, enemyTargetTransform.position, out hitInfo, 7168);
                                float dotToFire = Vector3.Dot(___MyBotController.MyPawn.VerticalMouseLook.transform.forward, normalized);
                                ___MyBotController.AI_ShouldUseActiveItem = ___MyBotController.MyPawn.GetPlayer().MyInventory.ActiveItem != null && (UnityEngine.Object)hitInfo.transform == (UnityEngine.Object)enemyTargetTransform && ___MyBotController.MyPawn.GetPlayer().MyInventory.ActiveItem.ShouldBeActive(dotToFire, -20f);
                            }
                            else
                                ___MyBotController.AI_ShouldUseActiveItem = false;
                            if ((UnityEngine.Object)__instance.PlayerOwner.GetPawn() != (UnityEngine.Object)null && (bool)__instance.PlayerOwner.GetPawn().Cloaked)
                            {
                                PLBotController plBotController = ___MyBotController;
                                plBotController.AI_ShouldUseActiveItem = ((plBotController.AI_ShouldUseActiveItem ? 1 : 0) & ((double)Time.time - (double)plBotController.LastCloakedActivatedTime <= 10.0 ? 0 : ((double)UnityEngine.Random.value < 0.10000000149011612 ? 1 : 0))) != 0;
                            }
                            if ((UnityEngine.Object)outTarget != (UnityEngine.Object)null && outTarget.MyCollisionSpheres.Length > index1)
                                ___MyBotController.AI_Item_Target = outTarget.MyCollisionSpheres[index1].transform;
                            else
                                ___MyBotController.AI_Item_Target = enemyTargetTransform;
                        }
                        else
                        {
                            __instance.EnablePathing = false;
                            ___MyBotController.AI_ShouldUseActiveItem = false;
                            ___MyBotController.AI_Item_Target = (Transform)null;
                            __instance.AI_TargetTLI = (PLTeleportationLocationInstance)null;
                        }
                        ___MyBotController.AI_ItemUtilityRequest = EItemUtilityType.E_DAMAGE;
                    }
                    __result = (double)Time.time - (double)___LastInitTime > 2.0 || (UnityEngine.Object)enemyTargetTransform == (UnityEngine.Object)null ? BehaveResult.Success : BehaveResult.Running;
                    return false;
                }
                if ((UnityEngine.Object)___MyBotController != (UnityEngine.Object)null)
                {
                    if (!___IsSimpleCombatBot || (UnityEngine.Object)___MyBotController.MyPawn.CurrentShip != (UnityEngine.Object)null)
                        __instance.TickFastExplore((int)___MyBotController.MyPawn.AreaIndex);
                    ___MyBotController.AI_ItemUtilityRequest = EItemUtilityType.E_DAMAGE;
                    ___MyBotController.AI_ShouldUseActiveItem = false;
                    ___MyBotController.AI_Item_Target = (Transform)null;
                    __instance.AI_TargetTLI = (PLTeleportationLocationInstance)null;
                }
                __result = BehaveResult.Failure;
                return false;
            }
        }

        [HarmonyPatch(typeof(PLBot), "TickOptimizeStationAction")]
        internal class EngineerInEngineeringPatch
        {
            public static void Postfix(PLBot __instance)
            {
                if ((int)__instance.PlayerOwner.TeamID == 0 || (UnityEngine.Object)__instance.PlayerOwner.StartingShip == (UnityEngine.Object)null || (UnityEngine.Object)__instance.PlayerOwner.GetPawn() == (UnityEngine.Object)null || __instance.PlayerOwner.GetClassID() != 4 || (UnityEngine.Object)__instance.PlayerOwner.MyCurrentTLI != (UnityEngine.Object)__instance.PlayerOwner.StartingShip.MyTLI)
                    return;
                PLUIScreen pluiScreen1 = (PLUIScreen)null;
                foreach (PLUIScreen allScreen in __instance.PlayerOwner.StartingShip.MyScreenBase.AllScreens)
                {
                    if (allScreen.name.ToLower().Contains("cloned") && (allScreen.name.ToLower().Contains("reactor") || allScreen.name.ToLower().Contains("engineering")))
                    {
                        pluiScreen1 = allScreen;
                        break;
                    }
                }
                if ((UnityEngine.Object)pluiScreen1 != (UnityEngine.Object)null)
                {
                    __instance.AI_TargetPos = pluiScreen1.transform.position + pluiScreen1.transform.forward - new Vector3(0.0f, 1f, 0.0f);
                    __instance.AI_TargetPos_Raw = __instance.AI_TargetPos;
                    __instance.EnablePathing = true;
                }
                if (__instance.PlayerOwner.ActiveSubPriority == null)
                    return;
                switch ((EAIPriorityListDisplayed)__instance.PlayerOwner.ActiveSubPriority.TypeData)
                {
                    case EAIPriorityListDisplayed.ENG_CHARGE_WARP_DRIVE:
                    case EAIPriorityListDisplayed.ENG_JUMP_SHIP:
                        PLUIScreen pluiScreen2 = (PLUIScreen)null;
                        foreach (PLUIScreen allScreen in __instance.PlayerOwner.StartingShip.MyScreenBase.AllScreens)
                        {
                            if (allScreen.name.ToLower().Contains("cloned") && (allScreen.name.ToLower().Contains("warp") || allScreen.name.ToLower().Contains("jump")))
                            {
                                pluiScreen2 = allScreen;
                                break;
                            }
                        }
                        if (!((UnityEngine.Object)pluiScreen2 != (UnityEngine.Object)null))
                            break;
                        __instance.AI_TargetPos = pluiScreen2.transform.position + pluiScreen2.transform.forward - new Vector3(0.0f, 1f, 0.0f);
                        __instance.AI_TargetPos_Raw = __instance.AI_TargetPos;
                        __instance.EnablePathing = true;
                        break;
                }
            }
        }

        [HarmonyPatch(typeof(PLEngineerReactorScreen), "OptimizeForBot_OnLeave")]
        private class EngineerOnLeavePatch
        {
            private static void Postfix(PLEngineerReactorScreen __instance, PLBot inBot)
            {
                if ((int)inBot.PlayerOwner.TeamID == 0 || __instance.MyScreenHubBase.OptionalShipInfo.ReactorCoolingEnabled)
                    return;
                __instance.MyScreenHubBase.OptionalShipInfo.ReactorCoolingEnabled = true;
                __instance.MyScreenHubBase.OptionalShipInfo.LastReactorCoolingToggleTime = Time.time;
            }
        }

        [HarmonyPatch(typeof(PLEngineerReactorScreen), "OptimizeForBot")]
        private class EngineerReactorSafetyPatch
        {
            private static void Postfix(PLEngineerReactorScreen __instance, PLBot inBot)
            {
                if ((int)inBot.PlayerOwner.TeamID == 0 || !__instance.MyScreenHubBase.OptionalShipInfo.ReactorCoolingEnabled)
                    return;
                __instance.MyScreenHubBase.OptionalShipInfo.ReactorCoolingEnabled = false;
                __instance.MyScreenHubBase.OptionalShipInfo.LastReactorCoolingToggleTime = Time.time;
            }
        }

        [HarmonyPatch(typeof(PLBot), "GetShipToBoard")]
        internal class PLBotGetShipToBoardFix
        {
            private static bool Prefix(PLBot __instance, ref PLShipInfo __result)
            {
                PLShipInfoBase plShipInfoBase = (PLShipInfoBase)null;
                if ((UnityEngine.Object)__instance.PlayerOwner != (UnityEngine.Object)null)
                {
                    if ((UnityEngine.Object)__instance.PlayerOwner.StartingShip != (UnityEngine.Object)null && !__instance.PlayerOwner.StartingShip.GetHasBeenDestroyed())
                        plShipInfoBase = __instance.PlayerOwner.StartingShip.TargetShip;
                    else if ((UnityEngine.Object)__instance.PlayerOwner.GetPawn() != (UnityEngine.Object)null && (UnityEngine.Object)__instance.PlayerOwner.GetPawn().CurrentShip != (UnityEngine.Object)__instance.PlayerOwner.StartingShip)
                        plShipInfoBase = (PLShipInfoBase)__instance.PlayerOwner.GetPawn().CurrentShip;
                    if ((UnityEngine.Object)plShipInfoBase != (UnityEngine.Object)null && (plShipInfoBase.IsQuantumShieldActive || plShipInfoBase.IsShipInfoBase || plShipInfoBase.InWarp))
                        plShipInfoBase = (PLShipInfoBase)null;
                }
                __result = (UnityEngine.Object)plShipInfoBase != (UnityEngine.Object)null ? plShipInfoBase as PLShipInfo : (PLShipInfo)null;
                return false;
            }
        }

        [HarmonyPatch(typeof(PLBot), "TickManTurretAction")]
        internal class PLBotManTurretFix
        {
            private static bool Prefix(
              PLBot __instance,
              ref PLBotController ___MyBotController,
              ref PLTurret ___MannedTurret,
              ref float ___lastRandomSysTime,
              ref float ___lastTargetSetTime,
              ref BehaveResult __result)
            {
                if ((UnityEngine.Object)___MyBotController == (UnityEngine.Object)null)
                {
                    __result = BehaveResult.Success;
                    return false;
                }
                if ((UnityEngine.Object)__instance.PlayerOwner != (UnityEngine.Object)null && (UnityEngine.Object)__instance.PlayerOwner.StartingShip != (UnityEngine.Object)null)
                {
                    __instance.AI_TargetInterior = (PLInterior)null;
                    __instance.AI_TargetTLI = __instance.PlayerOwner.StartingShip.MyTLI;
                }
                if ((UnityEngine.Object)___MyBotController != (UnityEngine.Object)null)
                    ___MyBotController.AI_ShouldUseActiveItem = false;
                if ((UnityEngine.Object)__instance.PlayerOwner != (UnityEngine.Object)null && (UnityEngine.Object)__instance.PlayerOwner.GetPawn() != (UnityEngine.Object)null && (UnityEngine.Object)__instance.PlayerOwner.StartingShip != (UnityEngine.Object)null)
                {
                    PLTurret ActivateableTurret = (PLTurret)null;
                    Transform ClosestTurretTransform = (Transform)null;
                    Transform MannedTurretTransform = (Transform)null;
                    float maxValue = float.MaxValue;
                    if ((UnityEngine.Object)__instance.PlayerOwner.StartingShip.WeaponsSystem.MainTurretStationTransform != (UnityEngine.Object)null && (int)__instance.PlayerOwner.Talents[4] > 0)
                    {
                        PLTurret mainTurret = __instance.PlayerOwner.StartingShip.MyStats.GetMainTurret();
                        if (mainTurret != null)
                            CrewAI.PLBotManTurretFix.CheckTurretTransform(__instance, __instance.PlayerOwner.StartingShip.WeaponsSystem.MainTurretStationTransform, mainTurret, ref ___MannedTurret, ref ActivateableTurret, ref maxValue, ref ClosestTurretTransform, ref MannedTurretTransform);
                    }
                    if (ActivateableTurret == null)
                    {
                        int index = 0;
                        foreach (Transform stationTransform in __instance.PlayerOwner.StartingShip.WeaponsSystem.RegularTurretStationTransforms)
                        {
                            CrewAI.PLBotManTurretFix.CheckTurretTransform(__instance, stationTransform, __instance.PlayerOwner.StartingShip.MyStats.GetRegularTurretAtIndex(index), ref ___MannedTurret, ref ActivateableTurret, ref maxValue, ref ClosestTurretTransform, ref MannedTurretTransform);
                            ++index;
                        }
                    }
                    if ((UnityEngine.Object)MannedTurretTransform != (UnityEngine.Object)null)
                    {
                        __instance.AI_TargetPos = MannedTurretTransform.position;
                        __instance.AI_TargetPos_Raw = __instance.AI_TargetPos;
                        __instance.EnablePathing = true;
                        ___MyBotController.AI_ItemUtilityRequest = EItemUtilityType.E_NONE;
                        ___MyBotController.AI_ShouldUseActiveItem = false;
                    }
                    else if ((UnityEngine.Object)ClosestTurretTransform != (UnityEngine.Object)null)
                    {
                        __instance.AI_TargetPos = ClosestTurretTransform.position;
                        __instance.AI_TargetPos_Raw = __instance.AI_TargetPos;
                        __instance.EnablePathing = true;
                        ___MyBotController.AI_ItemUtilityRequest = EItemUtilityType.E_NONE;
                        ___MyBotController.AI_ShouldUseActiveItem = false;
                    }
                    if ((double)maxValue < 16.0)
                    {
                        if (___MannedTurret == null)
                            ___MannedTurret = ActivateableTurret;
                        if (___MannedTurret != null && ___MannedTurret != ActivateableTurret)
                        {
                            ___MannedTurret.ShipStats.Ship.RequestTurretControl(___MannedTurret.TurretID, -1);
                            ___MannedTurret.ShipStats.Ship.RequestTurretControl(ActivateableTurret.TurretID, __instance.PlayerOwner.GetPlayerID());
                            ___MannedTurret = ActivateableTurret;
                        }
                        if ((UnityEngine.Object)___MyBotController != (UnityEngine.Object)null && (double)maxValue > 1.0)
                        {
                            ___MyBotController.TargetRot = PLGlobal.SafeLookRotation(Vector3.ProjectOnPlane((__instance.AI_TargetPos - ___MyBotController.transform.position).normalized, Vector3.up), Vector3.up);
                            __instance.transform.position = ClosestTurretTransform.position - ClosestTurretTransform.forward;
                        }
                    }
                    if (__instance.PlayerOwner.GetClassID() == 3 && ___MannedTurret != null && ___MannedTurret.GetHasTrackingMissileCapability() && __instance.PlayerOwner.ActiveSubPriority != null)
                    {
                        int num;
                        switch (__instance.PlayerOwner.ActiveSubPriority.TypeData)
                        {
                            case 41:
                                num = 2;
                                break;
                            case 42:
                                num = 3;
                                break;
                            case 43:
                                num = 1;
                                break;
                            case 44:
                                if ((double)Time.time - (double)___lastRandomSysTime > 16.0 && ___MannedTurret != null)
                                {
                                    ___lastRandomSysTime = Time.time;
                                    if ((UnityEngine.Object)__instance.PlayerOwner.StartingShip != (UnityEngine.Object)null && (UnityEngine.Object)__instance.PlayerOwner.StartingShip.TargetShip != (UnityEngine.Object)null)
                                    {
                                        num = UnityEngine.Random.Range(0, 4);
                                        for (int index = 0; index < 5 && __instance.PlayerOwner.StartingShip.TargetShip.IsDrone && num == 2; ++index)
                                            num = UnityEngine.Random.Range(0, 4);
                                        break;
                                    }
                                    num = UnityEngine.Random.Range(0, 4);
                                    break;
                                }
                                num = -1;
                                break;
                            default:
                                num = 0;
                                break;
                        }
                        if (num != -1 && __instance.PlayerOwner.StartingShip.SelectedMissileLauncher != null && ___MannedTurret != null && (double)Time.time - (double)___lastTargetSetTime > 2.0)
                        {
                            ___lastTargetSetTime = Time.time;
                            if (__instance.PlayerOwner.StartingShip.SelectedMissileLauncher.TargetedSystemID != num)
                            {
                                __instance.PlayerOwner.StartingShip.SelectedMissileLauncher.TargetedSystemID = num;
                                PLServer.Instance.photonView.RPC("SetShipMissileLauncherTarget", PhotonTargets.Others, (object)__instance.PlayerOwner.StartingShip.ShipID, (object)__instance.PlayerOwner.StartingShip.SelectedMissileLauncher.NetID, (object)num);
                            }
                        }
                    }
                }
                __result = BehaveResult.Success;
                return false;
            }

            private static void CheckTurretTransform(
              PLBot inBot,
              Transform tap,
              PLTurret inTurret,
              ref PLTurret MannedTurret,
              ref PLTurret ActivateableTurret,
              ref float ClosestTurretSqDist,
              ref Transform ClosestTurretTransform,
              ref Transform MannedTurretTransform)
            {
                if (inTurret == null || !((UnityEngine.Object)inBot.PlayerOwner != (UnityEngine.Object)null) || !((UnityEngine.Object)inBot.PlayerOwner.GetPawn() != (UnityEngine.Object)null))
                    return;
                if (inTurret == MannedTurret)
                    MannedTurretTransform = tap;
                float num = Vector3.SqrMagnitude(inBot.PlayerOwner.GetPawn().transform.position - tap.position);
                if ((double)num >= (double)ClosestTurretSqDist || inTurret.ShipStats.Ship.GetCurrentTurretControllerPlayerID(inTurret.TurretID) != -1 && inTurret.ShipStats.Ship.GetCurrentTurretControllerPlayerID(inTurret.TurretID) != inBot.PlayerOwner.GetPlayerID())
                    return;
                ClosestTurretSqDist = num;
                ActivateableTurret = inTurret;
                ClosestTurretTransform = tap;
            }
        }

        [HarmonyPatch(typeof(PLBotController), "GetCurrentTLISwitchPoint")]
        internal class BotBoardShipFix
        {
            private static bool Prefix(
              PLBotController __instance,
              PLTeleportationLocationInstance targetTLI,
              ref Vector3 switchPoint,
              ref bool __result)
            {
                if ((UnityEngine.Object)targetTLI != (UnityEngine.Object)null && (UnityEngine.Object)targetTLI.MyShipInfo != (UnityEngine.Object)null)
                {
                    bool flag = (UnityEngine.Object)__instance.MyPawn != (UnityEngine.Object)null && (UnityEngine.Object)__instance.MyPawn.MyPlayer != (UnityEngine.Object)null && (UnityEngine.Object)__instance.MyPawn.MyPlayer.StartingShip == (UnityEngine.Object)targetTLI.MyShipInfo;
                    if (targetTLI.MyShipInfo.IsQuantumShieldActive && !flag)
                    {
                        switchPoint = Vector3.zero;
                        __result = false;
                        return false;
                    }
                }
                if ((UnityEngine.Object)targetTLI != (UnityEngine.Object)null && (UnityEngine.Object)__instance.MyPawn.MyPlayer.MyCurrentTLI != (UnityEngine.Object)null && (UnityEngine.Object)__instance.MyPawn.MyPlayer.MyCurrentTLI != (UnityEngine.Object)targetTLI)
                {
                    if ((UnityEngine.Object)__instance.MyPawn.MyPlayer.MyCurrentTLI.MyShipInfo != (UnityEngine.Object)null)
                    {
                        PLTeleportationTargetInstance ttiOnCurrentShip = __instance.MyBot.GetNearestTTIOnCurrentShip();
                        if ((UnityEngine.Object)ttiOnCurrentShip != (UnityEngine.Object)null)
                        {
                            switchPoint = ttiOnCurrentShip.transform.position;
                            __result = true;
                            return false;
                        }
                        switchPoint = Vector3.zero;
                        __result = false;
                        return false;
                    }
                    if (__instance.MyPawn.MyPlayer.MyCurrentTLI.AllTTIs.Length != 0)
                    {
                        PLTeleportationTargetInstance teleportationTargetInstance = (PLTeleportationTargetInstance)null;
                        float num1 = float.MaxValue;
                        foreach (PLTeleportationTargetInstance allTtI in __instance.MyPawn.MyPlayer.MyCurrentTLI.AllTTIs)
                        {
                            if ((UnityEngine.Object)allTtI != (UnityEngine.Object)null && allTtI.Unlocked)
                            {
                                float num2 = Vector3.SqrMagnitude(allTtI.transform.position - __instance.transform.position);
                                if ((double)num2 < (double)num1)
                                {
                                    num1 = num2;
                                    teleportationTargetInstance = allTtI;
                                }
                            }
                        }
                        if ((UnityEngine.Object)teleportationTargetInstance != (UnityEngine.Object)null)
                        {
                            switchPoint = teleportationTargetInstance.transform.position;
                            __result = true;
                            return false;
                        }
                    }
                    else
                    {
                        if ((UnityEngine.Object)__instance.MyPawn.MyPlayer.MyCurrentTLI.MyBSO != (UnityEngine.Object)null && (UnityEngine.Object)__instance.MyPawn.MyPlayer.MyCurrentTLI.MyBSO.MyTTI != (UnityEngine.Object)null)
                        {
                            switchPoint = __instance.MyPawn.MyPlayer.MyCurrentTLI.MyBSO.MyTTI.transform.position;
                            __result = true;
                            return false;
                        }
                        if ((UnityEngine.Object)PLNetworkManager.Instance.CurrentGame != (UnityEngine.Object)null && (UnityEngine.Object)PLNetworkManager.Instance.CurrentGame.GetComponent<PLTeleportationTargetInstance>() != (UnityEngine.Object)null)
                        {
                            switchPoint = PLNetworkManager.Instance.CurrentGame.GetComponent<PLTeleportationTargetInstance>().transform.position;
                            __result = true;
                            return false;
                        }
                    }
                }
                switchPoint = Vector3.zero;
                __result = false;
                return false;
            }
        }

        internal class PLServerAICoroutine
        {
            public static void UpdateAIPriorities(PLPlayer player)
            {
                if (!((UnityEngine.Object)player.MyBot != (UnityEngine.Object)null) || player.GetAIData() == null)
                    return;
                if (!player.MyBot.GetIsSimpleCombatBot())
                {
                    Traverse traverse = Traverse.Create((object)player);
                    traverse.Method("ManageAIPriorities_PreSort").GetValue();
                    foreach (AIPriority priority in player.GetAIData().Priorities)
                        traverse.Method("UpdateAIPriority", new System.Type[1]
                        {
              typeof (AIPriority)
                        }, (object[])null).GetValue((object)priority);
                    traverse.Method("SortAIPriorities").GetValue();
                    player.ActiveMainPriority = (AIPriority)null;
                    player.ActiveSubPriority = (AIPriority)null;
                }
                player.MyBot.ClearTargetedPawn();
                player.MyBot.AI_TargetTLI = player.MyCurrentTLI;
                player.MyBot.AI_TargetInterior = !((UnityEngine.Object)player.GetPawn() != (UnityEngine.Object)null) ? player.CurrentInterior : player.GetPawn().MyInterior;
                if (!PhotonNetwork.isMasterClient || !((UnityEngine.Object)player.GetPawn() != (UnityEngine.Object)null) || (bool)player.GetPawn().IsDead || (double)Time.timeScale == 0.0)
                    return;
                if ((UnityEngine.Object)player.GetPawn().MyController != (UnityEngine.Object)null)
                    player.GetPawn().MyController.PreAIPriorityTick();
                if (player.MyBot.GetIsSimpleCombatBot())
                {
                    int invaderAction1 = (int)player.MyBot.TickFindInvaderAction((Tree)null);
                }
                else if (player.GetPawn().SpawnedInArena)
                    player.MyBot.TickHuntEnemies();
                else if ((UnityEngine.Object)PLLCChair.Instance != (UnityEngine.Object)null && PLLCChair.Instance.Triggered_LevelTwo && !PLLCChair.Instance.Triggered_LevelThree)
                    player.MyBot.Tick_HelpWithChairSyncMiniGame(false);
                else if ((UnityEngine.Object)PLLCChair.Instance != (UnityEngine.Object)null && !PLLCChair.Instance.Triggered && PLLCChair.Instance.AnyPrePhaseErrorHasBeenCleared && PLLCChair.Instance.GetNumErrors(true) > 0)
                {
                    player.MyBot.Tick_HelpWithChairSyncMiniGame(true);
                }
                else
                {
                    foreach (AIPriority priority in player.GetAIData().Priorities)
                    {
                        if (priority.Type != AIPriorityType.E_MAIN && priority.Type != AIPriorityType.E_CLASS_MAIN && priority.LastCalculated_PriorityValue >= 1)
                            priority.LastExecuteTime = Time.time;
                    }
                    foreach (AIPriority priority in player.GetAIData().Priorities)
                    {
                        if (priority.Type == AIPriorityType.E_MAIN || priority.Type == AIPriorityType.E_CLASS_MAIN)
                        {
                            if (player.ActiveMainPriority != priority)
                                priority.LastExecuteTime = Time.time;
                            player.ActiveMainPriority = priority;
                            if (player.ActiveMainPriority != null && player.ActiveMainPriority.Subpriorities.Count > 0)
                            {
                                if (player.ActiveSubPriority != player.ActiveMainPriority.Subpriorities[0])
                                    player.ActiveMainPriority.Subpriorities[0].LastExecuteTime = Time.time;
                                player.ActiveSubPriority = player.ActiveMainPriority.Subpriorities[0];
                            }
                            else
                                player.ActiveSubPriority = (AIPriority)null;
                            if (player.ActiveMainPriority == null)
                                break;
                            switch (player.ActiveMainPriority.Type)
                            {
                                case AIPriorityType.E_MAIN:
                                case AIPriorityType.E_CLASS_MAIN:
                                    switch ((EAIPriorityListDisplayed)player.ActiveMainPriority.TypeData)
                                    {
                                        case EAIPriorityListDisplayed.REPAIR_SYSTEM:
                                            int num1 = (int)player.MyBot.TickRepairNearbySystemAction((Tree)null);
                                            return;
                                        case EAIPriorityListDisplayed.FIRE_PATROL:
                                            int fireAction = (int)player.MyBot.TickGoToFireAction((Tree)null);
                                            return;
                                        case EAIPriorityListDisplayed.REPEL_INVADERS:
                                        case EAIPriorityListDisplayed.DEFEND_SELF:
                                            int invaderAction2 = (int)player.MyBot.TickFindInvaderAction((Tree)null);
                                            return;
                                        case EAIPriorityListDisplayed.MAN_STATION:
                                        case EAIPriorityListDisplayed.PIL_FLY_SHIP:
                                            int num2 = (int)player.MyBot.TickOptimizeStationAction((Tree)null);
                                            return;
                                        case EAIPriorityListDisplayed.MAN_TURRET:
                                            int num3 = (int)player.MyBot.TickManTurretAction((Tree)null);
                                            return;
                                        case EAIPriorityListDisplayed.CLOSE_TO_CAPTAIN:
                                            int captainAction = (int)player.MyBot.TickGoCloseToCaptainAction((Tree)null);
                                            return;
                                        case EAIPriorityListDisplayed.BOARD_ENEMY_SHIP:
                                            if (player.ActiveSubPriority == null)
                                                return;
                                            switch ((EAIPriorityListDisplayed)player.ActiveSubPriority.TypeData)
                                            {
                                                case EAIPriorityListDisplayed.CAPTURE_SCREENS:
                                                    int captureScreensAction = (int)player.MyBot.TickFindAndCaptureScreensAction((Tree)null);
                                                    return;
                                                case EAIPriorityListDisplayed.DESTROY_SHIP_SYSTEMS:
                                                    player.MyBot.TickHuntShipSystems();
                                                    return;
                                                case EAIPriorityListDisplayed.NEUTRALIZE_CREW:
                                                    player.MyBot.TickHuntEnemies();
                                                    return;
                                                case EAIPriorityListDisplayed.STAY_TOGETHER:
                                                    player.MyBot.TickFormHuntingParty();
                                                    return;
                                                default:
                                                    int enemyShipAction = (int)player.MyBot.TickGoToEnemyShipAction((Tree)null);
                                                    return;
                                            }
                                        case EAIPriorityListDisplayed.WEAPONS_USE:
                                            return;
                                        case EAIPriorityListDisplayed.VOCAL_SLASH_COMMS:
                                            return;
                                        case EAIPriorityListDisplayed.MISSILE_USAGE:
                                            return;
                                        case EAIPriorityListDisplayed.STARTUP_SHIP:
                                            int num4 = (int)player.MyBot.TickStartupShipAction((Tree)null);
                                            return;
                                        case EAIPriorityListDisplayed.HEAL_SELF:
                                            int atriumAction = (int)player.MyBot.TickFindAtriumAction((Tree)null);
                                            return;
                                        case EAIPriorityListDisplayed.ENG_LOWER_SHIELDS_IN_DEPOT:
                                            int num5 = (int)player.MyBot.TickStartupShipAction((Tree)null);
                                            return;
                                        case EAIPriorityListDisplayed.ENG_MANAGE_REACTOR:
                                            return;
                                        case EAIPriorityListDisplayed.SCI_SHIELD_FREQ_0:
                                            return;
                                        case EAIPriorityListDisplayed.SCI_SHIELD_FREQ_1:
                                            return;
                                        case EAIPriorityListDisplayed.DESTROY_SHIP_SYSTEMS:
                                            return;
                                        case EAIPriorityListDisplayed.NEUTRALIZE_CREW:
                                            return;
                                        case EAIPriorityListDisplayed.TAKE_OVER_SCREENS:
                                            return;
                                        case EAIPriorityListDisplayed.STAY_TOGETHER:
                                            return;
                                        case EAIPriorityListDisplayed.GOTO_ENEMY_SHIP:
                                            return;
                                        case EAIPriorityListDisplayed.GETAMMO:
                                            player.MyBot.TickGoToNearestAmmoSupply();
                                            return;
                                        case EAIPriorityListDisplayed.ENG_JUMP_SHIP:
                                            return;
                                        case EAIPriorityListDisplayed.SELL_BISCUITS:
                                            player.MyBot.TickSellBiscuits();
                                            return;
                                        case EAIPriorityListDisplayed.MANAGE_CLOAKING_SYS:
                                            player.MyBot.TickManageCloakingSys();
                                            return;
                                        case EAIPriorityListDisplayed.SCI_SEARCH_FOR_SHIPS:
                                            return;
                                        case EAIPriorityListDisplayed.REPAIR_CREWMATES:
                                            int num6 = (int)player.MyBot.TickRepairCrewmates();
                                            return;
                                        case EAIPriorityListDisplayed.REVIVE_CREWMATE:
                                            int num7 = (int)player.MyBot.TickReviveCrewmate();
                                            return;
                                        case EAIPriorityListDisplayed.AT_EASE:
                                            player.MyBot.TickAtEase();
                                            return;
                                        case EAIPriorityListDisplayed.HEAL_CREW:
                                            int num8 = (int)player.MyBot.TickHealCrewmates();
                                            return;
                                        case EAIPriorityListDisplayed.MANUAL_PRG_CRG:
                                            player.MyBot.TickManualProgramCharge();
                                            return;
                                        case EAIPriorityListDisplayed.LAUNCH_NUKE:
                                            player.MyBot.TickLaunchNuke();
                                            return;
                                        case EAIPriorityListDisplayed.PIL_USE_WARP_STATION:
                                            return;
                                        case EAIPriorityListDisplayed.PIL_PATROL:
                                            return;
                                        case EAIPriorityListDisplayed.ENG_OVERCLOCK:
                                            return;
                                        case EAIPriorityListDisplayed.SCI_SHUTDOWN_SHIP:
                                            int num9 = (int)player.MyBot.TickStartupShipAction((Behave.Runtime.Tree)null);
                                            return;
                                        case EAIPriorityListDisplayed.AUX_POWER_0:
                                            return;
                                        case EAIPriorityListDisplayed.AUX_POWER_1:
                                            return;
                                        case EAIPriorityListDisplayed.AUX_POWER_2:
                                            return;
                                        case EAIPriorityListDisplayed.AUX_POWER_3:
                                            return;
                                        case EAIPriorityListDisplayed.AUX_POWER_4:
                                            return;
                                        case EAIPriorityListDisplayed.AUX_POWER_5:
                                            return;
                                        case EAIPriorityListDisplayed.AUX_POWER_6:
                                            return;
                                        case EAIPriorityListDisplayed.AUX_POWER_7:
                                            return;
                                        case EAIPriorityListDisplayed.ENG_EJECT_CORE:
                                            player.MyBot.TickEjectReactor();
                                            return;
                                        default:
                                            return;
                                    }
                                default:
                                    return;
                            }
                        }
                    }
                }
            }

            private static IEnumerator ServerUpdateAIPrioritiesRoutine(PLServer instance)
            {
                int num = 0;
                while ((UnityEngine.Object)instance != (UnityEngine.Object)null)
                {
                    PLPlayer player;
                    for (player = (PLPlayer)null; (UnityEngine.Object)player == (UnityEngine.Object)null && num < instance.AllPlayers.Count; ++num)
                    {
                        if ((UnityEngine.Object)instance.AllPlayers[num] != (UnityEngine.Object)null && instance.AllPlayers[num].IsBot)
                            player = instance.AllPlayers[num];
                    }
                    if ((UnityEngine.Object)player != (UnityEngine.Object)null)
                    {
                        try
                        {
                            CrewAI.PLServerAICoroutine.UpdateAIPriorities(player);
                        }
                        catch (Exception ex)
                        {
                            Debug.Log((object)"[ExGal] Exception in ServerUpdateAIPrioritiesRoutine:");
                            Debug.Log((object)ex.ToString());
                            Debug.Log((object)"------------------------------");
                            Debug.Log((object)ex.Source);
                            Debug.Log((object)"------------------------------");
                            Debug.Log((object)ex.Message);
                            Debug.Log((object)"------------------------------");
                            Debug.Log((object)ex.StackTrace);
                            Debug.Log((object)"[ExGal] End of Exception Info");
                            Debug.Log((object)("[ExGal] Skipping update of player with ID: " + player.GetPlayerID().ToString()));
                        }
                    }
                    if (num >= instance.AllPlayers.Count)
                        num = 0;
                    yield return (object)new WaitForEndOfFrame();
                }
            }

            internal static void ServerStartAICoroutine(PLServer __instance) => __instance.StartCoroutine(CrewAI.PLServerAICoroutine.ServerUpdateAIPrioritiesRoutine(__instance));

            [HarmonyPatch(typeof(PLAIPriorityOverride), "IsActive")]
            internal class PLAIPriorityOverrideIsActivePatch
            {
                private static bool Prefix(
                  PLAIPriorityOverride __instance,
                  PLPlayer playerContext,
                  AIPriority priorityContext,
                  ref bool ___m_LastCalculated_IsActive,
                  ref bool __result)
                {
                    if (__instance.OverrideType != EPriorityOverrideType.E_CUSTOM || __instance.OverrideSubID != 35)
                        return true;
                    bool flag1 = (UnityEngine.Object)playerContext != (UnityEngine.Object)null && (UnityEngine.Object)playerContext.StartingShip != (UnityEngine.Object)null;
                    bool flag2 = flag1 && (playerContext.StartingShip.NuclearLaunchStage >= 2 || (UnityEngine.Object)playerContext.StartingShip.ActiveNuke != (UnityEngine.Object)null);
                    if (__instance.Inverted)
                        flag2 = !flag2;
                    ___m_LastCalculated_IsActive = flag2 & flag1;
                    __result = ___m_LastCalculated_IsActive;
                    return false;
                }

                private static void Postfix(
                  PLAIPriorityOverride __instance,
                  PLPlayer playerContext,
                  AIPriority priorityContext,
                  ref bool ___m_LastCalculated_IsActive_Init)
                {
                    ___m_LastCalculated_IsActive_Init = false;
                }
            }

            [HarmonyPatch(typeof(PLPlayer), "UpdateAIPriorities")]
            internal class PLPlayerUpdateAIPrioritiesPatch
            {
                private static bool Prefix() => false;
            }

            [HarmonyPatch(typeof(PLServer), "OnGameOver")]
            internal class EnsureBotUpdatesOnGameOver
            {
                private static void Postfix(PLServer __instance, bool backToMainMenu)
                {
                    if (backToMainMenu)
                        return;
                    CrewAI.PLServerAICoroutine.ServerStartAICoroutine(__instance);
                }
            }
        }

        [HarmonyPatch(typeof(PLGlobal), "SetupClassDefaultData")]
        internal class EnemyAIDataPatch
        {
            private static Exception Finalizer(
              Exception __exception,
              ref AIDataIndividual dataInv,
              int classID,
              bool enemyAI)
            {
                if (!enemyAI)
                    return __exception;
                dataInv.Priorities.Clear();
                switch (classID)
                {
                    case 1:
                        AIPriority aiPriority1 = new AIPriority(AIPriorityType.E_TWEAK, 82, 3);
                        dataInv.Priorities.Add(aiPriority1);
                        AIPriority aiPriority2 = new AIPriority(AIPriorityType.E_CLASS_MAIN, 32, 2);
                        dataInv.Priorities.Add(aiPriority2);
                        aiPriority2.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 0, 0, inData: 15));
                        aiPriority2.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 22, 0));
                        aiPriority2.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 1, 4));
                        aiPriority2.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20, 2, true));
                        aiPriority2.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 56, 5, inData: 10));
                        aiPriority2.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 13, 5, true, 20));
                        aiPriority2.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 50, 5, inData: 12));
                        aiPriority2.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 7, 5, inData: 30));
                        AIPriority aiPriority3 = new AIPriority(AIPriorityType.E_CLASS_MAIN, 70, 2);
                        aiPriority2.Subpriorities.Add(aiPriority3);
                        aiPriority3.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 27, 0, inData: 0));
                        aiPriority3.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 6, 0, true));
                        AIPriority aiPriority4 = new AIPriority(AIPriorityType.E_CLASS_MAIN, 35, 1);
                        aiPriority2.Subpriorities.Add(aiPriority4);
                        aiPriority4.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 64, 5));
                        aiPriority4.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 0, 1, true));
                        aiPriority4.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 75, 4, inData: 125));
                        AIPriority aiPriority5 = new AIPriority(AIPriorityType.E_CLASS_MAIN, 69, 0);
                        aiPriority2.Subpriorities.Add(aiPriority5);
                        AIPriority aiPriority6 = new AIPriority(AIPriorityType.E_CLASS_MAIN, 36, 0);
                        aiPriority2.Subpriorities.Add(aiPriority6);
                        aiPriority6.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 30, 5));
                        AIPriority aiPriority7 = new AIPriority(AIPriorityType.E_CLASS_MAIN, 34, 0);
                        aiPriority2.Subpriorities.Add(aiPriority7);
                        aiPriority7.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 6, 0));
                        aiPriority7.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 1, inData: 20));
                        AIPriority aiPriority8 = new AIPriority(AIPriorityType.E_CLASS_MAIN, 33, 1);
                        aiPriority2.Subpriorities.Add(aiPriority8);
                        aiPriority8.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 6, 0));
                        aiPriority8.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 27, 2, inData: 0));
                        aiPriority8.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 75, 2, true, 100));
                        aiPriority8.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 64, 1));
                        aiPriority8.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 50, 2, true, 8));
                        AIPriority aiPriority9 = new AIPriority(AIPriorityType.E_CLASS_MAIN, 29, 0);
                        aiPriority2.Subpriorities.Add(aiPriority9);
                        AIPriority aiPriority10 = new AIPriority(AIPriorityType.E_CLASS_MAIN, 28, 0);
                        aiPriority2.Subpriorities.Add(aiPriority10);
                        AIPriority aiPriority11 = new AIPriority(AIPriorityType.E_CLASS_MAIN, 27, 0);
                        aiPriority2.Subpriorities.Add(aiPriority11);
                        AIPriority aiPriority12 = new AIPriority(AIPriorityType.E_MAIN, 4, 1);
                        dataInv.Priorities.Add(aiPriority12);
                        aiPriority12.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20, 0, inData: 0));
                        AIPriority aiPriority13 = new AIPriority(AIPriorityType.E_CLASS_MAIN, 67, 0);
                        dataInv.Priorities.Add(aiPriority13);
                        AIPriority aiPriority14 = new AIPriority(AIPriorityType.E_MAIN, 66, 3);
                        dataInv.Priorities.Add(aiPriority14);
                        aiPriority14.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 69, 0, true));
                        aiPriority14.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 70, 0, true));
                        AIPriority aiPriority15 = new AIPriority(AIPriorityType.E_MAIN, 65, 0);
                        dataInv.Priorities.Add(aiPriority15);
                        aiPriority15.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 0, 0, inData: 15));
                        aiPriority15.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 50, 5, true, 25));
                        AIPriority aiPriority16 = new AIPriority(AIPriorityType.E_MAIN, 64, 0);
                        dataInv.Priorities.Add(aiPriority16);
                        aiPriority16.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 67, 5));
                        AIPriority aiPriority17 = new AIPriority(AIPriorityType.E_MAIN, 63, 3);
                        dataInv.Priorities.Add(aiPriority17);
                        aiPriority17.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 65, 0, true));
                        aiPriority17.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 66, 0, true));
                        aiPriority17.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 69, 0));
                        AIPriority aiPriority18 = new AIPriority(AIPriorityType.E_MAIN, 60, 0);
                        dataInv.Priorities.Add(aiPriority18);
                        AIPriority aiPriority19 = new AIPriority(AIPriorityType.E_MAIN, 58, 0);
                        dataInv.Priorities.Add(aiPriority19);
                        aiPriority19.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20, 0));
                        aiPriority19.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 57, 0, true));
                        aiPriority19.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 56, inData: 80));
                        AIPriority aiPriority20 = new AIPriority(AIPriorityType.E_MAIN, 49, 0);
                        dataInv.Priorities.Add(aiPriority20);
                        aiPriority20.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 52, 0, true));
                        aiPriority20.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 51, 5));
                        aiPriority20.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 12, 4, inData: 2));
                        AIPriority aiPriority21 = new AIPriority(AIPriorityType.E_MAIN, 11, 0);
                        dataInv.Priorities.Add(aiPriority21);
                        aiPriority21.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20, 0));
                        aiPriority21.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 52, 0));
                        aiPriority21.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 13, inInverted: true, inData: 55));
                        AIPriority aiPriority22 = new AIPriority(AIPriorityType.E_CLASS_MAIN, 10, 0);
                        dataInv.Priorities.Add(aiPriority22);
                        AIPriority aiPriority23 = new AIPriority(AIPriorityType.E_TWEAK, 8, 0);
                        dataInv.Priorities.Add(aiPriority23);
                        AIPriority aiPriority24 = new AIPriority(AIPriorityType.E_CLASS_MAIN, 6, 0);
                        dataInv.Priorities.Add(aiPriority24);
                        aiPriority24.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20));
                        aiPriority24.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 26, 0, true));
                        aiPriority24.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 0, 5, inData: 15));
                        aiPriority24.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 50, 0, inData: 12));
                        aiPriority24.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 7, 0, inData: 30));
                        aiPriority24.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 64));
                        AIPriority aiPriority25 = new AIPriority(AIPriorityType.E_MAIN, 57, 3);
                        aiPriority24.Subpriorities.Add(aiPriority25);
                        aiPriority25.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20, 0, inData: 0));
                        AIPriority aiPriority26 = new AIPriority(AIPriorityType.E_MAIN, 12, 2);
                        aiPriority24.Subpriorities.Add(aiPriority26);
                        aiPriority26.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 54, 0, inData: 5));
                        AIPriority aiPriority27 = new AIPriority(AIPriorityType.E_MAIN, 54, 1);
                        aiPriority24.Subpriorities.Add(aiPriority27);
                        aiPriority27.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 51, 5));
                        AIPriority aiPriority28 = new AIPriority(AIPriorityType.E_MAIN, 5, 0);
                        dataInv.Priorities.Add(aiPriority28);
                        aiPriority28.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20, 0));
                        aiPriority28.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 21, 0, true));
                        aiPriority28.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 0, 5, inData: 15));
                        aiPriority28.Metadata.Add((PLPriorityMetadata)new PLPriorityMetadata_Float(3f, inName: "Range (m)"));
                        AIPriority aiPriority29 = new AIPriority(AIPriorityType.E_MAIN, 2, 0);
                        dataInv.Priorities.Add(aiPriority29);
                        aiPriority29.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 52, 0));
                        aiPriority29.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 51, 5));
                        AIPriority aiPriority30 = new AIPriority(AIPriorityType.E_MAIN, 1, 0);
                        dataInv.Priorities.Add(aiPriority30);
                        aiPriority30.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 98, 0, true));
                        aiPriority30.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 54, 4, inData: 5));
                        AIPriority aiPriority31 = new AIPriority(AIPriorityType.E_MAIN, 0, 4);
                        dataInv.Priorities.Add(aiPriority31);
                        aiPriority31.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 66, 0, true));
                        aiPriority31.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20, 0, inData: 0));
                        aiPriority31.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 54, 0, inData: 5));
                        aiPriority31.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 55, inData: 100));
                        aiPriority31.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 86, 0, true, 60));
                        aiPriority31.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 44, 0, inData: 20));
                        break;
                    case 2:
                        AIPriority aiPriority32 = new AIPriority(AIPriorityType.E_TWEAK, 82, 3);
                        dataInv.Priorities.Add(aiPriority32);
                        AIPriority aiPriority33 = new AIPriority(AIPriorityType.E_PROGRAM, 44, 2);
                        dataInv.Priorities.Add(aiPriority33);
                        AIPriority aiPriority34 = new AIPriority(AIPriorityType.E_PROGRAM, 43, 2);
                        dataInv.Priorities.Add(aiPriority34);
                        AIPriority aiPriority35 = new AIPriority(AIPriorityType.E_PROGRAM, 42, 2);
                        dataInv.Priorities.Add(aiPriority35);
                        AIPriority aiPriority36 = new AIPriority(AIPriorityType.E_PROGRAM, 41, 2);
                        dataInv.Priorities.Add(aiPriority36);
                        AIPriority aiPriority37 = new AIPriority(AIPriorityType.E_PROGRAM, 22, 2);
                        dataInv.Priorities.Add(aiPriority37);
                        aiPriority37.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 75, 4, inData: 100));
                        AIPriority aiPriority38 = new AIPriority(AIPriorityType.E_PROGRAM, 21, 2);
                        dataInv.Priorities.Add(aiPriority38);
                        aiPriority38.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 75, 4, inData: 100));
                        AIPriority aiPriority39 = new AIPriority(AIPriorityType.E_PROGRAM, 19, 2);
                        dataInv.Priorities.Add(aiPriority39);
                        aiPriority39.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 75, 4, inData: 100));
                        AIPriority aiPriority40 = new AIPriority(AIPriorityType.E_PROGRAM, 12, 2);
                        dataInv.Priorities.Add(aiPriority40);
                        AIPriority aiPriority41 = new AIPriority(AIPriorityType.E_PROGRAM, 11, 2);
                        dataInv.Priorities.Add(aiPriority41);
                        AIPriority aiPriority42 = new AIPriority(AIPriorityType.E_PROGRAM, 10, 2);
                        dataInv.Priorities.Add(aiPriority42);
                        AIPriority aiPriority43 = new AIPriority(AIPriorityType.E_PROGRAM, 9, 2);
                        dataInv.Priorities.Add(aiPriority43);
                        aiPriority43.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 48, 0));
                        aiPriority43.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 49, 0));
                        AIPriority aiPriority44 = new AIPriority(AIPriorityType.E_PROGRAM, 3, 2);
                        dataInv.Priorities.Add(aiPriority44);
                        AIPriority aiPriority45 = new AIPriority(AIPriorityType.E_CLASS_MAIN, 3, 2);
                        dataInv.Priorities.Add(aiPriority45);
                        aiPriority45.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 0, 0, inData: 15));
                        aiPriority45.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20, 2, true));
                        aiPriority45.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 56, 5, inData: 10));
                        aiPriority45.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 13, 5, true, 20));
                        aiPriority45.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 7, 5, inData: 30));
                        AIPriority aiPriority46 = new AIPriority(AIPriorityType.E_CLASS_MAIN, 62, 1);
                        aiPriority45.Subpriorities.Add(aiPriority46);
                        aiPriority46.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 64, 0));
                        aiPriority46.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 6, 2));
                        AIPriority aiPriority47 = new AIPriority(AIPriorityType.E_CLASS_MAIN, 39, 1);
                        aiPriority45.Subpriorities.Add(aiPriority47);
                        aiPriority47.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 6, 2, true));
                        AIPriority aiPriority48 = new AIPriority(AIPriorityType.E_CLASS_MAIN, 38, 1);
                        aiPriority45.Subpriorities.Add(aiPriority48);
                        aiPriority48.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 64));
                        AIPriority aiPriority49 = new AIPriority(AIPriorityType.E_PROGRAM, 2, 2);
                        dataInv.Priorities.Add(aiPriority49);
                        aiPriority49.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 49, 0));
                        AIPriority aiPriority50 = new AIPriority(AIPriorityType.E_PROGRAM, 1, 2);
                        dataInv.Priorities.Add(aiPriority50);
                        aiPriority50.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 80, 0, true, 20));
                        AIPriority aiPriority51 = new AIPriority(AIPriorityType.E_PROGRAM, 0, 2);
                        dataInv.Priorities.Add(aiPriority51);
                        aiPriority51.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 64, 0));
                        AIPriority aiPriority52 = new AIPriority(AIPriorityType.E_TWEAK, 52, 1);
                        dataInv.Priorities.Add(aiPriority52);
                        AIPriority aiPriority53 = new AIPriority(AIPriorityType.E_TWEAK, 21, 1);
                        dataInv.Priorities.Add(aiPriority53);
                        aiPriority53.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 89, 0, true, 6));
                        aiPriority53.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 75, inData: 150));
                        aiPriority53.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_ALERT_LEVEL, 0, 2));
                        AIPriority aiPriority54 = new AIPriority(AIPriorityType.E_TWEAK, 18, 0);
                        dataInv.Priorities.Add(aiPriority54);
                        aiPriority54.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 50, 2, true, 12));
                        aiPriority54.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 64, 0));
                        aiPriority54.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 6, 1));
                        aiPriority54.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 92, 2, true));
                        AIPriority aiPriority55 = new AIPriority(AIPriorityType.E_MAIN, 4, 1);
                        dataInv.Priorities.Add(aiPriority55);
                        aiPriority55.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20, 0, inData: 0));
                        AIPriority aiPriority56 = new AIPriority(AIPriorityType.E_CLASS_MAIN, 72, 4);
                        dataInv.Priorities.Add(aiPriority56);
                        aiPriority56.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 0, 0, inData: 15));
                        aiPriority56.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 32, 0, true, 4));
                        aiPriority56.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 86, 0, true, 30));
                        AIPriority aiPriority57 = new AIPriority(AIPriorityType.E_CLASS_MAIN, 67, 0);
                        dataInv.Priorities.Add(aiPriority57);
                        AIPriority aiPriority58 = new AIPriority(AIPriorityType.E_MAIN, 66, 3);
                        dataInv.Priorities.Add(aiPriority58);
                        aiPriority58.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 69, 0, true));
                        aiPriority58.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 70, 0, true));
                        AIPriority aiPriority59 = new AIPriority(AIPriorityType.E_MAIN, 65, 0);
                        dataInv.Priorities.Add(aiPriority59);
                        AIPriority aiPriority60 = new AIPriority(AIPriorityType.E_MAIN, 64, 0);
                        dataInv.Priorities.Add(aiPriority60);
                        aiPriority60.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 67, 4));
                        AIPriority aiPriority61 = new AIPriority(AIPriorityType.E_MAIN, 63, 3);
                        dataInv.Priorities.Add(aiPriority61);
                        aiPriority61.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 65, 0, true));
                        aiPriority61.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 66, 0, true));
                        aiPriority61.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 69, 0));
                        AIPriority aiPriority62 = new AIPriority(AIPriorityType.E_CLASS_MAIN, 61, 4);
                        dataInv.Priorities.Add(aiPriority62);
                        aiPriority62.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 62, 0, true));
                        aiPriority62.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 64, 0));
                        aiPriority62.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 0, 0, inData: 15));
                        aiPriority62.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 1, 0));
                        aiPriority62.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 86, 0, true));
                        AIPriority aiPriority63 = new AIPriority(AIPriorityType.E_MAIN, 60, 0);
                        dataInv.Priorities.Add(aiPriority63);
                        AIPriority aiPriority64 = new AIPriority(AIPriorityType.E_MAIN, 58, 0);
                        dataInv.Priorities.Add(aiPriority64);
                        aiPriority64.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20, 0));
                        aiPriority64.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 57, 0, true));
                        aiPriority64.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 56, inData: 80));
                        AIPriority aiPriority65 = new AIPriority(AIPriorityType.E_TWEAK, 51, 0);
                        dataInv.Priorities.Add(aiPriority65);
                        aiPriority65.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 53, 2));
                        AIPriority aiPriority66 = new AIPriority(AIPriorityType.E_MAIN, 49, 0);
                        dataInv.Priorities.Add(aiPriority66);
                        aiPriority66.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 52, 0, true));
                        aiPriority66.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 51, 5));
                        aiPriority66.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 12, 4, inData: 2));
                        AIPriority aiPriority67 = new AIPriority(AIPriorityType.E_PROGRAM, 35, 3);
                        dataInv.Priorities.Add(aiPriority67);
                        aiPriority67.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 6, 0));
                        aiPriority67.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 1, 0, true));
                        AIPriority aiPriority68 = new AIPriority(AIPriorityType.E_PROGRAM, 34, 2);
                        dataInv.Priorities.Add(aiPriority68);
                        aiPriority68.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_ALERT_LEVEL, 2, 0));
                        aiPriority68.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 6, 0));
                        aiPriority68.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 92, 0, true));
                        AIPriority aiPriority69 = new AIPriority(AIPriorityType.E_PROGRAM, 32, 2);
                        dataInv.Priorities.Add(aiPriority69);
                        aiPriority69.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 72, 0, true, 1));
                        aiPriority69.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_ALERT_LEVEL, 0, 0, true));
                        AIPriority aiPriority70 = new AIPriority(AIPriorityType.E_PROGRAM, 31, 2);
                        dataInv.Priorities.Add(aiPriority70);
                        aiPriority70.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 72, 0, true, 5));
                        aiPriority70.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_ALERT_LEVEL, 0, 0, true));
                        AIPriority aiPriority71 = new AIPriority(AIPriorityType.E_PROGRAM, 30, 4);
                        dataInv.Priorities.Add(aiPriority71);
                        aiPriority71.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 32, 0, true, 0));
                        aiPriority71.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_ALERT_LEVEL, 2, 0));
                        AIPriority aiPriority72 = new AIPriority(AIPriorityType.E_PROGRAM, 29, 3);
                        dataInv.Priorities.Add(aiPriority72);
                        aiPriority72.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 6, 0));
                        aiPriority72.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 1, 0, true, 75));
                        AIPriority aiPriority73 = new AIPriority(AIPriorityType.E_PROGRAM, 28, 0);
                        dataInv.Priorities.Add(aiPriority73);
                        AIPriority aiPriority74 = new AIPriority(AIPriorityType.E_PROGRAM, 27, 0);
                        dataInv.Priorities.Add(aiPriority74);
                        AIPriority aiPriority75 = new AIPriority(AIPriorityType.E_PROGRAM, 26, 2);
                        dataInv.Priorities.Add(aiPriority75);
                        aiPriority75.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 1, 0, true, 15));
                        aiPriority75.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_ALERT_LEVEL, 0, 0, true));
                        AIPriority aiPriority76 = new AIPriority(AIPriorityType.E_PROGRAM, 25, 2);
                        dataInv.Priorities.Add(aiPriority76);
                        aiPriority76.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 93, 0));
                        aiPriority76.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_ALERT_LEVEL, 2, 0));
                        AIPriority aiPriority77 = new AIPriority(AIPriorityType.E_PROGRAM, 24, 2);
                        dataInv.Priorities.Add(aiPriority77);
                        aiPriority77.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_ALERT_LEVEL, 2, 0));
                        aiPriority77.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 77, 2, true));
                        aiPriority77.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 91, 0, true));
                        aiPriority77.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 2, 0, true, 75));
                        AIPriority aiPriority78 = new AIPriority(AIPriorityType.E_PROGRAM, 23, 2);
                        dataInv.Priorities.Add(aiPriority78);
                        aiPriority78.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 94, 0, true, 85));
                        AIPriority aiPriority79 = new AIPriority(AIPriorityType.E_PROGRAM, 20, 2);
                        dataInv.Priorities.Add(aiPriority79);
                        aiPriority79.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_ALERT_LEVEL, 0, 0, true));
                        aiPriority79.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 6, 0));
                        aiPriority79.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 92, 0, true));
                        AIPriority aiPriority80 = new AIPriority(AIPriorityType.E_TWEAK, 20, 2);
                        dataInv.Priorities.Add(aiPriority80);
                        aiPriority80.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 89, 0, true, 6));
                        aiPriority80.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_ALERT_LEVEL, 2, 0));
                        aiPriority80.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 6, 0));
                        aiPriority80.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 92, 0, true));
                        AIPriority aiPriority81 = new AIPriority(AIPriorityType.E_PROGRAM, 18, 2);
                        dataInv.Priorities.Add(aiPriority81);
                        aiPriority81.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 3, 0, true));
                        aiPriority81.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_ALERT_LEVEL, 0, 0, true));
                        AIPriority aiPriority82 = new AIPriority(AIPriorityType.E_PROGRAM, 17, 2);
                        dataInv.Priorities.Add(aiPriority82);
                        aiPriority82.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_ALERT_LEVEL, 0, 0, true));
                        aiPriority82.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 6, 0));
                        aiPriority82.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 92, 0, true));
                        AIPriority aiPriority83 = new AIPriority(AIPriorityType.E_PROGRAM, 16, 0);
                        dataInv.Priorities.Add(aiPriority83);
                        aiPriority83.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_ALERT_LEVEL, 0, 0, true));
                        aiPriority83.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 49, 2));
                        aiPriority83.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 9, 0, true, 20));
                        aiPriority83.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 81, 2));
                        AIPriority aiPriority84 = new AIPriority(AIPriorityType.E_PROGRAM, 15, 5);
                        dataInv.Priorities.Add(aiPriority84);
                        aiPriority84.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 92, 5, true));
                        aiPriority84.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 6, 0, true));
                        aiPriority84.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_ALERT_LEVEL, 2, 0));
                        AIPriority aiPriority85 = new AIPriority(AIPriorityType.E_PROGRAM, 14, 2);
                        dataInv.Priorities.Add(aiPriority85);
                        aiPriority85.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 7, 0, true, 20));
                        aiPriority85.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 6, 0));
                        AIPriority aiPriority86 = new AIPriority(AIPriorityType.E_PROGRAM, 13, 2);
                        dataInv.Priorities.Add(aiPriority86);
                        aiPriority86.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 72, 0, true, 3));
                        aiPriority86.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_ALERT_LEVEL, 0, 0, true));
                        AIPriority aiPriority87 = new AIPriority(AIPriorityType.E_MAIN, 11, 0);
                        dataInv.Priorities.Add(aiPriority87);
                        aiPriority87.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20, 0));
                        aiPriority87.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 52, 0));
                        aiPriority87.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 13, inInverted: true, inData: 55));
                        AIPriority aiPriority88 = new AIPriority(AIPriorityType.E_CLASS_MAIN, 10, 0);
                        dataInv.Priorities.Add(aiPriority88);
                        aiPriority88.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20, 0));
                        aiPriority88.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 19, 4, true));
                        AIPriority aiPriority89 = new AIPriority(AIPriorityType.E_TWEAK, 8, 0);
                        dataInv.Priorities.Add(aiPriority89);
                        AIPriority aiPriority90 = new AIPriority(AIPriorityType.E_PROGRAM, 7, 0);
                        dataInv.Priorities.Add(aiPriority90);
                        aiPriority90.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_ALERT_LEVEL, 2, 0));
                        aiPriority90.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 32, 4, inData: 1));
                        aiPriority90.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 32, 2, inData: 0));
                        AIPriority aiPriority91 = new AIPriority(AIPriorityType.E_PROGRAM, 6, 2);
                        dataInv.Priorities.Add(aiPriority91);
                        aiPriority91.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_ALERT_LEVEL, 2, 0));
                        aiPriority91.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 24, 0, true, 20));
                        AIPriority aiPriority92 = new AIPriority(AIPriorityType.E_CLASS_MAIN, 6, 0);
                        dataInv.Priorities.Add(aiPriority92);
                        aiPriority92.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20));
                        aiPriority92.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 26, 0, true));
                        aiPriority92.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 0, 5, inData: 15));
                        aiPriority92.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 7, 0, inData: 30));
                        aiPriority92.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 64));
                        AIPriority aiPriority93 = new AIPriority(AIPriorityType.E_MAIN, 57, 3);
                        aiPriority92.Subpriorities.Add(aiPriority93);
                        aiPriority93.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20, 0, inData: 0));
                        AIPriority aiPriority94 = new AIPriority(AIPriorityType.E_MAIN, 12, 2);
                        aiPriority92.Subpriorities.Add(aiPriority94);
                        aiPriority94.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 54, 0, inData: 5));
                        AIPriority aiPriority95 = new AIPriority(AIPriorityType.E_MAIN, 54, 1);
                        aiPriority92.Subpriorities.Add(aiPriority95);
                        aiPriority95.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 51, 5));
                        AIPriority aiPriority96 = new AIPriority(AIPriorityType.E_PROGRAM, 5, 2);
                        dataInv.Priorities.Add(aiPriority96);
                        aiPriority96.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_ALERT_LEVEL, 0, 0, true));
                        aiPriority96.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 0, 0, true));
                        aiPriority96.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 10, 0));
                        AIPriority aiPriority97 = new AIPriority(AIPriorityType.E_MAIN, 5, 0);
                        dataInv.Priorities.Add(aiPriority97);
                        aiPriority97.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20, 0));
                        aiPriority97.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 21, 0, true));
                        aiPriority97.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 0, 5, inData: 15));
                        aiPriority97.Metadata.Add((PLPriorityMetadata)new PLPriorityMetadata_Float(3f, inName: "Range (m)"));
                        AIPriority aiPriority98 = new AIPriority(AIPriorityType.E_PROGRAM, 4, 3);
                        dataInv.Priorities.Add(aiPriority98);
                        aiPriority98.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 6, 0));
                        aiPriority98.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 1, 0, true));
                        AIPriority aiPriority99 = new AIPriority(AIPriorityType.E_MAIN, 2, 0);
                        dataInv.Priorities.Add(aiPriority99);
                        aiPriority99.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 52, 0));
                        aiPriority99.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 51, 5));
                        AIPriority aiPriority100 = new AIPriority(AIPriorityType.E_MAIN, 1, 0);
                        dataInv.Priorities.Add(aiPriority100);
                        aiPriority100.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 98, 0, true));
                        aiPriority100.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 54, 4, inData: 5));
                        aiPriority100.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20, 0, inData: 0));
                        aiPriority100.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 23, 5, inData: 2));
                        AIPriority aiPriority101 = new AIPriority(AIPriorityType.E_MAIN, 0, 0);
                        dataInv.Priorities.Add(aiPriority101);
                        aiPriority101.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 66, 0, true));
                        aiPriority101.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20, 0, inData: 0));
                        aiPriority101.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 54, 0, inData: 5));
                        aiPriority101.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 55, 4, inData: 100));
                        aiPriority101.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 86, 0, true, 25));
                        aiPriority101.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 24, 4));
                        break;
                    case 3:
                        AIPriority aiPriority102 = new AIPriority(AIPriorityType.E_TWEAK, 82, 3);
                        dataInv.Priorities.Add(aiPriority102);
                        AIPriority aiPriority103 = new AIPriority(AIPriorityType.E_TWEAK, 9, 3);
                        dataInv.Priorities.Add(aiPriority103);
                        aiPriority103.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 68, 5, inData: 0));
                        aiPriority103.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 75, 4, inData: 125));
                        aiPriority103.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 75, 2, true, 80));
                        AIPriority aiPriority104 = new AIPriority(AIPriorityType.E_MISSILE, 12, 2);
                        dataInv.Priorities.Add(aiPriority104);
                        AIPriority aiPriority105 = new AIPriority(AIPriorityType.E_MISSILE, 10, 2);
                        dataInv.Priorities.Add(aiPriority105);
                        aiPriority105.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 15, 1, true, 20));
                        aiPriority105.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 43, 4));
                        aiPriority105.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 41, 4));
                        AIPriority aiPriority106 = new AIPriority(AIPriorityType.E_MISSILE, 8, 2);
                        dataInv.Priorities.Add(aiPriority106);
                        aiPriority106.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 15, 1, true, 20));
                        aiPriority106.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 40, inInverted: true, inData: 10));
                        AIPriority aiPriority107 = new AIPriority(AIPriorityType.E_MISSILE, 7, 2);
                        dataInv.Priorities.Add(aiPriority107);
                        aiPriority107.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 15, 1, true, 20));
                        aiPriority107.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 40, inInverted: true, inData: 10));
                        AIPriority aiPriority108 = new AIPriority(AIPriorityType.E_MAIN, 4, 2);
                        dataInv.Priorities.Add(aiPriority108);
                        aiPriority108.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 0, 0, inData: 15));
                        aiPriority108.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20, 2, true, 0));
                        aiPriority108.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 56, 5, inData: 10));
                        aiPriority108.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 13, 5, true, 20));
                        aiPriority108.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 7, 5, inData: 30));
                        AIPriority aiPriority109 = new AIPriority(AIPriorityType.E_CLASS_MAIN, 40, 5);
                        aiPriority108.Subpriorities.Add(aiPriority109);
                        aiPriority109.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 40, 0, true, 10));
                        aiPriority109.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 7, 4, inData: 20));
                        AIPriority aiPriority110 = new AIPriority(AIPriorityType.E_CLASS_MAIN, 43, 4);
                        aiPriority108.Subpriorities.Add(aiPriority110);
                        aiPriority110.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 43, 0, true, 10));
                        aiPriority110.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 7, 5, inData: 20));
                        AIPriority aiPriority111 = new AIPriority(AIPriorityType.E_CLASS_MAIN, 42, 3);
                        aiPriority108.Subpriorities.Add(aiPriority111);
                        aiPriority111.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 41, 0, true, 10));
                        AIPriority aiPriority112 = new AIPriority(AIPriorityType.E_CLASS_MAIN, 41, 2);
                        aiPriority108.Subpriorities.Add(aiPriority112);
                        aiPriority112.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 48, 0));
                        AIPriority aiPriority113 = new AIPriority(AIPriorityType.E_CLASS_MAIN, 44, 1);
                        aiPriority108.Subpriorities.Add(aiPriority113);
                        AIPriority aiPriority114 = new AIPriority(AIPriorityType.E_MISSILE, 2, 2);
                        dataInv.Priorities.Add(aiPriority114);
                        aiPriority114.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 15, 1, inData: 20));
                        aiPriority114.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 15, 5, true));
                        AIPriority aiPriority115 = new AIPriority(AIPriorityType.E_MISSILE, 9, 1);
                        dataInv.Priorities.Add(aiPriority115);
                        aiPriority115.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 15, 1, true, 20));
                        aiPriority115.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 43, 4));
                        aiPriority115.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 41, 4));
                        AIPriority aiPriority116 = new AIPriority(AIPriorityType.E_MISSILE, 1, 1);
                        dataInv.Priorities.Add(aiPriority116);
                        aiPriority116.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 40, 2, true, 10));
                        aiPriority116.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 40, inInverted: true, inData: 30));
                        aiPriority116.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 40, 4, true, 80));
                        AIPriority aiPriority117 = new AIPriority(AIPriorityType.E_MISSILE, 0, 1);
                        dataInv.Priorities.Add(aiPriority117);
                        aiPriority117.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 40, 4));
                        AIPriority aiPriority118 = new AIPriority(AIPriorityType.E_CLASS_MAIN, 68, 0);
                        dataInv.Priorities.Add(aiPriority118);
                        aiPriority118.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_ALERT_LEVEL, 0, 0, true));
                        aiPriority118.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 6, 0));
                        aiPriority118.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 50, 0, inData: 4));
                        aiPriority118.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20, 0));
                        aiPriority118.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 73, 0, true));
                        aiPriority118.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 82, 0, true));
                        aiPriority118.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 0, 0, inData: 15));
                        aiPriority118.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 35, 5));
                        aiPriority118.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 15, 0, true, 40));
                        aiPriority118.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 15, 0, inData: 10));
                        aiPriority118.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 86, 4, inData: 70));
                        AIPriority aiPriority119 = new AIPriority(AIPriorityType.E_CLASS_MAIN, 67, 0);
                        dataInv.Priorities.Add(aiPriority119);
                        AIPriority aiPriority120 = new AIPriority(AIPriorityType.E_MAIN, 66, 3);
                        dataInv.Priorities.Add(aiPriority120);
                        aiPriority120.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 69, 0, true));
                        aiPriority120.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 70, 0, true));
                        AIPriority aiPriority121 = new AIPriority(AIPriorityType.E_MAIN, 65, 0);
                        dataInv.Priorities.Add(aiPriority121);
                        AIPriority aiPriority122 = new AIPriority(AIPriorityType.E_MAIN, 64, 0);
                        dataInv.Priorities.Add(aiPriority122);
                        aiPriority122.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 67, 4));
                        AIPriority aiPriority123 = new AIPriority(AIPriorityType.E_MAIN, 63, 3);
                        dataInv.Priorities.Add(aiPriority123);
                        aiPriority123.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 65, 0, true));
                        aiPriority123.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 66, 0, true));
                        aiPriority123.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 69, 0));
                        AIPriority aiPriority124 = new AIPriority(AIPriorityType.E_MAIN, 60, 0);
                        dataInv.Priorities.Add(aiPriority124);
                        AIPriority aiPriority125 = new AIPriority(AIPriorityType.E_MAIN, 58, 0);
                        dataInv.Priorities.Add(aiPriority125);
                        aiPriority125.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20, 0));
                        aiPriority125.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 57, 0, true));
                        aiPriority125.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 56, 5, inData: 80));
                        AIPriority aiPriority126 = new AIPriority(AIPriorityType.E_MAIN, 49, 0);
                        dataInv.Priorities.Add(aiPriority126);
                        aiPriority126.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 52, 0, true));
                        aiPriority126.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 51, 5));
                        aiPriority126.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 12, 4, inData: 2));
                        AIPriority aiPriority127 = new AIPriority(AIPriorityType.E_MAIN, 11, 0);
                        dataInv.Priorities.Add(aiPriority127);
                        aiPriority127.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20, 0));
                        aiPriority127.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 52, 0));
                        aiPriority127.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 13, inInverted: true, inData: 55));
                        AIPriority aiPriority128 = new AIPriority(AIPriorityType.E_CLASS_MAIN, 10, 0);
                        dataInv.Priorities.Add(aiPriority128);
                        aiPriority128.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 17, 4, true));
                        AIPriority aiPriority129 = new AIPriority(AIPriorityType.E_TWEAK, 8, 0);
                        dataInv.Priorities.Add(aiPriority129);
                        AIPriority aiPriority130 = new AIPriority(AIPriorityType.E_MISSILE, 6, 0);
                        dataInv.Priorities.Add(aiPriority130);
                        aiPriority130.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 40, inInverted: true, inData: 10));
                        AIPriority aiPriority131 = new AIPriority(AIPriorityType.E_CLASS_MAIN, 6, 0);
                        dataInv.Priorities.Add(aiPriority131);
                        aiPriority131.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20));
                        aiPriority131.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 26, 0, true));
                        aiPriority131.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 0, 5, inData: 15));
                        aiPriority131.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 7, 0, inData: 10));
                        aiPriority131.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 64));
                        aiPriority131.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 40, 0, true, 99));
                        aiPriority131.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 41, 0, true, 99));
                        aiPriority131.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 42, 0, true, 99));
                        aiPriority131.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 43, 0, true, 99));
                        aiPriority131.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 6, 0));
                        aiPriority131.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 86, inData: 75));
                        AIPriority aiPriority132 = new AIPriority(AIPriorityType.E_MAIN, 57, 3);
                        aiPriority131.Subpriorities.Add(aiPriority132);
                        aiPriority132.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20, 0, inData: 0));
                        AIPriority aiPriority133 = new AIPriority(AIPriorityType.E_MAIN, 12, 2);
                        aiPriority131.Subpriorities.Add(aiPriority133);
                        aiPriority133.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 54, 0, inData: 5));
                        AIPriority aiPriority134 = new AIPriority(AIPriorityType.E_MAIN, 54, 1);
                        aiPriority131.Subpriorities.Add(aiPriority134);
                        aiPriority134.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 51, 5));
                        AIPriority aiPriority135 = new AIPriority(AIPriorityType.E_MAIN, 5, 0);
                        dataInv.Priorities.Add(aiPriority135);
                        aiPriority135.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20, 0));
                        aiPriority135.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 21, 0, true));
                        aiPriority135.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 0, 5, inData: 15));
                        aiPriority135.Metadata.Add((PLPriorityMetadata)new PLPriorityMetadata_Float(3f, inName: "Range (m)"));
                        AIPriority aiPriority136 = new AIPriority(AIPriorityType.E_MAIN, 2, 0);
                        dataInv.Priorities.Add(aiPriority136);
                        aiPriority136.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 52, 0));
                        aiPriority136.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 51, 5));
                        aiPriority136.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 25, inData: 1));
                        aiPriority136.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20, 0, true));
                        aiPriority136.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 54, 0, inData: 5));
                        aiPriority136.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 0, 0, inData: 15));
                        aiPriority136.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 55, 4, true));
                        AIPriority aiPriority137 = new AIPriority(AIPriorityType.E_MAIN, 1, 0);
                        dataInv.Priorities.Add(aiPriority137);
                        aiPriority137.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 98, 0, true));
                        aiPriority137.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 54, 4, inData: 5));
                        AIPriority aiPriority138 = new AIPriority(AIPriorityType.E_MAIN, 0, 0);
                        dataInv.Priorities.Add(aiPriority138);
                        aiPriority138.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 66, 0, true));
                        aiPriority138.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20, 0, inData: 0));
                        aiPriority138.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 54, 0, inData: 5));
                        aiPriority138.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 86, 0, true, 15));
                        aiPriority138.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 47, inInverted: true, inData: 80));
                        break;
                    case 4:
                        AIPriority aiPriority139 = new AIPriority(AIPriorityType.E_TWEAK, 14, 5);
                        dataInv.Priorities.Add(aiPriority139);
                        aiPriority139.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 16, 5, inData: 1));
                        aiPriority139.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 2, 5, true, 25));
                        aiPriority139.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_ALERT_LEVEL, 0, 5, true));
                        aiPriority139.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 90, 0));
                        aiPriority139.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 77, 0, true, 40));
                        aiPriority139.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 77, 1, true, 45));
                        aiPriority139.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 77, 2, true));
                        aiPriority139.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 77, inInverted: true, inData: 55));
                        aiPriority139.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 23, 2, inData: 0));
                        aiPriority139.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 24, 2, inData: 10));
                        aiPriority139.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 91, 4, true));
                        aiPriority139.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 2, 1, inData: 85));
                        aiPriority139.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 2, 2, inData: 80));
                        aiPriority139.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 2, inData: 75));
                        aiPriority139.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 2, 4, inData: 70));
                        AIPriority aiPriority140 = new AIPriority(AIPriorityType.E_TWEAK, 82, 3);
                        dataInv.Priorities.Add(aiPriority140);
                        AIPriority aiPriority141 = new AIPriority(AIPriorityType.E_TWEAK, 48, 2);
                        dataInv.Priorities.Add(aiPriority141);
                        aiPriority141.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 1, 1, inData: 20));
                        aiPriority141.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 1, 2, inData: 40));
                        aiPriority141.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 92, 5, true));
                        aiPriority141.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 64, 4));
                        aiPriority141.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 63, 5));
                        aiPriority141.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 5, 5));
                        aiPriority141.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 27, inInverted: true, inData: 0));
                        aiPriority141.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 1, 5, true, 80));
                        aiPriority141.Metadata.Add((PLPriorityMetadata)new PLPriorityMetadata_Float(15f, inName: "Player Override Time"));
                        AIPriority aiPriority142 = new AIPriority(AIPriorityType.E_TWEAK, 46, 3);
                        dataInv.Priorities.Add(aiPriority142);
                        aiPriority142.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 68, 5, inData: 0));
                        aiPriority142.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 5, 5));
                        aiPriority142.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 58, 5));
                        aiPriority142.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 1, 4));
                        aiPriority142.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 50, 4, inData: 2));
                        aiPriority142.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 64, 1));
                        aiPriority142.Metadata.Add((PLPriorityMetadata)new PLPriorityMetadata_Float(15f, inName: "Player Override Time"));
                        AIPriority aiPriority143 = new AIPriority(AIPriorityType.E_TWEAK, 47, 5);
                        dataInv.Priorities.Add(aiPriority143);
                        aiPriority143.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 49, 0));
                        aiPriority143.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 64, 2));
                        aiPriority143.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 7, 5, inData: 30));
                        aiPriority143.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 1, 2, true, 80));
                        aiPriority143.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 1, inInverted: true, inData: 60));
                        aiPriority143.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 1, 4, true, 40));
                        aiPriority143.Metadata.Add((PLPriorityMetadata)new PLPriorityMetadata_Float(15f, inName: "Player Override Time"));
                        AIPriority aiPriority144 = new AIPriority(AIPriorityType.E_TWEAK, 45, 4);
                        dataInv.Priorities.Add(aiPriority144);
                        aiPriority144.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 64, 1));
                        aiPriority144.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 6, 2));
                        aiPriority144.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 92, 1, true));
                        aiPriority144.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 1, 1, inData: 20));
                        aiPriority144.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 1, 2, inData: 40));
                        aiPriority144.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 1, inData: 70));
                        aiPriority144.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 50, 5, inData: 12));
                        aiPriority144.Metadata.Add((PLPriorityMetadata)new PLPriorityMetadata_Float(15f, inName: "Player Override Time"));
                        AIPriority aiPriority145 = new AIPriority(AIPriorityType.E_CLASS_MAIN, 3, 2);
                        dataInv.Priorities.Add(aiPriority145);
                        aiPriority145.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 0, 0, inData: 15));
                        aiPriority145.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 90, 4));
                        aiPriority145.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 2, 4, inData: 90));
                        aiPriority145.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20, 2, true));
                        aiPriority145.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 56, 5, inData: 10));
                        aiPriority145.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 13, 5, true));
                        aiPriority145.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 50, 5, inData: 12));
                        aiPriority145.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 7, 5, inData: 30));
                        AIPriority aiPriority146 = new AIPriority(AIPriorityType.E_CLASS_MAIN, 50, 2);
                        aiPriority145.Subpriorities.Add(aiPriority146);
                        AIPriority aiPriority147 = new AIPriority(AIPriorityType.E_CLASS_MAIN, 59, 0);
                        aiPriority145.Subpriorities.Add(aiPriority147);
                        aiPriority147.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20, 0));
                        aiPriority147.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 10, 0, true));
                        aiPriority147.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 0, 5, inData: 30));
                        AIPriority aiPriority148 = new AIPriority(AIPriorityType.E_CLASS_MAIN, 30, 0);
                        aiPriority145.Subpriorities.Add(aiPriority148);
                        aiPriority148.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 5, 0));
                        aiPriority148.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 10, 0));
                        aiPriority148.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 0, 4));
                        AIPriority aiPriority149 = new AIPriority(AIPriorityType.E_TWEAK, 80, 1);
                        dataInv.Priorities.Add(aiPriority149);
                        aiPriority149.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 90, 0));
                        aiPriority149.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 64, 0));
                        aiPriority149.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 96, 0, true));
                        AIPriority aiPriority150 = new AIPriority(AIPriorityType.E_TWEAK, 78, 1);
                        dataInv.Priorities.Add(aiPriority150);
                        aiPriority150.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 96, 0, true));
                        AIPriority aiPriority151 = new AIPriority(AIPriorityType.E_TWEAK, 77, 1);
                        dataInv.Priorities.Add(aiPriority151);
                        aiPriority151.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 64, 0));
                        aiPriority151.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 96, 0, true));
                        AIPriority aiPriority152 = new AIPriority(AIPriorityType.E_TWEAK, 76, 1);
                        dataInv.Priorities.Add(aiPriority152);
                        aiPriority152.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 90, 0));
                        aiPriority152.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 96, 0, true));
                        aiPriority152.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 16, 0, inData: 20));
                        AIPriority aiPriority153 = new AIPriority(AIPriorityType.E_TWEAK, 75, 1);
                        dataInv.Priorities.Add(aiPriority153);
                        aiPriority153.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 90, 0));
                        aiPriority153.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 64, 0));
                        aiPriority153.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 96, 0, true));
                        AIPriority aiPriority154 = new AIPriority(AIPriorityType.E_TWEAK, 74, 0);
                        dataInv.Priorities.Add(aiPriority154);
                        aiPriority154.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 90, 0));
                        aiPriority154.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 99, 0));
                        aiPriority154.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 23, 0, inData: 0));
                        aiPriority154.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 42, 0, true, 0));
                        aiPriority154.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 4, 0, true));
                        aiPriority154.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 4, 1, inData: -4));
                        aiPriority154.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 4, 1, true, 1));
                        AIPriority aiPriority155 = new AIPriority(AIPriorityType.E_TWEAK, 73, 1);
                        dataInv.Priorities.Add(aiPriority155);
                        aiPriority155.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 90, 0));
                        aiPriority155.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 96, 0, true));
                        AIPriority aiPriority156 = new AIPriority(AIPriorityType.E_MAIN, 4, 1);
                        dataInv.Priorities.Add(aiPriority156);
                        aiPriority156.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20, 0, inData: 0));
                        AIPriority aiPriority157 = new AIPriority(AIPriorityType.E_CLASS_MAIN, 81, 0);
                        dataInv.Priorities.Add(aiPriority157);
                        aiPriority157.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 97, 0));
                        aiPriority157.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 95, 5));
                        AIPriority aiPriority158 = new AIPriority(AIPriorityType.E_TWEAK, 79, 1);
                        dataInv.Priorities.Add(aiPriority158);
                        aiPriority158.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 90, 0));
                        aiPriority158.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 93, 0));
                        aiPriority158.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 64, 0));
                        aiPriority158.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 96, 0, true));
                        AIPriority aiPriority159 = new AIPriority(AIPriorityType.E_TWEAK, 71, 5);
                        dataInv.Priorities.Add(aiPriority159);
                        aiPriority159.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 96, 5, true));
                        aiPriority159.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_ALERT_LEVEL, 0, 0, true));
                        aiPriority159.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 16, 0, inData: 20));
                        aiPriority159.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 24, 0, inData: 10));
                        aiPriority159.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 23, 0, inData: 0));
                        aiPriority159.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 3, 0, true, 85));
                        aiPriority159.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 77, 0, true));
                        aiPriority159.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 91, 5, true));
                        aiPriority159.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 2, 0, inData: 80));
                        AIPriority aiPriority160 = new AIPriority(AIPriorityType.E_CLASS_MAIN, 67, 3);
                        dataInv.Priorities.Add(aiPriority160);
                        aiPriority160.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_ALERT_LEVEL, 2, 0));
                        aiPriority160.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20, 0));
                        aiPriority160.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 72, 0, true, 6));
                        aiPriority160.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 71, 0, true, 2));
                        aiPriority160.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 0, 0, inData: 15));
                        aiPriority160.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 86, 0, true, 15));
                        aiPriority160.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 89, 0, true, 15));
                        aiPriority160.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 32, inData: 1));
                        aiPriority160.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 89, 0, true, 45));
                        AIPriority aiPriority161 = new AIPriority(AIPriorityType.E_MAIN, 66, 3);
                        dataInv.Priorities.Add(aiPriority161);
                        aiPriority161.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 69, 0, true));
                        aiPriority161.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 70, 0, true));
                        AIPriority aiPriority162 = new AIPriority(AIPriorityType.E_MAIN, 65, 0);
                        dataInv.Priorities.Add(aiPriority162);
                        AIPriority aiPriority163 = new AIPriority(AIPriorityType.E_MAIN, 64, 0);
                        dataInv.Priorities.Add(aiPriority163);
                        aiPriority163.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 67, 4));
                        AIPriority aiPriority164 = new AIPriority(AIPriorityType.E_MAIN, 63, 3);
                        dataInv.Priorities.Add(aiPriority164);
                        aiPriority164.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 65, 0, true));
                        aiPriority164.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 66, 0, true));
                        aiPriority164.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 69, 0));
                        AIPriority aiPriority165 = new AIPriority(AIPriorityType.E_MAIN, 60, 0);
                        dataInv.Priorities.Add(aiPriority165);
                        AIPriority aiPriority166 = new AIPriority(AIPriorityType.E_MAIN, 58, 0);
                        dataInv.Priorities.Add(aiPriority166);
                        aiPriority166.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20, 0));
                        aiPriority166.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 57, 0, true));
                        aiPriority166.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 56, inData: 80));
                        AIPriority aiPriority167 = new AIPriority(AIPriorityType.E_MAIN, 49, 0);
                        dataInv.Priorities.Add(aiPriority167);
                        aiPriority167.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 52, 0, true));
                        aiPriority167.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 51, 5));
                        aiPriority167.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 12, 4, inData: 2));
                        AIPriority aiPriority168 = new AIPriority(AIPriorityType.E_CLASS_MAIN, 31, 0);
                        dataInv.Priorities.Add(aiPriority168);
                        AIPriority aiPriority169 = new AIPriority(AIPriorityType.E_TWEAK, 17, 0);
                        dataInv.Priorities.Add(aiPriority169);
                        aiPriority169.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 77, 5, true, 10));
                        aiPriority169.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 77, inInverted: true, inData: 20));
                        aiPriority169.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 77, 2, true, 25));
                        aiPriority169.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 32, inData: 0));
                        aiPriority169.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 24, inData: 20));
                        aiPriority169.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 2, 0, true));
                        aiPriority169.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 91, 0, true));
                        aiPriority169.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 2, 5, inData: 98));
                        aiPriority169.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 2, inData: 95));
                        aiPriority169.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 2, 2, inData: 90));
                        AIPriority aiPriority170 = new AIPriority(AIPriorityType.E_TWEAK, 13, 0);
                        dataInv.Priorities.Add(aiPriority170);
                        aiPriority170.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 77, 5, true, 10));
                        aiPriority170.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 77, inInverted: true, inData: 20));
                        aiPriority170.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 77, 2, true, 25));
                        aiPriority170.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 91, 0, true));
                        aiPriority170.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 2, 5, inData: 98));
                        aiPriority170.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 2, inData: 95));
                        aiPriority170.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 2, 2, inData: 90));
                        aiPriority170.Metadata.Add((PLPriorityMetadata)new PLPriorityMetadata_Float(15f, inName: "Player Override Time"));
                        AIPriority aiPriority171 = new AIPriority(AIPriorityType.E_MAIN, 11, 0);
                        dataInv.Priorities.Add(aiPriority171);
                        aiPriority171.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20, 0));
                        aiPriority171.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 52, 0));
                        aiPriority171.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 13, inInverted: true, inData: 55));
                        AIPriority aiPriority172 = new AIPriority(AIPriorityType.E_CLASS_MAIN, 10, 0);
                        dataInv.Priorities.Add(aiPriority172);
                        aiPriority172.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20, 0));
                        aiPriority172.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 19, 4, true));
                        AIPriority aiPriority173 = new AIPriority(AIPriorityType.E_TWEAK, 8, 0);
                        dataInv.Priorities.Add(aiPriority173);
                        AIPriority aiPriority174 = new AIPriority(AIPriorityType.E_CLASS_MAIN, 6, 0);
                        dataInv.Priorities.Add(aiPriority174);
                        aiPriority174.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20));
                        aiPriority174.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 26, 0, true));
                        aiPriority174.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 0, 5, inData: 15));
                        aiPriority174.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 50, 0, inData: 12));
                        aiPriority174.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 7, 0, inData: 30));
                        aiPriority174.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 64));
                        AIPriority aiPriority175 = new AIPriority(AIPriorityType.E_MAIN, 57, 3);
                        aiPriority174.Subpriorities.Add(aiPriority175);
                        aiPriority175.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20, 0, inData: 0));
                        AIPriority aiPriority176 = new AIPriority(AIPriorityType.E_MAIN, 12, 2);
                        aiPriority174.Subpriorities.Add(aiPriority176);
                        aiPriority176.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 54, 0, inData: 5));
                        AIPriority aiPriority177 = new AIPriority(AIPriorityType.E_MAIN, 54, 1);
                        aiPriority174.Subpriorities.Add(aiPriority177);
                        aiPriority177.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 51, 5));
                        AIPriority aiPriority178 = new AIPriority(AIPriorityType.E_MAIN, 5, 0);
                        dataInv.Priorities.Add(aiPriority178);
                        aiPriority178.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20, 0));
                        aiPriority178.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 21, 0, true));
                        aiPriority178.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 0, 5, inData: 15));
                        aiPriority178.Metadata.Add((PLPriorityMetadata)new PLPriorityMetadata_Float(3f, inName: "Range (m)"));
                        AIPriority aiPriority179 = new AIPriority(AIPriorityType.E_MAIN, 2, 0);
                        dataInv.Priorities.Add(aiPriority179);
                        aiPriority179.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 52, 0));
                        aiPriority179.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 51, 5));
                        AIPriority aiPriority180 = new AIPriority(AIPriorityType.E_MAIN, 1, 0);
                        dataInv.Priorities.Add(aiPriority180);
                        aiPriority180.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 98, 0, true));
                        aiPriority180.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 54, 4, inData: 5));
                        aiPriority180.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20, 0, inData: 0));
                        aiPriority180.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 2, 0, inData: 90));
                        aiPriority180.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 86, 0, true, 30));
                        aiPriority180.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 23, inData: 0));
                        AIPriority aiPriority181 = new AIPriority(AIPriorityType.E_MAIN, 0, 0);
                        dataInv.Priorities.Add(aiPriority181);
                        aiPriority181.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 66, 0, true));
                        aiPriority181.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20, 0, inData: 0));
                        aiPriority181.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 54, 0, inData: 5));
                        aiPriority181.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 2, 0, inData: 90));
                        aiPriority181.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 55, 4, inData: 100));
                        aiPriority181.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 86, 0, true, 40));
                        aiPriority181.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 24, 4, inData: 10));
                        break;
                    default:
                        AIPriority aiPriority182 = new AIPriority(AIPriorityType.E_TWEAK, 82, 3);
                        dataInv.Priorities.Add(aiPriority182);
                        AIPriority aiPriority183 = new AIPriority(AIPriorityType.E_MAIN, 4, 2);
                        dataInv.Priorities.Add(aiPriority183);
                        aiPriority183.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 0, 0, inData: 15));
                        aiPriority183.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20, 2, true, 0));
                        aiPriority183.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 56, 5, inData: 10));
                        aiPriority183.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 13, 5, true, 20));
                        aiPriority183.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 7, 5, inData: 30));
                        AIPriority aiPriority184 = new AIPriority(AIPriorityType.E_CLASS_MAIN, 67, 3);
                        dataInv.Priorities.Add(aiPriority184);
                        aiPriority184.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_ALERT_LEVEL, 2, 0));
                        aiPriority184.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20, 0));
                        aiPriority184.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 72, 0, true, 6));
                        aiPriority184.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 71, 0, true, 2));
                        aiPriority184.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 0, 0, inData: 15));
                        aiPriority184.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 89, 0, true, 15));
                        aiPriority184.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 32, inData: 1));
                        aiPriority184.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 89, 0, true, 45));
                        AIPriority aiPriority185 = new AIPriority(AIPriorityType.E_MAIN, 66, 3);
                        dataInv.Priorities.Add(aiPriority185);
                        aiPriority185.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 69, 0, true));
                        aiPriority185.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 70, 0, true));
                        AIPriority aiPriority186 = new AIPriority(AIPriorityType.E_MAIN, 65, 0);
                        dataInv.Priorities.Add(aiPriority186);
                        AIPriority aiPriority187 = new AIPriority(AIPriorityType.E_MAIN, 64, 0);
                        dataInv.Priorities.Add(aiPriority187);
                        aiPriority187.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 67, 4));
                        AIPriority aiPriority188 = new AIPriority(AIPriorityType.E_MAIN, 63, 3);
                        dataInv.Priorities.Add(aiPriority188);
                        aiPriority188.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 65, 0, true));
                        aiPriority188.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 66, 0, true));
                        aiPriority188.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 69, 0));
                        AIPriority aiPriority189 = new AIPriority(AIPriorityType.E_MAIN, 60, 0);
                        dataInv.Priorities.Add(aiPriority189);
                        AIPriority aiPriority190 = new AIPriority(AIPriorityType.E_MAIN, 58, 0);
                        dataInv.Priorities.Add(aiPriority190);
                        aiPriority190.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20, 0));
                        aiPriority190.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 57, 0, true));
                        aiPriority190.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 56, inData: 80));
                        AIPriority aiPriority191 = new AIPriority(AIPriorityType.E_MAIN, 49, 0);
                        dataInv.Priorities.Add(aiPriority191);
                        aiPriority191.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 52, 0, true));
                        aiPriority191.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 51, 5));
                        aiPriority191.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 12, 4, inData: 2));
                        AIPriority aiPriority192 = new AIPriority(AIPriorityType.E_MAIN, 11, 0);
                        dataInv.Priorities.Add(aiPriority192);
                        aiPriority192.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20, 0));
                        aiPriority192.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 52, 0));
                        aiPriority192.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 13, inInverted: true, inData: 55));
                        AIPriority aiPriority193 = new AIPriority(AIPriorityType.E_CLASS_MAIN, 10, 0);
                        dataInv.Priorities.Add(aiPriority193);
                        aiPriority193.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20, 0));
                        aiPriority193.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 19, 4, true));
                        AIPriority aiPriority194 = new AIPriority(AIPriorityType.E_TWEAK, 8, 0);
                        dataInv.Priorities.Add(aiPriority194);
                        AIPriority aiPriority195 = new AIPriority(AIPriorityType.E_CLASS_MAIN, 6, 0);
                        dataInv.Priorities.Add(aiPriority195);
                        aiPriority195.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20));
                        aiPriority195.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 26, 0, true));
                        aiPriority195.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 0, 5, inData: 15));
                        aiPriority195.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 7, 0, inData: 10));
                        aiPriority195.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 64));
                        aiPriority195.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 40, 0, true, 99));
                        aiPriority195.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 41, 0, true, 99));
                        aiPriority195.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 42, 0, true, 99));
                        aiPriority195.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 43, 0, true, 99));
                        aiPriority195.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 6, 0));
                        aiPriority195.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 86));
                        AIPriority aiPriority196 = new AIPriority(AIPriorityType.E_MAIN, 57, 2);
                        aiPriority195.Subpriorities.Add(aiPriority196);
                        aiPriority196.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20, 0, inData: 0));
                        AIPriority aiPriority197 = new AIPriority(AIPriorityType.E_MAIN, 12, 1);
                        aiPriority195.Subpriorities.Add(aiPriority197);
                        AIPriority aiPriority198 = new AIPriority(AIPriorityType.E_MAIN, 54, 0);
                        aiPriority195.Subpriorities.Add(aiPriority198);
                        aiPriority198.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 51, 5));
                        AIPriority aiPriority199 = new AIPriority(AIPriorityType.E_MAIN, 5, 0);
                        dataInv.Priorities.Add(aiPriority199);
                        aiPriority199.Metadata.Add((PLPriorityMetadata)new PLPriorityMetadata_Float(3f, inName: "Range (m)"));
                        AIPriority aiPriority200 = new AIPriority(AIPriorityType.E_MAIN, 2, 0);
                        dataInv.Priorities.Add(aiPriority200);
                        aiPriority200.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 52, 0));
                        aiPriority200.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 51, 5));
                        aiPriority200.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 25, inData: 0));
                        aiPriority200.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20, 0, true));
                        aiPriority200.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 54, 0, inData: 5));
                        aiPriority200.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 0, 0, inData: 15));
                        aiPriority200.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 55, 4, true));
                        AIPriority aiPriority201 = new AIPriority(AIPriorityType.E_MAIN, 1, 0);
                        dataInv.Priorities.Add(aiPriority201);
                        aiPriority201.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 98, 0, true));
                        aiPriority201.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 54, 4, inData: 5));
                        aiPriority201.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20, 0, inData: 0));
                        aiPriority201.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 86, 0, true, 30));
                        aiPriority201.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 23, inData: 1));
                        AIPriority aiPriority202 = new AIPriority(AIPriorityType.E_MAIN, 0, 0);
                        dataInv.Priorities.Add(aiPriority202);
                        aiPriority202.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 66, 0, true));
                        aiPriority202.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20, 0, inData: 0));
                        aiPriority202.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 54, 0, inData: 5));
                        aiPriority202.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 55, 4, inData: 100));
                        aiPriority202.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 86, 0, true, 40));
                        aiPriority202.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 24, 4, inData: 20));
                        break;
                }
                return __exception;
            }
        }

        [HarmonyPatch(typeof(PLShipInfoBase), "Update")]
        internal class EnemyShipTargetFix
        {
            private static void Postfix(PLShipInfoBase __instance)
            {
                if (__instance.GetIsPlayerShip() || !((UnityEngine.Object)__instance.TargetShip_ForAI != (UnityEngine.Object)null))
                    return;
                __instance.CaptainTargetedSpaceTargetID = __instance.TargetShip_ForAI.ShipID;
            }
        }
    }
}
