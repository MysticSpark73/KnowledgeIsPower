using System;
using System.Collections;
using System.Linq;
using Logic;
using UnityEngine;

namespace Enemies
{
    public class EnemyAnimator : MonoBehaviour, IAnimationStateReader
    {
        public EnemyAnimatorState State { get; private set; }
        public bool IsAttacking => _isAttacking;

        public event Action OnAttackEnd;

        [SerializeField] private Animator _animator;
        [SerializeField] private AnimationClip _attackClip;

        private static readonly int SpeedHash = Animator.StringToHash("Speed");
        private static readonly int IsMovingHash = Animator.StringToHash("IsMoving");
        private static readonly int Attack1Hash = Animator.StringToHash("Attack_1");
        private static readonly int Attack2Hash = Animator.StringToHash("Attack_2");
        private static readonly int HurtHash = Animator.StringToHash("Hurt");
        private static readonly int WinHash = Animator.StringToHash("Win");
        private static readonly int DieHash = Animator.StringToHash("Die");

        private static readonly int IdleStateHash = Animator.StringToHash("idle");
        private static readonly int MoveStateHash = Animator.StringToHash("Move");
        private static readonly int AttackStateHash = Animator.StringToHash("attack01");
        private static readonly int Attack2StateHash = Animator.StringToHash("attack02");
        private static readonly int HurtStateHash = Animator.StringToHash("attack02");
        private static readonly int WinStateHash = Animator.StringToHash("victory");
        private static readonly int DieStateHash = Animator.StringToHash("die");

        private float _attackDuration;
        private bool _isAttacking;
        private Coroutine _attackRoutine = null;


        public event Action<EnemyAnimatorState> StateEntered; 
        public event Action<EnemyAnimatorState> StateExited;

        private void Awake()
        {
            InitializeAttackDuration();
        }

        public void EnteredState(int stateHash)
        {
            State = GetStateByHash(stateHash);
            StateEntered?.Invoke(State);
        }

        public void ExitedState(int stateHash)
        {
            StateExited?.Invoke(GetStateByHash(stateHash));
        }

        public void PlayMove(float speed)
        {
            _animator.SetBool(IsMovingHash, true);
            _animator.SetFloat(SpeedHash, speed);
        }

        public void StopMoving() => _animator.SetBool(IsMovingHash, false);

        public void PlayAttack()
        {
            _isAttacking = true;
            _animator.SetTrigger(Attack1Hash);
            ResetAttackRoutine();
        }

        public void PlayAttack2() => _animator.SetTrigger(Attack2Hash);

        public void PlayHurt() => _animator.SetTrigger(HurtHash);

        public void PlayWin() => _animator.SetTrigger(WinHash);

        public void PlayDeath() => _animator.SetTrigger(DieHash);

        private EnemyAnimatorState GetStateByHash(int stateHash)
        {
            EnemyAnimatorState state = EnemyAnimatorState.Unknown;

            if (stateHash == IdleStateHash) return EnemyAnimatorState.Idle;
            if (stateHash == MoveStateHash) return EnemyAnimatorState.Moving;
            if (stateHash == AttackStateHash) return EnemyAnimatorState.Attack1;
            if (stateHash == Attack2StateHash) return EnemyAnimatorState.Attack2;
            if (stateHash == HurtStateHash) return EnemyAnimatorState.Hurt;
            if (stateHash == WinStateHash) return EnemyAnimatorState.Win;
            if (stateHash == DieStateHash) return EnemyAnimatorState.Death;

            return state;
        }

        private void InitializeAttackDuration()
        {
            AnimationClip attackClip = _animator.runtimeAnimatorController.animationClips.FirstOrDefault(i => i.name == _attackClip.name);
            _attackDuration = attackClip?.length ?? 1f;
        }

        private void ResetAttackRoutine()
        {
            if (_attackRoutine != null) StopCoroutine(_attackRoutine);
            StartCoroutine(AttackRoutine());
        }

        private IEnumerator AttackRoutine()
        {
            yield return new WaitForSeconds(_attackDuration);
            _isAttacking = false;
            OnAttackEnd?.Invoke();
        }
    }
}