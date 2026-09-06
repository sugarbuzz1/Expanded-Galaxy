using PulsarModLoader.Content.Components.WarpDriveProgram;
using UnityEngine;

namespace ExpandedGalaxy
{
    public class CorruptionProgramMod : WarpDriveProgramMod
    {
        public override string Name => "Corruption [VIRUS]";

        public override string Description => "Broadcasts [Corruption] virus to nearby ships on activation.\n\nCorruption: Causes the ship systems to malfunction for 5 minutes";

        public override int MarketPrice => 20000;

        public override string ShortName => "CR";

        public override bool IsVirus => true;

        public override int VirusSubtype => (int)EVirusType.CORRUPTION;

        public override int MaxLevelCharges => 4;

        public override float ActiveTime => 30f;

        public override Texture2D IconTexture => PLGlobal.Instance.VirusBGTexture;
    }
}

