using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLSpaceScrap), "OnCollect")]
    internal class CollectPersistantScrap
    {
        private static bool Prefix(PLSpaceScrap __instance)
        {
            if (!PhotonNetwork.isMasterClient)
                return true;
            if (__instance.Collected)
                return false;
            PLShipInfo shipInfo = PLEncounterManager.Instance.PlayerShip;
            if (shipInfo == null || PLServer.Instance == null)
                return true;
            PLSlot slot = shipInfo.MyStats.GetSlot(ESlotType.E_COMP_CARGO);
            if (slot == null || slot.Count >= slot.MaxItems)
                return false;
            PersistantScrapManager.Instance.RemoveScrap(PLServer.GetCurrentSector().ID, __instance.EncounterNetID);
            return true;
        }
    }
}
