using System.Linq;
using Hero;
using Infrastructure.Factory;
using Infrastructure.Services;
using UnityEngine;

namespace Enemies
{
    public class Attack : MonoBehaviour
    {
        [SerializeField] private EnemyAnimator _enemyAnimator;
        [SerializeField] private float _attackCooldown = 3f;
        [SerializeField] private float _weaponRadius = .5f;
        [SerializeField] public float _attackRange = .5f;
        [SerializeField] private float _damage = 10;

        private IGameFactory _gameFactory;
        private Transform _heroTransform;
        private Collider[] _hits = new Collider[1];
        private bool _isAttacking;
        private bool _isAttackEnabled;
        private float _cooldownTime;
        private int _layerMask;


        private void Awake()
        {
            _gameFactory = AllServices.Container.Single<IGameFactory>();
            _gameFactory.HeroCreated += OnHeroCreated;
            
            _layerMask = 1 << LayerMask.NameToLayer("Player");
        }

        private void Update()
        {
            if (CanAttack()) StartAttack();
            else _cooldownTime -= Time.deltaTime;
        }

        private void OnDestroy()
        {
            _gameFactory.HeroCreated -= OnHeroCreated;
        }

        private bool CanAttack() => _isAttackEnabled && !_isAttacking && _cooldownTime <= 0;

        private void OnHeroCreated()
        {
            _heroTransform = _gameFactory.HeroObject.transform;
        }

        private void StartAttack()
        {
            _isAttacking = true;
            transform.LookAt(_heroTransform);
            _enemyAnimator.PlayAttack();
        }

        private void OnAttack()
        {
            if (Hit(out Collider hit))
            {
                PhysicsDebug.DrawMultidirectionalSphere(GetAttackPoint(), _weaponRadius, Color.red, 1);
                
                hit.transform.GetComponent<HeroHealth>().TakeDamage(_damage);
            }
        }

        private void OnAttackEnded()
        {
            _cooldownTime = _attackCooldown;
            _isAttacking = false;
        }

        private bool Hit(out Collider collider)
        {
            int hitCount = Physics.OverlapSphereNonAlloc(GetAttackPoint(), _weaponRadius, _hits, _layerMask);
            collider = _hits.FirstOrDefault();
            return hitCount > 0;
        }


        private Vector3 GetAttackPoint()
        {
            return new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z) +
                   transform.forward * _attackRange;
        }

        public void EnableAttack(bool isAttackEnabled)
        {
            _isAttackEnabled = isAttackEnabled;
        }
    }
}