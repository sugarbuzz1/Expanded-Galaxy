using HarmonyLib;
using PulsarModLoader;
using PulsarModLoader.Patches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Talents.Framework;
using UnityEngine;
using UnityEngine.UI;

namespace ExpandedGalaxy
{
    internal class Talent
    {
        public class CustomJetpackFuel2 : TalentMod
        {
            public override string Name => "Jetpack Fuel Reserve";

            public override string Description => "Increases your jetpack's fuel reserve by 25% per rank";

            public override int MaxRank => 4;

            public override int[] ResearchCost => new int[6]
            {
                2,
                0,
                0,
                0,
                0,
                0,
            };

            public override int WarpsToResearch => 2;

            public override bool NeedsToBeResearched => true;
        }

        public class ResearchSpecialty2 : TalentMod
        {
            public override string Name => "Research Specialty";

            public override string Description => "Gain a material rebate upon researching a talent";

            public override int MaxRank => 1;

            public override int ClassID => (int)TalentModManager.CharacterClass.Scientist;

            public override int[] ResearchCost => new int[6]
            {
                0,
                0,
                0,
                2,
                0,
                2,
            };

            public override bool NeedsToBeResearched => true;
        }

        [HarmonyPatch(typeof(PLShipInfo), "UpdateResearchTalentChoices")]
        internal class MaterialRebate
        {
            private static bool Prefix(PLShipInfo __instance)
            {
                if (PLServer.Instance != null)
                {
                    if (PLServer.Instance.TalentToResearch != ETalents.MAX)
                    {
                        if ((int)PLServer.Instance.JumpsNeededToResearchTalent == 0 && PhotonNetwork.isMasterClient)
                        {
                            if (PLServer.Instance.TalentToResearch == ETalents.INC_JETPACK || PLServer.Instance.TalentToResearch == (ETalents)TalentModManager.Instance.GetTalentIDFromName("Jetpack Fuel Reserve"))
                            {
                                PLServer.Instance.SetTalentAsUnlocked((int)ETalents.INC_JETPACK);
                                TalentModManager.Instance.UnlockTalent(TalentModManager.Instance.GetTalentIDFromName("Jetpack Fuel Reserve"));

                            }
                            PLPlayer friendlyPlayerSci = PLServer.Instance.GetCachedFriendlyPlayerOfClass(2);
                            if (friendlyPlayerSci != null)
                            {
                                if ((int)friendlyPlayerSci.Talents[TalentModManager.Instance.GetTalentIDFromName("Research Specialty")] != 0)
                                {
                                    TalentInfo info = PLGlobal.GetTalentInfoForTalentType(PLServer.Instance.TalentToResearch);
                                    int min = 0;
                                    for (int i = 0; i < 6; i++)
                                    {
                                        if ((info.ResearchCost[i] > 0 && PLServer.Instance.ResearchMaterials[i] < PLServer.Instance.ResearchMaterials[min]) || info.ResearchCost[min] == 0)
                                            min = i;
                                    }
                                    if (friendlyPlayerSci.GetPhotonPlayer() != null && !friendlyPlayerSci.IsBot)
                                        PLServer.Instance.photonView.RPC("AddNotification_OneString_LocalizedString", friendlyPlayerSci.GetPhotonPlayer(), (object)"+1 [STR0] to supply (from <Research Specialty> talent)", (object)-1, (object)(PLServer.Instance.GetEstimatedServerMs() + 6000), (object)false, (object)PLPawnItem_ResearchMaterial.GetResearchTypeString(min));
                                    PLPlayer friendlyPlayerCap = PLServer.Instance.GetCachedFriendlyPlayerOfClass(0);
                                    if (friendlyPlayerCap != null && friendlyPlayerCap.GetPhotonPlayer() != null)
                                        PLServer.Instance.photonView.RPC("AddNotification_OneString_LocalizedString", friendlyPlayerCap.GetPhotonPlayer(), (object)"+1 [STR0] to supply (from <Research Specialty> talent)", (object)-1, (object)(PLServer.Instance.GetEstimatedServerMs() + 6000), (object)false, (object)PLPawnItem_ResearchMaterial.GetResearchTypeString(min));
                                    PLServer.Instance.AddToShipLog("CRW", "+1 " + PLPawnItem_ResearchMaterial.GetResearchTypeString(min) + " to supply (from <Research Specialty> talent)", Color.white);
                                    ++PLServer.Instance.ResearchMaterials[min];
                                }
                            }
                        }
                    }
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(PLGlobal), "GetTalentInfoForTalentType")]
        internal class MaterialRebalance
        {
            private static void Postfix(PLGlobal __instance, ETalents inTalent, ref Dictionary<int, TalentInfo> ___CachedTalentInfos, ref TalentInfo __result)
            {
                bool flag = false;
                switch (inTalent)
                {
                    case ETalents.INC_JETPACK:
                    case ETalents.PIL_REDUCE_SYS_DAMAGE:
                    case ETalents.ANTI_RAD_INJECTION:
                    case ETalents.CAP_SCREEN_SAFETY:
                    case ETalents.SCI_PROBE_COOLDOWN:
                        flag = true;
                        break;
                }
                if (!flag)
                    return;
                if (___CachedTalentInfos.ContainsKey((int)inTalent))
                    ___CachedTalentInfos.Remove((int)inTalent);
                switch (inTalent)
                {
                    case ETalents.INC_JETPACK:
                        __result.ResearchCost[0] = 2;
                        break;
                    case ETalents.PIL_REDUCE_SYS_DAMAGE:
                        __result.ResearchCost[1] = 3;
                        __result.ResearchCost[4] = 2;
                        __result.ResearchCost[5] = 0;
                        break;
                    case ETalents.ANTI_RAD_INJECTION:
                        __result.ResearchCost[1] = 2;
                        break;
                    case ETalents.CAP_SCREEN_SAFETY:
                        __result.ResearchCost[0] = 2;
                        __result.ResearchCost[5] = 1;
                        break;
                    case ETalents.SCI_PROBE_COOLDOWN:
                        __result.ResearchCost[3] = 1;
                        break;
                }
                ___CachedTalentInfos[(int)inTalent] = __result;
            }
        }

        public class Reloader2 : TalentMod
        {
            public override string Name => "Reloader";

            public override string Description => "Reloads nearby crew weapons over time";

            public override int MaxRank => 5;

            public override int ClassID => (int)TalentModManager.CharacterClass.Weapons;

            public override int[] ResearchCost => new int[6]
            {
                0,
                0,
                0,
                0,
                0,
                1,
            };

            public override int WarpsToResearch => 2;

            public override bool NeedsToBeResearched => true;
        }

        [HarmonyPatch(typeof(PLPlayer), "Update")]
        internal class ReloaderTalent
        {
            private static void Postfix(PLPlayer __instance, ref float ___LastSciHealPawnFrame, ref bool ___sciHealPawnFrame)
            {
                if (__instance.GetPawn() == null || PLNetworkManager.Instance.LocalPlayer != __instance)
                    return;
                PLAmmoRefill refill;
                if (PLNetworkManager.Instance.CurrentGame != null && __instance.GetClassID() == 3 && __instance.Talents[TalentModManager.Instance.GetTalentIDFromName("Reloader")] > 0)
                {
                    if (PLEncounterManager.Instance.PlayerShip != null && PLEncounterManager.Instance.PlayerShip.MyAmmoRefills.Length > 0)
                    {
                        refill = __instance.StartingShip.MyAmmoRefills[0];
                        if (refill == null || !((double)refill.SupplyAmount > 0.0))
                            return;
                    }
                    else
                    {
                        return;
                    }
                    List<int> playerIDs = new List<int>();
                    foreach (PLPawn pawn in PLGameStatic.Instance.AllPawns)
                    {
                        if (pawn != null && pawn.GetPlayer() != null && (int)pawn.GetPlayer().TeamID == (int)__instance.TeamID && !pawn.IsDead && (double)(pawn.transform.position - __instance.GetPawn().transform.position).sqrMagnitude < 9.0 && ((double)Time.time - (double)___LastSciHealPawnFrame > 4.0 || ___sciHealPawnFrame))
                        {
                            playerIDs.Add(pawn.GetPlayer().GetPlayerID());
                        }
                    }
                    if (playerIDs.Count > 0)
                    {
                        ___sciHealPawnFrame = true;

                        ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.ReloadRPC", PhotonTargets.All, new object[3]
                        {
                                __instance.StartingShip.ShipID,
                                playerIDs.ToArray(),
                                (int)__instance.Talents[TalentModManager.Instance.GetTalentIDFromName("Reloader")]
                        });
                        ___LastSciHealPawnFrame = Time.time;
                    }
                }
            }
        }

        internal class ReloadRPC : ModMessage
        {
            public override void HandleRPC(object[] arguments, PhotonMessageInfo sender)
            {
                PLAmmoRefill refill = (PLEncounterManager.Instance.GetShipFromID((int)arguments[0]) as PLShipInfo).MyAmmoRefills[0];
                int[] playerIDs = (int[])arguments[1];
                int refillRate = (int)arguments[2];
                foreach (int playerID in playerIDs)
                {
                    PLPlayer player = PLServer.Instance.GetPlayerFromPlayerID(playerID);
                    if (player == null || player.MyInventory == null)
                        continue;
                    foreach (PLPawnItem item in player.MyInventory.AllItems)
                    {
                        if (item != null && item.UsesAmmo)
                        {
                            if (item.AmmoCurrent < item.AmmoMax)
                            {
                                int num0 = Mathf.CeilToInt(item.AmmoMax * (0.05f * refillRate));
                                if (!(num0 > 0))
                                    num0 = 1;
                                PLMusic.PostEvent("play_sx_player_item_grenadelauncher_ammorefill", player.GetPawn().gameObject);
                                int num1 = item.AmmoCurrent;
                                item.AmmoCurrent += num0;
                                item.AmmoCurrent = Mathf.Min(item.AmmoCurrent, item.AmmoMax);
                                float num2 = Mathf.Abs(item.AmmoCurrent - num1) * (Ammunition.AmmoRefillPercent() / (float)item.AmmoMax);
                                if (!PhotonNetwork.isMasterClient)
                                    refill.SupplyAmount -= item.AmmoCurrent - num2;
                                PLServer.Instance.photonView.RPC("RemoveAmmoSupply", PhotonTargets.MasterClient, arguments[0], 0, num2, PLNetworkManager.Instance.LocalPlayer.GetPlayerID());
                                if (player == PLNetworkManager.Instance.LocalPlayer)
                                {
                                    PLPawn pawn = player.GetPawn();
                                    ++pawn.LerpedVignetteAmount;
                                    pawn.LerpedVignetteAmount = Mathf.Clamp(pawn.LerpedVignetteAmount, 0f, 5f);
                                    pawn.LerpedVignetteSize = 1f;
                                    pawn.LerpedVignetteColor = new Color(0.75f, 0.225f, 0f, 0.75f);
                                }
                            }
                        }
                    }
                }
            }
        }

        public class ProbeLocator : TalentMod
        {
            public override string Name => "Probe Specialty: Locator";

            public override string Description => "Probe spots are highlighted in the sensor dish view";

            public override int MaxRank => 1;

            public override int ClassID => (int)TalentModManager.CharacterClass.Scientist;

            public override int[] ResearchCost => new int[6]
            {
                0,
                0,
                1,
                0,
                2,
                1,
            };

            public override bool NeedsToBeResearched => true;
        }

        [HarmonyPatch(typeof(PLUIOutsideWorldUI), "UpdateKeenUIElements")]
        internal class ProbeLocatorUI
        {
            private static void Postfix(PLUIOutsideWorldUI __instance)
            {
                if (!(PLCameraSystem.Instance.GetModeString() == "SensorDish"))
                    return;
                if ((int)PLNetworkManager.Instance.LocalPlayer.Talents[TalentModManager.Instance.GetTalentIDFromName("Probe Specialty: Locator")] > 0)
                {
                    Traverse traverse = Traverse.Create(__instance);
                    foreach (PLProbePickup plProbePickup in UnityEngine.Object.FindObjectsOfType<PLProbePickup>())
                    {
                        if (plProbePickup != null && !plProbePickup.PickedUp)
                            traverse.Method("RequestKeenUIElement", new Type[3]
                            {
                                typeof(Transform),
                                typeof(string),
                                typeof(PLSpaceLevelObjective)
                            }).GetValue(new object[3]
                            {
                                plProbePickup.transform,
                                "Research",
                                null
                            });
                    }
                }
            }
        }

        public class CrewAcademyTraining : TalentMod
        {
            public override string Name => "Crew Academy Training";

            public override string Description => "All other crew members gain additional talent point (+1 per rank)";

            public override int MaxRank => 4;

            public override int ClassID => (int)TalentModManager.CharacterClass.Captain;

            public override int[] ResearchCost => new int[6]
            {
                2,
                2,
                0,
                0,
                0,
                2,
            };

            public override bool NeedsToBeResearched => true;
        }

        [HarmonyPatch(typeof(PLPlayer), "ServerRankTalent")]
        internal class AddOneTalentPoint
        {
            private static void Postfix(PLPlayer __instance, int inTalentID)
            {
                if (!PhotonNetwork.isMasterClient)
                    return;
                if (inTalentID == TalentModManager.Instance.GetTalentIDFromName("Crew Academy Training"))
                {
                    TalentInfo info = PLGlobal.GetTalentInfoForTalentType((ETalents)inTalentID);
                    if (info == null || (int)__instance.Talents[inTalentID] > info.MaxRank || (int)__instance.TalentPointsAvailable <= 0)
                        return;
                    ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.AddOneTalentPointRPC", PhotonTargets.All, new object[1]
                    {
                        __instance.GetPlayerID()
                    });
                }
            }
        }

        private class AddOneTalentPointRPC : ModMessage
        {
            public override void HandleRPC(object[] arguments, PhotonMessageInfo sender)
            {
                PLPlayer playerFromID = PLServer.Instance.GetPlayerFromPlayerID((int)arguments[0]);
                if (playerFromID == null)
                    return;
                foreach (PLPlayer player in PLServer.Instance.AllPlayers)
                {
                    if (player.TeamID == playerFromID.TeamID && player.GetClassID() != 0)
                        ++player.TalentPointsAvailable;
                }
            }
        }

        [HarmonyPatch(typeof(PLPlayer), "ResetTalentPoints")]
        internal class ResetExtraPoints
        {
            private static bool Prefix(PLPlayer __instance, out bool __state)
            {
                __state = false;
                if (__instance.Talents == null)
                    return true;
                if (__instance.GetClassID() != 0)
                    return true;
                if ((int)__instance.Talents[TalentModManager.Instance.GetTalentIDFromName("Crew Academy Training")] > 0)
                    __state = true;

                return true;
            }
            private static void Postfix(PLPlayer __instance, bool __state)
            {
                if (__instance.GetClassID() != 0)
                {
                    PLPlayer player = PLServer.Instance.GetCachedFriendlyPlayerOfClass(0);
                    if (player != null)
                        __instance.TalentPointsAvailable += player.Talents[TalentModManager.Instance.GetTalentIDFromName("Crew Academy Training")];
                }
                else
                {
                    if (__state)
                    {
                        foreach (PLPlayer player in PLServer.Instance.AllPlayers)
                        {
                            if (player.TeamID == __instance.TeamID && player.GetPlayerID() != __instance.GetPlayerID() && player.GetClassID() != 0)
                                PLServer.Instance.photonView.RpcSecure("ResetTalentPointsOfPlayer", PhotonTargets.All, true, (object)player.GetPlayerID());

                        }
                    }
                }
            }
        }

        public class EngineerImplant : TalentMod
        {
            public override string Name => "Engineer Eye Implant";

            public override string Description => "Gain system and reactor info UI while aboard home ship";

            public override int MaxRank => 1;

            public override int ClassID => (int)TalentModManager.CharacterClass.Engineer;

            public override int[] ResearchCost => new int[6]
            {
                0,
                0,
                1,
                2,
                0,
                0,
            };

            public override int WarpsToResearch => 4;

            public override bool NeedsToBeResearched => true;
        }

        [HarmonyPatch(typeof(PLInGameUI), "Update")]
        internal class HandleEngiUI
        {
            internal static bool SetupUI = false;
            internal static Image[] SystemHealthOutline = new Image[4];
            internal static Image[] SystemHealthBG = new Image[4];
            internal static Image[] SystemHealthBar = new Image[4];
            internal static Image[] SystemIcon = new Image[4];
            internal static Image[] SystemFireIcon = new Image[4];

            private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
            {
                Label succeed = generator.DefineLabel();
                List<CodeInstruction> list = instructions.ToList();

                List<CodeInstruction> targetSequence = new List<CodeInstruction>() {
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(PLInGameUI), "UpdateShipIndicators"))
                };
                List<CodeInstruction> patchSequence = new List<CodeInstruction>()
                {
                    new CodeInstruction(OpCodes.Ldarg_0),
                    CodeInstruction.Call(typeof(ExpandedGalaxy.Talent), "HandleEngineerImplantUI", new Type[1] {
                        typeof(PLInGameUI)
                    }),
                    new CodeInstruction(OpCodes.Brtrue_S, succeed)
                };
                list[5694].labels.Add(succeed);

                return HarmonyHelpers.PatchBySequence(list.AsEnumerable<CodeInstruction>(), targetSequence, patchSequence, HarmonyHelpers.PatchMode.AFTER, HarmonyHelpers.CheckMode.NONNULL, false);
            }
        }

        internal static bool HandleEngineerImplantUI(PLInGameUI pLInGameUI)
        {
            if (ShouldShowEngineerImplantUI())
            {
                if (!pLInGameUI.PilotingUIRoot.activeSelf)
                    pLInGameUI.PilotingUIRoot.SetActive(true);
                if (!HandleEngiUI.SetupUI)
                {
                    pLInGameUI.PilotAbilityRoot.SetActive(false);

                    pLInGameUI.PilotingSpeed.gameObject.SetActive(false);
                    pLInGameUI.PilotingSpeedBGCenter.gameObject.SetActive(false);
                    pLInGameUI.PilotingSpeedIndicator.gameObject.SetActive(false);
                    pLInGameUI.PilotingUIRoot.transform.Find("SpeedBG").gameObject.SetActive(false);

                    pLInGameUI.PilotingUIRoot.transform.Find("BoostBG").localPosition = new Vector3(0f, -238f, 0f);

                    for (int i = 0; i < 4; i++)
                    {
                        if (HandleEngiUI.SystemHealthOutline[i] == null)
                        {
                            Image image = UnityEngine.Object.Instantiate(pLInGameUI.BoostFill);
                            image.transform.SetParent(pLInGameUI.PilotingUIRoot.transform.Find("BoostBG"));
                            image.transform.localPosition = new Vector3(-24f + (16f * i), 23f, 0f);
                            image.rectTransform.sizeDelta = new Vector2(15f, 27f);
                            image.color = Color.white;
                            HandleEngiUI.SystemHealthOutline[i] = image;
                        }
                        else
                        {
                            HandleEngiUI.SystemHealthOutline[i].color = Color.white;
                            HandleEngiUI.SystemHealthOutline[i].gameObject.SetActive(true);
                        }
                        if (HandleEngiUI.SystemHealthBG[i] == null)
                        {
                            Image image = UnityEngine.Object.Instantiate(pLInGameUI.BoostFill);
                            image.transform.SetParent(pLInGameUI.PilotingUIRoot.transform.Find("BoostBG"));
                            image.transform.localPosition = new Vector3(-24f + (16f * i), 23f, 0f);
                            image.rectTransform.sizeDelta = new Vector2(12f, 24f);
                            image.color = Color.black;
                            HandleEngiUI.SystemHealthBG[i] = image;
                        }
                        else
                        {
                            HandleEngiUI.SystemHealthBG[i].gameObject.SetActive(true);
                        }
                        if (HandleEngiUI.SystemFireIcon[i] == null)
                        {
                            Image image = UnityEngine.Object.Instantiate(pLInGameUI.BoostFill);
                            Sprite sprite = Sprite.Create(PLGlobal.Instance.FireIcon, new Rect(0f, 0f, 128f, 128f), new Vector2(0.5f, 0.5f));
                            image.sprite = sprite;
                            image.transform.SetParent(pLInGameUI.PilotingUIRoot.transform.Find("BoostBG"));
                            image.transform.localPosition = new Vector3(-24f + (16f * i), 16f, 0f);
                            image.rectTransform.sizeDelta = new Vector2(12f, 12f);
                            image.color = new Color(1f, 0.6f, 0f, 1f);
                            image.gameObject.SetActive(false);
                            HandleEngiUI.SystemFireIcon[i] = image;
                        }
                        if (HandleEngiUI.SystemHealthBar[i] == null)
                        {
                            Image image = UnityEngine.Object.Instantiate(pLInGameUI.BoostFill);
                            image.transform.SetParent(pLInGameUI.PilotingUIRoot.transform.Find("BoostBG"));
                            image.transform.localPosition = new Vector3(-24f + (16f * i), 23f, 0f);
                            image.rectTransform.sizeDelta = new Vector2(12f, 24f);
                            image.color = Color.red;
                            HandleEngiUI.SystemHealthBar[i] = image;
                        }
                        else
                        {
                            HandleEngiUI.SystemHealthBar[i].transform.localPosition = new Vector3(-24f + (16f * i), 23f, 0f);
                            HandleEngiUI.SystemHealthBar[i].rectTransform.sizeDelta = new Vector2(12f, 24f);
                            HandleEngiUI.SystemHealthBar[i].gameObject.SetActive(true);
                        }
                        if (HandleEngiUI.SystemIcon[i] == null)
                        {
                            Image image = UnityEngine.Object.Instantiate(pLInGameUI.BoostFill);
                            Sprite sprite = Sprite.Create(PLGlobal.Instance.SystemIcon[i], new Rect(0f, 0f, 64f, 64f), new Vector2(0.5f, 0.5f));
                            image.sprite = sprite;
                            image.transform.SetParent(pLInGameUI.PilotingUIRoot.transform.Find("BoostBG"));
                            image.transform.localPosition = new Vector3(-24f + (16f * i), 45.5f, 0f);
                            image.rectTransform.sizeDelta = new Vector2(15f, 15f);
                            image.color = Color.white;
                            HandleEngiUI.SystemIcon[i] = image;
                        }
                        else
                        {
                            HandleEngiUI.SystemIcon[i].color = Color.white;
                            HandleEngiUI.SystemIcon[i].gameObject.SetActive(true);
                        }
                    }

                    pLInGameUI.BoostLabel.text = "COOLANT";
                    pLInGameUI.BoostLabel.color = Color.white;
                    pLInGameUI.BoostFillOutline.gameObject.SetActive(false);
                    pLInGameUI.BoostFill.color = new Color(0f, 0.6f, 0.3f, 1f);

                    pLInGameUI.PilotingControlMode.text = "Pump State:";
                    pLInGameUI.PilotingControlMode.transform.localPosition = new Vector3(-90.6f, -251.9f, 0f);
                    pLInGameUI.PilotingCameraMode.rectTransform.localPosition = new Vector3(-10.6f, -251.9f, 0f);
                    pLInGameUI.PilotingCameraMode.text = "OFF";

                    pLInGameUI.Piloting_ReactorPower.text = "STABILITY 100%";
                    HandleEngiUI.SetupUI = true;
                }

                PLShipInfo playership = PLEncounterManager.Instance.PlayerShip;
                pLInGameUI.BoostFill.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 130f * playership.ReactorCoolantLevelPercent);
                pLInGameUI.BoostFill.transform.localPosition = new Vector3(-65f + (65f * playership.ReactorCoolantLevelPercent), -0.0001f, 0f);

                float lerpSpeed = 0f;
                switch (playership.ReactorCoolingPumpState)
                {
                    case 0:
                        pLInGameUI.PilotingCameraMode.text = "OFF";
                        break;
                    case 1:
                        pLInGameUI.PilotingCameraMode.text = "LOW";
                        lerpSpeed = 3f;
                        break;
                    case 2:
                        pLInGameUI.PilotingCameraMode.text = "HIGH";
                        lerpSpeed = 12f;
                        break;
                }
                if (lerpSpeed == 0f)
                    pLInGameUI.BoostFill.color = new Color(0f, 0.6f, 0.3f, 1f);
                else
                    pLInGameUI.BoostFill.color = new Color(0f, 0.6f, 0.3f, 1f) * (float)(0.89999997615814209 + ((double)Mathf.Sin(Time.time * lerpSpeed) + 0.800000011920929) * 0.10000000149011612);

                pLInGameUI.PilotingSpeed.gameObject.SetActive(false);

                for (int i = 0; i < 4; i++)
                {
                    if (HandleEngiUI.SystemHealthBar[i] != null)
                    {
                        float num = 0f;
                        if (playership.GetSystemFromID(i) != null)
                            num = playership.GetSystemFromID(i).GetHealthRatio();
                        HandleEngiUI.SystemHealthBar[i].rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 24f * num);
                        HandleEngiUI.SystemHealthBar[i].transform.localPosition = new Vector3(-24f + (16f * i), 11f + (12f * num), 0f);
                    }
                    if (HandleEngiUI.SystemHealthOutline[i] != null)
                    {
                        if (playership.GetSystemFromID(i) != null && playership.GetSystemFromID(i).IsOnFire())
                        {
                            float t = Mathf.Clamp01((float)((double)Time.time * 1.5 % 1.25));
                            HandleEngiUI.SystemHealthOutline[i].color = Color.Lerp(new Color(1f, 0.6f, 0.0f, 1f), Color.white, t);
                        }
                        else
                            HandleEngiUI.SystemHealthOutline[i].color = Color.white;
                    }
                    if (HandleEngiUI.SystemIcon[i] != null)
                    {
                        if (playership.GetSystemFromID(i) != null && playership.GetSystemFromID(i).IsOnFire())
                        {
                            float t = Mathf.Clamp01((float)((double)Time.time * 1.5 % 1.25));
                            HandleEngiUI.SystemIcon[i].color = Color.Lerp(new Color(1f, 0.6f, 0.0f, 1f), Color.white, t);
                        }
                        else
                            HandleEngiUI.SystemIcon[i].color = Color.white;
                    }
                    if (HandleEngiUI.SystemFireIcon[i] != null)
                    {
                        if (playership.GetSystemFromID(i) != null && playership.GetSystemFromID(i).IsOnFire())
                        {
                            HandleEngiUI.SystemFireIcon[i].gameObject.SetActive(true);
                        }
                        else
                            HandleEngiUI.SystemFireIcon[i].gameObject.SetActive(false);
                    }
                }

                pLInGameUI.PilotingControlMode.text = "Pump State:";
                pLInGameUI.Piloting_ReactorTemp.text = "TEMP " + (100f * playership.MyStats.ReactorTempCurrent / playership.MyStats.ReactorTempMax).ToString("0") + "%";
                pLInGameUI.Piloting_ReactorPower.text = "STABILITY " + (100f * (1f - playership.CoreInstability)).ToString("0") + "%";
                return true;
            }
            else
            {
                if (HandleEngiUI.SetupUI)
                {
                    pLInGameUI.PilotingSpeedBGCenter.gameObject.SetActive(true);
                    pLInGameUI.PilotingSpeedIndicator.gameObject.SetActive(true);
                    pLInGameUI.PilotingUIRoot.transform.Find("SpeedBG").gameObject.SetActive(true);

                    pLInGameUI.PilotingUIRoot.transform.Find("BoostBG").localPosition = new Vector3(0f, -220f, 0f);

                    for (int i = 0; i < 4; i++)
                    {
                        if (HandleEngiUI.SystemHealthOutline[i] != null)
                            HandleEngiUI.SystemHealthOutline[i].gameObject.SetActive(false);
                        if (HandleEngiUI.SystemHealthBG[i] != null)
                            HandleEngiUI.SystemHealthBG[i].gameObject.SetActive(false);
                        if (HandleEngiUI.SystemHealthBar[i] != null)
                            HandleEngiUI.SystemHealthBar[i].gameObject.SetActive(false);
                        if (HandleEngiUI.SystemIcon[i] != null)
                            HandleEngiUI.SystemIcon[i].gameObject.SetActive(false);
                        if (HandleEngiUI.SystemFireIcon[i] != null)
                            HandleEngiUI.SystemFireIcon[i].gameObject.SetActive(false);

                        pLInGameUI.BoostLabel.text = "BOOST";
                        pLInGameUI.BoostLabel.color = Color.white;
                        pLInGameUI.BoostFillOutline.gameObject.SetActive(true);
                        pLInGameUI.BoostFill.color = PLGlobal.Instance.ClassColors[0];

                        pLInGameUI.PilotingControlMode.text = "Manual";
                        pLInGameUI.PilotingControlMode.transform.localPosition = new Vector3(-110.6f, -249.9f, 0f);
                        pLInGameUI.PilotingCameraMode.rectTransform.localPosition = new Vector3(-110.6f, -237.9f, 0f);
                        pLInGameUI.PilotingCameraMode.text = "Orbit";

                        pLInGameUI.Piloting_ReactorPower.text = "";
                        HandleEngiUI.SetupUI = false;
                    }
                }
                return false;
            }
        }

        private static bool ShouldShowEngineerImplantUI()
        {
            return PLGlobal.Instance.BottomInfoLabelStringTop == "" && PLNetworkManager.Instance.LocalPlayer != null && ((int)PLNetworkManager.Instance.LocalPlayer.Talents[TalentModManager.Instance.GetTalentIDFromName("Engineer Eye Implant")] > 0 || (PLNetworkManager.Instance.LocalPlayer.GetClassID() == 0 && PLServer.Instance.GetCachedFriendlyPlayerOfClass(4) != null && PLServer.Instance.GetCachedFriendlyPlayerOfClass(4).Talents[TalentModManager.Instance.GetTalentIDFromName("Engineer Eye Implant")] > 0)) && PLNetworkManager.Instance.LocalPlayer.GetPawn() != null && !PLNetworkManager.Instance.LocalPlayer.GetPawn().IsDead && PLCameraManager.Instance != null && PLCameraSystem.Instance != null && PLCameraSystem.Instance.CurrentCameraMode != null && PLCameraSystem.Instance.CurrentCameraMode.GetModeString() == "LocalPawn" && !PLNetworkManager.Instance.LocalPlayer.IsSittingInCaptainsChair() && PLNetworkManager.Instance.LocalPlayer.GetPawn().CurrentShip != null && PLNetworkManager.Instance.LocalPlayer.GetPawn().CurrentShip.GetIsPlayerShip();
        }

        [HarmonyPatch(typeof(PLServer), "ServerManualProgramCharge")]
        internal class NoManualResearch
        {
            private static bool Prefix(PLServer __instance, int inShipID, PhotonMessageInfo pmi, out bool __state)
            {
                __state = false;
                PLShipInfoBase shipFromId = PLEncounterManager.Instance.GetShipFromID(inShipID);
                if (!((UnityEngine.Object)shipFromId != (UnityEngine.Object)null))
                    return true;
                __state = shipFromId.WarpCapsuleIsLoaded;
                return true;
            }
            private static void Postfix(PLServer __instance, int inShipID, PhotonMessageInfo pmi, bool __state)
            {
                PLShipInfoBase shipFromId = PLEncounterManager.Instance.GetShipFromID(inShipID);
                if (!((UnityEngine.Object)shipFromId != (UnityEngine.Object)null) || !__state || shipFromId.NumberOfFuelCapsules < 0)
                    return;
                if (shipFromId.MyWarpDrive != null && __instance.TalentToResearch != ETalents.MAX && (int)__instance.JumpsNeededToResearchTalent >= 0)
                    ++__instance.JumpsNeededToResearchTalent;

            }
        }

        internal static void SetTalentsAsUnhidden()
        {
            TalentModManager.Instance.UnHideTalent((int)ETalents.INC_JETPACK);
            TalentModManager.Instance.UnHideTalent((int)ETalents.SCI_RESEARCH_SPECIALTY);
            TalentModManager.Instance.UnHideTalent((int)ETalents.WPN_AMMO_BOOST);
        }
    }
}
