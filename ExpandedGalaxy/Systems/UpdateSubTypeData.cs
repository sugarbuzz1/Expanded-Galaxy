using PulsarModLoader;

namespace ExpandedGalaxy
{
    internal class UpdateSubTypeData : ModMessage
    {
        public override void HandleRPC(object[] arguments, PhotonMessageInfo sender)
        {
            PLShipInfoBase shipInfo = PLEncounterManager.Instance.GetShipFromID((int)arguments[0]);
            if (shipInfo.MyStats != null)
            {
                shipInfo.MyStats.GetComponentFromNetID((int)arguments[1]).SubTypeData = (short)arguments[2];
            }
        }
    }
}

