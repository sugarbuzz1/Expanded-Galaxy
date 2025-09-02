using CodeStage.AntiCheat.ObscuredTypes;
using HarmonyLib;
using PulsarModLoader;
using PulsarModLoader.Content.Components.CaptainsChair;
using PulsarModLoader.Content.Components.CPU;
using PulsarModLoader.Content.Components.MissionShipComponent;
using PulsarModLoader.Patches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

namespace ExpandedGalaxy
{
    public class Systems
    {
        private static string bottomInfo = "";
        private static string bottomInput = "";

        public static void setBottomInfoText(string infoText, string inputText = "")
        {
            Systems.bottomInfo = infoText;
            Systems.bottomInput = inputText;
        }

        public static async void TickDamage(int ticks, PLSpaceTarget target, float dmg, bool bottomHit, EDamageType dmgType, int SystemTargetID, PLShipInfoBase attackingShip, int turretID)
        {
            if (target is PLShipInfoBase)
            {
                PLShipInfoBase targetShip = (PLShipInfoBase)target;
                for (int i = ticks; i > 0; i--)
                {
                    if (!(targetShip != null && targetShip.MyStats != null && targetShip.MyStats.Ship != null) && !targetShip.HasBeenDestroyed)
                        return;
                    await Task.Delay(1000);
                    targetShip.TakeDamage(dmg, bottomHit, dmgType, 1f, SystemTargetID, attackingShip, turretID);
                }
            }
            else
            {
                for (int i = ticks; i > 0; i--)
                {
                    if (!(target != null) || !(target.GetHPAlphaCurrent() > 0f))
                        return;
                    await Task.Delay(1000);
                    target.TakeDamage(dmg);
                }
            }
        }

        [HarmonyPatch(typeof(PLPlayer), "Update")]
        internal class PawnAbilityHandler
        {
            private static void Postfix(PLPlayer __instance)
            {
                if ((UnityEngine.Object)PLServer.Instance != (UnityEngine.Object)null)
                {
                    if (PLNetworkManager.Instance != (UnityEngine.Object)null && PLNetworkManager.Instance.LocalPlayer != null && PLNetworkManager.Instance.LocalPlayer != __instance)
                        return;
                    setBottomInfoText("");
                    if ((UnityEngine.Object)PLNetworkManager.Instance != (UnityEngine.Object)null && (UnityEngine.Object)PLNetworkManager.Instance.MyLocalPawn != (UnityEngine.Object)null && (UnityEngine.Object)PLNetworkManager.Instance.MyLocalPawn.MyController != (UnityEngine.Object)null && PLCameraSystem.Instance.CurrentCameraMode != null && PLCameraSystem.Instance.CurrentCameraMode.GetModeString() == "LocalPawn" && (UnityEngine.Object)PLNetworkManager.Instance.LocalPlayer != (UnityEngine.Object)null)
                    {
                        if (PLNetworkManager.Instance.MyLocalPawn.CurrentShip != null)
                        {

                            PLShipInfo ship = __instance.GetPawn().CurrentShip;
                            if (__instance.IsSittingInCaptainsChair())
                            {
                                if (ship.MyStats.GetShipComponent<PLCaptainsChair>(ESlotType.E_COMP_CAPTAINS_CHAIR) != null && ship.MyStats.GetShipComponent<PLCaptainsChair>(ESlotType.E_COMP_CAPTAINS_CHAIR).SubType == CaptainsChairModManager.Instance.GetCaptainsChairIDFromName("Seat of the Surveyor"))
                                {
                                    PLCaptainsChair chair = ship.MyStats.GetShipComponent<PLCaptainsChair>(ESlotType.E_COMP_CAPTAINS_CHAIR);
                                    if (chair.SubTypeData == 0)
                                        setBottomInfoText("<color=yellow>[" + PLInput.Instance.GetPrimaryKeyStringForAction(PLInputBase.EInputActionName.pilot_ability) + "] Launch Drone</color>");
                                    else if (chair.SubTypeData > 0)
                                        setBottomInfoText("<color=yellow>[" + PLInput.Instance.GetPrimaryKeyStringForAction(PLInputBase.EInputActionName.pilot_ability) + "] Control Drone</color>");
                                }
                            }
                            else
                            {
                                PLCPU plcpu = null;
                                foreach (PLShipComponent component in ship.MyStats.GetComponentsOfType(ESlotType.E_COMP_CPU))
                                {
                                    plcpu = component as PLCPU;
                                    if (plcpu != null && plcpu.SubType == CPUModManager.Instance.GetCPUIDFromName("Sylvassi Shield Charger"))
                                        break;
                                }
                                if (plcpu != null)
                                {
                                    if (plcpu.SubType == CPUModManager.Instance.GetCPUIDFromName("Sylvassi Shield Charger"))
                                    {
                                        if (!(PLNetworkManager.Instance.LocalPlayer.GetClassID() == 0 || PLNetworkManager.Instance.LocalPlayer.GetClassID() == 2))
                                            return;
                                        if (plcpu.SubTypeData == 0)
                                        {
                                            setBottomInfoText("<color=yellow>[" + PLInput.Instance.GetPrimaryKeyStringForAction(PLInputBase.EInputActionName.pilot_ability) + "] Enable Shield Charger</color>");
                                        }
                                        else
                                        {
                                            setBottomInfoText("<color=yellow>[" + PLInput.Instance.GetPrimaryKeyStringForAction(PLInputBase.EInputActionName.pilot_ability) + "] Disable Shield Charger</color>");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        [HarmonyPatch(typeof(PLGameStatic), "Update")]
        internal class ShowAbilityText
        {
            private static void Postfix(PLGameStatic __instance)
            {
                if (PLNetworkManager.Instance.MyLocalPawn != null && PLNetworkManager.Instance.LocalPlayer != null)
                {
                    Traverse traverse = Traverse.Create(PLGlobal.Instance);
                    if (PLCameraSystem.Instance.CurrentCameraMode != null && (PLCameraSystem.Instance.CurrentCameraMode.GetModeString() == "Pilot" || PLCameraSystem.Instance.CurrentCameraMode.GetModeString() == "SensorDish"))
                    {
                        traverse.Field("m_BottomInfoLabelString").SetValue("");
                        traverse.Field("m_BottomInfoLabelString_InputAction").SetValue("");
                        traverse.Field("m_BottomInfoLabelStringTop").SetValue("");
                        traverse.Field("m_BottomInfoLabelStringTop_InputAction").SetValue("");
                    }
                    else
                    {
                        if (PLGlobal.Instance.BottomInfoLabelString == "")
                        {
                            traverse.Field("m_BottomInfoLabelString").SetValue(bottomInfo);
                            traverse.Field("m_BottomInfoLabelString_InputAction").SetValue(bottomInput);
                        }
                    }
                }
            }

            private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
            {
                Label failed = generator.DefineLabel();
                Label failed2 = generator.DefineLabel();
                List<CodeInstruction> list = instructions.ToList();

                List<CodeInstruction> targetSequence = new List<CodeInstruction>() {
                    CodeInstruction.LoadField(typeof(CargoObjectDisplay), "DisplayedItem"),
                    new CodeInstruction(OpCodes.Callvirt),
                    new CodeInstruction(OpCodes.Ldc_I4_S, (sbyte)21),
                    new CodeInstruction(OpCodes.Bne_Un)
                };
                List<CodeInstruction> patchSequence = new List<CodeInstruction>()
                {
                    CodeInstruction.Call(typeof(ExpandedGalaxy.Systems), "IsCargoScrappable", new Type[1] {
                        typeof(CargoObjectDisplay)
                    }),
                    new CodeInstruction(OpCodes.Brfalse_S, failed),
                };
                list[4998].labels.Add(failed);
                list[5772].labels.Add(failed2);

                List<CodeInstruction> list2 = HarmonyHelpers.PatchBySequence(list.AsEnumerable<CodeInstruction>(), targetSequence, patchSequence, HarmonyHelpers.PatchMode.REPLACE, HarmonyHelpers.CheckMode.NONNULL, false).ToList();

                List<CodeInstruction> targetSequence2 = new List<CodeInstruction>()
                {
                    new CodeInstruction(OpCodes.Ldstr, "Leave Captain's Chair"),
                    new CodeInstruction(OpCodes.Ldc_I4_0),
                    new CodeInstruction(OpCodes.Call),
                    new CodeInstruction(OpCodes.Ldstr, ""),
                    new CodeInstruction(OpCodes.Ldstr, "activate_station"),
                    new CodeInstruction(OpCodes.Callvirt),
                    new CodeInstruction(OpCodes.Ldsfld),
                    new CodeInstruction(OpCodes.Ldc_I4_S),
                    new CodeInstruction(OpCodes.Callvirt),
                    new CodeInstruction(OpCodes.Brfalse)
                };
                List<CodeInstruction> patchSequence2 = new List<CodeInstruction>()
                {
                    CodeInstruction.LoadField(typeof(PLNetworkManager), "Instance"),
                    CodeInstruction.LoadField(typeof(PLNetworkManager), "LocalPlayer"),
                    new CodeInstruction(OpCodes.Ldnull),
                    CodeInstruction.Call(typeof(UnityEngine.Object), "op_Inequality", new Type[2]
                    {
                        typeof(UnityEngine.Object),
                        typeof(UnityEngine.Object)
                    }),
                    new CodeInstruction(OpCodes.Brfalse, failed2),
                    CodeInstruction.LoadField(typeof(PLNetworkManager), "Instance"),
                    CodeInstruction.LoadField(typeof(PLNetworkManager), "LocalPlayer"),
                    new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(PLPlayer), "GetPlayerID")),
                    CodeInstruction.Call(typeof(ExpandedGalaxy.Systems), "IsPlayerPiloting", new Type[1]
                    {
                        typeof(int)
                    }),
                    new CodeInstruction(OpCodes.Brtrue, failed2)
                };

                return HarmonyHelpers.PatchBySequence(list2.AsEnumerable<CodeInstruction>(), targetSequence2, patchSequence2, HarmonyHelpers.PatchMode.AFTER, HarmonyHelpers.CheckMode.NONNULL, true);
            }
        }

        public static bool IsPlayerPiloting(int playerID)
        {
            if (playerID == -1)
                return false;
            foreach (PLShipInfoBase infoBase in UnityEngine.Object.FindObjectsOfType<PLShipInfoBase>())
            {
                if (infoBase.GetCurrentShipControllerPlayerID() == playerID)
                    return true;
            }
            return false;
        }

        internal static bool IsCargoScrappable(CargoObjectDisplay cargo)
        {
            if (cargo.DisplayedItem.ActualSlotType == ESlotType.E_COMP_MISSION_COMPONENT && (cargo.DisplayedItem.SubType == MissionShipComponentModManager.Instance.GetMissionShipComponentIDFromName("Ammunition Cache") || cargo.DisplayedItem.SubType == MissionShipComponentModManager.Instance.GetMissionShipComponentIDFromName("Reward")))
                return true;
            if (cargo.DisplayedItem.ActualSlotType == ESlotType.E_COMP_SCRAP)
                return true;
            if (PLServer.GetCurrentSector() != null && PLServer.GetCurrentSector().VisualIndication == ESectorVisualIndication.SPACE_SCRAPYARD)
            {
                if (PLServer.Instance.CurrentCrewCredits >= (cargo.DisplayedItem.Level + 1) * 800)
                    return !cargo.DisplayedItem.ImportantItem && !cargo.DisplayedItem.Contraband && !Relic.getIsRelic(cargo.DisplayedItem);
            }
            return false;
        }

        public static PLPlayer GetPlayerFromPhotonPlayer(PhotonPlayer photon)
        {
            foreach (PLPlayer allPlayer in PLServer.Instance.AllPlayers)
            {
                if ((UnityEngine.Object)allPlayer != (UnityEngine.Object)null && allPlayer.GetPhotonPlayer() != null && allPlayer.GetPhotonPlayer().Equals(photon))
                    return allPlayer;
            }
            return (PLPlayer)null;
        }

        [HarmonyPatch(typeof(PLServer), "ClaimShip")]
        internal class LoseRepOnShipClaim
        {
            private static bool Prefix(PLServer __instance, int inShipID)
            {
                PLShipInfo shipFromId = PLEncounterManager.Instance.GetShipFromID(inShipID) as PLShipInfo;
                if (shipFromId == null || shipFromId == PLEncounterManager.Instance.PlayerShip)
                    return true;
                if (!shipFromId.Abandoned && !shipFromId.IsFlagged && !shipFromId.NoRepLossOnKilled && shipFromId.TeamID == 1 && shipFromId.FactionID != -1 && shipFromId.FactionID < 6)
                {
                    if (!PLServer.Instance.LongRangeCommsDisabled)
                    {
                        ref ObscuredInt local = ref PLServer.Instance.RepLevels[shipFromId.FactionID];
                        local = (ObscuredInt)((int)local - 2);
                        PLServer.Instance.AddToShipLog_OneStringLocalized("REP", "-2 Rep for [STR0] (due to removing claim)", Color.white, inString0: PLGlobal.GetFactionTextForFactionID(shipFromId.FactionID));
                    }
                    else
                        PLServer.Instance.AddToShipLog("REP", "Long range transmission blocked (prevented Reputation loss)", Color.white);
                }
                return true;
            }
        }

        internal static PLShipInfoBase TurretTargetingUI(PLTurret turret)
        {
            if (PLUIOutsideWorldUI.Instance != null && PLCameraSystem.Instance.GetModeString() == "Turret")
            {
                float greatestDot = 0f;
                PLShipInfoBase shipInfo = null;
                Traverse traverse = Traverse.Create(PLUIOutsideWorldUI.Instance);
                foreach (PLShipInfoBase pLShipInfoBase in UnityEngine.Object.FindObjectsOfType<PLShipInfoBase>())
                {
                    if (pLShipInfoBase != turret.ShipStats.Ship)
                    {
                        Vector3 dir = (pLShipInfoBase.Exterior.transform.position - turret.ShipStats.Ship.Exterior.transform.position).normalized;
                        float dot = Vector3.Dot(dir, turret.TurretInstance.RefJoint.forward.normalized);
                        if (dot > 0.85f && dot > greatestDot)
                        {
                            greatestDot = dot;
                            shipInfo = pLShipInfoBase;
                        }
                    }
                }
                if (shipInfo != null)
                {
                    traverse.Method("RequestKeenUIElement", new Type[3]
                            {
                                typeof(Transform),
                                typeof(string),
                                typeof(PLSpaceLevelObjective)
                            }).GetValue(new object[3]
                            {
                                shipInfo.Exterior.transform,
                                "Target",
                                null
                            });
                    return shipInfo;
                }
            }
            return null;
        }

        public class PilotDrone : ModMessage
        {
            public override void HandleRPC(object[] arguments, PhotonMessageInfo sender)
            {
                if (sender.sender != PhotonNetwork.masterClient)
                    return;
                Systems.DelayedPilotDrone((int)arguments[0]);
            }
        }

        internal static async void DelayedPilotDrone(int shipID)
        {
            float Timer = Time.time;
            PLShipInfoBase ship = PLEncounterManager.Instance.GetShipFromID(shipID);
            YieldAwaitable yieldAwaitable;
            while ((UnityEngine.Object)ship == (UnityEngine.Object)null && (double)Time.time - (double)Timer < 15.0)
            {
                ship = PLEncounterManager.Instance.GetShipFromID(shipID);
                yieldAwaitable = Task.Yield();
                await yieldAwaitable;
            }
            if ((double)Time.time - (double)Timer >= 15.0 && (UnityEngine.Object)ship == (UnityEngine.Object)null)
            {
                ship = (PLShipInfoBase)null;
            }
            else
            {
                if ((UnityEngine.Object)ship.PilotingSystem == (UnityEngine.Object)null)
                {
                    ship.PilotingSystem = ship.gameObject.AddComponent<PLPilotingSystem>();
                    ship.PilotingSystem.MyShipInfo = ship;
                }
                if ((UnityEngine.Object)ship.PilotingHUD == (UnityEngine.Object)null)
                {
                    ship.PilotingHUD = ship.gameObject.AddComponent<PLPilotingHUD>();
                    ship.PilotingHUD.MyShipInfo = ship;
                }
                ship.OrbitCameraMaxDistance = 40f;
                ship.OrbitCameraMinDistance = 7f;
                yieldAwaitable = Task.Yield();
                await yieldAwaitable;
                ship.photonView.RPC("NewShipController", PhotonTargets.All, (object)(int)PLNetworkManager.Instance.LocalPlayerID);
            }
        }

        public static async void PhaseAway(PLShipInfo ship)
        {
            if (!((UnityEngine.Object)ship != (UnityEngine.Object)null))
            {
                ship = (PLShipInfo)null;
            }
            else
            {
                float StartedPhasing = Time.time;
                UnityEngine.Object.Instantiate<GameObject>(PLGlobal.Instance.PhasePS, ship.Exterior.transform.position, Quaternion.identity);
                GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(PLGlobal.Instance.PhaseTrailPS, ship.Exterior.transform.position, Quaternion.identity);
                if ((UnityEngine.Object)gameObject != (UnityEngine.Object)null)
                {
                    PLPhaseTrail component = gameObject.GetComponent<PLPhaseTrail>();
                    if ((UnityEngine.Object)component != (UnityEngine.Object)null)
                    {
                        component.StartPos = ship.Exterior.transform.position;
                        component.End = ship.Exterior.transform;
                    }
                    component = (PLPhaseTrail)null;
                }
                foreach (PLShipInfoBase plshipInfoBase in PLEncounterManager.Instance.AllShips.Values)
                {
                    if ((UnityEngine.Object)plshipInfoBase != (UnityEngine.Object)null && (UnityEngine.Object)plshipInfoBase.MySensorObjectShip != (UnityEngine.Object)null)
                    {
                        PLSensorObjectCacheData plsensorObjectCacheData = plshipInfoBase.MySensorObjectShip.IsDetectedBy_CachedInfo((PLShipInfoBase)ship);
                        if (plsensorObjectCacheData != null)
                        {
                            plsensorObjectCacheData.LastDetectedCheckTime = 0.0f;
                            plsensorObjectCacheData.IsDetected = false;
                        }
                        plsensorObjectCacheData = (PLSensorObjectCacheData)null;
                    }
                }
                Systems.DelayedEndPhasePS(ship);
                List<MeshRenderer> exteriorRenderers = ship.ExteriorRenderers;
                MeshRenderer[] hullplanting = ship.HullPlatingRenderers;
                List<PLShipComponent> componentsOfType = ship.MyStats.GetComponentsOfType(ESlotType.E_COMP_TURRET);
                componentsOfType.AddRange((IEnumerable<PLShipComponent>)ship.MyStats.GetComponentsOfType(ESlotType.E_COMP_MAINTURRET));
                componentsOfType.AddRange((IEnumerable<PLShipComponent>)ship.MyStats.GetComponentsOfType(ESlotType.E_COMP_AUTO_TURRET));
                ship.Exterior.transform.position = ship.Exterior.transform.position + ship.Exterior.transform.forward * 200f;
                PLMusic.PostEvent("play_sx_ship_enemy_phasedrone_warp", ship.Exterior);
                while ((double)Time.time - (double)StartedPhasing < 1.0)
                {
                    ship.MyStats.EMSignature = 0.0f;
                    ship.MyStats.CanBeDetected = (ObscuredBool)false;
                    foreach (Renderer rend in exteriorRenderers)
                    {
                        if ((UnityEngine.Object)rend != (UnityEngine.Object)null)
                            rend.enabled = false;
                    }
                    MeshRenderer[] meshRendererArray = hullplanting;
                    for (int index = 0; index < meshRendererArray.Length; ++index)
                    {
                        Renderer rend = (Renderer)meshRendererArray[index];
                        if ((UnityEngine.Object)rend != (UnityEngine.Object)null)
                            rend.enabled = false;
                        rend = (Renderer)null;
                    }
                    meshRendererArray = (MeshRenderer[])null;
                    foreach (PLShipComponent comp in componentsOfType)
                    {
                        PLTurret turret = comp as PLTurret;
                        if (turret != null && (UnityEngine.Object)turret.TurretInstance != (UnityEngine.Object)null)
                        {
                            Renderer[] rendererArray = turret.TurretInstance.MyMainRenderers;
                            for (int index = 0; index < rendererArray.Length; ++index)
                            {
                                Renderer rend = rendererArray[index];
                                rend.enabled = false;
                                rend = (Renderer)null;
                            }
                            rendererArray = (Renderer[])null;
                        }
                        turret = (PLTurret)null;
                    }
                    if ((ship is PLFluffyShipInfo || ship is PLFluffyShipInfo2) && (UnityEngine.Object)(ship as PLFluffyShipInfo).MyVisibleBomb != (UnityEngine.Object)null)
                        (ship as PLFluffyShipInfo).MyVisibleBomb.gameObject.SetActive(false);
                    ship.MyStats.ThrustOutputCurrent = 0.0f;
                    ship.MyStats.ManeuverThrustOutputCurrent = 0.0f;
                    ship.MyStats.InertiaThrustOutputCurrent = 0.0f;
                    if ((UnityEngine.Object)ship.GetExteriorMeshCollider() != (UnityEngine.Object)null)
                        ship.GetExteriorMeshCollider().enabled = false;
                    await Task.Yield();
                }
                foreach (Renderer rend in exteriorRenderers)
                {
                    if ((UnityEngine.Object)rend != (UnityEngine.Object)null)
                        rend.enabled = true;
                }
                MeshRenderer[] meshRendererArray1 = hullplanting;
                for (int index = 0; index < meshRendererArray1.Length; ++index)
                {
                    Renderer rend = (Renderer)meshRendererArray1[index];
                    if ((UnityEngine.Object)rend != (UnityEngine.Object)null)
                        rend.enabled = true;
                    rend = (Renderer)null;
                }
                meshRendererArray1 = (MeshRenderer[])null;
                componentsOfType = ship.MyStats.GetComponentsOfType(ESlotType.E_COMP_TURRET);
                componentsOfType.AddRange((IEnumerable<PLShipComponent>)ship.MyStats.GetComponentsOfType(ESlotType.E_COMP_MAINTURRET));
                componentsOfType.AddRange((IEnumerable<PLShipComponent>)ship.MyStats.GetComponentsOfType(ESlotType.E_COMP_AUTO_TURRET));
                foreach (PLShipComponent comp in componentsOfType)
                {
                    PLTurret turret = comp as PLTurret;
                    if (turret != null && (UnityEngine.Object)turret.TurretInstance != (UnityEngine.Object)null)
                    {
                        Renderer[] rendererArray = turret.TurretInstance.MyMainRenderers;
                        for (int index = 0; index < rendererArray.Length; ++index)
                        {
                            Renderer rend = rendererArray[index];
                            rend.enabled = true;
                            rend = (Renderer)null;
                        }
                        rendererArray = (Renderer[])null;
                    }
                    turret = (PLTurret)null;
                }
                if ((ship is PLFluffyShipInfo || ship is PLFluffyShipInfo2) && (UnityEngine.Object)(ship as PLFluffyShipInfo).MyVisibleBomb != (UnityEngine.Object)null)
                    (ship as PLFluffyShipInfo).MyVisibleBomb.gameObject.SetActive(true);
                if ((UnityEngine.Object)ship.GetExteriorMeshCollider() != (UnityEngine.Object)null)
                    ship.GetExteriorMeshCollider().enabled = true;
                ship.MyStats.CanBeDetected = (ObscuredBool)true;
                PLMusic.PostEvent("stop_sx_ship_enemy_phasedrone_warp", ship.Exterior);
                gameObject = (GameObject)null;
                exteriorRenderers = (List<MeshRenderer>)null;
                hullplanting = (MeshRenderer[])null;
                componentsOfType = (List<PLShipComponent>)null;
                ship = (PLShipInfo)null;
            }
        }

        private static async void DelayedEndPhasePS(PLShipInfo ship)
        {
            await Task.Delay(1000);
            UnityEngine.Object.Instantiate<GameObject>(PLGlobal.Instance.PhasePS, ship.Exterior.transform.position, Quaternion.identity);
        }

        internal class EMPPulse : ModMessage
        {
            public override void HandleRPC(object[] arguments, PhotonMessageInfo sender)
            {
                int inID = (int)arguments[0];
                float range = (float)arguments[1];
                PLShipStats stats = PLEncounterManager.Instance.GetShipFromID(inID).MyStats;
                PLShipInfo shipFromId = PLEncounterManager.Instance.GetShipFromID(inID) as PLShipInfo;
                UnityEngine.Object.Instantiate<GameObject>(PLGlobal.Instance.EMPExplosionPrefab, shipFromId.Exterior.transform.position, Quaternion.identity);
                if (PhotonNetwork.isMasterClient)
                {
                    foreach (PLShipInfoBase plShipInfoBase in UnityEngine.Object.FindObjectsOfType(typeof(PLShipInfoBase)))
                    {
                        if ((UnityEngine.Object)plShipInfoBase != (UnityEngine.Object)shipFromId && (plShipInfoBase.GetCurrentSensorPosition() - shipFromId.GetCurrentSensorPosition()).magnitude < (range / 5f))
                            plShipInfoBase.photonView.RPC("Overcharged", PhotonTargets.All);
                    }
                }
            }
        }

        internal class UpdateSubTypeData : ModMessage
        {
            public override void HandleRPC(object[] arguments, PhotonMessageInfo sender)
            {
                PLShipInfoBase shipInfo = PLEncounterManager.Instance.GetShipFromID((int)arguments[0]);
                if (shipInfo.MyStats != null)
                {
                    shipInfo.MyStats.GetComponentFromNetID((int)arguments[1]).SubTypeData = (short)arguments[2];
                }
            }
        }

        internal class SendWarning : ModMessage
        {
            public override void HandleRPC(object[] arguments, PhotonMessageInfo sender)
            {
                PLTabMenu.Instance.TimedErrorMsg = (string)arguments[0];
            }
        }


        [HarmonyPatch(typeof(PLShipInfo), "GetUpgradableComponents")]
        internal class MoreUpgrades
        {
            public static void Patch(PLShipInfo instance, List<PLShipComponent> m_CachedUpgradableComponents)
            {
                m_CachedUpgradableComponents.AddRange((IEnumerable<PLShipComponent>)instance.MyStats.GetComponentsOfType(ESlotType.E_COMP_AUTO_TURRET, false));
                m_CachedUpgradableComponents.AddRange((IEnumerable<PLShipComponent>)instance.MyStats.GetComponentsOfType(ESlotType.E_COMP_HULLPLATING, false));
            }
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                return HarmonyHelpers.PatchBySequence(instructions, new CodeInstruction[]
{
                new CodeInstruction(OpCodes.Ldarg_0, null),
                new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(PLShipInfo), "m_CachedUpgradableComponents")),
                new CodeInstruction(OpCodes.Callvirt, null)
                }, new CodeInstruction[]
                {
                new CodeInstruction(OpCodes.Ldarg_0, null),
                new CodeInstruction(OpCodes.Dup, null),
                new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(PLShipInfo), "m_CachedUpgradableComponents")),
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(MoreUpgrades), "Patch", null, null)),
                }, HarmonyHelpers.PatchMode.AFTER, HarmonyHelpers.CheckMode.NONNULL, false);
            }
        }

        [HarmonyPatch(typeof(PLTurret), "ShouldAIFire")]
        internal class AIFireBase
        {
            private static bool Prefix(PLTurret __instance, bool operatedByBot, float heatOffset, float heatGeneratedOnFire, ref bool __result)
            {
                if (__instance.ShipStats != null && (UnityEngine.Object)__instance.ShipStats.Ship != (UnityEngine.Object)null)
                {
                    if (__instance.ShipStats.Ship == PLEncounterManager.Instance.PlayerShip)
                    {
                        if (!__instance.ShipStats.Ship.IsAuxSystemActive(4))
                        {
                            __result = false;
                            return false;
                        }
                    }
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(PLFlamelanceTurret), "ShouldAIFire")]
        internal class AIFireFlamelance
        {
            private static bool Prefix(PLFlamelanceTurret __instance, bool operatedByBot, float heatOffset, float heatGenOnFire, ref bool __result)
            {
                if (__instance.ShipStats != null && (UnityEngine.Object)__instance.ShipStats.Ship != (UnityEngine.Object)null)
                {
                    if (__instance.ShipStats.Ship.ShipID == PLEncounterManager.Instance.PlayerShip.ShipID)
                    {
                        if (!__instance.ShipStats.Ship.IsAuxSystemActive(4))
                        {
                            __result = false;
                            return false;
                        }
                    }
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(PLUIOutsideWorldUI), "Update")]
        internal class alwaysShowLead
        {
            private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                List<CodeInstruction> list = instructions.ToList<CodeInstruction>();
                list[99].opcode = OpCodes.Nop;
                list[100].opcode = OpCodes.Nop;
                list[101].opcode = OpCodes.Nop;
                list[102].opcode = OpCodes.Nop;
                list[103].opcode = OpCodes.Nop;
                list[104].opcode = OpCodes.Ldc_I4_1;
                list[104].operand = (object)null;
                return list.AsEnumerable<CodeInstruction>();
            }
        }

        [HarmonyPatch(typeof(PLGalaxy), "Reset")]
        internal class AddExtraFactions
        {
            private static void Postfix(PLGalaxy __instance, int inSeed)
            {
                Traverse traverse = Traverse.Create(__instance);
                traverse.Method("AddFaction", new Type[1] { typeof(PLFactionInfo) }).GetValue(new PLFactionInfo_Polytechnic());
                traverse.Method("AddFaction", new Type[1] { typeof(PLFactionInfo) }).GetValue(new PLFactionInfo_Unknown());
            }
        }

        internal class PLFactionInfo_Unknown : PLFactionInfo
        {
            public override void Setup(PLGalaxy inGalaxy)
            {
                base.Setup(inGalaxy);
                this.FactionID = 6;
            }

            public override void CreateStartingPoints(int inSeed)
            {
            }
        }
    }
}

