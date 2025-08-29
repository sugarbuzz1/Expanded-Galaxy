using HarmonyLib;
using PulsarModLoader;
using PulsarModLoader.Content.Components.CPU;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ExpandedGalaxy
{
    internal class CPU
    {
        public class SylvassiCPU : CPUMod
        {
            public override string Name => "Sylvassi Shield Charger";

            public override string Description => "Shield co-processor that greatly boosts shield charge rate at the cost of a massive power draw.";

            public override int MarketPrice => 20000;

            public override Texture2D IconTexture => (Texture2D)Resources.Load("Icons/63_Processer");

            public override int CargoVisualID => 37;

            public override float MaxPowerUsage_Watts => 1f;

            public override int SysInstConduit => 2;

            public override string GetStatLineLeft(PLShipComponent InComp) => "Shield Recharge" + "\n" + "Status" + "\n";

            public override string GetStatLineRight(PLShipComponent InComp)
            {
                PLCPU plcpu = InComp as PLCPU;

                if (!plcpu.IsEquipped || !plcpu.IsPowered)
                {
                    return "+" + (0).ToString() + "/" + (100 + plcpu.Level * 10).ToString() + "\n" + "INACTIVE" + "\n";
                }
                if (plcpu.SubTypeData == 0)
                {
                    return "+" + (0).ToString() + "/" + (100 + plcpu.Level * 10).ToString() + "\n" + "DISABLED" + "\n";
                }
                else
                {
                    return "+" + ((100 + plcpu.Level * 10) * plcpu.GetPowerPercentInput()).ToString("0") + "/" + (100 + plcpu.Level * 10).ToString() + "\n" + "ENABLED" + "\n";
                }
            }

            public override void OnWarp(PLShipComponent InComp)
            {

            }

            public override void FinalLateAddStats(PLShipComponent InComp)
            {
                PLCPU plcpu = InComp as PLCPU;
                PLShipStats shipStats = InComp.ShipStats;
                if (InComp.ShipStats.ShieldsCurrent < InComp.ShipStats.ShieldsMax)
                {
                    if (InComp.SubTypeData > 0)
                    {
                        shipStats.ShieldsChargeRate += plcpu.GetPowerPercentInput() * (100f + plcpu.Level * 10f);
                        shipStats.ShieldsChargeRateMax += (100f + plcpu.Level * 10f);
                    }
                }
            }

            public override void Tick(PLShipComponent InComp)
            {
                base.Tick(InComp);
                PLCPU plcpu = InComp as PLCPU;
                if (plcpu.SubTypeData < 0 || plcpu.SubTypeData > 1)
                {
                    plcpu.SubTypeData = 0;
                    if (PhotonNetwork.isMasterClient)
                        ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.UpdateSubTypeData", PhotonTargets.Others, new object[3]
                        {
                        (object) InComp.ShipStats.Ship.ShipID,
                        (object) InComp.NetID,
                        (object) InComp.SubTypeData,
                        });
                }
                if (!plcpu.IsEquipped && plcpu.SubTypeData != 0)
                {
                    plcpu.SubTypeData = 0;
                    if (PhotonNetwork.isMasterClient)
                        ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.UpdateSubTypeData", PhotonTargets.Others, new object[3]
                        {
                        (object) InComp.ShipStats.Ship.ShipID,
                        (object) InComp.NetID,
                        (object) InComp.SubTypeData,
                        });
                    return;
                }
                if (plcpu.SubTypeData == 0)
                {
                    plcpu.CalculatedMaxPowerUsage_Watts = 1f;
                }
                else
                {
                    plcpu.CalculatedMaxPowerUsage_Watts = (40000f + 4000f * plcpu.Level);
                }
                if (PLNetworkManager.Instance.MyLocalPawn != null && PLNetworkManager.Instance.MyLocalPawn.CurrentShip != null && PLNetworkManager.Instance.MyLocalPawn.CurrentShip == plcpu.ShipStats.Ship && (PLNetworkManager.Instance.LocalPlayer.GetClassID() == 0 || PLNetworkManager.Instance.LocalPlayer.GetClassID() == 2) && PLInput.Instance.GetButtonUp(PLInputBase.EInputActionName.pilot_ability) && !PLNetworkManager.Instance.LocalPlayer.IsSittingInCaptainsChair() && PLCameraSystem.Instance.CurrentCameraMode != null && PLCameraSystem.Instance.CurrentCameraMode.GetModeString() == "LocalPawn")
                {
                    ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.UpdateSylvassiCPU", PhotonTargets.All, new object[1]
                    {
                        (object)PLNetworkManager.Instance.MyLocalPawn.CurrentShip.ShipID
                    });
                }
            }
        }

        internal class UpdateSylvassiCPU : ModMessage
        {
            public override void HandleRPC(object[] arguments, PhotonMessageInfo sender)
            {
                PLShipInfoBase ship = PLEncounterManager.Instance.GetShipFromID((int)arguments[0]);
                if (ship == null)
                    return;
                PLCPU plcpu = null;
                foreach (PLShipComponent component in ship.MyStats.GetComponentsOfType(ESlotType.E_COMP_CPU))
                {
                    plcpu = component as PLCPU;
                    if (plcpu != null && plcpu.SubType == CPUModManager.Instance.GetCPUIDFromName("Sylvassi Shield Charger"))
                        break;
                }
                if (plcpu == null)
                    return;
                if (plcpu.SubTypeData == 0)
                {
                    plcpu.SubTypeData = 1;
                }
                else
                {
                    plcpu.SubTypeData = 0;
                }
            }
        }
        public class SuperShieldCPU : CPUMod
        {
            public override string Name => "Super Shield";

            public override string Description => "If you're reading this, I fucked up :(";

            public override int MarketPrice => 9999999;

            public override bool CanBeDroppedOnShipDeath => false;

            public override bool Contraband => true;

            public override Texture2D IconTexture => (Texture2D)Resources.Load("Icons/51_Processer");

            public override float MaxPowerUsage_Watts => 100f;

            public override string GetStatLineLeft(PLShipComponent InComp) => "Activating in: " + "\n";

            public override string GetStatLineRight(PLShipComponent InComp)
            {
                if (!InComp.IsEquipped)
                    return "INACTIVE" + "\n";
                int num = InComp.SubTypeData;
                if (num > 0)
                    return (num).ToString("0") + "\n";
                else
                    return "ACTIVE" + "\n";
            }

            public override void Tick(PLShipComponent InComp)
            {
                base.Tick(InComp);
                if (PhotonNetwork.isMasterClient)
                {
                    if (InComp.SubTypeData < 0 || InComp.SubTypeData > 35 * 80)
                        InComp.SubTypeData = 0;
                    if (!InComp.IsEquipped)
                    {
                        InComp.SubTypeData = 30 * 80;
                        return;
                    }
                    PLShipInfo info = InComp.ShipStats.Ship as PLShipInfo;
                    if (info == null)
                        return;
                    if (info.StartupSwitchBoard.GetLateStatus(0))
                    {
                        if (InComp.SubTypeData > 0)
                            InComp.SubTypeData--;
                    }
                    else
                    {
                        InComp.SubTypeData = 30 * 80;
                    }
                    /*ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.UpdateSubTypeData", PhotonTargets.Others, new object[3]
                    {
                        (object) InComp.ShipStats.Ship.ShipID,
                        (object) InComp.NetID,
                        (object) InComp.SubTypeData,
                    });*/
                }
                if (InComp.IsEquipped && InComp.ShipStats.Ship.ShipID == PLEncounterManager.Instance.PlayerShip.ShipID)
                {
                    InComp.FlagForSelfDestruction();
                    return;
                }
                if (InComp.SubTypeData <= 0 && InComp.IsEquipped)
                {
                    InComp.ShipStats.Ship.SuperShieldActivateStartTime = PLServer.Instance.GetEstimatedServerMs() - 2000;
                }
            }
        }

        [HarmonyPatch(typeof(PLShipInfoBase), "AddSensorStrings")]
        internal class SuperShieldTooltip
        {
            private static bool Prefix(PLShipInfoBase __instance, ref List<PLSensorObjectString> inList, PLShipInfoBase inScanningShip)
            {
                if ((UnityEngine.Object)__instance.MySensorObjectShip != (UnityEngine.Object)null && (UnityEngine.Object)inScanningShip != (UnityEngine.Object)null && (UnityEngine.Object)inScanningShip.MySensorObjectShip != (UnityEngine.Object)null && (__instance.Ships_MinDetectCompHistory.Contains(inScanningShip.ShipID) || (UnityEngine.Object)__instance == (UnityEngine.Object)PLEncounterManager.Instance.PlayerShip))
                {
                    if ((UnityEngine.Object)__instance != (UnityEngine.Object)PLEncounterManager.Instance.PlayerShip)
                    {
                        if (__instance.IsInfected)
                            inList.Add(new PLSensorObjectString("[ff0000]Combat Level: ???[-]", 2f));
                        else if ((UnityEngine.Object)PLEncounterManager.Instance.PlayerShip != (UnityEngine.Object)null)
                        {
                            float combatLevel1 = __instance.GetCombatLevel();
                            float combatLevel2 = PLEncounterManager.Instance.PlayerShip.GetCombatLevel();
                            string str = "[ffff00]";
                            if ((double)combatLevel1 > (double)combatLevel2 * 1.3300000429153442)
                                str = "[ff0000]";
                            else if ((double)combatLevel1 < (double)combatLevel2 * 0.6600000262260437)
                                str = "[00ff00]";
                            inList.Add(new PLSensorObjectString(str + PLLocalize.Localize("Combat Level: ") + combatLevel1.ToString("0") + "[-]", 2f));
                        }
                        else
                            inList.Add(new PLSensorObjectString(PLLocalize.Localize("Combat Level: ") + __instance.GetCombatLevel().ToString("0"), 2f));
                        if (__instance.MyStats.GetShipComponent<PLNuclearDevice>(ESlotType.E_COMP_NUCLEARDEVICE) != null)
                            inList.Add(new PLSensorObjectString("[ff0000]ELEVATED RADIATION LEVELS![-]", 3f));
                        List<PLShipComponent> list = __instance.MyStats.GetComponentsOfType(ESlotType.E_COMP_CPU);
                        bool flag = false;
                        foreach (PLShipComponent comp in list)
                        {
                            PLCPU cpu = (PLCPU)comp;
                            if (cpu.SubType == CPUModManager.Instance.GetCPUIDFromName("Super Shield"))
                                flag = true;
                        }
                        if (flag)
                        {
                            if (__instance.IsSupershieldActive())
                                inList.Add(new PLSensorObjectString("Super Shields: [ff0000]Online[-]", 3f));
                            else
                                inList.Add(new PLSensorObjectString("Super Shields: [00ff00]Offline[-]", 3f));
                            inList.Add(new PLSensorObjectString("[f0ff00]SHIELDS WEAK TO EMP ATTACKS![-]", 3f));
                        }
                    }
                    else
                        inList.Add(new PLSensorObjectString(PLLocalize.Localize("Combat Level: ") + __instance.GetCombatLevel().ToString("0"), 2f));
                }
                if (__instance.GetShipTypeName() != "N/A")
                    inList.Add(new PLSensorObjectString(PLLocalize.Localize("is a match to ship type: ") + __instance.GetShipTypeName(), 0.0f));
                if ((UnityEngine.Object)__instance != (UnityEngine.Object)inScanningShip)
                {
                    if (__instance.IsAbandoned())
                        inList.Add(new PLSensorObjectString("appears to be abandoned.", 1f));
                    else if (!__instance.HostileShips.Contains(inScanningShip.ShipID))
                        inList.Add(new PLSensorObjectString("is potentially hostile and armed.", 1f));
                    else
                        inList.Add(new PLSensorObjectString("is currently hostile and armed.", 1f));
                    if (inScanningShip.MySensorObjectShip.IsDetectedBy(__instance))
                        inList.Add(new PLSensorObjectString("can detect you.", 1f));
                    else
                        inList.Add(new PLSensorObjectString("can't detect you.", 1f));
                }
                else
                    inList.Add(new PLSensorObjectString("is this vessel.", 1f));
                if (!__instance.IsInfected)
                {
                    inList.Add(new PLSensorObjectString(PLLocalize.Localize("is registered as ") + PLGlobal.GetFactionTextForFactionID(__instance.FactionID), 0.0f));
                    if (__instance.IsFlagged)
                        inList.Add(new PLSensorObjectString("has been flagged", 0.0f));
                    GeneralInfo gxEntryWithName = PLGlobal.Instance.GetGXEntryWithName(__instance.GX_ID);
                    if (gxEntryWithName == null)
                        return false;
                    inList.Add(new PLSensorObjectString(PLLocalize.Localize("is a match to GX entry ") + gxEntryWithName.ID.ToString(), 1f));
                }
                else
                {
                    GeneralInfo gxEntryWithName = PLGlobal.Instance.GetGXEntryWithName("The Infected");
                    if (gxEntryWithName == null)
                        return false;
                    inList.Add(new PLSensorObjectString(PLLocalize.Localize("is a match to GX entry ") + gxEntryWithName.ID.ToString(), 1f));
                }
                return false;
            }
        }

        [HarmonyPatch(typeof(PLCPU), MethodType.Constructor, new Type[2] { typeof(ECPUClass), typeof(int) })]
        internal class SCPBuff
        {
            private static void Postfix(PLCPU __instance, ECPUClass inClass, int inLevel, ref float ___m_Speed)
            {
                if (inClass == ECPUClass.E_CPUTYPE_SHIELD_COPROCESSOR)
                    ___m_Speed = 10f;
            }
        }
    }
}
