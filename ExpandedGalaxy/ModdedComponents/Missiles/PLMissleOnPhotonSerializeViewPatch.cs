using HarmonyLib;
using UnityEngine;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLMissle), "OnPhotonSerializeView")]
    internal class PLMissleOnPhotonSerializeViewPatch
    {
        private static bool Prefix(PLMissle __instance, PhotonStream stream, PhotonMessageInfo info, ref Rigidbody ___MyRigidbody, ref Vector3 ___LerpedHeading, ref float ___LerpedSpeed, ref Vector3 ___m_SyncedWorldPosition, ref float ___LastWorldPosSyncTime)
        {
            if (stream.isWriting)
            {
                stream.SendNext(__instance.ProjID);
                stream.SendNext(__instance.TargetShipID);
                stream.SendNext(__instance.OwnerShipID);
                stream.SendNext(__instance.TargetShipSystemID);

                stream.SendNext(___MyRigidbody.position + ___MyRigidbody.velocity);

                stream.SendNext(___LerpedHeading);
                stream.SendNext(___LerpedSpeed);
            }
            else
            {
                __instance.ProjID = (int)stream.ReceiveNext();
                __instance.TargetShipID = (int)stream.ReceiveNext();
                __instance.OwnerShipID = (int)stream.ReceiveNext();
                __instance.TargetShipSystemID = (int)stream.ReceiveNext();

                ___m_SyncedWorldPosition = (Vector3)stream.ReceiveNext();


                ___LerpedHeading = ((Vector3)stream.ReceiveNext()).normalized;

                ___LerpedSpeed = (float)stream.ReceiveNext();

                ___LastWorldPosSyncTime = Time.time;
            }
            return false;
        }
    }
}
