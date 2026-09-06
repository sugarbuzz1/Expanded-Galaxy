using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLHull), "AddStats")]
    internal class MassForBaseHulls
    {
        private static bool Prefix(PLHull __instance, PLShipStats inStats)
        {
            float mass = 0f;

            mass = Hull.GetBaseHullMass(__instance.SubType, __instance.Level);
            if (mass > 0f)
            {
                mass -= Hull.GetMassDiscountForShip(inStats.Ship.ShipTypeID);
            }
            inStats.Mass += mass;
            return true;
        }
    }
}
