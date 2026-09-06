using PulsarModLoader;
using System.Threading.Tasks;

namespace ExpandedGalaxy
{
    internal class ClientRequestPickupQueue : ModMessage
    {
        public override void HandleRPC(object[] arguments, PhotonMessageInfo sender)
        {
            if (PLEncounterManager.Instance != null && PLEncounterManager.Instance.GetCPEI() != null && PLEncounterManager.Instance.GetCPEI().GetSectorID() == (int)arguments[0])
                DelaySendPickupQueue(sender.sender, (int)arguments[0]);
            else
            {
                PLPlayer player = Systems.GetPlayerFromPhotonPlayer(sender.sender);
                if (player != null)
                    PulsarModLoader.Utilities.Logger.Info(string.Format("Could not send PickupQueue to {0}! (Sent Sector: {1})", player.GetPlayerName(), ((int)arguments[0]).ToString()));
            }
        }

        private async void DelaySendPickupQueue(PhotonPlayer sender, int inHubID)
        {
            await Task.Delay(100);
            while (PLEncounterManager.Instance == null || PLEncounterManager.Instance.GetCPEI() == null || !PLEncounterManager.Instance.GetCPEI().GameInitWithHubID)
                await Task.Yield();
            if (PLEncounterManager.Instance.GetCPEI().GetSectorID() == inHubID && PlanetCompPickups.PickupQueue.ContainsKey(inHubID) && PlanetCompPickups.PickupQueue[inHubID].Count > 0)
                ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.AddCompToPlanetRPC", sender, new object[4]
                {
                    PLEncounterManager.Instance.GetCPEI().GetSectorID(),
                    inHubID,
                    PlanetCompPickups.PickupQueue[inHubID].ToArray(),
                    true
                });
        }
    }
}
