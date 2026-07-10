using System.Threading.Tasks;
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
            _gameFactory.WarmUp();
            _sceneLoader.LoadScene(payload, OnMainSceneLoaded);
        }

        public void Exit()
        {
            _loadingCurtain.Hide();
        }

        private async void OnMainSceneLoaded()
        {
            await InitUIRoot();
            await InitGameWorld();
            InformProgressReaders();
            
            _gameStateMachine.Enter<GameLoopState>();
        }

        private async Task InitUIRoot() => await _uiFactory.CreateUIRoot();

        private async Task InitGameWorld()
        {
            LevelStaticData levelData = GetLevelStaticData();

            await InitSpawners(levelData);
            await SpawnUnclaimedLoot();
            
            GameObject hero = await _gameFactory.CreateHeroAsync(levelData.InitialHeroPosition);
            GameObject hud = await _gameFactory.CreateHUDAsync();
            ActorUI actorUI = hud.GetComponent<ActorUI>();
            actorUI.Construct(hero.GetComponent<HeroHealth>());
            
            SetupCameraFollow(hero);
        }

        private LevelStaticData GetLevelStaticData()
        {
            return _staticDataService.GetLevelData(SceneManager.GetActiveScene().name);
        }

        private async Task InitSpawners(LevelStaticData levelData)
        {
            foreach (var spawnerData in levelData.EnemySpawnerDatas)
            {
                await _gameFactory.CreateEnemySpawner(spawnerData.Id, spawnerData.Position, spawnerData.MonsterTypeID);
            }
        }

        private async Task SpawnUnclaimedLoot()
        {
            foreach (var unclaimedLoot in _progressService.PlayerProgress.WorldData.LootData.UnclaimedLootDatas)
            {
                LootTrigger loot = await _gameFactory.CreateLoot();
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