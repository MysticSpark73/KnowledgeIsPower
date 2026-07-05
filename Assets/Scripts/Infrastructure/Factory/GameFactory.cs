using System.Collections.Generic;
using Enemies;
using Infrastructure.AssetsManagement;
using Infrastructure.Services.PersistentProgress;
using StaticData;
using UI;
using UnityEngine;
using UnityEngine.AI;
using Object = UnityEngine.Object;

namespace Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private readonly IAssetsProvider _assetsProvider;
        private readonly IStaticDataService _staticDataService;

        public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
        public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();

        private GameObject HeroObject { get; set; }

        public GameFactory(IAssetsProvider assetsProvider, IStaticDataService staticDataService)
        {
            _assetsProvider = assetsProvider;
            _staticDataService = staticDataService;
        }

        public GameObject CreateHero(Vector3 position)
        {
            HeroObject = InstantiateRegistered(AssetsPath.HeroPrefabPath, position);
            return HeroObject;
        }

        public GameObject CreateHUD() => InstantiateRegistered(AssetsPath.HUDPrefabPath);

        public GameObject CreateMonster(MonsterTypeID monsterTypeID, Transform parent)
        {
            MonsterStaticData monsterData = _staticDataService.GetData(monsterTypeID);
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

            return monster;
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
                Debug.LogError($"Monster {monster.name} does not have RotateToHero component!", monster);
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

        private GameObject InstantiateRegistered(string heroPrefabPath, Vector3 position)
        {
            GameObject heroObject = _assetsProvider.InstantiatePrefabFromResources(heroPrefabPath, position);
            RegisterProgressWatchers(heroObject);
            return heroObject;
        }
        private GameObject InstantiateRegistered(string heroPrefabPath)
        {
            GameObject heroObject = _assetsProvider.InstantiatePrefabFromResources(heroPrefabPath);
            RegisterProgressWatchers(heroObject);
            return heroObject;
        }

        private void RegisterProgressWatchers(GameObject heroObject)
        {
            foreach (var progressReader in heroObject.transform.GetComponentsInChildren<ISavedProgressReader>())
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