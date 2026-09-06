using CodeStage.AntiCheat.ObscuredTypes;
using PulsarModLoader.Content.Items;
using UnityEngine;

namespace ExpandedGalaxy
{
    public class PLPawnItem_AutoRifle : PLPawnItem_PhasePistol
    {
        public PLPawnItem_AutoRifle()
        {
            ItemModManager.Instance.GetItemIDsFromName("Auto Rifle", out int MainType, out int Subtype);
            this.GunFireEvent = "play_sx_player_item_handcannon_shoot";
            this.Desc = "A modified burst rifle that is no longer limited to 3-round volleys.";
            this.m_MarketPrice = (ObscuredInt)4000;
            this.MinAutoFireDelay = 0.1f;
            this.PawnWalkSpeed = (ObscuredFloat)3.5f;
            this.PawnItemType = (EPawnItemType)MainType;
            this.SubType = Subtype;
            this.Accuracy = 1.8f;
            this.AnimName_Fire = "";
            this.m_AnimID = 4;
            this.UsesAmmo = true;
            this.AmmoMax = 60;
            this.AmmoCurrent = this.AmmoMax;
            this.invCameraFOVMultipler = 1.6f;
        }

        public override void FireShot(
            Vector3 aimAtPoint,
            Vector3 destNormal,
            int newBoltID,
            Collider hitCollider)
        {
            base.FireShot(aimAtPoint, destNormal, newBoltID, hitCollider);
            this.Heat -= 0.06f;
            this.MySetupPawn.VerticalMouseLook.RotationY_AddOverTime += 1.1f;
            this.MySetupPawn.VerticalMouseLook.RotationY += 0.35f;
        }

        public override GameObject CreateBoltGO() => Object.Instantiate<GameObject>(PLGlobal.Instance.HeavyPistolBoltPrefab, this.GetBoltSpawnPos(), Quaternion.identity);

        public override float CalcDamageDone() => (float)(14.0 + 6.0 * (double)this.Level);

        public override string GetItemName(bool skipLocalization = false) => PLLocalize.Localize("Auto Rifle", skipLocalization) + this.GetItemEnd();

        public override GameObject GetGunPrefab() => PLGlobal.Instance.BurstRiflePrefab;
    }
}
