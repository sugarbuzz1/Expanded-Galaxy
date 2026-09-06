using PulsarModLoader;
using UnityEngine;

namespace ExpandedGalaxy
{
    internal class ReciveAutoLaserDamage : ModMessage
    {
        public override void HandleRPC(object[] arguments, PhotonMessageInfo sender) => Turrets.LaserAutoTurretDamage((int)arguments[0], (int)arguments[1], (float)arguments[2], (float)arguments[3], (Vector3)arguments[4], (int)arguments[5], (int)arguments[6], (int)arguments[7], (int)arguments[8]);
    }
}
