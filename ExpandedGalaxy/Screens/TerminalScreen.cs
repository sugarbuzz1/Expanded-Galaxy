using System.Collections;
using System.Text;
using UnityEngine;

namespace ExpandedGalaxy
{
    internal class TerminalScreen : ModdedScreenBase
    {
        public string ScreenTitle;
        public string ScreenContent;
        private UISprite LogPanel;
        private UILabel LogPanelTitleLabel;
        private UILabel ContentLabel;
        private float LastCheckRiftCompletionTime = float.MinValue;
        private bool DecodeSequenceStarted;
        private byte DecodeSequenceState;
        private byte DecodeSequenceSubstate;

        public override void SetupUI()
        {
            base.SetupUI();
            DecodeSequenceStarted = false;
            this.LogPanel = this.CreatePanelEditable("Terminal", new Vector3(0.0f, 0.0f, 0.0f), new Vector2(512f, 512f), this.UI_White, out this.LogPanelTitleLabel);
            this.ContentLabel = this.CreateParagraph("", new Vector3(12f, -50f, 0.0f), 14, 1880, 1760, this.UI_White, this.LogPanel.transform);
        }

        public override bool UIIsSetup() => base.UIIsSetup() && this.LogPanel != null;

        public override void Update()
        {
            base.Update();
            if (DecodeSequenceStarted)
                return;
            if (Time.time - LastCheckRiftCompletionTime > 5f)
            {
                LastCheckRiftCompletionTime = Time.time;
                if (PLServer.Instance != null && PLServer.Instance.HasCompletedMissionWithID(8000014))
                    ContentLabel.text = "> Valid Stargate Detected\n> Insert Warp Key:";
                else
                    ContentLabel.text = "> ERROR: No Valid Stargate Found.\n> Error Code: D-846-SG-Z4495\"";
            }
        }

        public void StartDecodeSequence()
        {
            if (DecodeSequenceStarted)
                return;
            DecodeSequenceStarted = true;
        }

        private IEnumerator DecodeSequence()
        {
            DecodeSequenceState = 0;
            DecodeSequenceSubstate = 0;

            for (int i = 0; i < 12; i++)
            {
                ContentLabel.text = "> Analyzing" + ('.' * (DecodeSequenceSubstate + 1)).ToString();
                ++DecodeSequenceState;
                DecodeSequenceState = (byte)(DecodeSequenceState % 3);
                yield return new WaitForSeconds(0.5f);
            }

            StringBuilder builder = new StringBuilder();
            builder.AppendLine("> Analyzing...");
            builder.AppendLine("> ERROR: Warp Key signature corrupted!");
            ContentLabel.text = builder.ToString();
            yield return new WaitForSeconds(5f);
            builder.AppendLine(">");

            ContentLabel.text = builder.ToString() + "> Reconstructing heading from warp data" + ('.' * (DecodeSequenceSubstate + 1)).ToString();
            builder.AppendLine("> Reconstructing heading from warp data");
        }
    }
}
