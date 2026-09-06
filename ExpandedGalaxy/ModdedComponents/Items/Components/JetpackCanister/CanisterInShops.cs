using HarmonyLib;
using PulsarModLoader.Content.Items;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLShopkeeper_Random), "CreateInitialWares")]
    internal class CanisterInShops
    {
        private static void Postfix(PLShopkeeper_Random __instance, TraderPersistantDataEntry inPDE)
        {
            if (inPDE == null)
                return;
            PLRand rand = new PLRand(PLGlobal.Instance.Galaxy.Seed + __instance.MyPEI.GetSectorID());
            for (int i = 0; i < 3; i++)
            {
                if (rand.Next(100) > 75)
                {
                    ItemModManager.Instance.GetItemIDsFromName("Jetpack Canister", out int MainType, out int Subtype);
                    PLPawnItem item = ItemModManager.CreatePawnItem(MainType, Subtype, 0);
                    if (item != null)
                        inPDE.ServerAddWare(item);
                }
            }
        }
    }
}
