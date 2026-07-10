using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Enemies;
using Infrastructure.Services;
using Infrastructure.Services.PersistentProgress;
using Logic.EnemySpawners;
using StaticData;
using UnityEngine;

namespace Infrastructure.Factory
{
    public interface IGameFactory : IService, IDisposable
    {
        Task<GameObject> CreateHeroAsync(Vector3 position);
        Task<GameObject> CreateHUDAsync();
        List<ISavedProgressReader> ProgressReaders { get; }
        List<ISavedProgress> ProgressWriters { get; }
        Task<GameObject> CreateMonster(MonsterTypeID monsterTypeID, Transform parent);
        Task<LootTrigger> CreateLoot();
        Task<EnemySpawnPoint> CreateEnemySpawner(string id, Vector3 position, MonsterTypeID monsterType);
        Task WarmUp();
    }
}