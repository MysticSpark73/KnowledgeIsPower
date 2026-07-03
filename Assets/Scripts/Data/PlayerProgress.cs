using System;

namespace Data
{
    [Serializable]
    public class PlayerProgress
    {
        public WorldData WorldData;
        public HeroState HeroState;
        public HeroStats HeroStats;

        public PlayerProgress(string defaultSceneName)
        {
            WorldData = new WorldData(defaultSceneName);
            HeroState = new HeroState();
            HeroStats = new HeroStats();
        }
    }
}