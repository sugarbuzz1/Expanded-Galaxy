using PulsarModLoader;

namespace ExpandedGalaxy
{
    internal class TriggerModdedCountdownRPC : ModMessage
    {
        public override void HandleRPC(object[] arguments, PhotonMessageInfo sender)
        {
            int type = (int)arguments[0];
            ModdedCountdown.Instance.SetupCountdown(type);
        }
    }
}

