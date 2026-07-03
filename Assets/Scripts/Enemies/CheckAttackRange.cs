using UnityEngine;

namespace Enemies
{
    public class CheckAttackRange : MonoBehaviour
    {
        [SerializeField] private Attack _attack;
        [SerializeField] private TriggerObserver _triggerObserver;

        private void Start()
        {
            _attack.EnableAttack(false);
            _triggerObserver.TriggerEnter += OnTriggerEnter;
            _triggerObserver.TriggerExit += OnTriggerExit;
        }

        private void OnDestroy()
        {
            _triggerObserver.TriggerEnter -= OnTriggerEnter;
            _triggerObserver.TriggerExit -= OnTriggerExit;
        }

        private void OnTriggerEnter(Collider collider)
        {
            _attack.EnableAttack(true);
        }

        private void OnTriggerExit(Collider collider)
        {
            _attack.EnableAttack(false);
        }
    }
}