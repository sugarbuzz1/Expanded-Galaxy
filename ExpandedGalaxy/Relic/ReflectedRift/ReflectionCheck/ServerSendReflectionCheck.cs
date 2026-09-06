using PulsarModLoader;

namespace ExpandedGalaxy
{
    internal class ServerSendReflectionCheck : ModMessage
    {
        public override void HandleRPC(object[] arguments, PhotonMessageInfo sender)
        {
            if (PLNetworkManager.Instance == null || PLNetworkManager.Instance.MyLocalPawn == null)
                return;
            bool reflectionState = (bool)arguments[0];
            if (PLCameraSystem.Instance != null)
            {
                PLPostProcessReflection[] obj = PLCameraSystem.Instance.gameObject.GetComponentsInChildren<PLPostProcessReflection>();
                foreach (PLPostProcessReflection processReflection in obj)
                {
                    if (!processReflection.isActiveAndEnabled)
                        continue;
                    else
                    {
                        float num = processReflection.SmoothReflectionPercent;
                        if (reflectionState)
                        {
                            if (num < 0.5f)
                                ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.FailReflectionCheck", PhotonTargets.MasterClient, new object[0]);
                        }
                        else
                        {
                            if (num > 0.5f)
                                ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.FailReflectionCheck", PhotonTargets.MasterClient, new object[0]);
                        }
                        break;
                    }
                }
            }
        }
    }
}
