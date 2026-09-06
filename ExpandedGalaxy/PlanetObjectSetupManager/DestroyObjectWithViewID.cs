using PulsarModLoader;
using UnityEngine;

namespace ExpandedGalaxy
{
    internal class DestroyObjectWithViewID : ModMessage
    {
        public override void HandleRPC(object[] arguments, PhotonMessageInfo sender)
        {
            int viewID = (int)arguments[0];
            PhotonView view = PhotonView.Find(viewID);
            if (view != null)
            {
                PlanetObjectSetupManager.Instance.RemoveID(viewID);
                GameObject.Destroy(view.gameObject);
            }
        }
    }
}

