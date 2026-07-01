using System;
using System.Collections.Generic;
using Infrastructure.AssetsManagement;
using Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private readonly IAssetsProvider _assetsProvider;

        public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
        public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();

        public GameFactory(IAssetsProvider assetsProvider)
        {
            _assetsProvider = assetsProvider;
        }

        public GameObject CreateHero(Vector3 position) => InstantiateRegistered(AssetsPath.HeroPrefabPath, position);

        public void CreateHUD() => InstantiateRegistered(AssetsPath.HUDPrefabPath);

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