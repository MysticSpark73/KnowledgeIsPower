using DefaultNamespace.Camera;
using Infrastructure.Factory;
using Logic;
using UnityEngine;

namespace Infrastructure.States
{
    public class LoadLevelState : IPayloadState<string>
    {
        public const string PlayerSpawnPointTag = "PlayerSpawnPoint";
        
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly LoadingCurtain _loadingCurtain;
        private readonly IGameFactory _gameFactory;

        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, LoadingCurtain loadingCurtain, IGameFactory gameFactory)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
            _gameFactory = gameFactory;
        }

        public void Enter(string payload)
        {
            _loadingCurtain.Show();
            _sceneLoader.LoadScene(payload, OnMainSceneLoaded);
        }

        public void Exit()
        {
            _loadingCurtain.Hide();
        }

        private void OnMainSceneLoaded()
        {
            GameObject playerSpawnPoint = GameObject.FindGameObjectWithTag(PlayerSpawnPointTag);
            GameObject hero = _gameFactory.CreateHero(playerSpawnPoint.transform.position);
            _gameFactory.CreateHUD();
            SetupCameraFollow(hero);
            
            _gameStateMachine.Enter<GameLoopState>();
        }

        private static void SetupCameraFollow(GameObject hero)
        {
            CameraFollow cameraFollow = Camera.main.GetComponent<CameraFollow>();
            if (cameraFollow != null)
            {
                cameraFollow.SetTarget(hero.transform);
            }
        }
    }
}