using HarmonyLib;
using static ExpandedGalaxy.Turrets;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLProjectile), "Update")]
    internal class StrictProjRange
    {
        private static void Postfix(PLProjectile __instance)
        {
            if (!(__instance != null))
                return;
            if (!__instance.MissileFlag && __instance.TurretID != -1 && __instance.OwnerShipID != -1)
            {
                PLShipInfoBase shipFromID = PLEncounterManager.Instance.GetShipFromID(__instance.OwnerShipID);
                if (shipFromID != null)
                {
                    PLTurret turret = shipFromID.GetTurretAtID(__instance.TurretID);
                    if (turret == null)
                        turret = shipFromID.GetAutoTurretAtID(__instance.TurretID);
                    if (turret != null && turret.TurretInstance != null)
                    {
                        Traverse travere = Traverse.Create(turret);
                        if ((__instance.transform.position - turret.TurretInstance.RefJoint.position).magnitude > travere.Field("TurretRange").GetValue<float>() / 5f)
                        {
                            if (__instance.ExplodeOnMaxLifetime)
                            {
                                __instance.EmitExplostionParticleSystem(__instance._transform.position);
                                ServerExplosiveProjExplode(__instance, null);
                            }
                            UnityEngine.Object.Destroy(__instance.gameObject);
                        }
                        else
                            __instance.MaxLifetime = travere.Field("TurretRange").GetValue<float>() / 1000f * 200f / __instance.Speed + 3f;
                    }
                }
            }
        }
    }

}
