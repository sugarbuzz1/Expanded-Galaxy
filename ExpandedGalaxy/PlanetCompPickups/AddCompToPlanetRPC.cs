using PulsarModLoader;

namespace ExpandedGalaxy
{
    internal class AddCompToPlanetRPC : ModMessage
    {
        public override void HandleRPC(object[] arguments, PhotonMessageInfo sender)
        {
            if (!sender.sender.IsMasterClient)
                return;
            if (PLEncounterManager.Instance == null || PLEncounterManager.Instance.GetPersistantEncounterInstanceAtID((int)arguments[0]) == null)
                return;
            PlanetCompPickups.AddCompToPlanet(PLEncounterManager.Instance.GetPersistantEncounterInstanceAtID((int)arguments[0]), (int)arguments[1], (int[])arguments[2], (bool)arguments[3]);
        }
    }
}
