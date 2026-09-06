using PulsarModLoader;
using System.Collections.Generic;

namespace ExpandedGalaxy
{
    internal class ClientRequestViewIDs : ModMessage
    {
        public override void HandleRPC(object[] arguments, PhotonMessageInfo sender)
        {
            if (PlanetObjectSetupManager.Instance.IsSetupWithIDs())
            {
                List<object> obj = new List<object>();
                foreach (int num in PlanetObjectSetupManager.Instance.GetViewIDs())
                    obj.Add((object)num);
                ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.ServerSendViewIDs", sender.sender, obj.ToArray());
            }
        }
    }
}

