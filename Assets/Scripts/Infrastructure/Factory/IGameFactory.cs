using System;
using System.Collections.Generic;
using Infrastructure.Services;
using Infrastructure.Services.PersistentProgress;
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
    }
}