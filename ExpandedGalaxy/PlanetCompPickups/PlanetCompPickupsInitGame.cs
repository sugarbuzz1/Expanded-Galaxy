using HarmonyLib;
using PulsarModLoader;
using System;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLPersistantEncounterInstance), "InitGame")]
    internal class PlanetCompPickupsInitGame
    {
        private static Exception Finalizer(Exception __exception, PLPersistantEncounterInstance __instance, int inHubID)
        {
            if (!PhotonNetwork.isMasterClient)
            {
                ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.ClientRequestPickupQueue", PhotonTargets.MasterClient, new object[1]
                {
                        inHubID
                });
                return __exception;
            }
            if (PLServer.Instance == null)
                return __exception;
            if (PlanetCompPickups.PickupQueue.ContainsKey(inHubID) && PlanetCompPickups.PickupQueue[inHubID].Count > 0)
            {
                PlanetCompPickups.AddCompToPlanet(__instance, inHubID, PlanetCompPickups.PickupQueue[inHubID].ToArray());
            }
            return __exception;
        }
    }
}
