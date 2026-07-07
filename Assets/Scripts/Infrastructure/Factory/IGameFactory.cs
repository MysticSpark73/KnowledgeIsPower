using System;
using System.Collections.Generic;
using Enemies;
using Infrastructure.Services;
using Infrastructure.Services.PersistentProgress;
using Logic;
using Logic.EnemySpawners;
using StaticData;
using UnityEngine;

namespace Infrastructure.Factory
{
    public interface IGameFactory : IService, IDisposable
    {
        GameObject CreateHero(Vector3 position);
        GameObject CreateHUD();
        List<ISavedProgressReader> ProgressReaders { get; }
        List<ISavedProgress> ProgressWriters { get; }
        GameObject CreateMonster(MonsterTypeID monsterTypeID, Transform parent);
        LootTrigger CreateLoot();
        EnemySpawnPoint CreateEnemySpawner(string id, Vector3 position, MonsterTypeID monsterType); 
    }
}