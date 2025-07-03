using HarmonyLib;
using System.Collections.Generic;

namespace ExpandedGalaxy
{
    internal class Puppet
    {
        internal static Dictionary<int, int> shipDatas = new Dictionary<int, int>();

        [HarmonyPatch(typeof(PLShipInfoBase), "Captain_SetTargetShip")]
        internal class CaptainSetPuppetTarget
        {
            private static void Postfix(PLShipInfoBase __instance, int inShipID)
            {
                foreach (int shipID in shipDatas.Keys)
                {
                    if (shipDatas[shipID] == __instance.ShipID)
                    {
                        PLShipInfoBase pLShipInfoBase = PLEncounterManager.Instance.GetShipFromID(shipID);
                        if (pLShipInfoBase != null && PhotonNetwork.isMasterClient)
                            pLShipInfoBase.photonView.RPC("Captain_SetTargetShip", PhotonTargets.All, (object)inShipID);
                    }
                }
            }
        }

        [HarmonyPatch(typeof(PLShipInfoBase), "Ship_WarpOutNow")]
        internal class DisablePuppetDroneJump
        {
            private static bool Prefix(PLShipInfoBase __instance)
            {
                if (shipDatas.ContainsKey(__instance.ShipID))
                    return false;
                return true;
            }
        }

        [HarmonyPatch(typeof(PLShipInfoBase), "Update")]
        internal class PuppetAggro
        {
            private static bool Prefix(PLShipInfoBase __instance)
            {
                if (__instance == null)
                    return true;
                if (PLEncounterManager.Instance == null)
                    return true;
                if (shipDatas.ContainsKey(__instance.ShipID))
                {
                    PLShipInfoBase masterShip = PLEncounterManager.Instance.GetShipFromID(shipDatas[__instance.ShipID]);
                    if (masterShip == null)
                        return true;
                    if (__instance.HostileShips != null && __instance.HostileShips.Contains(masterShip.ShipID))
                        __instance.HostileShips.Remove(masterShip.ShipID);
                    if (masterShip.TargetShip != null && PhotonNetwork.isMasterClient)
                        __instance.photonView.RPC("Captain_SetTargetShip", PhotonTargets.All, (object)masterShip.TargetShip.ShipID);
                }
                return true;
            }

            private static void Postfix(PLShipInfoBase __instance)
            {
                if (__instance == null)
                    return;
                if (PLEncounterManager.Instance == null)
                    return;
                if (shipDatas.ContainsKey(__instance.ShipID))
                {
                    PLShipInfoBase masterShip = PLEncounterManager.Instance.GetShipFromID(shipDatas[__instance.ShipID]);
                    if (masterShip == null)
                        return;
                    if (__instance.HostileShips != null && __instance.HostileShips.Contains(masterShip.ShipID))
                        __instance.HostileShips.Remove(masterShip.ShipID);
                    if (masterShip.TargetShip != null && PhotonNetwork.isMasterClient)
                        __instance.photonView.RPC("Captain_SetTargetShip", PhotonTargets.All, (object)masterShip.TargetShip.ShipID);
                }
            }
        }

        [HarmonyPatch(typeof(PLSpaceScrap), "Update")]
        internal class DroneCollectScrap
        {
            private static void Postfix(PLSpaceScrap __instance)
            {
                if (__instance.Collected)
                    return;
                foreach (PLShipInfoBase plShipInfoBase in PLEncounterManager.Instance.AllShips.Values)
                {
                    if (plShipInfoBase != null && shipDatas.ContainsKey(plShipInfoBase.ShipID) && plShipInfoBase.ExteriorTransformCached != null)
                    {
                        float sqrMagnitude = (plShipInfoBase.ExteriorTransformCached.position - __instance.transform.position).sqrMagnitude;
                        if (PhotonNetwork.isMasterClient && (double)sqrMagnitude < 6400.0)
                            __instance.OnCollect();
                    }
                }
            }
        }

        [HarmonyPatch(typeof(PLTurret), "Tick")]
        internal class PuppetTurretTarget
        {
            private static bool Prefix(PLTurret __instance)
            {
                if (__instance.ShipStats == null || __instance.ShipStats.Ship == null)
                    return true;
                if (shipDatas.ContainsKey(__instance.ShipStats.Ship.ShipID))
                {
                    PLShipInfoBase masterShip = PLEncounterManager.Instance.GetShipFromID(shipDatas[__instance.ShipStats.Ship.ShipID]);
                    if (masterShip == null)
                        return true;
                    if (__instance.SpaceTargetIDsTargeted.Contains(masterShip.SpaceTargetID))
                        __instance.SpaceTargetIDsTargeted.Remove(masterShip.SpaceTargetID);
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(PLShipInfoBase), "AddHostileShip")]
        internal class ShipAggroToPuppet
        {
            private static bool Prefix(PLShipInfoBase __instance, PLShipInfoBase inShip)
            {
                if (shipDatas.ContainsKey(__instance.ShipID))
                {
                    if (shipDatas[__instance.ShipID] == inShip.ShipID)
                        return false;
                }
                else if (shipDatas.ContainsKey(inShip.ShipID))
                {
                    if (shipDatas[inShip.ShipID] == __instance.ShipID)
                        return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(PLShipInfoBase), "ShouldBeHostileToShip")]
        internal class ShipShouldBeAggroToPuppet
        {
            private static void Postfix(PLShipInfoBase __instance, PLShipInfoBase inShip, ref bool __result)
            {
                if (shipDatas.ContainsKey(__instance.ShipID))
                {
                    if (shipDatas[__instance.ShipID] == inShip.ShipID)
                        __result = false;
                }
                else if (shipDatas.ContainsKey(inShip.ShipID))
                {
                    if (shipDatas[inShip.ShipID] == __instance.ShipID)
                        __result = false;
                }
            }
        }

        [HarmonyPatch(typeof(PLInGameUI), "SetShipAsTarget")]
        internal class SetPuppetTarget
        {
            private static void Postfix(PLSpaceTarget target)
            {
                if (!PhotonNetwork.isMasterClient)
                    return;
                foreach (int shipID in shipDatas.Keys)
                {
                    PLShipInfoBase pLShipInfoBase = PLEncounterManager.Instance.GetShipFromID(shipID);
                    if (pLShipInfoBase != null)
                        pLShipInfoBase.photonView.RPC("Captain_SetTargetShip", PhotonTargets.All, (object)target.SpaceTargetID);
                }
            }
        }
    }
}
