using Data;
using Infrastructure.Services.PersistentProgress;
using StaticData;
using UnityEngine;

namespace Logic
{
    public class EnemySpawner : MonoBehaviour, ISavedProgress
    {
        [SerializeField] private MonsterTypeID _monsterTypeID;
        [SerializeField] private UniqueID _uniqueID;
        
        private string _id;
        [SerializeField] private bool _enemySlain;

        private void Awake()
        {
            _id = _uniqueID.ID;
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
            
        }

        public void UpdateProgress(PlayerProgress playerProgress)
        {
            if (_enemySlain)
            {
                playerProgress.KillData.AddSafe(_id);
            }
        }
    }
}