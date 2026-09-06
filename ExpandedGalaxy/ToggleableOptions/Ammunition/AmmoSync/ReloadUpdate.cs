using HarmonyLib;
using PulsarModLoader;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLPawnInventoryBase), "RemoveItem")]
    internal class ReloadUpdate
    {
        private static bool Prefix(PLPawnInventoryBase __instance, PLPawnItem inItem)
        {
            if (!(__instance is PLPawnInventory))
                return true;
            if (inItem is PLPawnItem_AmmoClip)
            {
                PLPawnInventory inventory = __instance as PLPawnInventory;
                if (inventory.MyPlayer != PLNetworkManager.Instance.LocalPlayer)
                    return true;
                if (inventory.ActiveItem != null)
                {
                    ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.ClientSendAmmoToServer", PhotonTargets.MasterClient, new object[3] { inventory.InventoryID, new int[1] { inItem.NetID }, new int[1] { inventory.ActiveItem.AmmoCurrent } });
                }
            }
            return true;
        }
    }
}
