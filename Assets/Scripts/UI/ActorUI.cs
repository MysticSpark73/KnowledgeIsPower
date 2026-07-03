using Hero;
using UnityEngine;

namespace UI
{
    public class ActorUI : MonoBehaviour
    {
        [SerializeField] private ProgressBar _progressBar;

        private HeroHealth _health;

        public void Construct(HeroHealth health)
        {
            _health = health;
            _health.HealthChanged += UpdateHpBar;
        }

        private void OnDestroy()
        {
            _health.HealthChanged -= UpdateHpBar;
        }

        private void UpdateHpBar()
        {
            _progressBar.SetProgress(_health.CurrentHealth, _health.MaxHealth);
        } 
    }
}