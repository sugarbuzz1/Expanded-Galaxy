using Microsoft.SqlServer.Server;
using PulsarModLoader;
using PulsarModLoader.CustomGUI;
using PulsarModLoader.Utilities;
using UnityEngine;

namespace ExpandedGalaxy
{
    internal class ConfigMenu : ModSettingsMenu
    {
        protected EMenuPage page = EMenuPage.SETTINGS;

        protected readonly string LockedColor;
        protected readonly string UnlockedColor;
        protected bool[] tempCosmeticData;
        public ConfigMenu()
        {
            LockedColor = "A60000";
            UnlockedColor = "00FF00";
        }
        protected enum EMenuPage
        {
            SETTINGS,
            ACHIEVEMENTS,
            UNLOCKS
        }
        
        public override string Name() => "Expanded Galaxy";
        public override void Draw()
        {
            if (page == EMenuPage.SETTINGS)
            {
                GUI.skin.label.alignment = TextAnchor.UpperLeft;
                GUILayout.HorizontalSlider(0.0f, 100f, 100f);
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Achievements"))
                {
                    page = EMenuPage.ACHIEVEMENTS;
                }
                if (GUILayout.Button("Unlocks"))
                {
                    tempCosmeticData = new bool[64];
                    for (int i = 0; i < 64; i++)
                    {
                        if (CosmeticManager.Instance.DataHasIndexFlag(-1, i))
                            tempCosmeticData[i] = true;
                    }
                    page = EMenuPage.UNLOCKS;
                }
                GUILayout.EndHorizontal();
                if (PhotonNetwork.isMasterClient || PLNetworkManager.Instance.CurrentGame == null)
                {
                    Jetpack.AdvancedJetPack = GUILayout.Toggle(Jetpack.AdvancedJetPack, "Advanced Jetpack");
                }
                else
                {
                    GUILayout.Label("Advanced Jetpack: " + (Jetpack.AdvancedJetPack ? "True" : "False"));
                }
                if (PLNetworkManager.Instance.CurrentGame == null)
                {
                    Ammunition.DynamicAmmunition = GUILayout.Toggle(Ammunition.DynamicAmmunition, "Dynamic Ammunition");
                }
                else
                {
                    GUILayout.Label("Dynamic Ammunition: " + (Ammunition.DynamicAmmunition ? "True" : "False"));
                }
                if (PhotonNetwork.isMasterClient || PLNetworkManager.Instance.CurrentGame == null)
                {
                    Exosuit.BetterExosuit = GUILayout.Toggle(Exosuit.BetterExosuit, "Better Exosuit");
                }
                else
                {
                    GUILayout.Label("Better Exosuit: " + (Exosuit.BetterExosuit ? "True" : "False"));
                }
                if (PhotonNetwork.isMasterClient || PLNetworkManager.Instance.CurrentGame == null)
                {
                    Missions.slowMissionPickups = GUILayout.Toggle(Missions.slowMissionPickups, "Slower Comms Missions");
                }
                else
                {
                    GUILayout.Label("Slower Missions: " + (Missions.slowMissionPickups ? "True" : "False"));
                }
                if (GUILayout.Button("Save Preferences", GUILayout.Width(120)))
                {
                    SaveValues.SavePreferences();
                    Messaging.Notification("Success!", durationMs: 10000);
                }
            }
            else if (page == EMenuPage.ACHIEVEMENTS)
            {
                GUILayout.HorizontalSlider(0.0f, 100f, 100f);
                if (GUILayout.Button("Back to Settings"))
                    page = EMenuPage.SETTINGS;
                GUILayout.Space(50);
                GUILayout.BeginHorizontal();
                for (int i = 0; i < 3; i++)
                {
                    DrawAchievement(new Rect(8f + 250f * i, 80f, 240f, 150f), i);
                }
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                for (int i = 0; i < 2; i++)
                {
                    DrawAchievement(new Rect(8f + 250f * i, 240f, 240f, 150f), i + 3);
                }
                GUILayout.EndHorizontal();
            }
            else if (page == EMenuPage.UNLOCKS)
            {
                GUILayout.HorizontalSlider(0.0f, 100f, 100f);
                if (GUILayout.Button("Back to Settings"))
                {
                    tempCosmeticData = new bool[0];
                    page = EMenuPage.SETTINGS;
                    return;
                }
                if (Achievements.HasUnlockedAchievement(0))
                {
                    tempCosmeticData[8] = GUILayout.Toggle(tempCosmeticData[8], " Dark Exosuit Visor");
                }
                else
                    GUILayout.Label("<color=#" + LockedColor + "> <LOCKED></color> Complete \"Full Rebuild\" to unlock!");
                if (Achievements.HasUnlockedAchievement(2))
                {
                    tempCosmeticData[4] = GUILayout.Toggle(tempCosmeticData[4], " Brown Sylvassi Suit");
                    tempCosmeticData[6] = GUILayout.Toggle(tempCosmeticData[6], " Brown Exosuit");
                }
                else
                {
                    GUILayout.Label("<color=#" + LockedColor + "> <LOCKED></color> Complete \"Echoes of Industry\" to unlock!");
                    GUILayout.Label("<color=#" + LockedColor + "> <LOCKED></color> Complete \"Echoes of Industry\" to unlock!");
                }
                if (Achievements.HasUnlockedAchievement(3))
                {
                    tempCosmeticData[5] = GUILayout.Toggle(tempCosmeticData[5], " Dark Sylvassi Suit");
                    tempCosmeticData[7] = GUILayout.Toggle(tempCosmeticData[7], " Dark Exosuit");
                }
                else
                {
                    GUILayout.Label("<color=#" + LockedColor + "> <LOCKED></color> Complete \"Riftwalker\" to unlock!");
                    GUILayout.Label("<color=#" + LockedColor + "> <LOCKED></color> Complete \"Riftwalker\" to unlock!");
                }
                if (GUILayout.Button("Confirm", GUILayout.Width(120)))
                {
                    ulong tempData = 0UL;
                    for (int i = 0; i < 64; i++)
                    {
                        if (tempCosmeticData[i])
                            tempData = tempData | (1UL << i);
                    }
                    CosmeticManager.Instance.SetMyCosmeticData(tempData);
                    if (PLServer.Instance != null && (int)PLNetworkManager.Instance.LocalPlayerID != -1)
                    {
                        ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.ClientSendCosmeticData", PhotonTargets.Others, new object[3] { (int)PLNetworkManager.Instance.LocalPlayerID, (long)tempData, false });
                    }
                    Messaging.Notification("Success!", durationMs: 10000);
                }
                GUILayout.HorizontalSlider(0.0f, 100f, 100f);
                if (Achievements.HasUnlockedAchievement(4))
                    StarterInfo.RolandSC = GUILayout.Toggle(StarterInfo.RolandSC, " Start as RolandSC Variant");
                else
                    GUILayout.Label("<color=#" + LockedColor + "> <LOCKED></color> Complete \"Reclamation\" to unlock!");
            }
            else
                page = EMenuPage.SETTINGS;
        }

        private void DrawAchievement(Rect rect, int index)
        {
            string name = Achievements.GetAchievement(index, out string desc);
            GUILayout.BeginArea(rect, "", (GUIStyle)"Box");
            GUILayout.BeginHorizontal();
            GUILayout.Label(name);
            GUI.skin.label.alignment = TextAnchor.UpperRight;
            if (Achievements.HasUnlockedAchievement(index))
                GUILayout.Label("<color=#" + UnlockedColor + "><UNLOCKED></color>");
            else
                GUILayout.Label("<color=#" + LockedColor + "><LOCKED></color>");
            GUI.skin.label.alignment = TextAnchor.UpperLeft;
            GUILayout.EndHorizontal();
            GUILayout.HorizontalSlider(0.0f, 100f, 100f);
            GUILayout.BeginHorizontal();
            GUILayout.Label(desc, GUILayout.Height(100f));
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }
    }
}
