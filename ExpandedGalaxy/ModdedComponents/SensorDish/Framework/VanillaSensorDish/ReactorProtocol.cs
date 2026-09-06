using System;
using UnityEngine;

namespace ExpandedGalaxy.SensorDish
{
    public class ReactorProtocol : SensorDishAbility
    {
        public override string Name => "Reactor Protocol";

        public override string Description => "Target ship's reactor will generate 20% more heat for 60 seconds";

        public override Texture2D IconTexture => (Texture2D)Resources.Load("Icons/27_Reactor");

        public override Type ScreenToBlink => typeof(PLEngineerReactorScreen);

        public override bool DoesWeaknessStillMakeSense(PLShipInfoBase inHomeShipInfo, PLShipInfoBase inTargetShipInfo)
        {
            return inTargetShipInfo.MyStats.ReactorTempCurrent / inTargetShipInfo.MyStats.ReactorTempMax > 0.85f;
        }
    }
}
