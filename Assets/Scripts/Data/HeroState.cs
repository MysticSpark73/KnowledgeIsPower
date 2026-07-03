using System;

namespace Data
{
    [Serializable]
    public class HeroState
    {
        public float CurrentHealth;
        public float MaxHealth;

        public HeroState()
        {
            MaxHealth = 50;
            ResetHealth();
        }
        
        public void ResetHealth() => CurrentHealth = MaxHealth;
    }
}