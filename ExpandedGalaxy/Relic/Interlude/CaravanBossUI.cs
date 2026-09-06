using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLShipInfoBase), "ShouldShowBossUI")]
    internal class CaravanBossUI
    {
        private static void Postfix(PLShipInfoBase __instance, ref bool __result)
        {
            if (!__instance.GetIsPlayerShip() && __instance.SelectedActorID == "ExGal_RelicCaravan" && PLMissionObjective.IDIsCompleted("ExGal_TheMap_Hidden_TalkCaravan"))
                __result = true;
        }
    }
}
