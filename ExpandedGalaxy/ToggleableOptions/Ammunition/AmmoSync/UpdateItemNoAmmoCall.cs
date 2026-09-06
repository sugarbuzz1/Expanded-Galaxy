using HarmonyLib;
using PulsarModLoader.Patches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using UnityEngine;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLPlayer), "AttemptToPickupDroppedPlayerPawnItem")]
    internal class UpdateItemNoAmmoCall
    {
        internal static void UpdateItemNoAmmo(PLPawnInventoryBase inventory, int inNetID, int inType, int inSubType, int inLevel, int inEquipID)
        {
            if (!Ammunition.DynamicAmmunition)
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
                    fromInfo.NetID = inNetID;
                    fromInfo.EquipID = inEquipID;
                    if (fromInfo.UsesAmmo && Ammunition.DynamicAmmunition)
                        fromInfo.AmmoCurrent = 0;
                    inventory.AddItem_Internal(inNetID, fromInfo);
                }
            }
            if ((bool)PLNetworkManager.Instance.IsInternalBuild)
                Debug.Log((object)("UpdateItem:    player: " + (inventory.PlayerOwner != null ? inventory.PlayerOwner.GetPlayerName() : "null") + "    equipID: " + inEquipID.ToString()));
            if (!(PLTabMenu.Instance != null))
                return;
            PLTabMenu.Instance.ShouldRecreateLocalInventory = true;
        }

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
                    CodeInstruction.Call(typeof(UpdateItemNoAmmoCall), "UpdateItemNoAmmo", new Type[6]
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
}
