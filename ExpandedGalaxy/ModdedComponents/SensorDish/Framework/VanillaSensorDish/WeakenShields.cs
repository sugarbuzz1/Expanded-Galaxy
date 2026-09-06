using System;
using UnityEngine;

namespace ExpandedGalaxy.SensorDish
{
    public class WeakenShields : SensorDishAbility
    {
        public override string Name => "Weaken Shields";

        public override string Description => "Target ship's shields will take 20% more damage for 60 seconds";

        public override Texture2D IconTexture => (Texture2D)Resources.Load("Icons/49_Warp");

        public override Type ScreenToBlink => typeof(PLScientistSensorScreen);

        public override bool DoesWeaknessStillMakeSense(PLShipInfoBase inHomeShipInfo, PLShipInfoBase inTargetShipInfo)
        {
            if (inTargetShipInfo.MyShieldGenerator != null)
                return inTargetShipInfo.MyStats.ShieldsCurrent > inTargetShipInfo.MyShieldGenerator.MinIntegrityAfterDamageScaled;
            else
                return false;
        }
    }
}
