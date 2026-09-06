using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLShipInfoBase), "get_IsSectorCommander")]
    internal class IsSectorCommanderPatch
    {
        private static void Postfix(PLShipInfoBase __instance, ref bool __result)
        {
            if (__instance.PersistantShipInfo != null)
            {
                if (!__instance.GetIsPlayerShip() && __instance.PersistantShipInfo.Type == EShipType.E_POLYTECH_SHIP && (__instance.PersistantShipInfo.ShipName == "The Recompiler: Config. 1" || __instance.PersistantShipInfo.ShipName == "The Recompiler: Config. 2" || __instance.PersistantShipInfo.ShipName == "The Recompiler: Config. 3" || __instance.PersistantShipInfo.ShipName == "The Recompiler: Config. 4" || __instance.PersistantShipInfo.ShipName == "The Recompiler: Config. 5"))
                    __result = true;
            }
        }
    }
}
