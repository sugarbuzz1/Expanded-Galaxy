using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLPlayer), "AttemptToPickupRandomComponentAtID")]
    internal class MarkPickupAsPickedUp
    {
        private static bool Prefix(int inPickupComponentID)
        {
            if (PLNetworkManager.Instance.CurrentGame == null)
                return false;
            if (PLServer.GetCurrentSector() != null && PlanetCompPickups.PickupQueue.ContainsKey(PLServer.GetCurrentSector().ID))
            {
                PLPickupRandomComponent randomComponentAtId = PLGameStatic.Instance.GetPickupRandomComponentAtID(inPickupComponentID);
                if (randomComponentAtId == null || randomComponentAtId.PickedUp)
                    return false;
                if (PlanetCompPickups.PickupQueue[PLServer.GetCurrentSector().ID].Contains(randomComponentAtId.RandComp))
                    PlanetCompPickups.PickupQueue[PLServer.GetCurrentSector().ID][PlanetCompPickups.PickupQueue[PLServer.GetCurrentSector().ID].IndexOf(randomComponentAtId.RandComp)] = -1;
            }
            return true;
        }
    }
}
