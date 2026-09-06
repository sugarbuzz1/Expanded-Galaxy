using PulsarModLoader;

namespace ExpandedGalaxy
{
    internal class TickCharge : ModMessage
    {
        public override void HandleRPC(object[] arguments, PhotonMessageInfo sender)
        {
            if (!sender.sender.IsMasterClient)
                return;
            PLShipInfoBase shipInfo = PLEncounterManager.Instance.GetShipFromID((int)arguments[0]);
            if (shipInfo.MyStats != null)
            {
                (shipInfo.MyStats.GetComponentFromNetID((int)arguments[1]) as ImpGlaive).tickCharge = true;
            }
        }
    }
}
