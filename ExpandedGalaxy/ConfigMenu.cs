using PulsarModLoader.CustomGUI;
using UnityEngine;

namespace ExpandedGalaxy
{
    internal class ConfigMenu : ModSettingsMenu
    {
        public override string Name() => "Expanded Galaxy";
        public override void Draw()
        {
            GUI.skin.label.alignment = TextAnchor.UpperLeft;
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
                Missions.slowMissionPickups = GUILayout.Toggle(Missions.slowMissionPickups, "Slower Comms Missions");
            }
            else
            {
                GUILayout.Label("Slower Missions: " + (Missions.slowMissionPickups ? "True" : "False"));
            }
        }
    }
}
