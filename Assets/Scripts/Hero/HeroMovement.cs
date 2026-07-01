using Data;
using DefaultNamespace;
using Infrastructure.Services;
using Infrastructure.Services.PersistentProgress;
using Services.Input;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Hero
{
    public class HeroMovement : MonoBehaviour, ISavedProgress
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

        public void UpdateProgress(PlayerProgress playerProgress)
        {
            playerProgress.WorldData.PositionOnLevel = new PositionOnLevel(GetCurrentLevelName(), transform.position.ToVectorData());
        }

        public void LoadProgress(PlayerProgress playerProgress)
        {
            if (GetCurrentLevelName().Equals(playerProgress.WorldData.PositionOnLevel.levelName))
            {
                var savedPosition = playerProgress.WorldData.PositionOnLevel;
                if(savedPosition.position == null) return;
                
                Warp(savedPosition.position);
            }
        }

        private void Warp(Vector3Data position)
        {
            _characterController.enabled = false;
            transform.position = position.ToVector3().AddY(_characterController.height * .5f);
            _characterController.enabled = true;
        }

        private static string GetCurrentLevelName()
        {
            return SceneManager.GetActiveScene().name;
        }
    }
}