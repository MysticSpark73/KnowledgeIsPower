using Infrastructure.AssetsManagement;
using Infrastructure.Services.Ads;
using Infrastructure.Services.IAP;
using Infrastructure.Services.PersistentProgress;
using TMPro;
using UnityEngine;

namespace UI.Windows.Shop
{
    public class ShopWindow : WindowBase
    {
        [SerializeField] private TextMeshProUGUI _currencyText;
        [SerializeField] private RewardedAdItem _adItem;
        [SerializeField] private ShopItemsContainer _itemsContainer;

        public void Initialize(IPersistentProgressService progressService, IAdsService adsService,
            IAssetsProvider assetsProvider, IIAPService iapService)
        {
            base.Initialize(progressService);
            _adItem.Initialize(adsService, _progressService);
            _itemsContainer.Initialize(iapService, progressService, assetsProvider);
        }

        protected override void OnStart()
        {
            OnScoreValueChanged();
            _adItem.OnStart();
            _itemsContainer.OnStart();
        }

        protected override void SubscribeToEvents()
        {
            Progress.WorldData.LootData.OnValueChanged += OnScoreValueChanged;
            _adItem.SubscribeToEvents();
            _itemsContainer.SubscribeToEvents();
            
        }

        protected override void UnSubscribeFromEvents()
        {
            Progress.WorldData.LootData.OnValueChanged -= OnScoreValueChanged;
            _adItem.UnSubscribeFromEvents();
            _itemsContainer.UnSubscribeFromEvents();
        }


        private void OnScoreValueChanged() =>
            _currencyText.text = Progress.WorldData.LootData.Score.ToString();
    }
}