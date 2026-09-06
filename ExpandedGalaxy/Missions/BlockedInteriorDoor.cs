namespace ExpandedGalaxy
{
    public class BlockedInteriorDoor : PLInteriorDoor
    {
        public string BlockingScript;

        private void Update()
        {
            bool isOpen = PLMissionObjective_Custom.IDIsCompleted(BlockingScript);
            if (isOpen && !AllInteriorDoors.Contains(this))
                AllInteriorDoors.Add(this);
            else if (!isOpen && AllInteriorDoors.Contains(this))
                AllInteriorDoors.Remove(this);
        }
    }
}

