using System.Collections;
using System.Linq;
using DefaultNamespace;
using UnityEngine;

namespace Hero
{
    public class HeroAnimator : MonoBehaviour
    {
        public bool IsAttacking => _isAttacking;

        private static readonly int IdleHash = Animator.StringToHash("Idle");
        private static readonly int DeathHash = Animator.StringToHash("Death");
        private static readonly int HurtHash = Animator.StringToHash("Hurt");
        private static readonly int AttackHash = Animator.StringToHash("Attack");
        private static readonly int RunHash = Animator.StringToHash("Running");

        private static readonly string UpperBodyLayerName = "UpperBody";
        private static readonly string LowerBodyLayerName = "LowerBody";

        [SerializeField] private Animator _animator;
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private AnimationClip _attackClip;

        private Coroutine _attackRoutine = null;

        private int UpperBodyLayer;
        private int LowerBodyLayer;
        private float _attackAnimationDuration;
        private bool _isRunning;
        private bool _isDead;
        private bool _isAttacking;
        
        private bool IsIdle => !_isRunning && !_isDead;

        private void Awake()
        {
            SetupLayers();
            InitializeAttackDuration();
            _animator.SetBool(IdleHash, _isRunning);
        }

        private void Update()
        {
            UpdateRunState();
        }

        public void SetDeath(bool isDeath)
        {
            _isDead = isDeath;
            _animator.SetBool(DeathHash, _isDead);
            _animator.SetBool(IdleHash, !_isDead);
        }

        public void Hurt() => _animator.SetTrigger(HurtHash);

        public void Attack()
        {
            _isAttacking = true;
            _animator.SetTrigger(AttackHash);
            StartAttackTimer();
        }

        private void StartAttackTimer()
        {
            if (_attackRoutine != null) StopCoroutine(_attackRoutine);
            StartCoroutine(AttackRoutine());
        }

        private void SetupLayers()
        {
            UpperBodyLayer = _animator.GetLayerIndex(UpperBodyLayerName);
            LowerBodyLayer = _animator.GetLayerIndex(LowerBodyLayerName);
            _animator.SetLayerWeight(UpperBodyLayer, 1);
            _animator.SetLayerWeight(LowerBodyLayer, 1);
        }

        private void InitializeAttackDuration()
        {
            var attackClip = _animator.runtimeAnimatorController.animationClips.FirstOrDefault(i =>
                i.name.Equals(_attackClip.name));
            _attackAnimationDuration = attackClip?.length ?? 1f;
        }

        private void UpdateRunState()
        {
            _isRunning = _characterController.velocity.sqrMagnitude > Constants.FloatApproximation;

            _animator.SetBool(RunHash, _isRunning);
            _animator.SetBool(IdleHash, IsIdle);
        }

        private IEnumerator AttackRoutine()
        {
            yield return new WaitForSeconds(_attackAnimationDuration);
            _isAttacking = false;
        }
    }
}