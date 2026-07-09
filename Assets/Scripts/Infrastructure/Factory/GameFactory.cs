using System.Collections.Generic;
using Enemies;
using Infrastructure.AssetsManagement;
using Infrastructure.Services.PersistentProgress;
using Logic.EnemySpawners;
using Services;
using StaticData;
using UI;
using UI.Elements;
using UI.Services.Windows;
using UnityEngine;
using UnityEngine.AI;
using Object = UnityEngine.Object;

namespace Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private readonly IAssetsProvider _assetsProvider;
        private readonly IStaticDataService _staticDataService;
        private readonly IPersistentProgressService _progressService;
        private readonly IRandomService _randomService;
        private readonly IWindowsService _windowsService;

        public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
        public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();

        private GameObject HeroObject { get; set; }

        public GameFactory(IAssetsProvider assetsProvider, IStaticDataService staticDataService,
            IPersistentProgressService progressService, IRandomService randomService, IWindowsService windowsService)
        {
            _assetsProvider = assetsProvider;
            _staticDataService = staticDataService;
            _randomService = randomService;
            _windowsService = windowsService;
            _progressService = progressService;
        }

        public GameObject CreateHero(Vector3 position)
        {
            HeroObject = InstantiateRegistered(AssetsPath.HeroPrefabPath, position);
            return HeroObject;
        }

        public GameObject CreateHUD()
        {
            GameObject hud = InstantiateRegistered(AssetsPath.HUDPrefabPath);
            InitializeLootCounter(hud);
            InitializeOpenWindowButtons(hud);
            return hud;
        }

        public GameObject CreateMonster(MonsterTypeID monsterTypeID, Transform parent)
        {
            MonsterStaticData monsterData = _staticDataService.GetMonsterData(monsterTypeID);
            if (monsterData == null)
            {
                Debug.LogError($"MonsterData with ID {monsterTypeID} not found!");
                return null;
            }
            
            GameObject monster = Object.Instantiate(monsterData.Prefab, parent.position, Quaternion.identity, parent);
            
            InitializeMonsterHealth(monster, monsterData);
            InitializeMonsterMovement(monster);
            InitializeMonsterNavMesh(monster, monsterData);
            InitializeMonsterAttack(monster, monsterData);
            InitializeMonsterLoot(monster, monsterData);

            return monster;
        }

        public LootTrigger CreateLoot()
        {
            GameObject lootObject = InstantiateRegistered(AssetsPath.Loot);
            if (lootObject.TryGetComponent<LootTrigger>(out LootTrigger lootTrigger))
            {
                lootTrigger.Initialize(_progressService.PlayerProgress.WorldData);
                return lootTrigger;
            }
            return null;
        }

        public EnemySpawnPoint CreateEnemySpawner(string id, Vector3 position, MonsterTypeID monsterType)
        {
            GameObject spawnerObject = InstantiateRegistered(AssetsPath.Spawner, position);
            EnemySpawnPoint spawnPoint = spawnerObject.GetComponent<EnemySpawnPoint>();
            spawnPoint.InitializeFromFactory(id, monsterType, this);
            return spawnPoint;
        }

        private void InitializeLootCounter(GameObject hud)
        {
            LootCounter lootCounter = hud.GetComponentInChildren<LootCounter>();
            if (lootCounter == null)
            {
                Debug.LogError($"LootCounter {hud.name} not found!");
                return;
            }
            Register(lootCounter);
        }

        private void InitializeOpenWindowButtons(GameObject hud)
        {
            foreach (var openWindowButton in hud.GetComponentsInChildren<OpenWindowButton>())
            {
                openWindowButton.Initialize(_windowsService);
            }
        }

        private void InitializeMonsterHealth(GameObject monster, MonsterStaticData monsterData)
        {
            EnemyHealth health = monster.GetComponent<EnemyHealth>();
            if (health == null)
            {
                Debug.LogError($"Monster {monster.name} does not have EnemyHealth component!");
                return;
            }
            health.InitializeValues(monsterData.Health, monsterData.Health);

            ActorUI actorUI = monster.GetComponent<ActorUI>();
            if (actorUI == null)
            {
                Debug.LogError($"Monster {monster.name} does not have ActorUI component!", monster);
                return;
            }

            actorUI.Construct(health);
        }

        private void InitializeMonsterMovement(GameObject monster)
        {
            AgentMoveToHero agentMoveToHero = monster.GetComponent<AgentMoveToHero>();
            if (agentMoveToHero == null)
            {
                Debug.LogError($"Monster {monster.name} Does not have AgentMoveToHero component!", monster);
                return;
            }

            agentMoveToHero.Initialize(HeroObject.transform);
            
            RotateToHero rotateToHero = monster.GetComponent<RotateToHero>();
            if (rotateToHero == null)
            {
                Debug.LogWarning($"Monster {monster.name} does not have RotateToHero component!", monster);
                return;
            }
            
            rotateToHero.Initialize(HeroObject.transform);
        }

        private void InitializeMonsterNavMesh(GameObject monster, MonsterStaticData monsterData)
        {
            NavMeshAgent navMeshAgent = monster.GetComponent<NavMeshAgent>();
            if (navMeshAgent == null)
            {
                Debug.LogError($"Monster {monster.name} does not have NavMeshAgent component!", monster);
                return;
            }
            navMeshAgent.speed = monsterData.MoveSpeed;
        }

        private void InitializeMonsterAttack(GameObject monster, MonsterStaticData monsterData)
        {
            Attack attack = monster.GetComponent<Attack>();
            if (attack == null)
            {
                Debug.LogError($"Monster {monster.name} does not have Attack component!", monster);
                return;
            }

            attack.Initialize(HeroObject.transform, monsterData.AttackCooldown, monsterData.WeaponRadius,
                monsterData.AttackRange, monsterData.Damage);
        }

        private void InitializeMonsterLoot(GameObject monster, MonsterStaticData monsterData)
        {
            LootSpawner lootSpawner = monster.GetComponentInChildren<LootSpawner>();
            if (lootSpawner == null)
            {
                Debug.LogError($"Monster {monster.name} does not have LootSpawner component!");
                return;
            }
            lootSpawner.Initialize(this, _randomService);
            lootSpawner.SetLoot(monsterData.MinLoot, monsterData.MaxLoot);
        }

        private GameObject InstantiateRegistered(string path, Vector3 position)
        {
            GameObject gameObject = _assetsProvider.InstantiatePrefabFromResources(path, position);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private GameObject InstantiateRegistered(string path)
        {
            GameObject gameObject = _assetsProvider.InstantiatePrefabFromResources(path);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private void RegisterProgressWatchers(GameObject gameObject)
        {
            foreach (var progressReader in gameObject.transform.GetComponentsInChildren<ISavedProgressReader>())
            {
                Register(progressReader);
            }
        }

        private void Register(ISavedProgressReader progressReader)
        {
            ProgressReaders.Add(progressReader);
            
            if (progressReader is ISavedProgress savedProgress)
            {
                ProgressWriters.Add(savedProgress);
            }
        }

        public void Dispose()
        {
            ProgressReaders.Clear();
            ProgressWriters.Clear();
        }
    }
}