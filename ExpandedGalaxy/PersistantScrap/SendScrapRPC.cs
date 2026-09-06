using PulsarModLoader;
using UnityEngine;

namespace ExpandedGalaxy
{
    public class SendScrapRPC : ModMessage
    {
        public override void HandleRPC(object[] arguments, PhotonMessageInfo sender)
        {
            PLSpecialEncounterNetObject.m_IDCounter = (int)arguments[0];
            if (arguments.Length > 1)
            {
                if (PLEncounterManager.Instance.GetCPEI() == null)
                    return;
                PLSpaceScrap component = UnityEngine.Object.Instantiate<GameObject>(PLGlobal.Instance.ScrapPrefab, (Vector3)arguments[1], (Quaternion)arguments[2]).GetComponent<PLSpaceScrap>();
                if (!((UnityEngine.Object)component != (UnityEngine.Object)null))
                    return;
                component.IsSpecificComponentScrap = (bool)arguments[3];
                component.SpecificComponent_CompHash = (int)arguments[4];
                component.CanGiveComponent = (int)arguments[4] != -1;
                PLSpecialEncounterNetObject.InitNewObject(component, PLEncounterManager.Instance.GetCPEI());
            }
        }
    }
}
