using CodeStage.AntiCheat.ObscuredTypes;
using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLFluffyShipInfo2), "SetupShipStats")]
    internal class FluffyTwoInfoMod
    {
        private static void Postfix(PLFluffyShipInfo2 __instance, bool previewStats, bool startingPlayerShip)
        {
            if (__instance == null || __instance.MyStats == null)
                return;
            try
            {
                if (!__instance.ShouldCreateDefaultComponents || !(PhotonNetwork.isMasterClient | previewStats))
                    return;
                if (!previewStats)
                {
                    __instance.Vents[1].Health = (ObscuredFloat)0f;
                    __instance.Vents[1].IsDead = (ObscuredBool)true;
                }
                if (startingPlayerShip)
                {
                    return;
                }
                PLRand deterministicRand = PLShipInfoBase.GetShipDeterministicRand(__instance.PersistantShipInfo);
                __instance.MyStats.RemoveShipComponent((PLShipComponent)__instance.MyStats.GetShipComponent<PLHullPlating>(ESlotType.E_COMP_HULLPLATING));
                __instance.MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_HULLPLATING, (int)EHullPlatingType.E_HULLPLATING_CCGE, __instance.GetChaosBoost(deterministicRand.Next() % 50), 0, 12)), visualSlot: ESlotType.E_COMP_HULLPLATING);
                __instance.NumberOfFuelCapsules = 15;
            }
            catch { }
        }
    }
}
