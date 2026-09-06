using HarmonyLib;
using PulsarModLoader.Content.Components.MegaTurret;
using PulsarModLoader.Content.Components.Turret;
using UnityEngine;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLWDDestroyerInfo), "SetupShipStats")]
    internal class DestroyerInfoMod
    {
        private static bool bypass = false;
        internal static void Postfix(PLWDDestroyerInfo __instance, bool previewStats, bool startingPlayerShip)
        {
            if (!bypass && PhotonNetwork.offlineMode && previewStats)
            {
                bypass = true;
                PLUICreateGameMenu.Instance.StartCoroutine(StarterInfo.DelayedSetupShipStatsPostfix(1, __instance, previewStats, startingPlayerShip));
                return;
            }
            bypass = false;
            if (__instance == null || __instance.MyStats == null)
                return;
            __instance.MyStats.SetSlotLimit(ESlotType.E_COMP_NUCLEARDEVICE, 3);
            try
            {
                if (!__instance.ShouldCreateDefaultComponents || !(PhotonNetwork.isMasterClient | previewStats))
                    return;
                __instance.MyStats.AddShipComponent(PLNuclearDevice.CreateNuclearDeviceFromHash(0, 0, 0));
                __instance.MyStats.RemoveShipComponent((PLShipComponent)__instance.MyStats.GetShipComponent<PLMegaTurret>(ESlotType.E_COMP_MAINTURRET));
                if (startingPlayerShip || previewStats)
                {
                    __instance.MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_MAINTURRET, MegaTurretModManager.Instance.GetMegaTurretIDFromName("WD Standard"), 0, 0, 12)), visualSlot: ESlotType.E_COMP_MAINTURRET);
                }
                else
                {
                    PLRand deterministicRand = PLShipInfoBase.GetShipDeterministicRand(__instance.PersistantShipInfo);
                    __instance.MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_MAINTURRET, MegaTurretModManager.Instance.GetMegaTurretIDFromName("WD Standard"), __instance.GetChaosBoost(deterministicRand.Next() % 50), 0, 12)), visualSlot: ESlotType.E_COMP_MAINTURRET);
                    __instance.MyStats.RemoveShipComponent((PLShipComponent)__instance.MyStats.GetShipComponent<PLHullPlating>(ESlotType.E_COMP_HULLPLATING));
                    __instance.MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_HULLPLATING, (int)EHullPlatingType.E_HULLPLATING_CCGE, __instance.GetChaosBoost(deterministicRand.Next() % 50), 0, 12)), visualSlot: ESlotType.E_COMP_HULLPLATING);
                    __instance.NumberOfFuelCapsules = 15;
                    if ((double)(float)PLServer.Instance.ChaosLevel > 1.0)
                    {
                        float num = Mathf.Clamp01((float)((double)deterministicRand.NextFloat() + (double)(float)PLServer.Instance.ChaosLevel * 0.01999999731779099));
                        if (num >= 0.85)
                        {
                            __instance.MyStats.RemoveShipComponent(__instance.MyStats.GetComponentsOfType(ESlotType.E_COMP_TURRET)[deterministicRand.Next() % 1]);
                            bool flag = (double)(float)PLServer.Instance.ChaosLevel > 2.0 && (int)deterministicRand.Next() % 3 == 0;
                            if (flag)
                                __instance.MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_TURRET, TurretModManager.Instance.GetTurretIDFromName("Particle Lance"), __instance.GetChaosBoost(deterministicRand.Next() % 50), 0, 12)), visualSlot: ESlotType.E_COMP_TURRET);
                            else
                                __instance.MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_TURRET, TurretModManager.Instance.GetTurretIDFromName("Sylvassi Turret"), __instance.GetChaosBoost(deterministicRand.Next() % 50), 0, 12)), visualSlot: ESlotType.E_COMP_TURRET);
                        }
                        float num2 = Mathf.Clamp01((float)((double)deterministicRand.NextFloat() + (double)(float)PLServer.Instance.ChaosLevel * 0.05));
                        if ((double)(float)PLServer.Instance.ChaosLevel > 3.0 && num2 >= 1.0)
                            __instance.MyStats.AddShipComponent(new PLNuclearDevice(ENuclearDeviceType.WD_LARGE));
                        else if ((double)(float)PLServer.Instance.ChaosLevel > 2.0 && num2 >= 0.8)
                            __instance.MyStats.AddShipComponent(new PLNuclearDevice(ENuclearDeviceType.WD_SMALL));
                        else if (num2 >= 0.3)
                            __instance.MyStats.AddShipComponent(new PLNuclearDevice(ENuclearDeviceType.WD_TACTICAL));
                    }
                }
            }
            catch { }
        }
    }
}
