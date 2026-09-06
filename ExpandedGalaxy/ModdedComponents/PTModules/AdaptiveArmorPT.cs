using HarmonyLib;
using PulsarModLoader.Content.Components.PolytechModule;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLShipStats), "TakeHullDamage")]
    internal class AdaptiveArmorPT
    {
        private static void Postfix(PLShipStats __instance, float inDmg, EDamageType inDmgType, PLShipInfoBase attackingShip, PLTurret turret, ref float __result)
        {
            if (__instance.Ship != null)
            {
                if (__instance.Ship.ShipTypeID == EShipType.E_POLYTECH_SHIP)
                {
                    foreach (PLShipComponent component in __instance.GetComponentsOfType(ESlotType.E_COMP_POLYTECH_MODULE))
                    {
                        PLPolytechModule module = component as PLPolytechModule;
                        if (module != null)
                        {
                            if (component.SubType == PolytechModuleModManager.Instance.GetPolytechModuleIDFromName("P.T. Module: Recompiler 1"))
                            {
                                if (PhotonNetwork.isMasterClient)
                                {
                                    component.SubTypeData = 0;
                                }
                            }
                        }
                    }
                }
            }
        }
    }

}
