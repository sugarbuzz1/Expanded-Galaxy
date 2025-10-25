using HarmonyLib;
using PulsarModLoader.Content.Components.CaptainsChair;
using PulsarModLoader.Content.Components.Extractor;
using PulsarModLoader.Content.Components.Hull;

namespace ExpandedGalaxy
{
    internal class ManualPatches
    {
        [HarmonyPatch(typeof(PLWare), "GetStatLineRight")]
        private class CaptainsChairRightDescFix
        {
            private static void Postfix(PLWare __instance, ref string __result)
            {
                PLCaptainsChair chair = __instance as PLCaptainsChair;
                if (chair == null)
                    return;
                int index = chair.SubType - CaptainsChairModManager.Instance.VanillaCaptainsChairMaxType;
                if (index <= -1 || index >= CaptainsChairModManager.Instance.CaptainsChairTypes.Count || chair.ShipStats == null)
                    return;
                __result = CaptainsChairModManager.Instance.CaptainsChairTypes[index].GetStatLineRight((PLShipComponent)chair);
            }
        }

        [HarmonyPatch(typeof(PLWare), "GetStatLineLeft")]
        private class CaptainsChairLeftDescFix
        {
            private static void Postfix(PLWare __instance, ref string __result)
            {
                PLCaptainsChair chair = __instance as PLCaptainsChair;
                if (chair == null)
                    return;
                int index = chair.SubType - CaptainsChairModManager.Instance.VanillaCaptainsChairMaxType;
                if (index <= -1 || index >= CaptainsChairModManager.Instance.CaptainsChairTypes.Count || chair.ShipStats == null)
                    return;
                __result = CaptainsChairModManager.Instance.CaptainsChairTypes[index].GetStatLineLeft((PLShipComponent)chair);
            }
        }

        [HarmonyPatch(typeof(PLShipComponent), "Tick")]
        private class CaptainsChairTickPatch
        {
            private static void Postfix(PLShipComponent __instance)
            {
                PLCaptainsChair chair = __instance as PLCaptainsChair;
                if (chair == null)
                    return;
                int index = chair.SubType - CaptainsChairModManager.Instance.VanillaCaptainsChairMaxType;
                if (index <= -1 || index >= CaptainsChairModManager.Instance.CaptainsChairTypes.Count || chair.ShipStats == null)
                    return;
                CaptainsChairModManager.Instance.CaptainsChairTypes[index].Tick((PLShipComponent)chair);
            }
        }

        [HarmonyPatch(typeof(PLShipComponent), "OnWarp")]
        private class CaptainsChairWarpPatch
        {
            private static void Postfix(PLShipComponent __instance)
            {
                PLCaptainsChair chair = __instance as PLCaptainsChair;
                if (chair == null)
                    return;
                int index = chair.SubType - CaptainsChairModManager.Instance.VanillaCaptainsChairMaxType;
                if (index <= -1 || index >= CaptainsChairModManager.Instance.CaptainsChairTypes.Count || chair.ShipStats == null)
                    return;
                CaptainsChairModManager.Instance.CaptainsChairTypes[index].OnWarp((PLShipComponent)chair);
            }
        }

        [HarmonyPatch(typeof(PLShipComponent), "FinalLateAddStats")]
        internal class HullFinalLateAddStats
        {
            private static void Postfix(PLShipComponent __instance)
            {
                if (!(__instance is PLHull))
                    return;
                int index = __instance.SubType - HullModManager.Instance.VanillaHullMaxType;
                if (index <= -1 || index >= HullModManager.Instance.HullTypes.Count || __instance.ShipStats == null)
                    return;
                HullModManager.Instance.HullTypes[index].FinalLateAddStats(__instance);
            }
        }

        [HarmonyPatch(typeof(PLShipComponent), "OnWarp")]
        private class ExtractorWarpPatch
        {
            private static void Postfix(PLShipComponent __instance)
            {
                PLExtractor extractor = __instance as PLExtractor;
                if (extractor == null)
                    return;
                if (extractor.SubType == ExtractorModManager.Instance.GetExtractorIDFromName("P.T. Extractor Prototype"))
                    extractor.SubTypeData = 0;
            }
        }
    }
}
