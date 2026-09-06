using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace ExpandedGalaxy
{
    internal class SetupBrokenDriveScreen
    {
        internal static int inputLabel;
        internal static int targetLabel;
        internal static int statusLabel;

        internal static void Setup(PLWarpDriveScreen __instance, out UISprite WarpDrivePanel)
        {
            UIAtlas screenThemeAtlas = __instance.MyScreenHubBase.ScreenThemeAtlas;
            __instance.MyScreenHubBase.ScreenThemeAtlas = PLGlobal.Instance.SquareThemeAtlas;
            Traverse traverse = Traverse.Create((object)__instance);
            object[] objArray1 = new object[6]
            {
          (object) "Warp Drive Controls: FTL-X899",
          (object) new Vector3(0.0f, 0.5f, 0.0f),
          (object) new Vector2(512f, 512f),
          (object) new Color(0.65f, 0.65f, 0.65f),
          null,
          (object) UIWidget.Pivot.TopLeft
            };
            UISprite uiSprite1 = traverse.Method("CreatePanel", new System.Type[6]
            {
          typeof (string),
          typeof (Vector3),
          typeof (Vector2),
          typeof (Color),
          typeof (Transform),
          typeof (UIWidget.Pivot)
            }, (object[])null).GetValue<UISprite>(objArray1);
            WarpDrivePanel = uiSprite1;
            float y = -60f;
            for (int index1 = 0; index1 < 5; ++index1)
            {
                float x = 152f;
                for (int index2 = 0; index2 < 5; ++index2)
                {
                    object[] objArray2 = new object[7]
                    {
              (object) ("inputOption_" + (index2 + 5 * index1).ToString()),
              (object) StargatePuzzle.SubLetters[index2 + 5 * index1].ToString(),
              (object) new Vector3(x, y),
              (object) new Vector2(40f, 40f),
              (object) new Color(0.65f, 0.65f, 0.65f),
              (object) uiSprite1.transform,
              (object) UIWidget.Pivot.TopLeft
                    };
                    UISprite uiSprite2 = traverse.Method("CreateButton", new System.Type[7]
                    {
              typeof (string),
              typeof (string),
              typeof (Vector3),
              typeof (Vector2),
              typeof (Color),
              typeof (Transform),
              typeof (UIWidget.Pivot)
                    }, (object[])null).GetValue<UISprite>(objArray2);
                    traverse.Field("AllStylizedElements").GetValue<List<UISprite>>().Remove(uiSprite2);
                    x += 42f;
                }
                y -= 42f;
            }
            object[] objArray3 = new object[7]
            {
          (object) "backspace",
          (object) "<-",
          (object) new Vector3(152f, -270f),
          (object) new Vector2(82f, 40f),
          (object) new Color(0.65f, 0.65f, 0.65f),
          (object) uiSprite1.transform,
          (object) UIWidget.Pivot.TopLeft
            };
            UISprite uiSprite3 = traverse.Method("CreateButton", new System.Type[7]
            {
          typeof (string),
          typeof (string),
          typeof (Vector3),
          typeof (Vector2),
          typeof (Color),
          typeof (Transform),
          typeof (UIWidget.Pivot)
            }, (object[])null).GetValue<UISprite>(objArray3);
            traverse.Field("AllStylizedElements").GetValue<List<UISprite>>().Remove(uiSprite3);
            object[] objArray4 = new object[7]
            {
          (object) "enter",
          (object) ">>",
          (object) new Vector3(236f, -270f),
          (object) new Vector2(124f, 40f),
          (object) new Color(0.65f, 0.65f, 0.65f),
          (object) uiSprite1.transform,
          (object) UIWidget.Pivot.TopLeft
            };
            UISprite uiSprite4 = traverse.Method("CreateButton", new System.Type[7]
            {
          typeof (string),
          typeof (string),
          typeof (Vector3),
          typeof (Vector2),
          typeof (Color),
          typeof (Transform),
          typeof (UIWidget.Pivot)
            }, (object[])null).GetValue<UISprite>(objArray4);
            traverse.Field("AllStylizedElements").GetValue<List<UISprite>>().Remove(uiSprite4);
            object[] objArray5 = new object[7]
            {
          (object) "reverse",
          (object) "Inv.",
          (object) new Vector3(236f, -312f),
          (object) new Vector2(124f, 40f),
          (object) new Color(0.65f, 0.65f, 0.65f),
          (object) uiSprite1.transform,
          (object) UIWidget.Pivot.TopLeft
            };
            UISprite uiSprite5 = traverse.Method("CreateButton", new System.Type[7]
            {
          typeof (string),
          typeof (string),
          typeof (Vector3),
          typeof (Vector2),
          typeof (Color),
          typeof (Transform),
          typeof (UIWidget.Pivot)
            }, (object[])null).GetValue<UISprite>(objArray5);
            traverse.Field("AllStylizedElements").GetValue<List<UISprite>>().Remove(uiSprite5);
            object[] objArray6 = new object[6]
            {
          (object) string.Empty,
          (object) new Vector3(185f, -390f),
          (object) 20,
          (object) Color.white,
          (object) uiSprite1.transform,
          (object) UIWidget.Pivot.Left
            };
            traverse.Method("CreateLabel", new System.Type[6]
            {
          typeof (string),
          typeof (Vector3),
          typeof (int),
          typeof (Color),
          typeof (Transform),
          typeof (UIWidget.Pivot)
            }, (object[])null).GetValue<UILabel>(objArray6);
            SetupBrokenDriveScreen.inputLabel = traverse.Field("AllLabels").GetValue<List<UILabel>>().Count - 1;
            object[] objArray7 = new object[6]
            {
          (object) "TARGET:",
          (object) new Vector3(20f, -430f),
          (object) 18,
          (object) Color.white,
          (object) uiSprite1.transform,
          (object) UIWidget.Pivot.Left
            };
            traverse.Method("CreateLabel", new System.Type[6]
            {
          typeof (string),
          typeof (Vector3),
          typeof (int),
          typeof (Color),
          typeof (Transform),
          typeof (UIWidget.Pivot)
            }, (object[])null).GetValue<UILabel>(objArray7);
            SetupBrokenDriveScreen.targetLabel = traverse.Field("AllLabels").GetValue<List<UILabel>>().Count - 1;
            object[] objArray8 = new object[6]
            {
          (object) "STATUS:",
          (object) new Vector3(20f, -470f),
          (object) 18,
          (object) Color.white,
          (object) uiSprite1.transform,
          (object) UIWidget.Pivot.Left
            };
            traverse.Method("CreateLabel", new System.Type[6]
            {
          typeof (string),
          typeof (Vector3),
          typeof (int),
          typeof (Color),
          typeof (Transform),
          typeof (UIWidget.Pivot)
            }, (object[])null).GetValue<UILabel>(objArray8);
            SetupBrokenDriveScreen.statusLabel = traverse.Field("AllLabels").GetValue<List<UILabel>>().Count - 1;
            ClickBrokenWarpDriveScreen.currentInput.Clear();
            uiSprite1.gameObject.SetActive(false);
            __instance.MyScreenHubBase.ScreenThemeAtlas = screenThemeAtlas;
        }
    }
}