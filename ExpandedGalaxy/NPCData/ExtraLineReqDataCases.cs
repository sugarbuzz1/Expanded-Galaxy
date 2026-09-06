using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(LineRequirementData), "Passes")]
    internal class ExtraLineReqDataCases
    {
        private static void Postfix(LineRequirementData __instance, ref PLDialogueActorInstance dai, ref PLPersistantShipInfo optionalPSIContext, ref PLFluffyRankingUI optionalFluffyRankingUI, ref bool __result)
        {
            int result;
            if (!int.TryParse(__instance.Parameter, out result))
            {
                result = -1;
            }
            if (!(PLServer.Instance != null))
            {
                __result = true;
                return;
            }
            switch (__instance.Type)
            {
                case "38":
                    __result = PLServer.Instance.BiscuitContestIsOver.GetDecrypted() && !PLServer.Instance.PlayerCrew_WonFBContest.GetDecrypted();
                    break;
            }
        }
    }
}
