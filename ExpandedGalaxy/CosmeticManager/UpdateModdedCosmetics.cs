using HarmonyLib;
using UnityEngine;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLPawn), "UpdateCustomPawn")]
    internal class UpdateModdedCosmetics
    {
        private static void Postfix(PLPawn __instance)
        {
            if (__instance.MyPlayer == null || __instance.MyPlayer.IsBot)
                return;
            int ID = __instance.MyPlayer.GetPlayerID();
            if (ID < 0)
                return;
            if (__instance.MyPlayer.GetPlayerID() == PLNetworkManager.Instance.LocalPlayerID)
                ID = -1;
            CosmeticManager.Instance.SetCosmeticForPawn(__instance, ID);
            
        }       
    }
}

/*
 private static void Postfix(PLPawn __instance)
        {
            if (__instance.MyPlayer == null)
                return;
            int ID = __instance.MyPlayer.GetPlayerID();
            if (ID < 0)
                return;
            if (__instance.MyPlayer.GetPlayerID() == PLNetworkManager.Instance.LocalPlayerID)
                ID = -1;
            if (!CosmeticManager.Instance.HasCosmeticData(__instance.MyPlayer.GetPlayerID()))
                return;
            if (!__instance.GetExosuitIsActive())
            {
                bool bodyColor = false;
                bool visorColor = false;
                if ((int)__instance.MyPlayer.RaceID == 0)
                {
                    if (!bodyColor)
                    {
                        __instance.MySkinnedMeshRenderer.materials[0].color = Color.white;
                    }
                    if (!visorColor)
                    {
                    }
                }
                else if ((int)__instance.MyPlayer.RaceID == 1)
                {
                    if (CosmeticManager.Instance.DataHasIndexFlag(ID, 4))
                    {
                        __instance.MySkinnedMeshRenderer.materials[0].color = CosmeticHandler.ExosuitBrown;
                        __instance.CustomPawn.SlyvassiHelmetRenderer.materials[0].color = CosmeticHandler.ExosuitBrown;
                        bodyColor = true;
                    }
                    if (CosmeticManager.Instance.DataHasIndexFlag(ID, 5))
                    {
                        __instance.MySkinnedMeshRenderer.materials[0].color = CosmeticHandler.SylvassiSuitDark;
                        __instance.CustomPawn.SlyvassiHelmetRenderer.materials[0].color = CosmeticHandler.SylvassiSuitDark;
                        bodyColor = true;
                    }
                    if (!bodyColor)
                    {
                        __instance.MySkinnedMeshRenderer.materials[0].color = Color.white;
                        __instance.CustomPawn.SlyvassiHelmetRenderer.materials[0].color = Color.white;
                    }
                    if (!visorColor)
                    {
                        var mats = __instance.CustomPawn.SlyvassiHelmetGlassRenderer.materials;
                        mats[0] = CosmeticHandler.GlassMaterial;
                        __instance.CustomPawn.SlyvassiHelmetGlassRenderer.materials = mats;
                        __instance.CustomPawn.SlyvassiHelmetGlassRenderer.materials[0].color = new Color(0, 0.1154f, 0.5f, 0.0649f);
                    }
                }

            }
            else
            {
                bool bodyColor = false;
                bool visorColor = false;
                bool antenna = false;
                if (CosmeticManager.Instance.DataHasIndexFlag(ID, 6))
                {
                    __instance.MySkinnedMeshRenderer.materials[0].color = CosmeticHandler.ExosuitBrown;
                    if ((int)__instance.MyPlayer.RaceID == 0)
                    {
                        __instance.CustomPawn.MyExosuit.ExteriorHelmet.materials[0].color = CosmeticHandler.ExosuitBrown;
                        __instance.CustomPawn.MyExosuit.ExteriorPack.materials[0].color = CosmeticHandler.ExosuitBrown;
                        var mats = __instance.CustomPawn.MyExosuit.ExteriorHelmet.materials;
                        mats[1] = CosmeticHandler.GlassMaterial;
                        __instance.CustomPawn.MyExosuit.ExteriorHelmet.materials = mats;
                        __instance.CustomPawn.MyExosuit.ExteriorHelmet.materials[1].color = CosmeticHandler.ExosuitVisorRed;
                    }
                    else if ((int)__instance.MyPlayer.RaceID == 1)
                    {
                        var mats = __instance.CustomPawn.SlyvassiHelmetGlassRenderer.materials;
                        mats[0] = CosmeticHandler.GlassMaterial;
                        __instance.CustomPawn.SlyvassiHelmetGlassRenderer.materials = mats;
                        __instance.CustomPawn.SlyvassiHelmetRenderer.materials[0].color = CosmeticHandler.ExosuitBrown;
                        __instance.CustomPawn.SlyvassiHelmetGlassRenderer.materials[0].color = CosmeticHandler.ExosuitVisorRed;
                    }
                    bodyColor = true;
                    visorColor = true;
                }
                if (CosmeticManager.Instance.DataHasIndexFlag(ID, 7))
                {
                    __instance.MySkinnedMeshRenderer.materials[0].color = CosmeticHandler.ExosuitDark;
                    if ((int)__instance.MyPlayer.RaceID == 0)
                    {
                        __instance.CustomPawn.MyExosuit.ExteriorHelmet.materials[0].color = CosmeticHandler.ExosuitDark;
                        __instance.CustomPawn.MyExosuit.ExteriorPack.materials[0].color = CosmeticHandler.ExosuitDark;
                        var mats = __instance.CustomPawn.MyExosuit.ExteriorHelmet.materials;
                        mats[1] = CosmeticHandler.GlassMaterial;
                        __instance.CustomPawn.MyExosuit.ExteriorHelmet.materials = mats;
                        __instance.CustomPawn.MyExosuit.ExteriorHelmet.materials[1].color = CosmeticHandler.ExosuitVisorBlue;
                    }
                    else if ((int)__instance.MyPlayer.RaceID == 1)
                    {
                        var mats = __instance.CustomPawn.SlyvassiHelmetGlassRenderer.materials;
                        mats[0] = CosmeticHandler.GlassMaterial;
                        __instance.CustomPawn.SlyvassiHelmetGlassRenderer.materials = mats;
                        __instance.CustomPawn.SlyvassiHelmetRenderer.materials[0].color = CosmeticHandler.ExosuitDark;
                        __instance.CustomPawn.SlyvassiHelmetGlassRenderer.materials[0].color = CosmeticHandler.ExosuitVisorBlue;
                    }
                    bodyColor = true;
                    visorColor = true;
                }
                if (CosmeticManager.Instance.DataHasIndexFlag(ID, 8))
                {
                    if ((int)__instance.MyPlayer.RaceID == 0)
                    {
                        var mats = __instance.CustomPawn.MyExosuit.ExteriorHelmet.materials;
                        mats[1] = CosmeticHandler.DarkSteelMaterial;
                        __instance.CustomPawn.MyExosuit.ExteriorHelmet.materials = mats;
                        __instance.CustomPawn.MyExosuit.ExteriorHelmet.materials[1].color = Color.black;
                        if (__instance.CustomPawn.MyExosuit.ExteriorHelmet.gameObject.transform.childCount < 3)
                        {
                            GameObject gameObject = new GameObject("Antenna");
                            gameObject.transform.SetParent(__instance.CustomPawn.MyExosuit.ExteriorHelmet.gameObject.transform);
                            gameObject.transform.localPosition = CosmeticHandler.ExosuitAntennaLocalPos;
                            gameObject.transform.localRotation = CosmeticHandler.ExosuitAntennaLocalRot;
                            gameObject.transform.localScale = CosmeticHandler.ExosuitAntennaScale;
                            gameObject.layer = 13;
                            MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
                            meshFilter.mesh = CosmeticHandler.ExosuitAntenna;
                            MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
                            meshRenderer.materials = new Material[1] { CosmeticHandler.DarkSteelMaterial };

                        }
                        __instance.CustomPawn.MyExosuit.ExteriorHelmet.gameObject.transform.GetChild(2).gameObject.layer = __instance.MySkinnedMeshRenderer.gameObject.layer;
                        if (__instance == PLNetworkManager.Instance.MyLocalPawn)
                        {
                            bool flag = __instance.MyPlayer.MyInventory.ActiveItem is PLPawnItem_Hands && (double)__instance.VerticalMouseLook.RotationY > 0.0;
                            flag |= (double)__instance.MyController.LerpedSpeed > 0.1;
                            __instance.CustomPawn.MyExosuit.ExteriorHelmet.gameObject.transform.GetChild(2).gameObject.SetActive(!flag);
                        }
                    }
                    else if ((int)__instance.MyPlayer.RaceID == 1)
                    {
                        var mats = __instance.CustomPawn.SlyvassiHelmetGlassRenderer.materials;
                        mats[0] = CosmeticHandler.DarkSteelMaterial;
                        __instance.CustomPawn.SlyvassiHelmetGlassRenderer.materials = mats;
                    }
                    visorColor = true;
                    antenna = true;
                }
                if (!bodyColor)
                {
                    if ((int)__instance.MyPlayer.RaceID == 0)
                    {
                        __instance.CustomPawn.MyExosuit.ExteriorHelmet.materials[0].color = Color.white;
                        __instance.CustomPawn.MyExosuit.ExteriorPack.materials[0].color = Color.white;
                        __instance.MySkinnedMeshRenderer.materials[0].color = Color.white;
                    }
                    else if ((int)__instance.MyPlayer.RaceID == 1)
                    {
                        __instance.CustomPawn.SlyvassiHelmetRenderer.materials[0].color = Color.white;
                        __instance.MySkinnedMeshRenderer.materials[0].color = Color.white;
                    }
                }
                if (!visorColor)
                {
                    if ((int)__instance.MyPlayer.RaceID == 0)
                    {
                        var mats = __instance.CustomPawn.MyExosuit.ExteriorHelmet.materials;
                        mats[1] = __instance.CustomPawn.MyExosuit.ExteriorHelmetGlassMat;
                        __instance.CustomPawn.MyExosuit.ExteriorHelmet.materials = mats;
                    }
                    else if ((int)__instance.MyPlayer.RaceID == 1)
                    {
                        var mats = __instance.CustomPawn.SlyvassiHelmetGlassRenderer.materials;
                        mats[0] = CosmeticHandler.GlassMaterial;
                        __instance.CustomPawn.SlyvassiHelmetGlassRenderer.materials = mats;
                        __instance.CustomPawn.SlyvassiHelmetGlassRenderer.materials[0].color = new Color(0, 0.1154f, 0.5f, 0.0649f);
                    }
                }
                if (!antenna && (int)__instance.MyPlayer.RaceID == 0)
                {
                    while (__instance.CustomPawn.MyExosuit.ExteriorHelmet.gameObject.transform.childCount > 2)
                    {
                        GameObject gameObject = __instance.CustomPawn.MyExosuit.ExteriorHelmet.gameObject.transform.GetChild(2).gameObject;
                        gameObject.transform.SetParent(null);
                        Object.Destroy(gameObject);
                    }
                }
            }
            CosmeticManager.Instance.RemoveDataIfEmpty(ID);
        }
 */
