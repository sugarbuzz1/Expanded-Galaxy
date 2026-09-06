using HarmonyLib;
using UnityEngine;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLShipInfoBase), "FireProbe")]
    internal class FireProbePatch
    {
        private static bool Prefix(PLShipInfoBase __instance, Vector3 startPos, Quaternion startRot, int startMs, int playerIDOwner, int endMs, Vector3 endPos)
        {
            if (__instance.MyStats.GetShipComponent<PLSensorDish>(ESlotType.E_COMP_SENSORDISH) == null || __instance.MyStats.GetShipComponent<PLSensorDish>(ESlotType.E_COMP_SENSORDISH).SubType == 0)
                return true;
            if (!(__instance.MyStats.GetShipComponent<PLSensorDish>(ESlotType.E_COMP_SENSORDISH) is AncientSensorDish))
                return true;
            AncientSensorDish sensorDish = __instance.MyStats.GetShipComponent<PLSensorDish>(ESlotType.E_COMP_SENSORDISH) as AncientSensorDish;
            float probeEnergyCost = 0.65f;
            float probeCooldown = 3f;
            PLPlayer player = PLServer.Instance.GetPlayerFromPlayerID(playerIDOwner);
            if (player != null)
            {
                probeEnergyCost = (float)(0.65 * (1.0 - (double)(int)player.Talents[53] * 0.10000000149011612));
            }
            if ((double)Time.time - (double)__instance.LastFireProbeTime <= (double)probeCooldown || (double)__instance.SensorDishCapacitorCharge < (double)probeEnergyCost)
                return false;
            __instance.LastFireProbeTime = Time.time;
            if ((double)Time.time - (double)__instance.LastCloakingSystemActivatedTime > 4.0)
                __instance.SetIsCloakingSystemActive(false);
            __instance.SensorDishCapacitorCharge -= probeEnergyCost;



            Ray ray = new Ray(startPos, startRot * Vector3.forward);
            int layerMask = 524289;
            int num1 = 0;
            if ((UnityEngine.Object)__instance.GetExteriorMeshCollider() != (UnityEngine.Object)null)
            {
                num1 = __instance.GetExteriorMeshCollider().gameObject.layer;
                __instance.GetExteriorMeshCollider().gameObject.layer = 31;
            }
            try
            {
                PLMusic.PostEvent("play_ship_generic_external_weapon_phaseturret_impact", __instance.Exterior.gameObject);
                UnityEngine.RaycastHit hitInfo;
                if (Physics.SphereCast(ray, 3f, out hitInfo, 20000f, layerMask))
                {
                    PLShipInfoBase pLShipInfoBase = (PLShipInfoBase)null;
                    if ((UnityEngine.Object)hitInfo.collider != (UnityEngine.Object)null)
                        pLShipInfoBase = hitInfo.collider.GetComponentInParent<PLShipInfoBase>();
                    if ((UnityEngine.Object)pLShipInfoBase != (UnityEngine.Object)null)
                    {
                        if ((UnityEngine.Object)pLShipInfoBase != (UnityEngine.Object)__instance)
                        {
                            __instance.SensorDishTargetShipID = pLShipInfoBase.ShipID;
                        }
                    }
                }
            }
            catch
            {
            }
            if (!((UnityEngine.Object)__instance.GetExteriorMeshCollider() != (UnityEngine.Object)null))
                return false;
            __instance.GetExteriorMeshCollider().gameObject.layer = num1;
            return false;
        }
    }
}
