using HarmonyLib;
using PulsarModLoader;
using PulsarModLoader.Patches;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using UnityEngine;

namespace ExpandedGalaxy
{
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
            if ((double)Time.time - (double)__instance.LastAmmoCheckTime > 2.0 && PLServer.Instance != null)
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
}
