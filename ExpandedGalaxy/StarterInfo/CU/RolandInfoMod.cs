using HarmonyLib;
using PulsarModLoader.Content.Components.Turret;
using System;
using UnityEngine;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLRolandInfo), "SetupShipStats")]
    internal class RolandInfoMod
    {
        private static void Postfix(PLRolandInfo __instance, bool previewStats, bool startingPlayerShip)
        {
            if (__instance == null || __instance.MyStats == null)
                return;
            try
            {
                if (!__instance.ShouldCreateDefaultComponents || !(PhotonNetwork.isMasterClient | previewStats))
                    return;
                if (startingPlayerShip || previewStats)
                {
                    if (StarterInfo.RolandSC)
                    {
                        if (previewStats)
                            __instance.MyStats.SetSlotLimit(ESlotType.E_COMP_TURRET, 6);
                        else
                            __instance.MyStats.AddShipComponent(new RolandSCFlag(), visualSlot: ESlotType.E_COMP_REAC_COOLING);
                    }
                    return;
                }
                if (__instance.PersistantShipInfo != null && __instance.PersistantShipInfo.SelectedActorID == "ExGal_RelicCaravan")
                    __instance.MyStats.SetSlotLimit(ESlotType.E_COMP_TURRET, 6);
                PLRand deterministicRand = PLShipInfoBase.GetShipDeterministicRand(__instance.PersistantShipInfo);
                __instance.MyStats.RemoveShipComponent((PLShipComponent)__instance.MyStats.GetShipComponent<PLHullPlating>(ESlotType.E_COMP_HULLPLATING));
                __instance.MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_HULLPLATING, (int)EHullPlatingType.E_HULLPLATING_CCGE, __instance.GetChaosBoost(deterministicRand.Next() % 50), 0, 12)), visualSlot: ESlotType.E_COMP_HULLPLATING);
                __instance.NumberOfFuelCapsules = 15;
                if ((double)(float)PLServer.Instance.ChaosLevel > 1.0)
                {
                    float num = Mathf.Clamp01((float)((double)deterministicRand.NextFloat() + (double)(float)PLServer.Instance.ChaosLevel * 0.01999999731779099));
                    if (num >= 0.95)
                    {
                        __instance.MyStats.RemoveShipComponent(__instance.MyStats.GetComponentsOfType(ESlotType.E_COMP_TURRET)[deterministicRand.Next() % 2]);
                        bool flag = (double)(float)PLServer.Instance.ChaosLevel > 2.0 && (int)deterministicRand.Next() % 3 == 0;
                        if (flag)
                            __instance.MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_TURRET, TurretModManager.Instance.GetTurretIDFromName("Particle Lance"), __instance.GetChaosBoost(deterministicRand.Next() % 50), 0, 12)), visualSlot: ESlotType.E_COMP_TURRET);
                        else
                            __instance.MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_TURRET, TurretModManager.Instance.GetTurretIDFromName("Sylvassi Turret"), __instance.GetChaosBoost(deterministicRand.Next() % 50), 0, 12)), visualSlot: ESlotType.E_COMP_TURRET);
                    }
                    float num2 = Mathf.Clamp01((float)((double)deterministicRand.NextFloat() + (double)(float)PLServer.Instance.ChaosLevel * 0.03));
                    if ((double)(float)PLServer.Instance.ChaosLevel > 3.0 && num2 >= 1.0)
                        __instance.MyStats.AddShipComponent(new PLNuclearDevice(ENuclearDeviceType.CU_PEACEKEEPER));
                    else if ((double)(float)PLServer.Instance.ChaosLevel > 2.0 && num2 >= 0.8)
                        __instance.MyStats.AddShipComponent(new PLNuclearDevice(ENuclearDeviceType.CU_ENFORCER));
                    else if (num2 >= 0.3)
                        __instance.MyStats.AddShipComponent(new PLNuclearDevice(ENuclearDeviceType.WD_TACTICAL));
                }
            }
            catch { }
        }

        private static Exception Finalizer(Exception __exception, PLRolandInfo __instance, bool previewStats, bool startingPlayerShip)
        {
            if (previewStats)
                return __exception;

            return __exception;
        }
    }
}
