using HarmonyLib;
using PulsarModLoader.Content.Components.CaptainsChair;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLShipComponent), "Tick")]
    internal class CaptainsChairTickPatch
    {
        private static void Postfix(PLShipComponent __instance)
        {
            PLCaptainsChair chair = __instance as PLCaptainsChair;
            if (chair == null)
                return;
            int index = chair.SubType - CaptainsChairModManager.Instance.VanillaCaptainsChairMaxType;
            if (index <= -1 || index >= CaptainsChairModManager.Instance.CaptainsChairTypes.Count || chair.ShipStats == null)
                return;
            CaptainsChairModManager.Instance.CaptainsChairTypes[index].Tick((PLShipComponent)chair);
        }
    }
}
