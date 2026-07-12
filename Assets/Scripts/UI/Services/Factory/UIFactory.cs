using System.Threading.Tasks;
using Infrastructure.AssetsManagement;
using Infrastructure.Services.Ads;
using Infrastructure.Services.IAP;
using Infrastructure.Services.PersistentProgress;
using StaticData;
using UI.Services.Windows;
using UI.Windows.Shop;
using UnityEngine;

namespace UI.Services.Factory
{
    public class UIFactory : IUIFactory
    {
        private const string UIRootPath = "UIRoot";
        
        private readonly IAssetsProvider _assetsProvider;
        private readonly IStaticDataService _staticData;
        private readonly IPersistentProgressService _progressService;
        private readonly IAdsService _adsService;
        private readonly IIAPService _iapService;
        private Transform _uiRoot;

        public UIFactory(IAssetsProvider assetsProvider, IStaticDataService staticData,
            IPersistentProgressService progressService, IAdsService adsService, IIAPService iapService)
        {
            _assetsProvider = assetsProvider;
            _staticData = staticData;
            _progressService = progressService;
            _adsService = adsService;
            _iapService = iapService;
        }

        public async Task CreateUIRoot()
        {
            GameObject uiRootObject = await _assetsProvider.InstantiateFromAddressables(UIRootPath);
            _uiRoot = uiRootObject.transform;
        }

        public ShopWindow CreateShop()
        {
            var windowConfig = _staticData.GetWindowsData(WindowType.Shop);
            ShopWindow shopWindow = Object.Instantiate(windowConfig.Prefab, _uiRoot) as ShopWindow;
            
            if (shopWindow != null)
            {
                shopWindow.Initialize(_progressService, _adsService, _assetsProvider, _iapService);
            }
            
            return shopWindow;
        }
    }
}