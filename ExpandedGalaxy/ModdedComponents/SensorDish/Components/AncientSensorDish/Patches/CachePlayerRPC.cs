using PulsarModLoader;

namespace ExpandedGalaxy.SensorDish.Components.AncientSensorDish.Patches
{
    internal class CachePlayerRPC : ModMessage
    {
        public override void HandleRPC(object[] arguments, PhotonMessageInfo sender)
        {
            AncientSensorDishMod.lastToInteract = PLServer.Instance.GetPlayerFromPlayerID((int)arguments[0]);
        }
    }
}
