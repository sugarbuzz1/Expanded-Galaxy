using HarmonyLib;
using PulsarModLoader;
using PulsarModLoader.Patches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;
using UnityEngine;

namespace ExpandedGalaxy
{
    internal class SensorDish
    {
        private static PLPlayer lastToInteract;

        private static Dictionary<PLScientistSensorScreen, int> screenInfos = new Dictionary<PLScientistSensorScreen, int>();
        private static Dictionary<PLScientistSensorScreen, UITexture[]> buttons = new Dictionary<PLScientistSensorScreen, UITexture[]>();
        private static Dictionary<PLScientistSensorScreen, UITexture[]> buttonsCustom = new Dictionary<PLScientistSensorScreen, UITexture[]>();

        private static List<int> turretMissileTick = new List<int>();

        public class AncientSensorDish : PLSensorDish
        {
            public GameObject beam;
            public AncientSensorDish(ESensorDishType inType, int inLevel) : base(inType, inLevel)
            {
                this.SubType = (int)inType;
                this.Level = inLevel;
                this.SubTypeData = 0;
                this.Name = "Ancient Sensor Dish";
                this.Desc = "A sensor dish more focused on combat capabilaties rather than assisting in ship detection.";
                this.CalculatedMaxPowerUsage_Watts = 6000f;
                this.CargoVisualPrefabID = 45;
            }

            public override void Tick()
            {
                base.Tick();
                if (PhotonNetwork.isMasterClient)
                    ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.UpdateSubTypeData", PhotonTargets.Others, new object[3]
                    {
                        (object) this.ShipStats.Ship.ShipID,
                        (object) this.NetID,
                        (object) this.SubTypeData,
                    });
            }
        }
        public static PLSensorDish CreateSensorDish(int Subtype, int level)
        {
            PLSensorDish sensorDish = new PLSensorDish(ESensorDishType.E_NORMAL, level);
            sensorDish.CargoVisualPrefabID = 44;
            if (Subtype > 0)
            {
                return new AncientSensorDish((ESensorDishType)Subtype, level);
            }
            return sensorDish;
        }

        [HarmonyPatch(typeof(PLShipComponent), "OnWarp")]
        internal class warpDataReset
        {
            private static void Postfix(PLShipComponent __instance)
            {
                PLSensorDish sensorDish = __instance as PLSensorDish;
                if (sensorDish == null)
                    return;
                sensorDish.SubTypeData = 0;
            }
        }

        public static UITexture[] createButtonsFromSubType(PLScientistSensorScreen screen, int subType)
        {
            UITexture[] uITextures = new UITexture[6];
            Traverse traverse = Traverse.Create(screen);
            UIWidget ShipInfoScreen_WeaknessRoot = traverse.Field("ShipInfoScreen_WeaknessRoot").GetValue<UIWidget>();
            if (subType == 1)
            {
                float num = 33f;
                object[] params1;
                params1 = new object[6]
                {
                    (Texture2D)Resources.Load("Icons/78_Thrusters"),
                    new Vector3(175f, -70f),
                    new Vector2(num, num),
                    Color.black,
                    ShipInfoScreen_WeaknessRoot.transform,
                    UIWidget.Pivot.TopLeft
                };
                uITextures[0] = traverse.Method("CreateTexture", new Type[6] { typeof(Texture2D), typeof(Vector3), typeof(Vector2), typeof(Color), typeof(Transform), typeof(UIWidget.Pivot) }).GetValue<UITexture>(params1);
                params1 = new object[6]
                {
                    (Texture2D)Resources.Load("Icons/20_Hull"),
                    new Vector3(175f, -115f),
                    new Vector2(num, num),
                    Color.black,
                    ShipInfoScreen_WeaknessRoot.transform,
                    UIWidget.Pivot.TopLeft
                };
                uITextures[1] = traverse.Method("CreateTexture", new Type[6] { typeof(Texture2D), typeof(Vector3), typeof(Vector2), typeof(Color), typeof(Transform), typeof(UIWidget.Pivot) }).GetValue<UITexture>(params1);
                params1 = new object[6]
                {
                    (Texture2D)Resources.Load("Icons/80_Thrusters"),
                    new Vector3(175f, -160f),
                    new Vector2(num, num),
                    Color.black,
                    ShipInfoScreen_WeaknessRoot.transform,
                    UIWidget.Pivot.TopLeft
                };
                uITextures[2] = traverse.Method("CreateTexture", new Type[6] { typeof(Texture2D), typeof(Vector3), typeof(Vector2), typeof(Color), typeof(Transform), typeof(UIWidget.Pivot) }).GetValue<UITexture>(params1);
                params1 = new object[7]
                {
                    "weaknessScanCyberDef",
                    (Texture2D)Resources.Load("Icons/78_Thrusters"),
                    new Vector3(175f, -70f),
                    new Vector2(num, num),
                    Color.white,
                    ShipInfoScreen_WeaknessRoot.transform,
                    UIWidget.Pivot.TopLeft
                };
                uITextures[3] = traverse.Method("CreateButtonEditable", new Type[7] { typeof(string), typeof(Texture2D), typeof(Vector3), typeof(Vector2), typeof(Color), typeof(Transform), typeof(UIWidget.Pivot) }).GetValue<UITexture>(params1);
                params1 = new object[7]
                {
                    "weaknessScanShieldWeakPoint",
                    (Texture2D)Resources.Load("Icons/20_Hull"),
                    new Vector3(175f, -115f),
                    new Vector2(num, num),
                    Color.white,
                    ShipInfoScreen_WeaknessRoot.transform,
                    UIWidget.Pivot.TopLeft
                };
                uITextures[4] = traverse.Method("CreateButtonEditable", new Type[7] { typeof(string), typeof(Texture2D), typeof(Vector3), typeof(Vector2), typeof(Color), typeof(Transform), typeof(UIWidget.Pivot) }).GetValue<UITexture>(params1);
                params1 = new object[7]
                {
                    "weaknessScanReactor",
                    (Texture2D)Resources.Load("Icons/80_Thrusters"),
                    new Vector3(175f, -160f),
                    new Vector2(num, num),
                    Color.white,
                    ShipInfoScreen_WeaknessRoot.transform,
                    UIWidget.Pivot.TopLeft
                };
                uITextures[5] = traverse.Method("CreateButtonEditable", new Type[7] { typeof(string), typeof(Texture2D), typeof(Vector3), typeof(Vector2), typeof(Color), typeof(Transform), typeof(UIWidget.Pivot) }).GetValue<UITexture>(params1);

            }
            return uITextures;
        }

        [HarmonyPatch(typeof(PLSensorDish), "CreateSensorDishFromHash")]
        internal class SensorDishHashFix
        {
            private static bool Prefix(int inSubType, int inLevel, int inSubTypeData, ref PLShipComponent __result)
            {
                __result = (PLShipComponent)CreateSensorDish(inSubType, inLevel);
                return false;
            }
        }

        [HarmonyPatch(typeof(PLShipInfoBase), "FireProbe")]
        internal class AncientSensorProbe
        {
            private static bool Prefix(PLShipInfoBase __instance, Vector3 startPos, Quaternion startRot, int startMs, int playerIDOwner, int endMs, Vector3 endPos)
            {
                if (__instance.MyStats.GetShipComponent<PLSensorDish>(ESlotType.E_COMP_SENSORDISH) == null || __instance.MyStats.GetShipComponent<PLSensorDish>(ESlotType.E_COMP_SENSORDISH).SubType == 0)
                    return true;
                if (!(__instance.MyStats.GetShipComponent<PLSensorDish>(ESlotType.E_COMP_SENSORDISH) is AncientSensorDish))
                    return true;
                AncientSensorDish sensorDish = __instance.MyStats.GetShipComponent<PLSensorDish>(ESlotType.E_COMP_SENSORDISH) as AncientSensorDish;
                float probeEnergyCost = 0.65f;
                float probeCooldown = 3f;
                PLPlayer player = PLServer.Instance.GetPlayerFromPlayerID(playerIDOwner);
                if (player != null)
                {
                    probeEnergyCost = (float)(0.65 * (1.0 - (double)(int)player.Talents[53] * 0.10000000149011612));
                }
                Traverse traverse = Traverse.Create(__instance);
                traverse.Field("LastFireProbeTime").GetValue<float>();
                if ((double)Time.time - (double)traverse.Field("LastFireProbeTime").GetValue<float>() <= (double)probeCooldown || (double)__instance.SensorDishCapacitorCharge < (double)probeEnergyCost)
                    return false;
                traverse.Field("LastFireProbeTime").SetValue(Time.time);
                if ((double)Time.time - (double)__instance.LastCloakingSystemActivatedTime > 4.0)
                    __instance.SetIsCloakingSystemActive(false);
                __instance.SensorDishCapacitorCharge -= probeEnergyCost;



                Ray ray = new Ray(startPos, startRot * Vector3.forward);
                int layerMask = 524289;
                int num1 = 0;
                if ((UnityEngine.Object)__instance.GetExteriorMeshCollider() != (UnityEngine.Object)null)
                {
                    num1 = __instance.GetExteriorMeshCollider().gameObject.layer;
                    __instance.GetExteriorMeshCollider().gameObject.layer = 31;
                }
                try
                {
                    PLMusic.PostEvent("play_ship_generic_external_weapon_phaseturret_impact", __instance.Exterior.gameObject);
                    UnityEngine.RaycastHit hitInfo;
                    if (Physics.SphereCast(ray, 3f, out hitInfo, 20000f, layerMask))
                    {
                        PLShipInfoBase pLShipInfoBase = (PLShipInfoBase)null;
                        if ((UnityEngine.Object)hitInfo.collider != (UnityEngine.Object)null)
                            pLShipInfoBase = hitInfo.collider.GetComponentInParent<PLShipInfoBase>();
                        if ((UnityEngine.Object)pLShipInfoBase != (UnityEngine.Object)null)
                        {
                            if ((UnityEngine.Object)pLShipInfoBase != (UnityEngine.Object)__instance)
                            {
                                __instance.SensorDishTargetShipID = pLShipInfoBase.ShipID;
                            }
                        }
                    }
                }
                catch
                {
                }
                if (!((UnityEngine.Object)__instance.GetExteriorMeshCollider() != (UnityEngine.Object)null))
                    return false;
                __instance.GetExteriorMeshCollider().gameObject.layer = num1;
                return false;
            }
        }

        internal class cachePlayer : ModMessage
        {
            public override void HandleRPC(object[] arguments, PhotonMessageInfo sender)
            {
                SensorDish.lastToInteract = PLServer.Instance.GetPlayerFromPlayerID((int)arguments[0]);
            }
        }

        [HarmonyPatch(typeof(PLScientistSensorScreen), "OnButtonClick")]
        internal class cacheRealPlayer
        {
            private static bool Prefix(PLScientistSensorScreen __instance, UIWidget inButton)
            {
                if (SensorDish.screenInfos.ContainsKey(__instance))
                {
                    SensorDish.lastToInteract = PLNetworkManager.Instance.LocalPlayer;
                    ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.cachePlayer", PhotonTargets.Others, new object[1]
                    {
                    (object) PLNetworkManager.Instance.LocalPlayer.GetPlayerID(),
                    });
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(PLScientistSensorScreen), "OptimizeForBot")]
        internal class cacheBotPlayer
        {
            private static bool Prefix(PLScientistSensorScreen __instance, PLBot inBot)
            {
                if (!PhotonNetwork.isMasterClient)
                    return true;
                if (!((UnityEngine.Object)inBot != (UnityEngine.Object)null) || !((UnityEngine.Object)inBot.PlayerOwner != (UnityEngine.Object)null) || !((UnityEngine.Object)inBot.PlayerOwner.GetPawn() != (UnityEngine.Object)null) || !((UnityEngine.Object)inBot.PlayerOwner.GetPawn().CurrentShip != (UnityEngine.Object)null) || !((UnityEngine.Object)__instance.MyScreenHubBase.OptionalShipInfo != (UnityEngine.Object)null))
                    return true;
                if (!((UnityEngine.Object)__instance.MyScreenHubBase.OptionalShipInfo.TargetShip != (UnityEngine.Object)null) || (double)__instance.MyScreenHubBase.OptionalShipInfo.TargetShip.MySensorObjectShip.GetDetectionSignal(Vector3.SqrMagnitude(__instance.MyScreenHubBase.OptionalShipInfo.TargetShip.Exterior.transform.position - __instance.MyScreenHubBase.OptionalShipInfo.Exterior.transform.position), __instance.MyScreenHubBase.OptionalShipInfo.TargetShip.MyStats.EMSignature, __instance.MyScreenHubBase.OptionalShipInfo.MyStats.EMDetection, (PLShipInfoBase)__instance.MyScreenHubBase.OptionalShipInfo, (PLSensorObject)__instance.MyScreenHubBase.OptionalShipInfo.TargetShip.MySensorObjectShip) < 18.0 || !((UnityEngine.Object)__instance.MyScreenHubBase.OptionalShipInfo.TargetShip != (UnityEngine.Object)__instance.MyScreenHubBase.OptionalShipInfo) || (double)Time.time - (double)__instance.MyScreenHubBase.OptionalShipInfo.TargetShip.LastRecievedWeaknessTime <= 30.0)
                    return true;
                if (SensorDish.screenInfos.ContainsKey(__instance))
                {
                    SensorDish.lastToInteract = inBot.PlayerOwner;
                    ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.cachePlayer", PhotonTargets.Others, new object[1]
                    {
                    (object) inBot.PlayerOwner.GetPlayerID(),
                    });
                }
                return true;

            }
        }

        [HarmonyPatch(typeof(PLScientistSensorScreen), "Update")]
        internal class updateSensorScreen
        {
            private static bool Prefix(PLScientistSensorScreen __instance)
            {
                if (!__instance.UIIsSetup() || !__instance.LocalPlayerInSameLocation())
                    return true;
                if (__instance.MyScreenHubBase.OptionalShipInfo != null)
                {
                    Traverse traverse = Traverse.Create(__instance);
                    if (__instance.MyScreenHubBase.OptionalShipInfo.MyStats.GetShipComponent<PLSensorDish>(ESlotType.E_COMP_SENSORDISH) == null && SensorDish.screenInfos.ContainsKey(__instance))
                    {
                        if (SensorDish.screenInfos[__instance] != 0)
                        {
                            foreach (UITexture texture1 in SensorDish.buttonsCustom[__instance])
                            {
                                PLGlobal.SafeGameObjectSetActive(texture1.gameObject, false);
                            }
                            foreach (UITexture texture in SensorDish.buttons[__instance])
                            {
                                PLGlobal.SafeGameObjectSetActive(texture.gameObject, true);
                            }
                            SensorDish.screenInfos.Remove(__instance);
                            traverse.Field("weaknessScanCyberDefBG").SetValue(buttons[__instance][0]);
                            traverse.Field("weaknessScanShieldsWeakPointBG").SetValue(buttons[__instance][1]);
                            traverse.Field("weaknessScanReactorWeakPointBG").SetValue(buttons[__instance][2]);
                            traverse.Field("weaknessScanCyberDef").SetValue(buttons[__instance][3]);
                            traverse.Field("weaknessScanShieldsWeakPoint").SetValue(buttons[__instance][4]);
                            traverse.Field("weaknessScanReactorWeakPoint").SetValue(buttons[__instance][5]);
                        }
                    }
                    else if (__instance.MyScreenHubBase.OptionalShipInfo.MyStats.GetShipComponent<PLSensorDish>(ESlotType.E_COMP_SENSORDISH) != null && SensorDish.screenInfos.ContainsKey(__instance))
                    {
                        if (__instance.MyScreenHubBase.OptionalShipInfo.MyStats.GetShipComponent<PLSensorDish>(ESlotType.E_COMP_SENSORDISH).SubType != SensorDish.screenInfos[__instance])
                        {
                            if (SensorDish.screenInfos[__instance] != 0)
                            {
                                foreach (UITexture texture1 in SensorDish.buttonsCustom[__instance])
                                {
                                    PLGlobal.SafeGameObjectSetActive(texture1.gameObject, false);
                                }
                                foreach (UITexture texture in SensorDish.buttons[__instance])
                                {
                                    PLGlobal.SafeGameObjectSetActive(texture.gameObject, true);
                                }
                                SensorDish.screenInfos.Remove(__instance);
                                traverse.Field("weaknessScanCyberDefBG").SetValue(buttons[__instance][0]);
                                traverse.Field("weaknessScanShieldsWeakPointBG").SetValue(buttons[__instance][1]);
                                traverse.Field("weaknessScanReactorWeakPointBG").SetValue(buttons[__instance][2]);
                                traverse.Field("weaknessScanCyberDef").SetValue(buttons[__instance][3]);
                                traverse.Field("weaknessScanShieldsWeakPoint").SetValue(buttons[__instance][4]);
                                traverse.Field("weaknessScanReactorWeakPoint").SetValue(buttons[__instance][5]);
                            }
                        }
                    }
                    else if (__instance.MyScreenHubBase.OptionalShipInfo.MyStats.GetShipComponent<PLSensorDish>(ESlotType.E_COMP_SENSORDISH) != null && !SensorDish.screenInfos.ContainsKey(__instance))
                    {
                        if (__instance.MyScreenHubBase.OptionalShipInfo.MyStats.GetShipComponent<PLSensorDish>(ESlotType.E_COMP_SENSORDISH).SubType != 0)
                        {
                            SensorDish.screenInfos.Add(__instance, __instance.MyScreenHubBase.OptionalShipInfo.MyStats.GetShipComponent<PLSensorDish>(ESlotType.E_COMP_SENSORDISH).SubType);
                            if (!buttons.ContainsKey(__instance))
                            {
                                SensorDish.buttons.Add(__instance, new UITexture[6]
                                {
                                traverse.Field("weaknessScanCyberDefBG").GetValue<UITexture>(),
                                traverse.Field("weaknessScanShieldsWeakPointBG").GetValue<UITexture>(),
                                traverse.Field("weaknessScanReactorWeakPointBG").GetValue<UITexture>(),
                                traverse.Field("weaknessScanCyberDef").GetValue<UITexture>(),
                                traverse.Field("weaknessScanShieldsWeakPoint").GetValue<UITexture>(),
                                traverse.Field("weaknessScanReactorWeakPoint").GetValue<UITexture>()
                            });
                            }
                            if (!buttonsCustom.ContainsKey(__instance))
                            {
                                SensorDish.buttonsCustom.Add(__instance, SensorDish.createButtonsFromSubType(__instance, __instance.MyScreenHubBase.OptionalShipInfo.MyStats.GetShipComponent<PLSensorDish>(ESlotType.E_COMP_SENSORDISH).SubType));
                            }
                            traverse.Field("weaknessScanCyberDefBG").SetValue(buttonsCustom[__instance][0]);
                            traverse.Field("weaknessScanShieldsWeakPointBG").SetValue(buttonsCustom[__instance][1]);
                            traverse.Field("weaknessScanReactorWeakPointBG").SetValue(buttonsCustom[__instance][2]);
                            traverse.Field("weaknessScanCyberDef").SetValue(buttonsCustom[__instance][3]);
                            traverse.Field("weaknessScanShieldsWeakPoint").SetValue(buttonsCustom[__instance][4]);
                            traverse.Field("weaknessScanReactorWeakPoint").SetValue(buttonsCustom[__instance][5]);
                            foreach (UITexture texture1 in SensorDish.buttonsCustom[__instance])
                            {
                                PLGlobal.SafeGameObjectSetActive(texture1.gameObject, true);
                            }
                            foreach (UITexture texture in SensorDish.buttons[__instance])
                            {
                                PLGlobal.SafeGameObjectSetActive(texture.gameObject, false);
                            }
                        }
                    }
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(PLScientistSensorScreen), "OnButtonHover")]
        internal class updateTooltip
        {
            private static void Postfix(PLScientistSensorScreen __instance, UIWidget inButton)
            {
                if (SensorDish.screenInfos.ContainsKey(__instance))
                {
                    if (SensorDish.screenInfos[__instance] == 1)
                    {
                        switch (inButton.name)
                        {
                            case "weaknessScanCyberDef":
                                PLInGameUI.SetTooltipLargeText("Hostile Takeover", "Control target drone for 60 seconds\n<color=red>One use per jump</color>", false, 0.0f);
                                break;
                            case "weaknessScanReactor":
                                PLInGameUI.SetTooltipLargeText("Missile Lock", "Greatly improve missile lock-on speed and reload time against target ship for 60 seconds", false, 0.0f);
                                break;
                            case "weaknessScanShieldWeakPoint":
                                PLInGameUI.SetTooltipLargeText("Weaken Armor", "Nullify all armor and damage reduction gained from hull plating on target ship for 60 seconds", false, 0.0f);
                                break;
                        }
                    }
                }
            }
        }

        internal class hostileTakeoverRPC : ModMessage
        {
            public override void HandleRPC(object[] arguments, PhotonMessageInfo sender)
            {
                PLShipInfo playerShip = (PLShipInfo)PLEncounterManager.Instance.GetShipFromID((int)arguments[0]);
                PLShipInfoBase target = (PLShipInfoBase)PLEncounterManager.Instance.GetShipFromID((int)arguments[1]);

                playerShip.MySensorDish.SubTypeData = 1;
                Puppet.shipDatas[target.ShipID] = playerShip.ShipID;
                target.IsFlagged = true;
                target.TargetShip = playerShip.TargetShip;
                SensorDish.removePuppet(target);
            }
        }

        private static void hostileTakeover(PLPlayer player, PLShipInfoBase target)
        {
            if (player == null)
                return;
            if (!player.IsBot)
            {
                if ((UnityEngine.Object)target.PilotingSystem == (UnityEngine.Object)null)
                {
                    target.PilotingSystem = target.gameObject.AddComponent<PLPilotingSystem>();
                    target.PilotingSystem.MyShipInfo = target;
                }
                if ((UnityEngine.Object)target.PilotingHUD == (UnityEngine.Object)null)
                {
                    target.PilotingHUD = target.gameObject.AddComponent<PLPilotingHUD>();
                    target.PilotingHUD.MyShipInfo = target;
                }
                target.OrbitCameraMaxDistance = 40f;
                target.OrbitCameraMinDistance = 7f;
                if (player.GetPhotonPlayer().IsMasterClient)
                {
                    target.photonView.RPC("NewShipController", PhotonTargets.All, (object)player.GetPlayerID());
                }
                else
                {
                    ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.PilotDrone", player.GetPhotonPlayer(), new object[1]
                            {
                                target.ShipID
                            });
                }
                ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.hostileTakeoverRPC", PhotonTargets.All, new object[2]
                {
                    player.GetPawn().CurrentShip.ShipID,
                    target.ShipID
                });

            }
        }

        private static async void removePuppet(PLShipInfoBase ship)
        {
            await Task.Delay(60000);
            if (ship == null)
                return;
            if (!Puppet.shipDatas.ContainsKey(ship.ShipID))
                return;
            ship.TargetShip = PLEncounterManager.Instance.GetShipFromID(Puppet.shipDatas[ship.ShipID]);
            Puppet.shipDatas.Remove(ship.ShipID);
            if (PhotonNetwork.isMasterClient)
                ship.photonView.RPC("NewShipController", PhotonTargets.All, (object)-1);

        }

        public static bool IsSensorWeaknessActiveReal(PLShipInfoBase shipInfo, int weaknessID, int subTypeID = -1)
        {
            Traverse traverse = Traverse.Create(shipInfo);
            int[] SensorWeaknessActiveStartTime = traverse.Field("SensorWeaknessActiveStartTime").GetValue<int[]>();
            int[] SensorWeaknessData = traverse.Field("SensorWeaknessData").GetValue<int[]>();
            if (SensorWeaknessActiveStartTime == null)
                return false;
            if (SensorWeaknessData == null)
                return false;
            return weaknessID >= 0 && SensorWeaknessActiveStartTime.Length > weaknessID && SensorWeaknessData[weaknessID] == subTypeID && PLGlobal.WithinTimeLimit(PLServer.Instance.GetEstimatedServerMs(), SensorWeaknessActiveStartTime[weaknessID], PLShipInfoBase.GetSensorWeaknessTimeInSeconds((ESensorWeakness)weaknessID) * 1000);
        }

        [HarmonyPatch(typeof(PLShipInfoBase), "AddSensorWeakness")]
        internal class customSensorAbilities
        {
            private static void Postfix(PLShipInfoBase __instance, int weaknessID, int startTime, int data, ref int[] ___SensorWeaknessActiveStartTime, ref int[] ___SensorWeaknessData)
            {
                if (SensorDish.lastToInteract == null || SensorDish.lastToInteract.GetPawn() == null || weaknessID == 0)
                    return;
                if (SensorDish.lastToInteract.GetPawn().CurrentShip.MySensorDish == null || SensorDish.lastToInteract.GetPawn().CurrentShip.MySensorDish.SubType == 0)
                    return;
                if (weaknessID < 0 || ___SensorWeaknessActiveStartTime.Length <= weaknessID || ___SensorWeaknessData.Length <= weaknessID)
                    return;

                if (SensorDish.lastToInteract.GetPawn().CurrentShip.MySensorDish.SubType == 1)
                {
                    switch (weaknessID)
                    {
                        case 3:
                            if (SensorDish.lastToInteract.StartingShip.MySensorDish.SubTypeData == 0)
                            {
                                bool flag = false;
                                bool flag2 = false;
                                switch (__instance.ShipTypeID)
                                {
                                    case EShipType.E_WDDRONE1:
                                    case EShipType.E_WDDRONE2:
                                    case EShipType.E_WDDRONE3:
                                    case EShipType.E_DEATHSEEKER_DRONE:
                                    case EShipType.E_SHOCK_DRONE:
                                    case EShipType.E_PHASE_DRONE:
                                    case EShipType.E_UNSEEN_FIGHTER:
                                        flag = true; break;
                                    case EShipType.E_GUARDIAN:
                                    case EShipType.E_CORRUPTED_DRONE:
                                    case EShipType.E_DEATHSEEKER_DRONE_SC:
                                    case EShipType.E_SWARM_KEEPER:
                                    case EShipType.E_REPAIR_DRONE:
                                    case EShipType.E_PTDRONE:
                                        flag2 = true; break;
                                }
                                if (!flag)
                                {
                                    if (PhotonNetwork.isMasterClient)
                                    {
                                        if (!flag2)
                                        {
                                            ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.SendWarning", SensorDish.lastToInteract.GetPhotonPlayer(), new object[1]
                                            {
                                                    (object) "Target must be a drone!"
                                            });
                                        }
                                        else
                                        {
                                            ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.SendWarning", SensorDish.lastToInteract.GetPhotonPlayer(), new object[1]
                                            {
                                                    (object) "Target too strong!"
                                            });
                                        }
                                    }
                                    for (int index = 0; index < ___SensorWeaknessActiveStartTime.Length; ++index)
                                        ___SensorWeaknessActiveStartTime[index] = int.MinValue;
                                    return;
                                }
                                SensorDish.hostileTakeover(lastToInteract, __instance);

                            }
                            else
                            {
                                if (PhotonNetwork.isMasterClient)
                                {
                                    ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.SendWarning", SensorDish.lastToInteract.GetPhotonPlayer(), new object[1]
                                        {
                                            (object) "Only useable once per jump!"
                                        });
                                }
                                for (int index = 0; index < ___SensorWeaknessActiveStartTime.Length; ++index)
                                    ___SensorWeaknessActiveStartTime[index] = int.MinValue;
                                return;
                            }

                            break;
                    }
                }
                ___SensorWeaknessData[weaknessID] = SensorDish.lastToInteract.GetPawn().CurrentShip.MySensorDish.SubType;
            }
        }

        [HarmonyPatch(typeof(PLShipInfoBase), "TakeDamage")]
        internal class SensorDishShieldWeaknessCheck
        {
            private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                List<CodeInstruction> list = instructions.ToList();

                List<CodeInstruction> targetSequence = new List<CodeInstruction>() {
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Ldc_I4_2),
                    CodeInstruction.Call(typeof(PLShipInfoBase), "IsSensorWeaknessActive", new Type[1]
                    {
                        typeof(ESensorWeakness)
                    }),
                };
                List<CodeInstruction> patchSequence = new List<CodeInstruction>()
                {
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Ldc_I4_2),
                    new CodeInstruction(OpCodes.Ldc_I4, -1),
                    CodeInstruction.Call(typeof(ExpandedGalaxy.SensorDish), "IsSensorWeaknessActiveReal", new Type[3] {
                        typeof(PLShipInfoBase),
                        typeof(int),
                        typeof(int)
                    }),
                };
                return HarmonyHelpers.PatchBySequence(list.AsEnumerable<CodeInstruction>(), targetSequence, patchSequence, HarmonyHelpers.PatchMode.REPLACE, HarmonyHelpers.CheckMode.NONNULL, false);
            }
        }

        [HarmonyPatch(typeof(PLShipStats), "CalculateStats")]
        internal class SensorDishCyberdefWeaknessCheck
        {
            private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                List<CodeInstruction> list = instructions.ToList();

                List<CodeInstruction> targetSequence = new List<CodeInstruction>() {
                    CodeInstruction.Call(typeof(PLShipStats), "get_Ship"),
                    new CodeInstruction(OpCodes.Ldc_I4_3),
                    new CodeInstruction(list[953]),
                };
                List<CodeInstruction> patchSequence = new List<CodeInstruction>()
                {
                    CodeInstruction.Call(typeof(PLShipStats), "get_Ship"),
                    new CodeInstruction(OpCodes.Ldc_I4_3),
                    new CodeInstruction(OpCodes.Ldc_I4, -1),
                    CodeInstruction.Call(typeof(ExpandedGalaxy.SensorDish), "IsSensorWeaknessActiveReal", new Type[3] {
                        typeof(PLShipInfoBase),
                        typeof(int),
                        typeof(int)
                    }),
                };
                return HarmonyHelpers.PatchBySequence(list.AsEnumerable<CodeInstruction>(), targetSequence, patchSequence, HarmonyHelpers.PatchMode.REPLACE, HarmonyHelpers.CheckMode.NONNULL, false);
            }
        }

        [HarmonyPatch(typeof(PLShipStats), "CalculateStats")]
        internal class SensorDishReactorWeaknessCheck
        {
            private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                List<CodeInstruction> list = instructions.ToList();

                List<CodeInstruction> targetSequence = new List<CodeInstruction>() {
                    CodeInstruction.Call(typeof(PLShipStats), "get_Ship"),
                    new CodeInstruction(OpCodes.Ldc_I4_5),
                    new CodeInstruction(list[965]),
                };
                List<CodeInstruction> patchSequence = new List<CodeInstruction>()
                {
                    CodeInstruction.Call(typeof(PLShipStats), "get_Ship"),
                    new CodeInstruction(OpCodes.Ldc_I4_5),
                    new CodeInstruction(OpCodes.Ldc_I4, -1),
                    CodeInstruction.Call(typeof(ExpandedGalaxy.SensorDish), "IsSensorWeaknessActiveReal", new Type[3] {
                        typeof(PLShipInfoBase),
                        typeof(int),
                        typeof(int)
                    }),
                };
                return HarmonyHelpers.PatchBySequence(list.AsEnumerable<CodeInstruction>(), targetSequence, patchSequence, HarmonyHelpers.PatchMode.REPLACE, HarmonyHelpers.CheckMode.NONNULL, false);
            }
        }

        [HarmonyPatch(typeof(PLShipStats), "CalculateStats")]
        internal class AncientSensorDishRemoveArmor
        {
            private static void Postfix(PLShipStats __instance)
            {
                if (__instance.Ship != null)
                {
                    if (IsSensorWeaknessActiveReal(__instance.Ship, 2, 1))
                    {
                        __instance.HullArmor = 0f;
                    }
                }
            }
        }

        public static PLShipInfoBase GetTurretMissileTarget(PLTurret turret)
        {
            return turret.TargetedMissileLockShip;
        }

        [HarmonyPatch(typeof(PLTurret), "Tick")]
        internal class AncientSensorDishMissileLock
        {
            private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
            {
                Label failed = generator.DefineLabel();
                Label succeed = generator.DefineLabel();
                List<CodeInstruction> list = instructions.ToList();

                List<CodeInstruction> targetSequence = new List<CodeInstruction>() {
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Ldarg_0),
                    CodeInstruction.LoadField(typeof(PLTurret), "LockedOnAmount"),
                    CodeInstruction.Call(typeof(UnityEngine.Time), "get_deltaTime"),
                };
                List<CodeInstruction> patchSequence = new List<CodeInstruction>()
                {
                    new CodeInstruction(OpCodes.Ldarg_0),
                    CodeInstruction.Call(typeof(ExpandedGalaxy.SensorDish), "GetTurretMissileTarget", new Type[1]
                    {
                        typeof(PLTurret)
                    }),
                    new CodeInstruction(OpCodes.Ldc_I4_5),
                    new CodeInstruction(OpCodes.Ldc_I4_1),
                    CodeInstruction.Call(typeof(ExpandedGalaxy.SensorDish), "IsSensorWeaknessActiveReal", new Type[3] {
                        typeof(PLShipInfoBase),
                        typeof(int),
                        typeof(int)
                    }),
                    new CodeInstruction(OpCodes.Brfalse_S, failed),
                    new CodeInstruction(OpCodes.Ldc_R4, 1.35f),
                    new CodeInstruction(OpCodes.Br_S, succeed)
                };
                list[1406].labels.Add(failed);
                list[1407].labels.Add(succeed);

                return HarmonyHelpers.PatchBySequence(list.AsEnumerable<CodeInstruction>(), targetSequence, patchSequence, HarmonyHelpers.PatchMode.AFTER, HarmonyHelpers.CheckMode.NONNULL, false);
            }
        }

        [HarmonyPatch(typeof(PLTurret), "ServerFireMissile")]
        internal class AncientSensorDishMissileRebateP1
        {
            private static void Postfix(PLTurret __instance, PLTrackerMissile inMissile, int inTargetShipID)
            {
                if (!((UnityEngine.Object)__instance.TurretInstance != (UnityEngine.Object)null) || inMissile.SubTypeData <= (short)0)
                    return;
                PLShipInfoBase pLShipInfoBase = PLEncounterManager.Instance.GetShipFromID(inTargetShipID);
                if (pLShipInfoBase == null)
                    return;
                if (IsSensorWeaknessActiveReal(pLShipInfoBase, 5, 1))
                {
                    __instance.LastFireMissileTime = Time.time - (__instance.TrackerMissileReloadTime * 100000f);
                    turretMissileTick.Add(__instance.NetID);
                }
            }
        }

        [HarmonyPatch(typeof(PLTurret), "Tick")]
        internal class AncientSensorDishMissileRebateP2
        {
            private static void Postfix(PLTurret __instance)
            {
                if (turretMissileTick.Contains(__instance.NetID))
                {
                    __instance.LastFireMissileTime = Time.time - (__instance.TrackerMissileReloadTime * 0.5f);
                    turretMissileTick.Remove(__instance.NetID);
                }
            }
        }
    }
}
