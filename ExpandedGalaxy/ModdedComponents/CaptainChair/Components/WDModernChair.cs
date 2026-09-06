using HarmonyLib;
using PulsarModLoader.Content.Components.CaptainsChair;

namespace ExpandedGalaxy
{
    public class WDModernChair : CaptainsChairMod
    {
        public override string Name => "W.D. Modern Captain's Chair";

        public override string Description => "Similar to its classic counterpart, this chair is not very comfortable to sit on. It has a tendancy to make any captain who sits in it staunchly defensive of thier plotted courses.\n\nBoosts Ship Armor";

        public override int MarketPrice => 1200;

        public override string GetStatLineLeft(PLShipComponent InComp)
        {
            return "Boost\n";
        }

        public override string GetStatLineRight(PLShipComponent InComp)
        {
            return "+" + (8 + InComp.Level).ToString() + "%\n";
        }

        public override void FinalLateAddStats(PLShipComponent InComp)
        {
            if (InComp.ShipStats == null || !InComp.IsEquipped)
                return;
            InComp.ShipStats.HullArmor *= 1 + ((8 + InComp.Level) / 100f);
        }

        [HarmonyPatch(typeof(PLCaptainsChair), "GetChairPrefabName")]
        internal class WDModernChairPrefab
        {
            private static bool Prefix(PLCaptainsChair __instance, ref string __result)
            {
                if (__instance.SubType == CaptainsChairModManager.Instance.GetCaptainsChairIDFromName("W.D. Modern Captain's Chair"))
                {
                    __result = "WDClassicChair";
                    return false;
                }
                return true;
            }
        }

    }
}
