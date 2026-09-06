using System;
using UnityEngine;

namespace ExpandedGalaxy.SensorDish
{
    public abstract class SensorDishAbility
    {
        public virtual string Name => "";
        //Description for Tooltip text
        public virtual string Description => "";

        public virtual Texture2D IconTexture => (Texture2D)Resources.Load("defaultShipCompIcon");

        //What screen will blink when weakness is active
        public virtual Type ScreenToBlink => null;

        //What color the screen will be when blinking
        public virtual Color ScreenBlinkColor => new Color(0.8f, 0.0f, 0.0f, 1f);

        public virtual void OnActivate(PLShipStats inStats)
        {
        }

        //Runs before CalculateStats() as a Prefix
        public virtual void PreCalculateStats(PLShipStats inStats)
        {
        }

        //Runs after CalculateStats() as a Postfix
        public virtual void PostCalculateStats(PLShipStats inStats)
        {
        }

        //Called via PLShipInfoBase.OnCalculateStatsFinal(), Runs after components have been processed but before talents, modifiers, ect.
        public virtual void CalculateStatsFinal(PLShipStats inStats)
        {
        }


        public virtual bool DoesWeaknessStillMakeSense(PLShipInfoBase inHomeShipInfo, PLShipInfoBase inTargetShipInfo)
        {
            return false;
        }
    }
}
