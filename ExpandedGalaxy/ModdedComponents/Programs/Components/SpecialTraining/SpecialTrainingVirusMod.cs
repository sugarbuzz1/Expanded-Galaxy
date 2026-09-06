using PulsarModLoader.Content.Components.Virus;
using UnityEngine;

namespace ExpandedGalaxy
{
    public class SpecialTrainingVirusMod : VirusMod
    {
        public override string Name => "Special Training";

        public override string Description => "Slowly fills ship with acidic gas";

        public override int InfectionTimeLimitMs => 30000;

        public override void FinalLateAddStats(PLShipComponent InComp)
        {
            PLShipInfoBase pLShipInfoBase = InComp.ShipStats.Ship;
            if (pLShipInfoBase != null)
            {
                pLShipInfoBase.AcidicAtmoBoostAlpha += 20f * Time.deltaTime;
                pLShipInfoBase.AuxConfig &= (byte)251U;
            }
        }
    }
}

