using HarmonyLib;
using PulsarModLoader.Patches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLPersistantEncounterInstance), "MusicUpdate")]
    internal class BurrowMusic
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            Label failed = generator.DefineLabel();

            List<CodeInstruction> list = instructions.ToList();

            List<CodeInstruction> targetSequence = new List<CodeInstruction>() {
                    new CodeInstruction(OpCodes.Call),
                    new CodeInstruction(OpCodes.Stloc_1),
                    new CodeInstruction(OpCodes.Ldloc_1),
                    new CodeInstruction(OpCodes.Brfalse)
                };
            List<CodeInstruction> patchSequence = new List<CodeInstruction>()
                {
                    new CodeInstruction(OpCodes.Ldloc_1),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(BurrowMusic), "HandleBurrowMusic", new Type[1] {typeof(PLSectorInfo)})),
                    new CodeInstruction(OpCodes.Brfalse, failed),
                    new CodeInstruction(OpCodes.Ret),
                    new CodeInstruction(OpCodes.Nop)
                };
            patchSequence[4].labels.Add(failed);

            return HarmonyHelpers.PatchBySequence(list.AsEnumerable<CodeInstruction>(), targetSequence, patchSequence, HarmonyHelpers.PatchMode.AFTER, HarmonyHelpers.CheckMode.NONNULL, false);
        }

        public static bool HandleBurrowMusic(PLSectorInfo currentSector)
        {
            if (PLNetworkManager.Instance.LocalPlayer != null && PLNetworkManager.Instance.LocalPlayer.GetPawn() != null && currentSector != null)
            {
                if (currentSector.VisualIndication == ESectorVisualIndication.DESERT_HUB && !PLNetworkManager.Instance.LocalPlayer.GetPawn().SpawnedInArena)
                {
                    if (PLNetworkManager.Instance.LocalPlayer.CurrentInterior == null)
                    {
                        if (PLMusic.Instance.CurrentPlayingMusicEventString != "mx_AllGent_AmbientLP")
                            PLMusic.Instance.PlayMusic("mx_AllGent_AmbientLP", false, false, false);
                    }
                    else
                        PLMusic.Instance.StopCurrentMusic();
                    return true;
                }
            }
            return false;
        }
    }
}

