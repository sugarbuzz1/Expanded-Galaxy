using UnityEngine;

namespace ExpandedGalaxy
{
    public class ModdedScreenBase : PLUIScreen
    {
        public override void Start()
        {
            this.ResetCachedValues();
            this.Start();
        }

        public override void Update()
        {
            base.Update();
            if (!(this.MyScreenHubBase == null))
                return;
            Object.Destroy(this);
        }
    }
}
