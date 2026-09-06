using PulsarModLoader.Content.Items;

namespace ExpandedGalaxy
{
    public class WarpKeyMod : ItemMod
    {
        public override string Name => "Warp Key";

        public override PLPawnItem PLPawnItem => new PLPawnItem_WarpKey();
    }
}
