using CodeStage.AntiCheat.ObscuredTypes;
using HarmonyLib;
using PulsarModLoader;
using PulsarModLoader.Content.Components.AutoTurret;
using PulsarModLoader.Content.Components.CaptainsChair;
using PulsarModLoader.Content.Components.CPU;
using PulsarModLoader.Content.Components.Hull;
using PulsarModLoader.Content.Components.HullPlating;
using PulsarModLoader.Content.Components.InertiaThruster;
using PulsarModLoader.Content.Components.MegaTurret;
using PulsarModLoader.Content.Components.Missile;
using PulsarModLoader.Content.Components.MissionShipComponent;
using PulsarModLoader.Content.Components.Reactor;
using PulsarModLoader.Content.Components.Shield;
using PulsarModLoader.Content.Components.Turret;
using PulsarModLoader.Content.Components.WarpDriveProgram;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using static ExpandedGalaxy.DistressSignal;

namespace ExpandedGalaxy
{
    internal class Relic
    {
        internal static ulong RelicData = 0U;
        static readonly int MaxRelicType = 10;
        private static Color relicColor = new Color(85f / 255f, 0f, 255f / 255f);

        public class RelicRewardMod : MissionShipComponentMod
        {
            public override string Name => "Reward";

            public override string Description => "A gift from the Caravan for delivering a prized Junk Cube. Process it to find out what's inside!";

            public override int CargoVisualID => 1;
        }
        public static PLShipComponent GenerateRelic(int seed)
        {
            PLRand rand = new PLRand(seed);
            int type = rand.Next() & MaxRelicType;
            int num = 0;
            while (num < MaxRelicType)
            {
                if (HasRelic(type))
                {
                    type = rand.Next(MaxRelicType);
                    ++num;
                }
                else
                    break;
            }
            if (num == MaxRelicType)
                return new PLScrapCargo();
            PLShipComponent component;
            switch (type)
            {
                case 1:
                    component = PLHull.CreateHullFromHash(HullModManager.Instance.GetHullIDFromName("Juggernaut Hull"), 0, 0);
                    break;
                case 2:
                    component = PLSensorDish.CreateSensorDishFromHash(1, 0, 0);
                    break;
                case 3:
                    component = PLReactor.CreateReactorFromHash(ReactorModManager.Instance.GetReactorIDFromName("Dark-Matter Reactor"), 0, 0);
                    break;
                case 4:
                    component = PLCaptainsChair.CreateCaptainsChairFromHash(CaptainsChairModManager.Instance.GetCaptainsChairIDFromName("Seat of the Surveyor"), 0, 0);
                    component.SubTypeData = 0;
                    break;
                case 5:
                    component = PLMegaTurret.CreateMainTurretFromHash(MegaTurretModManager.Instance.GetMegaTurretIDFromName("Imperial Glaive"), 0, 0);
                    break;
                case 6:
                    component = PLWarpDriveProgram.CreateWarpDriveProgramFromHash(WarpDriveProgramModManager.Instance.GetWarpDriveProgramIDFromName("Digital Earthquake"), 0, 0);
                    break;
                case 7:
                    component = PLWarpDriveProgram.CreateWarpDriveProgramFromHash(WarpDriveProgramModManager.Instance.GetWarpDriveProgramIDFromName("Corruption [VIRUS]"), 0, 0);
                    break;
                case 8:
                    component = PLCPU.CreateCPUFromHash(CPUModManager.Instance.GetCPUIDFromName("Sylvassi Shield Charger"), 0, 0);
                    component.SubTypeData = 0;
                    break;
                case 9:
                    component = PLInertiaThruster.CreateInertiaThrusterFromHash(InertiaThrusterModManager.Instance.GetInertiaThrusterIDFromName("Integrated Stabilizer Thruster"), 0, 0);
                    break;
                default:
                    component = PLShieldGenerator.CreateShieldGeneratorFromHash(ShieldModManager.Instance.GetShieldIDFromName("Anti-Matter Suspension Field"), 0, 0);
                    break;
            }
            RelicData = RelicData | (1U << type);
            return component;
        }
        public static bool HasRelic(int relicType)
        {
            return (RelicData & (1U << relicType)) > 0U;
        }

        [HarmonyPatch(typeof(PLPersistantEncounterInstance), "OnEndWarp")]
        internal class RelicCompPickups
        {
            private static void Postfix(PLPersistantEncounterInstance __instance)
            {
                if (!PhotonNetwork.isMasterClient)
                    return;
                if (__instance is PLWastedWingEncounter || __instance is PLAOGFactionMission_MadmansMansion || __instance.LevelID == (ObscuredInt)113)
                {
                    if (PLServer.GetCurrentSector() != null && !PLServer.GetCurrentSector().Visited && PhotonNetwork.isMasterClient)
                    {
                        int[] hashes = new int[1]
                        {
                            (int)Relic.GenerateRelic(PLServer.Instance.GalaxySeed).getHash()
                        };
                        Relic.AddCompToPlanet(__instance, hashes, true, true);
                    }
                }
            }
        }

        private class AddCompToPlanetRPC : ModMessage
        {
            public override void HandleRPC(object[] arguments, PhotonMessageInfo sender)
            {
                if (!sender.sender.IsMasterClient)
                    return;
                if (PLEncounterManager.Instance == null || PLEncounterManager.Instance.GetPersistantEncounterInstanceAtID((int)arguments[0]) == null)
                    return;
                Relic.AddCompToPlanet(PLEncounterManager.Instance.GetPersistantEncounterInstanceAtID((int)arguments[0]), (int[])arguments[1], (bool)arguments[2], (bool)arguments[3], (bool)arguments[4]);
            }
        }

        public static async void AddCompToPlanet(PLPersistantEncounterInstance pei, int[] compHashes, bool replace = true, bool randomPlacing = true, bool checkParent = false)
        {
            await Task.Delay(100);
            while (PLNetworkManager.Instance.CurrentGame == null || !pei.GameInitWithHubID || PLEncounterManager.Instance.GetCPEI() != pei)
                await Task.Yield();
            await Task.Delay(100);
            if (PhotonNetwork.isMasterClient)
                ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.AddCompToPlanetRPC", PhotonTargets.Others, new object[5]
                {
                    pei.GetSectorID(),
                    compHashes,
                    replace,
                    randomPlacing,
                    checkParent
                });
            PLGamePlanet current = PLNetworkManager.Instance.CurrentGame as PLGamePlanet;
            Transform[] objects = current.PlanetRoot.GetComponentsInChildren<Transform>(true);
            List<Transform> cargoBaseTransforms = new List<Transform>();
            foreach (Transform obj in objects)
            {
                if (obj != null)
                {
                    if (obj.name.Contains("Cargo_Base_"))
                    {
                        if (obj.gameObject != null && obj.gameObject.activeSelf && !cargoBaseTransforms.Contains(obj) && !obj.gameObject.TryGetComponent<PLPickupComponent>(out PLPickupComponent component))
                        {
                            if (obj.parent != null)
                            {
                                Transform parent = obj.parent;
                                bool flag = false;
                                while (true)
                                {
                                    if (parent == null)
                                        break;
                                    if (parent.gameObject != null & !parent.gameObject.activeSelf)
                                    {
                                        flag = true;
                                        break;
                                    }
                                    parent = parent.parent;

                                }
                                if (flag)
                                    continue;
                            }
                            if (obj.gameObject.TryGetComponent<PLPickupRandomComponent>(out PLPickupRandomComponent component1))
                            {
                                if (replace)
                                {
                                    Traverse traverse = Traverse.Create(component1);
                                    component1.PickedUp = true;
                                    traverse.Field("HasSetupVisualObject").SetValue(true);
                                    if (PLGameStatic.Instance.m_AllPickupRandomComponents.Contains(component1))
                                        PLGameStatic.Instance.m_AllPickupRandomComponents.Remove(component1);
                                    if (pei.MyPersistantData.PickupRandomComponentPersistantData.ContainsKey((ObscuredInt)component1.PickupID))
                                        pei.MyPersistantData.PickupRandomComponentPersistantData.Remove((ObscuredInt)component1.PickupID);
                                    UnityEngine.Object.Destroy(component1);
                                    for (int i = 0; i < obj.childCount; i++)
                                    {
                                        UnityEngine.Object.Destroy(obj.GetChild(i).gameObject, 2f);
                                    }
                                    cargoBaseTransforms.Add(obj);
                                }
                            }
                            else
                            {
                                cargoBaseTransforms.Add(obj);
                            }
                        }
                    }
                }
            }
            await Task.Delay(100);
            if (!(cargoBaseTransforms.Count > 0))
            {
                PulsarModLoader.Utilities.Logger.Info(string.Format("Could not find open cargo base at sector {0}! ({1})", PLServer.GetCurrentSector().ID.ToString(), PLServer.GetCurrentSector().VisualIndication.ToString()));
                return;
            }
            int minimumFreePickupID = 0;
            foreach (int key in pei.MyPersistantData.PickupRandomComponentPersistantData.Keys)
                if (key > minimumFreePickupID)
                    minimumFreePickupID = key;
            if (randomPlacing)
            {
                PLRand rand = new PLRand(PLServer.GetCurrentSector().ID + PLServer.Instance.GalaxySeed);
                int num = 0;
                while (cargoBaseTransforms.Count > 0 && compHashes.Length > num)
                {
                    if (compHashes[num] != -1)
                    {
                        int num1 = rand.Next(0, cargoBaseTransforms.Count);
                        PLPickupRandomComponent pLPickupRandom = cargoBaseTransforms[num1].gameObject.AddComponent<PLPickupRandomComponent>();
                        Traverse traverse = Traverse.Create(pLPickupRandom);
                        pLPickupRandom.MyPEI = pei;
                        pLPickupRandom.PickedUp = false;
                        pLPickupRandom.RandComp = compHashes[num];
                        pLPickupRandom.RandCompSetup = true;
                        traverse.Field("m_InternalComp").SetValue(PLShipComponent.CreateShipComponentFromHash(pLPickupRandom.RandComp));
                        PLShipComponent shipComponent = traverse.Field("m_InternalComp").GetValue<PLShipComponent>();
                        if (shipComponent != null && shipComponent.CargoVisualPrefabID > PLGlobal.Instance.CargoVisualPrefabs.Length)
                            shipComponent.CargoVisualPrefabID = 1;
                        pLPickupRandom.PickupID = ++minimumFreePickupID;
                        pei.MyPersistantData.PickupRandomComponentPersistantData.Add((ObscuredInt)pLPickupRandom.PickupID, (ObscuredBool)pLPickupRandom.PickedUp);
                        pLPickupRandom.MyInterior = pLPickupRandom.GetComponentInParent<PLInterior>();
                        PLGameStatic.Instance.m_AllPickupRandomComponents.Add(pLPickupRandom);
                        cargoBaseTransforms.Remove(cargoBaseTransforms[num1]);
                    }
                    ++num;
                }
                if (compHashes.Length > num)
                    PulsarModLoader.Utilities.Logger.Info(string.Format("Could not place {0} components at sector {1}! ({2})", (compHashes.Length - num).ToString(), PLServer.GetCurrentSector().ID.ToString(), PLServer.GetCurrentSector().VisualIndication.ToString()));
            }
            else
            {
                int num = 0;
                while (cargoBaseTransforms.Count > 0 && compHashes.Length > num)
                {
                    if (compHashes[num] != -1)
                    {
                        PLPickupRandomComponent pLPickupRandom = cargoBaseTransforms[num].gameObject.AddComponent<PLPickupRandomComponent>();
                        pLPickupRandom.MyPEI = pei;
                        pLPickupRandom.PickedUp = false;
                        pLPickupRandom.RandComp = compHashes[num];
                        pLPickupRandom.RandCompSetup = true;
                        pLPickupRandom.PickupID = ++minimumFreePickupID;
                        pei.MyPersistantData.PickupRandomComponentPersistantData.Add((ObscuredInt)pLPickupRandom.PickupID, (ObscuredBool)pLPickupRandom.PickedUp);
                        pLPickupRandom.MyInterior = pLPickupRandom.GetComponentInParent<PLInterior>();
                        PLGameStatic.Instance.m_AllPickupRandomComponents.Add(pLPickupRandom);
                        cargoBaseTransforms.Remove(cargoBaseTransforms[num]);
                    }
                    ++num;
                }
                if (compHashes.Length > num)
                    PulsarModLoader.Utilities.Logger.Info(string.Format("Could not place {0} components at sector {1}!", (compHashes.Length - num).ToString(), PLServer.GetCurrentSector().ID.ToString()));
            }
        }

        public static bool getIsRelic(PLWare ware)
        {
            PLShieldGenerator shield = ware as PLShieldGenerator;
            if (shield != null)
            {
                if (shield.SubType == ShieldModManager.Instance.GetShieldIDFromName("Anti-Matter Suspension Field"))
                {
                    return true;
                }
            }
            PLHull hull = ware as PLHull;
            if (hull != null)
            {
                if (hull.SubType == HullModManager.Instance.GetHullIDFromName("Juggernaut Hull"))
                {
                    return true;
                }
            }
            PLReactor reactor = ware as PLReactor;
            if (reactor != null)
            {
                if (reactor.SubType == ReactorModManager.Instance.GetReactorIDFromName("Dark-Matter Reactor"))
                {
                    return true;
                }
            }
            PLCaptainsChair chair = ware as PLCaptainsChair;
            if (chair != null)
            {
                if (chair.SubType == CaptainsChairModManager.Instance.GetCaptainsChairIDFromName("Seat of the Surveyor"))
                {
                    return true;
                }
            }
            PLMegaTurret megaTurret = ware as PLMegaTurret;
            if (megaTurret != null)
            {
                if (megaTurret.SubType == MegaTurretModManager.Instance.GetMegaTurretIDFromName("Imperial Glaive"))
                {
                    return true;
                }
            }
            PLTurret turret = ware as PLTurret;
            if (turret != null)
            {
                if (turret.ActualSlotType == ESlotType.E_COMP_TURRET && turret.SubType == TurretModManager.Instance.GetTurretIDFromName("Mining Laser"))
                {
                    return true;
                }
                else if (turret.ActualSlotType == ESlotType.E_COMP_AUTO_TURRET && turret.SubType == AutoTurretModManager.Instance.GetAutoTurretIDFromName("Ancient Auto Laser Turret"))
                {
                    return true;
                }
            }
            PLSensorDish sensorDish = ware as PLSensorDish;
            if (sensorDish != null)
            {
                if (sensorDish.SubType == 1)
                {
                    return true;
                }
            }
            PLCPU cpu = ware as PLCPU;
            if (cpu != null)
            {
                if (cpu.SubType == CPUModManager.Instance.GetCPUIDFromName("Sylvassi Shield Charger"))
                {
                    return true;
                }
            }
            PLWarpDriveProgram warpDriveProgram = ware as PLWarpDriveProgram;
            if (warpDriveProgram != null)
            {
                if (warpDriveProgram.SubType == WarpDriveProgramModManager.Instance.GetWarpDriveProgramIDFromName("Digital Earthquake"))
                    return true;
                else if (warpDriveProgram.SubType == WarpDriveProgramModManager.Instance.GetWarpDriveProgramIDFromName("Corruption [VIRUS]"))
                    return true;
                else if (warpDriveProgram.SubType == WarpDriveProgramModManager.Instance.GetWarpDriveProgramIDFromName("Syber Swap"))
                    return true;
            }
            PLInertiaThruster inertiaThruster = ware as PLInertiaThruster;
            if (inertiaThruster != null)
            {
                if (inertiaThruster.SubType == InertiaThrusterModManager.Instance.GetInertiaThrusterIDFromName("Integrated Stabilizer Thruster"))
                    return true;
            }
            PLDistressSignal signal = ware as PLDistressSignal;
            if (signal != null)
            {
                if (signal.SubType == 4)
                {
                    return true;
                }
            }
            return false;
        }

        public static Color getRelicColor()
        {
            return relicColor;
        }

        [HarmonyPatch(typeof(PLGlobal), "GetColorBGForWare")]
        internal class RelicColor
        {
            private static void Postfix(PLGlobal __instance, PLWare ware, ref Color __result)
            {
                if (ware != null)
                {
                    if (Relic.getIsRelic(ware))
                    {
                        __result = Relic.getRelicColor();
                    }
                    else if (ware is PLMissionShipComponent && (ware as PLMissionShipComponent).SubType == MissionShipComponentModManager.Instance.GetMissionShipComponentIDFromName("Ammunition Cache"))
                        __result = PLGlobal.Instance.Galaxy.FactionColors[2];
                }
            }
        }

        [HarmonyPatch(typeof(PLTooltipRequest), "Draw")]
        internal class RelicText
        {
            private static void Postfix(PLTooltipRequest __instance, ref Rect ___tooltipRect)
            {
                if (__instance.Ware != null)
                {
                    if (Relic.getIsRelic(__instance.Ware))
                    {
                        GUI.color = Relic.getRelicColor();
                        GUI.Label(new Rect(___tooltipRect.x + 10f, ___tooltipRect.y + 12f, 420f - 20f, 50f), "RELIC", PLGlobal.Instance.guiStyleComponentMenuExtraLineRight);
                    }
                }
            }
        }

        internal class MiningDroneQuest
        {
            internal static bool dronesActive = true;
            internal static int GXData = 0;

            [HarmonyPatch(typeof(PLGalaxy), "CreateStartingShips")]
            internal class SpawnMiningDrones
            {
                private static bool Prefix(PLGalaxy __instance, ref Dictionary<int, PLSectorInfo> ___m_AllSectorInfos, out Dictionary<int, PLSectorInfo> __state)
                {
                    __state = new Dictionary<int, PLSectorInfo>();
                    if (PLServer.Instance == null)
                        return true;
                    PLRand rand = new PLRand(__instance.Seed);
                    foreach (int sectorNum in ___m_AllSectorInfos.Keys)
                    {
                        if (___m_AllSectorInfos[sectorNum].VisualIndication == ESectorVisualIndication.NONE && ___m_AllSectorInfos[sectorNum].MySPI != null && ___m_AllSectorInfos[sectorNum].MySPI.Faction != 4 && rand.Next(100) > 80)
                        {
                            __state.Add(sectorNum, ___m_AllSectorInfos[sectorNum]);
                        }
                        if (___m_AllSectorInfos[sectorNum].VisualIndication == ESectorVisualIndication.LAVA2)
                        {
                            ___m_AllSectorInfos[sectorNum].Discovered = false;
                            ___m_AllSectorInfos[sectorNum].Name = sectorNum.ToString();
                            ___m_AllSectorInfos[sectorNum].MySPI.Faction = 6;
                            for (int i = 0; i < 3; i++)
                            {
                                PLPersistantShipInfo pLPersistantShipInfo = new PLPersistantShipInfo(EShipType.E_WDDRONE2, 6, ___m_AllSectorInfos[sectorNum])
                                {
                                    ShipName = "Guardian Drone",
                                    HullPercent = 1f,
                                    ShldPercent = 1f,
                                    IsFlagged = true,
                                    ForcedHostile = true,
                                };
                                pLPersistantShipInfo.CompOverrides.AddRange((IEnumerable<ComponentOverrideData>)GetComponentsFromDroneType(2, rand));
                                PLServer.Instance.AllPSIs.Add(pLPersistantShipInfo);
                            }
                        }
                    }
                    foreach (int sectorNum in __state.Keys)
                    {
                        if (___m_AllSectorInfos.ContainsKey(sectorNum))
                            ___m_AllSectorInfos.Remove(sectorNum);
                    }
                    return true;
                }

                private static void Postfix(PLGalaxy __instance, ref Dictionary<int, PLSectorInfo> ___m_AllSectorInfos, Dictionary<int, PLSectorInfo> __state)
                {
                    if (PLServer.Instance == null)
                        return;
                    PLRand rand = new PLRand(__instance.Seed);
                    foreach (int sectorNum in __state.Keys)
                    {
                        PLSectorInfo sectorInfo = __state[sectorNum];
                        for (int i = 0; i < 1 + rand.Next(3); i++)
                        {
                            PLPersistantShipInfo pLPersistantShipInfo = new PLPersistantShipInfo(EShipType.E_WDDRONE1, 6, sectorInfo)
                            {
                                ShipName = "Mining Drone",
                                HullPercent = rand.Next(0.85f, 1f),
                                ShldPercent = 1f,
                                IsFlagged = true
                            };
                            pLPersistantShipInfo.CompOverrides.AddRange((IEnumerable<ComponentOverrideData>)GetComponentsFromDroneType(0, rand));
                            PLServer.Instance.AllPSIs.Add(pLPersistantShipInfo);
                        }
                        for (int i = 1; i < rand.Next(4); i++)
                        {
                            PLPersistantShipInfo pLPersistantShipInfo = new PLPersistantShipInfo(EShipType.E_WDDRONE2, 6, sectorInfo)
                            {
                                ShipName = "Escort Drone",
                                HullPercent = rand.Next(0.8f, 1f),
                                ShldPercent = 1f,
                                IsFlagged = true,

                            };
                            pLPersistantShipInfo.CompOverrides.AddRange((IEnumerable<ComponentOverrideData>)GetComponentsFromDroneType(1, rand));
                            PLServer.Instance.AllPSIs.Add(pLPersistantShipInfo);
                        }
                        ___m_AllSectorInfos.Add(sectorNum, sectorInfo);
                    }
                }
            }

            [HarmonyPatch(typeof(PLStarmap), "ShouldShowSector")]
            private class SectorOnStarMap
            {
                private static void Postfix(PLSectorInfo sectorInfo, ref bool __result)
                {
                    if (sectorInfo == null)
                        return;
                    if (sectorInfo.VisualIndication == ESectorVisualIndication.ALCHEMIST && sectorInfo.MySPI.Faction == 5)
                        __result = true;
                    if (sectorInfo.VisualIndication == ESectorVisualIndication.LAVA2)
                    {
                        __result = false;
                        if (sectorInfo.Visited || PLServer.Instance.m_ShipCourseGoals.Contains(sectorInfo.ID))
                            __result = true;
                        if (PLEncounterManager.Instance != null && PLEncounterManager.Instance.PlayerShip != null && PLEncounterManager.Instance.PlayerShip.DistressSignalActive)
                        {
                            PLDistressSignal component = PLEncounterManager.Instance.PlayerShip.MyStats.GetComponentFromNetID<PLDistressSignal>(PLEncounterManager.Instance.PlayerShip.SelectedDistressSignalNetID);
                            if (component != null && component.SubType == 4 && sectorInfo.IsThisSectorWithinPlayerWarpRange())
                            {
                                __result = true;
                                return;
                            }
                        }
                    }
                }
            }

            [HarmonyPatch(typeof(PLStarmap), "ShouldShowSectorBG")]
            private class SectorDotsOnStarMap
            {
                private static void Postfix(PLSectorInfo sectorInfo, ref bool __result)
                {
                    if (sectorInfo == null)
                        return;
                    if (sectorInfo.VisualIndication == ESectorVisualIndication.ALCHEMIST && sectorInfo.MySPI.Faction == 5)
                        __result = true;
                    if (sectorInfo.VisualIndication == ESectorVisualIndication.LAVA2)
                    {
                        __result = false;
                        if (sectorInfo.Visited || PLServer.Instance.m_ShipCourseGoals.Contains(sectorInfo.ID))
                            __result = true;
                        if (PLEncounterManager.Instance != null && PLEncounterManager.Instance.PlayerShip != null && PLEncounterManager.Instance.PlayerShip.DistressSignalActive)
                        {
                            PLDistressSignal component = PLEncounterManager.Instance.PlayerShip.MyStats.GetComponentFromNetID<PLDistressSignal>(PLEncounterManager.Instance.PlayerShip.SelectedDistressSignalNetID);
                            if (component != null && component.SubType == 4 && sectorInfo.IsThisSectorWithinPlayerWarpRange())
                            {
                                __result = true;
                            }
                        }
                    }
                }
            }

            [HarmonyPatch(typeof(PLShipInfoBase), "ShouldBeHostileToShip")]
            internal class MiningDroneBePassive
            {
                private static void Postfix(PLShipInfoBase __instance, PLShipInfoBase inShip, ref bool __result)
                {
                    if (!PhotonNetwork.isMasterClient || !__instance.IsDrone || __instance.MyStats == null || __instance is PLWarpGuardian || __instance is PLUnseenEye)
                        return;
                    bool flag = false;
                    foreach (PLShipComponent component in __instance.MyStats.GetComponentsOfType(ESlotType.E_COMP_DISTRESS_SIGNAL))
                    {
                        if (component is MiningDroneSignal)
                        {
                            if (!__instance.PersistantShipInfo.ForcedHostile && (component.Level < 4 && __instance.LastTookDamageTime() == float.MinValue) || !Relic.MiningDroneQuest.dronesActive)
                            {
                                flag = true;
                                break;
                            }
                            if (inShip.FactionID == 6)
                            {
                                flag = true;
                                break;
                            }
                        }
                    }
                    if (flag)
                    {
                        __result = false;
                        if (__instance.HostileShips.Contains(inShip.ShipID))
                            __instance.HostileShips.Remove(inShip.ShipID);
                    }
                }
            }

            [HarmonyPatch(typeof(PLShipInfoBase), "LeaveExtraScrap")]
            internal class DropMiningDroneLoot
            {
                private static void Postfix(PLShipInfoBase __instance, ref List<PLShipComponent> droppedShipComponents)
                {
                    if (__instance.GetIsPlayerShip())
                        return;
                    if (__instance.ShipTypeID == EShipType.E_WDDRONE1)
                    {
                        bool flag = false;
                        foreach (PLShipComponent component in __instance.MyStats.GetComponentsOfType(ESlotType.E_COMP_DISTRESS_SIGNAL))
                        {
                            if (component is MiningDroneSignal)
                            {
                                flag = true;
                                break;
                            }
                        }
                        if (!flag)
                            return;
                        bool flag1 = false;
                        foreach (PLShipComponent component in PLEncounterManager.Instance.PlayerShip.MyStats.GetComponentsOfType(ESlotType.E_COMP_DISTRESS_SIGNAL, true))
                        {
                            if (component is MiningDroneSignal)
                            {
                                flag1 = true;
                                break;
                            }
                        }
                        int compHash = -1;
                        if (!flag1 && UnityEngine.Random.Range(0f, 1f) > 0.66f)
                        {
                            compHash = (int)PLShipComponent.createHashFromInfo(22, 4, 0, 0, (int)ESlotType.E_COMP_NONE);
                            PLServer.Instance.photonView.RPC("CreateSpecificShipScrapAtLocation", PhotonTargets.All, __instance.Exterior.transform.position + UnityEngine.Random.onUnitSphere * 20f, __instance.Exterior.transform.position, (int)compHash, true);
                        }
                        else if (UnityEngine.Random.Range(0f, 1f) > 0.9f)
                        {
                            compHash = (int)PLShipComponent.createHashFromInfo(10, TurretModManager.Instance.GetTurretIDFromName("Mining Laser"), 0, 0, (int)ESlotType.E_COMP_NONE);
                            PLServer.Instance.photonView.RPC("CreateSpecificShipScrapAtLocation", PhotonTargets.All, __instance.Exterior.transform.position + UnityEngine.Random.onUnitSphere * 20f, __instance.Exterior.transform.position, (int)compHash, true);
                        }
                    }
                }
            }

            [HarmonyPatch(typeof(PLShipInfoBase), "Update")]
            internal class EscortDroneSpawn
            {
                private static void Postfix(PLShipInfoBase __instance)
                {
                    if (!Relic.MiningDroneQuest.dronesActive || !PhotonNetwork.isMasterClient)
                        return;
                    if (__instance.DistressSignalActive)
                    {
                        bool flag = false;
                        if (__instance.GetIsPlayerShip())
                        {
                            if (PhotonNetwork.isMasterClient && PLEncounterManager.Instance != null && PLEncounterManager.Instance.GetCPEI() != null && PLServer.Instance != null && !__instance.InWarp && PLServer.GetCurrentSector() != null && PLServer.Instance.GetCurrentHubID() > 0 && PLServer.GetCurrentSector().VisualIndication != ESectorVisualIndication.LCWBATTLE && PLServer.GetCurrentSector().VisualIndication != ESectorVisualIndication.TOPSEC && PLServer.GetCurrentSector().VisualIndication != ESectorVisualIndication.LAVA2)
                            {
                                PLDistressSignal component = __instance.MyStats.GetComponentFromNetID<PLDistressSignal>(__instance.SelectedDistressSignalNetID);
                                if (component != null && (component is MiningDroneSignal))
                                {
                                    flag = true;
                                }
                            }
                        }
                        else if (__instance.FactionID == 6 && __instance.ShipTypeID == EShipType.E_WDDRONE1 || __instance.ShipTypeID == EShipType.E_WDDRONE2)
                        {
                            foreach (PLShipComponent component in __instance.MyStats.GetComponentsOfType(ESlotType.E_COMP_DISTRESS_SIGNAL))
                            {
                                if (component is MiningDroneSignal)
                                {
                                    if (component.Level < 4)
                                    {
                                        flag = true;
                                        break;
                                    }
                                }
                            }
                        }
                        if (flag)
                        {
                            int num = 0;
                            foreach (PLPersistantShipInfo allPsI in PLServer.Instance.AllPSIs)
                            {
                                if (allPsI != null && allPsI.MyCurrentSector == PLServer.GetCurrentSector())
                                    ++num;
                            }
                            if (num < 3 || PLServer.GetCurrentSector().VisualIndication == ESectorVisualIndication.NONE && num < 6)
                            {
                                if (UnityEngine.Random.Range(0, 1000 + num * 5000) == 15)
                                {
                                    PLRand rand = new PLRand(PLServer.GetCurrentSector().ID * (1 + num));
                                    PLPersistantShipInfo pLPersistantShipInfo = new PLPersistantShipInfo(EShipType.E_WDDRONE2, 6, PLServer.GetCurrentSector())
                                    {
                                        ShipName = "Escort Drone",
                                        HullPercent = 1f,
                                        ShldPercent = 1f,
                                        IsFlagged = true,
                                        ForcedHostile = true,

                                    };
                                    pLPersistantShipInfo.CompOverrides.AddRange((IEnumerable<ComponentOverrideData>)GetComponentsFromDroneType(1, rand));
                                    Vector3 pos = GetEmptyLocationForEscortDrone(PLEncounterManager.Instance.GetCPEI(), pLPersistantShipInfo);
                                    if (pos != Vector3.zero)
                                    {
                                        PLServer.Instance.AllPSIs.Add(pLPersistantShipInfo);
                                        PLEncounterManager.Instance.GetCPEI().SpawnEnemyShip(pLPersistantShipInfo.Type, pLPersistantShipInfo, spawnPos: pos);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            [HarmonyPatch(typeof(PLShipInfoBase), "Ship_WarpOutNow")]
            private class DisableGuardianDroneWarp
            {
                private static bool Prefix(PLShipInfoBase __instance)
                {
                    if (PLServer.GetCurrentSector() == null || !(PLEncounterManager.Instance.PlayerShip != null))
                        return true;
                    if (__instance.IsDrone && !__instance.HasBeenDestroyed)
                    {
                        foreach (PLShipComponent component in __instance.MyStats.GetComponentsOfType(ESlotType.E_COMP_DISTRESS_SIGNAL))
                        {
                            if (component is MiningDroneSignal)
                            {
                                if (component.Level == 4)
                                    return false;
                                if (!Relic.MiningDroneQuest.dronesActive)
                                    return false;
                                break;
                            }
                        }
                    }
                    return true;
                }
            }

            [HarmonyPatch(typeof(PLPersistantEncounterInstance), "OnEndWarp")]
            internal class MiningHubSetupCall
            {
                private static void Postfix(PLPersistantEncounterInstance __instance)
                {
                    if (__instance is PLLavaPlanet2Encounter)
                    {
                        Relic.MiningDroneQuest.SetupMiningHubPlanet(__instance, PLServer.GetCurrentSector().Visited);
                        if (PLServer.GetCurrentSector() != null && !PLServer.GetCurrentSector().Visited && PhotonNetwork.isMasterClient)
                        {
                            int[] hashes = new int[4]
                            {
                            (int)new PLScrapCargo(9).getHash(),
                            (int)new PLScrapCargo(9).getHash(),
                            (int)new PLScrapCargo(9).getHash(),
                            (int)Relic.GenerateRelic(PLServer.Instance.GalaxySeed).getHash(),
                            };
                            Relic.AddCompToPlanet(__instance, hashes, true, true);
                        }
                    }
                }
            }

            private static async void SetupMiningHubPlanet(PLPersistantEncounterInstance pei, bool visited)
            {
                await Task.Delay(100);
                while (PLNetworkManager.Instance.CurrentGame == null || !pei.GameInitWithHubID || PLEncounterManager.Instance.GetCPEI() != pei)
                    await Task.Yield();
                PLGamePlanet current = PLNetworkManager.Instance.CurrentGame as PLGamePlanet;
                if (current == null)
                {
                    PulsarModLoader.Utilities.Logger.Info(string.Format("Could not find planet at sector {0}! ({1})", PLServer.GetCurrentSector().ID.ToString(), PLServer.GetCurrentSector().VisualIndication.ToString()));
                    return;
                }
                Transform[] objects = current.PlanetRoot.GetComponentsInChildren<Transform>(true);
                foreach (Transform obj in objects)
                {
                    if (obj != null)
                    {
                        if (obj.gameObject != null)
                        {
                            switch (obj.name)
                            {
                                case "LargeDebris_01":
                                case "LargeDebris_01_Deco":
                                    obj.gameObject.SetActive(true);
                                    break;
                                case "Factory_Structure_01":
                                    obj.gameObject.SetActive(false);
                                    break;
                            }
                            if (!visited)
                            {
                                if (obj.name.Contains("HG_77328") || obj.name.Contains("CreepingLung_01"))
                                {
                                    if (obj.gameObject.TryGetComponent<PLPickupObject>(out PLPickupObject component))
                                        component.PickedUp = false;
                                }
                            }
                        }
                    }
                }
            }

            [HarmonyPatch(typeof(PLPersistantPlanetEncounterInstance), "Update")]
            internal class UpdateMiningHubLights
            {
                private static void Postfix(PLPersistantPlanetEncounterInstance __instance)
                {
                    if (PLEncounterManager.Instance == null || PLEncounterManager.Instance.PlayerShip == null || PLEncounterManager.Instance.PlayerShip.Get_IsInWarpMode())
                        return;
                    if (!(__instance is PLLavaPlanet2Encounter))
                        return;
                    if (PLNetworkManager.Instance.CurrentGame == null || !__instance.GameInitWithHubID || PLEncounterManager.Instance.GetCPEI() != __instance)
                        return;
                    bool flag = false;
                    foreach (PLShipInfoBase plShipInfoBase in UnityEngine.Object.FindObjectsOfType(typeof(PLShipInfoBase)))
                    {
                        if (plShipInfoBase != null && plShipInfoBase.ShipTypeID == EShipType.E_WDDRONE2)
                        {
                            foreach (PLShipComponent component in plShipInfoBase.MyStats.GetComponentsOfType(ESlotType.E_COMP_DISTRESS_SIGNAL))
                            {
                                if (component is MiningDroneSignal)
                                {
                                    flag = true;
                                    break;
                                }
                            }
                        }
                    }
                    if (flag && !PLMusic.Instance.SpecialMusicPlaying)
                        PLMusic.Instance.PlayMusic("mx_infected_commander", true, false, true, true);
                    if (!flag && PLMusic.Instance.SpecialMusicPlaying)
                        PLMusic.Instance.StopCurrentMusic();
                    PLGamePlanet currentGame = PLNetworkManager.Instance.CurrentGame as PLGamePlanet;
                    PLInterior interior = currentGame.PlanetRoot.GetComponentInChildren<PLInterior>();
                    if (interior == null)
                        return;
                    for (int i = 5; i < interior.Lights.Length; i++)
                    {
                        if (i == 5 && interior.Lights[5].gameObject != null)
                            interior.Lights[5].gameObject.SetActive(Relic.MiningDroneQuest.dronesActive);
                        if (i != 6 && i != 7)
                        {
                            if (interior.Lights[i].gameObject != null && interior.Lights[i].transform != null)
                            {
                                if (interior.Lights[i].transform.parent != null && interior.Lights[i].transform.parent.name.Contains("Carrier_Exterior_Light") && interior.Lights[i].name == "Point light")
                                {
                                    interior.Lights[i].gameObject.SetActive(Relic.MiningDroneQuest.dronesActive);
                                }
                            }
                        }
                    }
                    if (Relic.MiningDroneQuest.dronesActive && interior.AmbienceSFX == "")
                    {
                        interior.AmbienceSFX = "sx_planet_terra_station_hum";
                        if (PLNetworkManager.Instance.LocalPlayer.CurrentInterior == interior)
                            PLMusic.PostEvent("play_" + interior.AmbienceSFX, interior.gameObject);

                    }
                    else if (!Relic.MiningDroneQuest.dronesActive && interior.AmbienceSFX != "")
                    {
                        if (PLNetworkManager.Instance.LocalPlayer.CurrentInterior == interior)
                            PLMusic.PostEvent("stop_" + interior.AmbienceSFX, interior.gameObject);
                        interior.AmbienceSFX = "";
                    }
                }
            }

            private static Vector3 GetEmptyLocationForEscortDrone(PLPersistantEncounterInstance inPEI, PLPersistantShipInfo inPSI)
            {
                if (PLServer.Instance != null)
                {
                    if (PLGlobal.Instance.Galaxy != null)
                    {
                        Traverse traverse = Traverse.Create(inPEI);
                        Vector3 hubLoc = Vector3.zero;
                        foreach (PLSectorInfo info in PLGlobal.Instance.Galaxy.AllSectorInfos.Values)
                        {
                            if (info.VisualIndication == ESectorVisualIndication.LAVA2)
                            {
                                hubLoc = info.Position;
                                break;
                            }
                        }
                        if (hubLoc != Vector3.zero)
                        {
                            Vector3 entryDir = (hubLoc - PLServer.GetCurrentSector().Position).normalized;
                            for (int i = 0; i < 18; i++)
                            {
                                Vector3 playerPos = PLEncounterManager.Instance.PlayerShip.GetCurrentSensorPosition();
                                Vector3 pos = entryDir * (700f + (i * 100f) + UnityEngine.Random.Range(0f, 500f));
                                pos.z = pos.y;
                                pos.x += playerPos.x;
                                pos.y = playerPos.y;
                                pos.z += playerPos.z;
                                if (traverse.Method("Check_IsPositionSafeForShipCheck", new Type[1] { typeof(Vector3) }).GetValue<bool>(new object[1] { pos }))
                                    return pos;
                            }
                        }
                    }
                }
                return Vector3.zero;
            }

            internal class MiningHubScreen
            {
                private static UILabel LabelOff;
                private static UILabel LabelOn;
                private static UITexture SwitchBox;

                [HarmonyPatch(typeof(PLGeothermalPlanetScreen), "SetupUI")]
                internal class SetupDroneSwitch
                {
                    private static void Postfix(PLGeothermalPlanetScreen __instance)
                    {
                        Traverse traverse = Traverse.Create(__instance);
                        object[] params1;
                        params1 = new object[6]
                        {
                        PLGlobal.Instance.WhitePixel,
                        new Vector3(37.5f, -60f),
                        new Vector2(80f, 35f),
                        new Color(0.65f, 0.65f, 0.65f),
                        __instance.MyRootPanel.transform,
                        UIWidget.Pivot.Center
                        };
                        SwitchBox = traverse.Method("CreateTexture", new Type[6] { typeof(Texture2D), typeof(Vector3), typeof(Vector2), typeof(Color), typeof(Transform), typeof(UIWidget.Pivot) }).GetValue<UITexture>(params1);
                        params1 = new object[6]
                        {
                            "ON",
                            new Vector3(37.5f, -60f),
                            16,
                            new Color(0.65f, 0.65f, 0.65f),
                            __instance.MyRootPanel.transform,
                            UIWidget.Pivot.Center
                        };
                        LabelOn = traverse.Method("CreateLabel", new Type[6] { typeof(string), typeof(Vector3), typeof(int), typeof(Color), typeof(Transform), typeof(UIWidget.Pivot) }).GetValue<UILabel>(params1);
                        params1 = new object[6]
                        {
                            "OFF",
                            new Vector3(-37.5f, -60f),
                            16,
                            new Color(0.65f, 0.65f, 0.65f),
                            __instance.MyRootPanel.transform,
                            UIWidget.Pivot.Center
                        };
                        LabelOff = traverse.Method("CreateLabel", new Type[6] { typeof(string), typeof(Vector3), typeof(int), typeof(Color), typeof(Transform), typeof(UIWidget.Pivot) }).GetValue<UILabel>(params1);
                        if (__instance.gameObject != null)
                            __instance.gameObject.transform.localPosition = new Vector3(20f, -21f, -20f);
                    }
                }

                [HarmonyPatch(typeof(PLGeothermalPlanetScreen), "Update")]
                internal class UpdateScreen
                {
                    internal static int click = 0;
                    private static void Postfix(PLGeothermalPlanetScreen __instance, ref float ___Circle1Vel, ref float ___Circle2Vel, ref float ___Circle3Vel, ref UILabel[] ___PumpLabels)
                    {
                        if (!__instance.UIIsSetup() || SwitchBox == null || LabelOff == null || LabelOn == null)
                            return;
                        if (click == 1)
                        {
                            __instance.PlaySoundEventOnAllClonedScreens("play_ship_generic_internal_computer_ui_on");
                            click = 0;

                        }
                        else if (click == 2)
                        {
                            __instance.PlaySoundEventOnAllClonedScreens("play_ship_generic_internal_computer_ui_off");
                            click = 0;
                        }
                        if (Relic.MiningDroneQuest.dronesActive)
                        {
                            SwitchBox.transform.localPosition = Vector3.Lerp(SwitchBox.transform.localPosition, LabelOn.transform.localPosition, Time.deltaTime * 8f);
                            LabelOn.color = Color.Lerp(LabelOn.color, Color.black, Time.deltaTime * 4f);
                            LabelOff.color = Color.Lerp(LabelOff.color, new Color(0.22f, 0.22f, 0.22f, 0.95f), Time.deltaTime * 4f);
                            SwitchBox.color = Color.Lerp(SwitchBox.color, new Color(0.65f, 0.65f, 0.65f), Time.deltaTime * 4f);
                        }
                        else
                        {
                            SwitchBox.transform.localPosition = Vector3.Lerp(SwitchBox.transform.localPosition, LabelOff.transform.localPosition, Time.deltaTime * 8f);
                            LabelOn.color = Color.Lerp(LabelOn.color, new Color(0.22f, 0.22f, 0.22f, 0.95f), Time.deltaTime * 4f);
                            LabelOff.color = Color.Lerp(LabelOff.color, Color.black, Time.deltaTime * 4f);
                            SwitchBox.color = Color.Lerp(SwitchBox.color, new Color(0.22f, 0.22f, 0.22f, 0.95f), Time.deltaTime * 4f);
                            ___Circle1Vel = 0f;
                            ___Circle2Vel = 0f;
                            ___Circle3Vel = 0f;
                            for (int i = 0; i < ___PumpLabels.Length; i++)
                            {
                                ___PumpLabels[i].text = "P" + (i + 1).ToString() + ": " + 0.ToString();
                            }
                        }
                    }
                }

                [HarmonyPatch(typeof(PLUIScreen), "DoInput")]
                internal class ProcessInput
                {
                    private static void Postfix(PLUIScreen __instance, Vector2 inMouseLoc, bool inCapturingMouse, PLUIScreen targetScreen, ref UIWidget ___LastHoverButton)
                    {
                        if (!(__instance is PLGeothermalPlanetScreen))
                            return;
                        if (!__instance.UIIsSetup() || SwitchBox == null || LabelOff == null || LabelOn == null)
                            return;
                        Vector2 ui = __instance.screenPointToUI(new Vector3(inMouseLoc.x * 512f, inMouseLoc.y * 512f, 0f));
                        Rect uiRectOfWidget1 = __instance.GetUIRectOfWidget(LabelOn);
                        Rect uiRectOfWidget2 = __instance.GetUIRectOfWidget(LabelOff);
                        uiRectOfWidget1.xMin -= 30f;
                        uiRectOfWidget2.xMin -= 30f;
                        uiRectOfWidget1.yMin += 17.5f;
                        uiRectOfWidget2.yMin += 17.5f;
                        uiRectOfWidget1.width = 60f;
                        uiRectOfWidget1.height = 35f;
                        uiRectOfWidget2.width = 60f;
                        uiRectOfWidget2.height = 35f;
                        if (inCapturingMouse && PLUIScreen.PointWithinUIRect(ui, uiRectOfWidget1))
                        {
                            LabelOn.alpha = 1f;
                            LabelOn.color = Color.white;
                            if (___LastHoverButton != LabelOn)
                            {
                                ___LastHoverButton = LabelOn;
                                __instance.PlaySoundEventOnAllClonedScreens("play_ship_generic_internal_computer_ui_hover");
                            }
                            if (PLInput.Instance.GetButtonUp(PLInputBase.EInputActionName.click) && !Relic.MiningDroneQuest.dronesActive)
                            {
                                __instance.mouseUpFrame = false;
                                ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.MiningDroneToggle", PhotonTargets.All, new object[2] { true, 1 });
                            }
                        }
                        else
                        {
                            if (___LastHoverButton == LabelOn)
                                ___LastHoverButton = null;
                            LabelOn.alpha = 0.85f;
                        }
                        if (inCapturingMouse && PLUIScreen.PointWithinUIRect(ui, uiRectOfWidget2))
                        {
                            LabelOff.alpha = 1f;
                            LabelOff.color = Color.white;
                            if (___LastHoverButton != LabelOff)
                            {
                                ___LastHoverButton = LabelOff;
                                __instance.PlaySoundEventOnAllClonedScreens("play_ship_generic_internal_computer_ui_hover");
                            }
                            if (PLInput.Instance.GetButtonUp(PLInputBase.EInputActionName.click) && Relic.MiningDroneQuest.dronesActive)
                            {
                                __instance.mouseUpFrame = false;
                                ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.MiningDroneToggle", PhotonTargets.All, new object[2] { false, 2 });

                            }
                        }
                        else
                        {
                            if (___LastHoverButton == LabelOff)
                                ___LastHoverButton = null;
                            LabelOff.alpha = 0.85f;
                        }
                    }
                }

                private class MiningDroneToggle : ModMessage
                {
                    public override void HandleRPC(object[] arguments, PhotonMessageInfo sender)
                    {
                        if (PhotonNetwork.isMasterClient)
                            Relic.MiningDroneQuest.dronesActive = (bool)arguments[0];
                        Relic.MiningDroneQuest.MiningHubScreen.UpdateScreen.click = (int)arguments[1];
                    }
                }
            }
            private static List<ComponentOverrideData> GetComponentsFromDroneType(int type, PLRand rand)
            {
                List<ComponentOverrideData> droneParts = new List<ComponentOverrideData>();
                int num = 0;
                switch (type)
                {
                    case 0:
                        if (rand.Next(4) == 3)
                            num = 1;
                        else
                            num = 0;
                        droneParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_SHLD,
                            CompSubType = (int)EShieldGeneratorType.E_TETRAGONAL_SURFACE,
                            ReplaceExistingComp = true,
                            CompLevel = 3 + Mathf.RoundToInt(2 * (PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar - 1f)) + num,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_SHLD,
                            SlotNumberToReplace = 0
                        }
                        );
                        if (rand.Next(4) == 3)
                            num = 1;
                        else
                            num = 0;
                        droneParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_REACTOR,
                            CompSubType = (int)ReactorModManager.Instance.GetReactorIDFromName("Extraction Drone Reactor"),
                            ReplaceExistingComp = true,
                            CompLevel = 3 + Mathf.RoundToInt(2 * (PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar - 1f)) + num,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_REACTOR,
                            SlotNumberToReplace = 0
                        }
                        );
                        if (rand.Next(4) == 3)
                            num = 1;
                        else
                            num = 0;
                        droneParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_HULL,
                            CompSubType = (int)HullModManager.Instance.GetHullIDFromName("Extraction Drone Hull"),
                            ReplaceExistingComp = true,
                            CompLevel = 3 + Mathf.RoundToInt(2 * (PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar - 1f)) + num,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_HULL,
                            SlotNumberToReplace = 0
                        }
                        );
                        if (rand.Next(4) == 3)
                            num = 1;
                        else
                            num = 0;
                        droneParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_CPU,
                            CompSubType = (int)ECPUClass.E_CPUTYPE_CYBER_DEF,
                            ReplaceExistingComp = true,
                            CompLevel = 3 + Mathf.RoundToInt(2 * (PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar - 1f)) + num,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_CPU,
                            SlotNumberToReplace = 0
                        }
                        );
                        if (rand.Next(4) == 3)
                            num = 1;
                        else
                            num = 0;
                        droneParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_CPU,
                            CompSubType = (int)ECPUClass.E_CPUTYPE_CYBER_DEF,
                            ReplaceExistingComp = true,
                            CompLevel = 3 + Mathf.RoundToInt(2 * (PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar - 1f)) + num,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_CPU,
                            SlotNumberToReplace = 1
                        }
                        );
                        if (rand.Next(4) == 3)
                            num = 1;
                        else
                            num = 0;
                        droneParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_CPU,
                            CompSubType = (int)ECPUClass.E_CPUTYPE_JUMP_PROCESSOR,
                            ReplaceExistingComp = true,
                            CompLevel = 3 + Mathf.RoundToInt(2 * (PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar - 1f)) + num,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_CPU,
                            SlotNumberToReplace = 2
                        }
                        );
                        if (rand.Next(4) == 3)
                            num = 1;
                        else
                            num = 0;
                        droneParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_CPU,
                            CompSubType = (int)ECPUClass.E_CPUTYPE_TARGETING,
                            ReplaceExistingComp = true,
                            CompLevel = 3 + Mathf.RoundToInt(2 * (PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar - 1f)) + num,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_CPU,
                            SlotNumberToReplace = 3
                        }
                        );
                        if (rand.Next(4) == 3)
                            num = 1;
                        else
                            num = 0;
                        droneParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_CPU,
                            CompSubType = (int)ECPUClass.E_CPUTYPE_WARPCHARGE_BACKUP,
                            ReplaceExistingComp = true,
                            CompLevel = 3 + Mathf.RoundToInt(2 * (PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar - 1f)) + num,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_CPU,
                            SlotNumberToReplace = 4
                        }
                        );
                        if (rand.Next(4) == 3)
                            num = 1;
                        else
                            num = 0;
                        droneParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_CPU,
                            CompSubType = (int)ECPUClass.E_CPUTYPE_THRUST_BACKUP,
                            ReplaceExistingComp = true,
                            CompLevel = 3 + Mathf.RoundToInt(2 * (PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar - 1f)) + num,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_CPU,
                            SlotNumberToReplace = 5
                        }
                        );
                        if (rand.Next(4) == 3)
                            num = 1;
                        else
                            num = 0;
                        droneParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_TURRET,
                            CompSubType = (int)TurretModManager.Instance.GetTurretIDFromName("Mining Laser"),
                            ReplaceExistingComp = true,
                            CompLevel = 3 + Mathf.RoundToInt(2 * (PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar - 1f)) + num,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_TURRET,
                            SlotNumberToReplace = 0
                        }
                        );
                        if (rand.Next(4) == 3)
                            num = 1;
                        else
                            num = 0;
                        droneParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_TURRET,
                            CompSubType = (int)ETurretType.BURST,
                            ReplaceExistingComp = true,
                            CompLevel = 3 + Mathf.RoundToInt(2 * (PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar - 1f)) + num,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_TURRET,
                            SlotNumberToReplace = 1
                        }
                        );
                        if (rand.Next(4) == 3)
                            num = 1;
                        else
                            num = 0;
                        droneParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_WARP,
                            CompSubType = (int)EWarpDriveType.E_WARPDR_DARKDRIVE,
                            ReplaceExistingComp = true,
                            CompLevel = 3 + Mathf.RoundToInt(2 * (PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar - 1f)) + num,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_WARP,
                            SlotNumberToReplace = 0
                        }
                        );
                        if (rand.Next(4) == 3)
                            num = 1;
                        else
                            num = 0;
                        droneParts.Add
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
                        if (rand.Next(4) == 3)
                            num = 1;
                        else
                            num = 0;
                        droneParts.Add
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
                        droneParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_DISTRESS_SIGNAL,
                            CompSubType = (int)4,
                            ReplaceExistingComp = true,
                            CompLevel = 0,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_DISTRESS_SIGNAL,
                            SlotNumberToReplace = 0
                        }
                        );
                        break;
                    case 1:
                        if (rand.Next(4) == 3)
                            num = 1;
                        else
                            num = 0;
                        droneParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_SHLD,
                            CompSubType = (int)EShieldGeneratorType.E_TETRAGONAL_SURFACE,
                            ReplaceExistingComp = true,
                            CompLevel = 3 + Mathf.RoundToInt(2 * (PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar - 1f)) + num,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_SHLD,
                            SlotNumberToReplace = 0
                        }
                        );
                        if (rand.Next(4) == 3)
                            num = 1;
                        else
                            num = 0;
                        droneParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_REACTOR,
                            CompSubType = (int)ReactorModManager.Instance.GetReactorIDFromName("Extraction Drone Reactor"),
                            ReplaceExistingComp = true,
                            CompLevel = 3 + Mathf.RoundToInt(2 * (PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar - 1f)) + num,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_REACTOR,
                            SlotNumberToReplace = 0
                        }
                        );
                        if (rand.Next(4) == 3)
                            num = 1;
                        else
                            num = 0;
                        droneParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_HULL,
                            CompSubType = (int)HullModManager.Instance.GetHullIDFromName("Escort Drone Hull"),
                            ReplaceExistingComp = true,
                            CompLevel = 3 + Mathf.RoundToInt(2 * (PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar - 1f)) + num,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_HULL,
                            SlotNumberToReplace = 0
                        }
                        );
                        if (rand.Next(4) == 3)
                            num = 1;
                        else
                            num = 0;
                        droneParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_CPU,
                            CompSubType = (int)ECPUClass.E_CPUTYPE_CYBER_DEF,
                            ReplaceExistingComp = true,
                            CompLevel = 3 + Mathf.RoundToInt(2 * (PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar - 1f)) + num,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_CPU,
                            SlotNumberToReplace = 0
                        }
                        );
                        if (rand.Next(4) == 3)
                            num = 1;
                        else
                            num = 0;
                        droneParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_CPU,
                            CompSubType = (int)ECPUClass.E_CPUTYPE_CYBER_DEF,
                            ReplaceExistingComp = true,
                            CompLevel = 3 + Mathf.RoundToInt(2 * (PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar - 1f)) + num,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_CPU,
                            SlotNumberToReplace = 1
                        }
                        );
                        if (rand.Next(4) == 3)
                            num = 1;
                        else
                            num = 0;
                        droneParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_CPU,
                            CompSubType = (int)ECPUClass.E_CPUTYPE_JUMP_PROCESSOR,
                            ReplaceExistingComp = true,
                            CompLevel = 3 + Mathf.RoundToInt(2 * (PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar - 1f)) + num,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_CPU,
                            SlotNumberToReplace = 2
                        }
                        );
                        if (rand.Next(4) == 3)
                            num = 1;
                        else
                            num = 0;
                        droneParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_CPU,
                            CompSubType = (int)ECPUClass.E_CPUTYPE_TARGETING,
                            ReplaceExistingComp = true,
                            CompLevel = 3 + Mathf.RoundToInt(2 * (PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar - 1f)) + num,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_CPU,
                            SlotNumberToReplace = 3
                        }
                        );
                        if (rand.Next(4) == 3)
                            num = 1;
                        else
                            num = 0;
                        droneParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_CPU,
                            CompSubType = (int)ECPUClass.E_CPUTYPE_WARPCHARGE_BACKUP,
                            ReplaceExistingComp = true,
                            CompLevel = 3 + Mathf.RoundToInt(2 * (PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar - 1f)) + num,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_CPU,
                            SlotNumberToReplace = 4
                        }
                        );
                        if (rand.Next(4) == 3)
                            num = 1;
                        else
                            num = 0;
                        droneParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_CPU,
                            CompSubType = (int)ECPUClass.E_CPUTYPE_THRUST_BACKUP,
                            ReplaceExistingComp = true,
                            CompLevel = 3 + Mathf.RoundToInt(2 * (PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar - 1f)) + num,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_CPU,
                            SlotNumberToReplace = 5
                        }
                        );
                        if (rand.Next(4) == 3)
                            num = 1;
                        else
                            num = 0;
                        droneParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_CPU,
                            CompSubType = (int)ECPUClass.CYBERWARFARE_MODULE,
                            ReplaceExistingComp = true,
                            CompLevel = 3 + Mathf.RoundToInt(2 * (PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar - 1f)) + num,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_CPU,
                            SlotNumberToReplace = 6
                        }
                        );
                        num = rand.Next(5, 7);
                        switch (num)
                        {
                            case 5:
                                if (rand.Next(4) == 3)
                                    num = 1;
                                else
                                    num = 0;
                                droneParts.Add
                                (
                                new ComponentOverrideData()
                                {
                                    CompType = (int)ESlotType.E_COMP_TURRET,
                                    CompSubType = (int)ETurretType.SPREAD,
                                    ReplaceExistingComp = true,
                                    CompLevel = 3 + Mathf.RoundToInt(2 * (PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar - 1f)) + num,
                                    IsCargo = false,
                                    CompTypeToReplace = (int)ESlotType.E_COMP_TURRET,
                                    SlotNumberToReplace = 0
                                }
                                );
                                break;
                            case 6:
                                if (rand.Next(4) == 3)
                                    num = 1;
                                else
                                    num = 0;
                                droneParts.Add
                                (
                                new ComponentOverrideData()
                                {
                                    CompType = (int)ESlotType.E_COMP_TURRET,
                                    CompSubType = (int)ETurretType.FLAMELANCE,
                                    ReplaceExistingComp = true,
                                    CompLevel = 3 + Mathf.RoundToInt(2 * (PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar - 1f)) + num,
                                    IsCargo = false,
                                    CompTypeToReplace = (int)ESlotType.E_COMP_TURRET,
                                    SlotNumberToReplace = 0
                                }
                                );
                                break;
                            case 7:
                                if (rand.Next(4) == 3)
                                    num = 1;
                                else
                                    num = 0;
                                droneParts.Add
                                (
                                new ComponentOverrideData()
                                {
                                    CompType = (int)ESlotType.E_COMP_TURRET,
                                    CompSubType = (int)ETurretType.DEFENDER,
                                    ReplaceExistingComp = true,
                                    CompLevel = 3 + Mathf.RoundToInt(2 * (PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar - 1f)) + num,
                                    IsCargo = false,
                                    CompTypeToReplace = (int)ESlotType.E_COMP_TURRET,
                                    SlotNumberToReplace = 0
                                }
                                );
                                break;
                        }
                        if (rand.Next(4) == 3)
                            num = 1;
                        else
                            num = 0;
                        droneParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_TURRET,
                            CompSubType = (int)ETurretType.LASER,
                            ReplaceExistingComp = true,
                            CompLevel = 3 + Mathf.RoundToInt(2 * (PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar - 1f)) + num,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_TURRET,
                            SlotNumberToReplace = 1
                        }
                        );
                        if (rand.Next(4) == 3)
                            num = 1;
                        else
                            num = 0;
                        droneParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_MAINTURRET,
                            CompSubType = (int)0,
                            ReplaceExistingComp = true,
                            CompLevel = 3 + Mathf.RoundToInt(2 * (PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar - 1f)) + num,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_MAINTURRET,
                            SlotNumberToReplace = 0
                        }
                        );
                        droneParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_TRACKERMISSILE,
                            CompSubType = (int)MissileModManager.Instance.GetMissileIDFromName("Thermobaric Missile"),
                            ReplaceExistingComp = true,
                            CompLevel = num,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_TRACKERMISSILE,
                            SlotNumberToReplace = 0
                        }
                        );
                        droneParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_TRACKERMISSILE,
                            CompSubType = (int)ETrackerMissileType.ARMOR_PIERCE,
                            ReplaceExistingComp = true,
                            CompLevel = num,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_TRACKERMISSILE,
                            SlotNumberToReplace = 1
                        }
                        );
                        if (rand.Next(4) == 3)
                            num = 1;
                        else
                            num = 0;
                        droneParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_WARP,
                            CompSubType = (int)EWarpDriveType.E_WARPDR_DARKDRIVE,
                            ReplaceExistingComp = true,
                            CompLevel = 3 + Mathf.RoundToInt(2 * (PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar - 1f)) + num,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_WARP,
                            SlotNumberToReplace = 0
                        }
                        );
                        if (rand.Next(4) == 3)
                            num = 1;
                        else
                            num = 0;
                        droneParts.Add
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
                        if (rand.Next(4) == 3)
                            num = 1;
                        else
                            num = 0;
                        droneParts.Add
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
                        num = rand.Next(3);
                        droneParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_DISTRESS_SIGNAL,
                            CompSubType = (int)4,
                            ReplaceExistingComp = true,
                            CompLevel = 1 + num,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_DISTRESS_SIGNAL,
                            SlotNumberToReplace = 0
                        }
                        );
                        break;
                    case 2:
                        if (rand.Next(4) == 3)
                            num = 1;
                        else
                            num = 0;
                        droneParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_SHLD,
                            CompSubType = (int)EShieldGeneratorType.E_TETRAGONAL_SURFACE,
                            ReplaceExistingComp = true,
                            CompLevel = 3 + Mathf.RoundToInt(2 * (PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar - 1f)) + num,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_SHLD,
                            SlotNumberToReplace = 0
                        }
                        );
                        if (rand.Next(4) == 3)
                            num = 1;
                        else
                            num = 0;
                        droneParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_REACTOR,
                            CompSubType = (int)ReactorModManager.Instance.GetReactorIDFromName("Extraction Drone Reactor"),
                            ReplaceExistingComp = true,
                            CompLevel = 3 + Mathf.RoundToInt(2 * (PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar - 1f)) + num,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_REACTOR,
                            SlotNumberToReplace = 0
                        }
                        );
                        if (rand.Next(4) == 3)
                            num = 1;
                        else
                            num = 0;
                        droneParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_HULL,
                            CompSubType = (int)HullModManager.Instance.GetHullIDFromName("Guardian Drone Hull"),
                            ReplaceExistingComp = true,
                            CompLevel = 3 + Mathf.RoundToInt(2 * (PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar - 1f)) + num,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_HULL,
                            SlotNumberToReplace = 0
                        }
                        );
                        if (rand.Next(4) == 3)
                            num = 1;
                        else
                            num = 0;
                        droneParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_CPU,
                            CompSubType = (int)ECPUClass.E_CPUTYPE_CYBER_DEF,
                            ReplaceExistingComp = true,
                            CompLevel = 3 + Mathf.RoundToInt(2 * (PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar - 1f)) + num,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_CPU,
                            SlotNumberToReplace = 0
                        }
                        );
                        if (rand.Next(4) == 3)
                            num = 1;
                        else
                            num = 0;
                        droneParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_CPU,
                            CompSubType = (int)ECPUClass.E_CPUTYPE_CYBER_DEF,
                            ReplaceExistingComp = true,
                            CompLevel = 3 + Mathf.RoundToInt(2 * (PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar - 1f)) + num,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_CPU,
                            SlotNumberToReplace = 1
                        }
                        );
                        if (rand.Next(4) == 3)
                            num = 1;
                        else
                            num = 0;
                        droneParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_CPU,
                            CompSubType = (int)ECPUClass.E_CPUTYPE_JUMP_PROCESSOR,
                            ReplaceExistingComp = true,
                            CompLevel = 3 + Mathf.RoundToInt(2 * (PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar - 1f)) + num,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_CPU,
                            SlotNumberToReplace = 2
                        }
                        );
                        if (rand.Next(4) == 3)
                            num = 1;
                        else
                            num = 0;
                        droneParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_CPU,
                            CompSubType = (int)ECPUClass.E_CPUTYPE_TARGETING,
                            ReplaceExistingComp = true,
                            CompLevel = 3 + Mathf.RoundToInt(2 * (PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar - 1f)) + num,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_CPU,
                            SlotNumberToReplace = 3
                        }
                        );
                        if (rand.Next(4) == 3)
                            num = 1;
                        else
                            num = 0;
                        droneParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_CPU,
                            CompSubType = (int)ECPUClass.E_CPUTYPE_TARGETING,
                            ReplaceExistingComp = true,
                            CompLevel = 3 + Mathf.RoundToInt(2 * (PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar - 1f)) + num,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_CPU,
                            SlotNumberToReplace = 4
                        }
                        );
                        if (rand.Next(4) == 3)
                            num = 1;
                        else
                            num = 0;
                        droneParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_CPU,
                            CompSubType = (int)ECPUClass.E_CPUTYPE_THRUST_BACKUP,
                            ReplaceExistingComp = true,
                            CompLevel = 3 + Mathf.RoundToInt(2 * (PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar - 1f)) + num,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_CPU,
                            SlotNumberToReplace = 5
                        }
                        );
                        if (rand.Next(4) == 3)
                            num = 1;
                        else
                            num = 0;
                        droneParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_CPU,
                            CompSubType = (int)ECPUClass.CYBERWARFARE_MODULE,
                            ReplaceExistingComp = true,
                            CompLevel = 3 + Mathf.RoundToInt(2 * (PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar - 1f)) + num,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_CPU,
                            SlotNumberToReplace = 6
                        }
                        );
                        if (rand.Next(4) == 3)
                            num = 1;
                        else
                            num = 0;
                        droneParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_TURRET,
                            CompSubType = (int)ETurretType.FLAMELANCE,
                            ReplaceExistingComp = true,
                            CompLevel = 3 + Mathf.RoundToInt(2 * (PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar - 1f)) + num,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_TURRET,
                            SlotNumberToReplace = 0
                        }
                        );
                        if (rand.Next(4) == 3)
                            num = 1;
                        else
                            num = 0;
                        droneParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_TURRET,
                            CompSubType = (int)ETurretType.FOCUS_LASER,
                            ReplaceExistingComp = true,
                            CompLevel = 3 + Mathf.RoundToInt(2 * (PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar - 1f)) + num,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_TURRET,
                            SlotNumberToReplace = 1
                        }
                        );
                        if (rand.Next(4) == 3)
                            num = 1;
                        else
                            num = 0;
                        droneParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_MAINTURRET,
                            CompSubType = (int)MegaTurretModManager.Instance.GetMegaTurretIDFromName("GuardianMainTurret"),
                            ReplaceExistingComp = true,
                            CompLevel = 3 + Mathf.RoundToInt(2 * (PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar - 1f)) + num,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_MAINTURRET,
                            SlotNumberToReplace = 0
                        }
                        );
                        droneParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_TRACKERMISSILE,
                            CompSubType = (int)MissileModManager.Instance.GetMissileIDFromName("Thermobaric Missile"),
                            ReplaceExistingComp = true,
                            CompLevel = num,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_TRACKERMISSILE,
                            SlotNumberToReplace = 0
                        }
                        );
                        droneParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_TRACKERMISSILE,
                            CompSubType = (int)ETrackerMissileType.STRAIGHTSHOT,
                            ReplaceExistingComp = true,
                            CompLevel = num,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_TRACKERMISSILE,
                            SlotNumberToReplace = 1
                        }
                        );
                        if (rand.Next(4) == 3)
                            num = 1;
                        else
                            num = 0;
                        droneParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_WARP,
                            CompSubType = (int)EWarpDriveType.E_WARPDR_DARKDRIVE,
                            ReplaceExistingComp = true,
                            CompLevel = 3 + Mathf.RoundToInt(2 * (PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar - 1f)) + num,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_WARP,
                            SlotNumberToReplace = 0
                        }
                        );
                        if (rand.Next(4) == 3)
                            num = 1;
                        else
                            num = 0;
                        droneParts.Add
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
                        if (rand.Next(4) == 3)
                            num = 1;
                        else
                            num = 0;
                        droneParts.Add
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
                        droneParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_DISTRESS_SIGNAL,
                            CompSubType = (int)4,
                            ReplaceExistingComp = true,
                            CompLevel = 4,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_DISTRESS_SIGNAL,
                            SlotNumberToReplace = 0
                        }
                        );
                        break;
                }
                return droneParts;
            }

            private static bool TeleporterActive(PLSectorInfo sectorInfo)
            {
                if (sectorInfo.VisualIndication == ESectorVisualIndication.LAVA2 && PLEncounterManager.Instance != null && PLEncounterManager.Instance.PlayerShip != null && !PLEncounterManager.Instance.PlayerShip.InWarp)
                {
                    bool flag = false;
                    foreach (PLShipInfoBase plShipInfoBase in UnityEngine.Object.FindObjectsOfType(typeof(PLShipInfoBase)))
                    {
                        if (plShipInfoBase.ShipTypeID == EShipType.E_WDDRONE2)
                        {
                            foreach (PLShipComponent component in plShipInfoBase.MyStats.GetComponentsOfType(ESlotType.E_COMP_DISTRESS_SIGNAL))
                            {
                                if (component is MiningDroneSignal)
                                {
                                    flag = true;
                                    break;
                                }
                            }
                        }
                    }
                    return !flag;
                }
                return true;
            }

            [HarmonyPatch(typeof(PLTeleportationLocationInstance), "ShouldBeUsable")]
            internal class DisableHubTeleporter
            {
                private static bool Prefix(PLTeleportationLocationInstance __instance, ref bool __result)
                {
                    if (PLEncounterManager.Instance != null && PLEncounterManager.Instance.PlayerShip != null && !PLEncounterManager.Instance.PlayerShip.InWarp)
                    {
                        if (__instance.MyShipInfo != null)
                            return true;
                        if (PLServer.Instance != null)
                        {
                            if (TeleporterActive(PLServer.GetCurrentSector()))
                                return true;
                            else
                            {
                                __result = false;
                                return false;
                            }
                        }
                    }
                    return true;
                }
            }

            [HarmonyPatch(typeof(PLShipInfoBase), "GetChaosBoost", new Type[2] { typeof(PLPersistantShipInfo), typeof(int) })]
            internal class MiningDroneScalingFix
            {
                private static Exception Finalizer(Exception __exception, PLShipInfoBase __instance, PLPersistantShipInfo inPersistantShipInfo, int offset, ref int __result)
                {
                    if (!((UnityEngine.Object)PLServer.Instance != (UnityEngine.Object)null) || inPersistantShipInfo == null)
                        return __exception;
                    if ((inPersistantShipInfo.Type == EShipType.E_WDDRONE1 || inPersistantShipInfo.Type == EShipType.E_WDDRONE2) && inPersistantShipInfo.FactionID == 6)
                    {
                        bool flag = false;
                        foreach (ComponentOverrideData data in inPersistantShipInfo.CompOverrides)
                        {
                            if (data.CompType == (int)ESlotType.E_COMP_DISTRESS_SIGNAL && data.CompSubType == 4)
                            {
                                flag = true;
                                break;
                            }
                        }
                        if (flag)
                            __result = 0;

                    }
                    return __exception;
                }
            }

            [HarmonyPatch(typeof(PLPersistantEncounterInstance), "PlayMusicBasedOnShipType")]
            private class MiningDroneMusic
            {
                private static bool Prefix(PLPersistantEncounterInstance __instance, EShipType inType, bool combat)
                {
                    if (PLServer.GetCurrentSector() == null || !(PLEncounterManager.Instance.PlayerShip != null) || PLEncounterManager.Instance.PlayerShip.InWarp || PLMusic.Instance.SpecialMusicPlaying || !combat)
                        return true;
                    bool flag = false;
                    foreach (PLShipInfoBase ship in __instance.MyCreatedShipInfos)
                    {
                        if (ship.IsDrone && !ship.HasBeenDestroyed && ship.AlertLevel == 2 && Relic.MiningDroneQuest.dronesActive)
                        {
                            foreach (PLShipComponent component in ship.MyStats.GetComponentsOfType(ESlotType.E_COMP_DISTRESS_SIGNAL))
                            {
                                if (component is MiningDroneSignal)
                                {
                                    if (ship.ShipTypeID == EShipType.E_WDDRONE1 || ship.ShipTypeID == EShipType.E_WDDRONE2)
                                    {
                                        flag = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    if (!flag)
                        return true;
                    PLMusic.Instance.PlayMusic("mx_infected_attack", true, false, false);
                    return false;
                }
            }

            [HarmonyPatch(typeof(PLScientistComputerScreen), "SetupGeneralInfoResult")]
            internal class HideDroneInfos
            {
                private static bool Prefix(ref string ___CurrentSearchString)
                {
                    int result = -1;
                    if (!int.TryParse(___CurrentSearchString, out result) || PLGlobal.Instance.AllGeneralInfos == null || result < 0 || PLGlobal.Instance.AllGeneralInfos.Count <= result)
                        return false;
                    if ((PLGlobal.Instance.AllGeneralInfos[result].Name == "Mining Drone" || PLGlobal.Instance.AllGeneralInfos[result].Name == "Escort Drone") && GXData < 1)
                        return false;
                    else if (PLGlobal.Instance.AllGeneralInfos[result].Name == "Guardian Drone" && GXData < 2)
                        return false;
                    return true;
                }
            }

            [HarmonyPatch(typeof(PLGlobal), "LoadGXFile")]
            internal class AddDroneGXEntries
            {
                private static void Postfix(PLGlobal __instance)
                {
                    __instance.AllGeneralInfos.Add(new GeneralInfo
                    {
                        Name = "Mining Drone",
                        Name_lower = "mining drone",
                        ID = 29,
                        Desc = "Drone used for extracting minerals from space asteroids.\n\nThey appear to be communicating with some uncharted hub sector within this galaxy. They are equipped with powerful laser technology.\n\nMining drones are not hostile unless provoked.",
                        MyStats = new List<GeneralInfoStat> { new GeneralInfoStat
                {
                    Left = "Type",
                    Right = "Unmanned Drone"
                },
                new GeneralInfoStat
                {
                    Left = "Faction",
                    Right = "Unknown"
                },
                new GeneralInfoStat
                {
                    Left = "Service",
                    Right = "Resource Extraction"
                }
                }
                    });
                    __instance.AllGeneralInfos.Add(new GeneralInfo
                    {
                        Name = "Escort Drone",
                        Name_lower = "escort drone",
                        ID = 30,
                        Desc = "Drone used to protect mining drone operations.\n\nThey appear to be communicating with some uncharted hub sector within this galaxy. They reinforce other mining and escort drones in distress.\n\nEscort drones are not hostile unless a drone is provoked.",
                        MyStats = new List<GeneralInfoStat> { new GeneralInfoStat
                {
                    Left = "Type",
                    Right = "Unmanned Drone"
                },
                new GeneralInfoStat
                {
                    Left = "Faction",
                    Right = "Unknown"
                },
                new GeneralInfoStat
                {
                    Left = "Service",
                    Right = "Asset Protection"
                }
                }
                    });
                    __instance.AllGeneralInfos.Add(new GeneralInfo
                    {
                        Name = "Guardian Drone",
                        Name_lower = "guardian drone",
                        ID = 31,
                        Desc = "Drone seen protecting the mining drone hub world in this galaxy.\n\nThey are outfitted with powerful weaponry designed to debilitate attacking ships. Each drone is equipped with a signal jammer that blocks teleportation to thier world's surface.\n\nGuardian drones are always hostile.",
                        MyStats = new List<GeneralInfoStat> { new GeneralInfoStat
                {
                    Left = "Type",
                    Right = "Unmanned Drone"
                },
                new GeneralInfoStat
                {
                    Left = "Faction",
                    Right = "Unknown"
                },
                new GeneralInfoStat
                {
                    Left = "Service",
                    Right = "Hub World Protector"
                }
                }
                    });
                }
            }
        }

        internal class RelicCaravan
        {
            internal static int CaravanCurrentSector = -1;
            internal static int CaravanTargetSector = -1;
            private static List<PLSectorInfo> CaravanPath = new List<PLSectorInfo>();
            private static int CaravanPathIndex = 0;
            internal static int CaravanSpecialsData = 0;
            internal static int CaravanUpdateTime = 0;
            internal static TraderPersistantDataEntry CaravanTraderData;

            internal static void ClearCaravanPath()
            {
                CaravanPath.Clear();
                CaravanPathIndex = 0;
            }

            public class Shop_Caravan : PLShop
            {
                public bool HasPDEBeenSetup;
                protected override void Start()
                {
                    base.Start();
                    this.Name = "Wandering Caravan";
                    this.Desc = "The best exotic ware seller around!";
                    this.ContrabandDealer = true;
                    if (!PhotonNetwork.isMasterClient)
                        this.MyPDE = new TraderPersistantDataEntry();
                }

                public override void CreateSpecials(TraderPersistantDataEntry inPDE)
                {
                }
            }

            [HarmonyPatch(typeof(PLRolandInfo), "SetupShipStats")]
            internal class AddShopComponent
            {
                private static void Postfix(PLRolandInfo __instance, bool previewStats, bool startingPlayerShip)
                {
                    if (__instance.PersistantShipInfo == null || !(__instance.PersistantShipInfo.ShipName == "Wandering Caravan" && __instance.PersistantShipInfo.Type == EShipType.E_ROLAND && __instance.PersistantShipInfo.SelectedActorID == "ExGal_RelicCaravan"))
                        return;
                    if (!startingPlayerShip && !previewStats && __instance.ShipRoot != null)
                    {
                        if (PhotonNetwork.isMasterClient)
                            ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.RecieveShopComponent", PhotonTargets.Others, new object[1] { __instance.ShipID });
                        Shop_Caravan shop = __instance.ShipRoot.AddComponent<Shop_Caravan>();
                        shop.OptionalShip = __instance;
                        shop.MySensorObject = __instance.MySensorObjectShip;
                        __instance.photonView.ObservedComponents.Add((Component)shop);
                        Traverse traverse = Traverse.Create(__instance);
                    }
                }
            }

            internal class RecieveShopComponent : ModMessage
            {
                public override void HandleRPC(object[] arguments, PhotonMessageInfo sender)
                {
                    LateAddShopComponent((int)arguments[0]);
                }
            }

            internal static async void LateAddShopComponent(int inShipID)
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
                if (shipInfoBase.ShipRoot.TryGetComponent(typeof(Shop_Caravan), out Component component))
                    return;
                Shop_Caravan shop = shipInfoBase.ShipRoot.AddComponent<Shop_Caravan>();
                shop.OptionalShip = shipInfoBase;
                shop.MySensorObject = shipInfoBase.MySensorObjectShip;
                shipInfoBase.photonView.ObservedComponents.Add(shop);
            }


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
                            CaravanTraderData = traderPersistantDataEntry;
                            RelicCaravan.CaravanCurrentSector = info.ID;
                            caravanInfo.CompOverrides.AddRange(CaravanComponents(__instance.Seed));
                            PLServer.Instance.AllPSIs.Add(caravanInfo);
                            CaravanUpdateTime = 60000;
                        }
                    }
                }
            }

            [HarmonyPatch(typeof(PLShipInfo), "CreateDefaultItemsForEnemyBotPlayer")]
            internal class CaravanCrewSpawn
            {
                private static void Postfix(PLPlayer inPlayer)
                {
                    if (!((UnityEngine.Object)inPlayer != (UnityEngine.Object)null) || !((UnityEngine.Object)inPlayer.StartingShip != (UnityEngine.Object)null))
                        return;
                    if (inPlayer.StartingShip.PersistantShipInfo == null || !(inPlayer.StartingShip.PersistantShipInfo.ShipName == "Wandering Caravan" && inPlayer.StartingShip.PersistantShipInfo.Type == EShipType.E_ROLAND && inPlayer.StartingShip.PersistantShipInfo.SelectedActorID == "ExGal_RelicCaravan"))
                        return;
                    inPlayer.Talents[(int)ETalents.HEALTH_BOOST] = (ObscuredInt)5;
                    inPlayer.Talents[(int)ETalents.PISTOL_DMG_BOOST] = (ObscuredInt)3;
                    inPlayer.Talents[(int)ETalents.QUICK_RESPAWN] = (ObscuredInt)4;
                    inPlayer.Talents[(int)ETalents.ARMOR_BOOST] = (ObscuredInt)5;
                    inPlayer.Talents[(int)ETalents.WPN_AMMO_BOOST] = (ObscuredInt)50;
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
                    inPlayer.gameObject.name += " ExGal";
                }
            }

            [HarmonyPatch(typeof(PLCampaignIO), "GetActorTypeData")]
            internal class CreateCaravanActorData
            {
                private static void Postfix(PLCampaignIO __instance, string inActorName, ref ActorTypeData __result)
                {
                    if (inActorName == "ExGal_RelicCaravan")
                    {
                        ActorTypeData data = new ActorTypeData();
                        data.Name = inActorName;

                        LineData lineDataOpener = new LineData();
                        lineDataOpener.TextOptions.Add("Greetings [PLAYERSHIP_NAME], come all this way to look at our stock? I guarentee you can't find quality like these anywhere else in the galaxy!");
                        lineDataOpener.Actions.Add(new LineActionData() { Type = "1" });
                        lineDataOpener.Actions.Add(new LineActionData() { Type = "0" });

                        LineData lineDataShop = new LineData();
                        lineDataShop.TextOptions.Add("BROWSE EXOTIC GOODS");
                        lineDataShop.IsPlayerLine = true;
                        lineDataShop.Actions.Add(new LineActionData() { Type = "1" });
                        lineDataShop.Actions.Add(new LineActionData { Type = "0" });
                        lineDataShop.Actions.Add(new LineActionData() { Type = "6" });

                        LineData lineDataShopText = new LineData();
                        lineDataShopText.TextOptions.Add("Take all the time you need.");
                        lineDataShopText.Actions.Add(new LineActionData() { Type = "1" });
                        lineDataShopText.Actions.Add(new LineActionData { Type = "0" });

                        LineData lineDataShop2 = new LineData();
                        lineDataShop2.TextOptions.Add("BROWSE EXOTIC GOODS");
                        lineDataShop2.IsPlayerLine = true;
                        lineDataShop2.Actions.Add(new LineActionData() { Type = "1" });
                        lineDataShop2.Actions.Add(new LineActionData() { Type = "6" });

                        LineData lineDataShopClose = new LineData();
                        lineDataShopClose.TextOptions.Add("CLOSE TRANSMISSION");
                        lineDataShopClose.IsPlayerLine = true;
                        lineDataShopClose.Actions.Add(new LineActionData() { Type = "1" });
                        lineDataShopClose.Actions.Add(new LineActionData() { Type = "10" });

                        lineDataShopText.ChildLines.Add(lineDataShop2);
                        lineDataShopText.ChildLines.Add(lineDataShopClose);
                        lineDataShop.ChildLines.Add(lineDataShopText);
                        lineDataOpener.ChildLines.Add(lineDataShop);

                        LineData lineDataDestination = new LineData();
                        lineDataDestination.TextOptions.Add("DESTINATION");
                        lineDataDestination.IsPlayerLine = true;
                        lineDataDestination.Actions.Add(new LineActionData() { Type = "1" });
                        lineDataDestination.Actions.Add(new LineActionData() { Type = "0" });

                        LineData lineDataDestinationText = new LineData();
                        lineDataDestinationText.TextOptions.Add(GetTextForDestinationSector());
                        lineDataDestinationText.Actions.Add(new LineActionData() { Type = "1" });
                        lineDataDestinationText.Actions.Add(new LineActionData() { Type = "0" });

                        lineDataDestination.ChildLines.Add(lineDataDestinationText);
                        lineDataDestinationText.ChildLines.Add(lineDataShopClose);
                        lineDataOpener.ChildLines.Add(lineDataDestination);

                        LineData lineDataMissionOption = new LineData();
                        lineDataMissionOption.TextOptions.Add("\"SPECIAL\" OFFER");
                        lineDataMissionOption.IsPlayerLine = true;
                        lineDataMissionOption.Actions.Add(new LineActionData() { Type = "1" });
                        lineDataMissionOption.Actions.Add(new LineActionData { Type = "0" });
                        lineDataMissionOption.Requirements.Add(new LineRequirementData { Type = "12", Parameter = "8000000" });

                        LineData lineDataMissionDesc = new LineData();
                        lineDataMissionDesc.TextOptions.Add("I'm looking for something. On the outside it looks like a worthless hunk of garbage, but it's actully considered a high deity to some denizens of the galaxy. If you can find it and bring it to me, I will make it worth your while.");
                        lineDataMissionDesc.Actions.Add(new LineActionData() { Type = "1" });
                        lineDataMissionDesc.Actions.Add(new LineActionData { Type = "0" });

                        LineData lineDataAcceptMission = new LineData();
                        lineDataAcceptMission.TextOptions.Add("ACCEPT");
                        lineDataAcceptMission.IsPlayerLine = true;
                        lineDataAcceptMission.Actions.Add(new LineActionData() { Type = "1" });
                        lineDataAcceptMission.Actions.Add(new LineActionData() { Type = "0" });

                        LineData lineDataAcceptMissionDesc = new LineData();
                        lineDataAcceptMissionDesc.TextOptions.Add("Excellent! When you find it bring it to me and I'll take a look at it.");
                        lineDataAcceptMissionDesc.Actions.Add(new LineActionData() { Type = "1" });
                        lineDataAcceptMissionDesc.Actions.Add(new LineActionData() { Type = "0" });
                        lineDataAcceptMissionDesc.Actions.Add(new LineActionData() { Type = "3", Parameter = "8000000" });

                        LineData lineDataDeclineMission = new LineData();
                        lineDataDeclineMission.TextOptions.Add("DECLINE");
                        lineDataDeclineMission.IsPlayerLine = true;
                        lineDataDeclineMission.Actions.Add(new LineActionData() { Type = "1" });
                        lineDataDeclineMission.Actions.Add(new LineActionData { Type = "10" });


                        lineDataAcceptMissionDesc.ChildLines.Add(lineDataShopClose);
                        lineDataAcceptMission.ChildLines.Add(lineDataAcceptMissionDesc);
                        lineDataMissionDesc.ChildLines.Add(lineDataAcceptMission);
                        lineDataMissionDesc.ChildLines.Add(lineDataDeclineMission);
                        lineDataMissionOption.ChildLines.Add(lineDataMissionDesc);
                        lineDataOpener.ChildLines.Add(lineDataMissionOption);

                        LineData lineDataMissionOption2 = new LineData();
                        lineDataMissionOption2.TextOptions.Add("GIVE JUNK CUBE");
                        lineDataMissionOption2.IsPlayerLine = true;
                        lineDataMissionOption2.Actions.Add(new LineActionData() { Type = "1" });
                        lineDataMissionOption2.Actions.Add(new LineActionData() { Type = "0" });
                        lineDataMissionOption2.Requirements.Add(new LineRequirementData() { Type = "21", Parameter = "ExGal_GotJunkCube1" });
                        lineDataMissionOption2.Requirements.Add(new LineRequirementData { Type = "12", Parameter = "8000001" });

                        LineData lineDataAcceptMission2Desc = new LineData();
                        lineDataAcceptMission2Desc.TextOptions.Add("Incredible! I'm suprised you were able to find it in a galaxy so vast... assuming what you've brought me is authentic that is! I'll need some time to make sure what you brought me isn't actually junk. Come find me later and I'll give you your reward.");
                        lineDataAcceptMission2Desc.Actions.Add(new LineActionData() { Type = "1" });
                        lineDataAcceptMission2Desc.Actions.Add(new LineActionData() { Type = "0" });
                        lineDataAcceptMission2Desc.Actions.Add(new LineActionData() { Type = "5", Parameter = "ExGal_DeliverJunkCube1" });
                        lineDataAcceptMission2Desc.Actions.Add(new LineActionData() { Type = "8", Parameter = "8000000" });
                        lineDataAcceptMission2Desc.Actions.Add(new LineActionData() { Type = "3", Parameter = "8000001" });

                        lineDataAcceptMission2Desc.ChildLines.Add(lineDataShopClose);
                        lineDataMissionOption2.ChildLines.Add(lineDataAcceptMission2Desc);
                        lineDataOpener.ChildLines.Add(lineDataMissionOption2);

                        LineData lineDataMissionOption3 = new LineData();
                        lineDataMissionOption3.TextOptions.Add("JUNK CUBE");
                        lineDataMissionOption3.IsPlayerLine = true;
                        lineDataMissionOption3.Actions.Add(new LineActionData() { Type = "1" });
                        lineDataMissionOption3.Actions.Add(new LineActionData() { Type = "0" });
                        lineDataMissionOption3.Requirements.Add(new LineRequirementData() { Type = "21", Parameter = "ExGal_WaitJunkCube1" });
                        lineDataMissionOption3.Requirements.Add(new LineRequirementData { Type = "12", Parameter = "8000002" });

                        LineData lineDataAcceptMission3Desc = new LineData();
                        lineDataAcceptMission3Desc.TextOptions.Add("I was wondering when you'd return for that. Unfortunatly some bandits took off with it while we were stopped at Cornelia. They kept shouting \"Praise be the Cube,\" whatever that means. I've tracked them to this sector, bring it back to me so I can finish looking it over and get you your reward.");
                        lineDataAcceptMission3Desc.Actions.Add(new LineActionData() { Type = "1" });
                        lineDataAcceptMission3Desc.Actions.Add(new LineActionData() { Type = "0" });
                        lineDataAcceptMission3Desc.Actions.Add(new LineActionData() { Type = "5", Parameter = "ExGal_WaitJunkCubeReturn1" });
                        lineDataAcceptMission3Desc.Actions.Add(new LineActionData() { Type = "8", Parameter = "8000001" });
                        lineDataAcceptMission3Desc.Actions.Add(new LineActionData() { Type = "3", Parameter = "8000002" });

                        lineDataAcceptMission3Desc.ChildLines.Add(lineDataShopClose);
                        lineDataMissionOption3.ChildLines.Add(lineDataAcceptMission3Desc);
                        lineDataOpener.ChildLines.Add(lineDataMissionOption3);

                        LineData lineDataMissionOption4 = new LineData();
                        lineDataMissionOption4.TextOptions.Add("GIVE JUNK CUBE");
                        lineDataMissionOption4.IsPlayerLine = true;
                        lineDataMissionOption4.Actions.Add(new LineActionData() { Type = "1" });
                        lineDataMissionOption4.Actions.Add(new LineActionData() { Type = "0" });
                        lineDataMissionOption4.Requirements.Add(new LineRequirementData() { Type = "21", Parameter = "ExGal_GotJunkCube2" });
                        lineDataMissionOption4.Requirements.Add(new LineRequirementData { Type = "12", Parameter = "8000003" });

                        LineData lineDataAcceptMission4Desc = new LineData();
                        lineDataAcceptMission4Desc.TextOptions.Add("Nice work. I won't lose it this time I promise! Once I finish examining this come find me and I'll give you your reward.");
                        lineDataAcceptMission4Desc.Actions.Add(new LineActionData() { Type = "1" });
                        lineDataAcceptMission4Desc.Actions.Add(new LineActionData() { Type = "0" });
                        lineDataAcceptMission4Desc.Actions.Add(new LineActionData() { Type = "5", Parameter = "ExGal_DeliverJunkCube2" });
                        lineDataAcceptMission4Desc.Actions.Add(new LineActionData() { Type = "8", Parameter = "8000002" });
                        lineDataAcceptMission4Desc.Actions.Add(new LineActionData() { Type = "3", Parameter = "8000003" });

                        lineDataAcceptMission4Desc.ChildLines.Add(lineDataShopClose);
                        lineDataMissionOption4.ChildLines.Add(lineDataAcceptMission4Desc);
                        lineDataOpener.ChildLines.Add(lineDataMissionOption4);

                        LineData lineDataMissionReward = new LineData();
                        lineDataMissionReward.TextOptions.Add("REWARD");
                        lineDataMissionReward.IsPlayerLine = true;
                        lineDataMissionReward.Actions.Add(new LineActionData() { Type = "1" });
                        lineDataMissionReward.Actions.Add(new LineActionData() { Type = "0" });
                        lineDataMissionReward.Requirements.Add(new LineRequirementData() { Type = "21", Parameter = "ExGal_WaitJunkCube2" });
                        lineDataMissionReward.Requirements.Add(new LineRequirementData() { Type = "14", Parameter = "8000003" });

                        LineData lineDataMissionRewardDesc = new LineData();
                        lineDataMissionRewardDesc.TextOptions.Add("Ah! My favorite relic chasing crew returns! That junk cube is 100% geniune, and is now a prized part of my collection. Here's your reward after all of that, it should already be in your cargo hold.");
                        lineDataMissionRewardDesc.Actions.Add(new LineActionData() { Type = "1" });
                        lineDataMissionRewardDesc.Actions.Add(new LineActionData() { Type = "0" });
                        lineDataMissionRewardDesc.Actions.Add(new LineActionData() { Type = "5", Parameter = "ExGal_WaitJunkCubeReturn2" });

                        lineDataMissionRewardDesc.ChildLines.Add(lineDataShopClose);
                        lineDataMissionReward.ChildLines.Add(lineDataMissionRewardDesc);
                        lineDataOpener.ChildLines.Add(lineDataMissionReward);

                        data.OpeningLines.Add(lineDataOpener);
                        __result = data;
                    }
                    else if (inActorName == "RACENPC_06")
                    {
                        if (PLServer.Instance != null && PLGlobal.Instance.Galaxy != null)
                        {
                            PLSectorInfo info = PLGlobal.Instance.Galaxy.GetSectorOfVisualIndication(ESectorVisualIndication.WASTE2);
                            if (info == null)
                                return;
                            string sysName = info.Name.Split(' ')[0];
                            __result.OpeningLines[4].TextOptions[0] = "These noodles are good, but they aren't as good as the ones I had in the " + sysName + " system. I recommend visiting if you can find the time, though you might want to invest in a good exosuit...";
                        }
                    }
                }
            }

            private static string GetTextForDestinationSector()
            {
                string result = "We aren't headed to anywhere in particular at the moment. Feel free to browse our wares while we are anchored here!";
                if (CaravanTargetSector != -1)
                {
                    PLSectorInfo sector = PLServer.GetSectorWithID(CaravanTargetSector);
                    if (sector != null)
                    {
                        switch (sector.VisualIndication)
                        {
                            case ESectorVisualIndication.CORNELIA_HUB:
                                result = "Our current heading is Cornelia. I heard someone there is in possesion of a data fragment if you're willing to do his dirty work.";
                                break;
                            case ESectorVisualIndication.DESERT_HUB:
                                result = "We are currently headed to the Burrow. Watching crews scramble around in the arena never gets old! They also have quite the selection of handheld weaponry too.";
                                break;
                            case ESectorVisualIndication.AOG_HUB:
                                if (PLServer.Instance.CrewFactionID == 2)
                                    result = "We are headed to the Es... Harbor! It's a great place with a lot of great people! You should head there as soon as possible!";
                                else
                                    result = "We are headed to the Estate. It's got one of the best bars in the galaxy. Our pilot frequents the specials menu every time we anchor there.";
                                break;
                            case ESectorVisualIndication.GENTLEMEN_START:
                                if (PLServer.Instance.CrewFactionID == 1)
                                    result = "We are headed to the hideout. We have a client there who is a reputable borthix trader if you're looking get your hands on some.";
                                else
                                    result = "We are headed to... nowhere in particluar. Feel free to browse our wares while we are here.";
                                break;
                            case ESectorVisualIndication.THE_HARBOR:
                                if (PLServer.Instance.CrewFactionID == 1)
                                    result = "We are currently headed to the Harbor. It's a nice place but be careful, they aren't too fond of Gentlemen there";
                                else
                                    result = "We are currently headed to the Harbor. It may be remote, but it's a great place to resupply.";
                                break;
                        }
                    }
                }
                return result;
            }

            [HarmonyPatch(typeof(PLStarmap), "Update")]
            internal class CaravanIcon
            {
                public static Image CaravanLocImage;
                public static Image CaravanLocBG;
                private static void Postfix(PLStarmap __instance)
                {
                    if (CaravanLocImage == null)
                    {
                        CaravanLocImage = UnityEngine.Object.Instantiate(__instance.HunterLocImage, __instance.HunterLocImage.transform.parent);
                        CaravanLocImage.GetComponent<Image>().color = getRelicColor();
                        Image[] image = CaravanLocImage.GetComponentsInChildren<Image>();
                        image[1].color = new Color(0.1383f, 0f, 0.415f, 0.5f);
                        CaravanLocBG = image[1];
                        image[2].color = getRelicColor();
                        CaravanLocImage.GetComponentInChildren<Text>().text = "CARAVAN";
                        CaravanLocImage.GetComponentInChildren<Text>().color = getRelicColor();
                    }
                    if (__instance.IsActive && CaravanLocImage != null && CaravanLocBG != null && RelicCaravan.CaravanCurrentSector != -1)
                    {
                        if (PLServer.Instance != null)
                        {
                            PLSectorInfo sectorWithId = PLServer.GetSectorWithID(RelicCaravan.CaravanCurrentSector);
                            if (sectorWithId != null)
                            {
                                bool flag = sectorWithId.IsThisSectorWithinPlayerWarpRange();
                                CaravanLocImage.gameObject.SetActive(flag);
                                CaravanLocBG.gameObject.SetActive(flag);
                                if (CaravanLocImage.gameObject.activeSelf)
                                {
                                    CaravanLocImage.transform.localPosition = sectorWithId.Position * 2000f + new Vector3(0.0f, -15f, 0.0f);
                                    CaravanLocImage.transform.localPosition = new Vector3(CaravanLocImage.transform.localPosition.x, CaravanLocImage.transform.localPosition.y, 0.0f);
                                }
                            }

                        }
                        else
                            PLGlobal.SafeGameObjectSetActive(CaravanLocImage.gameObject, false);
                    }
                }
            }

            [HarmonyPatch(typeof(PLShipInfoBase), "ShouldBeHostileToShip")]
            internal class CaravanNoHostile
            {
                private static bool Prefix(PLShipInfoBase __instance, PLShipInfoBase inShip, bool enforceDialogueNotHostile, bool makeHostileIfTrue, bool enforceWarp, PLShipInfoBase ___LastValue_TakeDamage_attackingShip, ref bool __result)
                {
                    if (__instance.ShipTypeID == EShipType.E_ROLAND && __instance.SelectedActorID == "ExGal_RelicCaravan")
                    {
                        if (___LastValue_TakeDamage_attackingShip == inShip || __instance.MyStats.GetComponentsOfType(ESlotType.E_COMP_VIRUS).Count > 0)
                            return true;
                        else
                        {
                            __result = false;
                            return false;
                        }
                    }
                    return true;
                }
            }

            [HarmonyPatch(typeof(PLServer), "Update")]
            internal class UpdateCaravan
            {
                private static PLPersistantShipInfo persistantCaravanInfo = null;
                private static void Postfix()
                {
                    if (PLServer.Instance == null)
                        return;
                    if (!PhotonNetwork.isMasterClient)
                        return;
                    if (persistantCaravanInfo == null)
                    {
                        foreach (PLPersistantShipInfo pLPersistantShipInfo in PLServer.Instance.AllPSIs)
                        {
                            if (pLPersistantShipInfo.ShipName == "Wandering Caravan" && pLPersistantShipInfo.Type == EShipType.E_ROLAND && pLPersistantShipInfo.SelectedActorID == "ExGal_RelicCaravan")
                            {
                                persistantCaravanInfo = pLPersistantShipInfo;
                                break;
                            }
                        }
                    }
                    if (persistantCaravanInfo == null || persistantCaravanInfo.IsShipDestroyed)
                        return;
                    if (persistantCaravanInfo.OptionalTPDE != null)
                        CaravanTraderData = persistantCaravanInfo.OptionalTPDE;
                    if (persistantCaravanInfo.OptionalTPDE == null && CaravanTraderData != null)
                        persistantCaravanInfo.OptionalTPDE = CaravanTraderData;
                    if (persistantCaravanInfo.ShipInstance != null && persistantCaravanInfo.ShipInstance.ShipNameValue.Contains("Wandering Caravan"))
                        persistantCaravanInfo.ShipInstance.ShipNameValue = "Wandering Caravan";
                    if (CaravanCurrentSector != -1 && PLServer.GetCurrentSector() != null && CaravanCurrentSector == PLServer.GetCurrentSector().ID && PLEncounterManager.Instance.PlayerShip != null && !PLEncounterManager.Instance.PlayerShip.InWarp)
                    {

                        if (persistantCaravanInfo != null && !persistantCaravanInfo.IsShipDestroyed && persistantCaravanInfo.ShipInstance == null)
                        {
                            persistantCaravanInfo.MyCurrentSector = PLServer.GetCurrentSector();
                            persistantCaravanInfo.CreateShipInstance(PLEncounterManager.Instance.GetCPEI());
                        }
                    }
                    bool flag = false;
                    bool flag1 = false;
                    if (CaravanCurrentSector != -1 && PLServer.GetCurrentSector() != null && CaravanCurrentSector != PLServer.GetCurrentSector().ID)
                    {
                        if (PLServer.Instance.GetEstimatedServerMs() > 0 && PLServer.Instance.GetEstimatedServerMs() - CaravanUpdateTime > 0)
                        {
                            CaravanUpdateTime = PLServer.Instance.GetEstimatedServerMs() + 60000;
                            if (CaravanTargetSector != -1)
                            {
                                if (CaravanPath.Count > 1 && CaravanPath.Count > CaravanPathIndex + 1)
                                {
                                    if (CaravanPath[CaravanPathIndex + 1].MySPI.Faction == 4 || CaravanPath[CaravanPathIndex + 1].DistressSignalActive)
                                    {
                                        flag = true;
                                        flag1 = true;
                                    }
                                    else
                                    {
                                        CaravanCurrentSector = CaravanPath[CaravanPathIndex + 1].ID;
                                        persistantCaravanInfo.MyCurrentSector = CaravanPath[CaravanPathIndex + 1];
                                        persistantCaravanInfo.ShldPercent = 1f;
                                        CaravanPathIndex++;
                                    }
                                    if (CaravanCurrentSector == CaravanTargetSector)
                                    {
                                        persistantCaravanInfo.HullPercent = 1f;
                                        if (persistantCaravanInfo.OptionalTPDE != null)
                                        {
                                            foreach (int wareID in persistantCaravanInfo.OptionalTPDE.Wares.Keys)
                                                if (persistantCaravanInfo.OptionalTPDE.Wares[wareID] == null)
                                                    persistantCaravanInfo.OptionalTPDE.Wares.Remove(wareID);
                                            while (persistantCaravanInfo.OptionalTPDE.Wares.Count < 30)
                                                persistantCaravanInfo.OptionalTPDE.ServerAddWare(PLShipComponent.CreateRandom());
                                            while (persistantCaravanInfo.OptionalTPDE.Wares.Count > 30)
                                                persistantCaravanInfo.OptionalTPDE.Wares.Remove(UnityEngine.Random.Range(0, persistantCaravanInfo.OptionalTPDE.Wares.Count));
                                            List<int> wareKeys = new List<int>();
                                            foreach (int wareID in persistantCaravanInfo.OptionalTPDE.Wares.Keys)
                                            {
                                                if (UnityEngine.Random.Range(0f, 1000f) > 750f)
                                                    wareKeys.Add(wareID);
                                            }
                                            int num = 0;
                                            while (wareKeys.Count > 0)
                                            {
                                                int currentKey = wareKeys[UnityEngine.Random.Range(0, wareKeys.Count)];
                                                if (num == 0)
                                                {
                                                    persistantCaravanInfo.OptionalTPDE.Wares.Remove(currentKey);
                                                    persistantCaravanInfo.OptionalTPDE.ServerAddWare(GetSpecialOffer());
                                                    ++num;
                                                    wareKeys.Remove(currentKey);
                                                }
                                                else if (num == 1)
                                                {
                                                    if (UnityEngine.Random.Range(0f, 1000f) > 500f)
                                                    {
                                                        persistantCaravanInfo.OptionalTPDE.Wares.Remove(currentKey);
                                                        persistantCaravanInfo.OptionalTPDE.ServerAddWare(GetSpecialOffer());
                                                        ++num;
                                                    }
                                                    else
                                                    {
                                                        persistantCaravanInfo.OptionalTPDE.Wares.Remove(currentKey);
                                                        persistantCaravanInfo.OptionalTPDE.ServerAddWare(PLShipComponent.CreateRandom());
                                                    }
                                                    wareKeys.Remove(currentKey);
                                                }
                                                else
                                                {
                                                    persistantCaravanInfo.OptionalTPDE.Wares.Remove(currentKey);
                                                    persistantCaravanInfo.OptionalTPDE.ServerAddWare(PLShipComponent.CreateRandom());
                                                    wareKeys.Remove(currentKey);
                                                }
                                            }
                                            if (PLServer.DoesPEIOfTypeExist_AndBeenVisited(ESectorVisualIndication.ANCIENT_SENTRY) && CaravanSpecialsData % 10 != 1)
                                            {
                                                PLPersistantShipInfo psiWithShipType1 = PLServer.GetPSIWithShipType(EShipType.E_CORRUPTED_DRONE);
                                                if (psiWithShipType1 == null || psiWithShipType1.IsShipDestroyed)
                                                {
                                                    persistantCaravanInfo.OptionalTPDE.ServerAddWare(new Turrets.AutoTurrets.AncientAutoLaser());
                                                    persistantCaravanInfo.OptionalTPDE.ServerAddWare(new Turrets.AutoTurrets.AncientAutoLaser());
                                                    CaravanSpecialsData = (CaravanSpecialsData / 10) + 1;
                                                }
                                            }

                                        }
                                        flag = true;
                                        flag1 = true;
                                        CaravanUpdateTime += 60000;
                                    }
                                }
                                else
                                {
                                    flag = true;
                                }
                            }
                            else
                            {
                                flag = true;
                                flag1 = true;
                            }
                            if (flag)
                            {
                                int targetID = CaravanTargetSector;
                                if (flag1)
                                {
                                    List<ESectorVisualIndication> potentialTargets = new List<ESectorVisualIndication>()
                            {
                                ESectorVisualIndication.CORNELIA_HUB,
                                ESectorVisualIndication.DESERT_HUB,
                                ESectorVisualIndication.AOG_HUB,
                                ESectorVisualIndication.GENTLEMEN_START,
                                ESectorVisualIndication.THE_HARBOR
                            };
                                    if (CaravanTargetSector != -1)
                                        potentialTargets.Remove(PLServer.GetSectorWithID(CaravanTargetSector).VisualIndication);
                                    else
                                        potentialTargets.Remove(ESectorVisualIndication.AOG_HUB);
                                    if (potentialTargets.Contains(PLServer.GetSectorWithID(CaravanCurrentSector).VisualIndication))
                                        potentialTargets.Remove(PLServer.GetSectorWithID(CaravanCurrentSector).VisualIndication);
                                    ESectorVisualIndication target = potentialTargets[UnityEngine.Random.Range(0, potentialTargets.Count)];
                                    if (PLGlobal.Instance.Galaxy.GetSectorOfVisualIndication(target) != null)
                                        targetID = PLGlobal.Instance.Galaxy.GetSectorOfVisualIndication(target).ID;
                                    else
                                        targetID = -1;
                                }
                                CaravanTargetSector = targetID;
                                CaravanPath.Clear();
                                if (CaravanTargetSector != -1)
                                    CaravanPath = GetPathToSector_NPC(PLServer.GetSectorWithID(CaravanCurrentSector), PLServer.GetSectorWithID(CaravanTargetSector), 0.12f);
                                CaravanPathIndex = 0;
                            }
                        }
                    }
                }
            }

            private static PLShipComponent GetSpecialOffer()
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
                        component = PLMegaTurret.CreateMainTurretFromHash(3, level, 1);
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
                float customWarpRange)
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
                Traverse traverse = Traverse.Create(PLGlobal.Instance.Galaxy);
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
                        if (!(neighbor.MySPI.Faction == 4 || neighbor.MySPI.Faction == 6 || neighbor.MissionSpecificID != -1 || neighbor.VisualIndication == ESectorVisualIndication.COLONIAL_HUB || neighbor.VisualIndication == ESectorVisualIndication.WD_START || neighbor.VisualIndication == ESectorVisualIndication.GWG || neighbor.VisualIndication == ESectorVisualIndication.CYPHER_LAB || neighbor.VisualIndication == ESectorVisualIndication.GREY_HUNTSMAN_HQ || neighbor.VisualIndication == ESectorVisualIndication.HIGHROLLERS_STATION || neighbor.VisualIndication == ESectorVisualIndication.BLACKHOLE || neighbor.VisualIndication == ESectorVisualIndication.MINE_FIELD || neighbor.VisualIndication == ESectorVisualIndication.INTREPID_SECTOR_CMDR || neighbor.VisualIndication == ESectorVisualIndication.ANCIENT_SENTRY || neighbor.VisualIndication == ESectorVisualIndication.ALCHEMIST || neighbor.VisualIndication == ESectorVisualIndication.DEATHSEEKER_COMMANDER || neighbor.VisualIndication == ESectorVisualIndication.SWARM_CMDR || neighbor.VisualIndication == ESectorVisualIndication.SWARM_KEEPER) && neighbor.Category != NodeCategory.NODE_CLOSED && PLStarmap.ShouldShowSectorBG(neighbor) && (double)(new Vector2(plSectorInfo3.Position.x, plSectorInfo3.Position.y) - new Vector2(neighbor.Position.x, neighbor.Position.y)).sqrMagnitude <= (double)customWarpRange * (double)customWarpRange)
                        {
                            float num3 = traverse.Method("Heuristic", new Type[2]
                            {
                                typeof(PLSectorInfo),
                                typeof(PLSectorInfo)
                            }).GetValue<float>(new object[2]
                            {
                                neighbor,
                                plSectorInfo3
                            });
                            if (neighbor.Category == NodeCategory.NODE_DEF)
                            {
                                if (!OpenList.Contains(neighbor))
                                    OpenList.Add(neighbor);
                                neighbor.Category = NodeCategory.NODE_OPEN;
                                neighbor.SearchParentNode = plSectorInfo3;
                                float num4 = traverse.Method("Heuristic", new Type[2]
                            {
                                typeof(PLSectorInfo),
                                typeof(PLSectorInfo)
                            }).GetValue<float>(new object[2]
                            {
                                neighbor,
                                inEndSector
                            });
                                float HWeight = traverse.Field("HWeight").GetValue<float>();
                                neighbor.GCost = plSectorInfo3.GCost + num4 * HWeight;
                                neighbor.HCost = num4 * HWeight;
                                neighbor.FCost = neighbor.GCost + neighbor.HCost * HWeight;
                                SortOpenList(ref OpenList);
                            }
                            else if ((double)neighbor.GCost > (double)plSectorInfo3.GCost + (double)num3)
                            {
                                float HWeight = traverse.Field("HWeight").GetValue<float>();
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

            [HarmonyPatch(typeof(PLShipInfoBase), "Ship_WarpOutNow")]
            private class HandleCaravanEmergencyWarp
            {
                private static bool Prefix(PLShipInfoBase __instance)
                {
                    if (__instance.PersistantShipInfo == null || !(__instance.PersistantShipInfo.ShipName == "Wandering Caravan" && __instance.PersistantShipInfo.Type == EShipType.E_ROLAND && __instance.PersistantShipInfo.SelectedActorID == "ExGal_RelicCaravan"))
                        return true;
                    PLSectorInfo currentSector = PLServer.GetCurrentSector();
                    if (currentSector.Neighbors.Count > 0)
                    {
                        PLServer.Instance.photonView.RPC("WarpOutEffect", PhotonTargets.All, (object)__instance.Exterior.transform.position, (object)__instance.Exterior.transform.rotation);
                        CaravanCurrentSector = currentSector.Neighbors[UnityEngine.Random.Range(0, currentSector.Neighbors.Count)].ID;
                        CaravanPath.Clear();
                        CaravanPathIndex = 0;
                        CaravanUpdateTime += 60000;
                    }
                    else
                    {
                        if (CaravanPath.Count > CaravanPathIndex + 1)
                        {
                            PLServer.Instance.photonView.RPC("WarpOutEffect", PhotonTargets.All, (object)__instance.Exterior.transform.position, (object)__instance.Exterior.transform.rotation);
                            CaravanCurrentSector = CaravanPath[CaravanPathIndex + 1].ID;
                            CaravanPathIndex++;
                            CaravanUpdateTime += 60000;
                        }
                        else
                            return false;
                    }
                    PhotonNetwork.Destroy(__instance.ShipRoot);
                    return false;
                }
            }

            [HarmonyPatch(typeof(PLPersistantEncounterInstance), "OnEndWarp")]
            internal class CanyonPlanetSetupCall
            {
                private static void Postfix(PLPersistantEncounterInstance __instance)
                {
                    if (__instance is PLCanyonPlanetEncounter)
                    {
                        if (PLServer.GetCurrentSector() != null && !PLServer.GetCurrentSector().Visited && PLServer.GetCurrentSector().MissionSpecificID == 8000002 && PhotonNetwork.isMasterClient)
                        {
                            int[] hashes = new int[1]
                            {
                            (int)PLMissionShipComponent.CreateMissionComponentFromHash(8, 0, 0).getHash()
                            };
                            Relic.AddCompToPlanet(__instance, hashes, true, true);
                        }
                    }
                }
            }

            internal static List<ComponentOverrideData> CaravanComponents(int seed)
            {
                List<ComponentOverrideData> caravanParts = new List<ComponentOverrideData>();
                int num = 0;
                PLRand rand = new PLRand(seed);
                num = rand.Next() % 2;
                caravanParts.Add
                (
                new ComponentOverrideData()
                {
                    CompType = (int)ESlotType.E_COMP_SHLD,
                    CompSubType = (int)EShieldGeneratorType.E_GRIMCUTLASS_SHIELDS,
                    ReplaceExistingComp = true,
                    CompLevel = 6 + num,
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
                    CompSubType = (int)EReactorType.ROLAND_REACTOR,
                    ReplaceExistingComp = true,
                    CompLevel = 6 + num,
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
                    CompSubType = (int)EHullType.E_LAYERED_HULL,
                    ReplaceExistingComp = true,
                    CompLevel = 6 + num,
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
                    CompLevel = 6 + num,
                    IsCargo = false,
                    CompTypeToReplace = (int)ESlotType.E_COMP_HULLPLATING,
                    SlotNumberToReplace = 0
                }
                );
                caravanParts.Add
                (
                new ComponentOverrideData()
                {
                    CompType = (int)ESlotType.E_COMP_CAPTAINS_CHAIR,
                    CompSubType = (int)ECaptainsChairType.E_COLONIAL_MODERN,
                    ReplaceExistingComp = true,
                    CompLevel = 0,
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
                    CompLevel = 6 + num,
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
                    CompLevel = 6 + num,
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
                    CompLevel = 6 + num,
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
                    CompLevel = 6 + num,
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
                    CompLevel = 2 + num,
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
                    CompSubType = (int)ETurretType.LIGHTNING,
                    ReplaceExistingComp = true,
                    CompLevel = 3 + num,
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
                    CompSubType = TurretModManager.Instance.GetTurretIDFromName("Particle Lance"),
                    ReplaceExistingComp = true,
                    CompLevel = 6 + num,
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
                    CompType = (int)ESlotType.E_COMP_MAINTURRET,
                    CompSubType = 3,
                    ReplaceExistingComp = true,
                    CompLevel = 6 + num,
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
                    CompLevel = 3 + num,
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
                    CompLevel = 3 + num,
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
                    CompLevel = 3 + num,
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
                    CompLevel = 3 + num,
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
                    CompSubType = (int)MissileModManager.Instance.GetMissileIDFromName("Thermobaric Missile"),
                    ReplaceExistingComp = true,
                    CompLevel = 2 + num,
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
                    CompLevel = 2 + num,
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
                    CompLevel = 6 + num,
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
                    CompLevel = 2 + num,
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
                    CompLevel = 2 + num,
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
                    CompLevel = 2 + num,
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
                    CompLevel = 2 + num,
                    IsCargo = false,
                    CompTypeToReplace = (int)ESlotType.E_COMP_THRUSTER,
                    SlotNumberToReplace = 3
                }
                );
                return caravanParts;
            }
        }
    }
}
