using DefaultNamespace.Camera;
using Logic;
using UnityEngine;

namespace Infrastructure
{
    public class LoadLevelState : IPayloadState<string>
    {
        private const string PlayerSpawnPointTag = "PlayerSpawnPoint";
        private const string HeroPrefabPath = "Characters/Hero";
        private const string HUDPrefabPath = "UI/HUD";

        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly LoadingCurtain _loadingCurtain;

        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, LoadingCurtain loadingCurtain)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
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
            GameObject hero = InstantiatePrefabFromResources(HeroPrefabPath, playerSpawnPoint.transform.position);
            InstantiatePrefabFromResources(HUDPrefabPath);
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

        private static GameObject InstantiatePrefabFromResources(string path)
        {
            var prefab = Resources.Load<GameObject>(path);
            return Object.Instantiate(prefab);
        }

        private static GameObject InstantiatePrefabFromResources(string path, Vector3 position)
        {
            var  prefab = Resources.Load<GameObject>(path);
            return Object.Instantiate(prefab, position, Quaternion.identity);
        }
    }
}