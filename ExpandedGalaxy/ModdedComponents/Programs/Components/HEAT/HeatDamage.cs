using HarmonyLib;
using PulsarModLoader.Content.Components.WarpDriveProgram;
using System.Collections.Generic;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLShipInfoBase), "TakeDamage")]
    internal class HeatDamage
    {
        private static bool Prefix(PLShipInfoBase __instance, ref float dmg, ref EDamageType dmgType, float randomNum, int SystemTargetID, PLShipInfoBase attackingShip, int turretID)
        {
            bool flag = false;
            if (attackingShip != null)
            {
                List<PLShipComponent> programs = attackingShip.MyStats.GetComponentsOfType(ESlotType.E_COMP_PROGRAM);
                foreach (PLShipComponent p in programs)
                {
                    PLWarpDriveProgram program = p as PLWarpDriveProgram;
                    if (program != null && program.SubType == WarpDriveProgramModManager.Instance.GetWarpDriveProgramIDFromName("H.E.A.T."))
                    {
                        if (program.GetActiveTimerAlpha() < 1f)
                        {
                            flag = true;
                        }
                    }
                }
                if (!flag)
                    return true;

                PLTurret turret = attackingShip.GetTurretAtID(turretID);
                if (turret != null && dmgType == EDamageType.E_PHYSICAL)
                {
                    dmg = dmg * 0.8f;
                    dmgType = EDamageType.E_BIOHAZARD;
                }
            }
            return true;
        }
    }
}

