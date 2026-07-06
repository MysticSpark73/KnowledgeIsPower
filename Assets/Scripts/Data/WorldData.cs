using System;

namespace Data
{
    [Serializable]
    public class WorldData
    {
        public PositionOnLevel PositionOnLevel;
        public LootSaveData LootData;

        public WorldData(string defaultSceneName)
        {
            PositionOnLevel = new PositionOnLevel(defaultSceneName);
        }

    }
}