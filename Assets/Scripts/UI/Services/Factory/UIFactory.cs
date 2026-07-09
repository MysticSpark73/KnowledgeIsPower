using Infrastructure.AssetsManagement;
using Infrastructure.Services.Ads;
using Infrastructure.Services.PersistentProgress;
using StaticData;
using UI.Services.Windows;
using UI.Windows.Shop;
using UnityEngine;

namespace UI.Services.Factory
{
    public class UIFactory : IUIFactory
    {
        private const string UIRootPath = "UI/UIRoot";
        
        private readonly IAssetsProvider _assetsProvider;
        private readonly IStaticDataService _staticData;
        private readonly IPersistentProgressService _progressService;
        private readonly IAdsService _adsService;
        private Transform _uiRoot;

        public UIFactory(IAssetsProvider assetsProvider, IStaticDataService staticData,
            IPersistentProgressService progressService, IAdsService adsService)
        {
            _assetsProvider = assetsProvider;
            _staticData = staticData;
            _progressService = progressService;
            _adsService = adsService;
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
                shopWindow.Initialize(_progressService, _adsService);
            }
            
            return shopWindow;
        }
    }
}