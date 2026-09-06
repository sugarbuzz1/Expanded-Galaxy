namespace ExpandedGalaxy
{
    internal class Ammunition
    {
        internal static bool DynamicAmmunition = true;

        internal static float AmmoRefillPercent()
        {
            if (Ammunition.DynamicAmmunition)
                return 0.05f;
            return 0.1f;
        }
    }
}
