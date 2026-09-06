using HarmonyLib;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLIntro), "Update")]
    internal class CustomLoad
    {
        internal static bool CustomLoadStarted = false;
        internal static bool CustomLoadComplete = false;
        private static bool Prefix(PLIntro __instance, ref float ___LifeTime)
        {
            if (!CustomLoadComplete)
            {
                if (!CustomLoadStarted)
                {
                    CustomLoadStarted = true;
                    SceneManager.sceneLoaded += LoadFunctions.OnStartupSceneLoaded;
                    PLGlobal.Instance.StartCoroutine(ExGalStartupLoad());
                }
                ___LifeTime += Time.deltaTime;
                return false;
            }
            return true;
        }

        private static IEnumerator ExGalStartupLoad()
        {
            Debug.Log("[ExGal] Init StartupLoad...");
            float timeStarted = Time.time;
            bool isDone = false;
            AsyncOperation[] op = new AsyncOperation[2];
            op[0] = SceneManager.LoadSceneAsync(119, LoadSceneMode.Additive);   //SYLVASSI LAB
            op[1] = SceneManager.LoadSceneAsync(105, LoadSceneMode.Additive);   //FORSAKEN FLAGSHIP
            while (!isDone)
            {
                isDone = op.All(v => v.isDone);
                yield return 0;
            }
            Scene scene = SceneManager.GetSceneByBuildIndex(119);
            GameObject[] objects = scene.GetRootGameObjects();
            Items.JetpackCanisterPrefab = Object.Instantiate(objects[1].transform.GetChild(121).gameObject);
            Object.Destroy(Items.JetpackCanisterPrefab.GetComponent<Animator>());
            Object.Destroy(Items.JetpackCanisterPrefab.GetComponent<BoxCollider>());
            Items.JetpackCanisterPrefab.layer = 21;
            Items.JetpackCanisterPrefab.transform.position = Vector3.zero;
            Object.DontDestroyOnLoad(Items.JetpackCanisterPrefab);

            Items.WarpKeyPrefab = Object.Instantiate(objects[1].transform.GetChild(116).gameObject);
            Items.WarpKeyPrefab.layer = 21;
            Items.WarpKeyPrefab.transform.position = Vector3.zero;
            Object.DontDestroyOnLoad(Items.WarpKeyPrefab);

            NPCData.HandbookPrefab = Object.Instantiate(objects[1].transform.GetChild(127).gameObject);
            Object.Destroy(NPCData.HandbookPrefab.GetComponent<BoxCollider>());
            NPCData.HandbookPrefab.layer = 21;
            NPCData.HandbookPrefab.transform.position = Vector3.zero;
            NPCData.HandbookPrefab.transform.rotation = Quaternion.Euler(0, 0, 180);
            Object.DontDestroyOnLoad(NPCData.HandbookPrefab);

            Scene scene1 = SceneManager.GetSceneByBuildIndex(105);
            GameObject[] objects1 = scene1.GetRootGameObjects();
            ModdedCountdown.SetVisualRoot(GameObject.Instantiate(objects1[8]));
            GameObject.DestroyImmediate(objects1[7]);

            AsyncOperation[] opUnload = new AsyncOperation[2];
            opUnload[0] = SceneManager.UnloadSceneAsync(scene);
            opUnload[1] = SceneManager.UnloadSceneAsync(scene1);

            isDone = false;
            while (!isDone)
            {
                isDone = opUnload.All(v => v.isDone);
                yield return 0;
            }

            AsyncOperation[] op1 = new AsyncOperation[2];
            op1[0] = SceneManager.LoadSceneAsync(95, LoadSceneMode.Additive); //BURROW
            op1[1] = SceneManager.LoadSceneAsync(132, LoadSceneMode.Additive); //PT HUB

            isDone = false;
            while (!isDone)
            {
                isDone = op1.All(v => v.isDone);
                yield return 0;
            }

            CosmeticManager cosmeticManager = CosmeticManager.Instance;

            Scene scene2 = SceneManager.GetSceneByBuildIndex(95);
            GameObject[] objects2 = scene2.GetRootGameObjects();
            cosmeticManager.GlassMaterial = objects2[1].transform.GetChild(71).GetChild(0).GetChild(54).gameObject.GetComponent<MeshRenderer>().materials[0];

            yield return 0;

            Scene scene3 = SceneManager.GetSceneByBuildIndex(132);
            GameObject[] objects3 = scene3.GetRootGameObjects();
            cosmeticManager.DarkSteelMaterial = objects3[1].transform.GetChild(1).GetChild(0).GetChild(135).GetChild(3).GetChild(2).gameObject.GetComponent<MeshRenderer>().materials[1];
            cosmeticManager.ExosuitAntenna = objects3[1].transform.GetChild(1).GetChild(0).GetChild(135).GetChild(3).GetChild(2).GetChild(0).gameObject.GetComponent<MeshFilter>().mesh;
            cosmeticManager.ExosuitAntennaLocalPos = objects3[1].transform.GetChild(1).GetChild(0).GetChild(135).GetChild(3).GetChild(2).GetChild(0).localPosition;
            cosmeticManager.ExosuitAntennaLocalRot = objects3[1].transform.GetChild(1).GetChild(0).GetChild(135).GetChild(3).GetChild(2).GetChild(0).localRotation;
            cosmeticManager.ExosuitAntennaScale = objects3[1].transform.GetChild(1).GetChild(0).GetChild(135).GetChild(3).GetChild(2).GetChild(0).localScale;

            AsyncOperation[] opUnload1 = new AsyncOperation[2];
            opUnload1[0] = SceneManager.UnloadSceneAsync(scene2);
            opUnload1[1] = SceneManager.UnloadSceneAsync(scene3);

            isDone = false;
            while (!isDone)
            {
                isDone = opUnload1.All(v => v.isDone);
                yield return 0;
            }

            SceneManager.sceneLoaded -= LoadFunctions.OnStartupSceneLoaded;
            Debug.Log(string.Format("[ExGal] StartupLoad Complete! Time Elapsed: {0:F3}s", Time.time - timeStarted));
            CustomLoadComplete = true;
        }
    }
}
