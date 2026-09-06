using PulsarModLoader.Content.Components.AutoTurret;
using PulsarModLoader.Content.Components.Hull;
using PulsarModLoader.Content.Components.HullPlating;
using PulsarModLoader.Content.Components.Missile;
using PulsarModLoader.Content.Components.Reactor;
using PulsarModLoader.Content.Components.Shield;
using PulsarModLoader.Content.Components.Turret;
using PulsarModLoader.Content.Components.WarpDriveProgram;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace ExpandedGalaxy
{
    internal class RelicCaravan
    {
        internal static int CaravanCurrentSector = -1;
        internal static int CaravanTargetSector = -1;
        internal static List<PLSectorInfo> CaravanPath = new List<PLSectorInfo>();
        internal static int CaravanPathIndex = 0;
        internal static int CaravanSpecialsData = 0;
        internal static int CaravanUpdateTime = 0;
        internal static TraderPersistantDataEntry CaravanTraderData;

        internal static void ClearCaravanPath()
        {
            CaravanPath.Clear();
            CaravanPathIndex = 0;
        }

        internal static async void LateAddShopComponent(int inShipID, int shopID)
        {
            PLShipInfoBase shipInfoBase = null;
            while (true)
            {
                shipInfoBase = PLEncounterManager.Instance.GetShipFromID(inShipID);
                if (shipInfoBase == null)
                    await Task.Yield();
                else
                    break;
            }
            if (shopID == 0)
            {
                if (shipInfoBase.ShipRoot.TryGetComponent(typeof(Shop_Caravan), out Component component))
                    return;
                Shop_Caravan shop = shipInfoBase.ShipRoot.AddComponent<Shop_Caravan>();
                shop.OptionalShip = shipInfoBase;
                shop.MySensorObject = shipInfoBase.MySensorObjectShip;
                shipInfoBase.photonView.ObservedComponents.Add(shop);
            }
            else if (shopID == 1)
            {
                if (shipInfoBase.ShipRoot.TryGetComponent(typeof(Shop_FBCarrier), out Component component))
                    return;
                Shop_FBCarrier shop = shipInfoBase.ShipRoot.AddComponent<Shop_FBCarrier>();
                shop.OptionalShip = shipInfoBase;
                shop.MySensorObject = shipInfoBase.MySensorObjectShip;
                shipInfoBase.photonView.ObservedComponents.Add(shop);
            }
        }

        internal static PLShipComponent GetSpecialOffer()
        {
            PLShipComponent component;
            int level = Mathf.RoundToInt(Mathf.Pow((float)UnityEngine.Random.Range(0f, 1f), 4f) * 2.7f);
            int num = -1;
            if (PLServer.Instance != null)
            {
                if ((double)(float)PLServer.Instance.ChaosLevel < 2.0)
                    num += 2;
                if ((double)(float)PLServer.Instance.ChaosLevel < 1.0)
                    ++num;
            }
            switch (UnityEngine.Random.Range(num, 17))
            {
                case -1:
                    component = PLTurret.CreateTurretFromHash(TurretModManager.Instance.GetTurretIDFromName("Particle Lance"), level, 0);
                    break;
                case 0:
                    component = PLShieldGenerator.CreateShieldGeneratorFromHash(ShieldModManager.Instance.GetShieldIDFromName("Reflector Shield Generator"), level, 0);
                    break;
                case 1:
                    component = PLTurret.CreateTurretFromHash(TurretModManager.Instance.GetTurretIDFromName("Sylvassi Turret"), level, 0);
                    break;
                case 2:
                    component = PLMegaTurret.CreateMainTurretFromHash(1, level, 1);
                    break;
                case 3:
                    component = new RipperTurrret();
                    break;
                case 4:
                    component = PLShieldGenerator.CreateShieldGeneratorFromHash((int)EShieldGeneratorType.E_XC7_SHIELDS, level, 0);
                    break;
                case 5:
                    component = PLHull.CreateHullFromHash((int)EHullType.E_DESTROYER_HULL, level, 0);
                    break;
                case 6:
                    component = PLHullPlating.CreateHullPlatingFromHash(HullPlatingModManager.Instance.GetHullPlatingIDFromName("HeavyDutyPlating"), level, 0);
                    break;
                case 7:
                    component = PLNuclearDevice.CreateNuclearDeviceFromHash((int)ENuclearDeviceType.WD_LARGE, 0, 0);
                    break;
                case 8:
                    component = PLNuclearDevice.CreateNuclearDeviceFromHash((int)ENuclearDeviceType.CU_PEACEKEEPER, 0, 0);
                    break;
                case 9:
                    component = PLWarpDrive.CreateWarpDriveFromHash((int)EWarpDriveType.E_WARPDR_PARALLEL, level, 0);
                    break;
                case 10:
                    component = PLWarpDriveProgram.CreateWarpDriveProgramFromHash(WarpDriveProgramModManager.Instance.GetWarpDriveProgramIDFromName("H.E.A.T."), 0, 0);
                    break;
                case 11:
                    component = PLWarpDriveProgram.CreateWarpDriveProgramFromHash((int)EWarpDriveProgramType.VIRUS_BOOSTER, 0, 0);
                    break;
                case 12:
                    component = PLWarpDriveProgram.CreateWarpDriveProgramFromHash((int)EWarpDriveProgramType.COOLDOWN_RESET, 0, 0);
                    break;
                case 13:
                    component = PLTrackerMissile.CreateTrackerMissileFromHash(MissileModManager.Instance.GetMissileIDFromName("Thermobaric Missile"), 0, 0);
                    break;
                case 14:
                    component = PLReactor.CreateReactorFromHash((int)EReactorType.ROLAND_REACTOR, level, 0);
                    break;
                case 15:
                    component = PLAutoTurret.CreateAutoTurretFromHash(AutoTurretModManager.Instance.GetAutoTurretIDFromName("Auto Laser Turret"), level, 0);
                    break;
                default:
                    component = PLExtractor.CreateExtractorFromHash((int)EExtractorType.E_PT_EXTRACTOR, level, 0);
                    break;
            }
            component.Level += Mathf.RoundToInt((UnityEngine.Object)PLServer.Instance != (UnityEngine.Object)null ? (float)PLServer.Instance.ChaosLevel * 0.2f : 0.0f);
            return component;
        }

        public static List<PLSectorInfo> GetPathToSector_NPC(
            PLSectorInfo inStartSector,
            PLSectorInfo inEndSector,
            float customWarpRange,
            int factionID = 1)
        {
            bool flag = false;
            List<PLSectorInfo> OpenList = new List<PLSectorInfo>();
            List<PLSectorInfo> ReturnSolution = new List<PLSectorInfo>();
            foreach (PLSectorInfo plSectorInfo2 in PLGlobal.Instance.Galaxy.AllSectorInfos.Values)
            {
                plSectorInfo2.Category = NodeCategory.NODE_DEF;
            }
            OpenList.Clear();
            OpenList.Add((PLSectorInfo)null);
            OpenList.Add(inStartSector);
            inStartSector.FCost = 0.0f;
            inStartSector.SearchParentNode = (PLSectorInfo)null;
            PLSectorInfo plSectorInfo3 = (PLSectorInfo)null;
            while (OpenList.Count > 1 && !flag)
            {
                plSectorInfo3 = OpenList[1];
                plSectorInfo3.Category = NodeCategory.NODE_CLOSED;
                OpenList[1] = OpenList[OpenList.Count - 1];
                OpenList.RemoveAt(OpenList.Count - 1);
                int index1 = 1;
                while (true)
                {
                    int index2 = index1;
                    if (2 * index2 + 1 <= OpenList.Count - 1)
                    {
                        if ((double)OpenList[index2].FCost >= (double)OpenList[2 * index2].FCost)
                            index1 = 2 * index2;
                        if ((double)OpenList[index1].FCost >= (double)OpenList[2 * index2 + 1].FCost)
                            index1 = 2 * index2 + 1;
                    }
                    else if (2 * index2 <= OpenList.Count - 1 && (double)OpenList[index2].FCost >= (double)OpenList[2 * index2].FCost)
                        index1 = 2 * index2;
                    if (index2 != index1)
                    {
                        PLSectorInfo plSectorInfo4 = OpenList[index2];
                        OpenList[index2] = OpenList[index1];
                        OpenList[index1] = plSectorInfo4;
                    }
                    else
                        break;
                }
                if (plSectorInfo3 == inEndSector)
                {
                    flag = true;
                    break;
                }
                foreach (PLSectorInfo neighbor in PLGlobal.Instance.Galaxy.AllSectorInfos.Values)
                {
                    if (!(neighbor.MySPI.Faction == 4 || neighbor.MySPI.Faction == 6 || neighbor.MissionSpecificID != -1 || (neighbor.VisualIndication == ESectorVisualIndication.COLONIAL_HUB && factionID != 0) || (neighbor.VisualIndication == ESectorVisualIndication.WD_START && factionID != 2) || neighbor.VisualIndication == ESectorVisualIndication.GWG || neighbor.VisualIndication == ESectorVisualIndication.CYPHER_LAB || neighbor.VisualIndication == ESectorVisualIndication.GREY_HUNTSMAN_HQ || neighbor.VisualIndication == ESectorVisualIndication.HIGHROLLERS_STATION || neighbor.VisualIndication == ESectorVisualIndication.BLACKHOLE || neighbor.VisualIndication == ESectorVisualIndication.MINE_FIELD || neighbor.VisualIndication == ESectorVisualIndication.INTREPID_SECTOR_CMDR || neighbor.VisualIndication == ESectorVisualIndication.ANCIENT_SENTRY || neighbor.VisualIndication == ESectorVisualIndication.ALCHEMIST || neighbor.VisualIndication == ESectorVisualIndication.DEATHSEEKER_COMMANDER || neighbor.VisualIndication == ESectorVisualIndication.SWARM_CMDR || neighbor.VisualIndication == ESectorVisualIndication.SWARM_KEEPER || neighbor.VisualIndication == ESectorVisualIndication.GENERAL_STORE || neighbor.VisualIndication == ESectorVisualIndication.EXOTIC4 || neighbor.VisualIndication == ESectorVisualIndication.EXOTIC5 || neighbor.VisualIndication == ESectorVisualIndication.EXOTIC6 || neighbor.VisualIndication == ESectorVisualIndication.EXOTIC7) && neighbor.Category != NodeCategory.NODE_CLOSED && PLStarmap.ShouldShowSectorBG(neighbor) && (double)(new Vector2(plSectorInfo3.Position.x, plSectorInfo3.Position.y) - new Vector2(neighbor.Position.x, neighbor.Position.y)).sqrMagnitude <= (double)customWarpRange * (double)customWarpRange)
                    {
                        float num3 = PLGlobal.Instance.Galaxy.Heuristic(neighbor, plSectorInfo3);
                        if (neighbor.Category == NodeCategory.NODE_DEF)
                        {
                            if (!OpenList.Contains(neighbor))
                                OpenList.Add(neighbor);
                            neighbor.Category = NodeCategory.NODE_OPEN;
                            neighbor.SearchParentNode = plSectorInfo3;
                            float num4 = PLGlobal.Instance.Galaxy.Heuristic(neighbor, inEndSector);
                            float HWeight = PLGlobal.Instance.Galaxy.HWeight;
                            neighbor.GCost = plSectorInfo3.GCost + num4 * HWeight;
                            neighbor.HCost = num4 * HWeight;
                            neighbor.FCost = neighbor.GCost + neighbor.HCost * HWeight;
                            SortOpenList(ref OpenList);
                        }
                        else if ((double)neighbor.GCost > (double)plSectorInfo3.GCost + (double)num3)
                        {
                            float HWeight = PLGlobal.Instance.Galaxy.HWeight;
                            neighbor.GCost = plSectorInfo3.GCost + num3 * HWeight;
                            neighbor.FCost = neighbor.GCost + neighbor.HCost * HWeight;
                            neighbor.SearchParentNode = plSectorInfo3;
                            SortOpenList(ref OpenList);
                        }
                    }
                }
            }
            if (flag)
            {
                PrepareSolution(ref ReturnSolution, plSectorInfo3);
                ReturnSolution.Reverse();
            }
            return ReturnSolution;
        }

        private static void SortOpenList(ref List<PLSectorInfo> OpenList)
        {
            int index1;
            for (int index2 = OpenList.Count - 1; index2 != 1; index2 = index1)
            {
                index1 = index2 / 2;
                if ((double)OpenList[index2].FCost > (double)OpenList[index1].FCost)
                    break;
                PLSectorInfo plSectorInfo = OpenList[index2];
                OpenList[index2] = OpenList[index1];
                OpenList[index1] = plSectorInfo;
            }
        }

        private static void PrepareSolution(ref List<PLSectorInfo> ReturnSolution, PLSectorInfo FromNode)
        {
            PLSectorInfo plSectorInfo = FromNode;
            ReturnSolution.Clear();
            if (plSectorInfo == null)
                return;
            for (; plSectorInfo.SearchParentNode != null; plSectorInfo = plSectorInfo.SearchParentNode)
                ReturnSolution.Add(plSectorInfo);
            ReturnSolution.Add(plSectorInfo);
        }

        internal static List<ComponentOverrideData> CaravanComponents(int seed)
        {
            List<ComponentOverrideData> caravanParts = new List<ComponentOverrideData>();
            int num = 0;
            PLRand rand = new PLRand(seed);
            caravanParts.Add
            (
            new ComponentOverrideData()
            {
                CompType = (int)ESlotType.E_COMP_REAC_COOLING,
                CompSubType = 6,
                ReplaceExistingComp = false,
                CompLevel = 0,
                IsCargo = false,
                CompTypeToReplace = (int)ESlotType.E_COMP_REAC_COOLING,
                SlotNumberToReplace = 0
            }
            );
            num = rand.Next() % 2;
            caravanParts.Add
            (
            new ComponentOverrideData()
            {
                CompType = (int)ESlotType.E_COMP_SHLD,
                CompSubType = (int)EShieldGeneratorType.E_SG_CGF_HEAVY_MK3,
                ReplaceExistingComp = true,
                CompLevel = 2 + num,
                IsCargo = false,
                CompTypeToReplace = (int)ESlotType.E_COMP_SHLD,
                SlotNumberToReplace = 0
            }
            );
            num = rand.Next() % 2;
            caravanParts.Add
            (
            new ComponentOverrideData()
            {
                CompType = (int)ESlotType.E_COMP_REACTOR,
                CompSubType = (int)ReactorModManager.Instance.GetReactorIDFromName("Modified Roland Reactor"),
                ReplaceExistingComp = true,
                CompLevel = 4 + num,
                IsCargo = false,
                CompTypeToReplace = (int)ESlotType.E_COMP_REACTOR,
                SlotNumberToReplace = 0
            }
            );
            num = rand.Next() % 2;
            caravanParts.Add
            (
            new ComponentOverrideData()
            {
                CompType = (int)ESlotType.E_COMP_HULL,
                CompSubType = (int)HullModManager.Instance.GetHullIDFromName("Caravaneer Hull"),
                ReplaceExistingComp = true,
                CompLevel = num,
                IsCargo = false,
                CompTypeToReplace = (int)ESlotType.E_COMP_HULL,
                SlotNumberToReplace = 0
            }
            );
            num = rand.Next() % 2;
            caravanParts.Add
            (
            new ComponentOverrideData()
            {
                CompType = (int)ESlotType.E_COMP_HULLPLATING,
                CompSubType = (int)EHullPlatingType.E_HULLPLATING_CCGE,
                ReplaceExistingComp = true,
                CompLevel = 3 + num,
                IsCargo = false,
                CompTypeToReplace = (int)ESlotType.E_COMP_HULLPLATING,
                SlotNumberToReplace = 0
            }
            );
            num = rand.Next() % 2;
            caravanParts.Add
            (
            new ComponentOverrideData()
            {
                CompType = (int)ESlotType.E_COMP_CAPTAINS_CHAIR,
                CompSubType = (int)ECaptainsChairType.E_COLONIAL_MODERN,
                ReplaceExistingComp = true,
                CompLevel = 2 + num,
                IsCargo = false,
                CompTypeToReplace = (int)ESlotType.E_COMP_CAPTAINS_CHAIR,
                SlotNumberToReplace = 0
            }
            );
            num = rand.Next() % 2;
            caravanParts.Add
            (
            new ComponentOverrideData()
            {
                CompType = (int)ESlotType.E_COMP_CPU,
                CompSubType = (int)ECPUClass.SYVASSI_CYBER_DEF,
                ReplaceExistingComp = true,
                CompLevel = 2 + num,
                IsCargo = false,
                CompTypeToReplace = (int)ESlotType.E_COMP_CPU,
                SlotNumberToReplace = 0
            }
            );
            num = rand.Next() % 2;
            caravanParts.Add
            (
            new ComponentOverrideData()
            {
                CompType = (int)ESlotType.E_COMP_CPU,
                CompSubType = (int)ECPUClass.SYVASSI_CYBER_DEF,
                ReplaceExistingComp = true,
                CompLevel = 2 + num,
                IsCargo = false,
                CompTypeToReplace = (int)ESlotType.E_COMP_CPU,
                SlotNumberToReplace = 1
            }
            );
            num = rand.Next() % 2;
            caravanParts.Add
            (
            new ComponentOverrideData()
            {
                CompType = (int)ESlotType.E_COMP_CPU,
                CompSubType = (int)ECPUClass.E_CPUTYPE_SHIELD_COPROCESSOR,
                ReplaceExistingComp = true,
                CompLevel = 3 + num,
                IsCargo = false,
                CompTypeToReplace = (int)ESlotType.E_COMP_CPU,
                SlotNumberToReplace = 2
            }
            );
            num = rand.Next() % 2;
            caravanParts.Add
            (
            new ComponentOverrideData()
            {
                CompType = (int)ESlotType.E_COMP_CPU,
                CompSubType = (int)ECPUClass.E_CPUTYPE_TARGETING,
                ReplaceExistingComp = true,
                CompLevel = 3 + num,
                IsCargo = false,
                CompTypeToReplace = (int)ESlotType.E_COMP_CPU,
                SlotNumberToReplace = 3
            }
            );
            num = rand.Next() % 2;
            caravanParts.Add
            (
            new ComponentOverrideData()
            {
                CompType = (int)ESlotType.E_COMP_CPU,
                CompSubType = (int)ECPUClass.E_CPUTYPE_JUMP_PROCESSOR,
                ReplaceExistingComp = true,
                CompLevel = num,
                IsCargo = false,
                CompTypeToReplace = (int)ESlotType.E_COMP_CPU,
                SlotNumberToReplace = 4
            }
            );
            num = rand.Next() % 2;
            caravanParts.Add
            (
            new ComponentOverrideData()
            {
                CompType = (int)ESlotType.E_COMP_TURRET,
                CompSubType = (int)ETurretType.BURST,
                ReplaceExistingComp = true,
                CompLevel = 4 + num,
                IsCargo = false,
                CompTypeToReplace = (int)ESlotType.E_COMP_TURRET,
                SlotNumberToReplace = 0
            }
            );
            num = rand.Next() % 2;
            caravanParts.Add
            (
            new ComponentOverrideData()
            {
                CompType = (int)ESlotType.E_COMP_TURRET,
                CompSubType = (int)ETurretType.BURST,
                ReplaceExistingComp = true,
                CompLevel = 4 + num,
                IsCargo = false,
                CompTypeToReplace = (int)ESlotType.E_COMP_TURRET,
                SlotNumberToReplace = 1
            }
            );
            num = rand.Next() % 2;
            caravanParts.Add
            (
            new ComponentOverrideData()
            {
                CompType = (int)ESlotType.E_COMP_TURRET,
                CompSubType = TurretModManager.Instance.GetTurretIDFromName("Sylvassi Turret"),
                ReplaceExistingComp = true,
                CompLevel = 1 + num,
                IsCargo = false,
                CompTypeToReplace = (int)ESlotType.E_COMP_TURRET,
                SlotNumberToReplace = 2
            }
            );
            num = rand.Next() % 2;
            caravanParts.Add
            (
            new ComponentOverrideData()
            {
                CompType = (int)ESlotType.E_COMP_TURRET,
                CompSubType = TurretModManager.Instance.GetTurretIDFromName("Sylvassi Turret"),
                ReplaceExistingComp = true,
                CompLevel = 1 + num,
                IsCargo = false,
                CompTypeToReplace = (int)ESlotType.E_COMP_TURRET,
                SlotNumberToReplace = 3
            }
            );
            num = rand.Next() % 2;
            caravanParts.Add
            (
            new ComponentOverrideData()
            {
                CompType = (int)ESlotType.E_COMP_TURRET,
                CompSubType = (int)ETurretType.SPREAD,
                ReplaceExistingComp = true,
                CompLevel = 5 + num,
                IsCargo = false,
                CompTypeToReplace = (int)ESlotType.E_COMP_TURRET,
                SlotNumberToReplace = 4
            }
            );
            num = rand.Next() % 2;
            caravanParts.Add
            (
            new ComponentOverrideData()
            {
                CompType = (int)ESlotType.E_COMP_TURRET,
                CompSubType = (int)ETurretType.SPREAD,
                ReplaceExistingComp = true,
                CompLevel = 5 + num,
                IsCargo = false,
                CompTypeToReplace = (int)ESlotType.E_COMP_TURRET,
                SlotNumberToReplace = 5
            }
            );
            num = rand.Next() % 2;
            caravanParts.Add
            (
            new ComponentOverrideData()
            {
                CompType = (int)ESlotType.E_COMP_MAINTURRET,
                CompSubType = 3,
                ReplaceExistingComp = true,
                CompLevel = 2 + num,
                IsCargo = false,
                CompTypeToReplace = (int)ESlotType.E_COMP_MAINTURRET,
                SlotNumberToReplace = 0
            }
            );
            num = rand.Next() % 2;
            caravanParts.Add
            (
            new ComponentOverrideData()
            {
                CompType = (int)ESlotType.E_COMP_AUTO_TURRET,
                CompSubType = AutoTurretModManager.Instance.GetAutoTurretIDFromName("Auto Laser Turret"),
                ReplaceExistingComp = true,
                CompLevel = 1 + num,
                IsCargo = false,
                CompTypeToReplace = (int)ESlotType.E_COMP_AUTO_TURRET,
                SlotNumberToReplace = 0
            }
            );
            num = rand.Next() % 2;
            caravanParts.Add
            (
            new ComponentOverrideData()
            {
                CompType = (int)ESlotType.E_COMP_AUTO_TURRET,
                CompSubType = AutoTurretModManager.Instance.GetAutoTurretIDFromName("Auto Laser Turret"),
                ReplaceExistingComp = true,
                CompLevel = 1 + num,
                IsCargo = false,
                CompTypeToReplace = (int)ESlotType.E_COMP_AUTO_TURRET,
                SlotNumberToReplace = 1
            }
            );
            num = rand.Next() % 2;
            caravanParts.Add
            (
            new ComponentOverrideData()
            {
                CompType = (int)ESlotType.E_COMP_AUTO_TURRET,
                CompSubType = AutoTurretModManager.Instance.GetAutoTurretIDFromName("Auto Laser Turret"),
                ReplaceExistingComp = true,
                CompLevel = 1 + num,
                IsCargo = false,
                CompTypeToReplace = (int)ESlotType.E_COMP_AUTO_TURRET,
                SlotNumberToReplace = 2
            }
            );
            num = rand.Next() % 2;
            caravanParts.Add
            (
            new ComponentOverrideData()
            {
                CompType = (int)ESlotType.E_COMP_AUTO_TURRET,
                CompSubType = AutoTurretModManager.Instance.GetAutoTurretIDFromName("Auto Laser Turret"),
                ReplaceExistingComp = true,
                CompLevel = 1 + num,
                IsCargo = false,
                CompTypeToReplace = (int)ESlotType.E_COMP_AUTO_TURRET,
                SlotNumberToReplace = 3
            }
            );
            num = rand.Next() % 2;
            caravanParts.Add
            (
            new ComponentOverrideData()
            {
                CompType = (int)ESlotType.E_COMP_TRACKERMISSILE,
                CompSubType = (int)ETrackerMissileType.BIO,
                ReplaceExistingComp = true,
                CompLevel = 1 + num,
                IsCargo = false,
                CompTypeToReplace = (int)ESlotType.E_COMP_TRACKERMISSILE,
                SlotNumberToReplace = 0
            }
            );
            num = rand.Next() % 2;
            caravanParts.Add
            (
            new ComponentOverrideData()
            {
                CompType = (int)ESlotType.E_COMP_TRACKERMISSILE,
                CompSubType = (int)ETrackerMissileType.FB_MISSILE,
                ReplaceExistingComp = true,
                CompLevel = 1 + num,
                IsCargo = false,
                CompTypeToReplace = (int)ESlotType.E_COMP_TRACKERMISSILE,
                SlotNumberToReplace = 1
            }
            );
            num = rand.Next() % 2;
            caravanParts.Add
            (
            new ComponentOverrideData()
            {
                CompType = (int)ESlotType.E_COMP_NUCLEARDEVICE,
                CompSubType = (int)ENuclearDeviceType.WD_LARGE,
                ReplaceExistingComp = true,
                CompLevel = num,
                IsCargo = false,
                CompTypeToReplace = (int)ESlotType.E_COMP_NUCLEARDEVICE,
                SlotNumberToReplace = 0
            }
            );
            caravanParts.Add
            (
            new ComponentOverrideData()
            {
                CompType = (int)ESlotType.E_COMP_PROGRAM,
                CompSubType = (int)EWarpDriveProgramType.REMOVE_ALL_VIRUSES,
                ReplaceExistingComp = true,
                CompLevel = 0,
                IsCargo = false,
                CompTypeToReplace = (int)ESlotType.E_COMP_PROGRAM,
                SlotNumberToReplace = 0
            }
            );
            caravanParts.Add
            (
            new ComponentOverrideData()
            {
                CompType = (int)ESlotType.E_COMP_PROGRAM,
                CompSubType = (int)EWarpDriveProgramType.SUPER_SHIELD_BOOSTER,
                ReplaceExistingComp = true,
                CompLevel = 0,
                IsCargo = false,
                CompTypeToReplace = (int)ESlotType.E_COMP_PROGRAM,
                SlotNumberToReplace = 1
            }
            );
            caravanParts.Add
            (
            new ComponentOverrideData()
            {
                CompType = (int)ESlotType.E_COMP_PROGRAM,
                CompSubType = (int)EWarpDriveProgramType.SUPER_SHIELD_BOOSTER,
                ReplaceExistingComp = true,
                CompLevel = 0,
                IsCargo = false,
                CompTypeToReplace = (int)ESlotType.E_COMP_PROGRAM,
                SlotNumberToReplace = 2
            }
            );
            caravanParts.Add
            (
            new ComponentOverrideData()
            {
                CompType = (int)ESlotType.E_COMP_PROGRAM,
                CompSubType = (int)EWarpDriveProgramType.BARRAGE,
                ReplaceExistingComp = true,
                CompLevel = 0,
                IsCargo = false,
                CompTypeToReplace = (int)ESlotType.E_COMP_PROGRAM,
                SlotNumberToReplace = 3
            }
            );
            caravanParts.Add
            (
            new ComponentOverrideData()
            {
                CompType = (int)ESlotType.E_COMP_PROGRAM,
                CompSubType = (int)EWarpDriveProgramType.BARRAGE,
                ReplaceExistingComp = true,
                CompLevel = 0,
                IsCargo = false,
                CompTypeToReplace = (int)ESlotType.E_COMP_PROGRAM,
                SlotNumberToReplace = 4
            }
            );
            caravanParts.Add
            (
            new ComponentOverrideData()
            {
                CompType = (int)ESlotType.E_COMP_PROGRAM,
                CompSubType = (int)EWarpDriveProgramType.SYBER_THREAT,
                ReplaceExistingComp = true,
                CompLevel = 0,
                IsCargo = false,
                CompTypeToReplace = (int)ESlotType.E_COMP_PROGRAM,
                SlotNumberToReplace = 5
            }
            );
            caravanParts.Add
            (
            new ComponentOverrideData()
            {
                CompType = (int)ESlotType.E_COMP_PROGRAM,
                CompSubType = (int)WarpDriveProgramModManager.Instance.GetWarpDriveProgramIDFromName("H.E.A.T."),
                ReplaceExistingComp = true,
                CompLevel = 0,
                IsCargo = false,
                CompTypeToReplace = (int)ESlotType.E_COMP_PROGRAM,
                SlotNumberToReplace = 6
            }
            );
            caravanParts.Add
            (
            new ComponentOverrideData()
            {
                CompType = (int)ESlotType.E_COMP_PROGRAM,
                CompSubType = (int)EWarpDriveProgramType.OVERCHARGE,
                ReplaceExistingComp = true,
                CompLevel = 0,
                IsCargo = false,
                CompTypeToReplace = (int)ESlotType.E_COMP_PROGRAM,
                SlotNumberToReplace = 7
            }
            );
            caravanParts.Add
            (
            new ComponentOverrideData()
            {
                CompType = (int)ESlotType.E_COMP_PROGRAM,
                CompSubType = (int)EWarpDriveProgramType.FULL_HEAL_ALL_SYSTEMS,
                ReplaceExistingComp = true,
                CompLevel = 0,
                IsCargo = false,
                CompTypeToReplace = (int)ESlotType.E_COMP_PROGRAM,
                SlotNumberToReplace = 8
            }
            );
            caravanParts.Add
            (
            new ComponentOverrideData()
            {
                CompType = (int)ESlotType.E_COMP_PROGRAM,
                CompSubType = (int)EWarpDriveProgramType.DIG_COOLANT,
                ReplaceExistingComp = true,
                CompLevel = 0,
                IsCargo = false,
                CompTypeToReplace = (int)ESlotType.E_COMP_PROGRAM,
                SlotNumberToReplace = 9
            }
            );
            num = rand.Next() % 2;
            caravanParts.Add
            (
            new ComponentOverrideData()
            {
                CompType = (int)ESlotType.E_COMP_WARP,
                CompSubType = (int)EWarpDriveType.E_WARPDR_SNAPDRIVE,
                ReplaceExistingComp = true,
                CompLevel = 2 + num,
                IsCargo = false,
                CompTypeToReplace = (int)ESlotType.E_COMP_WARP,
                SlotNumberToReplace = 0
            }
            );
            num = rand.Next() % 2;
            caravanParts.Add
            (
            new ComponentOverrideData()
            {
                CompType = (int)ESlotType.E_COMP_THRUSTER,
                CompSubType = (int)EThrusterType.E_THRUSTER_PERF,
                ReplaceExistingComp = true,
                CompLevel = num,
                IsCargo = false,
                CompTypeToReplace = (int)ESlotType.E_COMP_THRUSTER,
                SlotNumberToReplace = 0
            }
            );
            num = rand.Next() % 2;
            caravanParts.Add
            (
            new ComponentOverrideData()
            {
                CompType = (int)ESlotType.E_COMP_THRUSTER,
                CompSubType = (int)EThrusterType.E_THRUSTER_PERF,
                ReplaceExistingComp = true,
                CompLevel = num,
                IsCargo = false,
                CompTypeToReplace = (int)ESlotType.E_COMP_THRUSTER,
                SlotNumberToReplace = 1
            }
            );
            num = rand.Next() % 2;
            caravanParts.Add
            (
            new ComponentOverrideData()
            {
                CompType = (int)ESlotType.E_COMP_THRUSTER,
                CompSubType = (int)EThrusterType.E_THRUSTER_PERF,
                ReplaceExistingComp = true,
                CompLevel = num,
                IsCargo = false,
                CompTypeToReplace = (int)ESlotType.E_COMP_THRUSTER,
                SlotNumberToReplace = 2
            }
            );
            num = rand.Next() % 2;
            caravanParts.Add
            (
            new ComponentOverrideData()
            {
                CompType = (int)ESlotType.E_COMP_THRUSTER,
                CompSubType = (int)EThrusterType.E_THRUSTER_PERF,
                ReplaceExistingComp = true,
                CompLevel = num,
                IsCargo = false,
                CompTypeToReplace = (int)ESlotType.E_COMP_THRUSTER,
                SlotNumberToReplace = 3
            }
            );
            num = rand.Next() % 2;
            caravanParts.Add
            (
            new ComponentOverrideData()
            {
                CompType = (int)ESlotType.E_COMP_INERTIA_THRUSTER,
                CompSubType = (int)EInertiaThrusterType.E_NORMAL,
                ReplaceExistingComp = true,
                CompLevel = 1 + num,
                IsCargo = false,
                CompTypeToReplace = (int)ESlotType.E_COMP_INERTIA_THRUSTER,
                SlotNumberToReplace = 0
            }
            );
            num = rand.Next() % 2;
            caravanParts.Add
            (
            new ComponentOverrideData()
            {
                CompType = (int)ESlotType.E_COMP_INERTIA_THRUSTER,
                CompSubType = (int)EInertiaThrusterType.E_NORMAL,
                ReplaceExistingComp = true,
                CompLevel = 1 + num,
                IsCargo = false,
                CompTypeToReplace = (int)ESlotType.E_COMP_INERTIA_THRUSTER,
                SlotNumberToReplace = 1
            }
            );
            num = rand.Next() % 2;
            caravanParts.Add
            (
            new ComponentOverrideData()
            {
                CompType = (int)ESlotType.E_COMP_MANEUVER_THRUSTER,
                CompSubType = (int)EManeuverThrusterType.E_HEAVY,
                ReplaceExistingComp = true,
                CompLevel = num,
                IsCargo = false,
                CompTypeToReplace = (int)ESlotType.E_COMP_MANEUVER_THRUSTER,
                SlotNumberToReplace = 0
            }
            );
            return caravanParts;
        }
    }
}
