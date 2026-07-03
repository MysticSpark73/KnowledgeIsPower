using System;
using System.Collections;
using UnityEngine;

namespace Enemies
{
    public class EnemyDeath : MonoBehaviour
    {
        public event Action OnDeath;

        [SerializeField] private EnemyHealth _enemyHealth;
        [SerializeField] private EnemyAnimator _enemyAnimator;
        [SerializeField] private FollowBase _follow;

        private const float DestroyDelaySeconds = 3;

        private bool _isDead;

        private void Start()
        {
            _enemyHealth.HealthChanged += OnHealthChanged;
        }

        private void OnDestroy()
        {
            _enemyHealth.HealthChanged -= OnHealthChanged;
        }

        private void OnHealthChanged()
        {
            if (!_isDead && _enemyHealth.CurrentHealth <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            _isDead = true;
            _enemyAnimator.PlayDeath();
            _follow.enabled = false;
            OnDeath?.Invoke();

            StartCoroutine(DestroyCorpseRoutine(DestroyDelaySeconds));
        }

        private IEnumerator DestroyCorpseRoutine(float destroyDelaySeconds)
        {
            yield return new WaitForSeconds(destroyDelaySeconds);
            Destroy(gameObject);
        }
    }
}