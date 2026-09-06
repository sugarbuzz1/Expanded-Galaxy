using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLScientistVirusScreen), "Update")]
    internal class RandText
    {
        private static void Postfix(PLScientistVirusScreen __instance)
        {
            foreach (PLVirusDrawInfo drawInfo in __instance.VirusDrawInfos)
            {
                if (drawInfo.MyVirus == null)
                    continue;
                if (drawInfo.MyVirus.SubType == (int)EVirusType.RAND_SMALL || drawInfo.MyVirus.SubType == (int)EVirusType.RAND_LARGE)
                {
                    string Desc = "";
                    if (drawInfo.MyVirus.SubType == (int)EVirusType.RAND_SMALL)
                    {
                        switch (drawInfo.MyVirus.SubTypeData)
                        {
                            case 0:
                                Desc = "Backdoor/Phalanx";
                                break;
                            case 1:
                                Desc = "Sitting Duck/Warp Disable";
                                break;
                            case 2:
                                Desc = "Blindfold/Expose";
                                break;
                            case 3:
                                Desc = "Sitting Duck/Phalanx";
                                break;
                        }
                    }
                    else
                    {
                        switch (drawInfo.MyVirus.SubTypeData)
                        {
                            case 0:
                                Desc = "Syber's Shield/Phalanx";
                                break;
                            case 1:
                                Desc = "Armor Flaw/Breathless";
                                break;
                            case 2:
                                Desc = "Backdoor/Lazy Guns";
                                break;
                            case 3:
                                Desc = "Blindfold/Warp Disable";
                                break;
                        }
                    }
                    drawInfo.MyDescLabel.text = Desc;
                }
            }
        }
    }
}

