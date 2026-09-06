using System;
using UnityEngine;

namespace ExpandedGalaxy.SensorDish
{
    public class CyberDefenseFlaw : SensorDishAbility
    {
        public override string Name => "Cyber Defense Flaw";

        public override string Description => "Target ship will be 40% more susceptible to viruses for 60 seconds";

        public override Texture2D IconTexture => (Texture2D)Resources.Load("Icons/53_Processer");

        public override Type ScreenToBlink => typeof(PLScientistVirusScreen);

        public override bool DoesWeaknessStillMakeSense(PLShipInfoBase inHomeShipInfo, PLShipInfoBase inTargetShipInfo)
        {
            return inHomeShipInfo.MyStats.CyberAttackRating > inTargetShipInfo.MyStats.CyberDefenseRating && VanillaSensorDishMod.HasUseableVirus(inHomeShipInfo);
        }
    }
}
