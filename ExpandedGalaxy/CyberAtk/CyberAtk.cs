namespace ExpandedGalaxy
{
    internal class CyberAtk
    {
        public static double GetVirusCyberAtkModifier(int inSubType)
        {
            double mod = 0.0;
            switch (inSubType)
            {
                case (int)EVirusType.TROJAN_HORSE:
                    mod = 0.5;
                    break;
                case (int)EVirusType.SITTING_DUCK:
                    mod = -0.25;
                    break;
                case (int)EVirusType.GENTLEMENS_WELCOME:
                    mod = -0.5;
                    break;
                case (int)EVirusType.SHUTDOWN_DEFENSES:
                    mod = -0.25;
                    break;
            }
            return mod;
        }
    }
}
