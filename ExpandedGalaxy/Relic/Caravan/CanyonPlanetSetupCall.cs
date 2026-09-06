using HarmonyLib;
using System.Linq;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLPersistantEncounterInstance), "InitGame")]
    internal class CanyonPlanetSetupCall
    {
        private static bool Prefix(PLPersistantEncounterInstance __instance, int inHubID)
        {
            if (__instance is PLCanyonPlanetEncounter)
            {
                PLSectorInfo sector = PLServer.GetSectorWithID(inHubID);
                if (sector == null) return true;
                if (!sector.Visited && PhotonNetwork.isMasterClient && sector.MissionSpecificID == 8000002)
                {
                    int[] hashes = new int[1]
                    {
                            (int)PLMissionShipComponent.CreateMissionComponentFromHash(8, 0, 0).getHash(),
                    };
                    PlanetCompPickups.PickupQueue.Add(inHubID, hashes.ToList<int>());
                }
            }
            return true;
        }
    }
}
