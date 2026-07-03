using System;
using Logic;
using UnityEngine;

namespace Enemies
{
    public class EnemyHealth : MonoBehaviour, IHealth
    {
        [SerializeField] private EnemyAnimator _enemyAnimator;

        public float CurrentHealth
        {
            get => _currentHealth;
            private set => _currentHealth = value;
        }

        public float MaxHealth => _maxHealth;

        [SerializeField] private float _currentHealth;
        [SerializeField] private float _maxHealth;

        public event Action HealthChanged;

        public void TakeDamage(float damage)
        {
            CurrentHealth = Mathf.Min(Mathf.Max(CurrentHealth - damage, 0), MaxHealth);
            
            if (CurrentHealth > 0)
            {
                _enemyAnimator.PlayHurt();
            }
            else
            {
                _enemyAnimator.PlayDeath();
            }
            
            HealthChanged?.Invoke();
        }
    }
}