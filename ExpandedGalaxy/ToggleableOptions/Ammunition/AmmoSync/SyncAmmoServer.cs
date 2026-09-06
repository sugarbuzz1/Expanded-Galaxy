using PulsarModLoader;

namespace ExpandedGalaxy
{
    internal class SyncAmmoServer : ModMessage
    {
        public override void HandleRPC(object[] arguments, PhotonMessageInfo sender)
        {
            PLPawnInventoryBase inventory = PLNetworkManager.Instance.GetInvAtID((int)arguments[0]);
            if (inventory == null)
                return;
            PLPawnItem item = inventory.GetItemAtNetID((int)arguments[1]);
            if (item == null)
                return;
            ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.SyncAmmoClient", PhotonTargets.Others, new object[3] { arguments[0], arguments[1], item.AmmoCurrent });
        }
    }
}
