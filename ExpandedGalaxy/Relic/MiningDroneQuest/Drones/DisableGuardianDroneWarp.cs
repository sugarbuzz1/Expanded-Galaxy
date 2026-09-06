using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLShipInfoBase), "Ship_WarpOutNow")]
    internal class DisableGuardianDroneWarp
    {
        private static bool Prefix(PLShipInfoBase __instance)
        {
            if (PLServer.GetCurrentSector() == null || !(PLEncounterManager.Instance.PlayerShip != null))
                return true;
            if (__instance.IsDrone && !__instance.HasBeenDestroyed)
            {
                foreach (PLShipComponent component in __instance.MyStats.GetComponentsOfType(ESlotType.E_COMP_REAC_COOLING))
                {
                    if (component is MiningDroneFlag)
                    {
                        if (component.Level == 4)
                            return false;
                        if (!MiningDroneQuest.dronesActive)
                            return false;
                        break;
                    }
                }
            }
            return true;
        }
    }
}
