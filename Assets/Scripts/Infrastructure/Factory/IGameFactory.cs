using System;
using System.Collections.Generic;
using Infrastructure.Services;
using Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace Infrastructure.Factory
{
    public interface IGameFactory : IService, IDisposable
    {
        GameObject CreateHero(Vector3 position);
        void CreateHUD();
        event Action HeroCreated;
        List<ISavedProgressReader> ProgressReaders { get; }
        List<ISavedProgress> ProgressWriters { get; }
        GameObject HeroObject { get; }
    }
}