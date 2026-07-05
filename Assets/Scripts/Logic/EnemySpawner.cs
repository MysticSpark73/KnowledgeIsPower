using System;
using Data;
using Enemies;
using Infrastructure.Factory;
using Infrastructure.Services;
using Infrastructure.Services.PersistentProgress;
using StaticData;
using UnityEngine;

namespace Logic
{
    public class EnemySpawner : MonoBehaviour, ISavedProgress
    {
        [SerializeField] private MonsterTypeID _monsterTypeID;
        [SerializeField] private UniqueID _uniqueID;
        [SerializeField] private bool _enemySlain;
        
        private string _id;
        private IGameFactory _factory;
        private EnemyDeath _enemyDeath;

        private void Awake()
        {
            _id = _uniqueID.ID;
            _factory = AllServices.Container.Single<IGameFactory>();
        }

        public void LoadProgress(PlayerProgress playerProgress)
        {
            if (playerProgress.KillData.clearedSpawnerIds.Contains(_id))
            {
                _enemySlain = true;
            }
            else
            {
                SpawnEnemy();
            }
        }

        private void SpawnEnemy()
        {
            var monster = _factory.CreateMonster(_monsterTypeID, transform);
            _enemyDeath = monster.GetComponent<EnemyDeath>();
            _enemyDeath.OnDeath += OnEnemyDied;
        }

        public void UpdateProgress(PlayerProgress playerProgress)
        {
            if (_enemySlain)
            {
                playerProgress.KillData.AddSafe(_id);
            }
        }

        private void OnEnemyDied()
        {
            UnsubscribeFromEnemyDeath();
            _enemySlain = true;
        }

        private void OnDestroy()
        {
            UnsubscribeFromEnemyDeath();
        }

        private void UnsubscribeFromEnemyDeath()
        {
            if (_enemyDeath != null)
            {
                _enemyDeath.OnDeath -= OnEnemyDied;
            }
        }
    }
}