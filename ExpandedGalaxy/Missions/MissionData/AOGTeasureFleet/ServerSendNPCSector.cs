using PulsarModLoader;

namespace ExpandedGalaxy
{
    internal class ServerSendNPCSector : ModMessage
    {
        public override void HandleRPC(object[] arguments, PhotonMessageInfo sender)
        {
            int missionID = (int)arguments[0];
            int objectiveIndex = (int)arguments[1];
            int sectorID = (int)arguments[2];

            if (missionID == 8000008)
            {
                if (sectorID == -1)
                {
                    CrewLogManager.Instance.RemovePinOfName("W.D. FLEET");
                }
                else
                {
                    int id;
                    CrewLogManager.Instance.GetPinOfName("W.D. FLEET", out id);
                    if (id != -1)
                        CrewLogManager.Instance.MovePin("W.D. FLEET", id, sectorID);
                    else
                        CrewLogManager.Instance.AddPin("W.D. FLEET", sectorID, PLGlobal.Instance.Galaxy.FactionColors[2], 4);
                }
            }
            if (PLServer.Instance == null || !PLServer.Instance.HasActiveMissionWithID(missionID))
                return;
            if (PLServer.Instance.GetActiveMissionWithID(missionID).MyMissionData == null || PLServer.Instance.GetActiveMissionWithID(missionID).MyMissionData.Objectives.Count < objectiveIndex)
                return;
            PLServer.Instance.GetActiveMissionWithID(missionID).MyMissionData.Objectives[objectiveIndex].Data["ExGal_NPC_SectorCurrent"] = sectorID.ToString();
        }
    }
}

