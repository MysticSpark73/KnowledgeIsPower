using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
    public class AnimateAlongAgent : MonoBehaviour
    {
        private const float MinimalVelocity = .1f;
        
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private EnemyAnimator _enemyAnimator;

        private void Update()
        {
            if (ShouldMove())
            {
                _enemyAnimator.PlayMove(_agent.velocity.magnitude);
            }
            else
            {
                _enemyAnimator.StopMoving();
            }
        }

        private bool ShouldMove() => _agent.velocity.magnitude > MinimalVelocity &&
                                     _agent.remainingDistance > _agent.radius;
    }
}