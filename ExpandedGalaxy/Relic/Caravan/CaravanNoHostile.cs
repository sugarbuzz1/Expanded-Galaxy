using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLShipInfoBase), "ShouldBeHostileToShip")]
    internal class CaravanNoHostile
    {
        private static bool Prefix(PLShipInfoBase __instance, PLShipInfoBase inShip, bool enforceDialogueNotHostile, bool makeHostileIfTrue, bool enforceWarp, PLShipInfoBase ___LastValue_TakeDamage_attackingShip, ref bool __result)
        {
            if (__instance.ShipTypeID == EShipType.E_ROLAND && __instance.SelectedActorID == "ExGal_RelicCaravan")
            {
                if (PLMissionObjective.IDIsCompleted("ExGal_TheMap_Hidden_TalkCaravan") && inShip.GetIsPlayerShip())
                {
                    __result = true;
                    return false;
                }
                if (___LastValue_TakeDamage_attackingShip == inShip || __instance.MyStats.GetComponentsOfType(ESlotType.E_COMP_VIRUS).Count > 0)
                    return true;
                else
                {
                    __result = false;
                    return false;
                }
            }
            return true;
        }
    }
}
