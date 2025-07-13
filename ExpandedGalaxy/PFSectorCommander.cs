using CodeStage.AntiCheat.ObscuredTypes;
using HarmonyLib;
using PulsarModLoader.Content.Components.CPU;
using PulsarModLoader.Content.Components.HullPlating;
using PulsarModLoader.Content.Components.MegaTurret;
using PulsarModLoader.Content.Components.Missile;
using PulsarModLoader.Content.Components.PolytechModule;
using PulsarModLoader.Content.Components.Turret;
using PulsarModLoader.Content.Components.WarpDriveProgram;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ExpandedGalaxy
{
    internal class PFSectorCommander
    {
        internal static int bossFlag = 0;

        [HarmonyPatch(typeof(PLAlchemistEncounter), "PlayerEnter")]
        internal class SpawnFirstBoss
        {
            private static void Postfix(PLAlchemistEncounter __instance, int inHubID)
            {
                PLSectorInfo sectorWithId = PLServer.GetSectorWithID(inHubID);
                if (sectorWithId != null && sectorWithId.MySPI.Faction == 5)
                {
                    __instance.MyPersistantData.SpecialNetObjectPersistantData.Clear();
                    PFSectorCommanderUpdate.ambientMusic = false;
                    PFSectorCommanderUpdate.bossMusic = false;
                    PTModules.BossPT4.LastEMPTime = -1f;
                    PTModules.BossPT5.LastEMPTime = -1f;
                    if (bossFlag != 0 || !PhotonNetwork.isMasterClient)
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
                        ShipName = "The Recompiler Config: 1",
                        HullPercent = 1f,
                        ShldPercent = 2f,
                    };
                    bossInfo.CompOverrides.AddRange((IEnumerable<ComponentOverrideData>)PFSectorCommander.GetComponentsFromIteration(0));
                    PLServer.Instance.AllPSIs.Add(bossInfo);
                    PLShipInfo info = (PLShipInfo)__instance.SpawnEnemyShip(bossInfo.Type, bossInfo);
                    info.MyStats.RemoveShipComponent(info.MyStats.GetShipComponent<PLShipComponent>(ESlotType.E_COMP_MAINTURRET));
                    info.DropScrap = false;
                    info.CreditsLeftBehind = 0;
                    PFSectorCommander.bossFlag = 1;
                }
            }
        }

        [HarmonyPatch(typeof(PLPersistantEncounterInstance), "Update")]
        internal class PFSectorCommanderUpdate
        {
            internal static bool bossMusic;
            internal static bool ambientMusic;

            private static Vector3 lastBossLoc;

            private static void Postfix(PLPersistantEncounterInstance __instance)
            {
                if (!(PLEncounterManager.Instance.GetCurrentPersistantEncounterInstance() != null) || PLServer.GetSectorWithID(__instance.GetSectorID()) == null || PLEncounterManager.Instance.PlayerShip == null)
                    return;
                PLSectorInfo sectorInfo = PLServer.GetSectorWithID(__instance.GetSectorID());
                if (!(sectorInfo.VisualIndication == ESectorVisualIndication.ALCHEMIST && sectorInfo.MySPI.Faction == 5))
                    return;

                
                bool playerInEncounter = PLEncounterManager.Instance.GetCurrentPersistantEncounterInstance().GetSectorID() == __instance.GetSectorID();
                if ((bool)playerInEncounter && !PLEncounterManager.Instance.PlayerShip.Get_IsInWarpMode())
                {
                    if (bossFlag == 6 && bossMusic)
                    {
                        bossMusic = false;
                        PLMusic.Instance.StopCurrentMusic();
                    }
                    if (!bossMusic && bossFlag != 6)
                    {
                        bossMusic = true;
                        PLMusic.Instance.PlayMusic("mx_Polytechnic_Attack", true, false, true, true);
                        return;
                    }
                    if (bossFlag == 6 && !ambientMusic)
                    {
                        ambientMusic = true;
                        PLMusic.Instance.PlayMusic("mx_Polytechnic_Ambient", true, false, true, true);
                        return;
                    }
                }
            }
        }

        [HarmonyPatch(typeof(PLShipInfoBase), "LeaveExtraScrap")]
        internal class DropPFCommanderLoot
        {
            private static void Postfix(PLShipInfoBase __instance, ref List<PLShipComponent> droppedShipComponents)
            {
                if (__instance.GetIsPlayerShip())
                    return;
                if (__instance.ShipTypeID == EShipType.E_POLYTECH_SHIP)
                {
                    int compHash = -1;
                    bool flag = true;
                    PLShipInfo info;
                    PLPersistantEncounterInstance encounter = PLEncounterManager.Instance.GetCPEI();
                    PLSectorInfo sectorInfo = PLServer.GetSectorWithID(encounter.GetSectorID());
                    switch (__instance.ShipName.ToString())
                    {
                        case "The Recompiler Config: 1":
                            compHash = (int)PLShipComponent.createHashFromInfo(1, 20, 0, 0, (int)ESlotType.E_COMP_NONE);
                            PLPersistantShipInfo boss2 = new PLPersistantShipInfo(EShipType.E_POLYTECH_SHIP, 5, sectorInfo, isFlagged: true)
                            {
                                ForcedHostile = true,
                                ShipName = "The Recompiler Config: 2",
                                HullPercent = 1f,
                                ShldPercent = 2f,
                            };
                            boss2.CompOverrides.AddRange((IEnumerable<ComponentOverrideData>)PFSectorCommander.GetComponentsFromIteration(1));
                            PLServer.Instance.AllPSIs.Add(boss2);
                            info = (PLShipInfo)encounter.SpawnEnemyShip(boss2.Type, boss2, spawnPos: __instance.Exterior.transform.position);
                            info.DropScrap = false;
                            info.CreditsLeftBehind = 0;
                            if (bossFlag < 2)
                                bossFlag = 2;
                            Systems.PhaseAway(info);
                            info.TargetShip = PLEncounterManager.Instance.PlayerShip;
                            break;
                        case "The Recompiler Config: 2":
                            compHash = (int)PLShipComponent.createHashFromInfo(16, HullPlatingModManager.Instance.GetHullPlatingIDFromName("NanoActivePlating"), 0, 0, (int)ESlotType.E_COMP_NONE);
                            PLPersistantShipInfo boss3 = new PLPersistantShipInfo(EShipType.E_POLYTECH_SHIP, 5, sectorInfo, isFlagged: true)
                            {
                                ForcedHostile = true,
                                ShipName = "The Recompiler Config: 3",
                                HullPercent = 1f,
                                ShldPercent = 2f,
                            };
                            boss3.CompOverrides.AddRange((IEnumerable<ComponentOverrideData>)PFSectorCommander.GetComponentsFromIteration(2));
                            PLServer.Instance.AllPSIs.Add(boss3);
                            info = (PLShipInfo)encounter.SpawnEnemyShip(boss3.Type, boss3, spawnPos: __instance.Exterior.transform.position);
                            info.DropScrap = false;
                            info.CreditsLeftBehind = 0;
                            if (bossFlag < 3)
                                bossFlag = 3;
                            Systems.PhaseAway(info);
                            info.TargetShip = PLEncounterManager.Instance.PlayerShip;
                            break;
                        case "The Recompiler Config: 3":
                            compHash = (int)PLShipComponent.createHashFromInfo(20, MissileModManager.Instance.GetMissileIDFromName("Seeker Missile"), 0, 0, (int)ESlotType.E_COMP_NONE);
                            PLPersistantShipInfo boss4 = new PLPersistantShipInfo(EShipType.E_POLYTECH_SHIP, 5, sectorInfo, isFlagged: true)
                            {
                                ForcedHostile = true,
                                ShipName = "The Recompiler Config: 4",
                                HullPercent = 1f,
                                ShldPercent = 2f,
                            };
                            boss4.CompOverrides.AddRange((IEnumerable<ComponentOverrideData>)PFSectorCommander.GetComponentsFromIteration(3));
                            PLServer.Instance.AllPSIs.Add(boss4);
                            info = (PLShipInfo)encounter.SpawnEnemyShip(boss4.Type, boss4, spawnPos: __instance.Exterior.transform.position);
                            info.DropScrap = false;
                            info.CreditsLeftBehind = 0;
                            Systems.PhaseAway(info);
                            if (bossFlag < 4)
                                bossFlag = 4;
                            info.TargetShip = PLEncounterManager.Instance.PlayerShip;
                            break;
                        case "The Recompiler Config: 4":
                            compHash = (int)PLShipComponent.createHashFromInfo(17, WarpDriveProgramModManager.Instance.GetWarpDriveProgramIDFromName("EMP"), 0, 0, (int)ESlotType.E_COMP_NONE);
                            PLPersistantShipInfo boss5 = new PLPersistantShipInfo(EShipType.E_POLYTECH_SHIP, 5, sectorInfo, isFlagged: true)
                            {
                                ForcedHostile = true,
                                ShipName = "The Recompiler Config: 5",
                                HullPercent = 1f,
                                ShldPercent = 2f,
                            };
                            boss5.CompOverrides.AddRange((IEnumerable<ComponentOverrideData>)PFSectorCommander.GetComponentsFromIteration(4));
                            PLServer.Instance.AllPSIs.Add(boss5);
                            info = (PLShipInfo)encounter.SpawnEnemyShip(boss5.Type, boss5, spawnPos: __instance.Exterior.transform.position);
                            info.DropScrap = false;
                            info.CreditsLeftBehind = 20000;
                            Systems.PhaseAway(info);
                            if (bossFlag < 5)
                                bossFlag = 5;
                            info.TargetShip = PLEncounterManager.Instance.PlayerShip;
                            break;
                        case "The Recompiler Config: 5":
                            compHash = (int)PLShipComponent.createHashFromInfo(10, TurretModManager.Instance.GetTurretIDFromName("Nanite Railgun"), 0, 0, (int)ESlotType.E_COMP_NONE);
                            if (bossFlag < 6)
                                bossFlag = 6;
                            break;
                        default:
                            flag = false;
                            break;
                    }
                    if (flag)
                    {
                        PLServer.Instance.photonView.RPC("CreateSpecificShipScrapAtLocation", PhotonTargets.All, __instance.Exterior.transform.position + UnityEngine.Random.onUnitSphere * 20f, __instance.Exterior.transform.position, (int)compHash, true);
                    }
                }
            }
        }

        [HarmonyPatch(typeof(PLShipInfoBase), "DestroySelf")]
        internal class ExtraScrapFix
        {
            private static bool Prefix(PLShipInfoBase __instance, PLShipInfoBase attackingShip)
            {
                if (__instance.HasBeenDestroyed)
                    return true;
                if (__instance.GetIsPlayerShip())
                    return true;
                if ((UnityEngine.Object)PLServer.Instance != (UnityEngine.Object)null && PhotonNetwork.isMasterClient)
                {
                    if (!__instance.DropScrap)
                    {
                        List<PLShipComponent> list = new List<PLShipComponent>();
                        __instance.LeaveExtraScrap(droppedShipComponents: ref list);
                    }
                }
                return true;
            }
        }

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

        [HarmonyPatch(typeof(PLPersistantEncounterInstance), "CreateAsteroids")]
        internal class NoAsteroids
        {
            private static bool Prefix(PLPersistantEncounterInstance __instance, int inHubID)
            {
                PLSectorInfo sectorWithId = PLServer.GetSectorWithID(inHubID);
                if (sectorWithId.MySPI.Faction == 5 && sectorWithId.VisualIndication == ESectorVisualIndication.ALCHEMIST)
                    return false;
                return true;
            }
        }

        [HarmonyPatch(typeof(PLGalaxy), "Setup")]
        internal class CreatePFCommanderSector
        {
            private static void Postfix(PLGalaxy __instance, ref PLRand ___m_RandomGenerator)
            {
                if (__instance.AllSectorInfos.Count == 0)
                    return;
                for (int index = 0; index < 3000; ++index)
                {
                    PLSectorInfo randomSectorInfo = __instance.GetRandomSectorInfo(___m_RandomGenerator.Next());
                    if (randomSectorInfo.VisualIndication == ESectorVisualIndication.NONE && randomSectorInfo.MySPI.Faction != 4)
                    {
                        randomSectorInfo.VisualIndication = ESectorVisualIndication.ALCHEMIST;
                        randomSectorInfo.MySPI.Faction = 5;
                        break;
                    }
                }
            }
        }

        [HarmonyPatch(typeof(PLGame), "SpawnSimpleCombatBotAtTransformForPlayer")]
        internal class PolyTechBots
        {
            private static bool Prefix(PLGame __instance, object[] __args)
            {
                PLPlayer newPlayer = (PLPlayer)__args[1];
                if ((UnityEngine.Object)newPlayer != (UnityEngine.Object)null)
                {
                    if ((UnityEngine.Object)newPlayer.StartingShip != (UnityEngine.Object)null)
                    {
                        if (newPlayer.StartingShip.ShipTypeID == EShipType.E_POLYTECH_SHIP && (newPlayer.RaceID != 2 || !newPlayer.Gender_IsMale))
                        {
                            newPlayer.SetGender(true);
                            newPlayer.SetRace(2);
                            newPlayer.SetAIGender(true);
                            newPlayer.SetAIRace(2);
                            __args[1] = newPlayer;
                            __args[2] = string.Intern("PLPawnAndroid");
                        }
                    }
                }

                return true;
            }
        }

        [HarmonyPatch(typeof(PLShipInfoBase), "IsTypeSectorCmdr")]
        internal class PFSectorCommanderYes
        {
            private static void Postfix(PLShipInfoBase __instance, EShipType shipType, ref bool __result)
            {
                if (shipType == EShipType.E_POLYTECH_SHIP)
                {
                    __result = true;
                }
            }
        }

        [HarmonyPatch(typeof(PLMissionObjective_DestroySectorCommanders), "CheckIfCompleted")]
        internal class PFSectorCommanderKill
        {
            private static bool Prefix(PLMissionObjective_DestroySectorCommanders __instance)
            {
                if (PhotonNetwork.isMasterClient && (double)UnityEngine.Random.value < 0.0099999997764825821)
                {
                    __instance.AmountCompleted = 0;
                    if ((UnityEngine.Object)PLServer.Instance != (UnityEngine.Object)null && (double)PLServer.Instance.lifetime > 5.0)
                    {
                        PLPersistantShipInfo psiWithShipType1 = PLServer.GetPSIWithShipType(EShipType.E_CORRUPTED_DRONE);
                        PLPersistantShipInfo psiWithShipType2 = PLServer.GetPSIWithShipType(EShipType.E_SWARM_CMDR);
                        PLPersistantShipInfo psiWithShipType3 = PLServer.GetPSIWithShipType(EShipType.E_INTREPID_SC);
                        PLPersistantShipInfo psiWithShipType4 = PLServer.GetPSIWithShipType(EShipType.E_ALCHEMIST);
                        PLPersistantShipInfo psiWithShipType5 = PLServer.GetPSIWithShipType(EShipType.E_DEATHSEEKER_DRONE_SC);
                        if (PLServer.DoesPEIOfTypeExist_AndBeenVisited(ESectorVisualIndication.ANCIENT_SENTRY))
                            __instance.AmountCompleted += psiWithShipType1 == null || psiWithShipType1.IsShipDestroyed ? 1 : 0;
                        if (PLServer.DoesPEIOfTypeExist_AndBeenVisited(ESectorVisualIndication.SWARM_CMDR))
                            __instance.AmountCompleted += psiWithShipType2 == null || psiWithShipType2.IsShipDestroyed ? 1 : 0;
                        if (PLServer.DoesPEIOfTypeExist_AndBeenVisited(ESectorVisualIndication.INTREPID_SECTOR_CMDR))
                            __instance.AmountCompleted += psiWithShipType3 == null || psiWithShipType3.IsShipDestroyed ? 1 : 0;
                        if (PLServer.DoesPEIOfTypeExist_AndBeenVisited(ESectorVisualIndication.ALCHEMIST))
                        {
                            foreach (PLPersistantEncounterInstance encounterInstance in PLEncounterManager.Instance.AllPersistantEncounterInstances.Values)
                            {
                                if (encounterInstance != null)
                                {
                                    int sectorId = encounterInstance.GetSectorID();
                                    if ((UnityEngine.Object)PLGlobal.Instance.Galaxy != (UnityEngine.Object)null && PLGlobal.Instance.Galaxy.AllSectorInfos.ContainsKey(sectorId))
                                    {
                                        PLSectorInfo allSectorInfo = PLGlobal.Instance.Galaxy.AllSectorInfos[sectorId];
                                        if (allSectorInfo != null && allSectorInfo.VisualIndication == ESectorVisualIndication.ALCHEMIST)
                                        {
                                            if (allSectorInfo.MySPI.Faction == 5)
                                                __instance.AmountCompleted += bossFlag == 6 ? 1 : 0;
                                            else
                                                __instance.AmountCompleted += psiWithShipType4 == null || psiWithShipType4.IsShipDestroyed ? 1 : 0;
                                        }
                                    }
                                }
                            }
                        }
                        if (PLServer.DoesPEIOfTypeExist_AndBeenVisited(ESectorVisualIndication.DEATHSEEKER_COMMANDER))
                            __instance.AmountCompleted += psiWithShipType5 == null || psiWithShipType5.IsShipDestroyed ? 1 : 0;
                    }
                    __instance.AmountCompleted = Mathf.Min(__instance.AmountCompleted, __instance.AmountNeeded);
                }
                if (__instance.AmountCompleted < __instance.AmountNeeded)
                    return false;
                __instance.IsCompleted = true;
                return false;
            }
        }

        private static List<ComponentOverrideData> GetComponentsFromIteration(int iteration)
        {
            List<ComponentOverrideData> PFCommanderParts = new List<ComponentOverrideData>();

            switch (iteration)
            {
                case 0:
                    PFCommanderParts.Add
                    (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_SHLD,
                            CompSubType = (int)EShieldGeneratorType.E_POLYTECH_SHIELDS,
                            ReplaceExistingComp = true,
                            CompLevel = 4 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_SHLD,
                            SlotNumberToReplace = 0
                        }
                    );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_REACTOR,
                            CompSubType = (int)EReactorType.E_POLYTECH_ORIGINAL,
                            ReplaceExistingComp = true,
                            CompLevel = 4 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_REACTOR,
                            SlotNumberToReplace = 0
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_HULL,
                            CompSubType = (int)EHullType.E_POLYTECH_HULL,
                            ReplaceExistingComp = true,
                            CompLevel = 4 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_HULL,
                            SlotNumberToReplace = 0
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_CPU,
                            CompSubType = (int)ECPUClass.E_CPUTYPE_CYBER_DEF,
                            ReplaceExistingComp = true,
                            CompLevel = 4 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_CPU,
                            SlotNumberToReplace = 0
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_CPU,
                            CompSubType = (int)ECPUClass.E_CPUTYPE_CYBER_DEF,
                            ReplaceExistingComp = true,
                            CompLevel = 4 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_CPU,
                            SlotNumberToReplace = 1
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_CPU,
                            CompSubType = (int)ECPUClass.E_CPUTYPE_SHIELD_COPROCESSOR,
                            ReplaceExistingComp = true,
                            CompLevel = 4 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_CPU,
                            SlotNumberToReplace = 2
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_CPU,
                            CompSubType = (int)ECPUClass.E_CPUTYPE_SHIELD_COPROCESSOR,
                            ReplaceExistingComp = true,
                            CompLevel = 4 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_CPU,
                            SlotNumberToReplace = 3
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_TURRET,
                            CompSubType = TurretModManager.Instance.GetTurretIDFromName("Nanite Railgun"),
                            ReplaceExistingComp = true,
                            CompLevel = 4 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_TURRET,
                            SlotNumberToReplace = 0
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_MAINTURRET,
                            CompSubType = MegaTurretModManager.Instance.GetMegaTurretIDFromName("The Disassembler"),
                            ReplaceExistingComp = true,
                            CompLevel = 4 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_MAINTURRET,
                            SlotNumberToReplace = 0
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_PROGRAM,
                            CompSubType = (int)EWarpDriveProgramType.REMOVE_ALL_VIRUSES,
                            ReplaceExistingComp = true,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_PROGRAM,
                            SlotNumberToReplace = 0
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_PROGRAM,
                            CompSubType = (int)EWarpDriveProgramType.SUPER_SHIELD_BOOSTER,
                            ReplaceExistingComp = true,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_PROGRAM,
                            SlotNumberToReplace = 1
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_PROGRAM,
                            CompSubType = (int)EWarpDriveProgramType.SUPER_SHIELD_BOOSTER,
                            ReplaceExistingComp = true,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_PROGRAM,
                            SlotNumberToReplace = 2
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_PROGRAM,
                            CompSubType = (int)EWarpDriveProgramType.BARRAGE,
                            ReplaceExistingComp = true,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_PROGRAM,
                            SlotNumberToReplace = 3
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_PROGRAM,
                            CompSubType = (int)EWarpDriveProgramType.QUANTUM_DEFENSES,
                            ReplaceExistingComp = true,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_PROGRAM,
                            SlotNumberToReplace = 4
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_SENS,
                            CompSubType = (int)0,
                            ReplaceExistingComp = true,
                            CompLevel = 4 + Mathf.FloorToInt((float)PLServer.Instance.ChaosLevel / 3f),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_SENS,
                            SlotNumberToReplace = 0
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_THRUSTER,
                            CompSubType = (int)EThrusterType.E_THRUSTER_PERF,
                            ReplaceExistingComp = true,
                            CompLevel = 3 + Mathf.FloorToInt((float)PLServer.Instance.ChaosLevel / 3f),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_THRUSTER,
                            SlotNumberToReplace = 0
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_THRUSTER,
                            CompSubType = (int)EThrusterType.E_THRUSTER_PERF,
                            ReplaceExistingComp = true,
                            CompLevel = 3 + Mathf.FloorToInt((float)PLServer.Instance.ChaosLevel / 3f),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_THRUSTER,
                            SlotNumberToReplace = 1
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_INERTIA_THRUSTER,
                            CompSubType = (int)EInertiaThrusterType.E_NORMAL,
                            ReplaceExistingComp = true,
                            CompLevel = 3 + Mathf.FloorToInt((float)PLServer.Instance.ChaosLevel / 3f),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_INERTIA_THRUSTER,
                            SlotNumberToReplace = 0
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_MANEUVER_THRUSTER,
                            CompSubType = (int)EManeuverThrusterType.E_NORMAL,
                            ReplaceExistingComp = true,
                            CompLevel = 3 + Mathf.FloorToInt((float)PLServer.Instance.ChaosLevel / 3f),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_MANEUVER_THRUSTER,
                            SlotNumberToReplace = 0
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_POLYTECH_MODULE,
                            CompSubType = PolytechModuleModManager.Instance.GetPolytechModuleIDFromName("P.T. Module: Recombiner 1"),
                            ReplaceExistingComp = true,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_PROGRAM,
                            SlotNumberToReplace = 4
                        }
                        );
                    break;
                case 1:
                    PFCommanderParts.Add
                    (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_SHLD,
                            CompSubType = (int)EShieldGeneratorType.E_POLYTECH_SHIELDS,
                            ReplaceExistingComp = true,
                            CompLevel = 4 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_SHLD,
                            SlotNumberToReplace = 0
                        }
                    );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_REACTOR,
                            CompSubType = (int)EReactorType.E_POLYTECH_ORIGINAL,
                            ReplaceExistingComp = true,
                            CompLevel = 4 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_REACTOR,
                            SlotNumberToReplace = 0
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_HULL,
                            CompSubType = (int)EHullType.E_POLYTECH_HULL,
                            ReplaceExistingComp = true,
                            CompLevel = 4 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_HULL,
                            SlotNumberToReplace = 0
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_CPU,
                            CompSubType = (int)ECPUClass.E_CPUTYPE_CYBER_DEF,
                            ReplaceExistingComp = true,
                            CompLevel = 4 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_CPU,
                            SlotNumberToReplace = 0
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_CPU,
                            CompSubType = (int)ECPUClass.E_CPUTYPE_CYBER_DEF,
                            ReplaceExistingComp = true,
                            CompLevel = 4 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_CPU,
                            SlotNumberToReplace = 1
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_CPU,
                            CompSubType = (int)ECPUClass.E_CPUTYPE_SHIELD_COPROCESSOR,
                            ReplaceExistingComp = true,
                            CompLevel = 4 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_CPU,
                            SlotNumberToReplace = 2
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_CPU,
                            CompSubType = (int)ECPUClass.E_CPUTYPE_SHIELD_COPROCESSOR,
                            ReplaceExistingComp = true,
                            CompLevel = 4 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_CPU,
                            SlotNumberToReplace = 3
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_CPU,
                            CompSubType = (int)ECPUClass.E_CPUTYPE_TARGETING,
                            ReplaceExistingComp = true,
                            CompLevel = 4 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_CPU,
                            SlotNumberToReplace = 3
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_TURRET,
                            CompSubType = TurretModManager.Instance.GetTurretIDFromName("Nanite Railgun"),
                            ReplaceExistingComp = true,
                            CompLevel = 4 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_TURRET,
                            SlotNumberToReplace = 0
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_MAINTURRET,
                            CompSubType = MegaTurretModManager.Instance.GetMegaTurretIDFromName("The Disassembler"),
                            ReplaceExistingComp = true,
                            CompLevel = 4 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_MAINTURRET,
                            SlotNumberToReplace = 0
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_PROGRAM,
                            CompSubType = (int)EWarpDriveProgramType.REMOVE_ALL_VIRUSES,
                            ReplaceExistingComp = true,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_PROGRAM,
                            SlotNumberToReplace = 0
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_PROGRAM,
                            CompSubType = (int)EWarpDriveProgramType.SUPER_SHIELD_BOOSTER,
                            ReplaceExistingComp = true,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_PROGRAM,
                            SlotNumberToReplace = 1
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_PROGRAM,
                            CompSubType = (int)EWarpDriveProgramType.SUPER_SHIELD_BOOSTER,
                            ReplaceExistingComp = true,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_PROGRAM,
                            SlotNumberToReplace = 2
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_PROGRAM,
                            CompSubType = (int)EWarpDriveProgramType.BARRAGE,
                            ReplaceExistingComp = true,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_PROGRAM,
                            SlotNumberToReplace = 3
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_PROGRAM,
                            CompSubType = (int)EWarpDriveProgramType.QUANTUM_DEFENSES,
                            ReplaceExistingComp = true,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_PROGRAM,
                            SlotNumberToReplace = 4
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_SENS,
                            CompSubType = (int)0,
                            ReplaceExistingComp = true,
                            CompLevel = 4 + Mathf.FloorToInt((float)PLServer.Instance.ChaosLevel / 3f),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_SENS,
                            SlotNumberToReplace = 0
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_THRUSTER,
                            CompSubType = (int)EThrusterType.E_THRUSTER_PERF,
                            ReplaceExistingComp = true,
                            CompLevel = 3 + Mathf.FloorToInt((float)PLServer.Instance.ChaosLevel / 3f),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_THRUSTER,
                            SlotNumberToReplace = 0
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_THRUSTER,
                            CompSubType = (int)EThrusterType.E_THRUSTER_PERF,
                            ReplaceExistingComp = true,
                            CompLevel = 3 + Mathf.FloorToInt((float)PLServer.Instance.ChaosLevel / 3f),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_THRUSTER,
                            SlotNumberToReplace = 1
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_INERTIA_THRUSTER,
                            CompSubType = (int)EInertiaThrusterType.E_NORMAL,
                            ReplaceExistingComp = true,
                            CompLevel = 3 + Mathf.FloorToInt((float)PLServer.Instance.ChaosLevel / 3f),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_INERTIA_THRUSTER,
                            SlotNumberToReplace = 0
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_MANEUVER_THRUSTER,
                            CompSubType = (int)EManeuverThrusterType.E_NORMAL,
                            ReplaceExistingComp = true,
                            CompLevel = 3 + Mathf.FloorToInt((float)PLServer.Instance.ChaosLevel / 3f),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_MANEUVER_THRUSTER,
                            SlotNumberToReplace = 0
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_POLYTECH_MODULE,
                            CompSubType = PolytechModuleModManager.Instance.GetPolytechModuleIDFromName("P.T. Module: Recombiner 1"),
                            ReplaceExistingComp = true,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_POLYTECH_MODULE,
                            SlotNumberToReplace = 0
                        }
                        );
                    break;
                case 2:
                    PFCommanderParts.Add
                    (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_SHLD,
                            CompSubType = (int)EShieldGeneratorType.E_POLYTECH_SHIELDS,
                            ReplaceExistingComp = true,
                            CompLevel = 4 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_SHLD,
                            SlotNumberToReplace = 0
                        }
                    );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_REACTOR,
                            CompSubType = (int)EReactorType.E_POLYTECH_ORIGINAL,
                            ReplaceExistingComp = true,
                            CompLevel = 4 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_REACTOR,
                            SlotNumberToReplace = 0
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_HULL,
                            CompSubType = (int)EHullType.E_POLYTECH_HULL,
                            ReplaceExistingComp = true,
                            CompLevel = 4 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_HULL,
                            SlotNumberToReplace = 0
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_CPU,
                            CompSubType = (int)ECPUClass.E_CPUTYPE_CYBER_DEF,
                            ReplaceExistingComp = true,
                            CompLevel = 4 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_CPU,
                            SlotNumberToReplace = 0
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_CPU,
                            CompSubType = (int)ECPUClass.E_CPUTYPE_CYBER_DEF,
                            ReplaceExistingComp = true,
                            CompLevel = 4 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_CPU,
                            SlotNumberToReplace = 1
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_CPU,
                            CompSubType = (int)ECPUClass.E_CPUTYPE_SHIELD_COPROCESSOR,
                            ReplaceExistingComp = true,
                            CompLevel = 4 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_CPU,
                            SlotNumberToReplace = 2
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_CPU,
                            CompSubType = (int)ECPUClass.E_CPUTYPE_SHIELD_COPROCESSOR,
                            ReplaceExistingComp = true,
                            CompLevel = 4 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_CPU,
                            SlotNumberToReplace = 3
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_CPU,
                            CompSubType = (int)ECPUClass.E_CPUTYPE_TARGETING,
                            ReplaceExistingComp = true,
                            CompLevel = 4 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_CPU,
                            SlotNumberToReplace = 4
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_CPU,
                            CompSubType = (int)ECPUClass.E_CPUTYPE_TARGETING,
                            ReplaceExistingComp = true,
                            CompLevel = 4 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_CPU,
                            SlotNumberToReplace = 5
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_CPU,
                            CompSubType = (int)ECPUClass.CYBERWARFARE_MODULE,
                            ReplaceExistingComp = true,
                            CompLevel = 4 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_CPU,
                            SlotNumberToReplace = 6
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_TURRET,
                            CompSubType = TurretModManager.Instance.GetTurretIDFromName("Nanite Railgun"),
                            ReplaceExistingComp = true,
                            CompLevel = 4 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_TURRET,
                            SlotNumberToReplace = 0
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_TURRET,
                            CompSubType = (int)ETurretType.MISSILE,
                            ReplaceExistingComp = true,
                            CompLevel = 4 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 6) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_TURRET,
                            SlotNumberToReplace = 1
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_TRACKERMISSILE,
                            CompSubType = TurretModManager.Instance.GetTurretIDFromName("Seeker Missile"),
                            ReplaceExistingComp = true,
                            CompLevel = 4 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_TRACKERMISSILE,
                            SlotNumberToReplace = 0
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_MAINTURRET,
                            CompSubType = MegaTurretModManager.Instance.GetMegaTurretIDFromName("The Disassembler"),
                            ReplaceExistingComp = true,
                            CompLevel = 4 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_MAINTURRET,
                            SlotNumberToReplace = 0
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_PROGRAM,
                            CompSubType = (int)EWarpDriveProgramType.REMOVE_ALL_VIRUSES,
                            ReplaceExistingComp = true,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_PROGRAM,
                            SlotNumberToReplace = 0
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_PROGRAM,
                            CompSubType = (int)EWarpDriveProgramType.SUPER_SHIELD_BOOSTER,
                            ReplaceExistingComp = true,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_PROGRAM,
                            SlotNumberToReplace = 1
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_PROGRAM,
                            CompSubType = (int)EWarpDriveProgramType.SUPER_SHIELD_BOOSTER,
                            ReplaceExistingComp = true,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_PROGRAM,
                            SlotNumberToReplace = 2
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_PROGRAM,
                            CompSubType = (int)EWarpDriveProgramType.BARRAGE,
                            ReplaceExistingComp = true,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_PROGRAM,
                            SlotNumberToReplace = 3
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_PROGRAM,
                            CompSubType = (int)EWarpDriveProgramType.QUANTUM_DEFENSES,
                            ReplaceExistingComp = true,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_PROGRAM,
                            SlotNumberToReplace = 4
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_PROGRAM,
                            CompSubType = (int)EWarpDriveProgramType.FULL_HEAL_ALL_SYSTEMS,
                            ReplaceExistingComp = true,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_PROGRAM,
                            SlotNumberToReplace = 5
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_PROGRAM,
                            CompSubType = (int)EWarpDriveProgramType.VIRUS_BOOSTER,
                            ReplaceExistingComp = true,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_PROGRAM,
                            SlotNumberToReplace = 6
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_PROGRAM,
                            CompSubType = (int)EWarpDriveProgramType.SYBER_SHEILD,
                            ReplaceExistingComp = true,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_PROGRAM,
                            SlotNumberToReplace = 7
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_PROGRAM,
                            CompSubType = (int)EWarpDriveProgramType.HULL_POWERED_ARMOR_FLAW,
                            ReplaceExistingComp = true,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_PROGRAM,
                            SlotNumberToReplace = 8
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_PROGRAM,
                            CompSubType = (int)EWarpDriveProgramType.PHALANX_VIRUS_PROGRAM,
                            ReplaceExistingComp = true,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_PROGRAM,
                            SlotNumberToReplace = 9
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_SENS,
                            CompSubType = (int)0,
                            ReplaceExistingComp = true,
                            CompLevel = 4 + Mathf.FloorToInt((float)PLServer.Instance.ChaosLevel / 3f),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_SENS,
                            SlotNumberToReplace = 0
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_THRUSTER,
                            CompSubType = (int)EThrusterType.E_THRUSTER_PERF,
                            ReplaceExistingComp = true,
                            CompLevel = 3 + Mathf.FloorToInt((float)PLServer.Instance.ChaosLevel / 3f),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_THRUSTER,
                            SlotNumberToReplace = 0
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_THRUSTER,
                            CompSubType = (int)EThrusterType.E_THRUSTER_PERF,
                            ReplaceExistingComp = true,
                            CompLevel = 3 + Mathf.FloorToInt((float)PLServer.Instance.ChaosLevel / 3f),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_THRUSTER,
                            SlotNumberToReplace = 1
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_INERTIA_THRUSTER,
                            CompSubType = (int)EInertiaThrusterType.E_NORMAL,
                            ReplaceExistingComp = true,
                            CompLevel = 3 + Mathf.FloorToInt((float)PLServer.Instance.ChaosLevel / 3f),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_INERTIA_THRUSTER,
                            SlotNumberToReplace = 0
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_MANEUVER_THRUSTER,
                            CompSubType = (int)EManeuverThrusterType.E_NORMAL,
                            ReplaceExistingComp = true,
                            CompLevel = 3 + Mathf.FloorToInt((float)PLServer.Instance.ChaosLevel / 3f),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_MANEUVER_THRUSTER,
                            SlotNumberToReplace = 0
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_POLYTECH_MODULE,
                            CompSubType = PolytechModuleModManager.Instance.GetPolytechModuleIDFromName("P.T. Module: Recompiler 1"),
                            ReplaceExistingComp = true,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_POLYTECH_MODULE,
                            SlotNumberToReplace = 0
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_POLYTECH_MODULE,
                            CompSubType = PolytechModuleModManager.Instance.GetPolytechModuleIDFromName("P.T. Module: Recompiler 3"),
                            ReplaceExistingComp = true,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_POLYTECH_MODULE,
                            SlotNumberToReplace = 1
                        }
                        );
                    break;
                case 3:
                    PFCommanderParts.Add
                    (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_SHLD,
                            CompSubType = (int)EShieldGeneratorType.E_POLYTECH_SHIELDS,
                            ReplaceExistingComp = true,
                            CompLevel = 5 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_SHLD,
                            SlotNumberToReplace = 0
                        }
                    );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_REACTOR,
                            CompSubType = (int)EReactorType.E_POLYTECH_ORIGINAL,
                            ReplaceExistingComp = true,
                            CompLevel = 5 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_REACTOR,
                            SlotNumberToReplace = 0
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_HULL,
                            CompSubType = (int)EHullType.E_POLYTECH_HULL,
                            ReplaceExistingComp = true,
                            CompLevel = 5 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_HULL,
                            SlotNumberToReplace = 0
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_CPU,
                            CompSubType = (int)ECPUClass.E_CPUTYPE_CYBER_DEF,
                            ReplaceExistingComp = true,
                            CompLevel = 5 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_CPU,
                            SlotNumberToReplace = 0
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_CPU,
                            CompSubType = (int)ECPUClass.E_CPUTYPE_CYBER_DEF,
                            ReplaceExistingComp = true,
                            CompLevel = 5 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_CPU,
                            SlotNumberToReplace = 1
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_CPU,
                            CompSubType = (int)ECPUClass.E_CPUTYPE_SHIELD_COPROCESSOR,
                            ReplaceExistingComp = true,
                            CompLevel = 5 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_CPU,
                            SlotNumberToReplace = 2
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_CPU,
                            CompSubType = (int)ECPUClass.E_CPUTYPE_SHIELD_COPROCESSOR,
                            ReplaceExistingComp = true,
                            CompLevel = 5 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_CPU,
                            SlotNumberToReplace = 3
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_CPU,
                            CompSubType = (int)ECPUClass.E_CPUTYPE_TARGETING,
                            ReplaceExistingComp = true,
                            CompLevel = 5 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_CPU,
                            SlotNumberToReplace = 4
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_CPU,
                            CompSubType = (int)ECPUClass.E_CPUTYPE_TARGETING,
                            ReplaceExistingComp = true,
                            CompLevel = 5 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_CPU,
                            SlotNumberToReplace = 5
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_CPU,
                            CompSubType = (int)ECPUClass.CYBERWARFARE_MODULE,
                            ReplaceExistingComp = true,
                            CompLevel = 5 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_CPU,
                            SlotNumberToReplace = 6
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_CPU,
                            CompSubType = (int)ECPUClass.SKUNK,
                            ReplaceExistingComp = true,
                            CompLevel = 5 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_CPU,
                            SlotNumberToReplace = 7
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_TURRET,
                            CompSubType = TurretModManager.Instance.GetTurretIDFromName("Nanite Railgun"),
                            ReplaceExistingComp = true,
                            CompLevel = 5 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_TURRET,
                            SlotNumberToReplace = 0
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_TURRET,
                            CompSubType = (int)ETurretType.MISSILE,
                            ReplaceExistingComp = true,
                            CompLevel = 5 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 6) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_TURRET,
                            SlotNumberToReplace = 1
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_TRACKERMISSILE,
                            CompSubType = TurretModManager.Instance.GetTurretIDFromName("Seeker Missile"),
                            ReplaceExistingComp = true,
                            CompLevel = 5 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_TRACKERMISSILE,
                            SlotNumberToReplace = 0
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_MAINTURRET,
                            CompSubType = MegaTurretModManager.Instance.GetMegaTurretIDFromName("The Disassembler"),
                            ReplaceExistingComp = true,
                            CompLevel = 5 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_MAINTURRET,
                            SlotNumberToReplace = 0
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_PROGRAM,
                            CompSubType = (int)EWarpDriveProgramType.REMOVE_ALL_VIRUSES,
                            ReplaceExistingComp = true,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_PROGRAM,
                            SlotNumberToReplace = 0
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_PROGRAM,
                            CompSubType = (int)EWarpDriveProgramType.SUPER_SHIELD_BOOSTER,
                            ReplaceExistingComp = true,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_PROGRAM,
                            SlotNumberToReplace = 1
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_PROGRAM,
                            CompSubType = (int)EWarpDriveProgramType.SUPER_SHIELD_BOOSTER,
                            ReplaceExistingComp = true,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_PROGRAM,
                            SlotNumberToReplace = 2
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_PROGRAM,
                            CompSubType = (int)EWarpDriveProgramType.BARRAGE,
                            ReplaceExistingComp = true,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_PROGRAM,
                            SlotNumberToReplace = 3
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_PROGRAM,
                            CompSubType = (int)EWarpDriveProgramType.QUANTUM_DEFENSES,
                            ReplaceExistingComp = true,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_PROGRAM,
                            SlotNumberToReplace = 4
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_PROGRAM,
                            CompSubType = (int)EWarpDriveProgramType.FULL_HEAL_ALL_SYSTEMS,
                            ReplaceExistingComp = true,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_PROGRAM,
                            SlotNumberToReplace = 5
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_PROGRAM,
                            CompSubType = (int)EWarpDriveProgramType.VIRUS_BOOSTER,
                            ReplaceExistingComp = true,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_PROGRAM,
                            SlotNumberToReplace = 6
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_PROGRAM,
                            CompSubType = (int)EWarpDriveProgramType.SYBER_SHEILD,
                            ReplaceExistingComp = true,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_PROGRAM,
                            SlotNumberToReplace = 7
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_PROGRAM,
                            CompSubType = (int)EWarpDriveProgramType.HULL_POWERED_ARMOR_FLAW,
                            ReplaceExistingComp = true,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_PROGRAM,
                            SlotNumberToReplace = 8
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_PROGRAM,
                            CompSubType = (int)EWarpDriveProgramType.PHALANX_VIRUS_PROGRAM,
                            ReplaceExistingComp = true,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_PROGRAM,
                            SlotNumberToReplace = 9
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_SENS,
                            CompSubType = (int)0,
                            ReplaceExistingComp = true,
                            CompLevel = 4 + Mathf.FloorToInt((float)PLServer.Instance.ChaosLevel / 3f),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_SENS,
                            SlotNumberToReplace = 0
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_THRUSTER,
                            CompSubType = (int)EThrusterType.E_THRUSTER_PERF,
                            ReplaceExistingComp = true,
                            CompLevel = 3 + Mathf.FloorToInt((float)PLServer.Instance.ChaosLevel / 3f),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_THRUSTER,
                            SlotNumberToReplace = 0
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_THRUSTER,
                            CompSubType = (int)EThrusterType.E_THRUSTER_PERF,
                            ReplaceExistingComp = true,
                            CompLevel = 3 + Mathf.FloorToInt((float)PLServer.Instance.ChaosLevel / 3f),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_THRUSTER,
                            SlotNumberToReplace = 1
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_INERTIA_THRUSTER,
                            CompSubType = (int)EInertiaThrusterType.E_NORMAL,
                            ReplaceExistingComp = true,
                            CompLevel = 3 + Mathf.FloorToInt((float)PLServer.Instance.ChaosLevel / 3f),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_INERTIA_THRUSTER,
                            SlotNumberToReplace = 0
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_MANEUVER_THRUSTER,
                            CompSubType = (int)EManeuverThrusterType.E_NORMAL,
                            ReplaceExistingComp = true,
                            CompLevel = 3 + Mathf.FloorToInt((float)PLServer.Instance.ChaosLevel / 3f),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_MANEUVER_THRUSTER,
                            SlotNumberToReplace = 0
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_POLYTECH_MODULE,
                            CompSubType = PolytechModuleModManager.Instance.GetPolytechModuleIDFromName("P.T. Module: Recompiler 1"),
                            ReplaceExistingComp = true,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_POLYTECH_MODULE,
                            SlotNumberToReplace = 0
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_POLYTECH_MODULE,
                            CompSubType = PolytechModuleModManager.Instance.GetPolytechModuleIDFromName("P.T. Module: Recompiler 4"),
                            ReplaceExistingComp = true,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_POLYTECH_MODULE,
                            SlotNumberToReplace = 1
                        }
                        );
                    break;
                case 4:
                    PFCommanderParts.Add
                    (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_SHLD,
                            CompSubType = (int)EShieldGeneratorType.E_POLYTECH_SHIELDS,
                            ReplaceExistingComp = true,
                            CompLevel = 5 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_SHLD,
                            SlotNumberToReplace = 0
                        }
                    );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_REACTOR,
                            CompSubType = (int)EReactorType.E_POLYTECH_ORIGINAL,
                            ReplaceExistingComp = true,
                            CompLevel = 5 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_REACTOR,
                            SlotNumberToReplace = 0
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_HULL,
                            CompSubType = (int)EHullType.E_POLYTECH_HULL,
                            ReplaceExistingComp = true,
                            CompLevel = 5 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_HULL,
                            SlotNumberToReplace = 0
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_CPU,
                            CompSubType = (int)ECPUClass.E_CPUTYPE_CYBER_DEF,
                            ReplaceExistingComp = true,
                            CompLevel = 5 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_CPU,
                            SlotNumberToReplace = 0
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_CPU,
                            CompSubType = (int)ECPUClass.E_CPUTYPE_CYBER_DEF,
                            ReplaceExistingComp = true,
                            CompLevel = 5 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_CPU,
                            SlotNumberToReplace = 1
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_CPU,
                            CompSubType = (int)ECPUClass.E_CPUTYPE_SHIELD_COPROCESSOR,
                            ReplaceExistingComp = true,
                            CompLevel = 5 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_CPU,
                            SlotNumberToReplace = 2
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_CPU,
                            CompSubType = (int)ECPUClass.E_CPUTYPE_TARGETING,
                            ReplaceExistingComp = true,
                            CompLevel = 5 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_CPU,
                            SlotNumberToReplace = 3
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_CPU,
                            CompSubType = (int)ECPUClass.E_CPUTYPE_TARGETING,
                            ReplaceExistingComp = true,
                            CompLevel = 5 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_CPU,
                            SlotNumberToReplace = 4
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_CPU,
                            CompSubType = (int)ECPUClass.CYBERWARFARE_MODULE,
                            ReplaceExistingComp = true,
                            CompLevel = 5 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_CPU,
                            SlotNumberToReplace = 5
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_CPU,
                            CompSubType = (int)ECPUClass.SKUNK,
                            ReplaceExistingComp = true,
                            CompLevel = 5 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_CPU,
                            SlotNumberToReplace = 6
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_CPU,
                            CompSubType = CPUModManager.Instance.GetCPUIDFromName("Super Shield"),
                            ReplaceExistingComp = true,
                            CompLevel = 0,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_CPU,
                            SlotNumberToReplace = 7
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_TURRET,
                            CompSubType = TurretModManager.Instance.GetTurretIDFromName("Nanite Railgun"),
                            ReplaceExistingComp = true,
                            CompLevel = 5 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_TURRET,
                            SlotNumberToReplace = 0
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_TURRET,
                            CompSubType = (int)ETurretType.MISSILE,
                            ReplaceExistingComp = true,
                            CompLevel = 5 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 6) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_TURRET,
                            SlotNumberToReplace = 1
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_TRACKERMISSILE,
                            CompSubType = TurretModManager.Instance.GetTurretIDFromName("Seeker Missile"),
                            ReplaceExistingComp = true,
                            CompLevel = 5 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_TRACKERMISSILE,
                            SlotNumberToReplace = 0
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_MAINTURRET,
                            CompSubType = MegaTurretModManager.Instance.GetMegaTurretIDFromName("The Disassembler"),
                            ReplaceExistingComp = true,
                            CompLevel = 5 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_MAINTURRET,
                            SlotNumberToReplace = 0
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_PROGRAM,
                            CompSubType = (int)EWarpDriveProgramType.REMOVE_ALL_VIRUSES,
                            ReplaceExistingComp = true,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_PROGRAM,
                            SlotNumberToReplace = 0
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_PROGRAM,
                            CompSubType = (int)EWarpDriveProgramType.SUPER_SHIELD_BOOSTER,
                            ReplaceExistingComp = true,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_PROGRAM,
                            SlotNumberToReplace = 1
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_PROGRAM,
                            CompSubType = (int)EWarpDriveProgramType.SUPER_SHIELD_BOOSTER,
                            ReplaceExistingComp = true,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_PROGRAM,
                            SlotNumberToReplace = 2
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_PROGRAM,
                            CompSubType = (int)EWarpDriveProgramType.BARRAGE,
                            ReplaceExistingComp = true,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_PROGRAM,
                            SlotNumberToReplace = 3
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_PROGRAM,
                            CompSubType = (int)EWarpDriveProgramType.QUANTUM_DEFENSES,
                            ReplaceExistingComp = true,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_PROGRAM,
                            SlotNumberToReplace = 4
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_PROGRAM,
                            CompSubType = (int)EWarpDriveProgramType.FULL_HEAL_ALL_SYSTEMS,
                            ReplaceExistingComp = true,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_PROGRAM,
                            SlotNumberToReplace = 5
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_PROGRAM,
                            CompSubType = (int)EWarpDriveProgramType.VIRUS_BOOSTER,
                            ReplaceExistingComp = true,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_PROGRAM,
                            SlotNumberToReplace = 6
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_PROGRAM,
                            CompSubType = (int)EWarpDriveProgramType.SYBER_SHEILD,
                            ReplaceExistingComp = true,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_PROGRAM,
                            SlotNumberToReplace = 7
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_PROGRAM,
                            CompSubType = (int)EWarpDriveProgramType.HULL_POWERED_ARMOR_FLAW,
                            ReplaceExistingComp = true,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_PROGRAM,
                            SlotNumberToReplace = 8
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_PROGRAM,
                            CompSubType = (int)EWarpDriveProgramType.PHALANX_VIRUS_PROGRAM,
                            ReplaceExistingComp = true,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_PROGRAM,
                            SlotNumberToReplace = 9
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_SENS,
                            CompSubType = (int)0,
                            ReplaceExistingComp = true,
                            CompLevel = 5 + Mathf.FloorToInt((float)PLServer.Instance.ChaosLevel / 3f),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_SENS,
                            SlotNumberToReplace = 0
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_THRUSTER,
                            CompSubType = (int)EThrusterType.E_THRUSTER_PERF,
                            ReplaceExistingComp = true,
                            CompLevel = 3 + Mathf.FloorToInt((float)PLServer.Instance.ChaosLevel / 3f),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_THRUSTER,
                            SlotNumberToReplace = 0
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_THRUSTER,
                            CompSubType = (int)EThrusterType.E_THRUSTER_PERF,
                            ReplaceExistingComp = true,
                            CompLevel = 3 + Mathf.FloorToInt((float)PLServer.Instance.ChaosLevel / 3f),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_THRUSTER,
                            SlotNumberToReplace = 1
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_INERTIA_THRUSTER,
                            CompSubType = (int)EInertiaThrusterType.E_NORMAL,
                            ReplaceExistingComp = true,
                            CompLevel = 3 + Mathf.FloorToInt((float)PLServer.Instance.ChaosLevel / 3f),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_INERTIA_THRUSTER,
                            SlotNumberToReplace = 0
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_MANEUVER_THRUSTER,
                            CompSubType = (int)EManeuverThrusterType.E_NORMAL,
                            ReplaceExistingComp = true,
                            CompLevel = 3 + Mathf.FloorToInt((float)PLServer.Instance.ChaosLevel / 3f),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_MANEUVER_THRUSTER,
                            SlotNumberToReplace = 0
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_POLYTECH_MODULE,
                            CompSubType = PolytechModuleModManager.Instance.GetPolytechModuleIDFromName("P.T. Module: Recompiler 1"),
                            ReplaceExistingComp = true,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_POLYTECH_MODULE,
                            SlotNumberToReplace = 0
                        }
                        );
                    PFCommanderParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_POLYTECH_MODULE,
                            CompSubType = PolytechModuleModManager.Instance.GetPolytechModuleIDFromName("P.T. Module: Recompiler 5"),
                            ReplaceExistingComp = true,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_POLYTECH_MODULE,
                            SlotNumberToReplace = 1
                        }
                        );
                    break;
            }
            return PFCommanderParts;
        }

        [HarmonyPatch(typeof(PLShipInfoBase), "GetChaosBoost", new Type[2] { typeof(PLPersistantShipInfo), typeof(int) })]
        internal class PFSectorCommanderScalingFix
        {
            /*private static void Postfix(PLShipInfoBase __instance, PLPersistantShipInfo inPersistantShipInfo, int offset, ref int __result)
            {
                if (!((UnityEngine.Object)PLServer.Instance != (UnityEngine.Object)null) || inPersistantShipInfo == null)
                    return;
                if (inPersistantShipInfo.Type == EShipType.E_POLYTECH_SHIP)
                {
                    switch (inPersistantShipInfo.ShipName)
                    {
                        case "The Recompiler Config: 1":
                        case "The Recompiler Config: 2":
                        case "The Recompiler Config: 3":
                        case "The Recompiler Config: 4":
                        case "The Recompiler Config: 5":
                            __result = 0;
                            break;
                    }
                }
            }*/
            private static Exception Finalizer(Exception __exception, PLShipInfoBase __instance, PLPersistantShipInfo inPersistantShipInfo, int offset, ref int __result)
            {
                if (!((UnityEngine.Object)PLServer.Instance != (UnityEngine.Object)null) || inPersistantShipInfo == null)
                    return __exception;
                if (inPersistantShipInfo.Type == EShipType.E_POLYTECH_SHIP)
                {
                    switch (inPersistantShipInfo.ShipName)
                    {
                        case "The Recompiler Config: 1":
                        case "The Recompiler Config: 2":
                        case "The Recompiler Config: 3":
                        case "The Recompiler Config: 4":
                        case "The Recompiler Config: 5":
                            __result = 0;
                            break;
                    }
                }
                return __exception;
            }
        }

        [HarmonyPatch(typeof(PLShipInfo), "CreateDefaultItemsForEnemyBotPlayer")]
        internal class PFSectorCommanderCrewSpawn
        {
            private static void Postfix(PLPlayer inPlayer)
            {
                if (!((UnityEngine.Object)inPlayer != (UnityEngine.Object)null) || !((UnityEngine.Object)inPlayer.StartingShip != (UnityEngine.Object)null) || inPlayer.StartingShip.IsRelicHunter || inPlayer.StartingShip.IsBountyHunter || !(inPlayer.StartingShip.ShipTypeID == EShipType.E_POLYTECH_SHIP && inPlayer.StartingShip.IsSectorCommander))
                    return;
                int inLevel = Mathf.RoundToInt((float)(Mathf.FloorToInt((float)PLServer.Instance.ChaosLevel) + UnityEngine.Random.Range(0, 2)) + (float)Mathf.CeilToInt(inPlayer.StartingShip.GetCombatLevel() / 20f) * UnityEngine.Random.Range(0.7f, 1f));
                inPlayer.Talents[(int)ETalents.HEALTH_BOOST] = (ObscuredInt)5;
                inPlayer.Talents[(int)ETalents.PISTOL_DMG_BOOST] = (ObscuredInt)3;
                inPlayer.Talents[(int)ETalents.QUICK_RESPAWN] = (ObscuredInt)4;
                inPlayer.Talents[(int)ETalents.ARMOR_BOOST] = (ObscuredInt)5;
                switch (inPlayer.GetClassID())
                {
                    case 0: //CAP
                        inPlayer.Talents[(int)ETalents.CAP_CREW_SPEED_BOOST] = (ObscuredInt)5;
                        inPlayer.Talents[(int)ETalents.CAP_ARMOR_BOOST] = (ObscuredInt)0 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar);
                        inPlayer.Talents[(int)ETalents.CAP_SCREEN_DEFENSE] = (ObscuredInt)5 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar);
                        inPlayer.Talents[(int)ETalents.CAP_SCREEN_SAFETY] = (ObscuredInt)5 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar);
                        break;
                    case 1:  //PI
                        inPlayer.Talents[(int)ETalents.PIL_SHIP_SPEED] = (ObscuredInt)5;
                        inPlayer.Talents[(int)ETalents.PIL_SHIP_TURNING] = (ObscuredInt)5;
                        inPlayer.Talents[(int)ETalents.PIL_REDUCE_SYS_DAMAGE] = (ObscuredInt)18;
                        inPlayer.Talents[(int)ETalents.PIL_REDUCE_HULL_DAMAGE] = (ObscuredInt)2 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar);
                        break;
                    case 2: //SCI
                        inPlayer.Talents[(int)ETalents.SCI_SENSOR_BOOST] = (ObscuredInt)5;
                        inPlayer.Talents[(int)ETalents.SCI_SENSOR_HIDE] = (ObscuredInt)1 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar);
                        inPlayer.Talents[(int)ETalents.SCI_HEAL_NEARBY] = (ObscuredInt)5;
                        break;
                    case 3: //WEAP
                        inPlayer.Talents[(int)ETalents.WPNS_MISSILE_EXPERT] = (ObscuredInt)1 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar);
                        inPlayer.Talents[(int)ETalents.WPNS_COOLING] = (ObscuredInt)5;
                        inPlayer.Talents[(int)ETalents.WPNS_REDUCE_PAWN_DMG] = (ObscuredInt)2 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar);
                        inPlayer.Talents[(int)ETalents.WPNS_BOOST_CREW_TURRET_CHARGE] = (ObscuredInt)2 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar);
                        inPlayer.Talents[(int)ETalents.WPNS_BOOST_CREW_TURRET_DAMAGE] = (ObscuredInt)2 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar);
                        inPlayer.Talents[(int)ETalents.WPN_SCREEN_HACKER] = (ObscuredInt)2 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar);
                        inPlayer.Talents[(int)ETalents.E_TURRET_COOLING_CREW_WEAPONS] = (ObscuredInt)5;
                        break;
                    case 4: //ENG
                        inPlayer.Talents[(int)ETalents.ENG_FIRE_REDUCTION] = (ObscuredInt)5;
                        inPlayer.Talents[(int)ETalents.ENG_COOLANT_MIX_CUSTOM] = (ObscuredInt)5;
                        inPlayer.Talents[(int)ETalents.ENG_REPAIR_DRONES] = (ObscuredInt)5 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar);
                        inPlayer.Talents[(int)ETalents.ENG_COREPOWERBOOST] = (ObscuredInt)50;
                        inPlayer.Talents[(int)ETalents.ENG_CORECOOLINGBOOST] = (ObscuredInt)2 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar);
                        inPlayer.Talents[(int)ETalents.E_TURRET_COOLING_CREW_ENGINEER] = (ObscuredInt)5;
                        break;
                }
                inPlayer.MyInventory.Clear();
                switch (inPlayer.GetClassID())
                {
                    case 0:
                        inPlayer.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, 9, 0, 5, 1);
                        break;
                    case 1:
                        inPlayer.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, 10, 0, 5, 1);
                        break;
                    case 2:
                        inPlayer.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, 11, 0, 5, 1);
                        break;
                    case 3:
                        inPlayer.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, 12, 0, 5, 1);
                        break;
                    case 4:
                        inPlayer.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, 2, 0, 5, 1);
                        break;
                }

                if (inPlayer.GetClassID() != 4)
                {
                    int inNetID1 = PLServer.Instance.PawnInvItemIDCounter++;
                    inPlayer.MyInventory.UpdateItem(inNetID1, 3, 0, 5, 2);
                }
                else
                {
                    int inNetID3 = PLServer.Instance.PawnInvItemIDCounter++;
                    inPlayer.MyInventory.UpdateItem(inNetID3, 23, 0, 5, 2);
                }
                int inNetID2 = PLServer.Instance.PawnInvItemIDCounter++;
                inPlayer.MyInventory.UpdateItem(inNetID2, 4, 0, 5, 3);
                if (inPlayer.GetClassID() == 0)
                {
                    inPlayer.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, 13, 0, 0, 4);
                }
                inPlayer.gameObject.name = "Simple Combat Bot Player Modded";
            }
        }
    }
}
