using System.Collections;
using UnityEngine;

namespace Enemies
{
    public class Aggro : MonoBehaviour
    {
        [SerializeField] private TriggerObserver _triggerObserver;
        [SerializeField] private FollowBase _followComponent;
        [SerializeField] private float _cooldownTimeSeconds;
        
        private Coroutine _aggroRoutine;
        private bool _hasAggroTarget;

        private void Start()
        {
            _triggerObserver.TriggerEnter += OnTriggerEnter;
            _triggerObserver.TriggerExit += OnTriggerExit;
            SetFollow(false);
        }

        private void OnTriggerEnter(Collider obj)
        {
            if (!_hasAggroTarget)
            {
                StopAggroRoutine();
                SetFollow(true);
            }
        }

        private void OnTriggerExit(Collider obj)
        {
            if (_hasAggroTarget)
            {
                _aggroRoutine = StartCoroutine(ChaseRoutine());
            }
        }

        private IEnumerator ChaseRoutine()
        {
            yield return new WaitForSeconds(_cooldownTimeSeconds);
            SetFollow(false);
        }

        private void StopAggroRoutine()
        {
            if (_aggroRoutine != null)
            {
                StopCoroutine(_aggroRoutine);
                _aggroRoutine = null;
            }
        }

        private void SetFollow(bool isFollowing)
        {
            _hasAggroTarget = isFollowing;
            _followComponent.enabled = isFollowing;
        }
    }
}