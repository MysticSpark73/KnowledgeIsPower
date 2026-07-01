using System;

namespace Data
{
    [Serializable]
    public class PlayerProgress
    {
        public WorldData WorldData;

        public PlayerProgress(string defaultSceneName)
        {
            WorldData = new WorldData(defaultSceneName);
        }
    }
}