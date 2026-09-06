using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLShipInfo), "Update")]
    internal class VulcanusShipUpdate
    {
        private static void Postfix(PLShipInfo __instance)
        {
            if (PLServer.Instance == null)
                return;
            if (__instance == null)
                return;
            if (PLServer.Instance.HasActiveMissionWithID(8000004))
            {
                bool flag = false;
                if (__instance.ShipTypeID == EShipType.E_ANNIHILATOR)
                {
                    foreach (PLShipComponent pLShipComponent in __instance.MyStats.AllComponents)
                    {
                        if (pLShipComponent is MissionShipFlagComp)
                        {
                            flag = true;
                            break;
                        }
                    }
                }
                if (flag && !__instance.GetIsPlayerShip() && __instance.TeamID == -1)
                {
                    if ((PLServer.Instance.GetActiveMissionWithID(8000004).MyMissionData as PickupMissionData).Sectors[0].Ships.Count < 2 && PLEncounterManager.Instance.PlayerShip != null)
                    {
                        PickupShipData playerShipData = new PickupShipData();
                        playerShipData.Name = PLEncounterManager.Instance.PlayerShip.ShipNameValue;
                        playerShipData.ShipType = (int)PLEncounterManager.Instance.PlayerShip.ShipTypeID;
                        foreach (PLShipComponent pLShipComponent in PLEncounterManager.Instance.PlayerShip.MyStats.AllComponents)
                        {
                            ComponentOverrideData componentOverrideData = new ComponentOverrideData();
                            componentOverrideData.ReplaceExistingComp = true;
                            componentOverrideData.CompTypeToReplace = (int)pLShipComponent.ActualSlotType;
                            componentOverrideData.CompType = (int)pLShipComponent.ActualSlotType;
                            componentOverrideData.CompSubType = pLShipComponent.SubType;
                            componentOverrideData.CompLevel = pLShipComponent.Level;
                            componentOverrideData.IsCargo = pLShipComponent.VisualSlotType == ESlotType.E_COMP_CARGO || pLShipComponent.VisualSlotType == ESlotType.E_COMP_HIDDENCARGO;
                            componentOverrideData.SlotNumberToReplace = pLShipComponent.SortID;
                            playerShipData.AllComponentOverrides.Add(componentOverrideData);
                        }
                        playerShipData.AllComponentOverrides.Add(new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_REAC_COOLING,
                            CompSubType = 2,
                            CompLevel = 0,
                            IsCargo = false,
                            CompSubTypeToReplace = (int)ESlotType.E_COMP_REAC_COOLING,
                            SlotNumberToReplace = 0,
                            ReplaceExistingComp = false
                        });
                        playerShipData.AllComponentOverrides.Add(new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_REAC_COOLING,
                            CompSubType = 3,
                            CompLevel = 0,
                            IsCargo = false,
                            CompSubTypeToReplace = (int)ESlotType.E_COMP_REAC_COOLING,
                            SlotNumberToReplace = 1,
                            ReplaceExistingComp = false
                        });
                        playerShipData.AllComponentOverrides.Add(new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_REAC_COOLING,
                            CompSubType = 5,
                            CompLevel = 0,
                            IsCargo = false,
                            CompSubTypeToReplace = (int)ESlotType.E_COMP_REAC_COOLING,
                            SlotNumberToReplace = 2,
                            ReplaceExistingComp = false
                        });
                        playerShipData.FactionID = PLEncounterManager.Instance.PlayerShip.FactionID;
                        playerShipData.DialogueActorID = "";
                        playerShipData.Flagged = PLEncounterManager.Instance.PlayerShip.IsFlagged;
                        (PLServer.Instance.GetActiveMissionWithID(8000004).MyMissionData as PickupMissionData).Sectors[0].Ships.Add(playerShipData);
                        PLEncounterManager.Instance.PlayerShip.MyStats.AddShipComponent(new MissionNoExtractorFlag());
                        PLEncounterManager.Instance.PlayerShip.DropScrap = false;
                    }
                }
                if (!__instance.GetIsPlayerShip())
                    return;
                if (flag && !PLMissionObjective.IDIsCompleted("ExGal_ClaimVulcanus"))
                    PLMissionObjective_Custom.OnCustomObjEvent("ExGal_ClaimVulcanus");
                else if (!flag && PLMissionObjective.IDIsCompleted("ExGal_ClaimVulcanus"))
                {
                    foreach (PLMissionObjective missionObjective in PLMissionObjective.AllMissionObjectives)
                    {
                        if (missionObjective != null && !missionObjective.IsCompleted && missionObjective is PLMissionObjective_Custom missionObjectiveCustom && missionObjectiveCustom.ScriptName == "ExGal_ClaimVulcanus")
                        {
                            --missionObjectiveCustom.AmountCompleted;
                            break;
                        }
                    }
                }
            }
        }
    }
}

