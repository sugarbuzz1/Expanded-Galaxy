using HarmonyLib;
using PulsarModLoader.Content.Components.HullPlating;
using PulsarModLoader.Content.Components.Missile;
using PulsarModLoader.Content.Components.Turret;
using PulsarModLoader.Content.Components.WarpDriveProgram;
using System.Collections.Generic;

namespace ExpandedGalaxy
{
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
                    case "The Recompiler: Config. 1":
                        if (PFSectorCommander.bossFlag < 2)
                            PFSectorCommander.bossFlag = 2;
                        compHash = (int)PLShipComponent.createHashFromInfo(1, 20, 0, 0, (int)ESlotType.E_COMP_NONE);
                        PLPersistantShipInfo boss2 = new PLPersistantShipInfo(EShipType.E_POLYTECH_SHIP, 5, sectorInfo, isFlagged: true)
                        {
                            ForcedHostile = true,
                            ShipName = "The Recompiler: Config. 2",
                            HullPercent = 1f,
                            ShldPercent = 2f,
                            SelectedActorID = "ExGal_Recompiler_2"
                        };
                        boss2.CompOverrides.AddRange((IEnumerable<ComponentOverrideData>)PFSectorCommander.GetComponentsFromIteration(1));
                        PLServer.Instance.AllPSIs.Add(boss2);
                        boss2.CreateShipInstance(encounter);
                        info = (PLShipInfo)boss2.ShipInstance;
                        info.Exterior.transform.position = __instance.Exterior.transform.position;
                        info.Exterior.transform.rotation = __instance.Exterior.transform.rotation;
                        info.DropScrap = false;
                        info.CreditsLeftBehind = 0;
                        Systems.PhaseAway(info);
                        info.TargetShip = PLEncounterManager.Instance.PlayerShip;
                        break;
                    case "The Recompiler: Config. 2":
                        if (PFSectorCommander.bossFlag < 3)
                            PFSectorCommander.bossFlag = 3;
                        compHash = (int)PLShipComponent.createHashFromInfo(16, HullPlatingModManager.Instance.GetHullPlatingIDFromName("NanoActivePlating"), 0, 0, (int)ESlotType.E_COMP_NONE);
                        PLPersistantShipInfo boss3 = new PLPersistantShipInfo(EShipType.E_POLYTECH_SHIP, 5, sectorInfo, isFlagged: true)
                        {
                            ForcedHostile = true,
                            ShipName = "The Recompiler: Config. 3",
                            HullPercent = 1f,
                            ShldPercent = 2f,
                            SelectedActorID = "ExGal_Recompiler_3"
                        };
                        boss3.CompOverrides.AddRange((IEnumerable<ComponentOverrideData>)PFSectorCommander.GetComponentsFromIteration(2));
                        PLServer.Instance.AllPSIs.Add(boss3);
                        boss3.CreateShipInstance(encounter);
                        info = (PLShipInfo)boss3.ShipInstance;
                        info.Exterior.transform.position = __instance.Exterior.transform.position;
                        info.Exterior.transform.rotation = __instance.Exterior.transform.rotation;
                        info.DropScrap = false;
                        info.CreditsLeftBehind = 0;
                        Systems.PhaseAway(info);
                        info.TargetShip = PLEncounterManager.Instance.PlayerShip;
                        break;
                    case "The Recompiler: Config. 3":
                        if (PFSectorCommander.bossFlag < 4)
                            PFSectorCommander.bossFlag = 4;
                        compHash = (int)PLShipComponent.createHashFromInfo(20, MissileModManager.Instance.GetMissileIDFromName("Seeker Missile"), 0, 0, (int)ESlotType.E_COMP_NONE);
                        PLPersistantShipInfo boss4 = new PLPersistantShipInfo(EShipType.E_POLYTECH_SHIP, 5, sectorInfo, isFlagged: true)
                        {
                            ForcedHostile = true,
                            ShipName = "The Recompiler: Config. 4",
                            HullPercent = 1f,
                            ShldPercent = 2f,
                            SelectedActorID = "ExGal_Recompiler_4"
                        };
                        boss4.CompOverrides.AddRange((IEnumerable<ComponentOverrideData>)PFSectorCommander.GetComponentsFromIteration(3));
                        PLServer.Instance.AllPSIs.Add(boss4);
                        boss4.CreateShipInstance(encounter);
                        info = (PLShipInfo)boss4.ShipInstance;
                        info.Exterior.transform.position = __instance.Exterior.transform.position;
                        info.Exterior.transform.rotation = __instance.Exterior.transform.rotation;
                        info.DropScrap = false;
                        info.CreditsLeftBehind = 0;
                        Systems.PhaseAway(info);
                        info.TargetShip = PLEncounterManager.Instance.PlayerShip;
                        break;
                    case "The Recompiler: Config. 4":
                        if (PFSectorCommander.bossFlag < 5)
                            PFSectorCommander.bossFlag = 5;
                        compHash = (int)PLShipComponent.createHashFromInfo(17, WarpDriveProgramModManager.Instance.GetWarpDriveProgramIDFromName("EMP"), 0, 0, (int)ESlotType.E_COMP_NONE);
                        PLPersistantShipInfo boss5 = new PLPersistantShipInfo(EShipType.E_POLYTECH_SHIP, 5, sectorInfo, isFlagged: true)
                        {
                            ForcedHostile = true,
                            ShipName = "The Recompiler: Config. 5",
                            HullPercent = 1f,
                            ShldPercent = 2f,
                            SelectedActorID = "ExGal_Recompiler_5"
                        };
                        boss5.CompOverrides.AddRange((IEnumerable<ComponentOverrideData>)PFSectorCommander.GetComponentsFromIteration(4));
                        PLServer.Instance.AllPSIs.Add(boss5);
                        boss5.CreateShipInstance(encounter);
                        info = (PLShipInfo)boss5.ShipInstance;
                        info.Exterior.transform.position = __instance.Exterior.transform.position;
                        info.Exterior.transform.rotation = __instance.Exterior.transform.rotation;
                        info.DropScrap = false;
                        info.CreditsLeftBehind = 20000;
                        Systems.PhaseAway(info);
                        info.TargetShip = PLEncounterManager.Instance.PlayerShip;
                        break;
                    case "The Recompiler: Config. 5":
                        if (PFSectorCommander.bossFlag < 6)
                            PFSectorCommander.bossFlag = 6;
                        compHash = (int)PLShipComponent.createHashFromInfo(10, TurretModManager.Instance.GetTurretIDFromName("Nanite Railgun"), 0, 0, (int)ESlotType.E_COMP_NONE);
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
}
