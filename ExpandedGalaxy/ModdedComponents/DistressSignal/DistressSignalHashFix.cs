using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLDistressSignal), "CreateDistressSignalFromHash")]
    internal class DistressSignalHashFix
    {
        public static PLDistressSignal CreateDistressSignal(int Subtype, int level)
        {

            if (Subtype == 4)
            {
                return new MiningDroneSignal(4, level);
            }
            return new PLDistressSignal((EDistressSignalType)Subtype, level);
        }
        private static bool Prefix(int inSubType, int inLevel, int inSubTypeData, ref PLShipComponent __result)
        {
            __result = (PLShipComponent)CreateDistressSignal(inSubType, inLevel);
            return false;
        }
    }
}
