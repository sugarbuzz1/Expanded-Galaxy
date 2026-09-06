using PulsarModLoader.Content.Components.Virus;
using PulsarModLoader.Content.Components.WarpDriveProgram;
using UnityEngine;

namespace ExpandedGalaxy
{
    public class SpecialTrainingMod : WarpDriveProgramMod
    {
        public override string Name => "Special Training [VIRUS]";

        public override string Description => "Broadcasts [Special Training] virus to nearby ships on activation.\n\nSpecial Training: Slowly fills ship with acidic gas for 30 seconds.";

        public override int MarketPrice => 7600;

        public override string ShortName => "ST";

        public override bool IsVirus => true;

        public override int VirusSubtype => (int)VirusModManager.Instance.GetVirusIDFromName("Special Training");

        public override int MaxLevelCharges => 5;

        public override float ActiveTime => 30f;

        public override Texture2D IconTexture => PLGlobal.Instance.VirusBGTexture;

        public override bool Contraband => true;
    }
}

