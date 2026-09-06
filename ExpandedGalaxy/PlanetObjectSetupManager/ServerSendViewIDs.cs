using PulsarModLoader;
using System.Collections.Generic;

namespace ExpandedGalaxy
{
    internal class ServerSendViewIDs : ModMessage
    {
        public override void HandleRPC(object[] arguments, PhotonMessageInfo sender)
        {
            List<int> num = new List<int>();
            foreach (object o in arguments)
            {
                num.Add((int)o);
            }
            PlanetObjectSetupManager.Instance.SetIDs(num.ToArray());
        }
    }
}

