using HarmonyLib;
using UnityEngine;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLTooltipRequest), "Draw")]
    internal class RelicText
    {
        private static void Postfix(PLTooltipRequest __instance, ref Rect ___tooltipRect)
        {
            if (__instance.Ware != null)
            {
                if (Relic.GetIsRelic(__instance.Ware))
                {
                    GUI.color = Relic.GetRelicColor();
                    GUI.Label(new Rect(___tooltipRect.x + 10f, ___tooltipRect.y + 12f, 420f - 20f, 50f), "RELIC", PLGlobal.Instance.guiStyleComponentMenuExtraLineRight);
                }
            }
        }
    }
}
