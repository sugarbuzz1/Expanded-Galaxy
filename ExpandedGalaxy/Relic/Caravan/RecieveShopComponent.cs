using PulsarModLoader;

namespace ExpandedGalaxy
{
    internal class RecieveShopComponent : ModMessage
    {
        public override void HandleRPC(object[] arguments, PhotonMessageInfo sender)
        {
            RelicCaravan.LateAddShopComponent((int)arguments[0], (int)arguments[1]);
        }
    }
}
