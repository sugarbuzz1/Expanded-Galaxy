using HarmonyLib;
using UnityEngine;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLMissle), "FixedUpdate")]
    public static class PLMissleFixedUpdatePatch
    {
        public static bool Prefix(PLMissle __instance, ref Rigidbody ___MyRigidbody, ref Vector3 ___LerpedHeading, ref bool ___ShouldLerpSpeed, ref float ___LerpedSpeed, ref float ___Last_DistToTarget, ref Vector3 ___m_SyncedWorldPosition, ref float ___LastWorldPosSyncTime)
        {
            if (___MyRigidbody == null)
                return false;

            PhotonView photonView = __instance.photonView;

            bool isMine = photonView == null || photonView.isMine;

            bool hasTarget = __instance.TargetShip != null;

            Rigidbody targetRb = null;

            Vector3 targetPos = Vector3.zero;
            Vector3 targetVel = Vector3.zero;

            if (hasTarget)
            {
                targetRb = __instance.TargetShip.ExteriorRigidbody;
                targetPos = __instance.TargetShip.GetCurrentSensorPosition();
                targetVel = targetRb != null ? targetRb.velocity : Vector3.zero;
            }

            Vector3 missilePos = ___MyRigidbody.position;
            Vector3 missileVel = ___MyRigidbody.velocity;
            Vector3 desiredHeading = __instance.transform.forward;
            float distance = ___Last_DistToTarget;

            if (hasTarget)
            {
                Vector3 toTarget = targetPos - missilePos;
                distance = toTarget.magnitude;

                if (distance > 0.01f)
                {
                    Vector3 relativeVelocity = targetVel - missileVel;
                    Vector3 los = toTarget.normalized;
                    Vector3 losRate = Vector3.Cross(toTarget, relativeVelocity) / Mathf.Max(toTarget.sqrMagnitude, 0.001f);
                    losRate = Vector3.ClampMagnitude(losRate, 2f);
                    const float navigationConstant = 4f;
                    Vector3 commandedTurn = navigationConstant * Vector3.Cross(losRate, missileVel.normalized);
                    desiredHeading = (missileVel.normalized + commandedTurn).normalized;
                    if (distance < 15f)
                    {
                        desiredHeading = los;
                    }
                }
            }

            if (!__instance.TrackingEnabled())
            {
                desiredHeading = __instance.transform.forward;
            }


            float maxTurnRadians = Mathf.Deg2Rad * __instance.TurnFactor * 180f * Time.fixedDeltaTime;
            ___LerpedHeading = Vector3.RotateTowards(___LerpedHeading, desiredHeading, maxTurnRadians, 0f).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(___LerpedHeading);
            PLGlobal.RotateRigidbodyWithTorque(___MyRigidbody, targetRotation, 450f * __instance.TurnFactor);

            if (___ShouldLerpSpeed)
            {
                ___LerpedSpeed = Mathf.Lerp(___LerpedSpeed, __instance.Speed, Mathf.Clamp01(Time.fixedDeltaTime * __instance.AccelerationFactor));
            }

            float thrustMultiplier = 25f;
            if (distance < 80f)
                thrustMultiplier = 20f;
            if (distance < 40f)
                thrustMultiplier = 15f;
            float turnAlignment = Vector3.Dot(__instance.transform.forward, desiredHeading);
            float turnSpeedPenalty = Mathf.Lerp(0.6f, 1f, Mathf.Clamp01((turnAlignment + 1f) * 0.5f));
            ___MyRigidbody.AddRelativeForce(Vector3.forward * ___LerpedSpeed * thrustMultiplier * turnSpeedPenalty, ForceMode.Force);

            if (!isMine)
            {
                if (___m_SyncedWorldPosition != Vector3.zero)
                {
                    float t = Mathf.Clamp01(Time.fixedTime - ___LastWorldPosSyncTime);

                    if (t < 0.99f)
                    {
                        Vector3 predictedPos = __instance.transform.position + ___MyRigidbody.velocity * (1f - t);
                        Vector3 correction = Vector3.Lerp(___m_SyncedWorldPosition, __instance.transform.position, t) - predictedPos;
                        ___MyRigidbody.AddForce(correction * 2f, ForceMode.Acceleration);
                    }
                }
            }
            else
            {
                ___m_SyncedWorldPosition = ___MyRigidbody.position;
            }

            ___Last_DistToTarget = distance;

            return false;
        }
    }
}
