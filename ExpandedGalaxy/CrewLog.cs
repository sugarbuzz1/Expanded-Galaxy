using HarmonyLib;
using PulsarModLoader;
using PulsarModLoader.Patches;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.UI;

namespace ExpandedGalaxy
{
    public struct CrewLogData
    {
        public string Text;
        public float timeStamp;
        public int optionalSectorID;
        public Color optionalColor;
    }

    public struct CrewLogScreenObjects
    {
        public UITexture CrewLogButton;
        public UITexture StatusButton;
        public UISprite LogPanel;
        public UILabel SectorLabel;
        public UILabel TimeLabel;
        public List<UISprite> LogButtons;
        public List<UITexture> LogColors;
        public UISprite BackButton;
        public UISprite NextButton;
        public UISprite NewLogButton;
        public UIPanel LogInfoClippingPanel;
        public UIWidget LogInfoPanel;
        public UISprite LogInfoBox;
        public UILabel LogInfoBoxLabel;
        public UILabel LogInfoBoxText;
        public UISprite LogInfoBoxClose;
        public UISprite LogInfoBoxCreate;
        public UISprite LogInfoButtonDel;
        public UISprite LogInfoBoxSectorButton;
        public UITexture LogInfoSectorColor;
        public List<UITexture> LogInfoKeypadButtons;
        public List<UILabel> LogInfoKeypadLabels;

    }

    public class CrewLogManager
    {
        private static CrewLogManager m_instance;
        private Dictionary<PLCaptainScreen, CrewLogScreenObjects> m_screenobjects;
        private List<CrewLogData> m_logs;
        private int showlogindex;
        private int logindex;
        private CrewLogData tempData;
        private Dictionary<int, GameObject> m_mappins;

        public static CrewLogManager Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new CrewLogManager();
                return m_instance;
            }
        }

        public CrewLogManager()
        {
            m_screenobjects = new Dictionary<PLCaptainScreen, CrewLogScreenObjects>();
            m_logs = new List<CrewLogData>();
            logindex = -1;
            showlogindex = 0;
            tempData = new CrewLogData()
            {
                timeStamp = 0f,
                optionalColor = Color.black,
                Text = string.Empty,
                optionalSectorID = -1
            };
            m_mappins = new Dictionary<int, GameObject>();
        }

        public int LogIndex
        {
            get { return logindex; }
            set { logindex = value; }
        }

        public int ShowLogIndex
        {
            get { return showlogindex; }
            set { showlogindex = value; }
        }

        public CrewLogData TempData
        {
            get { return tempData; }
            set { tempData = value; }
        }

        public void OnNewGame()
        {
            m_screenobjects.Clear();
            m_logs.Clear();
            logindex = -1;
            showlogindex = 0;
            tempData = new CrewLogData()
            {
                timeStamp = 0f,
                optionalColor = Color.black,
                Text = string.Empty,
                optionalSectorID = -1
            };
            foreach (GameObject image in m_mappins.Values)
                UnityEngine.Object.Destroy(image);
            m_mappins.Clear();
        }

        public void ClearLogs()
        {
            m_logs.Clear();
            logindex = -1;
            foreach (GameObject image in m_mappins.Values)
                UnityEngine.Object.Destroy(image);
            m_mappins.Clear();
        }

        public Dictionary<int, GameObject> MapPins
        {
            get { return m_mappins; }
            set { m_mappins = value; }
        }

        public CrewLogScreenObjects GetObjectsForScreen(PLCaptainScreen captainScreen)
        {
            if (m_screenobjects.ContainsKey(captainScreen))
                return m_screenobjects[captainScreen];
            SetupScreen(captainScreen);
            return m_screenobjects[captainScreen];
        }

        public bool HasScreenObjects(PLCaptainScreen captainScreen)
        {
            return m_screenobjects.ContainsKey(captainScreen);
        }

        public static string FormatPlaytime(float playTime)
        {
            int playTimeInt = Mathf.FloorToInt(playTime);
            int sec = playTimeInt % 60;
            int min = (playTimeInt / 60) % 60;
            int hr = (playTimeInt / 3600);
            string outString = ((hr > 0) ? hr.ToString("00") + ":" : string.Empty);
            outString += (min > 0) ? min.ToString("00") + ":" : (hr > 0) ? "00:" : string.Empty;
            outString += (sec > 0) ? sec.ToString("00") : "00";
            return outString;

        }
        public void SetupScreen(PLCaptainScreen captainScreen)
        {
            if (m_screenobjects.ContainsKey(captainScreen))
                return;
            Traverse traverse = Traverse.Create(captainScreen);
            CrewLogScreenObjects screenObjects = new CrewLogScreenObjects();
            screenObjects.LogButtons = new List<UISprite>();
            screenObjects.LogColors = new List<UITexture>();
            screenObjects.LogInfoKeypadButtons = new List<UITexture>();
            screenObjects.LogInfoKeypadLabels = new List<UILabel>();
            object[] params1;
            params1 = new object[7]
            {
                    "CrewLogBtn",
                    PLGlobal.Instance.TriangleIcon,
                    new Vector3(468f, -45f),
                    new Vector2(32f, 32f),
                    Color.white,
                    traverse.Field("StatusPanel").GetValue<UISprite>().cachedTransform,
                    UIWidget.Pivot.Right
            };
            screenObjects.CrewLogButton = traverse.Method("CreateButtonEditable", new Type[7] { typeof(string), typeof(Texture2D), typeof(Vector3), typeof(Vector2), typeof(Color), typeof(Transform), typeof(UIWidget.Pivot) }).GetValue<UITexture>(params1);
            screenObjects.CrewLogButton.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
            params1 = new object[6]
            {
                    "CREW LOGS",
                    new Vector3(235f, -60f),
                    15,
                    new Color(0.65f, 0.65f, 0.65f),
                    traverse.Field("StatusPanel").GetValue<UISprite>().cachedTransform,
                    UIWidget.Pivot.Left
            };
            traverse.Method("CreateLabel", new Type[6] { typeof(string), typeof(Vector3), typeof(int), typeof(Color), typeof(Transform), typeof(UIWidget.Pivot) }).GetValue<UILabel>(params1);
            params1 = new object[6]
            {
                    "Crew Logs",
                    new Vector3(0f, 0f, 0f),
                    new Vector2(512f, 512f),
                    new Color(0.65f, 0.65f, 0.65f),
                    null,
                    UIWidget.Pivot.TopLeft
            };
            screenObjects.LogPanel = traverse.Method("CreatePanel", new Type[6] { typeof(string), typeof(Vector3), typeof(Vector2), typeof(Color), typeof(Transform), typeof(UIWidget.Pivot) }).GetValue<UISprite>(params1);
            screenObjects.LogPanel.gameObject.SetActive(false);
            params1 = new object[7]
            {
                    "StatusBtn",
                    PLGlobal.Instance.TriangleIcon,
                    new Vector3(468f, -45f),
                    new Vector2(32f, 32f),
                    Color.white,
                    screenObjects.LogPanel.cachedTransform,
                    UIWidget.Pivot.Right
            };
            screenObjects.StatusButton = traverse.Method("CreateButtonEditable", new Type[7] { typeof(string), typeof(Texture2D), typeof(Vector3), typeof(Vector2), typeof(Color), typeof(Transform), typeof(UIWidget.Pivot) }).GetValue<UITexture>(params1);
            screenObjects.StatusButton.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
            params1 = new object[7]
            {
                captainScreen.MyScreenHubBase.ScreenThemeAtlas,
                "small_button",
                new Vector3(32f, -120f),
                new Vector2(360f, 40f),
                new Color(0.1f, 0.1f, 0.1f, 0.95f),
                screenObjects.LogPanel.cachedTransform,
                UIWidget.Pivot.TopLeft
            };
            traverse.Method("CreateSprite", new Type[7] { typeof(UIAtlas), typeof(string), typeof(Vector3), typeof(Vector2),typeof(Color), typeof(Transform), typeof(UIWidget.Pivot) }).GetValue<UISprite>(params1);
            params1 = new object[6]
            {
                    "Logs",
                    new Vector3(54f, -140f),
                    15,
                    new Color(0.65f, 0.65f, 0.65f),
                    screenObjects.LogPanel.cachedTransform,
                    UIWidget.Pivot.Left
            };
            traverse.Method("CreateLabel", new Type[6] { typeof(string), typeof(Vector3), typeof(int), typeof(Color), typeof(Transform), typeof(UIWidget.Pivot) }).GetValue<UILabel>(params1);
            params1 = new object[6]
            {
                    "Sector: 0",
                    new Vector3(32f, -60f),
                    17,
                    new Color(0.65f, 0.65f, 0.65f),
                    screenObjects.LogPanel.cachedTransform,
                    UIWidget.Pivot.Left
            };
            screenObjects.SectorLabel = traverse.Method("CreateLabel", new Type[6] { typeof(string), typeof(Vector3), typeof(int), typeof(Color), typeof(Transform), typeof(UIWidget.Pivot) }).GetValue<UILabel>(params1);
            params1 = new object[6]
            {
                    "Time: 00:00:00",
                    new Vector3(32f, -96f),
                    17,
                    new Color(0.65f, 0.65f, 0.65f),
                    screenObjects.LogPanel.cachedTransform,
                    UIWidget.Pivot.Left
            };
            screenObjects.TimeLabel = traverse.Method("CreateLabel", new Type[6] { typeof(string), typeof(Vector3), typeof(int), typeof(Color), typeof(Transform), typeof(UIWidget.Pivot) }).GetValue<UILabel>(params1);
            float x = 32f;
            float y = -164;
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    int num = 1 + (5 * i) + j;
                    params1 = new object[7]
                    {
                    "LogBtn" + num.ToString(),
                    "00:00:00",
                    new Vector3(x, y),
                    new Vector2(170f, 60f),
                    new Color(0.65f, 0.65f, 0.65f),
                    screenObjects.LogPanel.transform,
                    UIWidget.Pivot.TopLeft
                    };
                    screenObjects.LogButtons.Add(traverse.Method("CreateButton", new Type[7] { typeof(string), typeof(string), typeof(Vector3), typeof(Vector2), typeof(Color), typeof(Transform), typeof(UIWidget.Pivot) }).GetValue<UISprite>(params1));
                    screenObjects.LogButtons[screenObjects.LogButtons.Count - 1].gameObject.SetActive(false);
                    y -= 62f;
                }
                x += 190f;
                y = -164f;
            }
            x = 176f;
            y = -186f;
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    params1 = new object[6]
                    {
                    PLGlobal.Instance.WhitePixel,
                    new Vector3(x, y),
                    new Vector2(16f, 16f),
                    Color.red,
                    screenObjects.LogPanel.cachedTransform,
                    UIWidget.Pivot.TopLeft
                    };
                    screenObjects.LogColors.Add(traverse.Method("CreateTexture", new Type[6] { typeof(Texture2D), typeof(Vector3), typeof(Vector2), typeof(Color), typeof(Transform), typeof(UIWidget.Pivot) }).GetValue<UITexture>(params1));
                    screenObjects.LogColors[screenObjects.LogColors.Count - 1].gameObject.SetActive(false);
                    y -= 62f;
                }
                x += 190f;
                y = -186f;
            }
            params1 = new object[7]
                    {
                    "BackBtn",
                    "<<",
                    new Vector3(432f, -422f),
                    new Vector2(48f, 48f),
                    new Color(0.65f, 0.65f, 0.65f),
                    screenObjects.LogPanel.transform,
                    UIWidget.Pivot.TopLeft
                    };
            screenObjects.BackButton = traverse.Method("CreateButton", new Type[7] { typeof(string), typeof(string), typeof(Vector3), typeof(Vector2), typeof(Color), typeof(Transform), typeof(UIWidget.Pivot) }).GetValue<UISprite>(params1);
            params1 = new object[7]
                    {
                    "NextBtn",
                    ">>",
                    new Vector3(432f, -360f),
                    new Vector2(48f, 48f),
                    new Color(0.65f, 0.65f, 0.65f),
                    screenObjects.LogPanel.transform,
                    UIWidget.Pivot.TopLeft
                    };
            screenObjects.NextButton = traverse.Method("CreateButton", new Type[7] { typeof(string), typeof(string), typeof(Vector3), typeof(Vector2), typeof(Color), typeof(Transform), typeof(UIWidget.Pivot) }).GetValue<UISprite>(params1);
            params1 = new object[7]
                    {
                    "NewBtn",
                    "New",
                    new Vector3(294f, -50f),
                    new Vector2(98f, 60f),
                    new Color(0.65f, 0.65f, 0.65f),
                    screenObjects.LogPanel.transform,
                    UIWidget.Pivot.TopLeft
                    };
            screenObjects.NewLogButton = traverse.Method("CreateButton", new Type[7] { typeof(string), typeof(string), typeof(Vector3), typeof(Vector2), typeof(Color), typeof(Transform), typeof(UIWidget.Pivot) }).GetValue<UISprite>(params1);
            params1 = new object[5]
                    {
                    "LogInfoBox",
                    new Vector3(0f, 0f),
                    new Vector2(512f, 512f),
                    null,
                    UIWidget.Pivot.TopLeft
                    };
            screenObjects.LogInfoPanel = traverse.Method("CreateBlankPanel", new Type[5] { typeof(string), typeof(Vector3), typeof(Vector2), typeof(Transform), typeof(UIWidget.Pivot) }).GetValue<UIWidget>(params1);
            screenObjects.LogInfoPanel.depth += 10000;
            screenObjects.LogInfoPanel.gameObject.SetActive(false);
            params1 = new object[7]
            {
                captainScreen.MyScreenHubBase.ScreenThemeAtlas,
                "small_button",
                new Vector3(0, 0),
                new Vector2(360f, 400f),
                new Color(0.65f, 0.65f, 0.65f),
                screenObjects.LogInfoPanel.cachedTransform,
                UIWidget.Pivot.Center
            };
            screenObjects.LogInfoBox = traverse.Method("CreateSprite", new Type[7] { typeof(UIAtlas), typeof(string), typeof(Vector3), typeof(Vector2), typeof(Color), typeof(Transform), typeof(UIWidget.Pivot) }).GetValue<UISprite>(params1);
            screenObjects.LogInfoBox.depth += 10000;
            params1 = new object[6]
            {
                    "Title",
                    new Vector3(-164f, 184f),
                    14,
                    new Color(0.65f, 0.65f, 0.65f),
                    screenObjects.LogInfoBox.transform,
                    UIWidget.Pivot.TopLeft,
            };
            screenObjects.LogInfoBoxLabel = traverse.Method("CreateLabel", new Type[6] { typeof(string), typeof(Vector3), typeof(int), typeof(Color), typeof(Transform), typeof(UIWidget.Pivot) }).GetValue<UILabel>(params1);
            screenObjects.LogInfoBoxLabel.depth += 10000;
            params1 = new object[8]
            {
                    "Text",
                    new Vector3(-164f, 128f),
                    12,
                    720,
                    1200,
                    new Color(0.65f, 0.65f, 0.65f),
                    screenObjects.LogInfoBox.transform,
                    UIWidget.Pivot.TopLeft,
            };
            screenObjects.LogInfoBoxText = traverse.Method("CreateParagraph", new Type[8] { typeof(string), typeof(Vector3), typeof(int), typeof(int), typeof(int), typeof(Color), typeof(Transform), typeof(UIWidget.Pivot) }).GetValue<UILabel>(params1);
            screenObjects.LogInfoBoxText.depth += 10000;
            screenObjects.LogInfoBoxText.overflowMethod = UILabel.Overflow.ClampContent;
            params1 = new object[7]
                    {
                    "LogInfoCloseBtn",
                    "X",
                    new Vector3(114f, 184f),
                    new Vector2(48f, 48f),
                    new Color(0.65f, 0.65f, 0.65f),
                    screenObjects.LogInfoBox.transform,
                    UIWidget.Pivot.TopLeft,
                    };
            screenObjects.LogInfoBoxClose = traverse.Method("CreateButton", new Type[7] { typeof(string), typeof(string), typeof(Vector3), typeof(Vector2), typeof(Color), typeof(Transform), typeof(UIWidget.Pivot) }).GetValue<UISprite>(params1);
            screenObjects.LogInfoBoxClose.depth += 10000;
            screenObjects.LogInfoBoxClose.GetComponentInChildren<UILabel>().depth += 10000;
            params1 = new object[7]
                    {
                    "LogInfoSectorBtn",
                    "0000",
                    new Vector3(-40f, 184f),
                    new Vector2(144f, 48f),
                    new Color(0.65f, 0.65f, 0.65f),
                    screenObjects.LogInfoBox.transform,
                    UIWidget.Pivot.TopLeft,
                    };
            screenObjects.LogInfoBoxSectorButton = traverse.Method("CreateButton", new Type[7] { typeof(string), typeof(string), typeof(Vector3), typeof(Vector2), typeof(Color), typeof(Transform), typeof(UIWidget.Pivot) }).GetValue<UISprite>(params1);
            screenObjects.LogInfoBoxSectorButton.depth += 10000;
            screenObjects.LogInfoBoxSectorButton.GetComponentInChildren<UILabel>().depth += 10000;
            params1 = new object[7]
                    {
                    "LogInfoCreateBtn",
                    "Create",
                    new Vector3(20f, -134f),
                    new Vector2(144f, 48f),
                    new Color(0.65f, 0.65f, 0.65f),
                    screenObjects.LogInfoBox.transform,
                    UIWidget.Pivot.TopLeft,
                    };
            screenObjects.LogInfoBoxCreate = traverse.Method("CreateButton", new Type[7] { typeof(string), typeof(string), typeof(Vector3), typeof(Vector2), typeof(Color), typeof(Transform), typeof(UIWidget.Pivot) }).GetValue<UISprite>(params1);
            screenObjects.LogInfoBoxCreate.depth += 10000;
            screenObjects.LogInfoBoxCreate.GetComponentInChildren<UILabel>().depth += 10000;
            params1 = new object[7]
                    {
                    "LogInfoDelBtn",
                    "Del",
                    new Vector3(114f, -134f),
                    new Vector2(48f, 48f),
                    new Color(0.65f, 0.65f, 0.65f),
                    screenObjects.LogInfoBox.transform,
                    UIWidget.Pivot.TopLeft,
                    };
            screenObjects.LogInfoButtonDel = traverse.Method("CreateButton", new Type[7] { typeof(string), typeof(string), typeof(Vector3), typeof(Vector2), typeof(Color), typeof(Transform), typeof(UIWidget.Pivot) }).GetValue<UISprite>(params1);
            screenObjects.LogInfoButtonDel.depth += 10000;
            screenObjects.LogInfoButtonDel.GetComponentInChildren<UILabel>().depth += 10000;
            params1 = new object[6]
                    {
                    PLGlobal.Instance.WhitePixel,
                    new Vector3(-164f, 156f),
                    new Vector2(98f, 16f),
                    Color.red,
                    screenObjects.LogInfoBox.transform,
                    UIWidget.Pivot.TopLeft
                    };
            screenObjects.LogInfoSectorColor = traverse.Method("CreateTexture", new Type[6] { typeof(Texture2D), typeof(Vector3), typeof(Vector2), typeof(Color), typeof(Transform), typeof(UIWidget.Pivot) }).GetValue<UITexture>(params1);
            screenObjects.LogInfoSectorColor.depth += 10000;
            x = 44f;
            y = 100f;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    params1 = new object[7]
                    {
                    "KeypadBtn_" + (i * 4 + j).ToString(),
                    PLGlobal.Instance.WhitePixel,
                    new Vector3(x, y),
                    new Vector2(16f, 16f),
                    new Color(0.65f, 0.65f, 0.65f),
                    screenObjects.LogInfoBox.transform,
                    UIWidget.Pivot.TopLeft
                    };
                    screenObjects.LogInfoKeypadButtons.Add(traverse.Method("CreateButtonEditable", new Type[7] { typeof(string), typeof(Texture2D), typeof(Vector3), typeof(Vector2), typeof(Color), typeof(Transform), typeof(UIWidget.Pivot) }).GetValue<UITexture>(params1));
                    screenObjects.LogInfoKeypadButtons[screenObjects.LogInfoKeypadButtons.Count - 1].depth += 10000;
                    string labelText = "";
                    switch(i)
                    {
                        case 0:
                            switch (j)
                            {
                                case 0:
                                    labelText = "1";
                                    break;
                                case 1:
                                    labelText = "4";
                                    break;
                                case 2:
                                    labelText = "7";
                                    break;
                                case 3:
                                    labelText = "<-";
                                    break;
                            }
                            break;
                        case 1:
                            switch (j)
                            {
                                case 0:
                                    labelText = "2";
                                    break;
                                case 1:
                                    labelText = "5";
                                    break;
                                case 2:
                                    labelText = "8";
                                    break;
                                case 3:
                                    labelText = "0";
                                    break;
                            }
                            break;
                        case 2:
                            switch (j)
                            {
                                case 0:
                                    labelText = "3";
                                    break;
                                case 1:
                                    labelText = "6";
                                    break;
                                case 2:
                                    labelText = "9";
                                    break;
                                case 3:
                                    labelText = "C";
                                    break;
                            }
                            break;
                    }
                    params1 = new object[6]
                    {
                    labelText,
                    new Vector3(x + 6f, y - 2f),
                    8,
                    Color.black,
                    screenObjects.LogInfoBox.transform,
                    UIWidget.Pivot.TopLeft,
                    };
                    screenObjects.LogInfoKeypadLabels.Add(traverse.Method("CreateLabel", new Type[6] { typeof(string), typeof(Vector3), typeof(int), typeof(Color), typeof(Transform), typeof(UIWidget.Pivot) }).GetValue<UILabel>(params1));
                    screenObjects.LogInfoKeypadLabels[screenObjects.LogInfoKeypadLabels.Count - 1].depth += 10000;
                    y -= 18f;
                }
                x += 18f;
                y = 100f;
            }
            m_screenobjects.Add(captainScreen, screenObjects);
        }

        public void HideKeyPad(PLCaptainScreen captainScreen, bool hide = true)
        {
            if (!m_screenobjects.ContainsKey(captainScreen))
                return;
            foreach (UITexture texture in m_screenobjects[captainScreen].LogInfoKeypadButtons)
                texture.gameObject.SetActive(!hide);
            foreach (UILabel label in m_screenobjects[captainScreen].LogInfoKeypadLabels)
                label.gameObject.SetActive(!hide);
        }

        internal IEnumerator ToggleLogButtons(PLCaptainScreen captainScreen, bool hide = true)
        {
            yield return null;
            if (!m_screenobjects.ContainsKey(captainScreen))
                yield break;
            Traverse traverse = Traverse.Create(captainScreen);
            if (hide)
            {
                traverse.Field("AllButtons").GetValue<List<UIWidget>>().Remove(m_screenobjects[captainScreen].StatusButton);
                traverse.Field("AllButtons").GetValue<List<UIWidget>>().Remove(m_screenobjects[captainScreen].NewLogButton);
                traverse.Field("AllButtons").GetValue<List<UIWidget>>().Remove(m_screenobjects[captainScreen].NextButton);
                traverse.Field("AllButtons").GetValue<List<UIWidget>>().Remove(m_screenobjects[captainScreen].BackButton);
                foreach (UISprite sprite in m_screenobjects[captainScreen].LogButtons)
                    traverse.Field("AllButtons").GetValue<List<UIWidget>>().Remove(sprite);
            }
            else
            {
                if (!traverse.Field("AllButtons").GetValue<List<UIWidget>>().Contains(m_screenobjects[captainScreen].StatusButton))
                    traverse.Field("AllButtons").GetValue<List<UIWidget>>().Add(m_screenobjects[captainScreen].StatusButton);
                if (!traverse.Field("AllButtons").GetValue<List<UIWidget>>().Contains(m_screenobjects[captainScreen].NewLogButton))
                    traverse.Field("AllButtons").GetValue<List<UIWidget>>().Add(m_screenobjects[captainScreen].NewLogButton);
                if (!traverse.Field("AllButtons").GetValue<List<UIWidget>>().Contains(m_screenobjects[captainScreen].NextButton))
                    traverse.Field("AllButtons").GetValue<List<UIWidget>>().Add(m_screenobjects[captainScreen].NextButton);
                if (!traverse.Field("AllButtons").GetValue<List<UIWidget>>().Contains(m_screenobjects[captainScreen].BackButton))
                    traverse.Field("AllButtons").GetValue<List<UIWidget>>().Add(m_screenobjects[captainScreen].BackButton);
                foreach (UISprite sprite in m_screenobjects[captainScreen].LogButtons)
                    if (!traverse.Field("AllButtons").GetValue<List<UIWidget>>().Contains(sprite))
                        traverse.Field("AllButtons").GetValue<List<UIWidget>>().Add(sprite);
            }
        }

        public List<CrewLogData> GetLogs()
        {
            return m_logs;
        }

        public void AddLog(CrewLogData logData)
        {
            m_logs.Add(logData);
            this.AddPin(m_logs.Count - 1);
        }

        public void RemoveLog(int index)
        {
            if (index < m_logs.Count)
            {
                this.RemovePin(index);
                m_logs.RemoveAt(index);
            }
        }

        private void AddPin(int index)
        {
            if (!(index < this.m_logs.Count))
                return;
            CrewLogData data = m_logs[index];
            if (m_mappins.ContainsKey(data.optionalSectorID))
                return;
            PLSectorInfo sectorWithId = PLServer.GetSectorWithID(data.optionalSectorID);
            if (sectorWithId == null)
                return;
            Image pin = UnityEngine.Object.Instantiate(PLStarmap.Instance.HunterLocImage, PLStarmap.Instance.HunterLocImage.transform.parent);
            pin.GetComponent<Image>().color = data.optionalColor;
            Image[] image = pin.GetComponentsInChildren<Image>();
            image[1].color = data.optionalColor * 0.5f;
            image[2].color = data.optionalColor;
            pin.GetComponentInChildren<Text>().text = FormatPlaytime(data.timeStamp);
            pin.GetComponentInChildren<Text>().color = data.optionalColor;
            pin.transform.localPosition = sectorWithId.Position * 2000f + new Vector3(0.0f, -15f, 0.0f);
            pin.transform.localPosition = new Vector3(pin.transform.localPosition.x, pin.transform.localPosition.y, 0.0f);
            pin.gameObject.SetActive(true);
            image[1].gameObject.SetActive(true);
            m_mappins.Add(data.optionalSectorID, pin.gameObject);
        }

        private void UpdatePinForSector(int inSectorID, int removedLogIndex)
        {
            int index = 0;
            foreach (CrewLogData logData in CrewLogManager.Instance.GetLogs())
            {
                if (logData.optionalSectorID == inSectorID && index != removedLogIndex)
                {
                    this.AddPin(index);
                    break;
                }
                index++;
            }
        }

        public void UpdateAllPins()
        {
            int index = 0;
            foreach (CrewLogData logData in CrewLogManager.Instance.GetLogs())
            {
                if (logData.optionalSectorID != -1 && !m_mappins.Keys.Contains(logData.optionalSectorID))
                {
                    this.AddPin(index);
                    break;
                }
                index++;
            }
        }

        private void RemovePin(int index)
        {
            if (!(index < this.m_logs.Count))
                return;
            CrewLogData data = m_logs[index];
            if (!m_mappins.ContainsKey(data.optionalSectorID))
                return;
            UnityEngine.Object.Destroy(m_mappins[data.optionalSectorID]);
            m_mappins.Remove(data.optionalSectorID);
            UpdatePinForSector(data.optionalSectorID, index);
        }
    }
    internal class CrewLog
    {
        [HarmonyPatch(typeof(PLCaptainScreen), "SetupUI")]
        internal class SetupCaptainScreen
        {
            private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                List<CodeInstruction> list = instructions.ToList();

                List<CodeInstruction> targetSequence = new List<CodeInstruction>() {
                    new CodeInstruction(OpCodes.Ldc_R4, -60f),
                };
                List<CodeInstruction> patchSequence = new List<CodeInstruction>()
                {
                    new CodeInstruction(OpCodes.Ldc_R4, 32f),
                    new CodeInstruction(OpCodes.Sub)

                };
                list = HarmonyHelpers.PatchBySequence(list.AsEnumerable<CodeInstruction>(), targetSequence, patchSequence, HarmonyHelpers.PatchMode.AFTER, HarmonyHelpers.CheckMode.NONNULL, false).ToList();

                List<CodeInstruction> targetSequence2 = new List<CodeInstruction>() {
                    new CodeInstruction(OpCodes.Ldc_R4, 64f),
                };
                List<CodeInstruction> patchSequence2 = new List<CodeInstruction>()
                {
                    new CodeInstruction(OpCodes.Ldc_R4, 32f),

                };

                return HarmonyHelpers.PatchBySequence(list.AsEnumerable<CodeInstruction>(), targetSequence2, patchSequence2, HarmonyHelpers.PatchMode.REPLACE, HarmonyHelpers.CheckMode.NONNULL, false);
            }

            private static void Postfix(PLCaptainScreen __instance)
            {
                CrewLogManager.Instance.SetupScreen(__instance);
            }
        }

        [HarmonyPatch(typeof(PLCaptainScreen), "OnButtonClick")]
        internal class CaptainScreenButton
        {
            private static void Postfix(PLCaptainScreen __instance, UIWidget inButton, ref UISprite ___StatusPanel, ref UISprite ___EnemyStatusPanel)
            {
                if (!CrewLogManager.Instance.HasScreenObjects(__instance))
                    return;
                CrewLogScreenObjects screenObjects = CrewLogManager.Instance.GetObjectsForScreen(__instance);
                if (inButton.name == "CrewLogBtn" && !screenObjects.LogPanel.gameObject.activeSelf)
                {
                    if (__instance.MyScreenHubBase.OptionalShipInfo != null && !__instance.MyScreenHubBase.OptionalShipInfo.GetIsPlayerShip())
                        return;
                    screenObjects.LogPanel.gameObject.SetActive(true);
                    CrewLogManager.Instance.UpdateAllPins();
                    __instance.mouseUpFrame = false;
                    ___StatusPanel.gameObject.SetActive(false);
                    ___EnemyStatusPanel.gameObject.SetActive(false);
                    __instance.PlaySoundEventOnAllClonedScreens("play_ship_generic_internal_computer_ui_click");
                }
                else if (inButton.name == "StatusBtn" && screenObjects.LogPanel.gameObject.activeSelf && !screenObjects.LogInfoPanel.gameObject.activeSelf)
                {
                    screenObjects.LogPanel.gameObject.SetActive(false);
                    CrewLogManager.Instance.UpdateAllPins();
                    __instance.mouseUpFrame = false;
                    ___StatusPanel.gameObject.SetActive(true);
                    ___EnemyStatusPanel.gameObject.SetActive(true);
                    __instance.PlaySoundEventOnAllClonedScreens("play_ship_generic_internal_computer_ui_click");
                }
                else if (inButton.name == "NewBtn" && !screenObjects.LogInfoPanel.gameObject.activeSelf)
                {
                    if ((bool)PLServer.Instance.CrewPurchaseLimitsEnabled)
                    {
                        if (PLNetworkManager.Instance.LocalPlayer != null && PLNetworkManager.Instance.LocalPlayer.GetClassID() != 0)
                            PLTabMenu.Instance.TimedErrorMsg = "You do not have permission to add logs at this time!";
                        return;
                    }
                    CrewLogManager.Instance.LogIndex = int.MinValue;
                    screenObjects.LogInfoPanel.gameObject.SetActive(true);
                    screenObjects.LogInfoBoxLabel.text = "New Log";
                    screenObjects.LogInfoBoxText.text = "";
                    CrewLogData data = new CrewLogData
                    {
                        optionalSectorID = -1,
                        timeStamp = (float)PLServer.Instance.Playtime,
                        optionalColor = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f)),
                        Text = string.Empty
                    };
                    screenObjects.LogInfoBoxSectorButton.gameObject.SetActive(false);
                    screenObjects.LogInfoBoxSectorButton.GetComponentInChildren<UILabel>().text = string.Empty;
                    screenObjects.LogInfoBoxCreate.gameObject.SetActive(true);
                    screenObjects.LogInfoButtonDel.gameObject.SetActive(false);
                    screenObjects.LogInfoSectorColor.color = data.optionalColor;
                    CrewLogManager.Instance.HideKeyPad(__instance, false);
                    CrewLogManager.Instance.TempData = data;
                    __instance.StartCoroutine(CrewLogManager.Instance.ToggleLogButtons(__instance));
                    __instance.PlaySoundEventOnAllClonedScreens("play_ship_generic_internal_computer_ui_click");
                }
                else if (inButton.name == "LogInfoCloseBtn")
                {
                    CrewLogManager.Instance.LogIndex = -1;
                    screenObjects.LogInfoPanel.gameObject.SetActive(false);
                    CrewLogManager.Instance.HideKeyPad(__instance);
                    __instance.StartCoroutine(CrewLogManager.Instance.ToggleLogButtons(__instance, false));
                    __instance.PlaySoundEventOnAllClonedScreens("play_ship_generic_internal_computer_ui_click");
                }
                else if (inButton.name == "LogInfoCreateBtn")
                {
                    CrewLogManager.Instance.LogIndex = -1;
                    if (PhotonNetwork.isMasterClient)
                    {
                        CrewLogManager.Instance.AddLog(CrewLogManager.Instance.TempData);
                        List<object> sendArgumentList = new List<object>();
                        int logCount = CrewLogManager.Instance.GetLogs().Count;
                        sendArgumentList.Add(logCount);
                        foreach (CrewLogData sendLogData in CrewLogManager.Instance.GetLogs())
                        {
                            sendArgumentList.Add(sendLogData.Text);
                            sendArgumentList.Add(sendLogData.timeStamp);
                            sendArgumentList.Add(sendLogData.optionalSectorID);
                            sendArgumentList.Add(sendLogData.optionalColor.r);
                            sendArgumentList.Add(sendLogData.optionalColor.g);
                            sendArgumentList.Add(sendLogData.optionalColor.b);
                            sendArgumentList.Add(sendLogData.optionalColor.a);
                        }
                        ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.ServerSendLog", PhotonTargets.Others, sendArgumentList.ToArray());
                    }
                    else
                    {
                        ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.ClientSendLog", PhotonTargets.MasterClient, new object[7]
                        {
                            CrewLogManager.Instance.TempData.Text,
                            CrewLogManager.Instance.TempData.timeStamp,
                            CrewLogManager.Instance.TempData.optionalSectorID,
                            CrewLogManager.Instance.TempData.optionalColor.r,
                            CrewLogManager.Instance.TempData.optionalColor.g,
                            CrewLogManager.Instance.TempData.optionalColor.b,
                            CrewLogManager.Instance.TempData.optionalColor.a
                        });
                    }
                    screenObjects.LogInfoPanel.gameObject.SetActive(false);
                    CrewLogManager.Instance.HideKeyPad(__instance);
                    __instance.StartCoroutine(CrewLogManager.Instance.ToggleLogButtons(__instance, false));
                    __instance.PlaySoundEventOnAllClonedScreens("play_ship_generic_internal_computer_ui_click");
                }
                else if (inButton.name == "LogInfoDelBtn")
                {
                    if (PLNetworkManager.Instance.LocalPlayer != null && PLNetworkManager.Instance.LocalPlayer.GetClassID() != 0)
                    {
                        PLTabMenu.Instance.TimedErrorMsg = "Only the captain can delete logs!";
                        return;
                    }
                    if (CrewLogManager.Instance.LogIndex > -1)
                    {
                        CrewLogManager.Instance.RemoveLog(CrewLogManager.Instance.LogIndex);
                        List<object> sendArgumentList = new List<object>();
                        int logCount = CrewLogManager.Instance.GetLogs().Count;
                        sendArgumentList.Add(logCount);
                        foreach (CrewLogData sendLogData in CrewLogManager.Instance.GetLogs())
                        {
                            sendArgumentList.Add(sendLogData.Text);
                            sendArgumentList.Add(sendLogData.timeStamp);
                            sendArgumentList.Add(sendLogData.optionalSectorID);
                            sendArgumentList.Add(sendLogData.optionalColor.r);
                            sendArgumentList.Add(sendLogData.optionalColor.g);
                            sendArgumentList.Add(sendLogData.optionalColor.b);
                            sendArgumentList.Add(sendLogData.optionalColor.a);
                        }
                        ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.ServerSendLog", PhotonTargets.Others, sendArgumentList.ToArray());
                    }
                    CrewLogManager.Instance.LogIndex = -1;
                    screenObjects.LogInfoPanel.gameObject.SetActive(false);
                    CrewLogManager.Instance.HideKeyPad(__instance);
                    __instance.StartCoroutine(CrewLogManager.Instance.ToggleLogButtons(__instance, false));
                    __instance.PlaySoundEventOnAllClonedScreens("play_ship_generic_internal_computer_ui_click");
                }
                else if (inButton.name.Contains("LogBtn") && !screenObjects.LogInfoPanel.gameObject.activeSelf)
                {
                    CrewLogManager.Instance.LogIndex = int.Parse(inButton.name.Remove(0, 6)) - 1 + (10 * CrewLogManager.Instance.ShowLogIndex);
                    CrewLogManager.Instance.TempData = CrewLogManager.Instance.GetLogs()[CrewLogManager.Instance.LogIndex];
                    screenObjects.LogInfoPanel.gameObject.SetActive(true);
                    screenObjects.LogInfoBoxLabel.text = "Log #" + (CrewLogManager.Instance.LogIndex + 1).ToString();
                    screenObjects.LogInfoBoxText.text = CrewLogManager.Instance.TempData.Text;
                    screenObjects.LogInfoBoxSectorButton.GetComponentInChildren<UILabel>().text = CrewLogManager.Instance.TempData.optionalSectorID.ToString();
                    screenObjects.LogInfoBoxSectorButton.gameObject.SetActive(false);
                    screenObjects.LogInfoSectorColor.color = CrewLogManager.Instance.TempData.optionalColor;
                    screenObjects.LogInfoBoxCreate.gameObject.SetActive(false);
                    screenObjects.LogInfoButtonDel.gameObject.SetActive(true);
                    __instance.StartCoroutine(CrewLogManager.Instance.ToggleLogButtons(__instance));
                    __instance.PlaySoundEventOnAllClonedScreens("play_ship_generic_internal_computer_ui_click");
                }
                else if (inButton.name == "LogInfoSectorBtn")
                {
                    if (CrewLogManager.Instance.TempData.optionalSectorID > -1)
                    {
                        if (PLGlobal.Instance.Galaxy != null && PLGlobal.Instance.Galaxy.AllSectorInfos.ContainsKey(CrewLogManager.Instance.TempData.optionalSectorID))
                        {
                            PLStarmap.Instance.OpenStarmapToSector(PLGlobal.Instance.Galaxy.AllSectorInfos[CrewLogManager.Instance.TempData.optionalSectorID]);
                            __instance.PlaySoundEventOnAllClonedScreens("play_ship_generic_internal_computer_ui_click");
                        }
                    }
                }
                else if (inButton.name.Contains("KeypadBtn_") && screenObjects.LogInfoPanel.gameObject.activeSelf)
                {
                    int keypadPressed = int.Parse(inButton.name.Remove(0, 10));
                    __instance.PlaySoundEventOnAllClonedScreens("play_ship_generic_internal_computer_ui_keypad");
                    string sectorString = CrewLogManager.Instance.GetObjectsForScreen(__instance).LogInfoBoxSectorButton.GetComponentInChildren<UILabel>().text;
                    if (sectorString.Length < 6 || keypadPressed == 3 || keypadPressed == 11)
                    {
                        switch (keypadPressed)
                        {
                            case 0:
                                sectorString += "1";
                                break;
                            case 1:
                                sectorString += "4";
                                break;
                            case 2:
                                sectorString += "7";
                                break;
                            case 3:
                                if (sectorString.Length > 1)
                                    sectorString = sectorString.Substring(0, sectorString.Length - 1);
                                else
                                    sectorString = "-1";
                                break;
                            case 4:
                                sectorString += "2";
                                break;
                            case 5:
                                sectorString += "5";
                                break;
                            case 6:
                                sectorString += "8";
                                break;
                            case 7:
                                sectorString += "0";
                                break;
                            case 8:
                                sectorString += "3";
                                break;
                            case 9:
                                sectorString += "6";
                                break;
                            case 10:
                                sectorString += "9";
                                break;
                            default:
                                if (PLServer.GetCurrentSector() != null)
                                    sectorString = PLServer.GetCurrentSector().ID.ToString();
                                break;
                        }
                    }
                    CrewLogManager.Instance.GetObjectsForScreen(__instance).LogInfoBoxSectorButton.GetComponentInChildren<UILabel>().text = sectorString != "-1" ? sectorString : string.Empty;
                    if (sectorString == "-1")
                        CrewLogManager.Instance.GetObjectsForScreen(__instance).LogInfoBoxSectorButton.gameObject.SetActive(false);
                    else
                        CrewLogManager.Instance.GetObjectsForScreen(__instance).LogInfoBoxSectorButton.gameObject.SetActive(true);
                    CrewLogData logData = CrewLogManager.Instance.TempData;
                    try
                    {
                        logData.optionalSectorID = int.Parse(sectorString);
                    }
                    catch
                    {
                        logData.optionalSectorID = -1;
                    }
                    CrewLogManager.Instance.TempData = logData;
                }
                else if (inButton.name == "NextBtn" && !screenObjects.LogInfoPanel.gameObject.activeSelf)
                {
                    ++CrewLogManager.Instance.ShowLogIndex;
                    __instance.PlaySoundEventOnAllClonedScreens("play_ship_generic_internal_computer_ui_keypad");
                }
                else if (inButton.name == "BackBtn" && !screenObjects.LogInfoPanel.gameObject.activeSelf)
                {
                    --CrewLogManager.Instance.ShowLogIndex;
                    __instance.PlaySoundEventOnAllClonedScreens("play_ship_generic_internal_computer_ui_keypad");
                }
            }
        }

        [HarmonyPatch(typeof(PLCaptainScreen), "Update")]
        internal class CaptainScreenUpdate
        {
            private static void Postfix(PLCaptainScreen __instance, ref UISprite ___StatusPanel, ref UISprite ___EnemyStatusPanel)
            {
                if (__instance.MyRootPanel == null || !__instance.LocalPlayerInSameLocation() || !CrewLogManager.Instance.HasScreenObjects(__instance))
                    return;
                if (__instance.MyScreenHubBase.OptionalShipInfo != null)
                {
                    if (!__instance.MyScreenHubBase.OptionalShipInfo.GetIsPlayerShip())
                    {
                        if (CrewLogManager.Instance.GetObjectsForScreen(__instance).LogInfoPanel.gameObject.activeSelf)
                            CrewLogManager.Instance.GetObjectsForScreen(__instance).LogInfoPanel.gameObject.SetActive(false);
                        if (CrewLogManager.Instance.GetObjectsForScreen(__instance).LogPanel.gameObject.activeSelf)
                        {
                            CrewLogManager.Instance.GetObjectsForScreen(__instance).LogPanel.gameObject.SetActive(false);
                            ___StatusPanel.gameObject.SetActive(true);
                            ___EnemyStatusPanel.gameObject.SetActive(true);
                        }
                    }
                }
                if (!CrewLogManager.Instance.GetObjectsForScreen(__instance).LogPanel.gameObject.activeSelf)
                    return;
                CrewLogScreenObjects screenObjects = CrewLogManager.Instance.GetObjectsForScreen(__instance);
                if (PLServer.GetCurrentSector() != null)
                    screenObjects.SectorLabel.text = "Sector: " + PLServer.GetCurrentSector().ID;
                else
                    screenObjects.SectorLabel.text = "Sector: N/A";
                if (PLServer.Instance != null)
                    screenObjects.TimeLabel.text = "Time: " + CrewLogManager.FormatPlaytime(PLServer.Instance.Playtime);
                int count = CrewLogManager.Instance.GetLogs().Count;
                for (int i = 0; i < 10; i++)
                {
                    int index = i + 10 * CrewLogManager.Instance.ShowLogIndex;
                    if (index < count)
                    {
                        screenObjects.LogButtons[i].GetComponentInChildren<UILabel>().text = CrewLogManager.FormatPlaytime(CrewLogManager.Instance.GetLogs()[index].timeStamp);
                        screenObjects.LogButtons[i].gameObject.SetActive(true);
                        screenObjects.LogColors[i].color = CrewLogManager.Instance.GetLogs()[index].optionalColor;
                        screenObjects.LogColors[i].gameObject.SetActive(true);

                    }
                    else
                    {
                        screenObjects.LogButtons[i].gameObject.SetActive(false);
                        screenObjects.LogColors[i].gameObject.SetActive(false);
                    }
                }
                if (count > 10 + CrewLogManager.Instance.ShowLogIndex * 10)
                {
                    screenObjects.NextButton.gameObject.SetActive(true);
                }
                else
                {
                    screenObjects.NextButton.gameObject.SetActive(false);
                    if (count < CrewLogManager.Instance.ShowLogIndex * 10)
                        --CrewLogManager.Instance.ShowLogIndex;
                }
                if (CrewLogManager.Instance.ShowLogIndex > 0)
                    screenObjects.BackButton.gameObject.SetActive(true);
                else
                    screenObjects.BackButton.gameObject.SetActive(false);
                if (CrewLogManager.Instance.GetObjectsForScreen(__instance).LogInfoPanel.gameObject.activeSelf)
                {
                    if (CrewLogManager.Instance.TempData.optionalSectorID != -1)
                        screenObjects.LogInfoBoxSectorButton.gameObject.SetActive(true);
                    if (CrewLogManager.Instance.LogIndex == int.MinValue)
                        screenObjects.LogInfoBoxText.text = CrewLogManager.Instance.TempData.Text;
                }
            }
        }

        internal class ClientSendLog : ModMessage
        {
            public override void HandleRPC(object[] arguments, PhotonMessageInfo sender)
            {
                CrewLogData logData = new CrewLogData()
                {
                    Text = (string)arguments[0],
                    timeStamp = (float)arguments[1],
                    optionalSectorID = (int)arguments[2],
                    optionalColor = new Color((float)arguments[3], (float)arguments[4], (float)arguments[5], (float)arguments[6])
                };
                CrewLogManager.Instance.AddLog(logData);
                PLPlayer player = Systems.GetPlayerFromPhotonPlayer(sender.sender);
                if (player != null)
                    PLServer.Instance.AddNotificationLocalize("[PL] added a crew log", player.GetPlayerID(), (PLServer.Instance.GetEstimatedServerMs() + 6000), false);

                List<object> sendArgumentList = new List<object>();
                int logCount = CrewLogManager.Instance.GetLogs().Count;
                sendArgumentList.Add(logCount);
                foreach (CrewLogData sendLogData in CrewLogManager.Instance.GetLogs())
                {
                    sendArgumentList.Add(sendLogData.Text);
                    sendArgumentList.Add(sendLogData.timeStamp);
                    sendArgumentList.Add(sendLogData.optionalSectorID);
                    sendArgumentList.Add(sendLogData.optionalColor.r);
                    sendArgumentList.Add(sendLogData.optionalColor.g);
                    sendArgumentList.Add(sendLogData.optionalColor.b);
                    sendArgumentList.Add(sendLogData.optionalColor.a);
                }
                ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.ServerSendLog", PhotonTargets.Others, sendArgumentList.ToArray());
            }
        }

        internal class ServerSendLog : ModMessage
        {
            public override void HandleRPC(object[] arguments, PhotonMessageInfo sender)
            {
                int logCount = (int)arguments[0];
                CrewLogManager.Instance.ClearLogs();
                int index = 1;
                for (int i = 0; i < logCount; i++)
                {
                    CrewLogData logData = new CrewLogData()
                    {
                        Text = (string)arguments[index++],
                        timeStamp = (float)arguments[index++],
                        optionalSectorID = (int)arguments[index++],
                        optionalColor = new Color((float)arguments[index++], (float)arguments[index++], (float)arguments[index++], (float)arguments[index++])
                    };
                    CrewLogManager.Instance.AddLog(logData);
                }
            }
        }

        [HarmonyPatch(typeof(PLServer), "SpawnNewPlayer")]
        internal class NewPlayerSendLogs
        {
            private static void Postfix(PLServer __instance, PhotonPlayer newPhotonPlayer, string inPlayerName)
            {
                if (!newPhotonPlayer.IsMasterClient)
                {
                    List<object> sendArgumentList = new List<object>();
                    int logCount = CrewLogManager.Instance.GetLogs().Count;
                    sendArgumentList.Add(logCount);
                    foreach (CrewLogData sendLogData in CrewLogManager.Instance.GetLogs())
                    {
                        sendArgumentList.Add(sendLogData.Text);
                        sendArgumentList.Add(sendLogData.timeStamp);
                        sendArgumentList.Add(sendLogData.optionalSectorID);
                        sendArgumentList.Add(sendLogData.optionalColor.r);
                        sendArgumentList.Add(sendLogData.optionalColor.g);
                        sendArgumentList.Add(sendLogData.optionalColor.b);
                        sendArgumentList.Add(sendLogData.optionalColor.a);
                    }
                    ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.ServerSendLog", newPhotonPlayer, sendArgumentList.ToArray());
                }
            }
        }
    }
}
