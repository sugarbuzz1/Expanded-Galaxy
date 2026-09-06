using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace ExpandedGalaxy
{
    public class CrewLogManager
    {
        private static CrewLogManager m_instance;
        private Dictionary<PLCaptainScreen, CrewLogScreenObjects> m_screenobjects;
        private List<CrewLogData> m_logs;
        private int showlogindex;
        private int logindex;
        private CrewLogData tempData;
        private Dictionary<int, List<MapPin>> m_mappins;
        private static Vector3 vector3 = Vector3.zero;

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
            m_mappins = new Dictionary<int, List<MapPin>>();
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
            ClearLogs();
            showlogindex = 0;
            tempData = new CrewLogData()
            {
                timeStamp = 0f,
                optionalColor = Color.black,
                Text = string.Empty,
                optionalSectorID = -1,
                specialData = -1
            };
            foreach (int key in m_mappins.Keys)
            {
                foreach (MapPin pin in m_mappins[key])
                    UnityEngine.Object.Destroy(pin.PinObject);
                m_mappins[key].Clear();
            }
            m_mappins.Clear();

        }

        public void ClearLogs()
        {
            while (m_logs.Count > 0)
                RemoveLog(0);
            logindex = -1;
            showlogindex = 0;
        }

        public Dictionary<int, List<MapPin>> MapPins
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

            CrewLogScreenObjects screenObjects = new CrewLogScreenObjects();

            screenObjects.LogButtons = new List<UISprite>();
            screenObjects.LogColors = new List<UITexture>();
            screenObjects.LogInfoKeypadButtons = new List<UITexture>();
            screenObjects.LogInfoKeypadLabels = new List<UILabel>();
            screenObjects.SpecialLogObjects = new List<UIWidget>();

            Transform statusPanel = captainScreen.StatusPanel.cachedTransform;

            screenObjects.CrewLogButton = captainScreen.CreateButtonEditable("CLBtn", PLGlobal.Instance.TriangleIcon, new Vector3(450f, -45f), new Vector2(32f, 32f), captainScreen.UI_White, statusPanel, UIWidget.Pivot.TopRight);
            screenObjects.CrewLogButton.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);

            captainScreen.CreateLabel("CREW LOGS", new Vector3(235f, -60f), 15, captainScreen.UI_White, statusPanel, UIWidget.Pivot.Left);

            screenObjects.LogPanel = captainScreen.CreatePanel("Crew Logs", new Vector3(0f, 0f, 0f), new Vector2(512f, 512f), captainScreen.UI_White, null, UIWidget.Pivot.TopLeft);
            screenObjects.LogPanel.gameObject.SetActive(false);
            Transform LogPanelTransform = screenObjects.LogPanel.cachedTransform;

            screenObjects.StatusButton = captainScreen.CreateButtonEditable("StatusBtn", PLGlobal.Instance.TriangleIcon, new Vector3(450f, -45f), new Vector2(32f, 32f), captainScreen.UI_White, LogPanelTransform, UIWidget.Pivot.TopRight);
            screenObjects.StatusButton.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);

            captainScreen.CreateSprite(captainScreen.MyScreenHubBase.ScreenThemeAtlas, "small_button", new Vector3(32f, -120f), new Vector2(360f, 40f), new Color(0.1f, 0.1f, 0.1f, 0.95f), LogPanelTransform, UIWidget.Pivot.TopLeft);

            captainScreen.CreateLabel("Logs", new Vector3(54f, -140f), 15, captainScreen.UI_White, LogPanelTransform, UIWidget.Pivot.Left);

            screenObjects.SectorLabel = captainScreen.CreateLabel("Sector: 0", new Vector3(32f, -60f), 17, captainScreen.UI_White, LogPanelTransform, UIWidget.Pivot.Left);
            screenObjects.TimeLabel = captainScreen.CreateLabel("Time: 00:00:00", new Vector3(32f, -96f), 17, captainScreen.UI_White, LogPanelTransform, UIWidget.Pivot.Left);

            float x = 32f;
            float y = -164f;

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    int num = 1 + (5 * i) + j;

                    UISprite btn = captainScreen.CreateButton("LogBtn" + num.ToString(), "00:00:00", new Vector3(x, y), new Vector2(170f, 60f), captainScreen.UI_White, LogPanelTransform, UIWidget.Pivot.TopLeft);
                    btn.gameObject.SetActive(false);
                    screenObjects.LogButtons.Add(btn);

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
                    UITexture tex = captainScreen.CreateTexture(PLGlobal.Instance.WhitePixel, new Vector3(x, y), new Vector2(16f, 16f), Color.white, LogPanelTransform, UIWidget.Pivot.TopLeft);
                    tex.gameObject.SetActive(false);
                    screenObjects.LogColors.Add(tex);

                    y -= 62f;
                }

                x += 190f;
                y = -186f;
            }

            screenObjects.BackButton = captainScreen.CreateButton("BackBtn", "<<", new Vector3(432f, -422f), new Vector2(48f, 48f), captainScreen.UI_White, LogPanelTransform, UIWidget.Pivot.TopLeft);
            screenObjects.NextButton = captainScreen.CreateButton("NextBtn", ">>", new Vector3(432f, -360f), new Vector2(48f, 48f), captainScreen.UI_White, LogPanelTransform, UIWidget.Pivot.TopLeft);
            screenObjects.NewLogButton = captainScreen.CreateButton("NewBtn", "New", new Vector3(294f, -50f), new Vector2(98f, 60f), captainScreen.UI_White, LogPanelTransform, UIWidget.Pivot.TopLeft);

            screenObjects.LogInfoPanel = captainScreen.CreateBlankPanel("LogInfoBox", new Vector3(0f, 0f), new Vector2(512f, 512f), null, UIWidget.Pivot.TopLeft);
            screenObjects.LogInfoPanel.depth += 10000;
            screenObjects.LogInfoPanel.gameObject.SetActive(false);

            screenObjects.LogInfoBox = captainScreen.CreateSprite(captainScreen.MyScreenHubBase.ScreenThemeAtlas, "small_button", Vector3.zero, new Vector2(360f, 400f), captainScreen.UI_White, screenObjects.LogInfoPanel.cachedTransform, UIWidget.Pivot.Center);
            screenObjects.LogInfoBox.depth += 10000;
            Transform LogInfoBoxTransform = screenObjects.LogInfoBox.cachedTransform;

            screenObjects.LogInfoBoxLabel = captainScreen.CreateLabel(string.Empty, new Vector3(-164f, 184f), 14, captainScreen.UI_White, LogInfoBoxTransform, UIWidget.Pivot.TopLeft);
            screenObjects.LogInfoBoxLabel.depth += 10000;

            screenObjects.LogInfoBoxText = captainScreen.CreateParagraph(string.Empty, new Vector3(-164f, 128f), 12, 720, 1200, captainScreen.UI_White, LogInfoBoxTransform, UIWidget.Pivot.TopLeft);
            screenObjects.LogInfoBoxText.depth += 10000;
            screenObjects.LogInfoBoxText.overflowMethod = UILabel.Overflow.ClampContent;

            screenObjects.LogInfoBoxClose = captainScreen.CreateButton("LogInfoCloseBtn", "X", new Vector3(114f, 184f), new Vector2(48f, 48f), captainScreen.UI_White, LogInfoBoxTransform, UIWidget.Pivot.TopLeft);
            screenObjects.LogInfoBoxClose.depth += 10000;
            screenObjects.LogInfoBoxClose.GetComponentInChildren<UILabel>().depth += 10000;

            screenObjects.LogInfoBoxSectorButton = captainScreen.CreateButton("LogInfoSectorBtn", "0000", new Vector3(-40f, 184f), new Vector2(144f, 48f), captainScreen.UI_White, LogInfoBoxTransform, UIWidget.Pivot.TopLeft);
            screenObjects.LogInfoBoxSectorButton.depth += 10000;
            screenObjects.LogInfoBoxSectorButton.GetComponentInChildren<UILabel>().depth += 10000;

            screenObjects.LogInfoBoxCreate = captainScreen.CreateButton("LogInfoCreateBtn", "Create", new Vector3(20f, -134f), new Vector2(144f, 48f), captainScreen.UI_White, LogInfoBoxTransform, UIWidget.Pivot.TopLeft);
            screenObjects.LogInfoBoxCreate.depth += 10000;
            screenObjects.LogInfoBoxCreate.GetComponentInChildren<UILabel>().depth += 10000;

            screenObjects.LogInfoButtonDel = captainScreen.CreateButton("LogInfoDelBtn", "Del", new Vector3(114f, -134f), new Vector2(48f, 48f), captainScreen.UI_White, LogInfoBoxTransform, UIWidget.Pivot.TopLeft);
            screenObjects.LogInfoButtonDel.depth += 10000;
            screenObjects.LogInfoButtonDel.GetComponentInChildren<UILabel>().depth += 10000;

            screenObjects.LogInfoSectorColor = captainScreen.CreateTexture(PLGlobal.Instance.WhitePixel, new Vector3(-164f, 156f), new Vector2(98f, 16f), Color.white, LogInfoBoxTransform, UIWidget.Pivot.TopLeft);
            screenObjects.LogInfoSectorColor.depth += 10000;

            screenObjects.LogInfoBoxWrite = captainScreen.CreateButton("LogInfoWriteBtn", ">_", new Vector3(44f, 28f), new Vector2(52f, 48f), captainScreen.UI_White, LogInfoBoxTransform, UIWidget.Pivot.TopLeft);
            screenObjects.LogInfoBoxWrite.depth += 10000;
            screenObjects.LogInfoBoxWrite.GetComponentInChildren<UILabel>().depth += 10000;

            x = 44f;
            y = 100f;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    UITexture btn = captainScreen.CreateButtonEditable("KeypadBtn_" + (i * 4 + j).ToString(), PLGlobal.Instance.WhitePixel, new Vector3(x, y), new Vector2(16f, 16f), captainScreen.UI_White, LogInfoBoxTransform, UIWidget.Pivot.TopLeft);
                    btn.depth += 10000;
                    screenObjects.LogInfoKeypadButtons.Add(btn);
                    screenObjects.LogInfoKeypadButtons[screenObjects.LogInfoKeypadButtons.Count - 1].depth += 10000;

                    string str = "";
                    switch (i)
                    {
                        case 0:
                            switch (j)
                            {
                                case 0:
                                    str = "1";
                                    break;
                                case 1:
                                    str = "4";
                                    break;
                                case 2:
                                    str = "7";
                                    break;
                                case 3:
                                    str = "<-";
                                    break;
                            }
                            break;
                        case 1:
                            switch (j)
                            {
                                case 0:
                                    str = "2";
                                    break;
                                case 1:
                                    str = "5";
                                    break;
                                case 2:
                                    str = "8";
                                    break;
                                case 3:
                                    str = "0";
                                    break;
                            }
                            break;
                        case 2:
                            switch (j)
                            {
                                case 0:
                                    str = "3";
                                    break;
                                case 1:
                                    str = "6";
                                    break;
                                case 2:
                                    str = "9";
                                    break;
                                case 3:
                                    str = "C";
                                    break;
                            }
                            break;
                    }
                    screenObjects.LogInfoKeypadLabels.Add(captainScreen.CreateLabel(str, new Vector3(x + 6f, y - 2f), 8, Color.black, LogInfoBoxTransform, UIWidget.Pivot.TopLeft));
                    screenObjects.LogInfoKeypadLabels[screenObjects.LogInfoKeypadLabels.Count - 1].depth += 10000;

                    y -= 18f;
                }

                x += 18f;
                y = 100f;
            }

            m_screenobjects.Add(captainScreen, screenObjects);
        }

        public void SetupSpecialLog(PLCaptainScreen captainScreen, int specialLogIndex)
        {
            if (!m_screenobjects.ContainsKey(captainScreen))
                return;
            CrewLogScreenObjects screenObjects = GetObjectsForScreen(captainScreen);
            screenObjects.LogInfoButtonDel.gameObject.SetActive(SpecialLogCanBeDeleted(specialLogIndex));
            screenObjects.LogInfoBoxText.text = string.Empty;
            float x;
            float y;
            if (specialLogIndex == 0)
            {
                int decrypted = (ReflectedRift.GetRiftData(1) ? 1 : 0) + (ReflectedRift.GetRiftData(2) ? 1 : 0) + (ReflectedRift.GetRiftData(3) ? 1 : 0);

                PLRand rand;

                for (int i = 0; i < 2; i++)
                {
                    rand = new PLRand((int)PLServer.Instance.GalaxySeed);

                    x = 1.5f;
                    y = 87f;

                    Texture2D icon = PLGlobal.Instance.TriangleIcon;
                    Vector2 size = (i == 0) ? new Vector2(50f, 50f) : new Vector2(48f, 48f);
                    Color color = (i == 0) ? Color.white : Color.black;

                    for (int j = 0; j < 9; j++)
                    {
                        if (j % 2 == 0)
                        {
                            if (decrypted > rand.Next(3))
                            {
                                screenObjects.SpecialLogObjects.Add(captainScreen.CreateTexture(icon, new Vector3(x, y), size, color, screenObjects.LogInfoBox.transform, UIWidget.Pivot.Center));
                                screenObjects.SpecialLogObjects[screenObjects.SpecialLogObjects.Count - 1].depth += 10000;
                                screenObjects.SpecialLogObjects[screenObjects.SpecialLogObjects.Count - 1].transform.Rotate(new Vector3(0f, 0f, 180f));
                            }

                            x -= 1.5f;
                            y -= 37f;
                        }
                        else
                        {
                            if (decrypted > rand.Next(3))
                            {
                                screenObjects.SpecialLogObjects.Add(captainScreen.CreateTexture(icon, new Vector3(x, y), size, color, screenObjects.LogInfoBox.transform, UIWidget.Pivot.Center));
                                screenObjects.SpecialLogObjects[screenObjects.SpecialLogObjects.Count - 1].depth += 10000;
                            }

                            x -= 19f;
                            y += 1f;
                        }
                    }

                    x = 22f;
                    y = 51f;

                    for (int j = 0; j < 7; j++)
                    {
                        if (j % 2 == 0)
                        {
                            if (decrypted > rand.Next(3))
                            {
                                screenObjects.SpecialLogObjects.Add(captainScreen.CreateTexture(icon, new Vector3(x, y), size, color, screenObjects.LogInfoBox.transform, UIWidget.Pivot.Center));
                                screenObjects.SpecialLogObjects[screenObjects.SpecialLogObjects.Count - 1].depth += 10000;
                                screenObjects.SpecialLogObjects[screenObjects.SpecialLogObjects.Count - 1].transform.Rotate(new Vector3(0f, 0f, 180f));
                            }

                            x -= 1.5f;
                            y -= 37f;
                        }
                        else
                        {
                            if (decrypted > rand.Next(3) || j == 3)
                            {
                                screenObjects.SpecialLogObjects.Add(captainScreen.CreateTexture(icon, new Vector3(x, y), size, color, screenObjects.LogInfoBox.transform, UIWidget.Pivot.Center));
                                screenObjects.SpecialLogObjects[screenObjects.SpecialLogObjects.Count - 1].depth += 10000;
                            }

                            x -= 19f;
                            y += 1f;
                        }
                    }

                    x = 42.5f;
                    y = 15f;

                    for (int j = 0; j < 5; j++)
                    {
                        if (j % 2 == 0)
                        {
                            if (decrypted > rand.Next(3))
                            {
                                screenObjects.SpecialLogObjects.Add(captainScreen.CreateTexture(icon, new Vector3(x, y), size, color, screenObjects.LogInfoBox.transform, UIWidget.Pivot.Center));
                                screenObjects.SpecialLogObjects[screenObjects.SpecialLogObjects.Count - 1].depth += 10000;
                                screenObjects.SpecialLogObjects[screenObjects.SpecialLogObjects.Count - 1].transform.Rotate(new Vector3(0f, 0f, 180f));
                            }

                            x -= 1.5f;
                            y -= 37f;
                        }
                        else
                        {
                            if (decrypted > rand.Next(3))
                            {
                                screenObjects.SpecialLogObjects.Add(captainScreen.CreateTexture(icon, new Vector3(x, y), size, color, screenObjects.LogInfoBox.transform, UIWidget.Pivot.Center));
                                screenObjects.SpecialLogObjects[screenObjects.SpecialLogObjects.Count - 1].depth += 10000;
                            }

                            x -= 19f;
                            y += 1f;
                        }
                    }

                    x = 63f;
                    y = -21f;

                    for (int j = 0; j < 3; j++)
                    {
                        if (j % 2 == 0)
                        {
                            if (decrypted > rand.Next(3))
                            {
                                screenObjects.SpecialLogObjects.Add(captainScreen.CreateTexture(icon, new Vector3(x, y), size, color, screenObjects.LogInfoBox.transform, UIWidget.Pivot.Center));
                                screenObjects.SpecialLogObjects[screenObjects.SpecialLogObjects.Count - 1].depth += 10000;
                                screenObjects.SpecialLogObjects[screenObjects.SpecialLogObjects.Count - 1].transform.Rotate(new Vector3(0f, 0f, 180f));
                            }

                            x -= 1.5f;
                            y -= 37f;
                        }
                        else
                        {
                            if (decrypted > rand.Next(3))
                            {
                                screenObjects.SpecialLogObjects.Add(captainScreen.CreateTexture(icon, new Vector3(x, y), size, color, screenObjects.LogInfoBox.transform, UIWidget.Pivot.Center));
                                screenObjects.SpecialLogObjects[screenObjects.SpecialLogObjects.Count - 1].depth += 10000;
                            }

                            x -= 19f;
                            y += 1f;
                        }
                    }

                    x = 83.5f;
                    y = -57f;

                    if (decrypted > rand.Next(3))
                    {
                        screenObjects.SpecialLogObjects.Add(captainScreen.CreateTexture(icon, new Vector3(x, y), size, color, screenObjects.LogInfoBox.transform, UIWidget.Pivot.Center));
                        screenObjects.SpecialLogObjects[screenObjects.SpecialLogObjects.Count - 1].depth += 10000;
                        screenObjects.SpecialLogObjects[screenObjects.SpecialLogObjects.Count - 1].transform.Rotate(new Vector3(0f, 0f, 180f));
                    }
                }

                x = 1.5f;
                y = 80f;

                int a = 0;

                rand = new PLRand((int)PLServer.Instance.GalaxySeed);

                for (int j = 0; j < 9; j++)
                {
                    a++;

                    if (decrypted > rand.Next(3))
                    {
                        screenObjects.SpecialLogObjects.Add(captainScreen.CreateLabel(StargatePuzzle.SubLetters[a].ToString(), new Vector3(x, y), 12, Color.white, screenObjects.LogInfoBox.transform, UIWidget.Pivot.Center));
                        screenObjects.SpecialLogObjects[screenObjects.SpecialLogObjects.Count - 1].depth += 10000;
                    }

                    if (j % 2 == 0)
                        y -= 27.5f;
                    else
                    {
                        x -= 20.5f;
                        y -= 7.5f;
                    }
                }

                x = 22f;
                y = 44f;

                for (int j = 0; j < 7; j++)
                {
                    a++;

                    if (decrypted > rand.Next(3) || j == 3)
                    {
                        screenObjects.SpecialLogObjects.Add(captainScreen.CreateLabel(StargatePuzzle.SubLetters[a].ToString(), new Vector3(x, y), 12, Color.white, screenObjects.LogInfoBox.transform, UIWidget.Pivot.Center));
                        screenObjects.SpecialLogObjects[screenObjects.SpecialLogObjects.Count - 1].depth += 10000;
                    }

                    if (j % 2 == 0)
                        y -= 27.5f;
                    else
                    {
                        x -= 20.5f;
                        y -= 7.5f;
                    }
                }

                x = 42.5f;
                y = 8f;

                for (int j = 0; j < 5; j++)
                {
                    a++;

                    if (decrypted > rand.Next(3))
                    {
                        screenObjects.SpecialLogObjects.Add(captainScreen.CreateLabel(StargatePuzzle.SubLetters[a].ToString(), new Vector3(x, y), 12, Color.white, screenObjects.LogInfoBox.transform, UIWidget.Pivot.Center));
                        screenObjects.SpecialLogObjects[screenObjects.SpecialLogObjects.Count - 1].depth += 10000;
                    }

                    if (j % 2 == 0)
                        y -= 27.5f;
                    else
                    {
                        x -= 20.5f;
                        y -= 7.5f;
                    }
                }

                x = 63f;
                y = -28f;

                for (int j = 0; j < 3; j++)
                {
                    a++;

                    if (decrypted > rand.Next(3))
                    {
                        screenObjects.SpecialLogObjects.Add(captainScreen.CreateLabel(StargatePuzzle.SubLetters[a].ToString(), new Vector3(x, y), 12, Color.white, screenObjects.LogInfoBox.transform, UIWidget.Pivot.Center));
                        screenObjects.SpecialLogObjects[screenObjects.SpecialLogObjects.Count - 1].depth += 10000;
                    }

                    if (j % 2 == 0)
                        y -= 27.5f;
                    else
                    {
                        x -= 20.5f;
                        y -= 7.5f;
                    }
                }

                x = 83.5f;
                y = -64f;

                if (decrypted > rand.Next(3))
                {
                    screenObjects.SpecialLogObjects.Add(captainScreen.CreateLabel(StargatePuzzle.SubLetters[a].ToString(), new Vector3(x, y), 12, Color.white, screenObjects.LogInfoBox.transform, UIWidget.Pivot.Center));
                    screenObjects.SpecialLogObjects[screenObjects.SpecialLogObjects.Count - 1].depth += 10000;
                }
            }
            else if (specialLogIndex == 1)
            {
                int decrypted =
                    (ReflectedRift.GetRiftData(4) ? 1 : 0) +
                    (ReflectedRift.GetRiftData(5) ? 1 : 0);

                PLRand rand;

                for (int i = 0; i < 2; i++)
                {
                    rand = new PLRand((int)PLServer.Instance.GalaxySeed);

                    x = 0f;
                    y = -22f;

                    Texture2D icon = PLGlobal.Instance.TriangleIcon;
                    Vector2 size = (i == 0) ? new Vector2(50f, 50f) : new Vector2(48f, 48f);
                    Color color = (i == 0) ? Color.white : Color.black;

                    screenObjects.SpecialLogObjects.Add(captainScreen.CreateTexture(icon, new Vector3(x, y), size, color, screenObjects.LogInfoBox.transform, UIWidget.Pivot.Center));
                    screenObjects.SpecialLogObjects[screenObjects.SpecialLogObjects.Count - 1].depth += 10000;

                    x += 1.5f;
                    y += 37f;

                    if (decrypted > rand.Next(2))
                    {
                        screenObjects.SpecialLogObjects.Add(captainScreen.CreateTexture(icon, new Vector3(x, y), size, color, screenObjects.LogInfoBox.transform, UIWidget.Pivot.Center));
                        screenObjects.SpecialLogObjects[screenObjects.SpecialLogObjects.Count - 1].depth += 10000;
                        screenObjects.SpecialLogObjects[screenObjects.SpecialLogObjects.Count - 1].transform.Rotate(new Vector3(0f, 0f, 180f));
                    }

                    x += 19f;
                    y -= 1f;

                    if (decrypted > rand.Next(2))
                    {
                        screenObjects.SpecialLogObjects.Add(captainScreen.CreateTexture(icon, new Vector3(x, y), size, color, screenObjects.LogInfoBox.transform, UIWidget.Pivot.Center));
                        screenObjects.SpecialLogObjects[screenObjects.SpecialLogObjects.Count - 1].depth += 10000;
                    }

                    x += 22f;
                    y += 1f;

                    if (decrypted > rand.Next(2))
                    {
                        screenObjects.SpecialLogObjects.Add(captainScreen.CreateTexture(icon, new Vector3(x, y), size, color, screenObjects.LogInfoBox.transform, UIWidget.Pivot.Center));
                        screenObjects.SpecialLogObjects[screenObjects.SpecialLogObjects.Count - 1].depth += 10000;
                        screenObjects.SpecialLogObjects[screenObjects.SpecialLogObjects.Count - 1].transform.Rotate(new Vector3(0f, 0f, 180f));
                    }

                    x += 19f;
                    y -= 1f;

                    if (decrypted > rand.Next(2))
                    {
                        screenObjects.SpecialLogObjects.Add(captainScreen.CreateTexture(icon, new Vector3(x, y), size, color, screenObjects.LogInfoBox.transform, UIWidget.Pivot.Center));
                        screenObjects.SpecialLogObjects[screenObjects.SpecialLogObjects.Count - 1].depth += 10000;
                    }

                    x += 1.5f;
                    y += 37f;

                    if (decrypted > rand.Next(2))
                    {
                        screenObjects.SpecialLogObjects.Add(captainScreen.CreateTexture(icon, new Vector3(x, y), size, color, screenObjects.LogInfoBox.transform, UIWidget.Pivot.Center));
                        screenObjects.SpecialLogObjects[screenObjects.SpecialLogObjects.Count - 1].depth += 10000;
                        screenObjects.SpecialLogObjects[screenObjects.SpecialLogObjects.Count - 1].transform.Rotate(new Vector3(0f, 0f, 180f));
                    }

                    x -= 22f;
                    y -= 1f;

                    if (decrypted > rand.Next(2))
                    {
                        screenObjects.SpecialLogObjects.Add(captainScreen.CreateTexture(icon, new Vector3(x, y), size, color, screenObjects.LogInfoBox.transform, UIWidget.Pivot.Center));
                        screenObjects.SpecialLogObjects[screenObjects.SpecialLogObjects.Count - 1].depth += 10000;
                    }

                    x = 0f;
                    y = -22f;

                    x -= 19f;
                    y += 1f;

                    if (decrypted > rand.Next(2))
                    {
                        screenObjects.SpecialLogObjects.Add(captainScreen.CreateTexture(icon, new Vector3(x, y), size, color, screenObjects.LogInfoBox.transform, UIWidget.Pivot.Center));
                        screenObjects.SpecialLogObjects[screenObjects.SpecialLogObjects.Count - 1].depth += 10000;
                        screenObjects.SpecialLogObjects[screenObjects.SpecialLogObjects.Count - 1].transform.Rotate(new Vector3(0f, 0f, 180f));
                    }

                    x -= 22f;
                    y -= 1f;

                    if (decrypted > rand.Next(2))
                    {
                        screenObjects.SpecialLogObjects.Add(captainScreen.CreateTexture(icon, new Vector3(x, y), size, color, screenObjects.LogInfoBox.transform, UIWidget.Pivot.Center));
                        screenObjects.SpecialLogObjects[screenObjects.SpecialLogObjects.Count - 1].depth += 10000;
                    }

                    x += 1.5f;
                    y += 37f;

                    if (decrypted > rand.Next(2))
                    {
                        screenObjects.SpecialLogObjects.Add(captainScreen.CreateTexture(icon, new Vector3(x, y), size, color, screenObjects.LogInfoBox.transform, UIWidget.Pivot.Center));
                        screenObjects.SpecialLogObjects[screenObjects.SpecialLogObjects.Count - 1].depth += 10000;
                        screenObjects.SpecialLogObjects[screenObjects.SpecialLogObjects.Count - 1].transform.Rotate(new Vector3(0f, 0f, 180f));
                    }

                    x -= 22f;
                    y -= 1f;

                    if (decrypted > rand.Next(2))
                    {
                        screenObjects.SpecialLogObjects.Add(captainScreen.CreateTexture(icon, new Vector3(x, y), size, color, screenObjects.LogInfoBox.transform, UIWidget.Pivot.Center));
                        screenObjects.SpecialLogObjects[screenObjects.SpecialLogObjects.Count - 1].depth += 10000;
                    }

                    x -= 19f;
                    y += 1f;

                    if (decrypted > rand.Next(2))
                    {
                        screenObjects.SpecialLogObjects.Add(captainScreen.CreateTexture(icon, new Vector3(x, y), size, color, screenObjects.LogInfoBox.transform, UIWidget.Pivot.Center));
                        screenObjects.SpecialLogObjects[screenObjects.SpecialLogObjects.Count - 1].depth += 10000;
                        screenObjects.SpecialLogObjects[screenObjects.SpecialLogObjects.Count - 1].transform.Rotate(new Vector3(0f, 0f, 180f));
                    }

                    x -= 1.5f;
                    y -= 37f;

                    if (decrypted > rand.Next(2))
                    {
                        screenObjects.SpecialLogObjects.Add(captainScreen.CreateTexture(icon, new Vector3(x, y), size, color, screenObjects.LogInfoBox.transform, UIWidget.Pivot.Center));
                        screenObjects.SpecialLogObjects[screenObjects.SpecialLogObjects.Count - 1].depth += 10000;
                    }
                }

                rand = new PLRand((int)PLServer.Instance.GalaxySeed);
                x = 0f;
                y = -22f;

                screenObjects.SpecialLogObjects.Add(captainScreen.CreateLabel(StargatePuzzle.SubLetters[12].ToString(), new Vector3(x + 1.5f, y + 2.5f), 12, Color.white, screenObjects.LogInfoBox.transform, UIWidget.Pivot.Center));
                screenObjects.SpecialLogObjects[screenObjects.SpecialLogObjects.Count - 1].depth += 10000;

                x += 1.5f;
                y += 37f;

                if (decrypted > rand.Next(2))
                {
                    screenObjects.SpecialLogObjects.Add(captainScreen.CreateLabel(StargatePuzzle.SubLetters[10].ToString(), new Vector3(x, y - 7f), 12, Color.white, screenObjects.LogInfoBox.transform, UIWidget.Pivot.Center));
                    screenObjects.SpecialLogObjects[screenObjects.SpecialLogObjects.Count - 1].depth += 10000;
                }

                x += 19f;
                y -= 1f;

                if (decrypted > rand.Next(2))
                {
                    screenObjects.SpecialLogObjects.Add(captainScreen.CreateLabel(StargatePuzzle.SubLetters[2].ToString(), new Vector3(x + 1.5f, y + 2.5f), 12, Color.white, screenObjects.LogInfoBox.transform, UIWidget.Pivot.Center));
                    screenObjects.SpecialLogObjects[screenObjects.SpecialLogObjects.Count - 1].depth += 10000;
                }

                x += 22f;
                y += 1f;

                if (decrypted > rand.Next(2))
                {
                    screenObjects.SpecialLogObjects.Add(captainScreen.CreateLabel(StargatePuzzle.SubLetters[4].ToString(), new Vector3(x, y - 7f), 12, Color.white, screenObjects.LogInfoBox.transform, UIWidget.Pivot.Center));
                    screenObjects.SpecialLogObjects[screenObjects.SpecialLogObjects.Count - 1].depth += 10000;
                }

                x += 19f;
                y -= 1f;

                if (decrypted > rand.Next(2))
                {
                    screenObjects.SpecialLogObjects.Add(captainScreen.CreateLabel(StargatePuzzle.SubLetters[1].ToString(), new Vector3(x + 1.5f, y + 2.5f), 12, Color.white, screenObjects.LogInfoBox.transform, UIWidget.Pivot.Center));
                    screenObjects.SpecialLogObjects[screenObjects.SpecialLogObjects.Count - 1].depth += 10000;
                }

                x += 1.5f;
                y += 37f;

                if (decrypted > rand.Next(2))
                {
                    screenObjects.SpecialLogObjects.Add(captainScreen.CreateLabel(StargatePuzzle.SubLetters[0].ToString(), new Vector3(x, y - 7f), 12, Color.white, screenObjects.LogInfoBox.transform, UIWidget.Pivot.Center));
                    screenObjects.SpecialLogObjects[screenObjects.SpecialLogObjects.Count - 1].depth += 10000;
                }

                x -= 22f;
                y -= 1f;

                if (decrypted > rand.Next(2))
                {
                    screenObjects.SpecialLogObjects.Add(captainScreen.CreateLabel(StargatePuzzle.SubLetters[3].ToString(), new Vector3(x + 1.5f, y + 2.5f), 12, Color.white, screenObjects.LogInfoBox.transform, UIWidget.Pivot.Center));
                    screenObjects.SpecialLogObjects[screenObjects.SpecialLogObjects.Count - 1].depth += 10000;
                }

                x = 0f;
                y = -22f;

                x -= 19f;
                y += 1f;

                if (decrypted > rand.Next(2))
                {
                    screenObjects.SpecialLogObjects.Add(captainScreen.CreateLabel(StargatePuzzle.SubLetters[9].ToString(), new Vector3(x, y - 7f), 12, Color.white, screenObjects.LogInfoBox.transform, UIWidget.Pivot.Center));
                    screenObjects.SpecialLogObjects[screenObjects.SpecialLogObjects.Count - 1].depth += 10000;
                }

                x -= 22f;
                y -= 1f;

                if (decrypted > rand.Next(2))
                {
                    screenObjects.SpecialLogObjects.Add(captainScreen.CreateLabel(StargatePuzzle.SubLetters[8].ToString(), new Vector3(x + 1.5f, y + 2.5f), 12, Color.white, screenObjects.LogInfoBox.transform, UIWidget.Pivot.Center));
                    screenObjects.SpecialLogObjects[screenObjects.SpecialLogObjects.Count - 1].depth += 10000;
                }

                x += 1.5f;
                y += 37f;

                if (decrypted > rand.Next(2))
                {
                    screenObjects.SpecialLogObjects.Add(captainScreen.CreateLabel(StargatePuzzle.SubLetters[11].ToString(), new Vector3(x, y - 7f), 12, Color.white, screenObjects.LogInfoBox.transform, UIWidget.Pivot.Center));
                    screenObjects.SpecialLogObjects[screenObjects.SpecialLogObjects.Count - 1].depth += 10000;
                }

                x -= 22f;
                y -= 1f;

                if (decrypted > rand.Next(2))
                {
                    screenObjects.SpecialLogObjects.Add(captainScreen.CreateLabel(StargatePuzzle.SubLetters[16].ToString(), new Vector3(x + 1.5f, y + 2.5f), 12, Color.white, screenObjects.LogInfoBox.transform, UIWidget.Pivot.Center));
                    screenObjects.SpecialLogObjects[screenObjects.SpecialLogObjects.Count - 1].depth += 10000;
                }

                x -= 19f;
                y += 1f;

                if (decrypted > rand.Next(2))
                {
                    screenObjects.SpecialLogObjects.Add(captainScreen.CreateLabel(StargatePuzzle.SubLetters[18].ToString(), new Vector3(x, y - 7f), 12, Color.white, screenObjects.LogInfoBox.transform, UIWidget.Pivot.Center));
                    screenObjects.SpecialLogObjects[screenObjects.SpecialLogObjects.Count - 1].depth += 10000;
                }

                x -= 1.5f;
                y -= 37f;

                if (decrypted > rand.Next(2))
                {
                    screenObjects.SpecialLogObjects.Add(captainScreen.CreateLabel(StargatePuzzle.SubLetters[14].ToString(), new Vector3(x + 1.5f, y + 2.5f), 12, Color.white, screenObjects.LogInfoBox.transform, UIWidget.Pivot.Center));
                    screenObjects.SpecialLogObjects[screenObjects.SpecialLogObjects.Count - 1].depth += 10000;
                }

                x = 0f;
                y = -22f;

                x += 22f;
                y += 1f;

                if (decrypted > rand.Next(2))
                {
                    screenObjects.SpecialLogObjects.Add(captainScreen.CreateLabel(StargatePuzzle.SubLetters[17].ToString(), new Vector3(x, y - 7f), 12, Color.white, screenObjects.LogInfoBox.transform, UIWidget.Pivot.Center));
                    screenObjects.SpecialLogObjects[screenObjects.SpecialLogObjects.Count - 1].depth += 10000;
                }

                x -= 1.5f;
                y -= 37f;

                if (decrypted > rand.Next(2))
                {
                    screenObjects.SpecialLogObjects.Add(captainScreen.CreateLabel(StargatePuzzle.SubLetters[19].ToString(), new Vector3(x + 1.5f, y + 2.5f), 12, Color.white, screenObjects.LogInfoBox.transform, UIWidget.Pivot.Center));
                    screenObjects.SpecialLogObjects[screenObjects.SpecialLogObjects.Count - 1].depth += 10000;
                }

                x -= 19f;
                y += 1f;

                if (decrypted > rand.Next(2))
                {
                    screenObjects.SpecialLogObjects.Add(captainScreen.CreateLabel(StargatePuzzle.SubLetters[15].ToString(), new Vector3(x, y - 7f), 12, Color.white, screenObjects.LogInfoBox.transform, UIWidget.Pivot.Center));
                    screenObjects.SpecialLogObjects[screenObjects.SpecialLogObjects.Count - 1].depth += 10000;
                }

                x -= 1.5f;
                y -= 37f;

                if (decrypted > rand.Next(2))
                {
                    screenObjects.SpecialLogObjects.Add(captainScreen.CreateLabel(StargatePuzzle.SubLetters[13].ToString(), new Vector3(x + 1.5f, y + 2.5f), 12, Color.white, screenObjects.LogInfoBox.transform, UIWidget.Pivot.Center));
                    screenObjects.SpecialLogObjects[screenObjects.SpecialLogObjects.Count - 1].depth += 10000;
                }
            }
            else if (specialLogIndex == 2)
            {
                screenObjects.LogInfoBoxText.fontSize = 45;
                screenObjects.LogInfoBoxText.width = 1200;
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("TIMESTAMP: 2191-03-24T14:37:09Z");
                stringBuilder.AppendLine("SYS-ID: A-993-FTL-X899");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("> INIT WARP SEQUENCE");
                stringBuilder.AppendLine();
                StargatePuzzle.GeneratePuzzleVector(out string solution, (int)PLServer.Instance.GalaxySeed);
                Vector3 vector = StargatePuzzle.VectorFromCode(solution) * -1;
                stringBuilder.AppendLine("> WARP HEADING: ?????");
                stringBuilder.AppendLine("> RIFT STABILIZATION ARRAY >> STARGATE 137");
                stringBuilder.AppendLine(string.Format("> <{0:F4}, {1:F4}, {2:F4}>", vector.x, vector.y, vector.z));
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("> WARP SEQUENCE UNSTABLE:                  [OVERIDE]");
                stringBuilder.AppendLine("> WARP VECTOR UNCERTAINTY 687%:   [OVERIDE]");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("> ERROR: FTL-Δ7 ** COLLISION DETECTED");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("> WARP STABILITY: 0.007% (CRITICAL)");
                stringBuilder.AppendLine("> COILS: OSCILLATING @ 492 Hz (MAX SAFE 198 Hz)");
                stringBuilder.AppendLine("> POWER FLUCTUATION: 137% NOMINAL");
                stringBuilder.AppendLine("> LIFE SUPPORT: CRITICAL");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("!!! INIT EMERGENCY WARP DROPOFF !!!");
                stringBuilder.Append("--- LOG INTERRUPTED – SEQUENCE ABORTED DUE TO CORE FAILURE ---");
                char[] fullText = stringBuilder.ToString().ToCharArray();
                string heading = StargatePuzzle.Solve(vector);
                int decrypted = (ReflectedRift.GetRiftData(1) ? 1 : 0) + (ReflectedRift.GetRiftData(2) ? 1 : 0) + (ReflectedRift.GetRiftData(3) ? 1 : 0) + (ReflectedRift.GetRiftData(4) ? 1 : 0) + (ReflectedRift.GetRiftData(5) ? 1 : 0);
                PLRand rand = new PLRand((int)PLServer.Instance.GalaxySeed);
                int num = 0;
                for (int i = 0; i < fullText.Length; i++)
                {
                    if (fullText[i] != ' ' && fullText[i] != '\n')
                    {
                        if (fullText[i] == '?')
                        {
                            ++num;
                            if (ReflectedRift.GetRiftData(num))
                            {
                                fullText[i] = heading[num - 1];
                                continue;
                            }
                        }
                        if (!(decrypted > rand.Next(5)))
                            fullText[i] = "!@#$%^&*"[rand.Next(8)];
                        else
                            rand.Next(8);
                    }
                }
                screenObjects.LogInfoBoxText.text = new string(fullText);
            }
            else if (specialLogIndex == 3)
            {
                PLRand rand = new PLRand((int)PLServer.Instance.GalaxySeed);
                List<UIWidget> list = new List<UIWidget>();

                for (int i = 0; i < 5; i++)
                {
                    x = -140f + (70f * i);
                    y = -140f + (rand.NextFloat() * 240f);

                    screenObjects.SpecialLogObjects.Add(captainScreen.CreateSprite(PLGlobal.Instance.SquareThemeAtlas, "button", new Vector3(x, y), new Vector2(4f, 4f), Color.white, screenObjects.LogInfoBox.transform, UIWidget.Pivot.Center));
                    screenObjects.SpecialLogObjects[screenObjects.SpecialLogObjects.Count - 1].transform.localRotation = Quaternion.Euler(0, 0, 45);
                    screenObjects.SpecialLogObjects[screenObjects.SpecialLogObjects.Count - 1].depth += 10000;

                    list.Add(screenObjects.SpecialLogObjects[screenObjects.SpecialLogObjects.Count - 1]);
                }

                for (int i = 0; i < 4; i++)
                {
                    Vector3 lengthVector = list[i + 1].transform.localPosition - list[i].transform.localPosition;
                    float length = lengthVector.magnitude;
                    float angle = Mathf.Rad2Deg * Mathf.Atan((lengthVector.y / lengthVector.x));
                    Vector3 pos = list[i].transform.localPosition + new Vector3(lengthVector.x / 2f, lengthVector.y / 2f);

                    screenObjects.SpecialLogObjects.Add(captainScreen.CreateTexture(
                        PLGlobal.Instance.WhitePixel,
                        pos,
                        new Vector2(length, 5f),
                        new Color(1f, 1f, 1f, 0.2f),
                        screenObjects.LogInfoBox.transform,
                        UIWidget.Pivot.Center));

                    screenObjects.SpecialLogObjects[screenObjects.SpecialLogObjects.Count - 1].depth += 10000;
                    screenObjects.SpecialLogObjects[screenObjects.SpecialLogObjects.Count - 1].transform.localRotation = Quaternion.Euler(0, 0, angle);
                }

                string[] labels = this.TempData.Text.Split(',');

                x = list[0].transform.localPosition.x;
                y = list[0].transform.localPosition.y - 30f;

                screenObjects.SpecialLogObjects.Add(captainScreen.CreateLabel(
                    labels[0],
                    new Vector3(x, y),
                    12,
                    Color.white,
                    screenObjects.LogInfoBox.transform,
                    UIWidget.Pivot.Center));

                screenObjects.SpecialLogObjects[screenObjects.SpecialLogObjects.Count - 1].depth += 10000;

                x = list[4].transform.localPosition.x;
                y = list[4].transform.localPosition.y - 30f;

                screenObjects.SpecialLogObjects.Add(captainScreen.CreateLabel(
                    labels[1],
                    new Vector3(x, y),
                    12,
                    Color.white,
                    screenObjects.LogInfoBox.transform,
                    UIWidget.Pivot.Center));

                screenObjects.SpecialLogObjects[screenObjects.SpecialLogObjects.Count - 1].depth += 10000;
            }
        }

        public void HideKeyPad(PLCaptainScreen captainScreen, bool hide = true)
        {
            if (!m_screenobjects.ContainsKey(captainScreen))
                return;
            foreach (UITexture texture in m_screenobjects[captainScreen].LogInfoKeypadButtons)
                texture.gameObject.SetActive(!hide);
            foreach (UILabel label in m_screenobjects[captainScreen].LogInfoKeypadLabels)
                label.gameObject.SetActive(!hide);
            m_screenobjects[captainScreen].LogInfoBoxWrite.gameObject.SetActive(!hide);
        }

        internal IEnumerator ToggleLogButtons(PLCaptainScreen captainScreen, bool hide = true)
        {
            yield return null;
            if (!m_screenobjects.ContainsKey(captainScreen))
                yield break;
            if (hide)
            {
                captainScreen.AllButtons.Remove(m_screenobjects[captainScreen].StatusButton);
                captainScreen.AllButtons.Remove(m_screenobjects[captainScreen].NewLogButton);
                captainScreen.AllButtons.Remove(m_screenobjects[captainScreen].NextButton);
                captainScreen.AllButtons.Remove(m_screenobjects[captainScreen].BackButton);
                foreach (UISprite sprite in m_screenobjects[captainScreen].LogButtons)
                    captainScreen.AllButtons.Remove(sprite);
            }
            else
            {
                if (!captainScreen.AllButtons.Contains(m_screenobjects[captainScreen].StatusButton))
                    captainScreen.AllButtons.Add(m_screenobjects[captainScreen].StatusButton);
                if (!captainScreen.AllButtons.Contains(m_screenobjects[captainScreen].NewLogButton))
                    captainScreen.AllButtons.Add(m_screenobjects[captainScreen].NewLogButton);
                if (!captainScreen.AllButtons.Contains(m_screenobjects[captainScreen].NextButton))
                    captainScreen.AllButtons.Add(m_screenobjects[captainScreen].NextButton);
                if (!captainScreen.AllButtons.Contains(m_screenobjects[captainScreen].BackButton))
                    captainScreen.AllButtons.Add(m_screenobjects[captainScreen].BackButton);
                foreach (UISprite sprite in m_screenobjects[captainScreen].LogButtons)
                    if (!captainScreen.AllButtons.Contains(sprite))
                        captainScreen.AllButtons.Add(sprite);
            }
        }

        public List<CrewLogData> GetLogs()
        {
            return m_logs;
        }

        public void AddLog(CrewLogData logData)
        {
            m_logs.Add(logData);
            if (logData.optionalSectorID != -1)
                AddPin(FormatPlaytime(logData.timeStamp), logData.optionalSectorID, logData.optionalColor, 0, m_logs.Count - 1);
        }

        public void RemoveLog(int index)
        {
            if (index < m_logs.Count)
            {
                if (m_logs[index].optionalSectorID != -1)
                    this.RemoveLogPin(index);
                foreach (int key in m_mappins.Keys)
                {
                    foreach (MapPin pin in m_mappins[key])
                    {
                        if (pin.LogIndex > index)
                            pin.LogIndex--;
                    }
                }
                m_logs.RemoveAt(index);
            }
        }

        private bool SpecialLogCanBeDeleted(int specialLogIndex)
        {
            switch (specialLogIndex)
            {
                case 0:
                case 1:
                    return false;
                case 2:
                    if (PLServer.Instance != null && PLServer.Instance.HasCompletedMissionWithID(8000014))
                        return true;
                    return false;
                case 3:
                    if (PLServer.Instance != null && PLServer.Instance.HasCompletedMissionWithID(8000018))
                        return true;
                    return false;
                default:
                    return true;
            }
        }

        public MapPin GetPinOfName(string name, out int sectorId)
        {
            foreach (int key in m_mappins.Keys)
            {
                foreach (MapPin pin in m_mappins[key])
                {
                    if (pin.Name == name)
                    {
                        sectorId = key;
                        return pin;
                    }
                }
            }
            sectorId = -1;
            return new MapPin();
        }

        public void AddPin(string name, int sectorID, Color color, int priority = 0, int logIndex = -1)
        {
            PLSectorInfo sectorWithId = PLServer.GetSectorWithID(sectorID);
            if (sectorWithId == null)
                return;
            MapPin pin = new MapPin()
            {
                Name = name,
                LogIndex = logIndex,
                Priority = priority,
                Color = color
            };
            if (m_mappins.ContainsKey(sectorID))
            {
                bool flag = false;
                for (int i = 0; i < m_mappins[sectorID].Count; i++)
                {
                    if (m_mappins[sectorID][i].Priority < priority)
                    {
                        flag = true;
                        m_mappins[sectorID].Insert(i, pin);
                        break;
                    }
                }
                if (!flag)
                    m_mappins[sectorID].Add(pin);
            }
            else
            {
                m_mappins.Add(sectorID, new List<MapPin>());
                m_mappins[sectorID].Add(pin);
            }
            UpdatePinForSector(sectorID);
        }

        public void MovePin(string name, int oldSectorID, int newSectorID)
        {
            if (!m_mappins.ContainsKey(oldSectorID))
                return;
            PLSectorInfo sectorWithId = PLServer.GetSectorWithID(newSectorID);
            if (sectorWithId == null)
                return;
            foreach (MapPin pin in m_mappins[oldSectorID])
            {
                if (pin.Name == name)
                {
                    string name1 = pin.Name;
                    Color color = pin.Color;
                    int priority = pin.Priority;
                    int logIndex = pin.LogIndex;
                    RemovePinOfName(pin.Name);
                    AddPin(name1, newSectorID, color, priority, logIndex);
                    break;
                }
            }
        }

        private void UpdatePinForSector(int inSectorID)
        {
            PLSectorInfo sectorWithId = PLServer.GetSectorWithID(inSectorID);
            if (sectorWithId == null)
                return;
            if (m_mappins.ContainsKey(inSectorID))
            {
                foreach (MapPin mapPin in m_mappins[inSectorID])
                {
                    GameObject.Destroy(mapPin.PinObject);
                }

                MapPin mapPin1 = m_mappins[inSectorID][0];
                Image pin = UnityEngine.Object.Instantiate(PLStarmap.Instance.HunterLocImage, PLStarmap.Instance.HunterLocImage.transform.parent);
                pin.GetComponent<Image>().color = mapPin1.Color;
                Image[] image = pin.GetComponentsInChildren<Image>();
                image[1].color = mapPin1.Color * 0.5f;
                image[2].color = mapPin1.Color;
                pin.GetComponentInChildren<Text>().text = mapPin1.Name;
                pin.GetComponentInChildren<Text>().color = mapPin1.Color;
                pin.transform.localPosition = sectorWithId.Position * 2000f + new Vector3(0.0f, -15f, 0.0f);
                pin.transform.localPosition = new Vector3(pin.transform.localPosition.x, pin.transform.localPosition.y, 0.0f);
                pin.gameObject.name = mapPin1.Name + "_" + mapPin1.Priority.ToString();
                pin.gameObject.SetActive(true);
                image[1].gameObject.SetActive(true);
                m_mappins[inSectorID][0].PinObject = pin.gameObject;
            }
        }

        public void UpdateAllPins()
        {
            foreach (int sectorId in m_mappins.Keys)
                UpdatePinForSector(sectorId);
        }

        private void RemoveLogPin(int logIndex)
        {
            if (!(logIndex < this.m_logs.Count))
                return;
            CrewLogData data = m_logs[logIndex];
            if (!m_mappins.ContainsKey(data.optionalSectorID))
                return;
            foreach (MapPin pin in m_mappins[data.optionalSectorID])
            {
                if (pin.LogIndex == logIndex)
                {
                    RemoveSectorPin(data.optionalSectorID, m_mappins[data.optionalSectorID].IndexOf(pin));
                    break;
                }
            }
        }

        public void RemoveSectorPin(int sectorID, int index)
        {
            if (!m_mappins.ContainsKey(sectorID))
                return;
            MapPin pin = m_mappins[sectorID][index];
            GameObject.Destroy(pin.PinObject);
            m_mappins[sectorID].RemoveAt(index);
            if (m_mappins[sectorID].Count < 1)
                m_mappins.Remove(sectorID);
            else
                UpdatePinForSector(sectorID);
        }

        public void RemovePinOfName(string name)
        {
            int ID;
            MapPin pin = GetPinOfName(name, out ID);
            if (ID != -1)
                RemoveSectorPin(ID, m_mappins[ID].IndexOf(pin));
        }
    }
}
