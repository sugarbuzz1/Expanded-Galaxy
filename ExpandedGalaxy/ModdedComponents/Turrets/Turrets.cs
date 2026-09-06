using PulsarModLoader.Content.Components.MegaTurret;
using UnityEngine;

namespace ExpandedGalaxy
{
    internal class Turrets
    {
        public static int GetTurretDamageType(PLTurret turret)
        {
            if (turret.SlotType == ESlotType.E_COMP_TURRET)
            {
                switch (turret.SubType)
                {
                    case (int)ETurretType.PLASMA:
                    case (int)ETurretType.BURST:
                        return 16;
                }
            }
            else if (turret.SlotType == ESlotType.E_COMP_MAINTURRET)
            {
                if (turret.SubType == MegaTurretModManager.Instance.GetMegaTurretIDFromName("Physical Main Turret"))
                    if (turret.SubTypeData == 1)
                        return 16;
            }
            return 2;
        }

        public static bool IsProjectileAOE(PLProjectile projectile) { return projectile.MyDamageType == (EDamageType)16; }


        public static void ServerExplosiveProjExplode(PLProjectile projectile, PLSpaceTarget hitTarget = null)
        {
            if (!PhotonNetwork.isMasterClient)
                return;
            if (projectile != null && projectile.MyDamageType == (EDamageType)16)
            {
                foreach (PLSpaceTarget target in UnityEngine.Object.FindObjectsOfType<PLSpaceTarget>())
                {
                    if (target.SpaceTargetID != (hitTarget != null ? hitTarget.SpaceTargetID : -1))
                    {
                        bool flag = false;
                        PLShipInfoBase inShip = target as PLShipInfoBase;
                        if (inShip != null && inShip.ShipID != projectile.OwnerShipID && (inShip.Exterior.transform.position - projectile.transform.position).magnitude < 100f)
                        {
                            if ((double)Vector3.Dot((projectile.transform.position - inShip.Exterior.transform.position).normalized, -inShip.Exterior.transform.up) > -0.10000000149011612)
                                flag = true;
                            PLServer.Instance.photonView.RPC("ClientShipTakeDamage", PhotonTargets.All, (object)inShip.ShipID, (object)projectile.Damage, (object)flag, (object)(int)projectile.MyDamageType, (object)1f, (object)projectile.TargetShipSystemID, (object)projectile.OwnerShipID, (object)projectile.TurretID);
                        }
                        else if (target != null && (target.transform.position - projectile.transform.position).magnitude < 100f)
                            PLServer.Instance.photonView.RPC("ClientSpaceTargetTakeDamage", PhotonTargets.All, (object)target.SpaceTargetID, (object)projectile.Damage);
                    }
                }
            }
        }

        public static void LaserAutoTurretDamage(
          int shipID,
          int inAttackingShipID,
          float damage,
          float randomNum,
          Vector3 localPosition,
          int inTurretID,
          int inProjID,
          int inBeamTickCount,
          int turretID)
        {
            PLShipInfoBase shipFromId1 = PLEncounterManager.Instance.GetShipFromID(shipID);
            PLShipInfoBase shipFromId2 = PLEncounterManager.Instance.GetShipFromID(inAttackingShipID);
            PLSpaceTarget spaceTargetFromId = PLEncounterManager.Instance.GetSpaceTargetFromID(shipID);
            if (shipFromId1 != null && shipFromId2 != null)
            {
                if (!(shipFromId2.GetAutoTurretAtID(inTurretID) is AutoLaser autoTurretAtId) || autoTurretAtId.GetCounterForProjID(inProjID).HasProcessedBeamTickCounter(inBeamTickCount))
                    return;
                Vector3 inWorldLoc = shipFromId1.Exterior.transform.TransformPoint(localPosition);
                bool bottomHit = false;
                if ((double)Vector3.Dot((inWorldLoc - shipFromId1.Exterior.transform.position).normalized, -shipFromId1.Exterior.transform.up) > -0.10000000149011612)
                    bottomHit = true;
                PLServer.Instance.LaserTurretExplosion(inWorldLoc, shipFromId1.ShipID, autoTurretAtId.laserTurretExplosionID);
                double damage1 = (double)shipFromId1.TakeDamage(damage, bottomHit, autoTurretAtId.LaserDamageType, randomNum, -1, shipFromId2, turretID);
            }
            else
            {
                if (!(spaceTargetFromId != null) || !(shipFromId2 != null) || !(shipFromId2.GetAutoTurretAtID(inTurretID) is AutoLaser autoTurretAtId) || autoTurretAtId.GetCounterForProjID(inProjID).HasProcessedBeamTickCounter(inBeamTickCount))
                    return;
                spaceTargetFromId.TakeDamage(damage);
            }
        }
        public static void HeldLaserTurretDamage(
            int shipID,
            int inAttackingShipID,
            float damage,
            float randomNum,
            Vector3 localPosition,
            int inTurretID,
            int inProjID,
            int inBeamTickCount,
            int turretID)
        {
            PLShipInfoBase shipFromId1 = PLEncounterManager.Instance.GetShipFromID(shipID);
            PLShipInfoBase shipFromId2 = PLEncounterManager.Instance.GetShipFromID(inAttackingShipID);
            PLSpaceTarget spaceTargetFromId = PLEncounterManager.Instance.GetSpaceTargetFromID(shipID);
            if (shipFromId1 != null && shipFromId2 != null)
            {
                if (!(shipFromId2.GetTurretAtID(inTurretID) is HeldLaserTurret HeldTurretAtId) || HeldTurretAtId.GetCounterForProjID(inProjID).HasProcessedBeamTickCounter(inBeamTickCount))
                    return;
                Vector3 inWorldLoc = shipFromId1.Exterior.transform.TransformPoint(localPosition);
                bool bottomHit = false;
                if ((double)Vector3.Dot((inWorldLoc - shipFromId1.Exterior.transform.position).normalized, -shipFromId1.Exterior.transform.up) > -0.10000000149011612)
                    bottomHit = true;
                PLServer.Instance.LaserTurretExplosion(inWorldLoc, shipFromId1.ShipID, HeldTurretAtId.laserTurretExplosionID);
                double damage1 = (double)shipFromId1.TakeDamage(damage, bottomHit, HeldTurretAtId.LaserDamageType, randomNum, -1, shipFromId2, turretID);
            }
            else
            {
                if (!(spaceTargetFromId != null) || !(shipFromId2 != null) || !(shipFromId2.GetTurretAtID(inTurretID) is HeldLaserTurret HeldTurretAtId) || HeldTurretAtId.GetCounterForProjID(inProjID).HasProcessedBeamTickCounter(inBeamTickCount))
                    return;
                spaceTargetFromId.TakeDamage(damage);
            }
        }   
    }
}
