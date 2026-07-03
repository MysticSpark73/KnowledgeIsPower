using System;
using Data;
using Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace Hero
{
    public class HeroHealth : MonoBehaviour, ISavedProgress
    {
        public event Action HealthChanged;
        
        [SerializeField] private HeroAnimator _heroAnimator;
        
        private HeroState _heroState;

        public float CurrentHealth
        {
            get => _heroState.CurrentHealth;
            private set
            {
                if(Mathf.Approximately(value, CurrentHealth)) return;
                
                _heroState.CurrentHealth = value;
                HealthChanged?.Invoke();
            }
        }

        public float MaxHealth
        {
            get => _heroState.MaxHealth;
            private set => _heroState.MaxHealth = value;
        }

        public void TakeDamage(float damage)
        {
             CurrentHealth = Mathf.Min(Mathf.Max(CurrentHealth - damage, 0), MaxHealth);
             if (CurrentHealth > 0)
             {
                _heroAnimator.Hurt();
             }
             else
             {
                 _heroAnimator.SetDeath(true);
             }
        }

        public void LoadProgress(PlayerProgress playerProgress)
        {
            _heroState = playerProgress.HeroState;
            HealthChanged?.Invoke();
        }

        public void UpdateProgress(PlayerProgress playerProgress)
        {
            _heroState.CurrentHealth = CurrentHealth;
            _heroState.MaxHealth = MaxHealth;
        }
    }
}