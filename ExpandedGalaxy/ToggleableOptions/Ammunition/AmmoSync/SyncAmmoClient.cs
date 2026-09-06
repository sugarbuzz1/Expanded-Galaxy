using PulsarModLoader;

namespace ExpandedGalaxy
{
    internal class SyncAmmoClient : ModMessage
    {
        public override void HandleRPC(object[] arguments, PhotonMessageInfo sender)
        {
            PLPawnInventoryBase inventory = PLNetworkManager.Instance.GetInvAtID((int)arguments[0]);
            if (inventory == null)
                return;
            PLPawnItem item = inventory.GetItemAtNetID((int)arguments[1]);
            if (item == null)
                return;
            item.AmmoCurrent = (int)arguments[2];
        }
    }
}
