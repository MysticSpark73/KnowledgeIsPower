using System.Threading.Tasks;
using Data;
using Enemies;
using Infrastructure.Factory;
using Infrastructure.Services.PersistentProgress;
using StaticData;
using UnityEngine;

namespace Logic.EnemySpawners
{
    public class EnemySpawnPoint : MonoBehaviour, ISavedProgress
    {
        [SerializeField] private MonsterTypeID _monsterTypeID;
        [SerializeField] private bool _enemySlain;
        
        private string ID { get; set; }
        
        private IGameFactory _factory;
        private EnemyDeath _enemyDeath;

        public void LoadProgress(PlayerProgress playerProgress)
        {
            if (playerProgress.KillData.clearedSpawnerIds.Contains(ID))
            {
                _enemySlain = true;
            }
            else
            {
                SpawnEnemy();
            }
        }

        public void InitializeFromFactory(string id, MonsterTypeID monsterTypeID, IGameFactory factory)
        {
            ID = id;
            _monsterTypeID = monsterTypeID;
            _factory = factory;
        }

        private async void SpawnEnemy()
        {
            var monster = await _factory.CreateMonster(_monsterTypeID, transform);
            _enemyDeath = monster.GetComponent<EnemyDeath>();
            _enemyDeath.OnDeath += OnEnemyDied;
        }

        public void UpdateProgress(PlayerProgress playerProgress)
        {
            if (_enemySlain)
            {
                playerProgress.KillData.AddSafe(ID);
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