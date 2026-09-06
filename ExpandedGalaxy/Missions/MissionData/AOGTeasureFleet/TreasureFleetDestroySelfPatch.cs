using HarmonyLib;
using PulsarModLoader.Content.Components.MissionShipComponent;
using System.Collections.Generic;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLShipInfoBase), "DestroySelf")]
    internal class TreasureFleetDestroySelfPatch
    {
        private static bool Prefix(PLShipInfoBase __instance, PLShipInfoBase attackingShip)
        {
            if (__instance.HasBeenDestroyed)
                return false;
            if (__instance.GetIsPlayerShip())
                return true;
            if (__instance.SelectedActorID == "ExGal_TreasureFleet_Cruiser")
            {

                List<int> netIDs = new List<int>();
                foreach (PLShipComponent component in __instance.MyStats.GetCargo())
                {
                    if (component.ActualSlotType == ESlotType.E_COMP_MISSION_COMPONENT && component.SubType == MissionShipComponentModManager.Instance.GetMissionShipComponentIDFromName("Irradiated Cargo"))
                        netIDs.Add(component.NetID);
                }
                foreach (int netID in netIDs)
                    __instance.MyStats.RemoveShipComponentByNetID(netID);
                return true;
            }
            else if (__instance.SelectedActorID == "ExGal_TreasureFleet_Friend")
            {
                if (attackingShip.GetIsPlayerShip())
                    return true;
                PLServer.Instance.AllPSIs.Remove(__instance.PersistantShipInfo);
                __instance.Ship_WarpOutNow();
                return false;
            }
            return true;
        }
    }
}

