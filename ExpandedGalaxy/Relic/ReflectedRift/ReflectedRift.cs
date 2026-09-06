using PulsarModLoader;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ExpandedGalaxy
{
    internal class ReflectedRift
    {
        internal static bool inRift
        {
            get
            {
                return (riftData & 1U) > 0;
            }
        }

        internal static bool GetRiftData(int index)
        {
            return (riftData & (byte)1U << index) > 0;
        }

        internal static void SetRiftData(int index, bool toggleState)
        {
            if (toggleState)
                riftData |= (byte)(1U << index);
            else
                riftData &= (byte)~(1U << index);
        }
        internal static byte riftData = 0;
    }
}
