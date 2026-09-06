using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using static UIKeyBinding;

namespace ExpandedGalaxy
{
    public class ModdedCountdown : MonoBehaviour
    {
        private static ModdedCountdown instance;
        private GameObject visualRoot;
        private Text countdownText;
        private Text countdownInfo;
        private Text warningText;
        private Image countdownGlow1;
        private Image countdownGlow2;

        private bool countdownTriggered;
        private float timeLeft;
        private bool playedWarning0;
        private float nextWarningTime;
        private bool playedWarning1;
        private float nextFinalWarningTime;
        private bool playedWarning2;
        private DateTime dt;
        private bool shouldBeVisible;
        private Color baseColor;
        public PLShipInfoBase Boss;
        public PLShipComponent BossComponent;
        public float WithinRangeActive;

        public static ModdedCountdown Instance
        {
            get
            {
                if (instance == null)
                    instance = new ModdedCountdown();
                return instance;
            }
        }

        public ModdedCountdown()
        {
            this.timeLeft = 60f;
            this.nextWarningTime = 40f;
            this.nextFinalWarningTime = 20f;
            this.playedWarning0 = false;
            this.playedWarning1 = false;
            this.playedWarning2 = false;
            this.baseColor = Color.red;
            WithinRangeActive = 50000f;
        }

        public void Init(GameObject gameObject)
        {
            instance = this;
            this.visualRoot = gameObject.transform.GetChild(0).gameObject;
            this.countdownText = gameObject.transform.GetChild(0).GetChild(2).gameObject.GetComponent<Text>();
            this.countdownInfo = gameObject.transform.GetChild(0).GetChild(3).gameObject.GetComponent<Text>();
            this.warningText = gameObject.transform.GetChild(0).GetChild(4).gameObject.GetComponent<Text>();
            this.countdownGlow1 = gameObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>();
            this.countdownGlow2 = gameObject.transform.GetChild(0).GetChild(1).gameObject.GetComponent<Image>();
        }

        public void SetupCountdown(int countDownType)
        {
            this.playedWarning0 = false;
            this.playedWarning1 = false;
            this.playedWarning2 = false;
            switch (countDownType)
            {
                case 0:
                    this.timeLeft = 20f;
                    this.nextWarningTime = -1f;
                    this.nextFinalWarningTime = 10f;
                    this.baseColor = new Color(1, 0.9216f, 0.0157f, 0.5f);
                    this.countdownGlow1.color = baseColor;
                    this.countdownGlow2.color = baseColor;
                    this.warningText.text = "DANGER";
                    this.countdownInfo.text = "EMP IMMINENT";
                    this.WithinRangeActive = 5000f;
                    break;
            }
            this.countdownTriggered = true;
        }

        private bool GetIsVisible()
        {
            if (PLServer.Instance == null)
            {
                shouldBeVisible = false;
                return false;
            }
            if (PLEncounterManager.Instance != null && PLEncounterManager.Instance.PlayerShip != null)
            {
                if (PLEncounterManager.Instance.PlayerShip.InWarp)
                    shouldBeVisible = false;
                else if (this.Boss != null && this.BossComponent != null)
                {
                    shouldBeVisible = true;
                    shouldBeVisible &= PLEncounterManager.Instance.PlayerShip.StartupSwitchBoard.GetLateStatus(0);
                    shouldBeVisible &= !this.Boss.HasBeenDestroyed;
                    shouldBeVisible &= (this.Boss.GetCurrentSensorPosition() - PLEncounterManager.Instance.PlayerShip.GetCurrentSensorPosition()).magnitude < (WithinRangeActive / 5f);
                    shouldBeVisible &= this.BossComponent.SubTypeData > 0;
                }
                return shouldBeVisible && countdownTriggered;
            }
            shouldBeVisible = false;
            return false;
        }

        private void Update()
        {
            if (!countdownTriggered)
            {
                visualRoot.SetActive(false);
                return;
            }
            else
                visualRoot.SetActive(true);
            this.timeLeft -= Time.deltaTime;
            if ((double)this.timeLeft <= 0.0)
                this.countdownTriggered = false;
            if (!this.playedWarning0)
            {
                if (GetIsVisible())
                {
                    AkSoundEngine.PostEvent("play_sx_ui_countdown_01", this.gameObject);
                    this.playedWarning0 = true;
                }
            }
            if (!this.playedWarning1 && (double)this.timeLeft <= (double)nextWarningTime)
            {
                if (GetIsVisible())
                    AkSoundEngine.PostEvent("play_sx_ui_countdown_01", this.gameObject);
                this.playedWarning1 = true;
            }
            if (!this.playedWarning2 && (double)this.timeLeft <= (double)nextFinalWarningTime)
            {
                if (GetIsVisible())
                    AkSoundEngine.PostEvent("play_sx_ui_countdown_final_01", this.gameObject);
                this.playedWarning2 = true;
            }
            if (GetIsVisible())
            {
                this.countdownGlow1.color = Color.Lerp(this.countdownGlow1.color, Color.Lerp(baseColor, Color.black * 0.0f, (float)((double)Mathf.Sin(Time.time * 6f) * 0.5 + 0.5)), Mathf.Clamp01(Time.deltaTime * 2f));
                this.dt = DateTime.MinValue;
                this.dt = this.dt.AddSeconds((double)this.timeLeft);
                this.countdownText.text = this.dt.ToString("mm:ss:ff", (IFormatProvider)CultureInfo.InvariantCulture);
                PLGlobal.SafeGameObjectSetActive(this.visualRoot, true);
            }
            else
                PLGlobal.SafeGameObjectSetActive(this.visualRoot, false);           
        }

        public static void SetVisualRoot(GameObject gameObject)
        {
            UnityEngine.Object.DontDestroyOnLoad(gameObject);
            UnityEngine.Object.Destroy(gameObject.GetComponent<PhotonView>());
            UnityEngine.Object.Destroy(gameObject.GetComponent<PLBasicUICanvasSwitcher>());
            UnityEngine.Object.Destroy(gameObject.GetComponent<PLForsakenFlagshipCountdown>());
            ModdedCountdown countdown = gameObject.AddComponent<ModdedCountdown>();
            gameObject.SetActive(true);
            gameObject.transform.position = Vector3.zero;
            countdown.Init(gameObject);
        }
    }
}
