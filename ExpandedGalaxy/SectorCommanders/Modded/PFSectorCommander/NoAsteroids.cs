using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLPersistantEncounterInstance), "CreateAsteroids")]
    internal class NoAsteroids
    {
        private static bool Prefix(PLPersistantEncounterInstance __instance, int inHubID)
        {
            PLSectorInfo sectorWithId = PLServer.GetSectorWithID(inHubID);
            if (sectorWithId.MySPI.Faction == 5 && sectorWithId.VisualIndication == ESectorVisualIndication.ALCHEMIST)
                return false;
            return true;
        }
    }
}
