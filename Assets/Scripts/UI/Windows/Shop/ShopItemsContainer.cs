using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.AssetsManagement;
using Infrastructure.Services.IAP;
using Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace UI.Windows.Shop
{
    internal class ShopItemsContainer : MonoBehaviour
    {
        [SerializeField] private GameObject[] _iapInactiveObjects;
        [SerializeField] private Transform _itemsContainer;
        
        private IIAPService _iapService;
        private IPersistentProgressService _progressService;
        private IAssetsProvider _assetsProvider;
        
        private readonly List<ShopItemView> _shopItems = new ();

        private const string ShopItemPath = "ShopItem";

        public void Initialize(IIAPService iapService, IPersistentProgressService progressService,
            IAssetsProvider assetsProvider)
        {
            _iapService = iapService;
            _progressService = progressService;
            _assetsProvider = assetsProvider;
        }

        public void OnStart() => RefreshItems();


        public void SubscribeToEvents()
        {
            _iapService.OnInitialized += RefreshItems;
            _progressService.PlayerProgress.PurchaseData.OnPurchasedItemsChanged += RefreshItems;
        }

        public void UnSubscribeFromEvents()
        {
            _iapService.OnInitialized -= RefreshItems;
            _progressService.PlayerProgress.PurchaseData.OnPurchasedItemsChanged -= RefreshItems;
        }

        private async void RefreshItems()
        {
            await UpdateIAPInactiveObjects();
        }

        private async Task UpdateIAPInactiveObjects()
        {
            foreach (var inactiveObject in _iapInactiveObjects)
            {
                inactiveObject.SetActive(!_iapService.IsInitialized);
            }

            if (!_iapService.IsInitialized) return;

            ClearShopItems();

            await CreateShopItems();
        }

        private void ClearShopItems()
        {
            foreach (var item in _shopItems)
            {
                Destroy(item.gameObject);
            }

            _shopItems.Clear();
        }

        private async Task CreateShopItems()
        {
            List<ProductDescription> descriptions = _iapService.GetAvailableDescriptions();
            foreach (var productDescription in descriptions)
            {
                GameObject shopItemObject = await _assetsProvider.InstantiateFromAddressables(ShopItemPath, _itemsContainer);
                ShopItemView shopItemView = shopItemObject.GetComponent<ShopItemView>();
                shopItemView.Initialize(_iapService, _assetsProvider, productDescription);
                
                _shopItems.Add(shopItemView);
            }
        }
    }
}