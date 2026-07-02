using System;
using Logic;
using UnityEngine;

namespace Enemies
{
    public class EnemyAnimator : MonoBehaviour, IAnimationStateReader
    {
        [SerializeField] private Animator _animator;
        
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
        
        public EnemyAnimatorState State { get; private set; }

        public event Action<EnemyAnimatorState> StateEntered; 
        public event Action<EnemyAnimatorState> StateExited; 

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

        public void PlayAttack() => _animator.SetTrigger(Attack1Hash);
        
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
    }
}