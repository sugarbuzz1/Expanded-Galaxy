using HarmonyLib;
using PulsarModLoader.Content.Components.HullPlating;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLShipInfoBase), "LeaveExtraScrap")]
    internal class DropBonePlating
    {
        private static void Postfix(PLShipInfoBase __instance)
        {
            if (!(__instance is PLInfectedCarrier))
                return;
            if (PLServer.GetCurrentSector() == null)
                return;
            PLRand rand = new PLRand(PLServer.GetCurrentSector().ID + __instance.ShipID);
            int num = 100;
            if (PLServer.Instance.HasCompletedMissionWithID(45421))
                num += 100;
            if (rand.Next() % 1000 < num)
                PLServer.Instance.photonView.RPC("CreateSpecificShipScrapAtLocation", PhotonTargets.All, (object)(__instance.Exterior.transform.position + UnityEngine.Random.onUnitSphere * 20f), (object)__instance.Exterior.transform.position, (object)(int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_HULLPLATING, HullPlatingModManager.Instance.GetHullPlatingIDFromName("Bone Plating"), 0, 0, 12), (object)true);
        }
    }
}
