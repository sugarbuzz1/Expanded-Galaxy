using HarmonyLib;
using PulsarModLoader.Patches;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using UnityEngine;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLCUInvestigatorDrone), "Update")]
    internal class ScanComponentsFromScreen
    {
        internal static bool ScanFromScreen(PLShipInfo currentShip, Transform target)
        {
            if (target.gameObject.TryGetComponent<PLUIScreen>(out PLUIScreen screen))
            {
                if (screen is PLClonedScreen)
                    screen = ((PLClonedScreen)screen).MyTargetScreen;
                List<PLShipComponent> scanningComponents = new List<PLShipComponent>();
                if (screen is PLCaptainScreen)
                {
                    scanningComponents.AddRange(currentShip.MyStats.GetComponentsOfType(ESlotType.E_COMP_CAPTAINS_CHAIR));
                }
                else if (screen is PLUIPilotingScreen)
                {
                    scanningComponents.AddRange(currentShip.MyStats.GetComponentsOfType(ESlotType.E_COMP_THRUSTER));
                    scanningComponents.AddRange(currentShip.MyStats.GetComponentsOfType(ESlotType.E_COMP_INERTIA_THRUSTER));
                    scanningComponents.AddRange(currentShip.MyStats.GetComponentsOfType(ESlotType.E_COMP_MANEUVER_THRUSTER));
                }
                else if (screen is PLScientistComputerScreen)
                {
                    scanningComponents.AddRange(currentShip.MyStats.GetComponentsOfType(ESlotType.E_COMP_SHLD));
                    scanningComponents.AddRange(currentShip.MyStats.GetComponentsOfType(ESlotType.E_COMP_HULL));
                    scanningComponents.AddRange(currentShip.MyStats.GetComponentsOfType(ESlotType.E_COMP_HULLPLATING));
                    scanningComponents.AddRange(currentShip.MyStats.GetComponentsOfType(ESlotType.E_COMP_REACTOR));
                    scanningComponents.AddRange(currentShip.MyStats.GetComponentsOfType(ESlotType.E_COMP_TURRET));
                    scanningComponents.AddRange(currentShip.MyStats.GetComponentsOfType(ESlotType.E_COMP_MAINTURRET));
                    scanningComponents.AddRange(currentShip.MyStats.GetComponentsOfType(ESlotType.E_COMP_AUTO_TURRET));
                    scanningComponents.AddRange(currentShip.MyStats.GetComponentsOfType(ESlotType.E_COMP_PROGRAM));
                    scanningComponents.AddRange(currentShip.MyStats.GetComponentsOfType(ESlotType.E_COMP_CPU));
                    scanningComponents.AddRange(currentShip.MyStats.GetComponentsOfType(ESlotType.E_COMP_WARP));
                    scanningComponents.AddRange(currentShip.MyStats.GetComponentsOfType(ESlotType.E_COMP_CARGO));
                    scanningComponents.AddRange(currentShip.MyStats.GetComponentsOfType(ESlotType.E_COMP_SALVAGE_SYSTEM));
                    scanningComponents.AddRange(currentShip.MyStats.GetComponentsOfType(ESlotType.E_COMP_CLOAKING_SYS));
                    scanningComponents.AddRange(currentShip.MyStats.GetComponentsOfType(ESlotType.E_COMP_POLYTECH_MODULE));
                    scanningComponents.AddRange(currentShip.MyStats.GetComponentsOfType(ESlotType.E_POWERED_ARMOR));
                    scanningComponents.AddRange(currentShip.MyStats.GetComponentsOfType(ESlotType.E_COMP_SENS));
                    scanningComponents.AddRange(currentShip.MyStats.GetComponentsOfType(ESlotType.E_COMP_THRUSTER));
                    scanningComponents.AddRange(currentShip.MyStats.GetComponentsOfType(ESlotType.E_COMP_INERTIA_THRUSTER));
                    scanningComponents.AddRange(currentShip.MyStats.GetComponentsOfType(ESlotType.E_COMP_MANEUVER_THRUSTER));
                }
                else if (screen is PLScientistSensorScreen)
                {
                    scanningComponents.AddRange(currentShip.MyStats.GetComponentsOfType(ESlotType.E_COMP_SENS));
                    scanningComponents.AddRange(currentShip.MyStats.GetComponentsOfType(ESlotType.E_COMP_SENSORDISH));
                    foreach (PLPoweredShipComponent component in currentShip.MyStats.AllPoweredComponents)
                    {
                        if (!scanningComponents.Contains(component) && (double)component.InputPower_Watts > 1.5)
                            scanningComponents.Add(component);
                    }
                }
                else if (screen is PLMissileLauncherScreen)
                    scanningComponents.AddRange(currentShip.MyStats.GetComponentsOfType(ESlotType.E_COMP_TRACKERMISSILE));
                else if (screen is PLWeaponsNukeScreen)
                {
                    scanningComponents.AddRange(currentShip.MyStats.GetComponentsOfType(ESlotType.E_COMP_NUCLEARDEVICE));
                    scanningComponents.AddRange(currentShip.MyStats.GetComponentsOfType(ESlotType.E_COMP_BISCUIT_BOMB));
                }
                else if (screen is PLEngineerAuxReactorScreen)
                    scanningComponents.AddRange(currentShip.MyStats.GetComponentsOfType(ESlotType.E_COMP_O2));
                else if (screen is PLEngineerReactorScreen)
                    scanningComponents.AddRange(currentShip.MyStats.GetComponentsOfType(ESlotType.E_COMP_REACTOR));
                else if (screen is PLWarpDriveScreen)
                    scanningComponents.AddRange(currentShip.MyStats.GetComponentsOfType(ESlotType.E_COMP_WARP));

                foreach (PLShipComponent component1 in scanningComponents)
                {
                    if (component1.Contraband && UnityEngine.Random.Range(0, 100) < 70)
                        return true;
                }
                return false;
            }
            else
                return false;
        }

        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> list = instructions.ToList();

            List<CodeInstruction> targetSequence = new List<CodeInstruction>()
                {
                    new CodeInstruction(OpCodes.Ldc_I4_0),
                    new CodeInstruction(OpCodes.Ldc_I4_S),
                    new CodeInstruction(OpCodes.Call),
                    new CodeInstruction(OpCodes.Brtrue_S),
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Ldfld),
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Ldfld),
                    new CodeInstruction(OpCodes.Callvirt),
                    new CodeInstruction(OpCodes.Callvirt),
                    new CodeInstruction(OpCodes.Brtrue_S),
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Ldfld),
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Ldfld),
                    new CodeInstruction(OpCodes.Callvirt),
                    new CodeInstruction(OpCodes.Callvirt),
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Call),
                    new CodeInstruction(OpCodes.Stfld),
                };
            List<CodeInstruction> patchSequence = new List<CodeInstruction>()
                {
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(PLCombatTarget), "CurrentShip")),
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(PLCUInvestigatorDrone), "currentInvestigationTarget")),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(ScanComponentsFromScreen), "ScanFromScreen", new System.Type[2] {typeof(PLShipInfo), typeof(Transform)})),
                    new CodeInstruction(OpCodes.Stloc_S, (byte)13)
                };

            return HarmonyHelpers.PatchBySequence(list.AsEnumerable<CodeInstruction>(), targetSequence, patchSequence, HarmonyHelpers.PatchMode.AFTER, HarmonyHelpers.CheckMode.NONNULL, false);
        }
    }
}
