using PulsarModLoader.Content.Components.Extractor;

namespace ExpandedGalaxy
{
    internal class PrototypeExtractorMod : ExtractorMod
    {
        public override string Name => "P.T. Extractor Prototype";

        public override string Description => "An extractor recovered from the wreckage of a Polytech ship. It guarentees that the first extraction attempt per jump is a success, but otherwise has poor stability.";

        public override float Stability => 0.8f;

        public override bool Experimental => true;
    }
}
