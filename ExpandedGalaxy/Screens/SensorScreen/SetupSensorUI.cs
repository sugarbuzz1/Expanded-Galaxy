
using HarmonyLib;
using UnityEngine;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLScientistSensorScreen), "SetupUI")]
    internal class SetupSensorUI
    {
        private static void Postfix(PLScientistSensorScreen __instance)
        {
            Transform InfoBoxBGTransform = __instance.InfoBoxBG.transform;
            for (int i = 0; i < 4; i++)
            {
                string name = "compScanOption_" + i.ToString();
                UISprite button = __instance.CreateButton(name, string.Empty, new Vector3(-190f, 34f - 52f * i), new Vector2(185f, 50f), __instance.UI_White, InfoBoxBGTransform, UIWidget.Pivot.TopLeft);
                button.depth += 10000;
                button.GetComponentInChildren<UILabel>().depth += 10000;
                button.GetComponentInChildren<UILabel>().fontSize /= 2;
                button.GetComponentInChildren<UILabel>().width = 600;
                button.GetComponentInChildren<UILabel>().overflowMethod = UILabel.Overflow.ResizeHeight;
            }
            for (int i = 0; i < 4; i++)
            {
                string name = "compScanOption_" + (i + 4).ToString();
                UISprite button = __instance.CreateButton(name, string.Empty, new Vector3(5f, 34f - 52f * i), new Vector2(185f, 50f), __instance.UI_White, InfoBoxBGTransform, UIWidget.Pivot.TopLeft);
                button.depth += 10000;
                button.GetComponentInChildren<UILabel>().depth += 10000;
                button.GetComponentInChildren<UILabel>().fontSize /= 2;
                button.GetComponentInChildren<UILabel>().width = 600;
                button.GetComponentInChildren<UILabel>().overflowMethod = UILabel.Overflow.ResizeHeight;
            }
            __instance.InfoBoxText.fontSize = 60;
            UISprite button1 = __instance.CreateButton("compScanNext", ">>", new Vector3(140f, 138f), new Vector2(50f, 50f), __instance.UI_White, InfoBoxBGTransform, UIWidget.Pivot.TopLeft);
            button1.depth += 10000;
            button1.GetComponentInChildren<UILabel>().depth += 10000;

            button1 = __instance.CreateButton("compScanBack", "<<", new Vector3(140f, 86f), new Vector2(50f, 50f), __instance.UI_White, InfoBoxBGTransform, UIWidget.Pivot.TopLeft);
            button1.depth += 10000;
            button1.GetComponentInChildren<UILabel>().depth += 10000;
        }
    }
}
