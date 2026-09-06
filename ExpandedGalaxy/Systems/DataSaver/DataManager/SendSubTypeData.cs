using HarmonyLib;
using System.Collections.Generic;
using System.Linq;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLShipInfoBase), "OnPhotonSerializeView")]
    internal class SendSubTypeData
    {
        private static void Postfix(PLShipInfoBase __instance, ref PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.isWriting)
            {
                Dictionary<int, short> data = new Dictionary<int, short>();
                if (__instance.MyStats != null)
                {
                    foreach (PLShipComponent component in __instance.MyStats.AllComponents)
                        if (DataManager.ShouldUpdateSubTypeData(component) && component.NetID != -1)
                        {
                            data.Add(component.NetID, component.SubTypeData);
                        }
                }
                stream.SendNext(data.Count);
                if (data.Count > 0)
                {
                    stream.SendNext(data.Keys.ToArray());
                    stream.SendNext(data.Values.ToArray());
                }
            }
            else
            {
                int count = (int)stream.ReceiveNext();
                if (count > 0)
                {
                    int[] netIDs = (int[])stream.ReceiveNext();
                    short[] subTypeDatas = (short[])stream.ReceiveNext();

                    if (__instance.MyStats != null)
                    {
                        for (int i = 0; i < netIDs.Length; i++)
                        {
                            if (__instance.MyStats.GetComponentFromNetID(netIDs[i]) != null && i < subTypeDatas.Length)
                                __instance.MyStats.GetComponentFromNetID(netIDs[i]).SubTypeData = subTypeDatas[i];
                        }
                    }
                }
            }
        }
    }
}
