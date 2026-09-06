using CodeStage.AntiCheat.ObscuredTypes;
using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLPolytechShipInfo), "Update")]
    internal class Fix
    {
        private static bool Prefix(PLPolytechShipInfo __instance, ref bool ___playedCutscene)
        {
            if ((bool)__instance.MyStats.isPreview)
                return true;
            if ((UnityEngine.Object)PLServer.Instance != (UnityEngine.Object)null && !___playedCutscene && !__instance.GetIsPlayerShip())
                ___playedCutscene = true;
            if ((UnityEngine.Object)PLServer.Instance != (UnityEngine.Object)null && (bool)PLServer.Instance.PTCountdownArmed && PLServer.GetCurrentSector() != null && !__instance.GetIsPlayerShip())
                PLServer.Instance.PTCountdownArmed = (ObscuredBool)false;
            if ((UnityEngine.Object)PLGameStatic.Instance != (UnityEngine.Object)null && (UnityEngine.Object)PLServer.Instance != (UnityEngine.Object)null && !__instance.GetIsPlayerShip())
            {
                if (PhotonNetwork.isMasterClient)
                {
                    foreach (PLPawn allPawn in PLGameStatic.Instance.AllPawns)
                    {
                        if ((UnityEngine.Object)allPawn.GetPlayer() != (UnityEngine.Object)null && !allPawn.IsDead)
                        {
                            if (allPawn.GetPlayer().StartingShip.ShipID == __instance.ShipID && allPawn.GetPlayer().IsBot)
                            {
                                allPawn.GetPlayer().photonView.RPC("SetAIRace", PhotonTargets.MasterClient, (object)2);
                                allPawn.GetPlayer().photonView.RPC("SetAIGender", PhotonTargets.MasterClient, (object)true);
                            }
                        }
                    }
                }
            }
            return true;

        }
    }
}
