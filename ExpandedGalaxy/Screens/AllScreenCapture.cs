using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLCaptainScreen), "GetClaimValues")]
    internal class AllScreenCapture
    {
        private static void Postfix(PLCaptainScreen __instance, ref int playerControlledScreensCountOut, ref int countScreensTotalOut, ref int reqScreenCountOut)
        {
            reqScreenCountOut = countScreensTotalOut;
        }
    }
}

