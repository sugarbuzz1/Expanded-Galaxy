using PulsarModLoader.Content.Items;

namespace ExpandedGalaxy
{
    public class AutoRifleMod : ItemMod
    {
        public override string Name => "Auto Rifle";

        public override PLPawnItem PLPawnItem => new PLPawnItem_AutoRifle();
    }
}
