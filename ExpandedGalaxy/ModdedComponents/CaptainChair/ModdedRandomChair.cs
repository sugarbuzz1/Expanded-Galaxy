using HarmonyLib;
using PulsarModLoader.Content.Components.CaptainsChair;
using System.Collections.Generic;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLCaptainsChair), "CreateRandom")]
    internal class ModdedRandomChair
    {
        private static bool Prefix(float inRarity, int inSeed, ref PLShipComponent __result)
        {
            List<int> subTypes = new List<int>();
            subTypes.Add(0);
            subTypes.Add(1);
            subTypes.Add(2);
            subTypes.Add(CaptainsChairModManager.Instance.GetCaptainsChairIDFromName("W.D. Modern Captain's Chair"));
            __result = PLCaptainsChair.CreateCaptainsChairFromHash(subTypes[new PLRand(inSeed).Next() % subTypes.Count], 0, 0);
            return false;
        }
    }
}
