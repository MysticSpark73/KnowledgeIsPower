using Infrastructure.AssetsManagement;
using UnityEngine;

namespace Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private readonly IAssetsProvider _assetsProvider;

        public GameFactory(IAssetsProvider assetsProvider)
        {
            _assetsProvider = assetsProvider;
        }

        public GameObject CreateHero(Vector3 position) => _assetsProvider.InstantiatePrefabFromResources(AssetsPath.HeroPrefabPath, position);

        public void CreateHUD() => _assetsProvider.InstantiatePrefabFromResources(AssetsPath.HUDPrefabPath);
    }
}