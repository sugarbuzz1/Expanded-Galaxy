using ExpandedGalaxy.SensorDish;
using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLShipStats), "CalculateStats")]
    internal class RemoveArmor
    {
        private static void Postfix(PLShipStats __instance)
        {
            if (__instance.Ship != null)
            {
                if (SensorDishModManager.IsSensorWeaknessActiveModded(__instance.Ship, 2, SensorDishModManager.Instance.GetSensorDishIDFromName("Ancient Sensor Dish")))
                {
                    __instance.HullArmor = 0f;
                }
            }
        }
    }
}
