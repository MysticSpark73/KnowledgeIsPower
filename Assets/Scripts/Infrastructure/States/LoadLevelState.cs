using Data;
using DefaultNamespace.Camera;
using Enemies;
using Hero;
using Infrastructure.Factory;
using Infrastructure.Services.PersistentProgress;
using Logic;
using StaticData;
using UI.Elements;
using UI.Services.Factory;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infrastructure.States
{
    public class LoadLevelState : IPayloadState<string>
    {
        private const string PlayerSpawnPointTag = "PlayerSpawnPoint";

        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly LoadingCurtain _loadingCurtain;
        private readonly IGameFactory _gameFactory;
        private readonly IPersistentProgressService _progressService;
        private readonly IStaticDataService _staticDataService;
        private readonly IUIFactory _uiFactory;

        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, LoadingCurtain loadingCurtain,
            IGameFactory gameFactory, IPersistentProgressService progressService, IStaticDataService staticDataService, IUIFactory uiFactory)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
            _gameFactory = gameFactory;
            _progressService = progressService;
            _staticDataService = staticDataService;
            _uiFactory = uiFactory;
        }

        public void Enter(string payload)
        {
            _loadingCurtain.Show();
            _gameFactory.Dispose();
            _sceneLoader.LoadScene(payload, OnMainSceneLoaded);
        }

        public void Exit()
        {
            _loadingCurtain.Hide();
        }

        private void OnMainSceneLoaded()
        {
            InitUIRoot();
            InitGameWorld();
            InformProgressReaders();
            
            _gameStateMachine.Enter<GameLoopState>();
        }

        private void InitUIRoot() => _uiFactory.CreateUIRoot();

        private void InitGameWorld()
        {
            InitSpawners();
            SpawnUnclaimedLoot();
            GameObject playerSpawnPoint = GameObject.FindGameObjectWithTag(PlayerSpawnPointTag);
            GameObject hero = _gameFactory.CreateHero(playerSpawnPoint.transform.position);
            GameObject hud = _gameFactory.CreateHUD();
            ActorUI actorUI = hud.GetComponent<ActorUI>();
            actorUI.Construct(hero.GetComponent<HeroHealth>());
            
            SetupCameraFollow(hero);
        }

        private void InitSpawners()
        {
            string sceneKey = SceneManager.GetActiveScene().name;
            LevelStaticData levelData = _staticDataService.GetLevelData(sceneKey);

            foreach (var spawnerData in levelData.EnemySpawnerDatas)
            {
                _gameFactory.CreateEnemySpawner(spawnerData.Id, spawnerData.Position, spawnerData.MonsterTypeID);
            }
        }

        private void SpawnUnclaimedLoot()
        {
            foreach (var unclaimedLoot in _progressService.PlayerProgress.WorldData.LootData.UnclaimedLootDatas)
            {
                LootTrigger loot = _gameFactory.CreateLoot();
                loot.SetLootData(new LootData(unclaimedLoot.Value));
                loot.transform.position = unclaimedLoot.Position.ToVector3();
                loot.transform.rotation = Quaternion.Euler(unclaimedLoot.Rotation.ToVector3());
            }
            _progressService.PlayerProgress.WorldData.LootData.UnclaimedLootDatas.Clear();
        }

        private void InformProgressReaders()
        {
            foreach (var progressReader in _gameFactory.ProgressReaders)
            {
                progressReader.LoadProgress(_progressService.PlayerProgress);
            }
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