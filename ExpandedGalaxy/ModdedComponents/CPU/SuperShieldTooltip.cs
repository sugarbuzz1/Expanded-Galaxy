using HarmonyLib;
using PulsarModLoader.Content.Components.CPU;
using System.Collections.Generic;

namespace ExpandedGalaxy
{
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
                /*
                ModdedSensorObjectCacheData socd = (ModdedSensorObjectCacheData)inScanningShip.MySensorObjectShip.IsDetectedBy_CachedInfo(__instance);
                if ((double)socd.DetectionSignal < 1.0)
                    inList.Add(new PLSensorObjectString("sensor lock: none", 1.5f));
                else if ((double)socd.DetectionSignal < 5.0)
                    inList.Add(new PLSensorObjectString("sensor lock: low", 1.5f));
                else if ((double)socd.DetectionSignal < 10.0)
                    inList.Add(new PLSensorObjectString("sensor lock: medium", 1.5f));
                else if ((double)socd.DetectionSignal < 18.0)
                    inList.Add(new PLSensorObjectString("sensor lock: high", 1.5f));
                else
                    inList.Add(new PLSensorObjectString("sensor lock: full", 1.5f));
                */
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
}
