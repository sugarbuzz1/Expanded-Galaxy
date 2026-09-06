using HarmonyLib;
using System.Collections.Generic;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLServer), "ServerTakeDamageProjectile")]
    internal class SeekerDamage
    {
        private static bool Prefix(PLServer __instance, int inShipID, float dmg, bool bottomHit, int inProjID, int dmgType, int SystemTargetID, int attackingShipID, int turretID, ref List<int> ___ProcessedProjIDs)
        {
            PLShipInfoBase shipFromId = PLEncounterManager.Instance.GetShipFromID(inShipID);
            PLSpaceTarget spaceTargetFromId = PLEncounterManager.Instance.GetSpaceTargetFromID(inShipID);
            if ((UnityEngine.Object)shipFromId != (UnityEngine.Object)null)
            {
                if (___ProcessedProjIDs.Contains(inProjID))
                    return false;
                bool flag = false;
                foreach (PLProjectile projectile in __instance.m_ActiveProjectiles)
                {
                    if (projectile != null && projectile.ProjID == inProjID && projectile.name.Contains("(seeker)"))
                    {
                        flag = true;
                        break;
                    }

                }
                if (!flag)
                    return true;
                Systems.TickDamage(10, shipFromId, dmg, false, EDamageType.E_ARMOR_PIERCE_PHYS, -1, PLEncounterManager.Instance.GetShipFromID(attackingShipID), -1);
                ___ProcessedProjIDs.Add(inProjID);
                return false;
            }
            else
            {
                if ((UnityEngine.Object)spaceTargetFromId == (UnityEngine.Object)null || ___ProcessedProjIDs.Contains(inProjID))
                    return false;
                bool flag = false;
                foreach (PLProjectile projectile in __instance.m_ActiveProjectiles)
                {
                    if (projectile != null && projectile.ProjID == inProjID && projectile.name.Contains("(seeker)"))
                    {
                        flag = true;
                        break;
                    }

                }
                if (!flag)
                    return true;
                ___ProcessedProjIDs.Add(inProjID);
                Systems.TickDamage(10, spaceTargetFromId, dmg, false, EDamageType.E_ARMOR_PIERCE_PHYS, -1, null, -1);
            }
            return true;
        }
    }
}
