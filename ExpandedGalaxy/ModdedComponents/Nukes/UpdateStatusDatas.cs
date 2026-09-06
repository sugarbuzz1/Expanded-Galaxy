using PulsarModLoader;
using static ExpandedGalaxy.Nukes;

namespace ExpandedGalaxy
{
    internal class UpdateStatusDatas : ModMessage
    {
        public override void HandleRPC(object[] arguments, PhotonMessageInfo sender)
        {
            AllStatusDatas.Clear();
            int num = (int)arguments[0];
            int num1 = 1;
            for (int i = 0; i < num; i++)
            {
                ShipStatusEffectData data = new ShipStatusEffectData();
                int id = (int)arguments[num1++];
                data.EffectId = (int)arguments[num1++];
                data.EffectStrength = (float)arguments[num1++];
                data.RemoveTime = float.MinValue;
                AllStatusDatas.Add(id, data);
            }
        }
    }
}
