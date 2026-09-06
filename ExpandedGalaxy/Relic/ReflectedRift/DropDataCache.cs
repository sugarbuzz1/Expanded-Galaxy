using HarmonyLib;
using PulsarModLoader.Content.Components.MissionShipComponent;
using System.Collections.Generic;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLShipInfoBase), "LeaveExtraScrap")]
    internal class DropDataCache
    {
        private static void Postfix(PLShipInfoBase __instance, ref List<PLShipComponent> droppedShipComponents)
        {
            if (__instance.GetIsPlayerShip())
                return;
            if (ReflectedRift.inRift && __instance.ShipTypeID == EShipType.E_WDDRONE2 && __instance.HasModifier(EShipModifierType.CORRUPTED))
            {
                int compHash = (int)PLShipComponent.createHashFromInfo(23, MissionShipComponentModManager.Instance.GetMissionShipComponentIDFromName("Data Cache"), 0, 0, (int)ESlotType.E_COMP_NONE);
                PLServer.Instance.photonView.RPC("CreateSpecificShipScrapAtLocation", PhotonTargets.All, __instance.Exterior.transform.position + UnityEngine.Random.onUnitSphere * 20f, __instance.Exterior.transform.position, (int)compHash, true);
            }
        }
    }
}
