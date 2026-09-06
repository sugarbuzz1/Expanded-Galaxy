using PulsarModLoader;
using System.Threading.Tasks;

namespace ExpandedGalaxy
{
    internal class HostileTakeoverRPC : ModMessage
    {
        public override void HandleRPC(object[] arguments, PhotonMessageInfo sender)
        {
            PLShipInfo playerShip = (PLShipInfo)PLEncounterManager.Instance.GetShipFromID((int)arguments[0]);
            PLShipInfoBase target = (PLShipInfoBase)PLEncounterManager.Instance.GetShipFromID((int)arguments[1]);

            playerShip.MySensorDish.SubTypeData = 1;
            if (PhotonNetwork.isMasterClient)
                ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.UpdateSubTypeData", PhotonTargets.Others, new object[3]
                {
                        (object) playerShip.ShipID,
                        (object) playerShip.MySensorDish.NetID,
                        (object) playerShip.MySensorDish.SubTypeData,
                });
            Puppet.shipDatas[target.ShipID] = playerShip.ShipID;
            target.IsFlagged = true;
            target.TargetShip = playerShip.TargetShip;
            RemovePuppet(target);
        }

        private static async void RemovePuppet(PLShipInfoBase ship)
        {
            await Task.Delay(60000);
            if (ship == null)
                return;
            if (!Puppet.shipDatas.ContainsKey(ship.ShipID))
                return;
            ship.TargetShip = PLEncounterManager.Instance.GetShipFromID(Puppet.shipDatas[ship.ShipID]);
            Puppet.shipDatas.Remove(ship.ShipID);
            if (PhotonNetwork.isMasterClient)
                ship.photonView.RPC("NewShipController", PhotonTargets.All, (object)-1);

        }
    }
}
