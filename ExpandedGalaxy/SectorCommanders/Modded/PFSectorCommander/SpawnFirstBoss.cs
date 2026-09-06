using HarmonyLib;
using PulsarModLoader.Patches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLAlchemistEncounter), "PlayerEnter")]
    internal class SpawnFirstBoss
    {
        internal static bool HelperMethod(PLSectorInfo pLSectorInfo)
        {
            if (pLSectorInfo != null && pLSectorInfo.MySPI.Faction == 5)
                return true;
            return false;
        }

        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> list = instructions.ToList();

            Label failed = generator.DefineLabel();

            List<CodeInstruction> targetSequence = new List<CodeInstruction>()
            {
                new CodeInstruction(OpCodes.Ldloc_0),
                new CodeInstruction(OpCodes.Brfalse)
            };
            List<CodeInstruction> patchSequence = new List<CodeInstruction>()
                {
                    new CodeInstruction(OpCodes.Ldloc_0),
                    CodeInstruction.Call(typeof(ExpandedGalaxy.SpawnFirstBoss), "HelperMethod", new Type[1]
                    {
                        typeof(PLSectorInfo)
                    }),
                    new CodeInstruction(OpCodes.Brfalse_S, failed),
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Ldarg_1),
                    CodeInstruction.Call(typeof(PLPersistantEncounterInstance), "PlayerEnter", new Type[1]
                    {
                        typeof(int)
                    }),
                    new CodeInstruction(OpCodes.Ret),
                    new CodeInstruction(OpCodes.Nop)
                };
            patchSequence[7].labels.Add(failed);

            return HarmonyHelpers.PatchBySequence(list.AsEnumerable<CodeInstruction>(), targetSequence, patchSequence, HarmonyHelpers.PatchMode.AFTER, HarmonyHelpers.CheckMode.NONNULL, false);
        }

        private static void Postfix(PLAlchemistEncounter __instance, int inHubID)
        {
            PLSectorInfo sectorWithId = PLServer.GetSectorWithID(inHubID);
            if (sectorWithId != null && sectorWithId.MySPI.Faction == 5)
            {
                PFSectorCommanderUpdate.ambientMusic = false;
                PFSectorCommanderUpdate.bossMusic = false;
                BossPT4.LastEMPTime = -1f;
                BossPT5.LastEMPTime = -1f;
                if (PFSectorCommander.bossFlag != 0 || !PhotonNetwork.isMasterClient)
                    return;
                List<PLPersistantShipInfo> pLPersistantShipInfos = new List<PLPersistantShipInfo>();
                foreach (PLPersistantShipInfo allPsI in PLServer.Instance.AllPSIs)
                {
                    if (allPsI != null && allPsI.MyCurrentSector == sectorWithId)
                        pLPersistantShipInfos.Add(allPsI);
                }
                if (pLPersistantShipInfos.Count != 0)
                {
                    foreach (PLPersistantShipInfo PsI in pLPersistantShipInfos)
                        PLServer.Instance.AllPSIs.Remove(PsI);
                }
                PLPersistantShipInfo bossInfo = new PLPersistantShipInfo(EShipType.E_POLYTECH_SHIP, 5, sectorWithId, isFlagged: true)
                {
                    ForcedHostile = true,
                    ShipName = "The Recompiler: Config. 1",
                    HullPercent = 1f,
                    ShldPercent = 2f,
                    SelectedActorID = "ExGal_Recompiler_1"
                };
                bossInfo.CompOverrides.AddRange((IEnumerable<ComponentOverrideData>)PFSectorCommander.GetComponentsFromIteration(0));
                PLServer.Instance.AllPSIs.Add(bossInfo);
                bossInfo.CreateShipInstance(__instance);
                PLShipInfo info = (PLShipInfo)bossInfo.ShipInstance;
                info.MyStats.RemoveShipComponent(info.MyStats.GetShipComponent<PLShipComponent>(ESlotType.E_COMP_MAINTURRET));
                info.DropScrap = false;
                info.CreditsLeftBehind = 0;
                PFSectorCommander.bossFlag = 1;

                if ((double)(float)PLServer.Instance.ChaosLevel >= 9.0)
                {
                    if (PLGlobal.Instance.Galaxy.GenerationSettings.CreateDataString() == PLGlobal.Instance.Galaxy.VeteranGenSettings.CreateDataString())
                    {
                        PLServer.Instance.StartPickupMission(8000020, true);
                    }
                }
                if ((int)PLServer.Instance.CrewFactionID == 5)
                {
                    PLServer.Instance.StartPickupMission(8000021, true);
                }
            }
        }
    }
}
