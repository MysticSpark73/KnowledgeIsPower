using Data;
using Infrastructure.Services.Ads;
using Infrastructure.Services.PersistentProgress;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows.Shop
{
    public class RewardedAdItem : MonoBehaviour
    {
        [SerializeField] protected Button _button;
        [SerializeField] private GameObject[] _adActiveObjects;
        [SerializeField] private GameObject[] _adInactiveObjects;
        
        private IAdsService _adsService;
        private IPersistentProgressService _progressService;

        public void Initialize(IAdsService adsService, IPersistentProgressService progressService)
        {
            _adsService = adsService;
            _progressService = progressService;
        }

        public void OnStart()
        {
            _button.onClick.AddListener(ShowAd);
            RefreshAvailableAd();
        }

        public void SubscribeToEvents()
        {
            _adsService.RewardedVideoClicked += OnRewardedVideoClicked;
        }

        public void UnSubscribeFromEvents()
        {
            _adsService.RewardedVideoClicked -= OnRewardedVideoClicked;
        }

        private void RefreshAvailableAd()
        {
            bool isVideoReady = _adsService.IsAdsReady;
            
            foreach (var activeObject in _adActiveObjects)
            {
                activeObject.SetActive(isVideoReady);
            }

            foreach (var inactiveObject in _adInactiveObjects)
            {
                inactiveObject.SetActive(!isVideoReady);
            }
        }

        private void ShowAd()
        {
            _adsService.ShowDefaultRewardedAd(OnRWFinished);
        }

        private void OnRWFinished()
        {
            _progressService.PlayerProgress.WorldData.LootData.AddScore(new LootData(_adsService.AdReward));
        }

        private void OnRewardedVideoClicked() => RefreshAvailableAd();
    }
}