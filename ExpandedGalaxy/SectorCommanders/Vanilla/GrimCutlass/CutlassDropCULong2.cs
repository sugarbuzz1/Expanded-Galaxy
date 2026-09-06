using HarmonyLib;
using System.Collections.Generic;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLIntrepidCommanderInfo), "LeaveExtraScrap")]
    internal class CutlassDropCULong2
    {
        private static bool Prefix(PLIntrepidCommanderInfo __instance, ref List<PLShipComponent> droppedShipComponents)
        {
            bool flag = false;
            foreach (PLShipComponent plShipComponent in droppedShipComponents)
            {
                if (plShipComponent.ActualSlotType == ESlotType.E_COMP_MAINTURRET && plShipComponent.SubType == 1)
                {
                    flag = true;
                    break;
                }
            }
            if (flag)
                return false;
            PLServer.Instance.photonView.RPC("CreateSpecificShipScrapAtLocation", PhotonTargets.All, (object)(__instance.Exterior.transform.position + UnityEngine.Random.onUnitSphere * 20f), (object)__instance.Exterior.transform.position, (object)(int)PLShipComponent.createHashFromInfo(11, 5, 0, 0, 12), (object)true);
            return false;
        }
    }
}
