using Infrastructure.AssetsManagement;
using Infrastructure.Services.PersistentProgress;
using StaticData;
using UI.Services.Windows;
using UI.Windows;
using UnityEngine;

namespace UI.Services.Factory
{
    public class UIFactory : IUIFactory
    {
        private const string UIRootPath = "UI/UIRoot";
        
        private readonly IAssetsProvider _assetsProvider;
        private readonly IStaticDataService _staticData;
        private readonly IPersistentProgressService _progressService;
        private Transform _uiRoot;

        public UIFactory(IAssetsProvider assetsProvider, IStaticDataService staticData, IPersistentProgressService progressService)
        {
            _assetsProvider = assetsProvider;
            _staticData = staticData;
            _progressService = progressService;
        }

        public void CreateUIRoot()
        {
            _uiRoot = _assetsProvider.InstantiatePrefabFromResources(UIRootPath).transform;
        }

        public ShopWindow CreateShop()
        {
            var windowConfig = _staticData.GetWindowsData(WindowType.Shop);
            ShopWindow shopWindow = Object.Instantiate(windowConfig.Prefab, _uiRoot) as ShopWindow;
            
            if (shopWindow != null)
            {
                shopWindow.Initialize(_progressService);
            }
            
            return shopWindow;
        }
    }
}