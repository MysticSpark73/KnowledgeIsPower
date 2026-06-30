using DefaultNamespace;
using Infrastructure.Services;
using Services.Input;
using UnityEngine;

namespace Hero
{
    public class HeroMovement : MonoBehaviour
    {
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private float _speed;
        
        private IInputService _inputService; 
        private Vector3 direction;
        private Camera _camera;

        private void Awake()
        {
            _inputService = AllServices.Container.Single<IInputService>();
        }

        private void Start()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            direction = Vector3.zero;
            
            if (_inputService.Axis.sqrMagnitude > Constants.FloatApproximation)
            {
                RotateHero(out direction);
            }

            direction += Physics.gravity;
            
            _characterController.Move(direction * (_speed * Time.deltaTime));
        }

        private void RotateHero(out Vector3 dir)
        {
            dir = _camera.transform.TransformDirection(_inputService.Axis);
            dir.y = 0;
            dir.Normalize();
            transform.forward = dir;
        }
    }
}