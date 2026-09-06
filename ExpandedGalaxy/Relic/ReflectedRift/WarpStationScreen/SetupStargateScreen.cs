using UnityEngine;
using static ExpandedGalaxy.Relic;

namespace ExpandedGalaxy
{
    internal class SetupStargateScreen
    {
        internal static int inputLabel;
        internal static int targetLabel;
        internal static int statusLabel;
        internal static string currentSolution;
        internal static void Setup(PLWarpStationScreen __instance)
        {
            UISprite panel = __instance.CreatePanel("Stargate Controls", new Vector3(0f, 0.5f, 0f), new Vector2(512f, 512f), new Color(0.65f, 0.65f, 0.65f), null, UIWidget.Pivot.TopLeft);

            panel.transform.position = __instance.WarpPanel.transform.position;

            float x = 152f;
            float y = -60f;

            for (int i = 0; i < 5; i++)
            {
                x = 152f;

                for (int j = 0; j < 5; j++)
                {
                    string name = "inputOption_" + (j + 5 * i).ToString();
                    __instance.CreateButton(name, StargatePuzzle.SubLetters[j + 5 * i].ToString(), new Vector3(x, y), new Vector2(40f, 40f), new Color(0.65f, 0.65f, 0.65f), panel.transform, UIWidget.Pivot.TopLeft);
                    x += 42f;
                }
                y -= 42f;
            }

            __instance.CreateButton("backspace", "<-", new Vector3(152f, -270f), new Vector2(82f, 40f), new Color(0.65f, 0.65f, 0.65f), panel.transform, UIWidget.Pivot.TopLeft);

            __instance.CreateButton("enter", ">>", new Vector3(236f, -270f), new Vector2(124f, 40f), new Color(0.65f, 0.65f, 0.65f), panel.transform, UIWidget.Pivot.TopLeft);

            __instance.CreateLabel(string.Empty, new Vector3(185f, -330f), 20, Color.white, panel.transform, UIWidget.Pivot.Left);
            inputLabel = __instance.AllLabels.Count - 1;

            __instance.CreateLabel("TARGET:", new Vector3(20f, -430f), 18, Color.white, panel.transform, UIWidget.Pivot.Left);
            targetLabel = __instance.AllLabels.Count - 1;

            __instance.CreateLabel("STATUS:", new Vector3(20f, -470f), 18, Color.white, panel.transform, UIWidget.Pivot.Left);
            statusLabel = __instance.AllLabels.Count - 1;

            ClickStargateScreen.currentInput.Clear();

            panel.gameObject.SetActive(true);
        }
    }
}
