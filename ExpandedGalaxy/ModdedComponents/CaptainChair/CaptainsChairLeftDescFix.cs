using HarmonyLib;
using PulsarModLoader.Content.Components.CaptainsChair;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLWare), "GetStatLineLeft")]
    internal class CaptainsChairLeftDescFix
    {
        private static void Postfix(PLWare __instance, ref string __result)
        {
            PLCaptainsChair chair = __instance as PLCaptainsChair;
            if (chair == null)
                return;
            if (chair.SubType < 3)
            {
                __result = "Boost\n";
                return;
            }
            int index = chair.SubType - CaptainsChairModManager.Instance.VanillaCaptainsChairMaxType;
            if (index <= -1 || index >= CaptainsChairModManager.Instance.CaptainsChairTypes.Count || chair.ShipStats == null)
                return;
            __result = CaptainsChairModManager.Instance.CaptainsChairTypes[index].GetStatLineLeft((PLShipComponent)chair);
        }
    }
}
