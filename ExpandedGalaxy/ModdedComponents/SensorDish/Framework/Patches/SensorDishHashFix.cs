using HarmonyLib;
using PulsarModLoader.Utilities;

namespace ExpandedGalaxy.SensorDish
{
    [HarmonyPatch(typeof(PLSensorDish), "CreateSensorDishFromHash")]
    internal class SensorDishHashFix
    {
        private static bool Prefix(int inSubType, int inLevel, int inSubTypeData, ref PLShipComponent __result)
        {
            if (inSubType >= SensorDishModManager.Instance.SensorDishTypes.Count)
                return true;
            Logger.Info("Creating SensorDish from list info");
            __result = SensorDishModManager.Instance.SensorDishTypes[inSubType].SensorDish;
            __result.SubType = inSubType;
            __result.Level = inLevel;
            return false;
        }
    }
}
