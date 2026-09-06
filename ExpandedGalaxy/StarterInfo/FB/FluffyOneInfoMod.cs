using HarmonyLib;
using PulsarModLoader.Content.Components.AutoTurret;
using PulsarModLoader.Content.Components.Turret;
using System;
using UnityEngine;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLFluffyShipInfo), "SetupShipStats")]
    internal class FluffyOneInfoMod
    {
        private static void Postfix(PLFluffyShipInfo __instance, bool previewStats, bool startingPlayerShip)
        {
            if (__instance == null || __instance.MyStats == null)
                return;
            try
            {
                if (!__instance.ShouldCreateDefaultComponents || !(PhotonNetwork.isMasterClient | previewStats))
                    return;
                if (startingPlayerShip || previewStats)
                {
                    return;
                }
                PLRand deterministicRand = PLShipInfoBase.GetShipDeterministicRand(__instance.PersistantShipInfo);
                __instance.MyStats.RemoveShipComponent((PLShipComponent)__instance.MyStats.GetShipComponent<PLHullPlating>(ESlotType.E_COMP_HULLPLATING));
                __instance.MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_HULLPLATING, (int)EHullPlatingType.E_HULLPLATING_CCGE, __instance.GetChaosBoost(deterministicRand.Next() % 50), 0, 12)), visualSlot: ESlotType.E_COMP_HULLPLATING);
                __instance.NumberOfFuelCapsules = 15;
                if ((double)(float)PLServer.Instance.ChaosLevel > 1.0)
                {
                    float num = Mathf.Clamp01((float)((double)deterministicRand.NextFloat() + (double)(float)PLServer.Instance.ChaosLevel * 0.01999999731779099));
                    if (num >= 0.85)
                    {
                        __instance.MyStats.RemoveShipComponent(__instance.MyStats.GetComponentsOfType(ESlotType.E_COMP_TURRET)[deterministicRand.Next() % 2]);
                        bool flag = (double)(float)PLServer.Instance.ChaosLevel > 2.0 && (int)deterministicRand.Next() % 3 == 0;
                        if (flag)
                            __instance.MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_TURRET, TurretModManager.Instance.GetTurretIDFromName("Particle Lance"), __instance.GetChaosBoost(deterministicRand.Next() % 50), 0, 12)), visualSlot: ESlotType.E_COMP_TURRET);
                        else
                            __instance.MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_TURRET, TurretModManager.Instance.GetTurretIDFromName("Sylvassi Turret"), __instance.GetChaosBoost(deterministicRand.Next() % 50), 0, 12)), visualSlot: ESlotType.E_COMP_TURRET);
                    }
                }
            }
            catch { }
        }

        private static Exception Finalizer(Exception __exception, PLFluffyShipInfo __instance, bool previewStats, bool startingPlayerShip)
        {
            if (__instance.MyStats.GetSlot(ESlotType.E_COMP_AUTO_TURRET).MaxItems > 0 || (__instance is PLFluffyShipInfo2))
                return __exception;
            __instance.MyStats.SetSlotLimit(ESlotType.E_COMP_AUTO_TURRET, 1);
            if (previewStats)
                return __exception;
            GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(__instance.RegularTurretPoints[0].gameObject);
            gameObject.transform.SetParent(__instance.Exterior.transform);
            gameObject.transform.localPosition = new Vector3(__instance.RegularTurretPoints[0].localPosition.x * -1, __instance.RegularTurretPoints[0].localPosition.y, __instance.RegularTurretPoints[0].localPosition.z);
            gameObject.transform.localRotation = __instance.RegularTurretPoints[0].localRotation;
            Transform[] transformArray = new Transform[1]
            {
                    gameObject.transform
            };
            __instance.AutoTurretPoints = transformArray;
            if (!__instance.ShouldCreateDefaultComponents || !(PhotonNetwork.isMasterClient | previewStats))
                return __exception;
            if (!startingPlayerShip)
            {
                PLRand deterministicRand = PLShipInfoBase.GetShipDeterministicRand(__instance.PersistantShipInfo);
                float num = Mathf.Clamp01((float)((double)deterministicRand.NextFloat() + (double)(float)PLServer.Instance.ChaosLevel * 0.02));
                if (num >= 0.65)
                {
                    __instance.MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_AUTO_TURRET, (int)AutoTurretModManager.Instance.GetAutoTurretIDFromName("Auto Laser Turret"), __instance.GetChaosBoost(deterministicRand.Next() % 50), 0, 12)), visualSlot: ESlotType.E_COMP_AUTO_TURRET);
                }
                else
                {
                    __instance.MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_AUTO_TURRET, (int)0, __instance.GetChaosBoost(deterministicRand.Next() % 50), 0, 12)), visualSlot: ESlotType.E_COMP_AUTO_TURRET);
                }
            }
            return __exception;
        }
    }
}
