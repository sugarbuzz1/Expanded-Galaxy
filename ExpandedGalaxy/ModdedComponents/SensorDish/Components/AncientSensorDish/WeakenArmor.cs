using ExpandedGalaxy.SensorDish;
using UnityEngine;

namespace ExpandedGalaxy
{
    internal class WeakenArmor : SensorDishAbility
    {
        public override string Name => "Weaken Armor";

        public override string Description => "Nullify all armor and damage reduction gained from hull plating on target ship for 60 seconds";

        public override Texture2D IconTexture => (Texture2D)Resources.Load("Icons/20_Hull");
    }
}
