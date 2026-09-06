using HarmonyLib;
using System;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLShipInfoBase), "GetChaosBoost", new Type[2] { typeof(PLPersistantShipInfo), typeof(int) })]
    internal class PFSectorCommanderScalingFix
    {
        private static Exception Finalizer(Exception __exception, PLShipInfoBase __instance, PLPersistantShipInfo inPersistantShipInfo, int offset, ref int __result)
        {
            if (!((UnityEngine.Object)PLServer.Instance != (UnityEngine.Object)null) || inPersistantShipInfo == null)
                return __exception;
            if (inPersistantShipInfo.Type == EShipType.E_POLYTECH_SHIP)
            {
                switch (inPersistantShipInfo.ShipName)
                {
                    case "The Recompiler: Config. 1":
                    case "The Recompiler: Config. 2":
                    case "The Recompiler: Config. 3":
                    case "The Recompiler: Config. 4":
                    case "The Recompiler: Config. 5":
                        __result = 0;
                        break;
                }
            }
            return __exception;
        }
    }
}
