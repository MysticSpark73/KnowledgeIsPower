using System;

namespace Logic
{
    public interface IHealth
    {
        float CurrentHealth { get; }
        float MaxHealth { get; }
        event Action HealthChanged;
        void TakeDamage(float damage);
    }
}