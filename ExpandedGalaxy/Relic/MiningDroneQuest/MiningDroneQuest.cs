using HarmonyLib;
using PulsarModLoader;
using PulsarModLoader.Content.Components.Hull;
using PulsarModLoader.Content.Components.MegaTurret;
using PulsarModLoader.Content.Components.Missile;
using PulsarModLoader.Content.Components.Reactor;
using PulsarModLoader.Content.Components.Shield;
using PulsarModLoader.Content.Components.Turret;
using PulsarModLoader.Content.Components.WarpDrive;
using System.Collections.Generic;
using UnityEngine;

namespace ExpandedGalaxy
{
    internal class MiningDroneQuest
    {
        internal static bool dronesActive = true;
        internal static int GXData = 0;

        

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
                    SwitchBox = __instance.CreateTexture(PLGlobal.Instance.WhitePixel, new Vector3(37.5f, -60f), new Vector2(80f, 35f), new Color(0.65f, 0.65f, 0.65f), __instance.MyRootPanel.transform, UIWidget.Pivot.Center);

                    LabelOn = __instance.CreateLabel("ON", new Vector3(37.5f, -60f), 16, new Color(0.65f, 0.65f, 0.65f), __instance.MyRootPanel.transform, UIWidget.Pivot.Center);

                    LabelOff = __instance.CreateLabel("OFF", new Vector3(-37.5f, -60f), 16, new Color(0.65f, 0.65f, 0.65f), __instance.MyRootPanel.transform, UIWidget.Pivot.Center);

                    if (__instance.gameObject != null) __instance.gameObject.transform.localPosition = new Vector3(20f, -21f, -20f);
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
                    if (MiningDroneQuest.dronesActive)
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
                        if (PLInput.Instance.GetButtonUp(PLInputBase.EInputActionName.click) && !MiningDroneQuest.dronesActive)
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
                        if (PLInput.Instance.GetButtonUp(PLInputBase.EInputActionName.click) && MiningDroneQuest.dronesActive)
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
                        MiningDroneQuest.dronesActive = (bool)arguments[0];
                        MiningDroneQuest.MiningHubScreen.UpdateScreen.click = (int)arguments[1];
                }
            }
        }
        internal static List<ComponentOverrideData> GetComponentsFromDroneType(int type, PLRand rand)
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
                        CompSubType = (int)ShieldModManager.Instance.GetShieldIDFromName("Layered Surface Projector"),
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
                        CompSubType = (int)WarpDriveModManager.Instance.GetWarpDriveIDFromName("Extration Drone Warp Drive"),
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
                        CompType = (int)ESlotType.E_COMP_REAC_COOLING,
                        CompSubType = (int)5,
                        ReplaceExistingComp = false,
                        CompLevel = 0,
                        IsCargo = false,
                        CompTypeToReplace = (int)ESlotType.E_COMP_REAC_COOLING,
                        SlotNumberToReplace = 0
                    }
                    );
                    droneParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_REAC_COOLING,
                            CompSubType = 0,
                            ReplaceExistingComp = false,
                            CompLevel = 0,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_REAC_COOLING,
                            SlotNumberToReplace = 1
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
                        CompSubType = (int)ShieldModManager.Instance.GetShieldIDFromName("Layered Surface Projector"),
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
                        CompSubType = (int)WarpDriveModManager.Instance.GetWarpDriveIDFromName("Asset Protection Drive"),
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
                        CompType = (int)ESlotType.E_COMP_REAC_COOLING,
                        CompSubType = (int)5,
                        ReplaceExistingComp = false,
                        CompLevel = 0,
                        IsCargo = false,
                        CompTypeToReplace = (int)ESlotType.E_COMP_REAC_COOLING,
                        SlotNumberToReplace = 0
                    }
                    );
                    droneParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_REAC_COOLING,
                            CompSubType = 0,
                            ReplaceExistingComp = false,
                            CompLevel = 1 + rand.Next(3),
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_REAC_COOLING,
                            SlotNumberToReplace = 1
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
                        CompSubType = (int)ShieldModManager.Instance.GetShieldIDFromName("Layered Surface Projector"),
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
                        CompType = (int)ESlotType.E_COMP_REAC_COOLING,
                        CompSubType = (int)5,
                        ReplaceExistingComp = false,
                        CompLevel = 0,
                        IsCargo = false,
                        CompTypeToReplace = (int)ESlotType.E_COMP_REAC_COOLING,
                        SlotNumberToReplace = 0
                    }
                    );
                    droneParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_REAC_COOLING,
                            CompSubType = 0,
                            ReplaceExistingComp = false,
                            CompLevel = 4,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_REAC_COOLING,
                            SlotNumberToReplace = 1
                        }
                        );
                    droneParts.Add
                        (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_REAC_COOLING,
                            CompSubType = 8,
                            ReplaceExistingComp = false,
                            CompLevel = 0,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_REAC_COOLING,
                            SlotNumberToReplace = 2
                        }
                        );
                    break;
            }
            return droneParts;
        }

        

        
    }
}
