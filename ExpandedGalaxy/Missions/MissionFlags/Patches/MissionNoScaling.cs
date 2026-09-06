using HarmonyLib;
using System;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLShipInfoBase), "GetChaosBoost", new Type[2] { typeof(PLPersistantShipInfo), typeof(int) })]
    internal class MissionNoScaling
    {
        private static Exception Finalizer(Exception __exception, PLShipInfoBase __instance, PLPersistantShipInfo inPersistantShipInfo, int offset, ref int __result)
        {
            if (!((UnityEngine.Object)PLServer.Instance != (UnityEngine.Object)null) || inPersistantShipInfo == null)
                return __exception;
            bool flag = false;
            foreach (ComponentOverrideData data in inPersistantShipInfo.CompOverrides)
            {
                if (data.CompType == (int)ESlotType.E_COMP_REAC_COOLING && (data.CompSubType == 5))
                {
                    flag = true;
                    break;
                }
            }
            if (flag)
                __result = 0;
            return __exception;
        }
    }

}

