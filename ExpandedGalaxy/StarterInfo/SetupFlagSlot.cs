using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLShipInfoBase), "SetupShipStats")]
    internal class SetupFlagSlot
    {
        private static void Postfix(PLShipInfoBase __instance, bool previewStats, bool startingPlayerShip)
        {
            __instance.MyStats.SetGlobalSlotLimit(ESlotType.E_COMP_REAC_COOLING, 12);
            __instance.MyStats.SetSlotLimit(ESlotType.E_COMP_REAC_COOLING, 12);
            __instance.MyStats.GetSlot(ESlotType.E_COMP_REAC_COOLING).Capacity = 12;
        }
    }
}
