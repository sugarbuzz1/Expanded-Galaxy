using ExpandedGalaxy.SensorDish;
using UnityEngine;

namespace ExpandedGalaxy
{
    internal class MissileLock : SensorDishAbility
    {
        public override string Name => "Missile Lock";

        public override string Description => "Greatly improve missile lock-on speed and reload time against target ship for 60 seconds";

        public override Texture2D IconTexture => (Texture2D)Resources.Load("Icons/80_Thrusters");
    }
}
