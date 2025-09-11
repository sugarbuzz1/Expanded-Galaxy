using CodeStage.AntiCheat.ObscuredTypes;
using HarmonyLib;
using PulsarModLoader;
using PulsarModLoader.Content.Components.MissionShipComponent;
using PulsarModLoader.Patches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using UnityEngine;

namespace ExpandedGalaxy
{
    internal class Ammunition
    {
        internal static bool DynamicAmmunition = true;

        [HarmonyPatch(typeof(PLPawnItem_BurstPistol), MethodType.Constructor)]
        internal class BurstRifleAmmo
        {
            private static void Postfix(PLPawnItem_BurstPistol __instance)
            {
                if (!DynamicAmmunition)
                    return;
                __instance.AmmoMax = 60;
                __instance.AmmoCurrent = __instance.AmmoMax;
            }
        }

        [HarmonyPatch(typeof(PLPawnItem_HandShotgun), MethodType.Constructor)]
        internal class SplitshotAmmo
        {
            private static void Postfix(PLPawnItem_HandShotgun __instance)
            {
                if (!DynamicAmmunition)
                    return;
                __instance.AmmoMax = 12;
                __instance.AmmoCurrent = __instance.AmmoMax;
            }
        }

        [HarmonyPatch(typeof(PLPawnItem_HeavyPistol), MethodType.Constructor)]
        internal class HeavyPistolAmmo
        {
            private static void Postfix(PLPawnItem_HeavyPistol __instance)
            {
                if (!DynamicAmmunition)
                    return;
                __instance.AmmoMax = 20;
                __instance.AmmoCurrent = __instance.AmmoMax;
            }
        }

        [HarmonyPatch(typeof(PLPawnItem_HeldBeamPistol_WithHealing), MethodType.Constructor)]
        internal class HealingBeamRifleAmmo
        {
            private static void Postfix(PLPawnItem_HeldBeamPistol_WithHealing __instance)
            {
                if (!DynamicAmmunition)
                    return;
                __instance.AmmoMax = 100;
                __instance.AmmoCurrent = __instance.AmmoMax;
            }
        }

        [HarmonyPatch(typeof(PLPawnItem_PhasePistol), MethodType.Constructor)]
        internal class PhasePistolAmmo
        {
            private static void Postfix(PLPawnItem_PhasePistol __instance)
            {
                if (!DynamicAmmunition)
                    return;
                __instance.UsesAmmo = true;
                __instance.AmmoMax = 30;
                __instance.AmmoCurrent = __instance.AmmoMax;
            }
        }

        [HarmonyPatch(typeof(PLPawnItem_PierceLaserPistol), MethodType.Constructor)]
        internal class BeamPistolAmmo
        {
            private static void Postfix(PLPawnItem_PierceLaserPistol __instance)
            {
                if (!DynamicAmmunition)
                    return;
                __instance.AmmoMax = 30;
                __instance.AmmoCurrent = __instance.AmmoMax;
            }
        }

        [HarmonyPatch(typeof(PLPawnItem_SmugglersPistol), MethodType.Constructor)]
        internal class SmugglerPistolAmmo
        {
            private static void Postfix(PLPawnItem_SmugglersPistol __instance)
            {
                if (!DynamicAmmunition)
                    return;
                __instance.UsesAmmo = true;
                __instance.AmmoMax = 48;
                __instance.AmmoCurrent = __instance.AmmoMax;
            }
        }

        [HarmonyPatch(typeof(PLPawnItem_SmugglersPistol), "CalcDamageDone")]
        internal class SmugglerPistolDamage
        {
            private static bool Prefix(PLPawnItem_SmugglersPistol __instance, ref float __result)
            {
                if (!DynamicAmmunition)
                    return true;
                __result = (float)(12.0 + 5.0 * __instance.Level);
                return false;
            }
        }

        [HarmonyPatch(typeof(PLShipInfo), "OnProgramsCharged")]
        internal class NoAmmoRefill
        {
            private static bool Prefix() { return !DynamicAmmunition; }
        }

        internal class AmmunitionCrate : MissionShipComponentMod
        {
            public override string Name => "Ammunition Cache";

            public override string Description => "A cache filled with various types of weapon ammo. Used to restock ship ammo refills.";

            public override int MarketPrice => 2500;

            public override int CargoVisualID => 49;

            public override float Price_LevelMultiplierExponent => 1.2f;
        }

        [HarmonyPatch(typeof(PLMissionShipComponent), "GetShortDesc")]
        internal class AmmunitionShortDesc
        {
            private static void Postfix(PLMissionShipComponent __instance, ref string __result)
            {
                if (__instance.SubType == MissionShipComponentModManager.Instance.GetMissionShipComponentIDFromName("Ammunition Cache"))
                    __result = "Ammunition".ToUpper();
            }
        }

        [HarmonyPatch(typeof(PLShop), "Update")]
        internal class AmmunitionCrateInShops
        {
            private static void Postfix(PLShop __instance)
            {
                if (!DynamicAmmunition || !PhotonNetwork.isMasterClient)
                    return;
                if (__instance.SpaceTrader && __instance.MyPDE != null && __instance is PLShop_General)
                {
                    bool flag = false;
                    foreach (PLWare ware in __instance.MyPDE.Wares.Values)
                    {
                        if (ware is PLShipComponent)
                        {
                            if (ware is PLMissionShipComponent && (ware as PLMissionShipComponent).SubType == MissionShipComponentModManager.Instance.GetMissionShipComponentIDFromName("Ammunition Cache"))
                            {
                                flag = true;
                                break;
                            }
                        }
                    }
                    if (!flag)
                    {
                        PLMissionShipComponent component = MissionShipComponentModManager.CreateMissionShipComponent(MissionShipComponentModManager.Instance.GetMissionShipComponentIDFromName("Ammunition Cache"), UnityEngine.Random.Range(0, 3));
                        __instance.MyPDE.ServerAddWare(component);
                    }
                }
            }
        }

        [HarmonyPatch(typeof(PLPlayer), "AttemptToProcessScrapCargo")]
        internal class AmmoRefill
        {
            private static bool Prefix(PLPlayer __instance, int inCurrentShipID, int inNetID)
            {
                if (!(PLEncounterManager.Instance != null))
                    return false;
                PLShipInfo shipFromId = PLEncounterManager.Instance.GetShipFromID(inCurrentShipID) as PLShipInfo;
                if (!(shipFromId != null))
                    return false;
                PLShipComponent component = shipFromId.MyStats.GetComponentFromNetID(inNetID);
                if (component == null)
                    return false;
                else if (component is PLMissionShipComponent && component.SubType == MissionShipComponentModManager.Instance.GetMissionShipComponentIDFromName("Ammunition Cache"))
                {
                    shipFromId.MyStats.RemoveShipComponentByNetID(inNetID);
                    for (int i = 0; i < shipFromId.MyAmmoRefills.Length; i++)
                    {
                        shipFromId.MyAmmoRefills[i].SupplyAmount = 1f;
                        shipFromId.MyAmmoRefills[i].ClipAvailable = true;
                    }
                    PLPlayer friendlyPlayerOfClass = PLServer.Instance.GetCachedFriendlyPlayerOfClass(0);
                    if ((int)__instance.TeamID != 0 || !(friendlyPlayerOfClass != null) || !(__instance != friendlyPlayerOfClass))
                        return false;
                    PLServer.Instance.photonView.RPC("AddNotificationLocalize", friendlyPlayerOfClass.GetPhotonPlayer(), (object)"[PL] has restocked ammunition", (object)__instance.GetPlayerID(), (object)(PLServer.Instance.GetEstimatedServerMs() + 6000), true);
                    return false;
                }
                else if (component is PLMissionShipComponent && component.SubType == MissionShipComponentModManager.Instance.GetMissionShipComponentIDFromName("Reward"))
                {
                    shipFromId.MyStats.RemoveShipComponentByNetID(inNetID);
                    if (PhotonNetwork.isMasterClient)
                        shipFromId.MyStats.AddShipComponent(Relic.GenerateRelic(PLGlobal.Instance.Galaxy.Seed), visualSlot: ESlotType.E_COMP_CARGO);
                    return false;
                }
                else if (component is PLWarpDriveProgram)
                {
                    PLServer.Instance.CurrentUpgradeMats++;
                    shipFromId.MyStats.RemoveShipComponentByNetID(inNetID);
                    return false;

                }
                else if (component.ActualSlotType != ESlotType.E_COMP_SCRAP)
                {
                    PLServer.Instance.CurrentCrewCredits -= (component.Level + 1) * 800;
                    if (PLServer.Instance.CurrentCrewCredits < 0)
                        PLServer.Instance.CurrentCrewCredits = 0;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(PLSpawner), "DoSpawnStatic")]
        internal class ExGalSpawn
        {
            private static void Postfix(PLPersistantEncounterInstance pei, string spawnType, Transform childTransform, PLSpawner spawner, PLTeleportationLocationInstance optionalTLI, PLInterior optionalInterior, PLCombatTarget combatTargetWhoSpawnedMe)
            {
                if (pei == null)
                    return;
                foreach (GameObject allPlayerObject in pei.MyCreatedPlayers)
                {
                    if (allPlayerObject == null)
                        continue;
                    PLPlayer allPlayer = allPlayerObject.GetComponent<PLPlayer>();
                    if (allPlayer != null && !allPlayer.gameObject.name.Contains("ExGal"))
                    {
                        allPlayer.Talents[(int)ETalents.WPN_AMMO_BOOST] = (ObscuredInt)20;
                        allPlayer.gameObject.name += " ExGal";

                        if (pei is PLLavaPlanet2Encounter && !allPlayer.gameObject.name.Contains("Lava2"))
                        {
                            int netID = PLServer.Instance.PawnInvItemIDCounter++;
                            int num = UnityEngine.Random.Range(0, 2);
                            allPlayer.MyInventory.Clear();
                            if (num == 0)
                                allPlayer.MyInventory.UpdateItem(netID, 9, 0, 4, 1);
                            else
                                allPlayer.MyInventory.UpdateItem(netID, 12, 0, 4, 1);
                            allPlayer.Talents[(int)ETalents.HEALTH_BOOST] = (ObscuredInt)5;
                            allPlayer.Talents[(int)ETalents.ARMOR_BOOST] = (ObscuredInt)7;
                            allPlayer.Talents[(int)ETalents.PISTOL_DMG_BOOST] = (ObscuredInt)3;
                            allPlayer.gameObject.name += " Lava2";
                            allPlayer.SetPlayerName("Guard");
                        }
                    }                    
                }
            }
        }

        internal static void UpdateItemNoAmmo(PLPawnInventoryBase inventory, int inNetID, int inType, int inSubType, int inLevel, int inEquipID)
        {
            if (!DynamicAmmunition)
            {
                inventory.UpdateItem(inNetID, inType, inSubType, inLevel, inEquipID);
                return;
            }
            PLPawnItem itemAtNetId = inventory.GetItemAtNetID(inNetID);
            if (itemAtNetId != null)
            {
                itemAtNetId.EquipID = inEquipID;
                itemAtNetId.Level = inLevel;
                itemAtNetId.SubType = inSubType;
                itemAtNetId.PawnItemType = (EPawnItemType)inType;
                if (itemAtNetId.UsesAmmo && Ammunition.DynamicAmmunition)
                    itemAtNetId.AmmoCurrent = 0;
            }
            else
            {
                PLPawnItem fromInfo = PLPawnItem.CreateFromInfo((EPawnItemType)inType, inSubType, inLevel);
                if (fromInfo != null)
                {
                    Traverse traverse = Traverse.Create(inventory);
                    fromInfo.NetID = inNetID;
                    fromInfo.EquipID = inEquipID;
                    if (fromInfo.UsesAmmo && Ammunition.DynamicAmmunition)
                        fromInfo.AmmoCurrent = 0;
                    traverse.Method("AddItem_Internal", new Type[2]
                    {
                        typeof(int),
                        typeof(PLPawnItem)
                    }).GetValue(new object[2]
                    {
                        inNetID,
                        fromInfo
                    });
                }
            }
            if ((bool)PLNetworkManager.Instance.IsInternalBuild)
                Debug.Log((object)("UpdateItem:    player: " + (inventory.PlayerOwner != null ? inventory.PlayerOwner.GetPlayerName() : "null") + "    equipID: " + inEquipID.ToString()));
            if (!(PLTabMenu.Instance != null))
                return;
            PLTabMenu.Instance.ShouldRecreateLocalInventory = true;
        }

        [HarmonyPatch(typeof(PLPlayer), "AttemptToPickupDroppedPlayerPawnItem")]
        internal class UpdateItemNoAmooCall
        {
            private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                List<CodeInstruction> list = instructions.ToList();

                List<CodeInstruction> targetSequence = new List<CodeInstruction>() {
                    new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(PLPawnInventoryBase), "UpdateItem", new Type[5]
                    {
                        typeof(int),
                        typeof(int),
                        typeof(int),
                        typeof(int),
                        typeof(int)
                    }))
                };
                List<CodeInstruction> patchSequence = new List<CodeInstruction>()
                {
                    CodeInstruction.Call(typeof(ExpandedGalaxy.Ammunition), "UpdateItemNoAmmo", new Type[6]
                    {
                        typeof(PLPawnInventoryBase),
                        typeof(int),
                        typeof(int),
                        typeof(int),
                        typeof(int),
                        typeof(int)
                    }),
                };
                IEnumerable<CodeInstruction> list2 = HarmonyHelpers.PatchBySequence(list.AsEnumerable<CodeInstruction>(), targetSequence, patchSequence, HarmonyHelpers.PatchMode.REPLACE, HarmonyHelpers.CheckMode.NONNULL, false);
                return HarmonyHelpers.PatchBySequence(list2, targetSequence, patchSequence, HarmonyHelpers.PatchMode.REPLACE, HarmonyHelpers.CheckMode.NONNULL, false);
            }
        }

        [HarmonyPatch(typeof(PLPawnInventoryBase), "UpdateItem")]
        internal class UpdateItemNoAmmoClient
        {
            private static void Postfix(PLPawnInventoryBase __instance, int inNetID, int inType, int inSubType, int inLevel, int inEquipID)
            {
                if (PhotonNetwork.isMasterClient)
                    return;
                PLPawnItem itemAtNetId = __instance.GetItemAtNetID(inNetID);
                if (itemAtNetId != null && itemAtNetId.UsesAmmo)
                {
                    ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.SyncAmmoServer", PhotonTargets.MasterClient, new object[2] { __instance.InventoryID, itemAtNetId.NetID });
                }
            }
        }

        internal class SyncAmmoClient : ModMessage
        {
            public override void HandleRPC(object[] arguments, PhotonMessageInfo sender)
            {
                PLPawnInventoryBase inventory = PLNetworkManager.Instance.GetInvAtID((int)arguments[0]);
                if (inventory == null)
                    return;
                PLPawnItem item = inventory.GetItemAtNetID((int)arguments[1]);
                if (item == null)
                    return;
                item.AmmoCurrent = (int)arguments[2];
            }
        }

        internal class SyncAmmoServer : ModMessage
        {
            public override void HandleRPC(object[] arguments, PhotonMessageInfo sender)
            {
                PLPawnInventoryBase inventory = PLNetworkManager.Instance.GetInvAtID((int)arguments[0]);
                if (inventory == null)
                    return;
                PLPawnItem item = inventory.GetItemAtNetID((int)arguments[1]);
                if (item == null)
                    return;
                ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.SyncAmmoClient", PhotonTargets.Others, new object[3] { arguments[0], arguments[1], item.AmmoCurrent });
            }
        }

        internal class ClientSendAmmoToServer : ModMessage
        {
            public override void HandleRPC(object[] arguments, PhotonMessageInfo sender)
            {
                int[] itemNetIDs = (int[])arguments[1];
                int[] itemAmmoAmmounts = (int[])arguments[2];

                PLPawnInventoryBase inventory = PLNetworkManager.Instance.GetInvAtID((int)arguments[0]);
                if (inventory == null)
                    return;

                for (int i = 0; i < itemNetIDs.Length; i++)
                {
                    if (inventory.GetItemAtNetID(itemNetIDs[i]) != null)
                        inventory.GetItemAtNetID(itemNetIDs[i]).AmmoCurrent = itemAmmoAmmounts[i];
                }
            }
        }

        [HarmonyPatch(typeof(PLAmmoRefill), "Update")]
        internal class ShipAmmoRefillPatch
        {
            private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                List<CodeInstruction> list = instructions.ToList();

                List<CodeInstruction> targetSequence = new List<CodeInstruction>() {
                    new CodeInstruction(OpCodes.Ldc_R4, 0.1f)
                };
                List<CodeInstruction> patchSequence = new List<CodeInstruction>()
                {
                    CodeInstruction.Call(typeof(ExpandedGalaxy.Ammunition), "AmmoRefillPercent"),
                };
                return HarmonyHelpers.PatchBySequence(list.AsEnumerable<CodeInstruction>(), targetSequence, patchSequence, HarmonyHelpers.PatchMode.REPLACE, HarmonyHelpers.CheckMode.ALWAYS, false);
            }

            private static bool Prefix(PLAmmoRefill __instance, out bool __state)
            {
                __state = false;
                if (PhotonNetwork.isMasterClient)
                    return true;
                if ((double)Time.time - (double)Traverse.Create(__instance).Field("LastAmmoCheckTime").GetValue<float>() > 2.0 && PLServer.Instance != null)
                    __state = true;
                return true;
            }

            private static void Postfix(PLAmmoRefill __instance, bool __state)
            {
                if (__state)
                {
                    List<int> itemNetIDs = new List<int>();
                    List<int> itemAmmoAmounts = new List<int>();
                    foreach (PLPawnItem item in PLNetworkManager.Instance.LocalPlayer.MyInventory.AllItems)
                    {
                        if (item.UsesAmmo)
                        {
                            itemNetIDs.Add(item.NetID);
                            itemAmmoAmounts.Add(item.AmmoCurrent);
                        }
                    }
                    if (itemNetIDs.Count > 0)
                        ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.ClientSendAmmoToServer", PhotonTargets.MasterClient, new object[3] { PLNetworkManager.Instance.LocalPlayer.MyInventory.InventoryID, itemNetIDs.ToArray(), itemAmmoAmounts.ToArray() });
                }
            }
        }

        [HarmonyPatch(typeof(PLAmmoRefill_Arena), "Update")]
        internal class ShipAmmoRefillPatchArena
        {
            private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                List<CodeInstruction> list = instructions.ToList();

                List<CodeInstruction> targetSequence = new List<CodeInstruction>() {
                    new CodeInstruction(OpCodes.Ldc_R4, 0.1f)
                };
                List<CodeInstruction> patchSequence = new List<CodeInstruction>()
                {
                    CodeInstruction.Call(typeof(ExpandedGalaxy.Ammunition), "AmmoRefillPercent"),
                };
                return HarmonyHelpers.PatchBySequence(list.AsEnumerable<CodeInstruction>(), targetSequence, patchSequence, HarmonyHelpers.PatchMode.REPLACE, HarmonyHelpers.CheckMode.ALWAYS, false);
            }
        }

        [HarmonyPatch(typeof(PLPawnInventoryBase), "RemoveItem")]
        internal class ReloadUpdate
        {
            private static bool Prefix(PLPawnInventoryBase __instance, PLPawnItem inItem)
            {
                if (!(__instance is PLPawnInventory))
                    return true;
                if (inItem is PLPawnItem_AmmoClip)
                {
                    PLPawnInventory inventory = __instance as PLPawnInventory;
                    if (inventory.MyPlayer != PLNetworkManager.Instance.LocalPlayer)
                        return true;
                    if (inventory.ActiveItem != null)
                    {
                        ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.ClientSendAmmoToServer", PhotonTargets.MasterClient, new object[3] { inventory.InventoryID, new int[1] {inItem.NetID}, new int[1] { inventory.ActiveItem.AmmoCurrent } });
                    }
                }
                return true;
            }
        }

        internal static float AmmoRefillPercent()
        {
            if (Ammunition.DynamicAmmunition)
                return 0.05f;
            return 0.1f;
        }

        internal static bool ShouldRefillPlayer(PLAmmoRefill ammoRefill, PLPlayer player)
        {
            if (ammoRefill.MyTLI == null || ammoRefill.MyTLI.MyShipInfo == null)
                return true;
            if (ammoRefill.MyTLI.MyShipInfo.TeamID == -1)
                return false;
            if (player.StartingShip != null && player.MyCurrentTLI != null && player.MyCurrentTLI == ammoRefill.MyTLI)
            {
                if (player.StartingShip.ShipID == ammoRefill.MyTLI.MyShipInfo.ShipID)
                    return true;
            }
            return false;
        }
    }
}
