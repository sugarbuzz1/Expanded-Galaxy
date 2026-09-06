using HarmonyLib;
using PulsarModLoader.Content.Components.Hull;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLShipStats), "TakeHullDamage")]
    internal class JuggernautDamage
    {
        private static bool Prefix(PLShipStats __instance, ref float inDmg, EDamageType inDmgType, PLShipInfoBase attackingShip, PLTurret turret)
        {
            if (__instance.GetShipComponent<PLHull>(ESlotType.E_COMP_HULL) != null)
            {
                PLHull hull = __instance.GetShipComponent<PLHull>(ESlotType.E_COMP_HULL);
                if (hull.SubType == HullModManager.Instance.GetHullIDFromName("Juggernaut Hull"))
                {
                    inDmg *= 0.8f;
                }
            }

            if (inDmgType == EDamageType.E_COLLISION)
            {
                if ((UnityEngine.Object)attackingShip != (UnityEngine.Object)null)
                {
                    if (attackingShip.MyHull != null)
                    {
                        PLHull enemyHull = attackingShip.MyHull;
                        if (enemyHull.SubType == HullModManager.Instance.GetHullIDFromName("Juggernaut Hull"))
                        {
                            inDmg *= 2.0f;
                        }
                    }
                }
            }
            return true;
        }
    }
}
