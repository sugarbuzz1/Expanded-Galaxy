using PulsarModLoader;

namespace ExpandedGalaxy
{
    internal class SendWarning : ModMessage
    {
        public override void HandleRPC(object[] arguments, PhotonMessageInfo sender)
        {
            PLTabMenu.Instance.TimedErrorMsg = (string)arguments[0];
        }
    }
}

