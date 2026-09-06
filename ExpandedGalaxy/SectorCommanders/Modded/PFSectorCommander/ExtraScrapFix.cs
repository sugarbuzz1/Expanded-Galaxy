using HarmonyLib;
using System.Collections.Generic;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLShipInfoBase), "DestroySelf")]
    internal class ExtraScrapFix
    {
        private static bool Prefix(PLShipInfoBase __instance, PLShipInfoBase attackingShip)
        {
            if (__instance.HasBeenDestroyed)
                return true;
            if (__instance.GetIsPlayerShip())
                return true;
            if ((UnityEngine.Object)PLServer.Instance != (UnityEngine.Object)null && PhotonNetwork.isMasterClient)
            {
                if (!__instance.DropScrap)
                {
                    List<PLShipComponent> list = new List<PLShipComponent>();
                    __instance.LeaveExtraScrap(droppedShipComponents: ref list);
                }
            }
            return true;
        }
    }
}
