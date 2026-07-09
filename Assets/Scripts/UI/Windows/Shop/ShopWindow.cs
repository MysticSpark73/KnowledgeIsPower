using Infrastructure.Services.Ads;
using Infrastructure.Services.PersistentProgress;
using TMPro;
using UnityEngine;

namespace UI.Windows.Shop
{
    public class ShopWindow : WindowBase
    {
        [SerializeField] private TextMeshProUGUI _currencyText;
        [SerializeField] private RewardedAdItem _adItem;

        public void Initialize(IPersistentProgressService progressService, IAdsService adsService)
        {
            base.Initialize(progressService);
            _adItem.Initialize(adsService, _progressService);
        }

        protected override void OnStart()
        {
            OnScoreValueChanged();
            _adItem.OnStart();
        }

        protected override void SubscribeToEvents()
        {
            Progress.WorldData.LootData.OnValueChanged += OnScoreValueChanged;
            _adItem.SubscribeToEvents();
        }

        protected override void UnSubscribeFromEvents()
        {
            Progress.WorldData.LootData.OnValueChanged -= OnScoreValueChanged;
            _adItem.UnSubscribeFromEvents();
        }


        private void OnScoreValueChanged() =>
            _currencyText.text = Progress.WorldData.LootData.Score.ToString();
    }
}