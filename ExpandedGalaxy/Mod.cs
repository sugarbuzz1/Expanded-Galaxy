using HarmonyLib;
using PulsarModLoader;
using System.Collections.Generic;

namespace ExpandedGalaxy
{
    public class Mod : PulsarMod
    {
        public Mod() : base()
        {
            PLGlobal.Instance.AuxSystemNames[4] = PLLocalize.Localize("Turret Autofire");
            PLGlobal.Instance.Galaxy.FactionColors[6] = Relic.getRelicColor();
        }
        public override string Version => "1.0.4";

        public override string Author => "sugarbuzz1";

        public override string ShortDescription => "Adds some stuff.";

        public override string Name => "ExpandedGalaxy";

        public override int MPRequirements => 3;

        public override string HarmonyIdentifier() => "sugarbuzz1.ExpandedGalaxy";

        public override void Unload()
        {
            PLGlobal.Instance.AuxSystemNames[4] = PLLocalize.Localize("Proj. Aim Assist");
            PLInput.Instance.EInputActionNameToString[54] = "flight_back";
            PLInput.Instance.EInputActionNameToString[53] = "flight_forward";
            PLInput.Instance.EInputActionNameToString[56] = "flight_left";
            PLInput.Instance.EInputActionNameToString[55] = "flight_right";
            PLInput.Instance.EInputActionNameToString[59] = "flight_roll_left";
            PLInput.Instance.EInputActionNameToString[60] = "flight_roll_right";
            PLInput.Instance.EInputActionNameToString[63] = "full_reverse_throttle";
            PLInput.Instance.EInputActionNameToString[62] = "full_throttle";
            Talent.SetTalentsAsUnhidden();
            PLGlobal.SafeGameObjectSetActive(Relic.RelicCaravan.CaravanIcon.CaravanLocImage.gameObject, false);
            PLGlobal.SafeGameObjectSetActive(Relic.RelicCaravan.CaravanIcon.CaravanLocBG.gameObject, false);
            Traverse traverse = Traverse.Create(PLGlobal.Instance);
            traverse.Field("CachedTalentInfos").SetValue(new Dictionary<int, TalentInfo>());
            base.Unload();
        }
    }
}
