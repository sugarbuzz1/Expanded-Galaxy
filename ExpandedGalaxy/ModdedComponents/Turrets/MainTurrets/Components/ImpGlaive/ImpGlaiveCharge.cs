using HarmonyLib;
using PulsarModLoader;
using PulsarModLoader.Content.Components.MegaTurret;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLShipStats), "TakeHullDamage")]
    internal class ImpGlaiveCharge
    {
        private static void Postfix(PLShipStats __instance, ref float __result)
        {
            if (!PhotonNetwork.isMasterClient)
                return;
            if (__instance.Ship != null)
            {
                if ((double)__result > 0.0)
                {
                    PLMegaTurret megaTurret = __instance.GetShipComponent<PLMegaTurret>(ESlotType.E_COMP_MAINTURRET);
                    if (!((object)megaTurret != (object)null && megaTurret.SubType == MegaTurretModManager.Instance.GetMegaTurretIDFromName("Imperial Glaive")))
                        return;
                    ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.TickCharge", PhotonTargets.All, new object[2]
                    {
                            __instance.Ship.ShipID,
                            megaTurret.NetID
                    });
                }
            }
        }
    }
}
