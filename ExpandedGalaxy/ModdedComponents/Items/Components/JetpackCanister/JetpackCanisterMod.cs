using PulsarModLoader.Content.Items;

namespace ExpandedGalaxy
{
    public class JetpackCanisterMod : ItemMod
    {
        public override string Name => "Jetpack Canister";

        public override PLPawnItem PLPawnItem => new PLPawnItem_JetpackCanister();
    }
}
