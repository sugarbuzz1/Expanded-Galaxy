using HarmonyLib;
using PulsarModLoader.Content.Components.Turret;
using UnityEngine;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLWDAnnihilatorInfo), "SetupShipStats")]
    internal class AnnihilatorInfoMod
    {
        private static void Postfix(PLWDAnnihilatorInfo __instance, bool previewStats, bool startingPlayerShip)
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
    }
}
