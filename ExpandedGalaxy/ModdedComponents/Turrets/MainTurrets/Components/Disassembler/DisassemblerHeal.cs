using HarmonyLib;
using PulsarModLoader.Content.Components.MegaTurret;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLShipStats), "TakeHullDamage")]
    internal class DisassemblerHeal
    {
        private static void Postfix(PLShipStats __instance, PLShipInfoBase attackingShip, PLTurret turret, ref float __result)
        {
            if (!PhotonNetwork.isMasterClient)
                return;
            if (attackingShip != null && turret != null && turret.SubType == MegaTurretModManager.Instance.GetMegaTurretIDFromName("The Disassembler"))
            {
                float healNum = __result * 0.2f;
                PLHull hull = attackingShip.MyHull;
                if (hull != null)
                {
                    if (hull.SubType == (int)EHullType.E_NANO_ACTIVE_HULL || hull.SubType == (int)EHullType.E_POLYTECH_HULL)
                        healNum *= 4f;
                    PLServer.Instance.ClientRepairHull(attackingShip.ShipID, (int)healNum, 0);
                    PLServer.Instance.photonView.RPC("ClientRepairHull", PhotonTargets.Others, attackingShip.ShipID, (int)healNum, 0);
                }
            }
        }
    }
}
