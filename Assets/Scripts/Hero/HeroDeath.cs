using UnityEngine;

namespace Hero
{
    public class HeroDeath : MonoBehaviour
    {
        [SerializeField] private HeroHealth _heroHealth;
        [SerializeField] private HeroMovement _heroMovement;
        [SerializeField] private HeroAttack _heroAttack;
        [SerializeField] private GameObject _deathFx;
        
        private bool _isDead;

        private void Start()
        {
            _heroHealth.HealthChanged += OnHealthChanged;
        }

        private void OnDestroy()
        {
            _heroHealth.HealthChanged -= OnHealthChanged;
        }

        private void OnHealthChanged()
        {
            if (!_isDead && _heroHealth.CurrentHealth <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            _isDead = true;
            _heroMovement.enabled = false;
            _heroAttack.enabled = false;
            Instantiate(_deathFx, transform.position, Quaternion.identity);
        }
    }
}