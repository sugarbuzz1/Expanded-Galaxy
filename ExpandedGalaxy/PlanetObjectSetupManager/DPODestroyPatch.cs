using HarmonyLib;
using PulsarModLoader;
using PulsarModLoader.Patches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLDamageablePlanetObject), "Update")]
    internal class DPODestroyPatch
    {
        internal static void HandleDPODestroy(PLDamageablePlanetObject pLDamageablePlanetObject)
        {
            if (pLDamageablePlanetObject.photonView != null)
            {
                if (PlanetObjectSetupManager.Instance.GetViewIDs().Contains(pLDamageablePlanetObject.photonView.viewID))
                {
                    if (PhotonNetwork.isMasterClient)
                        ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.DestroyObjectWithViewID", PhotonTargets.Others, new object[1] { pLDamageablePlanetObject.photonView.viewID });
                    PlanetObjectSetupManager.Instance.RemoveID(pLDamageablePlanetObject.photonView.viewID);
                    UnityEngine.Object.Destroy(pLDamageablePlanetObject.gameObject);
                }
                else
                    PhotonNetwork.Destroy(pLDamageablePlanetObject.gameObject);
            }
            else
                UnityEngine.Object.Destroy(pLDamageablePlanetObject.gameObject);
        }
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> list = instructions.ToList();

            List<CodeInstruction> targetSequence = new List<CodeInstruction>() {
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Call),
                    new CodeInstruction(OpCodes.Ldnull),
                    new CodeInstruction(OpCodes.Call),
                    new CodeInstruction(OpCodes.Brfalse_S),
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Call),
                    new CodeInstruction(OpCodes.Call),
                    new CodeInstruction(OpCodes.Ret),
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Call),
                    new CodeInstruction(OpCodes.Call),
                    new CodeInstruction(OpCodes.Ret),


                };
            List<CodeInstruction> patchSequence = new List<CodeInstruction>()
                {
                    new CodeInstruction(OpCodes.Ldarg_0),
                    CodeInstruction.Call(typeof(ExpandedGalaxy.DPODestroyPatch), "HandleDPODestroy", new Type[1] {
                        typeof(PLDamageablePlanetObject)
                    }),
                    new CodeInstruction(OpCodes.Ret)
                };

            return HarmonyHelpers.PatchBySequence(list.AsEnumerable<CodeInstruction>(), targetSequence, patchSequence, HarmonyHelpers.PatchMode.REPLACE, HarmonyHelpers.CheckMode.NONNULL, false);
        }
    }
}

