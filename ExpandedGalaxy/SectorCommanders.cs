using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using UnityEngine;

namespace ExpandedGalaxy
{
    internal class SectorCommanders
    {
        [HarmonyPatch(typeof(PLShipInfoBase), "ShipFinalCalculateStats")]
        internal class AllSCStats
        {
            private static void Postfix(PLShipInfoBase __instance, ref PLShipStats inStats)
            {
                if (__instance is PLCorruptedDroneShipInfo)
                {
                    inStats.CyberDefenseRating += 2.5f + 0.2f * Mathf.Floor(PLServer.Instance.ChaosLevel / 2) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar;
                    inStats.CyberAttackRating += 0.5f + 0.1f * Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar;
                }
                else if (__instance is PLWarpGuardian)
                {
                    inStats.CyberDefenseRating += 1.5f + 0.1f * Mathf.Floor(PLServer.Instance.ChaosLevel) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar;
                    inStats.CyberAttackRating += 0f + 0.1f * Mathf.Floor(PLServer.Instance.ChaosLevel) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar;
                }
                else if (__instance is PLDeathseekerCommanderDrone)
                {
                    inStats.CyberDefenseRating += 3.0f + 0.1f * Mathf.Floor(PLServer.Instance.ChaosLevel) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar;
                }
                else if (__instance is PLAlchemistShipInfo && !__instance.GetIsPlayerShip())
                {
                    inStats.CyberDefenseRating = Mathf.Clamp(inStats.CyberDefenseRating, 1.5f, float.MaxValue);
                    inStats.CyberAttackRating += 2.0f;
                }
                else if (__instance is PLIntrepidCommanderInfo && !__instance.GetIsPlayerShip())
                {
                    inStats.CyberDefenseRating = Mathf.Clamp(inStats.CyberDefenseRating, 2f, float.MaxValue);
                    inStats.CyberAttackRating += 1.0f;
                }
            }
        }
        internal class AncientSentry
        {
            [HarmonyPatch(typeof(PLCorruptedDroneShipInfo), "SetupShipStats")]
            internal class MoreSentryComps
            {
                private static void Postfix(PLCorruptedDroneShipInfo __instance)
                {
                    if (PhotonNetwork.isMasterClient)
                    {
                        PLShipStats stats = __instance.MyStats;
                        for (int i = 0; i < 5; i++)
                        {
                            stats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_CPU, (int)ECPUClass.E_CPUTYPE_TURRETCHARGE_BACKUP, 5, 0, (int)ESlotType.E_COMP_CPU)));
                        }
                    }
                }
            }

            [HarmonyPatch(typeof(PLCorruptedDroneShipInfo), "Update")]
            internal class SentryRage
            {
                private static void Postfix(PLCorruptedDroneShipInfo __instance, ref Material ___CorruptedGreenMaterial, ref float ___Server_LastMissleFireTime, ref float ___Server_LastEMPBlastTime)
                {
                    if (__instance.AlertLevel > 0)
                    {
                        if (Time.time - ___Server_LastEMPBlastTime > 30f && PhotonNetwork.isMasterClient)
                        {
                            __instance.photonView.RPC("EMPBlast", PhotonTargets.All);
                            ___Server_LastEMPBlastTime = Time.time;
                        }
                        if (__instance.MyStats.HullCurrent / __instance.MyStats.HullMax < 0.3f)
                        {

                            foreach (Light light in __instance.GreenLights)
                            {
                                light.color = Color.red;
                                light.intensity = (float)Math.Sin((double)Time.time) * 25f;
                            }
                            if (PhotonNetwork.isMasterClient)
                            {
                                if ((double)Time.time - (double)___Server_LastMissleFireTime > 30.0)
                                    ___Server_LastMissleFireTime -= 30f;
                            }
                        }
                    }
                }
            }

            [HarmonyPatch(typeof(PLCorruptedDroneShipInfo), "EMPBlast")]
            internal class SentryEMP
            {
                private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
                {
                    List<CodeInstruction> list = instructions.ToList();

                    list[28].operand = 600f;
                    list[78].operand = 10000000f;
                    return list.AsEnumerable<CodeInstruction>();
                }
            }

            [HarmonyPatch(typeof(PLCorruptedDroneShipInfo), "Start")]
            internal class SentryStart
            {
                private static void Postfix(PLCorruptedDroneShipInfo __instance, ref float ___Server_LastEMPBlastTime)
                {
                    ___Server_LastEMPBlastTime = Time.time;
                }
            }

            [HarmonyPatch(typeof(PLDeathseekerCommanderDrone), "Update")]
            internal class DeathwardenDischarge
            {
                private static void Postfix(PLDeathseekerCommanderDrone __instance)
                {
                    if (__instance.HasBeenDestroyed)
                        return;
                    foreach (PLShipInfoBase plShipInfoBase in UnityEngine.Object.FindObjectsOfType(typeof(PLShipInfoBase)))
                    {
                        if ((UnityEngine.Object)plShipInfoBase != (UnityEngine.Object)__instance && (plShipInfoBase.GetCurrentSensorPosition() - __instance.GetCurrentSensorPosition()).magnitude < (8000f / 5f))
                            plShipInfoBase.DischargeAmount += 0.08f * Time.deltaTime;
                    }
                }
            }

            [HarmonyPatch(typeof(PLIntrepidCommanderInfo), "LeaveExtraScrap")]
            internal class CutlassDropCULong2
            {
                private static bool Prefix(PLIntrepidCommanderInfo __instance, ref List<PLShipComponent> droppedShipComponents)
                {
                    bool flag = false;
                    foreach (PLShipComponent plShipComponent in droppedShipComponents)
                    {
                        if (plShipComponent.ActualSlotType == ESlotType.E_COMP_MAINTURRET && plShipComponent.SubType == 1)
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (flag)
                        return false;
                    PLServer.Instance.photonView.RPC("CreateSpecificShipScrapAtLocation", PhotonTargets.All, (object)(__instance.Exterior.transform.position + UnityEngine.Random.onUnitSphere * 20f), (object)__instance.Exterior.transform.position, (object)(int)PLShipComponent.createHashFromInfo(11, 5, 0, 0, 12), (object)true);
                    return false;
                }
            }
        }
    }
}
