using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLShipInfoBase), "ShouldBeHostileToShip")]
    internal class MiningDroneBePassive
    {
        private static void Postfix(PLShipInfoBase __instance, PLShipInfoBase inShip, ref bool __result)
        {
            if (!PhotonNetwork.isMasterClient || __instance.MyStats == null || __instance is PLWarpGuardian || __instance is PLUnseenEye)
                return;
            if (__instance.IsDrone)
            {
                bool flag = false;
                foreach (PLShipComponent component in __instance.MyStats.GetComponentsOfType(ESlotType.E_COMP_REAC_COOLING))
                {
                    if (component is MiningDroneFlag)
                    {
                        /*
                        if (inShip is StarterInfo.AsteroidInfo)
                        {
                            __result = false;
                            if (__instance.HostileShips.Contains(inShip.ShipID))
                                __instance.HostileShips.Remove(inShip.ShipID);
                            return;
                        }
                        */
                        if (!__instance.PersistantShipInfo.ForcedHostile && (component.Level < 4 && __instance.LastTookDamageTime() == float.MinValue) || !MiningDroneQuest.dronesActive)
                        {
                            flag = true;
                            break;
                        }
                        if (inShip.FactionID == 6)
                        {
                            flag = true;
                            break;
                        }
                    }
                }
                if (flag)
                {
                    __result = false;
                    if (__instance.HostileShips.Contains(inShip.ShipID))
                        __instance.HostileShips.Remove(inShip.ShipID);
                }
            }
            else if (inShip.IsDrone)
            {
                foreach (PLShipComponent component in inShip.MyStats.GetComponentsOfType(ESlotType.E_COMP_REAC_COOLING))
                {
                    if (component is MiningDroneFlag)
                    {
                        if ((!inShip.PersistantShipInfo.ForcedHostile || !MiningDroneQuest.dronesActive) && component.Level < 4)
                        {
                            __result = false;
                            if (__instance.HostileShips.Contains(inShip.ShipID))
                                __instance.HostileShips.Remove(inShip.ShipID);
                            break;
                        }
                        else
                            break;
                    }
                }
            }
        }
    }
}
