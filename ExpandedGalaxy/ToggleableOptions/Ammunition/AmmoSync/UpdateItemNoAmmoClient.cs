using HarmonyLib;
using PulsarModLoader;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLPawnInventoryBase), "UpdateItem")]
    internal class UpdateItemNoAmmoClient
    {
        private static void Postfix(PLPawnInventoryBase __instance, int inNetID, int inType, int inSubType, int inLevel, int inEquipID)
        {
            if (PhotonNetwork.isMasterClient)
                return;
            PLPawnItem itemAtNetId = __instance.GetItemAtNetID(inNetID);
            if (itemAtNetId != null && itemAtNetId.UsesAmmo)
            {
                ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.SyncAmmoServer", PhotonTargets.MasterClient, new object[2] { __instance.InventoryID, itemAtNetId.NetID });
            }
        }
    }
}
