using HarmonyLib;
using PulsarModLoader.Content.Components.MissionShipComponent;
using System.Collections.Generic;

namespace ExpandedGalaxy
{
    internal class Missions
    {
        public class JunkCubeStart
        {
            public static PickupMissionData MissionData => Missions.JunkCubeStart.CreateData();
            private static PickupMissionData CreateData()
            {
                PickupMissionData pickupMissionData = new PickupMissionData();
                pickupMissionData.Name = "\"Special\" Offer"; ;
                pickupMissionData.CanBeAbandonedByPlayers = false;
                pickupMissionData.MissionID = 8000000;
                pickupMissionData.CanBeBlockedByOtherPickupMissions = false;
                List<ObjectiveData> objectiveDatas = new List<ObjectiveData>()
                {
                    new ObjectiveData
                    {
                        ObjType = 2,
                        Data = new Dictionary<string, string>()
                        {
                            {
                                "ScriptName",
                                "ExGal_GotJunkCube1"
                            },
                            {
                                "CustomText",
                                "Find the Junk Cube"
                            },
                            {
                                "PC_ComponentType",
                                "E_COMP_MISSION_COMPONENT"
                            },
                            {
                                "PC_SubType",
                                "8"
                            },
                            {
                                "PC_CompName",
                                "Junk Cube"
                            },
                            {
                                "PC_AmountNeeded",
                                "1"
                            },
                            {
                                "PC_RemoveComponents",
                                "true"
                            }
                        }
                    },
                    new ObjectiveData
                    {
                        ObjType = 0,
                        Data = new Dictionary<string, string>()
                        {
                            {
                                "ScriptName",
                                "ExGal_DeliverJunkCube1"
                            },
                            {
                                "CustomText",
                                "Deliver the Junk Cube to the Caravan"
                            }
                        }
                    },
                };
                pickupMissionData.Objectives.AddRange(objectiveDatas);
                return pickupMissionData;
            }
        }

        public class JunkCubeWait1
        {
            public static PickupMissionData MissionData => Missions.JunkCubeWait1.CreateData();
            private static PickupMissionData CreateData()
            {
                PickupMissionData pickupMissionData = new PickupMissionData();
                pickupMissionData.Name = "Junk Processing";
                pickupMissionData.CanBeAbandonedByPlayers = false;
                pickupMissionData.MissionID = 8000001;
                pickupMissionData.CanBeBlockedByOtherPickupMissions = false;
                List<ObjectiveData> objectiveDatas = new List<ObjectiveData>()
                {
                    new ObjectiveData
                    {
                        ObjType = 21,
                        Data = new Dictionary<string, string>()
                        {
                            {
                                "ScriptName",
                                "ExGal_WaitJunkCube1"
                            },
                            {
                                "CMAJC_Value",
                                "15"
                            }
                        }
                    },
                    new ObjectiveData
                    {
                        ObjType = 0,
                        Data = new Dictionary<string, string>()
                        {
                            {
                                "ScriptName",
                                "ExGal_WaitJunkCubeReturn1"
                            },
                            {
                                "CustomText",
                                "Return to the Caravan"
                            }
                        }
                    },
                };
                pickupMissionData.Objectives.AddRange(objectiveDatas);
                return pickupMissionData;
            }
        }

        public class JunkCubeRetrieval
        {
            public static PickupMissionData MissionData => Missions.JunkCubeRetrieval.CreateData();
            private static PickupMissionData CreateData()
            {
                PickupMissionData pickupMissionData = new PickupMissionData();
                pickupMissionData.Name = "Recover Junk Cube";
                pickupMissionData.CanBeAbandonedByPlayers = false;
                pickupMissionData.MissionID = 8000002;
                pickupMissionData.CanBeBlockedByOtherPickupMissions = false;
                List<ObjectiveData> objectiveDatas = new List<ObjectiveData>()
                {
                    new ObjectiveData
                    {
                        ObjType = 1,
                        Data = new Dictionary<string, string>()
                        {
                            {
                                "CustomText",
                                "Reach the marked sector"
                            },
                            {
                                "RST_SectorTypeValue",
                                "16"
                            },
                            {
                                "RST_MustKillAll",
                                "0"
                            }
                        }
                    },
                    new ObjectiveData
                    {
                        ObjType = 2,
                        Data = new Dictionary<string, string>()
                        {
                            {
                                "ScriptName",
                                "ExGal_GotJunkCube2"
                            },
                            {
                                "CustomText",
                                "Retrieve the Junk Cube"
                            },
                            {
                                "PC_ComponentType",
                                "E_COMP_MISSION_COMPONENT"
                            },
                            {
                                "PC_SubType",
                                "8"
                            },
                            {
                                "PC_CompName",
                                "Junk Cube"
                            },
                            {
                                "PC_AmountNeeded",
                                "1"
                            },
                            {
                                "PC_RemoveComponents",
                                "true"
                            }
                        }
                    },
                    new ObjectiveData
                    {
                        ObjType = 0,
                        Data = new Dictionary<string, string>()
                        {
                            {
                                "ScriptName",
                                "ExGal_DeliverJunkCube2"
                            },
                            {
                                "CustomText",
                                "Deliver the Junk Cube back to the Caravan"
                            }
                        }
                    },
                };
                PickupSectorData sectorData = new PickupSectorData();
                sectorData.SectorType = (int)ESectorVisualIndication.CANYON;
                sectorData.Distance = 36f;
                sectorData.SpawnRegularShipsToo = false;
                sectorData.UniqueType = true;
                pickupMissionData.Sectors.Add(sectorData);
                pickupMissionData.Objectives.AddRange(objectiveDatas);
                return pickupMissionData;
            }
        }

        public class JunkCubeWait2
        {
            public static PickupMissionData MissionData => Missions.JunkCubeWait2.CreateData();
            private static PickupMissionData CreateData()
            {
                PickupMissionData pickupMissionData = new PickupMissionData();
                pickupMissionData.Name = "Junk Reprocessing";
                pickupMissionData.CanBeAbandonedByPlayers = false;
                pickupMissionData.MissionID = 8000003;
                pickupMissionData.CanBeBlockedByOtherPickupMissions = false;
                List<ObjectiveData> objectiveDatas = new List<ObjectiveData>()
                {
                    new ObjectiveData
                    {
                        ObjType = 21,
                        Data = new Dictionary<string, string>()
                        {
                            {
                                "ScriptName",
                                "ExGal_WaitJunkCube2"
                            },
                            {
                                "CMAJC_Value",
                                "15"
                            }
                        }
                    },
                    new ObjectiveData
                    {
                        ObjType = 0,
                        Data = new Dictionary<string, string>()
                        {
                            {
                                "ScriptName",
                                "ExGal_WaitJunkCubeReturn2"
                            },
                            {
                                "CustomText",
                                "Return to the Caravan"
                            }
                        }
                    },
                };
                List<RewardData> rewardDatas = new List<RewardData>()
                {
                    new RewardData
                    {
                        RwdType = 3,
                        RewardDataA = (int)ESlotType.E_COMP_MISSION_COMPONENT,
                        RewardDataB = (int)MissionShipComponentModManager.Instance.GetMissionShipComponentIDFromName("Reward"),
                        RewardAmount = 1

                    }
                };
                pickupMissionData.SuccessRewards.AddRange(rewardDatas);
                pickupMissionData.Objectives.AddRange(objectiveDatas);
                return pickupMissionData;
            }
        }

        public class PLMissionObjective_CompleteAfterJumpCount : PLMissionObjective
        {
            public PLMissionObjective_CompleteAfterJumpCount(int inJumpCount)
            {
                this.AmountNeeded = inJumpCount;
                this.AmountCompleted = 0;
                if (this.AmountNeeded <= 1)
                    return;
                this.m_ObjectiveText = PLLocalize.Localize("Complete mission after ") + this.AmountNeeded.ToString() + " " + PLLocalize.Localize("jumps.");
            }

            public override void CheckIfCompleted() => this.IsCompleted = this.AmountCompleted >= this.AmountNeeded;

            protected override string CustomPostString() => " (" + this.AmountCompleted.ToString() + "/" + this.AmountNeeded.ToString() + ")";

            public static void OnShipWarp()
            {
                foreach (PLMissionObjective missionObjective in PLMissionObjective.AllMissionObjectives)
                {
                    if (missionObjective != null && !missionObjective.MyMissionIsCompleted && missionObjective is PLMissionObjective_CompleteAfterJumpCount completeWithinJumpCount)
                        ++completeWithinJumpCount.AmountCompleted;
                }
            }
        }

        [HarmonyPatch(typeof(PLMissionObjective_CompleteWithinJumpCount), "OnShipWarp")]
        internal class CompleteAfterJumpWarp
        {
            private static void Postfix()
            {
                PLMissionObjective_CompleteAfterJumpCount.OnShipWarp();
            }
        }

        [HarmonyPatch(typeof(PLMissionBase), "CreateObjectiveFromData")]
        internal class AddCompleteAfterJump
        {
            private static bool Prefix(PLMissionBase __instance, PLMissionBase inMission, ObjectiveData inObjData)
            {
                PLMissionObjective missionObjective = null;
                if (inObjData.ObjType == 21)
                {
                    missionObjective = (PLMissionObjective)new PLMissionObjective_CompleteAfterJumpCount(int.Parse(inObjData.GetValueFromKey("CMAJC_Value")));
                }
                else
                    return true;
                if (missionObjective == null)
                    return true;
                missionObjective.Init();
                missionObjective.RawCustomText = inObjData.GetValueFromKey("CustomText");
                missionObjective.ScriptName = inObjData.GetValueFromKey("ScriptName");
                inMission.Objectives.Add(missionObjective);
                return false;
            }
        }

        [HarmonyPatch(typeof(PLLocalize), "Localize", new System.Type[] { typeof(string), typeof(string), typeof(bool) })]
        private class SkipLocalization
        {
            private static void Postfix(ref string __result, string value)
            {
                if (!(__result == string.Empty))
                    return;
                __result = value;
            }
        }

        [HarmonyPatch(typeof(PLCampaignIO), "GetMissionOfTypeID")]
        internal class CreateMissions
        {
            private static void Postfix(PLCampaignIO __instance, int inID, ref MissionData __result)
            {
                if (__result != null)
                    return;
                switch (inID)
                {
                    case 8000000:
                        PickupMissionData missiondata1 = Missions.JunkCubeStart.MissionData;
                        PLCampaignIO.Instance.GetAllPickupMissionData().Add(missiondata1);
                        __result = (MissionData)missiondata1;
                        break;
                    case 8000001:
                        PickupMissionData missiondata2 = Missions.JunkCubeWait1.MissionData;
                        PLCampaignIO.Instance.GetAllPickupMissionData().Add(missiondata2);
                        __result = (MissionData)missiondata2;
                        break;
                    case 8000002:
                        PickupMissionData missiondata3 = Missions.JunkCubeRetrieval.MissionData;
                        PLCampaignIO.Instance.GetAllPickupMissionData().Add(missiondata3);
                        __result = (MissionData)missiondata3;
                        break;
                    case 8000003:
                        PickupMissionData missiondata4 = Missions.JunkCubeWait2.MissionData;
                        PLCampaignIO.Instance.GetAllPickupMissionData().Add(missiondata4);
                        __result = (MissionData)missiondata4;
                        break;
                }
            }
        }
    }
}
