using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLAlchemistShipInfo), "SetupShipStats")]
    internal class AlchemistInfoMod
    {
        private static void Postfix(PLAlchemistShipInfo __instance, bool previewStats, bool startingPlayerShip)
        {
            if (__instance == null || __instance.MyStats == null)
                return;
            __instance.MyStats.SetSlotLimit(ESlotType.E_COMP_NUCLEARDEVICE, 1);
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
                __instance.MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_HULLPLATING, (int)EHullPlatingType.E_HULLPLATING_CCGE, 4 + __instance.GetChaosBoost(deterministicRand.Next() % 50), 0, 12)), visualSlot: ESlotType.E_COMP_HULLPLATING);
                __instance.NumberOfFuelCapsules = 15;
                __instance.MyStats.AddShipComponent(new PLNuclearDevice(ENuclearDeviceType.WD_LARGE));
                __instance.ActivateCloakingSystem();
            }
            catch { }
        }
    }
}
