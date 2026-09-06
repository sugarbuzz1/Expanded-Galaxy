using HarmonyLib;
using System;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLCaptainsChair), MethodType.Constructor, new Type[3] { typeof(ECaptainsChairType), typeof(int), typeof(short) })]
    internal class ChairDesc
    {
        private static void Postfix(PLCaptainsChair __instance, ECaptainsChairType inType, int inLevel, short inSubTypeData)
        {
            switch (inType)
            {
                case ECaptainsChairType.E_COLONIAL_MODERN:
                    __instance.Desc = "A new chair designed for Colonial Union ships. Though stiffer than the classic chairs used by the Colonial Union Fleet, this chair offers wonderful lumbar support. \n\nBoosts Shield Charge Rate";
                    break;
                case ECaptainsChairType.E_WD_CLASSIC:
                    __instance.Desc = "Like everything produced by the W.D. Corporation, this chair is hard and uncomfortable. It instills an aggressiveness in any captain who sits in it. \n\nBoosts Turret Charge Rate";
                    break;
                case ECaptainsChairType.E_COLONIAL_CLASSIC:
                    __instance.Desc = "A design of chair that has been installed in Colonial Union ships for many decades. It is quite comfortable. \n\nBoosts EM Detection";
                    break;
            }
        }
    }
}
