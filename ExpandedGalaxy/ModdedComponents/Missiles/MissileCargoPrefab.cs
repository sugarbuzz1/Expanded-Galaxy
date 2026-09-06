using HarmonyLib;
using System;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLTrackerMissile), MethodType.Constructor, new Type[3] { typeof(ETrackerMissileType), typeof(int), typeof(int) })]
    internal class MissileCargoPrefab
    {
        private static void Postfix(PLTrackerMissile __instance, ETrackerMissileType inType, int inLevel, int inSubTypeData)
        {
            switch (inType)
            {
                case ETrackerMissileType.NORMAL:
                case ETrackerMissileType.SYSTEM_DAMAGE:
                case ETrackerMissileType.FB_MISSILE:
                    __instance.CargoVisualPrefabID = 46;
                    break;
                case ETrackerMissileType.SHIELD_PIERCE:
                case ETrackerMissileType.ARMOR_PIERCE:
                    __instance.CargoVisualPrefabID = 47;
                    break;
                case ETrackerMissileType.WD_SPECIAL:
                case ETrackerMissileType.STRAIGHTSHOT:
                    __instance.CargoVisualPrefabID = 48;
                    break;
            }
        }
    }
}
