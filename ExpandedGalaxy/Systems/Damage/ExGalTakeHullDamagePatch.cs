using HarmonyLib;
using UnityEngine;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLShipStats), "TakeHullDamage")]
    internal class ExGalTakeHullDamagePatch
    {
        private static void Postfix(PLShipStats __instance, float inDmg, EDamageType inDmgType, PLShipInfoBase attackingShip, PLTurret turret, ref float __result)
        {
            if (PhotonNetwork.isMasterClient && inDmgType == (EDamageType)17 && (double)__result > 0.0 && attackingShip != null)
                PLServer.Instance.photonView.RPC("ClientRepairHull", PhotonTargets.All, (object)attackingShip.ShipID, (object)Mathf.RoundToInt(__result), (object)0);
        }
    }
}

