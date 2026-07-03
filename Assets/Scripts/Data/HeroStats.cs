using System;

namespace Data
{
    [Serializable]
    public class HeroStats
    {
        public float Damage;
        public float AttackRadius;

        public HeroStats()
        {
            Damage = 1;
            AttackRadius = .5f;
        }
    }
}