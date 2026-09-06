using PulsarModLoader.Content.Components.CPU;
using PulsarModLoader.Content.Components.MegaTurret;
using PulsarModLoader.Content.Components.PolytechModule;
using PulsarModLoader.Content.Components.Turret;
using System.Collections.Generic;
using UnityEngine;

namespace ExpandedGalaxy
{
    internal class PFSectorCommander
    {
        internal static int bossFlag = 0;

        internal static List<ComponentOverrideData> GetComponentsFromIteration(int iteration)
        {
            int baseLevel = 3;
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
                            CompLevel = baseLevel + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
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
                            CompLevel = baseLevel + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
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
                            CompLevel = baseLevel + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
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
                            CompLevel = baseLevel + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
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
                            CompLevel = baseLevel + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
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
                            CompLevel = baseLevel + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
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
                            CompLevel = baseLevel + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
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
                            CompLevel = baseLevel + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
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
                            CompLevel = baseLevel + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
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
                            CompLevel = baseLevel + Mathf.FloorToInt((float)PLServer.Instance.ChaosLevel / 3f),
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
                            CompLevel = baseLevel - 1 + Mathf.FloorToInt((float)PLServer.Instance.ChaosLevel / 3f),
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
                            CompLevel = baseLevel - 1 + Mathf.FloorToInt((float)PLServer.Instance.ChaosLevel / 3f),
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
                            CompLevel = baseLevel - 1 + Mathf.FloorToInt((float)PLServer.Instance.ChaosLevel / 3f),
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
                            CompLevel = baseLevel - 1 + Mathf.FloorToInt((float)PLServer.Instance.ChaosLevel / 3f),
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
                            CompLevel = baseLevel + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
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
                            CompLevel = baseLevel + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
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
                            CompLevel = baseLevel + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
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
                            CompLevel = baseLevel + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
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
                            CompLevel = baseLevel + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
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
                            CompLevel = baseLevel + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
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
                            CompLevel = baseLevel + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
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
                            CompLevel = baseLevel + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
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
                            CompLevel = baseLevel + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
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
                            CompLevel = baseLevel + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
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
                            CompLevel = baseLevel + Mathf.FloorToInt((float)PLServer.Instance.ChaosLevel / 3f),
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
                            CompLevel = baseLevel - 1 + Mathf.FloorToInt((float)PLServer.Instance.ChaosLevel / 3f),
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
                            CompLevel = baseLevel - 1 + Mathf.FloorToInt((float)PLServer.Instance.ChaosLevel / 3f),
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
                            CompLevel = baseLevel - 1 + Mathf.FloorToInt((float)PLServer.Instance.ChaosLevel / 3f),
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
                            CompLevel = baseLevel - 1 + Mathf.FloorToInt((float)PLServer.Instance.ChaosLevel / 3f),
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
                            CompLevel = baseLevel + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
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
                            CompLevel = baseLevel + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
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
                            CompLevel = baseLevel + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
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
                            CompLevel = baseLevel + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
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
                            CompLevel = baseLevel + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
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
                            CompLevel = baseLevel + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
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
                            CompLevel = baseLevel + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
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
                            CompLevel = baseLevel + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
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
                            CompLevel = baseLevel + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
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
                            CompLevel = baseLevel + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
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
                            CompLevel = baseLevel + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
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
                            CompLevel = baseLevel + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 6) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
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
                            CompLevel = baseLevel + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
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
                            CompLevel = baseLevel + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
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
                            CompLevel = baseLevel + Mathf.FloorToInt((float)PLServer.Instance.ChaosLevel / 3f),
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
                            CompLevel = baseLevel - 1 + Mathf.FloorToInt((float)PLServer.Instance.ChaosLevel / 3f),
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
                            CompLevel = baseLevel - 1 + Mathf.FloorToInt((float)PLServer.Instance.ChaosLevel / 3f),
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
                            CompLevel = baseLevel - 1 + Mathf.FloorToInt((float)PLServer.Instance.ChaosLevel / 3f),
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
                            CompLevel = baseLevel - 1 + Mathf.FloorToInt((float)PLServer.Instance.ChaosLevel / 3f),
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
                            CompLevel = baseLevel + 1 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
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
                            CompLevel = baseLevel + 1 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
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
                            CompLevel = baseLevel + 1 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
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
                            CompLevel = baseLevel + 1 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
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
                            CompLevel = baseLevel + 1 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
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
                            CompLevel = baseLevel + 1 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
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
                            CompLevel = baseLevel + 1 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
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
                            CompLevel = baseLevel + 1 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
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
                            CompLevel = baseLevel + 1 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
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
                            CompLevel = baseLevel + 1 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
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
                            CompLevel = baseLevel + 1 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
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
                            CompLevel = baseLevel + 1 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
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
                            CompLevel = baseLevel + 1 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 6) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
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
                            CompLevel = baseLevel + 1 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
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
                            CompLevel = baseLevel + 1 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
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
                            CompLevel = baseLevel + Mathf.FloorToInt((float)PLServer.Instance.ChaosLevel / 3f),
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
                            CompLevel = baseLevel + Mathf.FloorToInt((float)PLServer.Instance.ChaosLevel / 3f),
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
                            CompLevel = baseLevel + Mathf.FloorToInt((float)PLServer.Instance.ChaosLevel / 3f),
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
                            CompLevel = baseLevel + Mathf.FloorToInt((float)PLServer.Instance.ChaosLevel / 3f),
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
                            CompLevel = baseLevel + Mathf.FloorToInt((float)PLServer.Instance.ChaosLevel / 3f),
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
                            CompLevel = baseLevel + 1 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
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
                            CompLevel = baseLevel + 1 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
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
                            CompLevel = baseLevel + 1 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
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
                            CompLevel = baseLevel + 1 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
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
                            CompLevel = baseLevel + 1 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
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
                            CompLevel = baseLevel + 1 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
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
                            CompLevel = baseLevel + 1 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
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
                            CompLevel = baseLevel + 1 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
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
                            CompLevel = baseLevel + 1 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
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
                            CompLevel = baseLevel + 1 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
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
                            CompLevel = baseLevel + 1 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
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
                            CompLevel = baseLevel + 1 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 6) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
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
                            CompLevel = baseLevel + 1 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
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
                            CompLevel = baseLevel + 1 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar),
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
                            CompLevel = baseLevel + 1 + Mathf.FloorToInt((float)PLServer.Instance.ChaosLevel / 3f),
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
                            CompLevel = baseLevel + Mathf.FloorToInt((float)PLServer.Instance.ChaosLevel / 3f),
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
                            CompLevel = baseLevel + Mathf.FloorToInt((float)PLServer.Instance.ChaosLevel / 3f),
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
                            CompLevel = baseLevel + Mathf.FloorToInt((float)PLServer.Instance.ChaosLevel / 3f),
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
                            CompLevel = baseLevel + Mathf.FloorToInt((float)PLServer.Instance.ChaosLevel / 3f),
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
    }
}
