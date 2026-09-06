using CodeStage.AntiCheat.ObscuredTypes;
using PulsarModLoader.Content.Items;
using UnityEngine;

namespace ExpandedGalaxy
{
    public class PLPawnItem_WarpKey : PLPawnItem_QuestItem
    {
        public PLPawnItem_WarpKey() : base(28)
        {
            ItemModManager.Instance.GetItemIDsFromName("Warp Key", out int MainType, out int Subtype);
            this.Name = "Warp Key";
            this.PawnItemType = (EPawnItemType)MainType;
            this.SubType = Subtype;
            this.CanBeEquipped = false;
            this.Weight = 0f;
            this.Desc = "An old object that contains warp heading information for a Stargate. If only you could read it...";
            this.m_MarketPrice = (ObscuredInt)0;
            this.ImportantItem = true;
        }

        public override string GetItemName(bool skipLocalization = false) => "Warp Key";

        public override GameObject GetVisualPrefab()
        {
            if ((UnityEngine.Object)Items.WarpKeyPrefab != (UnityEngine.Object)null)
                return Items.WarpKeyPrefab;
            return base.GetVisualPrefab();
        }
    }
}
