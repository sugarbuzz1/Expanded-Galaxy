using HarmonyLib;
using System;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLVirus), MethodType.Constructor, new Type[3] { typeof(EVirusType), typeof(int), typeof(short) })]
    internal class BetterRand
    {
        private static void Postfix(PLVirus __instance, EVirusType inType, int inLevel, short inSubTypeData)
        {
            if (__instance.SubType != (int)EVirusType.RAND_SMALL && __instance.SubType != (int)EVirusType.RAND_LARGE)
                return;
            if (PhotonNetwork.isMasterClient)
            {
                PLRand rand = new PLRand((int)PLServer.Instance.GalaxySeed + PLServer.Instance.GetEstimatedServerMs());
                __instance.SubTypeData = (short)(rand.Next() % 4);
            }
            if (__instance.SubType == (int)EVirusType.RAND_LARGE)
            {
                __instance.Desc = "Unknown/random function";
            }
        }
    }
}

