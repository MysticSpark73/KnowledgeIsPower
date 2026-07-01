using System;

namespace Data
{
    [Serializable]
    public class WorldData
    {
        public PositionOnLevel PositionOnLevel;

        public WorldData(string defaultSceneName)
        {
            PositionOnLevel = new PositionOnLevel(defaultSceneName);
        }
    }
}