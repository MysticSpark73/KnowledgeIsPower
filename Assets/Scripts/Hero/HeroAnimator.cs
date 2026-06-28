using DefaultNamespace;
using UnityEngine;

namespace Hero
{
    public class HeroAnimator : MonoBehaviour
    {
        private static readonly int IdleHash = Animator.StringToHash("Idle");
        private static readonly int DeathHash = Animator.StringToHash("Death");
        private static readonly int HurtHash = Animator.StringToHash("Hurt");
        private static readonly int AttackHash = Animator.StringToHash("Attack");
        private static readonly int RunHash = Animator.StringToHash("Running");

        private static readonly string UpperBodyLayerName = "UpperBody";
        private static readonly string LowerBodyLayerName = "LowerBody";

        [SerializeField] private Animator _animator;
        [SerializeField] private CharacterController _characterController;

        private int UpperBodyLayer;
        private int LowerBodyLayer;
        private bool _isRunning;

        public void SetDeath(bool isDeath)
        {
            _animator.SetBool(DeathHash, isDeath);
            _animator.SetBool(IdleHash, !isDeath);
        }

        public void Hurt() => _animator.SetTrigger(HurtHash);
        public void Attack() => _animator.SetTrigger(AttackHash);

        private void Awake()
        {
            SetupLayers();
            _animator.SetBool(IdleHash, _isRunning);
        }

        private void Update()
        {
            UpdateRunState();
        }

        private void SetupLayers()
        {
            UpperBodyLayer = _animator.GetLayerIndex(UpperBodyLayerName);
            LowerBodyLayer = _animator.GetLayerIndex(LowerBodyLayerName);
            _animator.SetLayerWeight(UpperBodyLayer, 1);
            _animator.SetLayerWeight(LowerBodyLayer, 1);
        }

        private void UpdateRunState()
        {
            _isRunning = _characterController.velocity.sqrMagnitude > Constants.FloatApproximation;

            _animator.SetBool(RunHash, _isRunning);
            _animator.SetBool(IdleHash, !_isRunning);
        }
    }
}