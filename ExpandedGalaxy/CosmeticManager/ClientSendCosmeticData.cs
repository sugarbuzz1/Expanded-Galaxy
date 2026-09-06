using PulsarModLoader;

namespace ExpandedGalaxy
{
    internal class ClientSendCosmeticData : ModMessage
    {
        public override void HandleRPC(object[] arguments, PhotonMessageInfo sender)
        {
            CosmeticManager.Instance.AddCosmeticData((int)arguments[0], (ulong)(long)arguments[1]);
            if ((bool)arguments[2])
                ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.ClientSendCosmeticData", sender.sender, new object[3] { (int)PLNetworkManager.Instance.LocalPlayerID, (long)CosmeticManager.Instance.MyCosmeticData, false });
        }
    }
}
