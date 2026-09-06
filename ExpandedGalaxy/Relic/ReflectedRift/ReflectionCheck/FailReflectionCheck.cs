using PulsarModLoader;
using UnityEngine;

namespace ExpandedGalaxy
{
    internal class FailReflectionCheck : ModMessage
    {
        public override void HandleRPC(object[] arguments, PhotonMessageInfo sender)
        {
            if (PLServer.Instance != null && ReflectedRift.inRift && PLEncounterManager.Instance != null && PLEncounterManager.Instance.PlayerShip != null && !PLEncounterManager.Instance.PlayerShip.InWarp)
            {
                PLEncounterManager.Instance.PlayerShip.BlindJumpUnlocked = true;
                PLServer.Instance.Internal_AttemptBlindJump(PLEncounterManager.Instance.PlayerShip.ShipID, -1);
                PLServer.Instance.photonView.RPC("AddCrewWarning", PhotonTargets.All, new object[4]
                {
                            "The Rift Became Unstable!",
                            Color.blue,
                            0,
                            "WRN"
                });
            }
        }
    }
}
