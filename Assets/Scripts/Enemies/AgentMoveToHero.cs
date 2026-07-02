using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
    public class AgentMoveToHero : FollowBase
    {
        [SerializeField] private NavMeshAgent _navMeshAgent;
        
        private const float MinDistance = 1f;

        private void Update()
        {
            if (_heroTransform == null) return;
            
            if (HeroNotReached())
            {
                _navMeshAgent.destination = _heroTransform.position;
            }
        }

        private bool HeroNotReached()
        {
            return Vector3.Distance(transform.position, _heroTransform.position) >= MinDistance;
        }
    }
}