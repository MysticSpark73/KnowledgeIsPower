using Data;
using Infrastructure.Services;
using Infrastructure.Services.PersistentProgress;
using Logic;
using Services.Input;
using UnityEngine;

namespace Hero
{
    public class HeroAttack : MonoBehaviour, ISavedProgressReader
    {
        [SerializeField] private HeroAnimator _heroAnimator;
        [SerializeField] private CharacterController _characterController;

        [SerializeField] private float _attackRange = 1;

        [SerializeField] private float _weaponRadius = .5f;

        private IInputService _inputService;

        private HeroStats _attackStats;
        private Collider[] _hits = new Collider[3];

        private static int _damageableLayer;

        private void Awake()
        {
            _inputService = AllServices.Container.Single<IInputService>();
            _damageableLayer = 1 << LayerMask.NameToLayer("Damageable");
        }

        private void Update()
        {
            if (_inputService.IsAttackButtonClicked()/* && !_heroAnimator.IsAttacking*/)
            {
                Debug.Log($"IsAttacking = {_heroAnimator.IsAttacking}");
                if (!_heroAnimator.IsAttacking)
                {
                    _heroAnimator.Attack();
                }
            }
        }

        public void LoadProgress(PlayerProgress playerProgress)
        {
            _attackStats = playerProgress.HeroStats;
        }

        private void OnAttack()
        {
            for (int i = 0; i < Hit(); i++)
            {
                if (_hits[i] == null) continue;
                IHealth health = _hits[i].transform.parent.GetComponent<IHealth>();
                if (health == null) continue;
                health.TakeDamage(_attackStats.Damage);
            }
        }

        private int Hit() =>
            Physics.OverlapSphereNonAlloc(GetStartPoint() + transform.forward * _attackRange, _attackStats.AttackRadius,
                _hits, _damageableLayer);

        private Vector3 GetStartPoint() => 
            new(transform.position.x, _characterController.center.y * .5f, transform.position.z);
    }
}