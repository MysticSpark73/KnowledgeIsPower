using Logic;
using UnityEngine;

namespace UI
{
    public class ActorUI : MonoBehaviour
    {
        [SerializeField] private ProgressBar _progressBar;

        private IHealth _health;

        private void Start()
        {
            IHealth health = GetComponent<IHealth>();
            if (health != null)
            {
                Construct(health);
            }
        }

        public void Construct(IHealth health)
        {
            _health = health;
            _health.HealthChanged += UpdateHpBar;
        }

        private void OnDestroy()
        {
            if (_health == null) return;

            _health.HealthChanged -= UpdateHpBar;
        }

        private void UpdateHpBar()
        {
            _progressBar.SetProgress(_health.CurrentHealth, _health.MaxHealth);
        } 
    }
}