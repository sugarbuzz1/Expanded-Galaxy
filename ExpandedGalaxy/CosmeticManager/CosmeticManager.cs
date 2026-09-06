using System.Collections.Generic;
using UnityEngine;

namespace ExpandedGalaxy
{
    internal class CosmeticManager
    {
        private static CosmeticManager instance;
        private ulong myCosmeticData;
        private Dictionary<int, ulong> othersCosmeticData;

        internal GameObject CowboyHat;
        internal Mesh WDAdmiral_Male;
        internal Mesh CUAdmiral_Female;
        internal Material GlassMaterial;
        internal Material DarkSteelMaterial;
        internal Mesh ExosuitAntenna;
        internal Vector3 ExosuitAntennaLocalPos;
        internal Quaternion ExosuitAntennaLocalRot;
        internal Vector3 ExosuitAntennaScale;

        private readonly Color ExosuitBrown = new Color(1, 0.672f, 0.477f, 1);
        private readonly Color ExosuitVisorRed = new Color(1, 0, 0, 0.4f);
        private readonly Color ExosuitDark = new Color(0.53736f, 0.53864f, 0.72352f, 1);
        private readonly Color ExosuitVisorBlue = new Color(0, 0, 1, 0.4f);
        private readonly Color SylvassiSuitDark = new Color(0.3f, 0.3f, 0.3f, 1);

        public CosmeticManager()
        {
            myCosmeticData = SaveValues.CosmeticData.Value;
            othersCosmeticData = new Dictionary<int, ulong>();
        }

        public static CosmeticManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new CosmeticManager();
                return instance;
            }
        }

        public static void Init()
        {            
            instance = new CosmeticManager();
            Debug.Log("[ExGal] CosmeticManager Init");
        }

        public void AddCosmeticData(int inID, ulong data)
        {
            if (!othersCosmeticData.ContainsKey(inID))
                othersCosmeticData.Add(inID, data);
            else
                othersCosmeticData[inID] = data;
        }

        public void SetMyCosmeticData(ulong data)
        {
            myCosmeticData = data;
            SaveValues.CosmeticData.Value = myCosmeticData;
        }

        public void RemoveDataIfEmpty(int inID)
        {
            if (othersCosmeticData.ContainsKey(inID) && !(othersCosmeticData[inID] > 0))
                othersCosmeticData.Remove(inID);
        }

        public bool IsDataEmpty(int inID)
        {
            if (HasCosmeticData(inID))
                return (GetCosmeticData(inID) > 0);
            return true;
        }

        public bool HasCosmeticData(int inID)
        {
            if (othersCosmeticData.ContainsKey(inID))
                return true;
            else
                return inID == (int)PLNetworkManager.Instance.LocalPlayerID || inID == -1;
        }

        public ulong GetCosmeticData(int inID)
        {
            if (inID == (int)PLNetworkManager.Instance.LocalPlayerID || inID == -1)
                return myCosmeticData;
            else
                return othersCosmeticData[inID];
        }

        public void OnEnterNewGame()
        {
            othersCosmeticData.Clear();
        }

        public void ToggleIndex(int index)
        {
            if (CheckIndex(myCosmeticData, index))
                myCosmeticData &= ~((ulong)1 << index);
            else
                myCosmeticData |= (ulong)1 << index;
            SaveValues.CosmeticData.Value = myCosmeticData;
        }

        public bool DataHasIndexFlag(int inID, int index)
        {
            if (inID == (int)PLNetworkManager.Instance.LocalPlayerID || inID == -1)
                return CheckIndex(myCosmeticData, index);
            else
            {
                if (othersCosmeticData.ContainsKey(inID))
                    return CheckIndex(othersCosmeticData[inID], index);
                else
                    return false;
            }
        }

        private static bool CheckIndex(ulong data, int index)
        {
            return (data & ((ulong)1 << index)) > 0;
        }

        public ulong MyCosmeticData { get { return myCosmeticData; }}

        private void ApplyNormalHumanCosmetics(PLPawn pawn, int playerId)
        {
            pawn.MySkinnedMeshRenderer.materials[0].color = Color.white;
        }

        private void ApplyNormalSylvassiCosmetics(PLPawn pawn, int playerId)
        {
            bool bodyColorApplied = false;

            if (DataHasIndexFlag(playerId, 4))
            {
                SetSylvassiBodyColor(pawn, this.ExosuitBrown);
                bodyColorApplied = true;
            }

            if (DataHasIndexFlag(playerId, 5))
            {
                SetSylvassiBodyColor(pawn, this.SylvassiSuitDark);
                bodyColorApplied = true;
            }

            if (!bodyColorApplied)
                SetSylvassiBodyColor(pawn, Color.white);

            ResetSylvassiVisor(pawn);
        }

        private void ApplyHumanExosuitCosmetics(PLPawn pawn, int playerId)
        {
            bool bodyColorApplied = false;
            bool visorApplied = false;
            bool antennaApplied = false;

            if (DataHasIndexFlag(playerId, 6))
            {
                SetHumanExosuitColor(pawn, this.ExosuitBrown);
                SetExosuitHelmetGlass(pawn, this.GlassMaterial, this.ExosuitVisorRed);

                bodyColorApplied = true;
                visorApplied = true;
            }

            if (DataHasIndexFlag(playerId, 7))
            {
                SetHumanExosuitColor(pawn, this.ExosuitDark);
                SetExosuitHelmetGlass(pawn, this.GlassMaterial, this.ExosuitVisorBlue);

                bodyColorApplied = true;
                visorApplied = true;
            }

            if (DataHasIndexFlag(playerId, 8))
            {
                SetExosuitHelmetGlass(pawn, this.DarkSteelMaterial, Color.black);
                SetAntennaCosmetic(pawn);

                visorApplied = true;
                antennaApplied = true;
            }

            if (!bodyColorApplied)
                ResetHumanExosuitColor(pawn);

            if (!visorApplied)
                ResetHumanExosuitVisor(pawn);

            if (!antennaApplied)
                RemoveAntenna(pawn);
        }

        private void ApplySylvassiExosuitCosmetics(PLPawn pawn, int playerId)
        {
            bool bodyColorApplied = false;
            bool visorApplied = false;

            if (DataHasIndexFlag(playerId, 6))
            {
                SetSylvassiExosuitColor(pawn, this.ExosuitBrown);
                SetSylvassiVisor(pawn, this.GlassMaterial, this.ExosuitVisorRed);

                bodyColorApplied = true;
                visorApplied = true;
            }

            if (DataHasIndexFlag(playerId, 7))
            {
                SetSylvassiExosuitColor(pawn, this.ExosuitDark);
                SetSylvassiVisor(pawn, this.GlassMaterial, this.ExosuitVisorBlue);

                bodyColorApplied = true;
                visorApplied = true;
            }

            if (DataHasIndexFlag(playerId, 8))
            {
                SetSylvassiVisor(pawn, this.DarkSteelMaterial, Color.black);

                visorApplied = true;
            }

            if (!bodyColorApplied)
                ResetSylvassiExosuitColor(pawn);

            if (!visorApplied)
                ResetSylvassiVisor(pawn);
        }

        public void SetCosmeticForPawn(PLPawn pawn, int playerId)
        {
            if (!HasCosmeticData(playerId))
                return;

            switch ((int)pawn.MyPlayer.RaceID)
            {
                case 0:
                    SetHumanCosmetics(pawn, playerId);
                    break;
                case 1:
                    SetSylvassiCosmetics(pawn, playerId);
                    break;
            }

            RemoveDataIfEmpty(playerId);
        }

        private void SetHumanCosmetics(PLPawn pawn, int playerId)
        {
            if (pawn.GetExosuitIsActive())
                ApplyHumanExosuitCosmetics(pawn, playerId);
            else
                ApplyNormalHumanCosmetics(pawn, playerId);
        }

        private void SetHumanExosuitColor(PLPawn pawn, Color bodyColor)
        {
            pawn.MySkinnedMeshRenderer.materials[0].color = bodyColor;
            pawn.CustomPawn.MyExosuit.ExteriorHelmet.materials[0].color = bodyColor;
            pawn.CustomPawn.MyExosuit.ExteriorPack.materials[0].color = bodyColor;
        }

        private void SetExosuitHelmetGlass(PLPawn pawn, Material material, Color color)
        {
            MeshRenderer helmet = pawn.CustomPawn.MyExosuit.ExteriorHelmet;
            Material[] materials = helmet.materials;

            materials[1] = material;
            helmet.materials = materials;
            helmet.materials[1].color = color;
        }

        private void SetAntennaCosmetic(PLPawn pawn)
        {
            MeshRenderer helmet = pawn.CustomPawn.MyExosuit.ExteriorHelmet;

            if (helmet.transform.childCount < 3)
            {
                GameObject antenna = new GameObject("Antenna");
                antenna.transform.SetParent(helmet.transform);
                antenna.transform.localPosition = this.ExosuitAntennaLocalPos;
                antenna.transform.localRotation = this.ExosuitAntennaLocalRot;
                antenna.transform.localScale = this.ExosuitAntennaScale;
                antenna.layer = 13;

                MeshFilter meshFilter = antenna.AddComponent<MeshFilter>();
                meshFilter.mesh = this.ExosuitAntenna;

                MeshRenderer meshRenderer = antenna.AddComponent<MeshRenderer>();
                meshRenderer.materials = new[] { this.DarkSteelMaterial };
            }

            GameObject antennaObject = helmet.transform.GetChild(2).gameObject;
            antennaObject.layer = pawn.MySkinnedMeshRenderer.gameObject.layer;

            if (pawn == PLNetworkManager.Instance.MyLocalPawn)
            {
                bool hidden = pawn.MyPlayer.MyInventory.ActiveItem is PLPawnItem_Hands && pawn.VerticalMouseLook.RotationY > 0.0;
                hidden |= pawn.MyController.LerpedSpeed > 0.1;
                antennaObject.SetActive(!hidden);
            }
        }

        private void ResetHumanExosuitColor(PLPawn pawn)
        {
            pawn.MySkinnedMeshRenderer.materials[0].color = Color.white;
            pawn.CustomPawn.MyExosuit.ExteriorHelmet.materials[0].color = Color.white;
            pawn.CustomPawn.MyExosuit.ExteriorPack.materials[0].color = Color.white;
        }

        private void ResetHumanExosuitVisor(PLPawn pawn)
        {
            Material material = pawn.CustomPawn.MyExosuit.ExteriorHelmetGlassMat;
            SetExosuitHelmetGlass(pawn, material, material.color);
        }

        private void RemoveAntenna(PLPawn pawn)
        {
            if ((int)pawn.MyPlayer.RaceID != 0)
                return;

            MeshRenderer helmet = pawn.CustomPawn.MyExosuit.ExteriorHelmet;

            while (helmet.transform.childCount > 2)
            {
                GameObject antenna = helmet.transform.GetChild(2).gameObject;
                antenna.transform.SetParent(null);
                Object.Destroy(antenna);
            }
        }

        private void SetSylvassiCosmetics(PLPawn pawn, int playerId)
        {
            if (pawn.GetExosuitIsActive())
                ApplySylvassiExosuitCosmetics(pawn, playerId);
            else
                ApplyNormalSylvassiCosmetics(pawn, playerId);
        }

        private void SetSylvassiExosuitColor(PLPawn pawn, Color bodyColor)
        {
            pawn.MySkinnedMeshRenderer.materials[0].color = bodyColor;
            pawn.CustomPawn.SlyvassiHelmetRenderer.materials[0].color = bodyColor;
        }

        private void SetSylvassiVisor(PLPawn pawn, Material material, Color color)
        {
            MeshRenderer renderer = pawn.CustomPawn.SlyvassiHelmetGlassRenderer;
            Material[] materials = renderer.materials;

            materials[0] = material;
            renderer.materials = materials;
            renderer.materials[0].color = color;
        }

        private void SetSylvassiBodyColor(PLPawn pawn, Color color)
        {
            pawn.MySkinnedMeshRenderer.materials[0].color = color;
            pawn.CustomPawn.SlyvassiHelmetRenderer.materials[0].color = color;
        }

        private void ResetSylvassiExosuitColor(PLPawn pawn)
        {
            pawn.MySkinnedMeshRenderer.materials[0].color = Color.white;
            pawn.CustomPawn.SlyvassiHelmetRenderer.materials[0].color = Color.white;
        }

        private void ResetSylvassiVisor(PLPawn pawn)
        {
            SetSylvassiVisor(pawn, this.GlassMaterial, new Color(0, 0.1154f, 0.5f, 0.0649f));
        }
    }
}
