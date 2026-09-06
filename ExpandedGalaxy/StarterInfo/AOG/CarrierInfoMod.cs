using HarmonyLib;
using PulsarModLoader.Content.Components.Shield;
using PulsarModLoader.Content.Components.Turret;
using UnityEngine;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLCarrierInfo), "SetupShipStats")]
    internal class CarrierInfoMod
    {
        private static bool bypass = false;
        internal static void Postfix(PLCarrierInfo __instance, bool previewStats, bool startingPlayerShip)
        {
            if (!bypass && PhotonNetwork.offlineMode && previewStats)
            {
                bypass = true;
                PLUICreateGameMenu.Instance.StartCoroutine(StarterInfo.DelayedSetupShipStatsPostfix(3, __instance, previewStats, startingPlayerShip));
                return;
            }
            bypass = false;
            if (__instance == null || __instance.MyStats == null)
                return;
            __instance.MyStats.SetSlotLimit(ESlotType.E_COMP_NUCLEARDEVICE, 1);
            try
            {
                if (!__instance.ShouldCreateDefaultComponents || !(PhotonNetwork.isMasterClient | previewStats))
                    return;
                PLShipComponent[] shipComponents = __instance.MyStats.AllComponents.ToArray();
                int netID = -1;
                foreach (PLShipComponent shipComponent in shipComponents)
                {
                    if (shipComponent is PLWarpDriveProgram && shipComponent.SubType == (int)EWarpDriveProgramType.BLOCK_LONG_RANGE_COMMS)
                    {
                        netID = shipComponent.NetID;
                        break;
                    }
                }
                if (netID != -1)
                    __instance.MyStats.RemoveShipComponentByNetID(netID);
                __instance.MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_PROGRAM, (int)EWarpDriveProgramType.DETECTOR, 0, 0, (int)ESlotType.E_COMP_PROGRAM)));
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
                    if (num >= 0.75)
                    {
                        __instance.MyStats.RemoveShipComponent(__instance.MyStats.GetComponentsOfType(ESlotType.E_COMP_TURRET)[deterministicRand.Next() % 2]);
                        bool flag = (double)(float)PLServer.Instance.ChaosLevel > 2.0 && (int)deterministicRand.Next() % 3 == 0;
                        if (flag)
                            __instance.MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_TURRET, TurretModManager.Instance.GetTurretIDFromName("Particle Lance"), __instance.GetChaosBoost(deterministicRand.Next() % 50), 0, 12)), visualSlot: ESlotType.E_COMP_TURRET);
                        else
                            __instance.MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_TURRET, TurretModManager.Instance.GetTurretIDFromName("Sylvassi Turret"), __instance.GetChaosBoost(deterministicRand.Next() % 50), 0, 12)), visualSlot: ESlotType.E_COMP_TURRET);
                    }
                    if (num >= 0.5 && (double)(float)PLServer.Instance.ChaosLevel > 2.0)
                    {
                        if ((int)deterministicRand.Next() % 3 == 0)
                        {
                            __instance.MyStats.RemoveShipComponent(__instance.MyStats.GetShipComponent<PLShieldGenerator>(ESlotType.E_COMP_SHLD));
                            __instance.MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_SHLD, ShieldModManager.Instance.GetShieldIDFromName("Reflector Shield Generator"), __instance.GetChaosBoost(deterministicRand.Next() % 50), 0, 12)), visualSlot: ESlotType.E_COMP_SHLD);
                        }
                    }
                }
                __instance.ActivateCloakingSystem();
            }
            catch { }
        }
    }
}
