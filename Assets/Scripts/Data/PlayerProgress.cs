using System;

namespace Data
{
    [Serializable]
    public class PlayerProgress
    {
        public WorldData WorldData;
        public HeroState HeroState;

        public PlayerProgress(string defaultSceneName)
        {
            WorldData = new WorldData(defaultSceneName);
            HeroState = new HeroState();
            HeroState.MaxHealth = 50;
            HeroState.ResetHealth();
        }
    }
}