using HarmonyLib;
using PulsarModLoader;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLServer), "ClientGetPlayerID")]
    internal class ClientSendCosmeticDataCall
    {
        private static void Postfix(int inID)
        {
            ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.ClientSendCosmeticData", PhotonTargets.Others, new object[3] { inID, (long)CosmeticManager.Instance.MyCosmeticData, true });
        }
    }
}
