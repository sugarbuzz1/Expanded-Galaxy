using UnityEngine;
using UnityEngine.SceneManagement;

namespace ExpandedGalaxy
{
    internal class LoadFunctions
    {
        internal static void OnStartupSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.buildIndex == 0 || scene.buildIndex == 1 || scene.name == "PreGame" || scene.name == "Intro")
                return;
            foreach (GameObject gameObject in scene.GetRootGameObjects())
                gameObject.SetActive(false);
        }
    }
}
