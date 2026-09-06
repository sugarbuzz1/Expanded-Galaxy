using HarmonyLib;
using System.Collections.Generic;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLGalaxy), "CreateStartingShips")]
    internal class SpawnCaravan
    {
        private static void Postfix(PLGalaxy __instance, ref Dictionary<int, PLSectorInfo> ___m_AllSectorInfos)
        {
            if (PLServer.Instance == null)
                return;
            PLRand rand = new PLRand(__instance.Seed);
            ESectorVisualIndication visualIndicationStart = ESectorVisualIndication.NONE;
            List<ESectorVisualIndication> visualIndications = new List<ESectorVisualIndication>()
                            {
                                ESectorVisualIndication.CORNELIA_HUB,
                                ESectorVisualIndication.DESERT_HUB,
                                ESectorVisualIndication.GENTLEMEN_START,
                                ESectorVisualIndication.THE_HARBOR
                            };
            visualIndicationStart = visualIndications[rand.Next(visualIndications.Count)];
            foreach (PLSectorInfo info in ___m_AllSectorInfos.Values)
            {
                if (info.VisualIndication == visualIndicationStart)
                {
                    PLPersistantShipInfo caravanInfo = new PLPersistantShipInfo(EShipType.E_ROLAND, 1, info)
                    {
                        ShipName = "Wandering Caravan",
                        ShldPercent = 1f,
                        HullPercent = 1f,
                        SelectedActorID = "ExGal_RelicCaravan"
                    };
                    TraderPersistantDataEntry traderPersistantDataEntry = new TraderPersistantDataEntry();
                    traderPersistantDataEntry.ServerAddWare(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_SHLD, (int)EShieldGeneratorType.E_SG_CGF_HEAVY_SHIELD_GEN, 0, 0, (int)ESlotType.E_COMP_CARGO)));
                    traderPersistantDataEntry.ServerAddWare(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_SHLD, (int)EShieldGeneratorType.E_SG_GTC_BLUE_GOOSE, 1, 0, (int)ESlotType.E_COMP_CARGO)));
                    traderPersistantDataEntry.ServerAddWare(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_WARP, (int)EWarpDriveType.E_WARPDR_WDMILITARYJUMP, 0, 0, (int)ESlotType.E_COMP_CARGO)));
                    traderPersistantDataEntry.ServerAddWare(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_REACTOR, (int)EReactorType.E_SYLVASSI_REACTOR, 0, 0, (int)ESlotType.E_COMP_CARGO)));
                    traderPersistantDataEntry.ServerAddWare(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_REACTOR, (int)EReactorType.E_LEAKING_REACTOR, 0, 0, (int)ESlotType.E_COMP_CARGO)));
                    traderPersistantDataEntry.ServerAddWare(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_HULL, (int)EHullType.E_OBSOLETE_HULL, 1, 0, (int)ESlotType.E_COMP_CARGO)));
                    traderPersistantDataEntry.ServerAddWare(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_HULL, (int)EHullType.E_NANO_ACTIVE_HULL, 2, 0, (int)ESlotType.E_COMP_CARGO)));
                    traderPersistantDataEntry.ServerAddWare(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_CPU, (int)ECPUClass.E_WARP_RANGE_EXTENTION, 0, 0, (int)ESlotType.E_COMP_CARGO)));
                    traderPersistantDataEntry.ServerAddWare(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_CPU, (int)ECPUClass.E_SCRAP_PROCESSOR, 1, 0, (int)ESlotType.E_COMP_CARGO)));
                    traderPersistantDataEntry.ServerAddWare(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_CPU, UnityEngine.Random.Range(20, 25), 0, 0, (int)ESlotType.E_COMP_CARGO)));
                    traderPersistantDataEntry.ServerAddWare(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_CPU, (int)ECPUClass.CYBERWARFARE_MODULE, 1, 0, (int)ESlotType.E_COMP_CARGO)));
                    traderPersistantDataEntry.ServerAddWare(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_CPU, (int)ECPUClass.IMPROVED_DEFENSES, 0, 0, (int)ESlotType.E_COMP_CARGO)));
                    traderPersistantDataEntry.ServerAddWare(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_THRUSTER, (int)EThrusterType.E_THRUSTER_PERF, 1, 0, (int)ESlotType.E_COMP_CARGO)));
                    traderPersistantDataEntry.ServerAddWare(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_THRUSTER, (int)EThrusterType.E_THRUSTER_RACING, 0, 0, (int)ESlotType.E_COMP_CARGO)));
                    traderPersistantDataEntry.ServerAddWare(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_TURRET, (int)ETurretType.SCRAPPER, 1, 0, (int)ESlotType.E_COMP_CARGO)));
                    traderPersistantDataEntry.ServerAddWare(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_TURRET, (int)ETurretType.MISSILE, 0, 0, (int)ESlotType.E_COMP_CARGO)));
                    traderPersistantDataEntry.ServerAddWare(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_HULLPLATING, 0, 2, 0, (int)ESlotType.E_COMP_CARGO)));
                    traderPersistantDataEntry.ServerAddWare(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_PROGRAM, (int)EWarpDriveProgramType.SITTING_DUCK_VIRUS_PROGRAM, 0, 0, (int)ESlotType.E_COMP_CARGO)));
                    traderPersistantDataEntry.ServerAddWare(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_PROGRAM, (int)EWarpDriveProgramType.WARP_DISABLE_VIRUS_PROGRAM, 0, 0, (int)ESlotType.E_COMP_CARGO)));
                    traderPersistantDataEntry.ServerAddWare(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_PROGRAM, (int)EWarpDriveProgramType.RAND_SMALL, 0, 0, (int)ESlotType.E_COMP_CARGO)));
                    traderPersistantDataEntry.ServerAddWare(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_PROGRAM, (int)EWarpDriveProgramType.CAPACITOR, 0, 0, (int)ESlotType.E_COMP_CARGO)));
                    traderPersistantDataEntry.ServerAddWare(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_PROGRAM, (int)EWarpDriveProgramType.EXTENDED_SHIELDS, 0, 0, (int)ESlotType.E_COMP_CARGO)));
                    traderPersistantDataEntry.ServerAddWare(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_PROGRAM, (int)EWarpDriveProgramType.EXTENDED_SHIELDS, 0, 0, (int)ESlotType.E_COMP_CARGO)));
                    traderPersistantDataEntry.ServerAddWare(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_PROGRAM, (int)EWarpDriveProgramType.VIRUS_BOOSTER, 0, 0, (int)ESlotType.E_COMP_CARGO)));
                    traderPersistantDataEntry.ServerAddWare(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_PROGRAM, (int)EWarpDriveProgramType.QUANTUM_TUNNEL, 0, 0, (int)ESlotType.E_COMP_CARGO)));
                    traderPersistantDataEntry.ServerAddWare(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_TRACKERMISSILE, (int)ETrackerMissileType.STRAIGHTSHOT, 1, 0, (int)ESlotType.E_COMP_CARGO)));
                    traderPersistantDataEntry.ServerAddWare(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_TRACKERMISSILE, (int)ETrackerMissileType.ARMOR_PIERCE, 0, 0, (int)ESlotType.E_COMP_CARGO)));
                    traderPersistantDataEntry.ServerAddWare(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_AUTO_TURRET, 0, UnityEngine.Random.Range(0, 3), 0, (int)ESlotType.E_COMP_CARGO)));
                    traderPersistantDataEntry.ServerAddWare(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_AUTO_TURRET, 0, UnityEngine.Random.Range(0, 3), 0, (int)ESlotType.E_COMP_CARGO)));
                    traderPersistantDataEntry.ServerAddWare(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_MANEUVER_THRUSTER, (int)EManeuverThrusterType.E_HEAVY, 2, 0, (int)ESlotType.E_COMP_CARGO)));
                    traderPersistantDataEntry.ServerAddWare(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_SALVAGE_SYSTEM, (int)EExtractorType.E_STARSALVAGE_E70, 0, 0, (int)ESlotType.E_COMP_CARGO)));
                    caravanInfo.OptionalTPDE = traderPersistantDataEntry;
                    RelicCaravan.CaravanTraderData = traderPersistantDataEntry;
                    caravanInfo.MyCurrentSector = info;
                    RelicCaravan.CaravanCurrentSector = info.ID;
                    RelicCaravan.CaravanTargetSector = -1;
                    caravanInfo.CompOverrides.AddRange(RelicCaravan.CaravanComponents(__instance.Seed));
                    PLServer.Instance.AllPSIs.Add(caravanInfo);
                    RelicCaravan.CaravanUpdateTime = PLServer.Instance.GetEstimatedServerMs() + 120000;
                    UpdateCaravan.invalidTargets.Clear();
                    UpdateCaravan.invalidTargets.Add(ESectorVisualIndication.AOG_HUB);
                    UpdateCaravan.persistantCaravanInfo = caravanInfo;
                }
            }
        }
    }
}
