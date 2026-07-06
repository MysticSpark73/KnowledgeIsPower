using System.Linq;
using Logic;
using UnityEngine;

namespace Enemies
{
    public class Attack : MonoBehaviour
    {
        [SerializeField] private EnemyAnimator _enemyAnimator;
        
        private float _attackCooldown = 3f;
        private float _weaponRadius = .5f;
        public float _attackRange = .5f;
        private float _damage = 10;

        private Transform _heroTransform;
        private Collider[] _hits = new Collider[1];
        private bool _isAttackEnabled;
        private float _cooldownTime;
        private int _layerMask;

        public void Initialize(Transform heroTransform, float attackCooldown, float weaponRadius, float attackRange, float damage)
        {
            _heroTransform = heroTransform;
            _attackCooldown = attackCooldown;
            _weaponRadius = weaponRadius;
            _attackRange = attackRange;
            _damage = damage;
        }

        private void Awake()
        {
            _layerMask = 1 << LayerMask.NameToLayer("Player");
            _enemyAnimator.OnAttackEnd += OnAttackEnded;
        }

        private void Update()
        {
            if (CanAttack()) StartAttack();
            else _cooldownTime -= Time.deltaTime;
        }

        private void OnDestroy()
        {
            _enemyAnimator.OnAttackEnd -= OnAttackEnded;
        }

        private bool CanAttack() => _isAttackEnabled && !_enemyAnimator.IsAttacking && _cooldownTime <= 0;

        private void StartAttack()
        {
            transform.LookAt(_heroTransform);
            _enemyAnimator.PlayAttack();
        }

        private void OnAttack()
        {
            if (Hit(out Collider hit))
            {
                PhysicsDebug.DrawMultidirectionalSphere(GetAttackPoint(), _weaponRadius, Color.red, 1);
                
                hit.transform.GetComponent<IHealth>().TakeDamage(_damage);
            }
        }

        private void OnAttackEnded()
        {
            _cooldownTime = _attackCooldown;
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