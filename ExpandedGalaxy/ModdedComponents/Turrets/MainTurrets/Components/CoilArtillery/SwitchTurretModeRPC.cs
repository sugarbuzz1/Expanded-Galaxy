using PulsarModLoader;

namespace ExpandedGalaxy
{
    internal class SwitchTurretModeRPC : ModMessage
    {
        public override void HandleRPC(object[] arguments, PhotonMessageInfo sender)
        {
            PLShipInfoBase shipInfo = PLEncounterManager.Instance.GetShipFromID((int)arguments[0]);
            if (shipInfo.MyStats != null)
            {
                PhysicalTurret turret = (PhysicalTurret)shipInfo.MyStats.GetComponentFromNetID((int)arguments[1]);
                if (turret != null)
                {
                    turret.SubTypeData = (short)arguments[2];
                    turret.ChargeAmount = 0f;
                }
            }
        }
    }
}
