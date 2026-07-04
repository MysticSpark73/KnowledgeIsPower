using System;

namespace Data
{
    [Serializable]
    public class PlayerProgress
    {
        public WorldData WorldData;
        public HeroState HeroState;
        public HeroStats HeroStats;
        public KillData KillData;

        public PlayerProgress(string defaultSceneName)
        {
            WorldData = new WorldData(defaultSceneName);
            HeroState = new HeroState();
            HeroStats = new HeroStats();
            KillData = new KillData();
        }

    }
}