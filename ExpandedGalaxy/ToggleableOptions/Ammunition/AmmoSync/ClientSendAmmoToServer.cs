using PulsarModLoader;

namespace ExpandedGalaxy
{
    internal class ClientSendAmmoToServer : ModMessage
    {
        public override void HandleRPC(object[] arguments, PhotonMessageInfo sender)
        {
            int[] itemNetIDs = (int[])arguments[1];
            int[] itemAmmoAmmounts = (int[])arguments[2];

            PLPawnInventoryBase inventory = PLNetworkManager.Instance.GetInvAtID((int)arguments[0]);
            if (inventory == null)
                return;

            for (int i = 0; i < itemNetIDs.Length; i++)
            {
                if (inventory.GetItemAtNetID(itemNetIDs[i]) != null)
                    inventory.GetItemAtNetID(itemNetIDs[i]).AmmoCurrent = itemAmmoAmmounts[i];
            }
        }
    }
}
